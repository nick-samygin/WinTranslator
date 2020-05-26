using Microsoft.Win32;
using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace PasswordBoss.Views.UserControls
{
    public class WebBrowserEx : System.Windows.Forms.WebBrowser
    {
        public TabItem CurrentTabItem { get; set; }
        
        //private IWebBrowser2 axIWebBrowser2;

        //public WebBrowserEx()
        //{
        //}

        //protected override void AttachInterfaces(object nativeActiveXObject)
        //{
        //    base.AttachInterfaces(nativeActiveXObject);
        //    this.axIWebBrowser2 = (IWebBrowser2)nativeActiveXObject;
        //}

        //protected override void DetachInterfaces()
        //{
        //    base.DetachInterfaces();
        //    this.axIWebBrowser2 = null;
        //}

        
        public WebBrowserEx(TabItem tabItem)
        {
            SetBrowserFeatureControl();
            CurrentTabItem = tabItem;
        }

        #region WebBrowserSiteEx Class

        /// <summary>
        /// Intercepts downloads of files, to add as PDFs or suppliments
        /// </summary>
        [ComVisible(true)]
        [Guid("bdb9c34c-d0ca-448e-b497-8de62e709744")]
        [CLSCompliant(false)]
        public class DownloadManager : IDownloadManager
        {
            [DllImport("wininet.dll", SetLastError = true)]
            public static extern bool InternetGetCookieEx(
                string url,
                string cookieName,
                StringBuilder cookieData,
                ref int size,
                Int32 dwFlags,
                IntPtr lpReserved);

            private const Int32 InternetCookieHttponly = 0x2000;

            WebBrowserEx _webBrowser;
            public DownloadManager(WebBrowserEx webBrowser)
            {
                _webBrowser = webBrowser;
            }

            public CookieContainer GetUriCookieContainer(Uri uri)
            {
                CookieContainer cookies = null;
                // Determine the size of the cookie
                int datasize = 8192 * 16;
                StringBuilder cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
                {
                    if (datasize < 0)
                        return null;
                    // Allocate stringbuilder large enough to hold the cookie
                    cookieData = new StringBuilder(datasize);
                    if (!InternetGetCookieEx(
                        uri.ToString(),
                        null, cookieData,
                        ref datasize,
                        InternetCookieHttponly,
                        IntPtr.Zero))
                        return null;
                }
                if (cookieData.Length > 0)
                {
                    cookies = new CookieContainer();
                    cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
                }
                return cookies;
            }

            /// <summary>
            /// event called when the browser is about to download a file
            /// </summary>
            //public event EventHandler<FileDownloadEventArgs> FileDownloading;

            /// <summary>
            /// Return S_OK (0) so that IE will stop to download the file itself. 
            /// Else the default download user interface is used.
            /// </summary>
            public int Download(IMoniker pmk, IBindCtx pbc, uint dwBindVerb, int grfBINDF, IntPtr pBindInfo,
                                string pszHeaders, string pszRedir, uint uiCP)
            {
                // Get the display name of the pointer to an IMoniker interface that specifies the object to be downloaded.
                string name;
                pmk.GetDisplayName(pbc, null, out name);
                if (!string.IsNullOrEmpty(name))
                {
                    Uri url;
                    if (Uri.TryCreate(name, UriKind.Absolute, out url))
                    {
                        //if (FileDownloading != null)
                        //{
                        //    FileDownloading(this, new FileDownloadEventArgs(url));
                        //}
                        System.Net.WebRequest wc = System.Net.WebRequest.Create(name); //args[0]);
                         
                        ((HttpWebRequest)wc).UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/0.2.153.1 Safari/525.19";
                        wc.Timeout = 10000;
                        wc.Method = "HEAD";
                        ((HttpWebRequest)wc).CookieContainer = GetUriCookieContainer(url);
                        WebResponse res = wc.GetResponse();
                        
                        var streamReader = new System.IO.StreamReader(res.GetResponseStream());

                        var header = res.Headers["Content-Disposition"];
                        //System.Windows.MessageBox.Show(header);

                        //Console.WriteLine(streamReader.ReadToEnd());
                        //return Constants.S_OK;
                        return 1;
                    }
                }
                return 1;
            }
        }
        protected class WebBrowserSiteEx : System.Windows.Forms.WebBrowser.WebBrowserSite, IServiceProvider, IInternetSecurityManager
        {
            
            private static Guid IID_IInternetSecurityManager =
                Marshal.GenerateGuidForType(typeof(IInternetSecurityManager));

            private WebBrowserEx _webBrowser;

            public WebBrowserSiteEx(WebBrowserEx webBrowser)
                : base(webBrowser)
            {
                _webBrowser = webBrowser;
                 _manager = new DownloadManager(_webBrowser);
            }
            DownloadManager _manager = null;
            #region IServiceProvider Members
            public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
            {
                if (guidService == IID_IInternetSecurityManager &&
                    riid == IID_IInternetSecurityManager)
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this,
                        typeof(IInternetSecurityManager));
                    return Constants.S_OK;
                }
                //NOTE: right now we won't handle file downloading 
                //if ((guidService == Constants.IID_IDownloadManager && riid == Constants.IID_IDownloadManager))
                //{
                //    ppvObject = Marshal.GetComInterfaceForObject(_manager, typeof(IDownloadManager));
                //    return Constants.S_OK;
                //}
                ppvObject = IntPtr.Zero;
                return Constants.E_NOINTERFACE;
            }
            #endregion IServiceProvider Members

            #region IInternetSecurityManager Members
            public unsafe int SetSecuritySite(void* pSite)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int GetSecuritySite(void** ppSite)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int MapUrlToZone(string url, int* pdwZone, int dwFlags)
            {
                *pdwZone = 0;//local -> "Local", "Intranet", "Trusted", "Internet", "Restricted"
                //return Constants.S_OK;
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int GetSecurityId(string url, byte* pbSecurityId, int* pcbSecurityId, int dwReserved)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int ProcessUrlAction(string url, int dwAction, byte* pPolicy, int cbPolicy,
                byte* pContext, int cbContext, int dwFlags, int dwReserved)
            {
                *((int*)pPolicy) = (int)Constants.UrlPolicy.URLPOLICY_ALLOW;
                if (dwAction == Constants.URLACTION_SCRIPT_RUN)
                {
                    Debug.WriteLine(string.Format("{0}-{1}", dwAction, url));
                    return Constants.S_OK;
                }
                //NOTE: GLOBAL ENABLE OF ACTIVEX
                if (dwAction == Constants.URLACTION_ACTIVEX_RUN)
                {
                    Debug.WriteLine(string.Format("{0}-{1}", dwAction, url));
                    *((int*)pPolicy) = (int)Constants.UrlPolicy.URLPOLICY_ALLOW;

                    return Constants.S_OK;
                }
                if (dwAction == Constants.URLACTION_SHELL_FILE_DOWNLOAD)
                {
                    Debug.WriteLine(string.Format("{0}-{1}", dwAction, url));
                    return Constants.S_OK;
                }

                
                
                
                
                //return Constants.S_OK;
                return Constants.INET_E_DEFAULT_ACTION; 
            }

            public unsafe int QueryCustomPolicy(string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public int SetZoneMapping(int dwZone, string lpszPattern, int dwFlags)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }

            public unsafe int GetZoneMappings(int dwZone, void** ppenumString, int dwFlags)
            {
                return Constants.INET_E_DEFAULT_ACTION;
            }
            #endregion

        }
        #endregion WebBrowserSiteEx Class

        private WebBrowserSiteEx _site;

        private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
            }
        }
        private void SetBrowserFeatureControl()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make the control is not running inside Visual Studio Designer
            if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
                return;

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode()); // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_RESTRICT_ACTIVEXINSTALL", fileName, 0);
            
        }

        private UInt32 GetBrowserEmulationMode()
        {
            int browserVersion = 7;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                RegistryKeyPermissionCheck.ReadSubTree,
                System.Security.AccessControl.RegistryRights.QueryValues))
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        throw new ApplicationException("Microsoft Internet Explorer is required!");
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }

            UInt32 mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                    break;
                default:
                    // use IE10 mode by default
                    break;
            }

            return mode;
        }

        

        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            if (_site == null)
                _site = new WebBrowserSiteEx(this);
            return _site;
        }


    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    public interface IServiceProvider
    {
        [PreserveSig]
        int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b")]
    public interface IInternetSecurityManager
    {
        [PreserveSig]
        unsafe int SetSecuritySite(void* pSite);
        [PreserveSig]
        unsafe int GetSecuritySite(void** ppSite);
        [PreserveSig]
        unsafe int MapUrlToZone([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, int* pdwZone, [In] int dwFlags);
        [PreserveSig]
        unsafe int GetSecurityId([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, byte* pbSecurityId, int* pcbSecurityId, int dwReserved);
        [PreserveSig]
        unsafe int ProcessUrlAction([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, int dwAction, byte* pPolicy, int cbPolicy, byte* pContext, int cbContext, int dwFlags, int dwReserved);
        [PreserveSig]
        unsafe int QueryCustomPolicy([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved);
        [PreserveSig]
        int SetZoneMapping(int dwZone, [In, MarshalAs(UnmanagedType.LPWStr)] string lpszPattern, int dwFlags);
        [PreserveSig]
        unsafe int GetZoneMappings(int dwZone, void** ppenumString, int dwFlags);
    }

    [ComVisible(false), ComImport]
    [Guid("988934A4-064B-11D3-BB80-00104B35E7F9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDownloadManager
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Download(
            [In, MarshalAs(UnmanagedType.Interface)] IMoniker pmk,
            [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc,
            [In, MarshalAs(UnmanagedType.U4)] uint dwBindVerb,
            [In] int grfBINDF,
            [In] IntPtr pBindInfo,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszHeaders,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszRedir,
            [In, MarshalAs(UnmanagedType.U4)] uint uiCP);
    }

    public static class Constants
    {
        public const int S_OK = 0;
        public const int S_FALSE = 1;
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        public const int INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011);
        public static Guid IID_IDownloadManager = new Guid("988934A4-064B-11D3-BB80-00104B35E7F9");
        public const int URLACTION_SHELL_FILE_DOWNLOAD = unchecked((int)0x00001803);
        public const int URLACTION_SCRIPT_RUN = unchecked((int)0x00001400);
        public const int URLACTION_ACTIVEX_RUN = unchecked((int)0x00001200);
        public enum UrlPolicy
        {
            URLPOLICY_ALLOW = 0,
            URLPOLICY_QUERY = 1,
            URLPOLICY_DISALLOW = 3,
        }
    }
}