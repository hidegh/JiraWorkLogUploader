namespace JiraWorkLogUploader
{
    partial class DeleteWorklogs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "A",
            "B",
            "C"}, -1);
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.labelJiraServer = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.buttonLoadWorklogs = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelTo = new System.Windows.Forms.Label();
            this.comboBoxJiraServer = new System.Windows.Forms.ComboBox();
            this.jiraSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listViewWorklogs = new System.Windows.Forms.ListView();
            this.columnHeaderSelector = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIssue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderHours = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.jiraSettingBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(56, 68);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(320, 22);
            this.dateTimePickerTo.TabIndex = 0;
            // 
            // labelJiraServer
            // 
            this.labelJiraServer.AutoSize = true;
            this.labelJiraServer.Location = new System.Drawing.Point(4, 13);
            this.labelJiraServer.Name = "labelJiraServer";
            this.labelJiraServer.Size = new System.Drawing.Size(31, 17);
            this.labelJiraServer.TabIndex = 1;
            this.labelJiraServer.Text = "Jira";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(4, 45);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(40, 17);
            this.labelFrom.TabIndex = 1;
            this.labelFrom.Text = "From";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(56, 40);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(320, 22);
            this.dateTimePickerFrom.TabIndex = 0;
            // 
            // buttonLoadWorklogs
            // 
            this.buttonLoadWorklogs.Location = new System.Drawing.Point(382, 10);
            this.buttonLoadWorklogs.Name = "buttonLoadWorklogs";
            this.buttonLoadWorklogs.Size = new System.Drawing.Size(100, 81);
            this.buttonLoadWorklogs.TabIndex = 2;
            this.buttonLoadWorklogs.Text = "Load worklogs";
            this.buttonLoadWorklogs.UseVisualStyleBackColor = true;
            this.buttonLoadWorklogs.Click += new System.EventHandler(this.buttonLoadWorklogs_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(746, 11);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 80);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete selected";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(4, 73);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(25, 17);
            this.labelTo.TabIndex = 1;
            this.labelTo.Text = "To";
            // 
            // comboBoxJiraServer
            // 
            this.comboBoxJiraServer.DataSource = this.jiraSettingBindingSource;
            this.comboBoxJiraServer.DisplayMember = "Display";
            this.comboBoxJiraServer.FormattingEnabled = true;
            this.comboBoxJiraServer.Location = new System.Drawing.Point(56, 10);
            this.comboBoxJiraServer.Name = "comboBoxJiraServer";
            this.comboBoxJiraServer.Size = new System.Drawing.Size(320, 24);
            this.comboBoxJiraServer.TabIndex = 3;
            this.comboBoxJiraServer.ValueMember = "Url";
            // 
            // jiraSettingBindingSource
            // 
            this.jiraSettingBindingSource.DataSource = typeof(JiraWorkLogUploader.Config.JiraSetting);
            // 
            // listViewWorklogs
            // 
            this.listViewWorklogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewWorklogs.CheckBoxes = true;
            this.listViewWorklogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSelector,
            this.columnHeaderDate,
            this.columnHeaderIssue,
            this.columnHeaderHours,
            this.columnHeaderAuthor,
            this.columnHeaderComment});
            listViewItem1.StateImageIndex = 0;
            this.listViewWorklogs.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewWorklogs.Location = new System.Drawing.Point(7, 97);
            this.listViewWorklogs.Name = "listViewWorklogs";
            this.listViewWorklogs.Size = new System.Drawing.Size(839, 418);
            this.listViewWorklogs.TabIndex = 4;
            this.listViewWorklogs.UseCompatibleStateImageBehavior = false;
            this.listViewWorklogs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderSelector
            // 
            this.columnHeaderSelector.Text = "Self";
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "Date";
            // 
            // columnHeaderIssue
            // 
            this.columnHeaderIssue.Text = "Issue";
            // 
            // columnHeaderHours
            // 
            this.columnHeaderHours.Text = "Hours";
            // 
            // columnHeaderAuthor
            // 
            this.columnHeaderAuthor.Text = "Author";
            // 
            // columnHeaderComment
            // 
            this.columnHeaderComment.Text = "Comment";
            this.columnHeaderComment.Width = 31;
            // 
            // DeleteWorklogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 527);
            this.Controls.Add(this.listViewWorklogs);
            this.Controls.Add(this.comboBoxJiraServer);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonLoadWorklogs);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelJiraServer);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.dateTimePickerTo);
            this.Name = "DeleteWorklogs";
            this.Text = "DeleteWorklogs";
            this.Load += new System.EventHandler(this.DeleteWorklogs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jiraSettingBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label labelJiraServer;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Button buttonLoadWorklogs;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.ComboBox comboBoxJiraServer;
        private System.Windows.Forms.BindingSource jiraSettingBindingSource;
        private System.Windows.Forms.ListView listViewWorklogs;
        private System.Windows.Forms.ColumnHeader columnHeaderSelector;
        private System.Windows.Forms.ColumnHeader columnHeaderDate;
        private System.Windows.Forms.ColumnHeader columnHeaderIssue;
        private System.Windows.Forms.ColumnHeader columnHeaderHours;
        private System.Windows.Forms.ColumnHeader columnHeaderAuthor;
        private System.Windows.Forms.ColumnHeader columnHeaderComment;
    }
}