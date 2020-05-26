using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PasswordBoss.Helpers
{
    public static class JavaScriptInjectionManager
    {
        public static string GetJavaScriptForInjection()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string fullResourceName = "PasswordBoss.scripts.content-ie.min.js";

            using (var reader = new StreamReader(assembly.GetManifestResourceStream(fullResourceName)))
                return reader.ReadToEnd();
        }

        public static void InjectJavaScript(HtmlDocument document)
        {
            document.InvokeScript("eval", new object[] {"function PBSecureBrowserMessage(msg) { pb.content.apiImpl.read(msg); }"});
            document.InvokeScript("eval", new object[] {GetJavaScriptForInjection()});
        }
    }
}
