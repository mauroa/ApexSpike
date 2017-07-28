using Microsoft.Test.Apex.VisualStudio;
using System.Linq;
using Xamarin.Messaging;
using Xamarin.Messaging.Integration;

namespace Xamarin.VisualStudio.Apex.Core
{
    public class RemoteServerTestVerifier : VisualStudioInProcessTestExtensionVerifier<IRemoteServerInteractive>
    {
        protected new RemoteServerTestExtension TestExtension => base.TestExtension as RemoteServerTestExtension;

        public bool IsConnected() => ObjectUnderTest.MessagingService.IsConnected;

        public bool HasAllAgentsRunning() => ObjectUnderTest.IsReady;

        public bool IsAgentRunning(AgentInfo agentInfo) => ObjectUnderTest.GetAgentsRunning().Any(a => a.Name == agentInfo.Name);

        public bool MatchesPlatform(RemoteServerPlatform platform) => ObjectUnderTest.Platform == platform;
    }
}
