using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PasswordBoss.Helpers
{
    internal class Common
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(Common));
        private const string EmailTextbox = "textbox";
        private const string PasswordBox = "passwordbox";
        private const string EmailOffFocusStyle = "EmailTextBoxStyle";
        private const string EmailOnFocusStyle = "TextBoxStyle";
        private const string PasswordBoxOnFocusStyle = "PasswordBoxStyle";
        private const string PasswordBoxOffFocusStyle = "PasswordBoxEnterPasswordStyle";
        public static object imgLock = new object();

        /// <summary>
        ///  this will return multiple command parameter in array object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal object[] ReturnElement(object sender)
        {
            var elements = new object[2];
            var elementObjects = (FindCommandParameters)sender;
            elements[0] = elementObjects.element;
            elements[1] = elementObjects.element2;
            return elements;
        }

        /// <summary>
        /// Email validation with Regex expression
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        internal bool IsEmailValid(string emailId)
        {
            var isValidEmail = false;

            if (string.IsNullOrWhiteSpace(emailId))
            {
                return isValidEmail;
            }

            try
            {
                var userEmail = new Regex(@"^[a-zA-Z0-9][\w\.-]*\+*[a-zA-Z0-9]+@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

                if (emailId.Length > 0)
                {
                    isValidEmail = userEmail.IsMatch(emailId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }

            return isValidEmail;
        }

        /// <summary>
        /// Email validation with Regex expression
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        internal bool IsUrlValid(string url, UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            Uri result;
            if (Uri.TryCreate(url, uriKind, out result))
            {
                if (result.IsAbsoluteUri && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
                {
                    return true;
                }
                else if (!result.IsAbsoluteUri)
                {
                    return true;
                }


            }
            return false;
        }

        public bool IsImageForSiteExisting(string siteUUID)
        {
            bool isExisting = false;

            if (siteUUID != null)
            {
                string imageDirectory = AppHelper.ImageFolderLocation;
                string imageFileName = null;
                if (siteUUID != null)
                {
                    imageFileName = imageDirectory + siteUUID + ".png";
                }

                isExisting = File.Exists(imageFileName);
            }

            return isExisting;
        }

        public string GetImagePathForSite(string siteUUID = null, bool returnDefultIfNeeded = true)
        {
            if (IsImageForSiteExisting(siteUUID))
            {
                string imageDirectory = AppHelper.ImageFolderLocation;
                string imageFileName = null;
                if (siteUUID != null)
                {
                    imageFileName = imageDirectory + siteUUID + ".png";
                }

                return imageFileName;
            }

            if (returnDefultIfNeeded)
            {
                var img = Application.Current.Resources["imgPbOwlGrey"] as BitmapImage;
                if (img != null)
                {
                    return img.ToString();
                }
            }
            return null;
        }

        public ImageSource GetImageForPath(string imageFileName, bool loadDefaultIfNeeded = true)
        {
            if (imageFileName == null && loadDefaultIfNeeded)
            {
                return (ImageSource)Application.Current.Resources["imgPbOwlGrey"];
            }
            else if (imageFileName != null && !imageFileName.StartsWith("pack:", StringComparison.CurrentCultureIgnoreCase) && !File.Exists(imageFileName))
            {
                return (ImageSource)Application.Current.Resources["imgPbOwlGrey"]; //imgPasswordBossLogoGreenIcon //imgPbKey
            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.UriSource = new Uri(imageFileName, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            return bi.Clone() as ImageSource;
        }

        public ImageSource GetDefaultImageOrImageForSite(string siteUUID = null)
        {
            ImageSource siteImage = null;
            string imageDirectory = AppHelper.ImageFolderLocation;
            string imageFileName = null;
            if (siteUUID != null)
            {
                imageFileName = imageDirectory + siteUUID + ".png";
            }

            siteImage = GetImageForPath(imageFileName);

            return siteImage;
        }

        /// <summary>
        /// hides place holder text as soon as text is entered
        /// </summary>
        /// <param name="element"></param>
        /// <param name="onFocusStyle"></param>
        /// <param name="offFocusStyle"></param>
        internal void ElementTextChanged(object element, string onFocusStyle, string offFocusStyle)
        {

            if (element is TextBox)
            {
                var emailTextBox = element as TextBox;
                if (emailTextBox != null && emailTextBox.Text.Any())
                {
                    ElementFocusedChanged(element, onFocusStyle, EmailTextbox);
                }
                else
                    ElementFocusedChanged(element, offFocusStyle, EmailTextbox);

            }
            else if (element is PasswordBox)
            {
                var masterPasswordBox = element as PasswordBox;
                if (masterPasswordBox != null && masterPasswordBox.Password.Any())
                {
                    ElementFocusedChanged(element, onFocusStyle, PasswordBox);
                }
                else
                    ElementFocusedChanged(element, offFocusStyle, PasswordBox);
            }
        }

        /// <summary>
        /// Enable place holder text through binding style
        /// </summary>
        /// <param name="element"></param>
        internal void ElementLostFocus(object element)
        {

            if (element != null && element is TextBox)
            {
                ElementFocusedChanged(element, EmailOffFocusStyle, EmailTextbox);
            }
            else if (element != null && element is PasswordBox)
            {
                ElementFocusedChanged(element, PasswordBoxOffFocusStyle, PasswordBox);
            }
        }

        /// <summary>
        /// Diasble place holder text through binding style
        /// </summary>
        /// <param name="element"></param>
        internal void ElementGotFocus(object element)
        {
            if (element != null && element is TextBox)
            {
                ElementFocusedChanged(element, EmailOnFocusStyle, EmailTextbox);
            }
            else if (element != null && element is PasswordBox)
            {
                ElementFocusedChanged(element, PasswordBoxOnFocusStyle, PasswordBox);
            }
        }

        /// <summary>
        /// Applies style on Email text Box and Password Box on losing and getting focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="style"></param>
        /// <param name="elementType"></param>
        internal void ElementFocusedChanged(object sender, string style, string elementType)
        {
            switch (elementType)
            {
                case "textbox":
                    var emailTextBox = sender as TextBox;
                    emailTextBox.Style = Application.Current.Resources[style] as Style;

                    break;
                case "passwordbox":
                    var masterPasswordBox = sender as PasswordBox;
                    masterPasswordBox.Style = Application.Current.Resources[style] as Style;
                    break;
            }
        }

        /// <summary>
        /// return respective image from resource based on resource key name
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        internal ImageSource ReturnImageIcon(string resourceKey)
        {
            return (ImageSource)Application.Current.FindResource(resourceKey);
        }

        public int TimeDiffToDate(DateTime toDate, out string unit)
        {

            if (toDate < DateTime.Now)
            {
                unit = String.Empty;
                return 0;
            }
            int result = 0;
            TimeSpan ts = toDate - DateTime.Now;
            if (ts.Days > 365)
            {
                result = 0;
                unit = "---";
                return result;
            }

            if (ts.Days > 0)
            {
                result = ts.Days;
                unit = "d";
            }
            else if (ts.Hours > 0)
            {
                result = ts.Hours;
                unit = "h";
            }
            else if (ts.Minutes > 0)
            {
                result = ts.Minutes;
                unit = "m";
            }
            else
            {
                result = ts.Seconds;
                unit = "s";
            }
            if (result < 0) result = result * -1;
            return result;
        }

        public int TimeDiffFromNow(DateTime fromDate, out string unit)
        {
            if (fromDate > DateTime.Now)
            {
                unit = String.Empty;
                return 0;
            }
            int result = 0;
            TimeSpan ts = DateTime.Now - fromDate;
            if (ts.Days > 0)
            {
                result = ts.Days;
                unit = "d";
            }
            else if (ts.Hours > 0)
            {
                result = ts.Hours;
                unit = "h";
            }
            else if (ts.Minutes > 0)
            {
                result = ts.Minutes;
                unit = "m";
            }
            else
            {
                result = ts.Seconds;
                unit = "s";
            }
            if (result < 0) result = result * -1;
            return result;
        }
    }
}