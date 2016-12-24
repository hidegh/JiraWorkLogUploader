using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace JiraWorkLogUploader
{
    static class Program
    {
        internal static MainForm MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            MainForm = new MainForm();
            Application.Run(MainForm);
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether
        // or not they wish to abort execution.
        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            var result = DialogResult.Cancel;

            try
            {
                result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Windows Forms Error", "Fatal Windows Forms Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
                Application.Exit();
        }

        // Creates the error message and displays it.
        private static DialogResult ShowThreadExceptionDialog(string title, Exception e)
        {
            string errorMsg = 
                "An application error occurred. Please contact the adminstrator with the following information:\n\n"
                + e.Message + "\n\n"
                + "Stack Trace:\n" + e.StackTrace;

            return MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether or not they wish to abort execution.
        // NOTE: This exception cannot be kept from terminating the application - it can only log the event, and inform the user about it. 
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "An application error occurred. Please contact the adminstrator with the following information:\n\n"
                    + ex.Message + "\n\n"
                    + "Stack Trace:\n" + ex.StackTrace;

                // Since we can't prevent the app from terminating, log this to the event log.
                if (!EventLog.SourceExists("ThreadException"))
                {
                    EventLog.CreateEventSource("ThreadException", "JiraUploader");
                }

                // Create an EventLog instance, set source and write to it...
                new EventLog() { Source = "ThreadExeption" }.WriteEntry(errorMsg);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal Non-UI Error", "Fatal Non-UI Error. Could not write the error to the event log. Reason: " + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }
    }
}
