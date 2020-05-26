using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using mshtml;
using PasswordBoss.ViewModel;
using SHDocVw;

namespace PasswordBoss.Helpers
{
    [ComVisible(true)]
    public class PortManager
    {
        private const string Key = "SecureBrowser";
        private const string PbProxyFunction = "PBSecureBrowserMessage";
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
        private readonly IPBComm _iPbComm;
        private readonly ObservableCollection<TabItem> _tabs;
        private string _typeDefinitions;

        public PortManager(ObservableCollection<TabItem> tabs, IResolver resolver)
        {
            _tabs = tabs;
            _tabs.CollectionChanged += TabCollectionChanged;
            _iPbComm = resolver.GetInstanceOf<IPBComm>();

            _iPbComm.OnSecureBrowserMessageEvent(OnMessageFromBackend);
            _iPbComm.ConnectSecureBrowser();
            SendMessageToBackend(new BrowserInfoMessage("Secure Browser", "0.0.1"));
        }

        #region Messaging

        private static void SendMessageToWindow(HtmlDocument document, string msg)
        {
            document.InvokeScript("PBSecureBrowserMessage", new object[] {msg});
        }

        private static void SendMessageToWindow(IHTMLWindow2 window, string msg)
        {
            InvokeMember(window, "PBSecureBrowserMessage", msg);
        }

        private static object InvokeMember(IHTMLWindow2 window, string function, string arguments)
        {
            try
            {
                return window.GetType().InvokeMember(function, BindingFlags.InvokeMethod, null, window, new object[] {arguments});
            }
            catch (Exception)
            {
                // ignored - we can't run js on some frames - like google ads etcs
            }

            return null;
        }

        private string OnMessageFromBackend(string sessionKey, string msg)
        {
            var message = Serializer.Deserialize<ContentMessage>(msg);

            switch (message.message)
            {
                case "typeDefinitions":
                    _typeDefinitions = msg;
                    return null;
            }

            if (message.tabId == null)
                throw new Exception("Unhandled browser message");

            var tab = _tabs.First(tabItem => tabItem.TabId == message.tabId);

            var document = tab.WebBrowser.Document;

            if (message.contentId == "0")
                SendMessageToWindow(document, msg);
            else
            {
                var frame = GetFrameByContentId(document, message.contentId);
                if (frame != null)
                    SendMessageToWindow(frame, msg);
            }

            return null;
        }

        private static IEnumerable<IHTMLWindow2> GetFramesFromDocument(HtmlDocument document)
        {
            var windows = new List<IHTMLWindow2>();

            var iDocument2 = document.DomDocument as IHTMLDocument2;
            Debug.Assert(iDocument2 != null, "iDocument2 != null");

            var window = iDocument2.parentWindow;
            var frames = window.frames;

            for (var i = 0; i < frames.length; i++)
            {
                object refIdx = i;
                windows.Add(CrossFrameWindow.GetDocumentFromWindow((IHTMLWindow2) frames.item(ref refIdx)).parentWindow);
            }

            return windows;
        }

        private static IHTMLWindow2 GetFrameByContentId(HtmlDocument document, string contentId)
        {
            return GetFramesFromDocument(document).FirstOrDefault(frame =>
            {
                try
                {
                    return GetWindowContentId(frame) == contentId;
                }
                catch (Exception)
                {
                    // ignored - we can't run js on some frames - like google ads etc
                }

                return false;
            });
        }

        #endregion

        [ComVisible(true)]
        // ReSharper disable once MemberCanBePrivate.Global
        public class NativeMessenger
        {
            private readonly string _tabId;
            private readonly string _url;
            private readonly IPBComm _iPbComm;

            public NativeMessenger(string tabId, string url, IPBComm iPbComm)
            {
                _tabId = tabId;
                _url = url;
                _iPbComm = iPbComm;
            }

            // ReSharper disable once UnusedMember.Global
            public void SendMessageToBackend(string message)
            {
                //generic way to embed key, url and tab id
                var dictionary = Serializer.Deserialize<Dictionary<string, object>>(message);
                dictionary["key"] = Key;
                dictionary["url"] = _url;
                dictionary["tabId"] = _tabId;
                message = Serializer.Serialize(dictionary);

                _iPbComm.SendMessageToBackend(Key, message);
            }
        }

        private void SendMessageToBackend(Message message)
        {
            message.key = Key;
            _iPbComm.SendMessageToBackend(Key, Serializer.Serialize(message));
        }

        private void ConnectToTab(TabItem tabItem)
        {
            SendMessageToBackend(new TabOpenedMessage(tabItem.TabId));
            var webBrowser = tabItem.WebBrowser;
            var axInstance = webBrowser.ActiveXInstance as dynamic;

            axInstance.DocumentComplete += new Action<object, string>((pDisp, url) =>
            {
                if (string.Compare(url, "about:blank", StringComparison.OrdinalIgnoreCase) == 0) return;

                var browser = pDisp as IWebBrowser2;
                Debug.Assert(browser != null, "browser != null");

                var document = browser.Document as IHTMLDocument2;
                if (document == null) return;

                var window = document.parentWindow;

                // ReSharper disable once SuspiciousTypeConversion.Global
                var windowEx = window as IExpando;
                Debug.Assert(windowEx != null, "windowEx != null");
                windowEx.AddProperty("PBExtension").SetValue(window, new NativeMessenger(tabItem.TabId, window.location.href, _iPbComm), null);

                InvokeMember(window, "eval", "function " + PbProxyFunction + "(msg) { pb.content.apiImpl.read(msg); }");
                InvokeMember(window, "eval", JavaScriptInjectionManager.GetJavaScriptForInjection());

                SendMessageToWindow(window, _typeDefinitions);
                SendMessageToWindow(window, Serializer.Serialize(new BackendConnectedMessage()));
            });
        }

        private static string GetWindowContentId(IHTMLWindow2 window2)
        {
            var pbContentId = InvokeMember(window2, "eval", "typeof PBContentId === 'undefined' ? -1 : PBContentId");
            return pbContentId == null ? "-1" : pbContentId.ToString();
        }

        #region Monitor tab changes

        //Monitor when a tab has been added to the collection, and then wait for the WebBrowser to be instantiated
        private void TabCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TabItem newItem in args.NewItems)
                        newItem.PropertyChanged += TabCollectionPropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TabItem oldItem in args.OldItems)
                    {
                        oldItem.PropertyChanged -= TabCollectionPropertyChanged;
                        SendMessageToBackend(new TabClosedMessage(oldItem.TabId));
                    }
                    break;
            }
        }

        private void TabCollectionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "WebBrowserInitiated") return;

            ConnectToTab(sender as TabItem);
        }

        #endregion

        #region JSON Handling

        private class Message
        {
            public string message { get; set; }
            public string key { get; set; }
        }

        private class BackendConnectedMessage : Message
        {
            public BackendConnectedMessage()
            {
                message = "backendConnected";
                backendConnected = true;
            }

            public bool backendConnected { get; set; }
        }

        private class BrowserInfoMessage : Message
        {
            public BrowserInfoMessage(string browserName, string browserVersion)
            {
                message = "browserInfo";
                this.browserName = browserName;
                this.browserVersion = browserVersion;
            }

            public string browserName { get; set; }
            public string browserVersion { get; set; }
        }

        private class TabMessage : Message
        {
            public string tabId { get; set; }
        }

        private class ContentMessage : TabMessage
        {
            public string contentId { get; set; }
        }

        private class TabOpenedMessage : TabMessage
        {
            public TabOpenedMessage(string tabId)
            {
                message = "tabOpened";
                this.tabId = tabId;
            }
        }

        private class TabClosedMessage : TabMessage
        {
            public TabClosedMessage(string tabId)
            {
                message = "tabClosed";
                this.tabId = tabId;
            }
        }

        #endregion
    }
}
