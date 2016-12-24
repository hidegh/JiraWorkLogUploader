using System;
using System.Windows.Forms;

namespace JiraWorkLogUploader.Ui
{
    /// <summary>
    /// We don't use background thread...
    /// For work with backround threads check out this: https://msdn.microsoft.com/en-us/library/ms741870%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
    /// </summary>
    public class ProgressScope : IDisposable
    {
        private static object locker = new object();
        private static int scopeLevel = 0;

        private MainForm form;

        public ProgressScope()
        {
            this.form = Program.MainForm;

            lock (locker)
            {
                scopeLevel++;
            }

            form.toolStripProgressBar.Value = form.toolStripProgressBar.Minimum;
            form.toolStripProgressBar.Style = ProgressBarStyle.Marquee;
        }

        public void SetText(string text)
        {
            form.toolStripStatusLabel.Text = text;
            Application.DoEvents();
        }

        public void Dispose()
        {
            lock (locker)
            {
                scopeLevel--;

                if (scopeLevel == 0)
                {
                    form.toolStripProgressBar.Style = ProgressBarStyle.Continuous;
                    form.toolStripProgressBar.Value = form.toolStripProgressBar.Maximum;

                    form.toolStripStatusLabel.Text = "...";
                }
            }
        }
    }
}
