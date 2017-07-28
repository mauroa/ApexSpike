using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Messaging.Integration;
using Xamarin.VisualStudio.Apex.Core;

namespace Xamarin.VisualStudio.Apex.Tests
{
    [TestClass]
    public class iOSConnectionTests : XamariniOSTest
    {
        [TestMethod]
        public async Task When_Creating_iOS_App_And_Xma_Auto_Connects_Then_Succeeds()
        {
            CreateProject(XamarinTestContext.Templates.iOS.Universal.SingleView);

            await Messaging.WaitForConnectedAsync(RemoteServerPlatform.Mac, TimeSpan.FromSeconds(30));

            var remoteServer = Messaging.GetRemoteServer(RemoteServerPlatform.Mac);

            Assert.IsTrue(remoteServer.Verify.IsConnected());
            Assert.IsTrue(remoteServer.Verify.HasAllAgentsRunning());
            Assert.IsTrue(remoteServer.Verify.MatchesPlatform(RemoteServerPlatform.Mac));
        }

        [TestMethod]
        public async Task When_Creating_iOS_App_And_Xma_Connects_Manually_Then_Succeeds()
        {
            Messaging.PreventAutoConnection(RemoteServerPlatform.Mac);

            CreateProject(XamarinTestContext.Templates.iOS.Universal.SingleView);

            var remoteServer = Messaging.GetRemoteServer(RemoteServerPlatform.Mac);
            var hostToConnect = Messaging.GetKnownHosts(RemoteServerPlatform.Mac).FirstOrDefault();

            Assert.IsNotNull(hostToConnect);

            await remoteServer.ConnectAsync(hostToConnect);

            Assert.IsTrue(remoteServer.Verify.IsConnected());
            Assert.IsTrue(remoteServer.Verify.HasAllAgentsRunning());
            Assert.IsTrue(remoteServer.Verify.MatchesPlatform(RemoteServerPlatform.Mac));
        }

        [TestMethod]
        public async Task When_Creating_iOS_App_And_Disconnects_Xma_Once_Auto_Connected_Then_Succeeds()
        {
            CreateProject(XamarinTestContext.Templates.iOS.Universal.SingleView);

            await Messaging.WaitForConnectedAsync(RemoteServerPlatform.Mac, TimeSpan.FromSeconds(30));

            var remoteServer = Messaging.GetRemoteServer(RemoteServerPlatform.Mac);

            await remoteServer.DisconnectAsync();

            Assert.IsFalse(remoteServer.Verify.IsConnected());
            Assert.IsFalse(remoteServer.Verify.HasAllAgentsRunning());
            Assert.IsTrue(remoteServer.Verify.MatchesPlatform(RemoteServerPlatform.Mac));
        }
    }
}
