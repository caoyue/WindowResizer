using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel;

namespace WindowResizer
{
    public class SquirrelUpdater
    {
        private readonly Action<string, int, int> _showTips;
        private readonly Func<string, bool> _confirmUpdate;

        public SquirrelUpdater(Func<string, bool> confirmUpdate, Action<string, int, int> showTips = null)
        {
            _confirmUpdate = confirmUpdate;
            _showTips = showTips;
        }

        public async Task Update()
        {
            const string githubRepo = @"https://github.com/caoyue/WindowResizer";
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager(githubRepo))
                {
                    var updateInfo = await mgr.CheckForUpdate();
                    if (!updateInfo.ReleasesToApply.Any())
                    {
                        return;
                    }

                    var latest = updateInfo.ReleasesToApply.First().Version;
                    var message = new StringBuilder().AppendLine($"New version {latest} found.")
                                                     .AppendLine("Would you like to download and install update?").ToString();

                    var confirm = _confirmUpdate(message);
                    if (!confirm)
                    {
                        return;
                    }

                    var updateResult = await mgr.UpdateApp();
                    if (updateResult != null)
                    {
                        _showTips?.Invoke($" Version {updateResult.Version} download complete. The app will restart to take effect.", 0, 2000);
                        UpdateManager.RestartApp();
                    }
                }
            }
            catch (Exception e)
            {
                _showTips?.Invoke($"Check update error, {e.Message}.", 2, 2000);
            }
        }
    }
}
