using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using JiraWorkLogUploader.Config;
using JiraWorkLogUploader.Ui;

namespace JiraWorkLogUploader
{
    public partial class DeleteWorklogs : Form
    {
        private AppSettings Settings;

        public DeleteWorklogs()
        {
            InitializeComponent();
        }

        private void DeleteWorklogs_Load(object sender, EventArgs e)
        {
            // Load settings
            using (var p = new ProgressScope())
            {
                p.SetText("Loading JSON");
                Settings = AppSettings.From("App.json");
            }

            // Populate DDL, select first if exists
            comboBoxJiraServer.DataSource = Settings.Jiras;
            comboBoxJiraServer.SelectedIndex = Settings.Jiras.Length > 0 ? 0 : -1;

            // Set dates
            dateTimePickerFrom.Value = DateTime.Now.AddDays(1).Date;
            dateTimePickerTo.Value = new DateTime(9000, 1, 1);
        }

        private void buttonLoadWorklogs_Click(object sender, EventArgs e)
        {
            using (var p = new ProgressScope())
            {
                // clear
                listViewWorklogs.Items.Clear();

                // get selected jira setting
                var jira = Settings.Jiras.FirstOrDefault(i => i.Url == ((JiraSetting)comboBoxJiraServer.SelectedItem)?.Url);
                if (jira == null)
                    return;

                // log in
                p.SetText("Logging in to selected JIRA server...");
                Jira.JiraApiHelper.Login(jira);

                // get data
                p.SetText("Fetching worklogs...");
                var data = Jira.JiraApiHelper.GetWorklogsModifiedOn(
                    jira,
                    dateTimePickerFrom.Value,
                    dateTimePickerTo.Value
                    );

                // Fill with new data
                p.SetText("Initiating UI...");
                foreach (var d in data)
                {
                    var lvItem = new ListViewItem();
                    lvItem.Text = d.Self;
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = d.Issue });
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = d.Started.ToString("yyyy MMMM dd ddd") });
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = TimeSpan.FromSeconds(d.TimeSpentSeconds).TotalHours.ToString("00.00") });
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = d.Author });
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = d.Comment });
                    listViewWorklogs.Items.Add(lvItem);
                }

                // auto resize (exept 1. col.)
                listViewWorklogs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listViewWorklogs.Columns[0].Width = 60;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            // get items to delete
            var linkToRemovableWorklogs = new List<string>();

            foreach (ListViewItem item in listViewWorklogs.Items)
            {
                if (item.Checked)
                    linkToRemovableWorklogs.Add(item.Text);
            }

            // skip if no item selected
            if (linkToRemovableWorklogs.Count == 0)
                return;

            // ask first
            var deleteMsg = $"Items selected for deletion: {linkToRemovableWorklogs.Count}";

            var dialogResult = MessageBox.Show(deleteMsg, "Do you want to delete selected worklogs?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
                return;

            using (var p = new ProgressScope())
            {
                // log in to jiras
                p.SetText("Logging in to JIRA servers...");

                Settings
                    .Jiras
                    .ToList()
                    .ForEach(jira => {
                        Jira.JiraApiHelper.Login(jira);
                    });

                // delete
                var deleted = 0;
                var deleteFailed = 0;

                linkToRemovableWorklogs.ForEach(worklogUrl =>
                {
                    var code = Jira.JiraApiHelper.DeleteWorklog(worklogUrl);

                    var isSuccessCode = code >= 200 && code < 300;

                    if (isSuccessCode)
                        deleted++;
                    else
                        deleteFailed++;

                    p.SetText(string.Format("Deleting entries | Deleted: {0}, Failed: {1}", deleted, deleteFailed));
                });

                // remove deleted
                var items = listViewWorklogs.Items.OfType<ListViewItem>().ToList();
                var filteredItems = items.Where(i => !linkToRemovableWorklogs.Contains(i.Text)).ToList();

                listViewWorklogs.Items.Clear();
                listViewWorklogs.Items.AddRange(filteredItems.ToArray());
            }

        }
    }
}
