using System;
using System.Runtime.InteropServices;
using mshtml;
using PasswordBoss.Views.UserControls;
using SHDocVw;

namespace PasswordBoss.Helpers
{
    public static class CrossFrameWindow
    {
        private const int EAccessdenied = unchecked((int)0x80070005L);
        private static Guid _iidIWebBrowser2 = new Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E");
        private static Guid _iidIWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
        // Returns null in case of failure.
        public static IHTMLDocument2 GetDocumentFromWindow(IHTMLWindow2 htmlWindow)
        {
            if (htmlWindow == null)
                return null;

            // First try the usual way to get the document.
            try
            {
                var doc = htmlWindow.document;

                return doc;
            }
            catch (COMException comEx)
            {
                // I think COMException won't be ever fired but just to be sure ...
                if (comEx.ErrorCode != EAccessdenied)
                    return null;
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch
            {
                // Any other error.
                return null;
            }

            // At this point the error was E_ACCESSDENIED because the frame contains a document from another domain.
            // IE tries to prevent a cross frame scripting security issue.
            try
            {
                // Convert IHTMLWindow2 to IWebBrowser2 using IServiceProvider.
                var sp = (WpfWebBrowserWrapper.IServiceProvider)htmlWindow;

                // Use IServiceProvider.QueryService to get IWebBrowser2 object.
                var brws = sp.QueryService(ref _iidIWebBrowserApp, ref _iidIWebBrowser2);

                // Get the document from IWebBrowser2.
                var browser = (IWebBrowser2)(brws);

                return (IHTMLDocument2)browser.Document;
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}