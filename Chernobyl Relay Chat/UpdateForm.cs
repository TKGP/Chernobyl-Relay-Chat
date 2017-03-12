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
            if (gamedataRequired)
                label2.Text = "This update requires reinstallation of gamedata after downloading.";
            else
                label2.Text = "";
            webBrowser1.DocumentText = lineBreakRx.Replace(releaseNotes, new MatchEvaluator(addBr));
        }

        private static string addBr(Match match)
        {
            return "<br/>\n";
        }
    }
}
