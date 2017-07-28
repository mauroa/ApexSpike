using System;

namespace Xamarin.VisualStudio.Apex.Core
{
    public class XamarinTestException : ApplicationException
    {
        public XamarinTestException(string message) : base(message)
        {
        }

        public XamarinTestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
