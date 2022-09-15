namespace WindowResizer
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.ProcessesGrid = new System.Windows.Forms.DataGridView();
            this.ConfigExportBtn = new System.Windows.Forms.Button();
            this.ConfigImportBtn = new System.Windows.Forms.Button();
            this.PortableModeCheckBox = new System.Windows.Forms.CheckBox();
            this.ConfigExportGroup = new System.Windows.Forms.GroupBox();
            this.GithubLinkLabel = new System.Windows.Forms.LinkLabel();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.SettingTab = new System.Windows.Forms.TabControl();
            this.HotkeysPage = new System.Windows.Forms.TabPage();
            this.settingGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveAllKeyBtn = new System.Windows.Forms.Button();
            this.SaveAllKeyLabel = new System.Windows.Forms.Label();
            this.SaveAllLabel = new System.Windows.Forms.Label();
            this.SaveLabel = new System.Windows.Forms.Label();
            this.RestoreAllKeyLabel = new System.Windows.Forms.Label();
            this.RestoreLabel = new System.Windows.Forms.Label();
            this.RestoreKeyLabel = new System.Windows.Forms.Label();
            this.RestoreKeyBtn = new System.Windows.Forms.Button();
            this.SaveKeyLabel = new System.Windows.Forms.Label();
            this.SaveKeyBtn = new System.Windows.Forms.Button();
            this.DisableInFullScreenCheckBox = new System.Windows.Forms.CheckBox();
            this.RestoreAllKeyBtn = new System.Windows.Forms.Button();
            this.RestoreAllLabel = new System.Windows.Forms.Label();
            this.ProcessesPage = new System.Windows.Forms.TabPage();
            this.ProfilesTab = new System.Windows.Forms.TabPage();
            this.ProfileGroupBox = new System.Windows.Forms.GroupBox();
            this.ProfilesLayout = new System.Windows.Forms.TableLayoutPanel();
            this.NewProfile = new System.Windows.Forms.Button();
            this.AboutPage = new System.Windows.Forms.TabPage();
            this.AboutGroup = new System.Windows.Forms.GroupBox();
            this.UpdateCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessesGrid)).BeginInit();
            this.ConfigExportGroup.SuspendLayout();
            this.SettingTab.SuspendLayout();
            this.HotkeysPage.SuspendLayout();
            this.settingGroupBox.SuspendLayout();
            this.ProcessesPage.SuspendLayout();
            this.ProfilesTab.SuspendLayout();
            this.ProfileGroupBox.SuspendLayout();
            this.AboutPage.SuspendLayout();
            this.AboutGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProcessesGrid
            // 
            this.ProcessesGrid.AllowUserToResizeRows = false;
            this.ProcessesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ProcessesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProcessesGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ProcessesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ProcessesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ProcessesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProcessesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ProcessesGrid.GridColor = System.Drawing.SystemColors.Window;
            this.ProcessesGrid.Location = new System.Drawing.Point(17, 16);
            this.ProcessesGrid.Margin = new System.Windows.Forms.Padding(4);
            this.ProcessesGrid.MultiSelect = false;
            this.ProcessesGrid.Name = "ProcessesGrid";
            this.ProcessesGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ProcessesGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ProcessesGrid.RowHeadersVisible = false;
            this.ProcessesGrid.RowHeadersWidth = 51;
            this.ProcessesGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ProcessesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ProcessesGrid.ShowCellToolTips = false;
            this.ProcessesGrid.Size = new System.Drawing.Size(968, 638);
            this.ProcessesGrid.TabIndex = 7;
            this.ProcessesGrid.TabStop = false;
            // 
            // ConfigExportBtn
            // 
            this.ConfigExportBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ConfigExportBtn.Location = new System.Drawing.Point(148, 89);
            this.ConfigExportBtn.Name = "ConfigExportBtn";
            this.ConfigExportBtn.Size = new System.Drawing.Size(90, 40);
            this.ConfigExportBtn.TabIndex = 9;
            this.ConfigExportBtn.Text = "Export";
            this.ConfigExportBtn.UseVisualStyleBackColor = false;
            this.ConfigExportBtn.Click += new System.EventHandler(this.ConfigExportBtn_Click);
            // 
            // ConfigImportBtn
            // 
            this.ConfigImportBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ConfigImportBtn.Location = new System.Drawing.Point(26, 89);
            this.ConfigImportBtn.Name = "ConfigImportBtn";
            this.ConfigImportBtn.Size = new System.Drawing.Size(90, 40);
            this.ConfigImportBtn.TabIndex = 10;
            this.ConfigImportBtn.Text = "Import";
            this.ConfigImportBtn.UseVisualStyleBackColor = false;
            this.ConfigImportBtn.Click += new System.EventHandler(this.ConfigImportBtn_Click);
            // 
            // PortableModeCheckBox
            // 
            this.PortableModeCheckBox.AutoSize = true;
            this.PortableModeCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PortableModeCheckBox.Location = new System.Drawing.Point(26, 46);
            this.PortableModeCheckBox.Name = "PortableModeCheckBox";
            this.PortableModeCheckBox.Size = new System.Drawing.Size(133, 19);
            this.PortableModeCheckBox.TabIndex = 11;
            this.PortableModeCheckBox.Text = "Portable Mode";
            this.PortableModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfigExportGroup
            // 
            this.ConfigExportGroup.Controls.Add(this.PortableModeCheckBox);
            this.ConfigExportGroup.Controls.Add(this.ConfigImportBtn);
            this.ConfigExportGroup.Controls.Add(this.ConfigExportBtn);
            this.ConfigExportGroup.Location = new System.Drawing.Point(33, 280);
            this.ConfigExportGroup.Name = "ConfigExportGroup";
            this.ConfigExportGroup.Size = new System.Drawing.Size(925, 160);
            this.ConfigExportGroup.TabIndex = 12;
            this.ConfigExportGroup.TabStop = false;
            this.ConfigExportGroup.Text = "Import/Export Config";
            // 
            // GithubLinkLabel
            // 
            this.GithubLinkLabel.Location = new System.Drawing.Point(26, 100);
            this.GithubLinkLabel.Name = "GithubLinkLabel";
            this.GithubLinkLabel.Size = new System.Drawing.Size(500, 24);
            this.GithubLinkLabel.TabIndex = 14;
            this.GithubLinkLabel.TabStop = true;
            this.GithubLinkLabel.Text = "GithubLinkLabel";
            this.GithubLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VersionLabel
            // 
            this.VersionLabel.Location = new System.Drawing.Point(26, 48);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(300, 24);
            this.VersionLabel.TabIndex = 15;
            this.VersionLabel.Text = "VersionLabel";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingTab
            // 
            this.SettingTab.Controls.Add(this.HotkeysPage);
            this.SettingTab.Controls.Add(this.ProcessesPage);
            this.SettingTab.Controls.Add(this.ProfilesTab);
            this.SettingTab.Controls.Add(this.AboutPage);
            this.SettingTab.ItemSize = new System.Drawing.Size(180, 40);
            this.SettingTab.Location = new System.Drawing.Point(12, 12);
            this.SettingTab.Name = "SettingTab";
            this.SettingTab.Padding = new System.Drawing.Point(0, 0);
            this.SettingTab.SelectedIndex = 0;
            this.SettingTab.Size = new System.Drawing.Size(1000, 709);
            this.SettingTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.SettingTab.TabIndex = 18;
            // 
            // HotkeysPage
            // 
            this.HotkeysPage.BackColor = System.Drawing.SystemColors.Window;
            this.HotkeysPage.Controls.Add(this.settingGroupBox);
            this.HotkeysPage.Location = new System.Drawing.Point(4, 44);
            this.HotkeysPage.Name = "HotkeysPage";
            this.HotkeysPage.Padding = new System.Windows.Forms.Padding(3);
            this.HotkeysPage.Size = new System.Drawing.Size(992, 661);
            this.HotkeysPage.TabIndex = 0;
            this.HotkeysPage.Text = "Hotkeys";
            // 
            // settingGroupBox
            // 
            this.settingGroupBox.Controls.Add(this.SaveAllKeyBtn);
            this.settingGroupBox.Controls.Add(this.SaveAllKeyLabel);
            this.settingGroupBox.Controls.Add(this.SaveAllLabel);
            this.settingGroupBox.Controls.Add(this.SaveLabel);
            this.settingGroupBox.Controls.Add(this.RestoreAllKeyLabel);
            this.settingGroupBox.Controls.Add(this.RestoreLabel);
            this.settingGroupBox.Controls.Add(this.RestoreKeyLabel);
            this.settingGroupBox.Controls.Add(this.RestoreKeyBtn);
            this.settingGroupBox.Controls.Add(this.SaveKeyLabel);
            this.settingGroupBox.Controls.Add(this.SaveKeyBtn);
            this.settingGroupBox.Controls.Add(this.DisableInFullScreenCheckBox);
            this.settingGroupBox.Controls.Add(this.RestoreAllKeyBtn);
            this.settingGroupBox.Controls.Add(this.RestoreAllLabel);
            this.settingGroupBox.Location = new System.Drawing.Point(33, 24);
            this.settingGroupBox.Name = "settingGroupBox";
            this.settingGroupBox.Size = new System.Drawing.Size(925, 415);
            this.settingGroupBox.TabIndex = 16;
            this.settingGroupBox.TabStop = false;
            this.settingGroupBox.Text = "Hotkeys ";
            // 
            // SaveAllKeyBtn
            // 
            this.SaveAllKeyBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SaveAllKeyBtn.Location = new System.Drawing.Point(507, 179);
            this.SaveAllKeyBtn.Name = "SaveAllKeyBtn";
            this.SaveAllKeyBtn.Size = new System.Drawing.Size(120, 40);
            this.SaveAllKeyBtn.TabIndex = 18;
            this.SaveAllKeyBtn.Text = "Edit";
            this.SaveAllKeyBtn.UseVisualStyleBackColor = false;
            // 
            // SaveAllKeyLabel
            // 
            this.SaveAllKeyLabel.AutoSize = true;
            this.SaveAllKeyLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.SaveAllKeyLabel.Location = new System.Drawing.Point(204, 192);
            this.SaveAllKeyLabel.Name = "SaveAllKeyLabel";
            this.SaveAllKeyLabel.Size = new System.Drawing.Size(127, 15);
            this.SaveAllKeyLabel.TabIndex = 17;
            this.SaveAllKeyLabel.Text = "SaveAllKeyLabel";
            // 
            // SaveAllLabel
            // 
            this.SaveAllLabel.AutoSize = true;
            this.SaveAllLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SaveAllLabel.Location = new System.Drawing.Point(32, 192);
            this.SaveAllLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SaveAllLabel.Name = "SaveAllLabel";
            this.SaveAllLabel.Size = new System.Drawing.Size(71, 15);
            this.SaveAllLabel.TabIndex = 16;
            this.SaveAllLabel.Text = "Save All";
            this.SaveAllLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveLabel
            // 
            this.SaveLabel.AutoSize = true;
            this.SaveLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SaveLabel.Location = new System.Drawing.Point(32, 51);
            this.SaveLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SaveLabel.Name = "SaveLabel";
            this.SaveLabel.Size = new System.Drawing.Size(39, 15);
            this.SaveLabel.TabIndex = 2;
            this.SaveLabel.Text = "Save";
            this.SaveLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RestoreAllKeyLabel
            // 
            this.RestoreAllKeyLabel.AutoSize = true;
            this.RestoreAllKeyLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.RestoreAllKeyLabel.Location = new System.Drawing.Point(204, 269);
            this.RestoreAllKeyLabel.Name = "RestoreAllKeyLabel";
            this.RestoreAllKeyLabel.Size = new System.Drawing.Size(151, 15);
            this.RestoreAllKeyLabel.TabIndex = 15;
            this.RestoreAllKeyLabel.Text = "RestoreAllKeyLabel";
            // 
            // RestoreLabel
            // 
            this.RestoreLabel.AutoSize = true;
            this.RestoreLabel.Location = new System.Drawing.Point(32, 118);
            this.RestoreLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RestoreLabel.Name = "RestoreLabel";
            this.RestoreLabel.Size = new System.Drawing.Size(63, 15);
            this.RestoreLabel.TabIndex = 5;
            this.RestoreLabel.Text = "Restore";
            this.RestoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RestoreKeyLabel
            // 
            this.RestoreKeyLabel.AutoSize = true;
            this.RestoreKeyLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.RestoreKeyLabel.Location = new System.Drawing.Point(204, 118);
            this.RestoreKeyLabel.Name = "RestoreKeyLabel";
            this.RestoreKeyLabel.Size = new System.Drawing.Size(127, 15);
            this.RestoreKeyLabel.TabIndex = 14;
            this.RestoreKeyLabel.Text = "RestoreKeyLabel";
            // 
            // RestoreKeyBtn
            // 
            this.RestoreKeyBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.RestoreKeyBtn.Location = new System.Drawing.Point(507, 105);
            this.RestoreKeyBtn.Name = "RestoreKeyBtn";
            this.RestoreKeyBtn.Size = new System.Drawing.Size(120, 40);
            this.RestoreKeyBtn.TabIndex = 12;
            this.RestoreKeyBtn.Text = "Edit";
            this.RestoreKeyBtn.UseVisualStyleBackColor = false;
            // 
            // SaveKeyLabel
            // 
            this.SaveKeyLabel.AutoSize = true;
            this.SaveKeyLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.SaveKeyLabel.Location = new System.Drawing.Point(204, 51);
            this.SaveKeyLabel.Name = "SaveKeyLabel";
            this.SaveKeyLabel.Size = new System.Drawing.Size(103, 15);
            this.SaveKeyLabel.TabIndex = 11;
            this.SaveKeyLabel.Text = "SaveKeyLabel";
            // 
            // SaveKeyBtn
            // 
            this.SaveKeyBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SaveKeyBtn.Location = new System.Drawing.Point(507, 38);
            this.SaveKeyBtn.Name = "SaveKeyBtn";
            this.SaveKeyBtn.Size = new System.Drawing.Size(120, 40);
            this.SaveKeyBtn.TabIndex = 10;
            this.SaveKeyBtn.Text = "Edit";
            this.SaveKeyBtn.UseVisualStyleBackColor = false;
            // 
            // DisableInFullScreenCheckBox
            // 
            this.DisableInFullScreenCheckBox.AutoSize = true;
            this.DisableInFullScreenCheckBox.BackColor = System.Drawing.SystemColors.Window;
            this.DisableInFullScreenCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DisableInFullScreenCheckBox.Location = new System.Drawing.Point(32, 350);
            this.DisableInFullScreenCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.DisableInFullScreenCheckBox.Name = "DisableInFullScreenCheckBox";
            this.DisableInFullScreenCheckBox.Size = new System.Drawing.Size(237, 19);
            this.DisableInFullScreenCheckBox.TabIndex = 6;
            this.DisableInFullScreenCheckBox.TabStop = false;
            this.DisableInFullScreenCheckBox.Text = "Disable in FullScreen Mode";
            this.DisableInFullScreenCheckBox.UseVisualStyleBackColor = false;
            // 
            // RestoreAllKeyBtn
            // 
            this.RestoreAllKeyBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.RestoreAllKeyBtn.Location = new System.Drawing.Point(507, 256);
            this.RestoreAllKeyBtn.Name = "RestoreAllKeyBtn";
            this.RestoreAllKeyBtn.Size = new System.Drawing.Size(120, 40);
            this.RestoreAllKeyBtn.TabIndex = 13;
            this.RestoreAllKeyBtn.Text = "Edit";
            this.RestoreAllKeyBtn.UseVisualStyleBackColor = false;
            // 
            // RestoreAllLabel
            // 
            this.RestoreAllLabel.AutoSize = true;
            this.RestoreAllLabel.Location = new System.Drawing.Point(32, 269);
            this.RestoreAllLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RestoreAllLabel.Name = "RestoreAllLabel";
            this.RestoreAllLabel.Size = new System.Drawing.Size(95, 15);
            this.RestoreAllLabel.TabIndex = 9;
            this.RestoreAllLabel.Text = "Restore All";
            this.RestoreAllLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProcessesPage
            // 
            this.ProcessesPage.AutoScroll = true;
            this.ProcessesPage.BackColor = System.Drawing.SystemColors.Window;
            this.ProcessesPage.Controls.Add(this.ProcessesGrid);
            this.ProcessesPage.Location = new System.Drawing.Point(4, 44);
            this.ProcessesPage.Name = "ProcessesPage";
            this.ProcessesPage.Padding = new System.Windows.Forms.Padding(3);
            this.ProcessesPage.Size = new System.Drawing.Size(992, 661);
            this.ProcessesPage.TabIndex = 1;
            this.ProcessesPage.Text = "Processes";
            // 
            // ProfilesTab
            // 
            this.ProfilesTab.Controls.Add(this.ProfileGroupBox);
            this.ProfilesTab.Controls.Add(this.NewProfile);
            this.ProfilesTab.Location = new System.Drawing.Point(4, 44);
            this.ProfilesTab.Name = "ProfilesTab";
            this.ProfilesTab.Padding = new System.Windows.Forms.Padding(3);
            this.ProfilesTab.Size = new System.Drawing.Size(992, 661);
            this.ProfilesTab.TabIndex = 3;
            this.ProfilesTab.Text = "Profiles";
            this.ProfilesTab.UseVisualStyleBackColor = true;
            // 
            // ProfileGroupBox
            // 
            this.ProfileGroupBox.Controls.Add(this.ProfilesLayout);
            this.ProfileGroupBox.Location = new System.Drawing.Point(33, 104);
            this.ProfileGroupBox.Name = "ProfileGroupBox";
            this.ProfileGroupBox.Size = new System.Drawing.Size(925, 555);
            this.ProfileGroupBox.TabIndex = 16;
            this.ProfileGroupBox.TabStop = false;
            this.ProfileGroupBox.Text = "Profiles";
            // 
            // ProfilesLayout
            // 
            this.ProfilesLayout.AutoScroll = true;
            this.ProfilesLayout.AutoSize = true;
            this.ProfilesLayout.ColumnCount = 4;
            this.ProfilesLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ProfilesLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ProfilesLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ProfilesLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ProfilesLayout.Location = new System.Drawing.Point(37, 32);
            this.ProfilesLayout.Name = "ProfilesLayout";
            this.ProfilesLayout.RowCount = 1;
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ProfilesLayout.Size = new System.Drawing.Size(840, 60);
            this.ProfilesLayout.TabIndex = 16;
            // 
            // NewProfile
            // 
            this.NewProfile.BackColor = System.Drawing.SystemColors.Highlight;
            this.NewProfile.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.NewProfile.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.NewProfile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.NewProfile.Location = new System.Drawing.Point(33, 29);
            this.NewProfile.Name = "NewProfile";
            this.NewProfile.Size = new System.Drawing.Size(130, 50);
            this.NewProfile.TabIndex = 15;
            this.NewProfile.Text = "New Profile";
            this.NewProfile.UseVisualStyleBackColor = false;
            // 
            // AboutPage
            // 
            this.AboutPage.BackColor = System.Drawing.SystemColors.Window;
            this.AboutPage.Controls.Add(this.AboutGroup);
            this.AboutPage.Controls.Add(this.ConfigExportGroup);
            this.AboutPage.Location = new System.Drawing.Point(4, 44);
            this.AboutPage.Name = "AboutPage";
            this.AboutPage.Padding = new System.Windows.Forms.Padding(3);
            this.AboutPage.Size = new System.Drawing.Size(992, 661);
            this.AboutPage.TabIndex = 2;
            this.AboutPage.Text = "About";
            // 
            // AboutGroup
            // 
            this.AboutGroup.Controls.Add(this.UpdateCheckBox);
            this.AboutGroup.Controls.Add(this.VersionLabel);
            this.AboutGroup.Controls.Add(this.GithubLinkLabel);
            this.AboutGroup.Location = new System.Drawing.Point(33, 24);
            this.AboutGroup.Name = "AboutGroup";
            this.AboutGroup.Size = new System.Drawing.Size(925, 215);
            this.AboutGroup.TabIndex = 16;
            this.AboutGroup.TabStop = false;
            this.AboutGroup.Text = "About";
            // 
            // UpdateCheckBox
            // 
            this.UpdateCheckBox.AutoSize = true;
            this.UpdateCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.UpdateCheckBox.Location = new System.Drawing.Point(26, 160);
            this.UpdateCheckBox.Name = "UpdateCheckBox";
            this.UpdateCheckBox.Size = new System.Drawing.Size(277, 19);
            this.UpdateCheckBox.TabIndex = 12;
            this.UpdateCheckBox.Text = "Checking for updates at startup";
            this.UpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1018, 733);
            this.Controls.Add(this.SettingTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "SettingForm";
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProcessesGrid)).EndInit();
            this.ConfigExportGroup.ResumeLayout(false);
            this.ConfigExportGroup.PerformLayout();
            this.SettingTab.ResumeLayout(false);
            this.HotkeysPage.ResumeLayout(false);
            this.settingGroupBox.ResumeLayout(false);
            this.settingGroupBox.PerformLayout();
            this.ProcessesPage.ResumeLayout(false);
            this.ProfilesTab.ResumeLayout(false);
            this.ProfileGroupBox.ResumeLayout(false);
            this.ProfileGroupBox.PerformLayout();
            this.AboutPage.ResumeLayout(false);
            this.AboutGroup.ResumeLayout(false);
            this.AboutGroup.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel ProfilesLayout;

        private System.Windows.Forms.GroupBox ProfileGroupBox;

        private System.Windows.Forms.Button NewProfile;

        private System.Windows.Forms.Label SaveAllLabel;
        private System.Windows.Forms.Label SaveAllKeyLabel;
        private System.Windows.Forms.Button SaveAllKeyBtn;

        private System.Windows.Forms.CheckBox UpdateCheckBox;

        private System.Windows.Forms.GroupBox settingGroupBox;

        private System.Windows.Forms.GroupBox ConfigExportGroup;

        private System.Windows.Forms.CheckBox PortableModeCheckBox;

        #endregion

        private System.Windows.Forms.Label SaveLabel;
        private System.Windows.Forms.Label RestoreLabel;
        private System.Windows.Forms.CheckBox DisableInFullScreenCheckBox;
        private System.Windows.Forms.DataGridView ProcessesGrid;
        private System.Windows.Forms.Button ConfigExportBtn;
        private System.Windows.Forms.Button ConfigImportBtn;
        private System.Windows.Forms.GroupBox AboutGroup;
        private System.Windows.Forms.Label RestoreAllLabel;
        private System.Windows.Forms.LinkLabel GithubLinkLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.TabControl SettingTab;
        private System.Windows.Forms.TabPage HotkeysPage;
        private System.Windows.Forms.TabPage ProcessesPage;
        private System.Windows.Forms.TabPage AboutPage;
        private System.Windows.Forms.Button SaveKeyBtn;
        private System.Windows.Forms.Label SaveKeyLabel;
        private System.Windows.Forms.Button RestoreKeyBtn;
        private System.Windows.Forms.Button RestoreAllKeyBtn;
        private System.Windows.Forms.Label RestoreKeyLabel;
        private System.Windows.Forms.Label RestoreAllKeyLabel;
        private System.Windows.Forms.TabPage ProfilesTab;
    }
}
