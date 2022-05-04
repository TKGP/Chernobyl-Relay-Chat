using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class UpdateForm : Form
    {
        private Regex lineBreakRx = new Regex(@"(?<=\<p\>[^\<]+)\n");

        public UpdateForm(bool gamedataRequired, string releaseNotes)
        {
            InitializeComponent();
            labelDescription.Text = CRCStrings.Localize("update_description");
            if (gamedataRequired)
                labelGamedata.Text = CRCStrings.Localize("update_gamedata");
            else
                labelGamedata.Text = "";
            labelReleaseNotes.Text = CRCStrings.Localize("update_notes");
            buttonDownload.Text = CRCStrings.Localize("update_download");
            buttonClose.Text = CRCStrings.Localize("update_close");
            webBrowser1.DocumentText = lineBreakRx.Replace(releaseNotes, new MatchEvaluator(addBr));
        }

        private static string addBr(Match match)
        {
            return "<br/>\n";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void UpdateForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
