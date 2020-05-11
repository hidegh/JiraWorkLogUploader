using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using JiraWorkLogUploader.Config;
using JiraWorkLogUploader.Excel;
using JiraWorkLogUploader.Export;
using JiraWorkLogUploader.Ui;

namespace JiraWorkLogUploader
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/aa302326.aspx
    /// </summary>
    public partial class MainForm : Form
    {
        private AppSettings Settings;

        public MainForm()
        {
            InitializeComponent();

            this.propertyGrid.PropertySort = PropertySort.NoSort;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            propertyGrid.PropertyValueChanged += (o, args) =>
            {
                var type = args.ChangedItem.GetType();
                var helpKeywordProperty = type.GetProperty("HelpKeyword",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                var helpKeyword = helpKeywordProperty.GetValue(args.ChangedItem, null) as string;
                var name = args.ChangedItem.PropertyDescriptor.Name;

                if (helpKeyword == "JiraUploader.Config.AppSettings.ExcelFile")
                {
                    var excelFile = args.ChangedItem.Value as string;

                    using (var p = new ProgressScope())
                    {
                        p.SetText("Parsing excel for sheets...");
                        InitSheets(excelFile);
                    }
                }
            };

            LoadSettings();
        }

        //
        // Worklog upload...
        //

        private string GetUnhandledEntryDetails(bool includeToday)
        {
            using (var p = new ProgressScope())
            {
                var totalCount = 0;
                var totalItems = new int[Settings.Exports.Length];
                var firstDate = null as DateTime?;
                var lastDate = null as DateTime?;

                p.SetText("Searching for entries to upload...");

                new ExcelParser(Settings.ExcelFile)
                    .Process(Settings.SheetName, Settings.Exports, entry =>
                    {
                        var jiraIndex = Array.IndexOf(Settings.Exports, entry.ExportSettings);

                        // skip future dates
                        if (entry.Date.Date > DateTime.Now.Date)
                            return;

                        // skip today items if not 'includeToday'
                        if (entry.Date.Date == DateTime.Now.Date && includeToday == false)
                            return;

                        totalCount++;
                        totalItems[jiraIndex]++;
                       
                        p.SetText(string.Format("Searching for entries to upload | Found: {0}", totalCount));

                        if (firstDate == null || firstDate.Value > entry.Date)
                            firstDate = entry.Date;

                        if (lastDate == null || lastDate.Value < entry.Date)
                            lastDate = entry.Date;
                    });

                var sb = new StringBuilder();
                sb.AppendFormat("Excel parsing result:\r\n");
                sb.AppendFormat("\r\n");
                sb.AppendFormat("Total items:\r\n");
                for (int i = 0; i < totalItems.Length; i++)
                {
                    sb.AppendFormat(" - Group #{0,2:N0} [{1}] Count: {2,2:N0}, Name: {3}\r\n", i,
                        Settings.Exports[i].Column, totalItems[i], Settings.Exports[i].Display);
                }
                sb.AppendFormat("\r\n");
                sb.AppendFormat("Date range from: {0:yyyy-MM-dd} to {1:yyyy-MM-dd}\r\n", firstDate, lastDate);

                return sb.ToString();
            }
        }

        private bool UploadUnhandledEntries(bool includeToday)
        {
            using (var p = new ProgressScope())
            {
                p.SetText("Initializing entry upload...");

                var success = false;
                var needToSave = false;

                // create processor (loads excel)
                var processor = new ExcelParser(Settings.ExcelFile);

                /*
                // logins
                p.SetText("Logging in to jira(s)...");
                foreach (var jira in Settings.Exports)
                    JiraApiHelper.Login(jira);
                */

                var uploaded = 0;
                var uploadFailed = 0;

                try
                {
                    // parse and upload
                    p.SetText("Uploading entries...");

                    processor.Process(Settings.SheetName, Settings.Exports, entry =>
                    {
                        // skip future dates
                        if (entry.Date.Date > DateTime.Now.Date)
                            return;

                        // skip today items if not 'includeToday'
                        if (entry.Date.Date == DateTime.Now.Date && includeToday == false)
                            return;

                        // try to upload
                        var code = JiraApiHelper.LogWork(entry.ExportSettings, entry.Date, entry.Hours, entry.Issue, entry.Comment);
                        entry.SetUploadResult(code);

                        var isSuccessCode = code >= 200 && code < 300;

                        if (isSuccessCode)
                            uploaded++;
                        else uploadFailed++;

                        p.SetText(string.Format("Uploading entries | Uploaded: {0}, Failed: {1}", uploaded, uploadFailed));

                        // mark changed
                        needToSave = true;
                    });

                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                    MessageBox.Show(ex.Message, "Exception occured while processing excel file!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    // if any (even partial) changes were taken inside excel - e.g. some upload was done (even if not successful) - then it has to be saved
                    if (needToSave)
                    {
                        // backup
                        var backupSuccess = RetryWithQuestion("Creating backup of original file: " + Settings.ExcelFile,
                            () =>
                            {
                                var backupExcelFile = Path.ChangeExtension(Settings.ExcelFile,
                                    "." + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".xlsx");
                                File.Copy(Settings.ExcelFile, backupExcelFile);
                            }
                        );

                        if (!backupSuccess)
                            MessageBox.Show("Could not create a backup of the original excel file!", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        // save
                        var saveSuccess = RetryWithQuestion("Creating backup of original file: " + Settings.ExcelFile,
                            () => processor.Save(Settings.ExcelFile)
                        );

                        if (!saveSuccess)
                            MessageBox.Show(
                                "Could not save changes to excel! Please check tasks on JIRA and modify excel accordingly to avoid duplicity that next upload could generate.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        // no change, no need for save, let user know...
                    }
                }

                return success;
            }
        }

        private bool RetryWithQuestion(string actionDescription, Action action)
        {
            var retry = false;

            do
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception ex)
                {
                    var caption = string.IsNullOrWhiteSpace(actionDescription)
                        ? "Retry"
                        : "Retry action: " + actionDescription;

                    var message = "Exception occured while executing action. Message: " + ex.Message;

                    var dialogResult = MessageBox.Show(
                        message,
                        caption,
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Exclamation
                        );

                    retry = dialogResult == DialogResult.Retry;
                }

            } while (retry);

            return false;
        }

        private void buttonUploadWorklog_Click(object sender, EventArgs e)
        {
            var includeToday = checkBoxIncludeToday.Checked;

            var fetchDetails = GetUnhandledEntryDetails(includeToday);

            var dialogResult = MessageBox.Show(fetchDetails, "Do you want to upload excel sheet entries to JIRA?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
                return;

            var success = UploadUnhandledEntries(includeToday);
            if (success)
            {
                SaveSettings();
                MessageBox.Show("Everything uploaded!", "Jira upload result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //
        // Save
        //

        private void SaveSettings()
        {
            using (var p = new ProgressScope())
            {
                p.SetText("Saving JSON");
                Settings.SaveTo("App.json");
            }
        }

        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        //
        // Load
        //

        private void InitSheets(string excelFile)
        {
            var sheets = NpoiHelper.GetSheetNames(excelFile);

            var sheetCollection = sheets.ToList();
            Settings.ItemsFor_SheetName = sheetCollection;

            if (!sheetCollection.Any(i => i == Settings.SheetName))
                Settings.SheetName = sheetCollection.LastOrDefault();
        }

        private void LoadSettings()
        {
            using (var p = new ProgressScope())
            {
                p.SetText("Loading JSON");
                Settings = AppSettings.From("App.json");

                propertyGrid.SelectedObject = Settings;
                propertyGrid.ExpandAllGridItems();

                p.SetText("Parsing excel for sheets...");
                InitSheets(Settings.ExcelFile);
            }
        }

        private void buttonSettingsReload_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void buttonDeleteWorklogs_Click(object sender, EventArgs e)
        {
            var dialog = new Ui.Jira.DeleteWorklogs();
            var result = dialog.ShowDialog();
        }
    }
}
