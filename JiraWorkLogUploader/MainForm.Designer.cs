﻿namespace JiraWorkLogUploader
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonUploadWorklog = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonSettingsSave = new System.Windows.Forms.Button();
            this.buttonSettingsReload = new System.Windows.Forms.Button();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStripProgress = new System.Windows.Forms.StatusStrip();
            this.buttonDeleteWorklogs = new System.Windows.Forms.Button();
            this.statusStripProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonUploadWorklog
            // 
            this.buttonUploadWorklog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadWorklog.Location = new System.Drawing.Point(16, 484);
            this.buttonUploadWorklog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonUploadWorklog.Name = "buttonUploadWorklog";
            this.buttonUploadWorklog.Size = new System.Drawing.Size(100, 28);
            this.buttonUploadWorklog.TabIndex = 1;
            this.buttonUploadWorklog.Text = "Upload worklog";
            this.buttonUploadWorklog.UseVisualStyleBackColor = true;
            this.buttonUploadWorklog.Click += new System.EventHandler(this.buttonUploadWorklog_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(16, 15);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(587, 462);
            this.propertyGrid.TabIndex = 2;
            // 
            // buttonSettingsSave
            // 
            this.buttonSettingsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSettingsSave.Location = new System.Drawing.Point(124, 484);
            this.buttonSettingsSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSettingsSave.Name = "buttonSettingsSave";
            this.buttonSettingsSave.Size = new System.Drawing.Size(100, 28);
            this.buttonSettingsSave.TabIndex = 3;
            this.buttonSettingsSave.Text = "Save settings";
            this.buttonSettingsSave.UseVisualStyleBackColor = true;
            this.buttonSettingsSave.Click += new System.EventHandler(this.buttonSettingsSave_Click);
            // 
            // buttonSettingsReload
            // 
            this.buttonSettingsReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSettingsReload.Location = new System.Drawing.Point(232, 484);
            this.buttonSettingsReload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSettingsReload.Name = "buttonSettingsReload";
            this.buttonSettingsReload.Size = new System.Drawing.Size(100, 28);
            this.buttonSettingsReload.TabIndex = 4;
            this.buttonSettingsReload.Text = "Reload settings";
            this.buttonSettingsReload.UseVisualStyleBackColor = true;
            this.buttonSettingsReload.Click += new System.EventHandler(this.buttonSettingsReload_Click);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(18, 20);
            this.toolStripStatusLabel.Text = "...";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.AutoSize = false;
            this.toolStripProgressBar.Margin = new System.Windows.Forms.Padding(12, 3, 1, 3);
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(133, 17);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // statusStripProgress
            // 
            this.statusStripProgress.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStripProgress.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStripProgress.Location = new System.Drawing.Point(0, 518);
            this.statusStripProgress.Name = "statusStripProgress";
            this.statusStripProgress.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStripProgress.Size = new System.Drawing.Size(619, 25);
            this.statusStripProgress.SizingGrip = false;
            this.statusStripProgress.TabIndex = 5;
            // 
            // buttonDeleteWorklogs
            // 
            this.buttonDeleteWorklogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteWorklogs.Location = new System.Drawing.Point(503, 484);
            this.buttonDeleteWorklogs.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDeleteWorklogs.Name = "buttonDeleteWorklogs";
            this.buttonDeleteWorklogs.Size = new System.Drawing.Size(100, 28);
            this.buttonDeleteWorklogs.TabIndex = 6;
            this.buttonDeleteWorklogs.Text = "Delete worklogs";
            this.buttonDeleteWorklogs.UseVisualStyleBackColor = true;
            this.buttonDeleteWorklogs.Click += new System.EventHandler(this.buttonDeleteWorklogs_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 543);
            this.Controls.Add(this.buttonDeleteWorklogs);
            this.Controls.Add(this.statusStripProgress);
            this.Controls.Add(this.buttonSettingsReload);
            this.Controls.Add(this.buttonSettingsSave);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonUploadWorklog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "JIRA uploader - main form...";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStripProgress.ResumeLayout(false);
            this.statusStripProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonUploadWorklog;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonSettingsSave;
        private System.Windows.Forms.Button buttonSettingsReload;
        private System.Windows.Forms.StatusStrip statusStripProgress;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        internal System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Button buttonDeleteWorklogs;
    }
}

