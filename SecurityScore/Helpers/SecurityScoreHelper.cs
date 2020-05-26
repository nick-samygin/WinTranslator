using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PasswordBoss.Helpers
{
   internal  class SecurityScoreHelper
    {
        internal const string SemiboldWeight = "SemiboldWeight";
        internal const string NormalWeight = "NormalWeight";
        internal const string ProximaRegularFamily = "ProximaRegularFamily";
        internal const string ProximaSemiBoldfamily = "ProximaSemiBoldfamily";
        //internal const string TextBoxOnFocusStyle = "BlankTextBoxStyle";

        internal const string SecurityScoreDynamicContentColor = "SecurityScoreDynamicContentColor";
        internal const string FillEllipsesColor = "PasswordBossGreenColor";
        internal const string Foregroundselectedcolor = "WhiteColor";

       internal static Brush ReturnWizardcolors(string resource)
       {
           return (Brush)Application.Current.FindResource(resource);
       }

       /// <summary>
       /// this function returns default properties of background color
       /// </summary>
       /// <returns></returns>
       internal static Brush AlertBorderStrokecolor(string KeyName)
       {
           return (Brush)Application.Current.FindResource(KeyName);
       }

       // <summary>
       /// this function returns font weight
       /// </summary>
       /// <param name="resource"></param>
       /// <returns></returns>
       internal static FontWeight ReturnFontWeight(string resource)
       {
           return (FontWeight)Application.Current.FindResource(resource);
       }


       /// <summary>
       /// this function returns font family
       /// </summary>
       /// <param name="resource"></param>
       /// <returns></returns>
       internal static FontFamily ReturnFontFamily(string resource)
       {
           return (FontFamily)Application.Current.FindResource(resource);
       }
    }
}
