using System.Windows.Forms;

namespace JiraWorkLogUploader.Ui
{
    public class ExcelFileNameEditor : System.Windows.Forms.Design.FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Excel|*.xlsx";
        }
    }
}
