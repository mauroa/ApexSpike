using Microsoft.Test.Apex.VisualStudio;
using System.Threading.Tasks;
using Xamarin.Messaging;
using Xamarin.Messaging.Integration;
using Xamarin.Messaging.Integration.Models;

namespace Xamarin.VisualStudio.Apex.Core
{
    public class RemoteServerTestExtension: VisualStudioInProcessTestExtension<IRemoteServerInteractive, RemoteServerTestVerifier>
    {
        //Emulates connecting to a specific server from the UI
        public async Task ConnectAsync(ServerData serverData)
        {
            if (ObjectUnderTest.Platform != serverData.Platform)
                throw new XamarinTestException($"Incompatible platform between the current server interface ({ObjectUnderTest.Platform}) and the host to connect ({serverData.Platform})");

            //We connect silently to not show UI
            await ObjectUnderTest.TryConnectAsync(serverData, silent: true);
        }

        public Task DisconnectAsync() => ObjectUnderTest.DisconnectAsync();

        public Task StartAgentsAsync() => ObjectUnderTest.StartAgentsAsync();

        public Task StartAgentAsync(AgentInfo agentInfo) => ObjectUnderTest.StartAgentAsync(agentInfo);
    }
}
