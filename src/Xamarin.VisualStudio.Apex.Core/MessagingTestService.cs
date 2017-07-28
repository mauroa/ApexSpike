using Clide;
using Microsoft.Test.Apex.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Messaging.Integration;
using Xamarin.Messaging.Integration.Models;

namespace Xamarin.VisualStudio.Apex.Core
{
    [Export(typeof(MessagingTestService))]
    public class MessagingTestService : VisualStudioTestService
    {
        readonly object lockObject = new object();
        bool initialized;
        IDevEnv devenv;
        IRemoteServerSourceManager serverSourceManager;
        IRemoteServerProvider remoteServerProvider;
        IServerContextProvider serverContextProvider;

        //Get hosts that has been connected at least once
        public IEnumerable<ServerData> GetKnownHosts(RemoteServerPlatform platform) => GetHosts(platform, known: true);

        //Get hosts that has never been connected (mostly mdns discovered hosts)
        public IEnumerable<ServerData> GetUnknownHosts(RemoteServerPlatform platform) => GetHosts(platform, known: false);

        public void PreventAutoConnection(RemoteServerPlatform platform)
        {
           //TODO: Figure out the best way to prevent auto connection
           //Options:
           //1 - Manually edit registry and set IsDefault property of all Knwon Servers to false
           //2 - Add support on Xamarin.Messaging.Integration to override default auto connect behaviour
           //3 - ?
        }

        public async Task WaitForConnectedAsync(RemoteServerPlatform platform, TimeSpan dueTime)
        {
            EnsureInitialization();
            ValidatePlatform(platform);

            var serverSource = serverSourceManager.GetActiveSource(platform);
            var serverInterface = serverSource.GetServerInterface();

            if (serverInterface.IsConnected) return;

            try
            {
                await Observable
                   .FromEventPattern<IRemoteServer>(
                       h => remoteServerProvider.ServerConnected += h,
                       h => remoteServerProvider.ServerConnected -= h)
                   .FirstOrDefaultAsync(s => s.EventArgs.Platform == platform)
                   .Timeout(dueTime);
            }
            catch
            {
                //Use Test Logging
            }
        }

        public RemoteServerTestExtension GetRemoteServer(RemoteServerPlatform platform)
        {
            EnsureInitialization();
            ValidatePlatform(platform);

            var serverSource = serverSourceManager.GetActiveSource(platform);
            var serverInterface = serverSource.GetServerInterface();

            return CreateRemotableInstance<RemoteServerTestExtension>(serverInterface);
        }

        void EnsureInitialization()
        {
            if (!initialized)
            {
                lock (lockObject)
                {
                    if (!initialized)
                    {
                        //TODO: Replace this with XMA Package Guid in a future
                        devenv = DevEnv.Get(XamarinTestContext.Packages.iOS);
                        serverSourceManager = devenv.ServiceLocator.GetInstance<IRemoteServerSourceManager>();
                        remoteServerProvider = devenv.ServiceLocator.GetInstance<IRemoteServerProvider>();
                        serverContextProvider = devenv.ServiceLocator.GetInstance<IServerContextProvider>();
                        initialized = true;
                    }
                }
            }
        }

        void ValidatePlatform(RemoteServerPlatform platform)
        {
            if (!serverSourceManager.ActiveSources.Any(s => s.Platform == platform))
                throw new XamarinTestException($"No Server Source has been registered for platform {platform}");
        }

        IEnumerable<ServerData> GetHosts(RemoteServerPlatform platform, bool known = true)
        {
            EnsureInitialization();

            return serverContextProvider.Servers.Collection.Where(s => s.Platform == platform && s.IsKnown == known);
        }
    }
}
