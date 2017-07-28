using Microsoft.Test.Apex.VisualStudio.Solution;

namespace Xamarin.VisualStudio.Apex.Core
{
    public abstract class XamariniOSTest : XamarinTest
    {
        public MessagingTestService Messaging => VisualStudio.Get<MessagingTestService>();

        protected override void SetupProject(ProjectTestExtension project)
        {
            project.Configuration.ActivePlatformName = "iPhoneSimulator";
        }
    }
}
