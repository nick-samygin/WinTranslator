using PasswordBoss.ViewModel.ApplicationUpdates;
using PasswordBoss.Views.ApplicationUpdates;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace PasswordBoss.Actions
{
    [Export(typeof(IAction))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class DisplayUpdatePrompt : IAction
    {
        private readonly ILogger logger = Logger.GetLogger(typeof(DisplayUpdatePrompt));
        private TimeSpan chkFreq = TimeSpan.FromDays(3650);

        private readonly IPBData pbData;

        [ImportingConstructor]
        public DisplayUpdatePrompt([Import(typeof(IResolver))] IResolver resolver)
        {
            this.pbData = resolver.GetInstanceOf<IPBData>();
        }

        public string ActionType
        {
            get { return "IN_APP_UPDATE_PASSWORD_BOSS_PROMPT"; }
        }

        public ActionStatusInfo ExecuteAction(string uid, ActionStatusInfo statusInfo)
        {

            var ret = new ActionStatusInfo
            {
                Status = ActionStatus.Incomplete,
                WriteBackStartDate = true,
                StartDate = DateTime.Now.Add(chkFreq)
            };

            if (!pbData.UpdateTonightScheduled())
            {
            var jsonFile = FindUpdateJsonFile();
            var arg = GetArgumentsFromJsonFile(jsonFile);
            var updaterParams = UpdaterData.ReadFromFile(jsonFile);
            var version = AppVersion.GetInstalledVersion();
                var binary = UpdatePathHelper.PBUpdaterPath;



            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                    UpdateAvailableViewModel dc = new UpdateAvailableViewModel(pbData);
                lock (UpdateAvailableViewModel.UpdateLocker)
                {
                    if (!UpdateAvailable.IsShown)
                    {
                        var currentVersion = AppVersion.GetInstalledVersion();
                        if (currentVersion.Rank > version.Rank)
                        {
                            logger.Debug("app already updated");
                            return;
                        }
                        UpdateAvailable win = new UpdateAvailable();
                        win.Owner = Application.Current.MainWindow;
                        dc.LaterButtonVisibility = true;
                        dc.ShowIcon = true;
                        dc.HeaderText = Application.Current.Resources["InstallationIsOutdated"].ToString();
                        dc.BoldBodyText = Application.Current.Resources["InstallationIsOutdated_UpdateNow"].ToString();
                        win.DataContext = dc;
                        dc.UpdateNowTriggered += (o, e) => System.Diagnostics.Process.Start(binary);
                        win.ShowDialog();
                    }
                }
            }));
            ret.Status = ActionStatus.Completed;

            }
            return ret;
        }

        private string GetArgumentsFromJsonFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            var updaterParams = UpdaterData.ReadFromFile(path);
            return updaterParams.GetPasswordBossUpdater().silent;
        }

        private string FindUpdateBinary(UpdaterBinary binaryJsonData)
        {
            if (!Directory.Exists(UpdatePathHelper.PbUpdateFolderPath))
            {
                return "";
            }

            var name = Path.GetFileName(binaryJsonData.downloadLink.ToString());
            var res = Directory.EnumerateFiles(UpdatePathHelper.PbUpdateFolderPath)
                .FirstOrDefault(f => f.EndsWith(name));

            res = res ?? "";
            return res;
        }

        private string FindUpdateJsonFile()
        {
            if (!Directory.Exists(UpdatePathHelper.PbUpdateFolderPath))
            {
                return "";
            }

            var res = Directory.EnumerateFiles(UpdatePathHelper.PbUpdateFolderPath)
                .FirstOrDefault(f => f.EndsWith("json"));

            res = res ?? "";
            return res;
        }
    }
}
