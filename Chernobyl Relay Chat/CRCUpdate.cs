using GitHubUpdate;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCUpdate
    {
        public static bool CheckFirstUpdate()
        {
            UpdateChecker updateChecker = new UpdateChecker("TKGP", "Chernobyl-Relay-Chat");
            UpdateType updateType = updateChecker.CheckUpdate().Result;
            // Note to self: prereleases are ignored
            if(updateType != UpdateType.None)
            {
                DialogResult dialogResult = MessageBox.Show("A mandatory update for CRC is available.\r\n"
                    + "Click Yes to be taken to the download page, or No if you wish to navigate there manually."
                    + ((updateType == UpdateType.Major || updateType == UpdateType.Minor) ? "After downloading, this update requires re-installing the gamedata folder." : ""),
                    "Chernobyl Relay Chat", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                    updateChecker.DownloadAsset("Chernobyl-Relay-Chat.zip");
                return true;
            }
            return false;
        }

        public static async Task<bool> CheckUpdate(ClientDisplay display, CRCClient client)
        {
            UpdateChecker updateChecker = new UpdateChecker("TKGP", "Chernobyl-Relay-Chat");
            UpdateType updateType = await updateChecker.CheckUpdate();
            // Note to self: prereleases are ignored
            if (updateType != UpdateType.None)
            {
                client.OnUpdate("A mandatory update for CRC is available. Please check the external client to download it.");
                DialogResult dialogResult = MessageBox.Show("A mandatory update for CRC is available.\r\n"
                    + "Click Yes to be taken to the download page, or No if you wish to navigate there manually."
                    + ((updateType == UpdateType.Major || updateType == UpdateType.Minor) ? "After downloading, this update requires re-installing the gamedata folder." : ""),
                    "Chernobyl Relay Chat", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                    updateChecker.DownloadAsset("Chernobyl-Relay-Chat.zip");
                display.Close();
            }
            return true;
        }
    }
}
