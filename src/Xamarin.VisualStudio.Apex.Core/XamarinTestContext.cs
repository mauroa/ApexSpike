using System;

namespace Xamarin.VisualStudio.Apex.Core
{
    public class XamarinTestContext
    {
        public static readonly XamarinPackages Packages = new XamarinPackages();

        public static readonly ProjectTemplates Templates = new ProjectTemplates();
    }

    public class XamarinPackages
    {
        public readonly Guid iOS = new Guid("77875fa9-01e7-4fea-8e77-dfe942355ca1");
    }

    public class ProjectTemplates
    {
        public readonly iOSProjectTemplates iOS = new iOSProjectTemplates();
    }

    public class iOSProjectTemplates
    {
        public readonly iOSProjectTemplatesUniversal Universal = new iOSProjectTemplatesUniversal();
    }

    public class iOSProjectTemplatesUniversal
    {
        public readonly string SingleView = "Xamarin.iOS.Universal.SingleViewApp";

        public readonly string MasterDetail = "Xamarin.iOS.Universal.MasterDetailApp";
    }
}
