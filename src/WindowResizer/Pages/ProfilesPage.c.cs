using System;
using System.Drawing;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Controls;
using WindowResizer.Utils;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private const int ProfileMaxHeight = 480;
        private const int ProfileRowHeight = 60;

        private void ProfilesPageInit()
        {
            NewProfile.Click += NewProfileBtn_Click;

            InitProfilesLayout();
            ReRenderProfiles();
        }

        private void InitProfilesLayout()
        {
            ProfilesLayout.RowStyles.Clear();
            ProfilesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, ProfileRowHeight));

            ProfilesLayout.ColumnStyles.Clear();
            ProfilesLayout.ColumnCount = 4;
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            ProfilesLayout.AutoSize = true;
            ProfilesLayout.AutoScroll = false;
            ProfilesLayout.Height = ProfileRowHeight;
            ProfilesLayout.HorizontalScroll.Enabled = false;
        }

        private void ReRenderProfiles()
        {
            ClearProfiles();
            var profiles = ConfigLoader.Profiles.Configs;

            ProfilesLayout.RowCount = profiles.Count;

            foreach (var p in profiles)
            {
                RenderProfile(p);
            }
        }

        private void RenderProfile(Config profile)
        {
            var isCurrent = ConfigLoader.Current.ProfileId.Equals(profile.ProfileId, StringComparison.Ordinal);

            var label = new Label();
            label.AutoSize = false;
            label.ForeColor = SystemColors.ControlText;
            label.Name = $"ProfileLabel-{profile.ProfileId}";
            label.Size = new Size(150, 40);
            label.Text = profile.ProfileName;
            label.Font = Helper.ChangeFontSize(this.Font, 12F);
            label.TextAlign = ContentAlignment.MiddleLeft;

            if (isCurrent)
            {
                label.ForeColor = SystemColors.Highlight;
                label.Font = Helper.ChangeFontSize(this.Font, 12F, FontStyle.Bold);
            }

            label.Anchor = AnchorStyles.Left;
            ProfilesLayout.Controls.Add(label);

            var switchBtn = new Button();
            switchBtn.BackColor = SystemColors.ButtonFace;
            switchBtn.Name = $"ProfileSwitchBtn-{profile.ProfileId}";
            switchBtn.Size = new Size(100, 40);
            switchBtn.Text = "Switch";
            switchBtn.UseVisualStyleBackColor = false;
            switchBtn.Enabled = !isCurrent;
            switchBtn.Anchor = AnchorStyles.None;
            switchBtn.Click += (s, e) => ProfileSwitch_OnClick(profile.ProfileId);
            ProfilesLayout.Controls.Add(switchBtn);

            var renameBtn = new Button();
            renameBtn.BackColor = SystemColors.ButtonFace;
            renameBtn.Name = $"ProfileRenameBtn-{profile.ProfileId}";
            renameBtn.Size = new Size(100, 40);
            renameBtn.Text = "Rename";
            renameBtn.UseVisualStyleBackColor = false;
            renameBtn.Anchor = AnchorStyles.None;
            renameBtn.Click += (s, e) => ProfileRename_OnClick(profile.ProfileId);
            ProfilesLayout.Controls.Add(renameBtn);

            var removeBtn = new Button();
            removeBtn.BackColor = SystemColors.ButtonFace;
            removeBtn.Name = $"ProfileRemoveBtn-{profile.ProfileId}";
            removeBtn.Size = new Size(100, 40);
            removeBtn.Text = "Remove";
            removeBtn.UseVisualStyleBackColor = false;
            removeBtn.Enabled = !isCurrent && ConfigLoader.Profiles.Configs.Count > 1;
            removeBtn.Click += (s, e) => ProfileRemove_OnClick(profile.ProfileId);
            removeBtn.Anchor = AnchorStyles.None;
            ProfilesLayout.Controls.Add(removeBtn);

            OnRowAdded();
        }

        private void OnRowAdded()
        {
            ProfilesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, ProfileRowHeight));
            ProfileResetHeight();
        }

        private void OnRowRemoved()
        {
            var count = ProfilesLayout.RowStyles.Count;
            if (count > 0)
            {
                ProfilesLayout.RowStyles.RemoveAt(count - 1);
            }

            ProfileResetHeight();
        }

        private void ProfileResetHeight()
        {
            ProfilesLayout.AutoScroll = false;
            ProfilesLayout.HorizontalScroll.Enabled = false;

            var rowCount = ConfigLoader.Profiles.Configs.Count;
            var scroll = rowCount > ProfileMaxHeight / ProfileRowHeight;
            ProfilesLayout.AutoSize = !scroll;
            ProfilesLayout.AutoScroll = scroll;

            ProfilesLayout.Height = scroll ? ProfileMaxHeight : rowCount * ProfileRowHeight;
        }

        private void ClearProfiles()
        {
            ProfilesLayout.Controls.Clear();
        }

        private void NewProfileBtn_Click(object sender, EventArgs e)
        {
            using (Prompt prompt = new Prompt("Enter new profile name:", "New Profile", this.Font))
            {
                string result = prompt.Result;
                if (string.IsNullOrWhiteSpace(result))
                {
                    return;
                }

                var p = ConfigLoader.AddProfile(result.Trim());
                RenderProfile(p);
                OnRowAdded();
            }
        }

        private void ProfileSwitch_OnClick(string profileId)
        {
            if (profileId.Equals(ConfigLoader.Current.ProfileId))
            {
                return;
            }

            if (ConfigLoader.SwitchProfile(profileId))
            {
                ReRenderProfiles();
                OnProfileSwitch($"Profile switched to <{ConfigLoader.Current.ProfileName}>.");
            }
        }

        private void ProfileRename_OnClick(string profileId)
        {
            using (Prompt prompt = new Prompt("Enter new profile name:", "Rename profile", this.Font))
            {
                string result = prompt.Result.Trim();
                if (string.IsNullOrWhiteSpace(result))
                {
                    return;
                }

                if (ConfigLoader.RenameProfile(profileId, result))
                {
                    var label = ProfilesLayout.Controls[$"ProfileLabel-{profileId}"];
                    label.Text = result;

                    if (profileId.Equals(ConfigLoader.Current.ProfileId))
                    {
                        SetWindowTitle();
                    }
                }
            }
        }

        private void ProfileRemove_OnClick(string profileId)
        {
            if (ConfigLoader.RemoveProfile(profileId))
            {
                ProfilesLayout.Controls.RemoveByKey($"ProfileLabel-{profileId}");
                ProfilesLayout.Controls.RemoveByKey($"ProfileSwitchBtn-{profileId}");
                ProfilesLayout.Controls.RemoveByKey($"ProfileRenameBtn-{profileId}");
                ProfilesLayout.Controls.RemoveByKey($"ProfileRemoveBtn-{profileId}");

                OnRowRemoved();
                return;
            }

            Helper.ShowMessageBox("Profile can not be removed.");
        }

        private void OnProfileSwitch(string message)
        {
            SetWindowTitle();
            ReloadConfig(message);
        }
    }
}
