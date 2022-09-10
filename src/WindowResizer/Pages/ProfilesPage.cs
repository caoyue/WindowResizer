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

            EventsHandle();
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

                ConfigFactory.ProfileAdd(result.Trim());
            }
        }

        private void ProfileSwitch_OnClick(string profileId)
        {
            if (profileId.Equals(ConfigFactory.Current.ProfileId))
            {
                return;
            }

            ConfigFactory.ProfileSwitch(profileId);
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

                ConfigFactory.ProfileRename(profileId, result);
            }
        }

        private void ProfileRemove_OnClick(string profileId)
        {
            if (!ConfigFactory.ProfileRemove(profileId))
            {
                Helper.ShowMessageBox("Profile can not be removed.");
            }
        }

        #region Events

        private void EventsHandle()
        {
            ConfigFactory.Profiles.ProfileEvents.ProfileAdd += OnProfileAdd;
            ConfigFactory.Profiles.ProfileEvents.ProfileSwitch += id => OnProfileSwitch(id);
            ConfigFactory.Profiles.ProfileEvents.ProfileRename += OnProfileRename;
            ConfigFactory.Profiles.ProfileEvents.ProfileRemove += OnProfileRemove;
        }

        private void OnProfileAdd(string profileId, string profileName)
        {
            RenderProfile(profileId, profileName);
            OnRowAdded();
        }

        private void OnProfileSwitch(string profileId, string message = null)
        {
            ResetButtonState();
            SetWindowTitle();
            message = message ?? $"Profile switched to <{ConfigFactory.Current.ProfileName}>.";
            ReloadConfig(message);
        }

        private void OnProfileRename(string profileId, string profileName)
        {
            var label = ProfilesLayout.Controls[$"ProfileLabel-{profileId}"];
            label.Text = profileName;

            if (profileId.Equals(ConfigFactory.Current.ProfileId))
            {
                SetWindowTitle();
            }
        }

        private void OnProfileRemove(string profileId)
        {
            ProfilesLayout.Controls.RemoveByKey($"ProfileLabel-{profileId}");
            ProfilesLayout.Controls.RemoveByKey($"ProfileSwitchBtn-{profileId}");
            ProfilesLayout.Controls.RemoveByKey($"ProfileRenameBtn-{profileId}");
            ProfilesLayout.Controls.RemoveByKey($"ProfileRemoveBtn-{profileId}");

            OnRowRemoved();
        }

        #endregion

        #region layouts

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
            var profiles = ConfigFactory.Profiles.Configs;

            ProfilesLayout.RowCount = profiles.Count;

            foreach (var p in profiles)
            {
                RenderProfile(p.ProfileId, p.ProfileName);
            }
        }

        private void RenderProfile(string profileId, string profileName)
        {
            var isCurrent = ConfigFactory.Current.ProfileId.Equals(profileId, StringComparison.Ordinal);

            var label = new Label();
            label.AutoSize = false;
            label.ForeColor = SystemColors.ControlText;
            label.Name = $"ProfileLabel-{profileId}";
            label.Text = profileName;
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
            switchBtn.Name = $"ProfileSwitchBtn-{profileId}";
            switchBtn.Size = btnSize;
            switchBtn.Size = ConfigImportBtn.Size;
            switchBtn.Text = "Switch";
            switchBtn.UseVisualStyleBackColor = false;
            switchBtn.Enabled = !isCurrent;
            switchBtn.Anchor = AnchorStyles.None;
            switchBtn.Click += (s, e) => ProfileSwitch_OnClick(profileId);
            ProfilesLayout.Controls.Add(switchBtn);

            var renameBtn = new Button();
            renameBtn.BackColor = SystemColors.ButtonFace;
            renameBtn.Name = $"ProfileRenameBtn-{profileId}";
            renameBtn.Size = btnSize;
            renameBtn.Text = "Rename";
            renameBtn.UseVisualStyleBackColor = false;
            renameBtn.Anchor = AnchorStyles.None;
            renameBtn.Click += (s, e) => ProfileRename_OnClick(profileId);
            ProfilesLayout.Controls.Add(renameBtn);

            var removeBtn = new Button();
            removeBtn.BackColor = SystemColors.ButtonFace;
            removeBtn.Name = $"ProfileRemoveBtn-{profileId}";
            removeBtn.Size = btnSize;
            removeBtn.Text = "Remove";
            removeBtn.UseVisualStyleBackColor = false;
            removeBtn.Enabled = !isCurrent && ConfigFactory.Profiles.Configs.Count > 1;
            removeBtn.Click += (s, e) => ProfileRemove_OnClick(profileId);
            removeBtn.Anchor = AnchorStyles.None;
            ProfilesLayout.Controls.Add(removeBtn);

            OnRowAdded();
        }

        #endregion

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

            var rowCount = ConfigFactory.Profiles.Configs.Count;
            var scroll = rowCount > _profileMaxHeight / _profileRowHeight;
            ProfilesLayout.AutoSize = !scroll;
            ProfilesLayout.AutoScroll = scroll;

            ProfilesLayout.Height = scroll ? _profileMaxHeight : rowCount * _profileRowHeight;
        }

        private void ClearProfiles()
        {
            ProfilesLayout.Controls.Clear();
        }

        private void ResetButtonState()
        {
            foreach (var c in ConfigFactory.Profiles.Configs)
            {
                var isCurrent = c.ProfileId.Equals(ConfigFactory.Current.ProfileId);

                var label = ProfilesLayout.Controls[$"ProfileLabel-{c.ProfileId}"];
                label.Font = Helper.ChangeFontSize(this.Font, 12F, isCurrent ? FontStyle.Bold : FontStyle.Regular);
                label.ForeColor = isCurrent ? SystemColors.Highlight : SystemColors.ControlText;

                var switchBtn = ProfilesLayout.Controls[$"ProfileSwitchBtn-{c.ProfileId}"];
                switchBtn.Enabled = !isCurrent;

                var removeBtn = ProfilesLayout.Controls[$"ProfileRemoveBtn-{c.ProfileId}"];
                removeBtn.Enabled = !isCurrent && ConfigFactory.Profiles.Configs.Count > 1;
            }
        }
    }
}
