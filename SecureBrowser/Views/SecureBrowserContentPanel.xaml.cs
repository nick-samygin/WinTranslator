using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasswordBoss.ViewModel;
using System.Windows.Forms.Integration;
using PasswordBoss.Views.UserControls;
using System.Diagnostics;
using SecureBrowser.DNS;
using System.Net;
using System.Net.Sockets;
using System.IO;
using PasswordBoss.Helpers;
using System.Threading.Tasks;
using System.Threading;
using SHDocVw;



namespace PasswordBoss.Views
{
    
    /// <summary>
    /// Interaction logic for SecureBrowserContentPanel.xaml
    /// </summary>
    public partial class SecureBrowserContentPanel : UserControl
    {
        static SecureBrowserContentPanel()
        {
            WebBrowserHelper.ClearCache();
        }
        private IResolver resolver = null;
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecureBrowser));
        private IPBWebAPI webApi = null;
        public SecureBrowserContentPanel(IResolver resolver)
        {
            InitializeComponent();
            this.DataContext = new SecureBrowserViewModel(resolver);
            this.resolver = resolver;
            webApi = resolver.GetInstanceOf<IPBWebAPI>();
        }

        private void gWebBrowserContainer_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        public void OnShowSecureBrowser()
        {
            var model = this.DataContext as SecureBrowserViewModel;
            if(model != null)
            {
                model.LoadStartPage();
                model.InitialHomePageItemsLoad();
                model.LoadHomepageItemList();
            }
        }
        /// <summary>
        /// Fires before navigation occurs in the given object (on either a window or frameset element).
        /// </summary>
        /// <param name="pDisp">Object that evaluates to the top level or frame WebBrowser object corresponding to the navigation.</param>
        /// <param name="url">String expression that evaluates to the URL to which the browser is navigating.</param>
        /// <param name="Flags">Reserved. Set to zero.</param>
        /// <param name="TargetFrameName">String expression that evaluates to the name of the frame in which the resource will be displayed, or Null if no named frame is targeted for the resource.</param>
        /// <param name="PostData">Data to send to the server if the HTTP POST transaction is being used.</param>
        /// <param name="Headers">Value that specifies the additional HTTP headers to send to the server (HTTP URLs only). The headers can specify such things as the action required of the server, the type of data being passed to the server, or a status code.</param>
        /// <param name="Cancel">Boolean value that the container can set to True to cancel the navigation operation, or to False to allow it to proceed.</param>
        private delegate void BeforeNavigate2(object pDisp, ref dynamic url, ref dynamic Flags, ref dynamic TargetFrameName, ref dynamic PostData, ref dynamic Headers, ref bool Cancel);

        /// <summary>
        /// Fires to indicate that a file download is about to occur. If a file download dialog box can be displayed, this event fires prior to the appearance of the dialog box.
        /// </summary>
        /// <param name="bActiveDocument">A Boolean that specifies whether the file is an Active Document.</param>
        /// <param name="bCancel">A Boolean that specifies whether to continue the download process and display the download dialog box.</param>
        private delegate void FileDownload(bool bActiveDocument, ref bool bCancel);
 
        private void gWebBrowserContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var container = sender as Grid;
            if (container != null && container.Children.Count == 0)
            {
                WindowsFormsHost host = new WindowsFormsHost();
                container.Children.Add(host);
                var model = this.DataContext as SecureBrowserViewModel;
                WebBrowserEx wb = new WebBrowserEx(model.SelectedTabItem);
                wb.Navigating += wb_Navigating;
                wb.Navigated += wb_Navigated;
                //wb.Validating += wb_Validating;
                wb.FileDownload += wb_FileDownload;
                wb.DocumentCompleted += wb_DocumentCompleted;
                wb.ProgressChanged += wb_ProgressChanged;
                wb.ScriptErrorsSuppressed = true; //disable popup for javascript errors
                
                SHDocVw.WebBrowser axBrowser = (SHDocVw.WebBrowser)wb.ActiveXInstance;
                axBrowser.NewWindow3 += axBrowser_NewWindow3;
                //axBrowser.NewWindow2 += axBrowser_NewWindow2;
                //wb.IsWebBrowserContextMenuEnabled = false; //disable context menu
                //dynamic d = wb.ActiveXInstance;
                //string uri = string.Empty;

                //d.BeforeNavigate2 += new BeforeNavigate2((object pDisp,
                //    ref dynamic url,
                //    ref dynamic Flags,
                //    ref dynamic TargetFrameName,
                //    ref dynamic PostData,
                //    ref dynamic Headers,
                //    ref bool Cancel) =>
                //    {

                //        uri = url.ToString(); 
                //        Trace.WriteLine(uri);
                //    });

                //d.FileDownload += new FileDownload((bool bActiveDocument, ref bool bCancel) =>
                //{
                //    bool isFile = uri.EndsWith("exe");

                //    if (isFile)
                //    {
                //        bCancel = true;
                //        Trace.Write("Canceled a file download from the DLR.");
                //    }
                //});

                host.Child = wb;

                if (model != null && model.SelectedTabItem != null)
                {
                    model.SelectedTabItem.SearchBar.OnNavigateRequired += SearchBar_OnNavigateRequired;
                    model.SelectedTabItem.WebBrowser = wb;
                    if (!string.IsNullOrWhiteSpace(model.SelectedTabItem.SearchBar.Address))
                    {
                        wb.Navigate(model.SelectedTabItem.SearchBar.Address);
                    }
                }
                
            }
        }


        void axBrowser_NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
        {
            Cancel = true;
            if(!string.IsNullOrWhiteSpace(bstrUrl))
            {
                var model = this.DataContext as SecureBrowserViewModel;
                model.SelectedTabItem.AddNewTabForUrl(bstrUrl, true);
            }
        }


        

        void wb_ProgressChanged(object sender, System.Windows.Forms.WebBrowserProgressChangedEventArgs e)
        {
            var wb = sender as WebBrowserEx;
            var model = wb.CurrentTabItem;
            if (model != null && e.CurrentProgress != -1)
            {
                if (e.CurrentProgress < e.MaximumProgress)
                {
                    model.Navigating = true;
                }
                else
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(1000);
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            model.Navigating = false;
                        });
                    });
                    
                   
                }
            }
        }

        void wb_FileDownload(object sender, EventArgs e)
        {
            
        }

        

        void wb_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var wb = sender as WebBrowserEx;
            if (wb != null)
            {
                RemoveForbiddenElements(wb);
            }
        }

        void RemoveForbiddenElements(WebBrowserEx wb)
        {
            //NOTE: Right now we will enable all plugins to run
            //if (wb.Document != null)
            //{
            //    List<string> tagNames = new List<string>();
            //    StringBuilder sb = new StringBuilder();
            //    foreach (dynamic elm in wb.Document.All)
            //    {

            //        if (!tagNames.Contains((dynamic)elm.TagName))
            //        {
            //            tagNames.Add((dynamic)elm.TagName);
            //        }
            //        if ((dynamic)elm.TagName.ToLower() == "embed")
            //        {
            //            //NOTE: elm.GetAttribute doesn't return value for "type", it only works for src!?!?!?!?!
            //            //then there is always workaround
            //            elm.Enabled = false;
            //            if (elm.OuterHtml.ToLower().Contains("application/x-shockwave-flash"))
            //            {
            //                //elm.OuterHtml = elm.OuterHtml.Replace("application/x-shockwave-flash", "");
            //                //HTMLDocumentClass htmldoc = wbCtrl.Document.DomDocument as HTMLDocumentClass;
            //                //IHTMLDOMNode node = htmldoc.getElementById("xBar") as IHTMLDOMNode;
            //                //node.parentNode.removeChild(node);
            //                dynamic parentNode = elm.DomElement.parentNode;
            //                parentNode.removeChild(elm.DomElement);


            //                //dynamic elem = wb.Document.CreateElement("P");

            //                //elem.InnerText = "PLUGIN REMOVED BY SECURE BROWSER";

            //                // parentNode.appendChild(elem);

            //            }
            //            if (elm.GetAttribute("type") == "application/x-shockwave-flash")
            //            {
            //                sb.Append(elm.InnerHtml);
            //            }
            //        }
            //    }

            //    Debug.WriteLine(sb.ToString());

            //}
        }

     
        bool ValidateDNSEntry(Uri url,out string firstCustomIp, params IPEndPoint[] dnsList)
        {
            Resolver _resolver = new Resolver();
            firstCustomIp = string.Empty;
            bool isValid = false;
            try
            {
                if (url != null  && (url.AbsoluteUri == "about:blank" || url.AbsoluteUri == "ieframe.dll"))
                {
                    isValid = true;
                }
                else if (url != null && !string.IsNullOrWhiteSpace(url.DnsSafeHost))
                {
                    IPAddress[] systemDnsIpList = null;
                    Response response = null;
                    lock (_resolver)
                    {
                        _resolver.Recursion = true;
                        _resolver.UseCache = true;
                        //_resolver.TimeOut = 10000;
                        //_resolver.Retries = 10;
                        _resolver.TransportType = global::SecureBrowser.DNS.TransportType.Tcp;
                        _resolver.DnsServers = dnsList;

                        response = _resolver.Query(url.DnsSafeHost, QType.A, QClass.IN);

                    }
                   
                    try
                    {
                        //NativeErrorCode	11004	int
                        systemDnsIpList = System.Net.Dns.GetHostEntry(url.DnsSafeHost)
                            .AddressList.Where(x=>x.AddressFamily == AddressFamily.InterNetwork).ToArray();
                    }
                    catch (SocketException ex )
                    {
                        if ((ex.NativeErrorCode == 11004 && response.RecordsA.Length == 0) || ex.NativeErrorCode == 11001)
                        {
                            //Address can't be resolved by DNS
                            isValid = true;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    
                    if(response.RecordsA.Length == 0)
                    {
                        //can't resolve right now
                        isValid = true;
                    }

                    if(systemDnsIpList != null && response.RecordsA.Length != systemDnsIpList.Length)
                    {
                        logger.Warn(string.Format("Record list doesn't match, custom: {0}, system: {1}, url: {2}", response.RecordsA.Length, systemDnsIpList.Length, url));
                    }
                    
                    foreach (var record in response.RecordsA)
                    {
                        firstCustomIp = record.Address.ToString();
                        if (systemDnsIpList != null  && systemDnsIpList.Contains(record.Address))
                        {
                            isValid = true;
                        }
                    }

                }
                else
                {
                    isValid = true;
                }

            }
            catch(IOException ioex) //sometimes request to comodo dns times out
            {
                isValid = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                //isValid = false;
            }

            return isValid;
        }
        bool ValidateDNSEntrySameServer(Uri url)
        {
            string firstCustomIp = "";
            return ValidateDNSEntry(url, out firstCustomIp, Resolver.GetDnsServers());
        }
        bool ValidateDNSEntrySecureServer(Uri url, out string secureIP)
        {
            secureIP = string.Empty;
            var dnsServer = System.Net.IPAddress.Parse("8.26.56.26");
            var dnsServer2 = System.Net.IPAddress.Parse("8.20.247.20");
            IPEndPoint dns1 = new IPEndPoint(dnsServer, Resolver.DefaultPort);
            IPEndPoint dns2 = new IPEndPoint(dnsServer2, Resolver.DefaultPort);
            
            bool isValid = false;
            isValid = ValidateDNSEntry(url, out secureIP, new[] { dns1, dns2 });
            return isValid;
        }

        Common common = new Common();
        private bool IsTrustedByComodo(string uri)
        {
            
            bool isTrustedByComodo = true;
            if (!common.IsUrlValid(uri, uriKind: UriKind.Absolute))
            {
                return isTrustedByComodo;
            }
            try
            { 
                String test = String.Empty;
                using (HttpWebResponse response = webApi.CreateWhitelistedResponse(uri))
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            test = reader.ReadLine();
                            var safeHostUrl = response.ResponseUri.DnsSafeHost.ToLower();

                            if (safeHostUrl.Contains("comodo") && safeHostUrl.Contains("warn"))
                            {
                                isTrustedByComodo = false;
                            } 

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(uri);
                logger.Error(ex.ToString());
            }
           
            return isTrustedByComodo;
        }

        private static Dictionary<string, bool> checkedSiteDict = new Dictionary<string, bool>();

        void wb_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            var wb = sender as WebBrowserEx;
            var model = wb.CurrentTabItem;
            if(model != null && model.SearchBar.Address == null)
            {
                return;
            }

            if (model != null && model.WebBrowser.Url != null)
            {
                bool redirectToBlockedPageInfo = false;
                if(model.SearchBar.Address != wb.Url.AbsoluteUri)
                {
                    string secureIP = "";
                    bool isValid = ValidateDNSEntrySecureServer(wb.Url, out secureIP);
                    bool isTrustedByComodo = true;
                    if(!string.IsNullOrEmpty(secureIP) && !isValid)
                    {
                        //we create request by IP resolved by Comodo DNS
                        //if it resolves to problematic IP we can redirect browser somewhere...
                        if(!checkedSiteDict.ContainsKey(wb.Url.DnsSafeHost))
                        { 
                            isTrustedByComodo = IsTrustedByComodo("http://" + secureIP);
                            isValid = isTrustedByComodo;
                            checkedSiteDict.Add(wb.Url.DnsSafeHost, isTrustedByComodo); 
                        }
                        else
                        {
                            isValid = checkedSiteDict[wb.Url.DnsSafeHost];
                            isTrustedByComodo = checkedSiteDict[wb.Url.DnsSafeHost];
                        }
                    }
                    if (isValid)
                    {
                        wb.CurrentTabItem.SearchBar.IPBySecureDNS = secureIP;
                        wb.CurrentTabItem.SearchBar.AddressVerificationByDNSVisibility = System.Windows.Visibility.Collapsed; 
                    }
                    else
                    {
                        redirectToBlockedPageInfo = true;
                        wb.CurrentTabItem.SearchBar.IPBySecureDNS = secureIP;
                        wb.CurrentTabItem.SearchBar.AddressVerificationByDNSVisibility = System.Windows.Visibility.Visible;
                    }
                }
                if(redirectToBlockedPageInfo)
                {
                    wb.Navigate(DefaultProperties.PasswordBossBlockedSiteUrl);
                }
                else
                {
                    model.SearchBar.Address = wb.Url.AbsoluteUri; //e.Url.AbsoluteUri;
                }


                if (model != null)
                {
                    if (model.CanGoBack != model.WebBrowser.CanGoBack)
                    {
                        model.CanGoBack = model.WebBrowser.CanGoBack;
                    }
                    if (model.CanGoForward != model.WebBrowser.CanGoForward)
                    {
                        model.CanGoForward = model.WebBrowser.CanGoForward;
                    }
                }
               
            }

            RemoveForbiddenElements(wb);
        }

        void SearchBar_OnNavigateRequired(TabItemSearchBar model)
        {
            if(!string.IsNullOrEmpty(model.Address))
            { 
                //wb.Navigate(model.Address);  
                model.TabItem.WebBrowser.Navigating += WebBrowser_Navigating;
                model.TabItem.WebBrowser.Navigate(model.Address);
                
            }    
        }

        void WebBrowser_Navigating(object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e)
        {
            var wb = sender as WebBrowserEx;
            if (wb != null)
            {
                var model = wb.CurrentTabItem;
                if (model != null)
                {
                    if(model.CanGoBack != model.WebBrowser.CanGoBack)
                    {
                        model.CanGoBack = model.WebBrowser.CanGoBack;
                    }
                    if (model.CanGoForward != model.WebBrowser.CanGoForward)
                    {
                        model.CanGoForward = model.WebBrowser.CanGoForward;
                    }
                }
                
            }
        }

        void wb_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            var wb = sender as WebBrowserEx;
            RemoveForbiddenElements(wb);
            string title = ((dynamic)wb.Document).Title;
            if (string.IsNullOrEmpty(title))
            {
                title = e.Url.ToString().Replace("http://", "");
                title = e.Url.ToString().Replace("https://", "");
            }

            var model = wb.CurrentTabItem;
            if (model != null && title != "about:blank")
            {
                model.SearchBar.Title = title;
            }

            if (model != null)
            {
                if (model.CanGoBack != model.WebBrowser.CanGoBack)
                {
                    model.CanGoBack = model.WebBrowser.CanGoBack;
                }
                if (model.CanGoForward != model.WebBrowser.CanGoForward)
                {
                    model.CanGoForward = model.WebBrowser.CanGoForward;
                }
            }
           
            //FIXME: doesn't work for iframes, causes access violation exception in Port Manager - Nemanja Tosic
//            wb.Document.ContextMenuShowing += Document_ContextMenuShowing;
        }

        void Document_ContextMenuShowing(object sender, System.Windows.Forms.HtmlElementEventArgs e)
        {
            //Debugger.Launch();
            e.ReturnValue = false;
            //((mshtml.HTMLAnchorElementClass)(((System.Windows.Forms.HtmlDocument)(sender)).ActiveElement.DomElement)).href
            dynamic domElement = sender;
            if (domElement.ActiveElement != null)
            {
                if (domElement.ActiveElement.DomElement.TagName == "A") //just anchor
                {
                    // expandoObject.SomeMember exists.
                    dynamic href = domElement.ActiveElement.DomElement.href;
                    
                    var model = this.DataContext as SecureBrowserViewModel;
                    if (model.SelectedTabItem.WebBrowser != null)
                    {
                        var browser = model.SelectedTabItem.WebBrowser;
                        model.SelectedTabItem.WebBrowser.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                        //var menuItem = new System.Windows.Forms.MenuItem("Open in new tab",new EventHandler(delegate(object menuSender, EventArgs menuEventArgs)
                        //{
                        //    MessageBox.Show("Novi: " + href.ToString());
                        //}));
                       // Debugger.Launch();
                        if (model.SelectedTabItem.WebBrowser.ContextMenuStrip.Items.Count == 0)
                        {
                            model.SelectedTabItem.WebBrowser.ContextMenuStrip.Items.Add("Open in a new tab", null, menuItem1_Click);
                        }
                        //var menuItem = new System.Windows.Forms.MenuItem("Open in new tab", menuItem1_Click);
                        
                        //model.SelectedTabItem.WebBrowser.ContextMenu.MenuItems.Add(menuItem);
                        System.Drawing.Point p = GetMousePositionWindowsForms();//new System.Drawing.Point(e.MousePosition.X, e.MousePosition.Y);
                        model.SelectedTabItem.WebBrowser.ContextMenuStrip.Show(p);
                    }
                }
                else
                {
                    
                }
            }
           
        }
        public System.Drawing.Point GetMousePositionWindowsForms()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new System.Drawing.Point(point.X, point.Y);
        }

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            // Create a new OpenFileDialog and display it.
            var model = this.DataContext as SecureBrowserViewModel;
            dynamic activeElement = model.SelectedTabItem.WebBrowser.Document.ActiveElement;
            if(activeElement != null)
            {
                model.SelectedTabItem.AddNewTabForUrl(activeElement.DomElement.href, true);
                //MessageBox.Show(activeElement.DomElement.href);
            }
           
        }

        static IList<string> checkedAddressList = new List<string>();

        void wb_Navigating(object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e)
        {
            var wb = sender as WebBrowserEx;
            RemoveForbiddenElements(wb);  
            var model = wb.CurrentTabItem;
            
            //if (model != null && model.SearchBar.Address == e.Url.OriginalString)
            //{
            //    bool isValid = ValidateDNSEntrySameServer(e.Url); 
            //    if(!isValid)
            //    {
            //        MessageBox.Show("Navigation canceled! IP address is different when retreived from system dll and from secure browser.", "SecureDNS", MessageBoxButton.OK, MessageBoxImage.Stop);
            //        e.Cancel = true;
            //    } 
            //}
            ValidateLocalDNSEntry(e, wb);
        }

        private void ValidateLocalDNSEntry(System.Windows.Forms.WebBrowserNavigatingEventArgs e, WebBrowserEx wb)
        {
            string addressForChecking = string.Empty;
            if (wb.Url == null)
            {
                addressForChecking = e.Url.ToString();
            }
            if (wb.Url != null && e.Url != null
                && wb.Url.DnsSafeHost != e.Url.DnsSafeHost)
            {
                addressForChecking = e.Url.ToString();
            }
            else if (wb.Url != null)
            {
                addressForChecking = wb.Url.ToString();
            }

            if (!string.IsNullOrEmpty(addressForChecking))
            {
                Uri uriForChecking = new Uri(addressForChecking);
                if (!checkedAddressList.Contains(uriForChecking.DnsSafeHost))
                {
                    bool isValid = ValidateDNSEntrySameServer(uriForChecking);
                    if (!isValid)
                    {
                        isValid = IsTrustedByComodo(addressForChecking);
                        if(!isValid)
                        {
                            MessageBox.Show("Navigation canceled! IP address is different when retreived from system dll and from secure browser.", "SecureDNS", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        
                    }
                    checkedAddressList.Add(uriForChecking.DnsSafeHost);
                }

            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            var model = this.DataContext as SecureBrowserViewModel;
            if(model.SelectedTabItem.WebBrowser.CanGoBack)
            {
                model.SelectedTabItem.WebBrowser.GoBack();
            }
        
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            var model = this.DataContext as SecureBrowserViewModel;
            if (model.SelectedTabItem.WebBrowser.CanGoForward)
            {
                model.SelectedTabItem.WebBrowser.GoForward();
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
