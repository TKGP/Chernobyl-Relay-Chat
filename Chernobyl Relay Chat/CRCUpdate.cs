using GitHubUpdate;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCUpdate
    {
        // Note to self: prereleases are ignored

        public static bool CheckFirstUpdate()
        {
            UpdateChecker updateChecker = new UpdateChecker("TKGP", "Chernobyl-Relay-Chat");
            UpdateType updateType = updateChecker.CheckUpdate().Result;
            if (updateType != UpdateType.None)
            {
                string releaseNotes = updateChecker.RenderReleaseNotes().Result;
                SystemSounds.Asterisk.Play();
                using (UpdateForm updateForm = new UpdateForm((updateType == UpdateType.Major || updateType == UpdateType.Minor), releaseNotes))
                {
                    DialogResult dialogResult = updateForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                        updateChecker.DownloadAsset("Chernobyl-Relay-Chat.zip");
                }
                return true;
            }
            return false;
        }

        public static async Task CheckUpdate(ClientDisplay display, CRCClient client)
        {
            UpdateChecker updateChecker = new UpdateChecker("TKGP", "Chernobyl-Relay-Chat");
            UpdateType updateType = await updateChecker.CheckUpdate();
            if (updateType != UpdateType.None)
            {
                string releaseNotes = await updateChecker.RenderReleaseNotes();
                SystemSounds.Asterisk.Play();
                client.OnUpdate("A mandatory update for CRC is available. Please check the external client to download it.");
                using (UpdateForm updateForm = new UpdateForm((updateType == UpdateType.Major || updateType == UpdateType.Minor), releaseNotes))
                {
                    DialogResult dialogResult = updateForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                        updateChecker.DownloadAsset("Chernobyl-Relay-Chat.zip");
                }
                display.Close();
            }
        }
    }
}
