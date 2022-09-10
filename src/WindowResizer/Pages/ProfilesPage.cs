using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Controls;
using WindowResizer.Utils;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private int _profileRowHeight;
        private int _profileMaxHeight;

        private void ProfilesPageInit()
        {
            NewProfile.Click += NewProfileBtn_Click;

            _profileRowHeight = ProfilesLayout.GetRowHeights().FirstOrDefault();
            _profileMaxHeight = ProfileGroupBox.Height - 50;

            InitProfilesLayout();
            ReRenderProfiles();
        }

        public void OnProfileSwitch(string message = null)
        {
            SetWindowTitle();
            message = message ?? $"Profile switched to <{ConfigLoader.Current.ProfileName}>.";
            ReloadConfig(message);

            ResetButtonState();
        }

        private void InitProfilesLayout()
        {
            ProfilesLayout.RowStyles.Clear();
            ProfilesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, _profileRowHeight));

            ProfilesLayout.ColumnStyles.Clear();
            ProfilesLayout.ColumnCount = 4;
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            ProfilesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            ProfilesLayout.AutoSize = true;
            ProfilesLayout.AutoScroll = false;
            ProfilesLayout.Height = _profileRowHeight;
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
            label.Text = profile.ProfileName;
            label.Size = SaveKeyLabel.Size;
            label.Font = Helper.ChangeFontSize(this.Font, 12F);
            label.TextAlign = ContentAlignment.MiddleLeft;

            if (isCurrent)
            {
                label.ForeColor = SystemColors.Highlight;
                label.Font = Helper.ChangeFontSize(this.Font, 12F, FontStyle.Bold);
            }

            label.Anchor = AnchorStyles.Left;
            ProfilesLayout.Controls.Add(label);

            var btnSize = ConfigImportBtn.Size;
            var switchBtn = new Button();
            switchBtn.BackColor = SystemColors.ButtonFace;
            switchBtn.Name = $"ProfileSwitchBtn-{profile.ProfileId}";
            switchBtn.Size = btnSize;
            switchBtn.Size = ConfigImportBtn.Size;
            switchBtn.Text = "Switch";
            switchBtn.UseVisualStyleBackColor = false;
            switchBtn.Enabled = !isCurrent;
            switchBtn.Anchor = AnchorStyles.None;
            switchBtn.Click += (s, e) => ProfileSwitch_OnClick(profile.ProfileId);
            ProfilesLayout.Controls.Add(switchBtn);

            var renameBtn = new Button();
            renameBtn.BackColor = SystemColors.ButtonFace;
            renameBtn.Name = $"ProfileRenameBtn-{profile.ProfileId}";
            renameBtn.Size = btnSize;
            renameBtn.Text = "Rename";
            renameBtn.UseVisualStyleBackColor = false;
            renameBtn.Anchor = AnchorStyles.None;
            renameBtn.Click += (s, e) => ProfileRename_OnClick(profile.ProfileId);
            ProfilesLayout.Controls.Add(renameBtn);

            var removeBtn = new Button();
            removeBtn.BackColor = SystemColors.ButtonFace;
            removeBtn.Name = $"ProfileRemoveBtn-{profile.ProfileId}";
            removeBtn.Size = btnSize;
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
            ProfilesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, _profileRowHeight));
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
            var scroll = rowCount > _profileMaxHeight / _profileRowHeight;
            ProfilesLayout.AutoSize = !scroll;
            ProfilesLayout.AutoScroll = scroll;

            ProfilesLayout.Height = scroll ? _profileMaxHeight : rowCount * _profileRowHeight;
        }

        private void ClearProfiles()
        {
            ProfilesLayout.Controls.Clear();
        }

        private void NewProfileBtn_Click(object sender, EventArgs e)
        {
            using (Prompt prompt = new Prompt("Enter new profile name:", "New Profile"))
            {
                string result = prompt.Dialog();
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
                OnProfileSwitch();
            }
        }

        private void ProfileRename_OnClick(string profileId)
        {
            using (Prompt prompt = new Prompt("Enter new profile name:", "Rename profile"))
            {
                string result = prompt.Dialog();
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

        private void ResetButtonState()
        {
            foreach (var c in ConfigLoader.Profiles.Configs)
            {
                var isCurrent = c.ProfileId.Equals(ConfigLoader.Current.ProfileId);

                var label = ProfilesLayout.Controls[$"ProfileLabel-{c.ProfileId}"];
                label.Font = Helper.ChangeFontSize(this.Font, 12F, isCurrent ? FontStyle.Bold : FontStyle.Regular);
                label.ForeColor = isCurrent ? SystemColors.Highlight : SystemColors.ControlText;

                var switchBtn = ProfilesLayout.Controls[$"ProfileRemoveBtn-{c.ProfileId}"];
                switchBtn.Enabled = !isCurrent;

                var removeBtn = ProfilesLayout.Controls[$"ProfileRemoveBtn-{c.ProfileId}"];
                removeBtn.Enabled = !isCurrent && ConfigLoader.Profiles.Configs.Count > 1;
            }
        }
    }
}
