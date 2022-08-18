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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.WindowsGrid = new System.Windows.Forms.DataGridView();
            this.ConfigExportBtn = new System.Windows.Forms.Button();
            this.ConfigImportBtn = new System.Windows.Forms.Button();
            this.PortableModeCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GithubLinkLabel = new System.Windows.Forms.LinkLabel();
            this.GithubLabel = new System.Windows.Forms.Label();
            this.SettingTab = new System.Windows.Forms.TabControl();
            this.ShortcutsPage = new System.Windows.Forms.TabPage();
            this.RestoreAllKeyLabel = new System.Windows.Forms.Label();
            this.RestoreKeyLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveKeyBtn = new System.Windows.Forms.Button();
            this.RestoreAllKeyBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.DisableInFullScreenCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveKeyLabel = new System.Windows.Forms.Label();
            this.RestoreKeyBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ConfigPage = new System.Windows.Forms.TabPage();
            this.AboutPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.WindowsGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SettingTab.SuspendLayout();
            this.ShortcutsPage.SuspendLayout();
            this.ConfigPage.SuspendLayout();
            this.AboutPage.SuspendLayout();
            this.SuspendLayout();
            //
            // WindowsGrid
            //
            this.WindowsGrid.AllowUserToResizeRows = false;
            this.WindowsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WindowsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.WindowsGrid.Location = new System.Drawing.Point(11, 26);
            this.WindowsGrid.Margin = new System.Windows.Forms.Padding(6);
            this.WindowsGrid.MultiSelect = false;
            this.WindowsGrid.Name = "WindowsGrid";
            this.WindowsGrid.RowHeadersVisible = false;
            this.WindowsGrid.RowHeadersWidth = 51;
            this.WindowsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.WindowsGrid.ShowCellToolTips = false;
            this.WindowsGrid.Size = new System.Drawing.Size(1410, 992);
            this.WindowsGrid.TabIndex = 7;
            this.WindowsGrid.TabStop = false;
            //
            // ConfigExportBtn
            //
            this.ConfigExportBtn.Location = new System.Drawing.Point(309, 142);
            this.ConfigExportBtn.Margin = new System.Windows.Forms.Padding(6);
            this.ConfigExportBtn.Name = "ConfigExportBtn";
            this.ConfigExportBtn.Size = new System.Drawing.Size(238, 62);
            this.ConfigExportBtn.TabIndex = 9;
            this.ConfigExportBtn.Text = "Export config";
            this.ConfigExportBtn.UseVisualStyleBackColor = true;
            this.ConfigExportBtn.Click += new System.EventHandler(this.ConfigExportBtn_Click);
            //
            // ConfigImportBtn
            //
            this.ConfigImportBtn.Location = new System.Drawing.Point(29, 142);
            this.ConfigImportBtn.Margin = new System.Windows.Forms.Padding(6);
            this.ConfigImportBtn.Name = "ConfigImportBtn";
            this.ConfigImportBtn.Size = new System.Drawing.Size(238, 62);
            this.ConfigImportBtn.TabIndex = 10;
            this.ConfigImportBtn.Text = "Import config";
            this.ConfigImportBtn.UseVisualStyleBackColor = true;
            this.ConfigImportBtn.Click += new System.EventHandler(this.ConfigImportBtn_Click);
            //
            // PortableModeCheckBox
            //
            this.PortableModeCheckBox.AutoSize = true;
            this.PortableModeCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PortableModeCheckBox.Location = new System.Drawing.Point(42, 72);
            this.PortableModeCheckBox.Margin = new System.Windows.Forms.Padding(5);
            this.PortableModeCheckBox.Name = "PortableModeCheckBox";
            this.PortableModeCheckBox.Size = new System.Drawing.Size(216, 35);
            this.PortableModeCheckBox.TabIndex = 11;
            this.PortableModeCheckBox.Text = "Portable Mode";
            this.PortableModeCheckBox.UseVisualStyleBackColor = true;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.PortableModeCheckBox);
            this.groupBox1.Controls.Add(this.ConfigImportBtn);
            this.groupBox1.Controls.Add(this.ConfigExportBtn);
            this.groupBox1.Location = new System.Drawing.Point(53, 269);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(1334, 251);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import/Export Config";
            //
            // GithubLinkLabel
            //
            this.GithubLinkLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GithubLinkLabel.Location = new System.Drawing.Point(53, 138);
            this.GithubLinkLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.GithubLinkLabel.Name = "GithubLinkLabel";
            this.GithubLinkLabel.Size = new System.Drawing.Size(800, 38);
            this.GithubLinkLabel.TabIndex = 14;
            this.GithubLinkLabel.TabStop = true;
            this.GithubLinkLabel.Text = "GithubLinkLabel";
            this.GithubLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // GithubLabel
            //
            this.GithubLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GithubLabel.Location = new System.Drawing.Point(53, 59);
            this.GithubLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.GithubLabel.Name = "GithubLabel";
            this.GithubLabel.Size = new System.Drawing.Size(480, 38);
            this.GithubLabel.TabIndex = 15;
            this.GithubLabel.Text = "GithubLabel";
            this.GithubLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SettingTab
            //
            this.SettingTab.Controls.Add(this.ShortcutsPage);
            this.SettingTab.Controls.Add(this.ConfigPage);
            this.SettingTab.Controls.Add(this.AboutPage);
            this.SettingTab.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.SettingTab.Location = new System.Drawing.Point(19, 19);
            this.SettingTab.Margin = new System.Windows.Forms.Padding(5);
            this.SettingTab.Name = "SettingTab";
            this.SettingTab.Padding = new System.Drawing.Point(20, 3);
            this.SettingTab.SelectedIndex = 0;
            this.SettingTab.Size = new System.Drawing.Size(1458, 1098);
            this.SettingTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.SettingTab.TabIndex = 18;
            //
            // ShortcutsPage
            //
            this.ShortcutsPage.Controls.Add(this.RestoreAllKeyLabel);
            this.ShortcutsPage.Controls.Add(this.RestoreKeyLabel);
            this.ShortcutsPage.Controls.Add(this.label1);
            this.ShortcutsPage.Controls.Add(this.SaveKeyBtn);
            this.ShortcutsPage.Controls.Add(this.RestoreAllKeyBtn);
            this.ShortcutsPage.Controls.Add(this.label3);
            this.ShortcutsPage.Controls.Add(this.DisableInFullScreenCheckBox);
            this.ShortcutsPage.Controls.Add(this.SaveKeyLabel);
            this.ShortcutsPage.Controls.Add(this.RestoreKeyBtn);
            this.ShortcutsPage.Controls.Add(this.label2);
            this.ShortcutsPage.Location = new System.Drawing.Point(8, 45);
            this.ShortcutsPage.Margin = new System.Windows.Forms.Padding(5);
            this.ShortcutsPage.Name = "ShortcutsPage";
            this.ShortcutsPage.Padding = new System.Windows.Forms.Padding(5);
            this.ShortcutsPage.Size = new System.Drawing.Size(1442, 1045);
            this.ShortcutsPage.TabIndex = 0;
            this.ShortcutsPage.Text = "Shortcuts";
            this.ShortcutsPage.UseVisualStyleBackColor = true;
            //
            // RestoreAllKeyLabel
            //
            this.RestoreAllKeyLabel.AutoSize = true;
            this.RestoreAllKeyLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.RestoreAllKeyLabel.Location = new System.Drawing.Point(341, 274);
            this.RestoreAllKeyLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.RestoreAllKeyLabel.Name = "RestoreAllKeyLabel";
            this.RestoreAllKeyLabel.Size = new System.Drawing.Size(260, 35);
            this.RestoreAllKeyLabel.TabIndex = 15;
            this.RestoreAllKeyLabel.Text = "RestoreAllKeyLabel";
            //
            // RestoreKeyLabel
            //
            this.RestoreKeyLabel.AutoSize = true;
            this.RestoreKeyLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.RestoreKeyLabel.Location = new System.Drawing.Point(341, 170);
            this.RestoreKeyLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.RestoreKeyLabel.Name = "RestoreKeyLabel";
            this.RestoreKeyLabel.Size = new System.Drawing.Size(227, 35);
            this.RestoreKeyLabel.TabIndex = 14;
            this.RestoreKeyLabel.Text = "RestoreKeyLabel";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label1.Location = new System.Drawing.Point(64, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 35);
            this.label1.TabIndex = 2;
            this.label1.Text = "Save";
            //
            // SaveKeyBtn
            //
            this.SaveKeyBtn.Location = new System.Drawing.Point(739, 48);
            this.SaveKeyBtn.Margin = new System.Windows.Forms.Padding(5);
            this.SaveKeyBtn.Name = "SaveKeyBtn";
            this.SaveKeyBtn.Size = new System.Drawing.Size(120, 51);
            this.SaveKeyBtn.TabIndex = 10;
            this.SaveKeyBtn.Text = "Change";
            this.SaveKeyBtn.UseVisualStyleBackColor = true;
            //
            // RestoreAllKeyBtn
            //
            this.RestoreAllKeyBtn.Location = new System.Drawing.Point(739, 259);
            this.RestoreAllKeyBtn.Margin = new System.Windows.Forms.Padding(5);
            this.RestoreAllKeyBtn.Name = "RestoreAllKeyBtn";
            this.RestoreAllKeyBtn.Size = new System.Drawing.Size(120, 51);
            this.RestoreAllKeyBtn.TabIndex = 13;
            this.RestoreAllKeyBtn.Text = "Change";
            this.RestoreAllKeyBtn.UseVisualStyleBackColor = true;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label3.Location = new System.Drawing.Point(64, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 35);
            this.label3.TabIndex = 9;
            this.label3.Text = "Restore All";
            //
            // DisableInFullScreenCheckBox
            //
            this.DisableInFullScreenCheckBox.AutoSize = true;
            this.DisableInFullScreenCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DisableInFullScreenCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.DisableInFullScreenCheckBox.Location = new System.Drawing.Point(64, 387);
            this.DisableInFullScreenCheckBox.Name = "DisableInFullScreenCheckBox";
            this.DisableInFullScreenCheckBox.Size = new System.Drawing.Size(397, 39);
            this.DisableInFullScreenCheckBox.TabIndex = 6;
            this.DisableInFullScreenCheckBox.TabStop = false;
            this.DisableInFullScreenCheckBox.Text = "Disable in FullScreen Mode";
            this.DisableInFullScreenCheckBox.UseVisualStyleBackColor = true;
            //
            // SaveKeyLabel
            //
            this.SaveKeyLabel.AutoSize = true;
            this.SaveKeyLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.SaveKeyLabel.Location = new System.Drawing.Point(341, 62);
            this.SaveKeyLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.SaveKeyLabel.Name = "SaveKeyLabel";
            this.SaveKeyLabel.Size = new System.Drawing.Size(189, 35);
            this.SaveKeyLabel.TabIndex = 11;
            this.SaveKeyLabel.Text = "SaveKeyLabel";
            //
            // RestoreKeyBtn
            //
            this.RestoreKeyBtn.Location = new System.Drawing.Point(739, 155);
            this.RestoreKeyBtn.Margin = new System.Windows.Forms.Padding(5);
            this.RestoreKeyBtn.Name = "RestoreKeyBtn";
            this.RestoreKeyBtn.Size = new System.Drawing.Size(120, 51);
            this.RestoreKeyBtn.TabIndex = 12;
            this.RestoreKeyBtn.Text = "Change";
            this.RestoreKeyBtn.UseVisualStyleBackColor = true;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label2.Location = new System.Drawing.Point(64, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 35);
            this.label2.TabIndex = 5;
            this.label2.Text = "Restore";
            //
            // ConfigPage
            //
            this.ConfigPage.Controls.Add(this.WindowsGrid);
            this.ConfigPage.Location = new System.Drawing.Point(8, 45);
            this.ConfigPage.Margin = new System.Windows.Forms.Padding(5);
            this.ConfigPage.Name = "ConfigPage";
            this.ConfigPage.Padding = new System.Windows.Forms.Padding(5);
            this.ConfigPage.Size = new System.Drawing.Size(1442, 1045);
            this.ConfigPage.TabIndex = 1;
            this.ConfigPage.Text = "Configs";
            this.ConfigPage.UseVisualStyleBackColor = true;
            //
            // AboutPage
            //
            this.AboutPage.Controls.Add(this.groupBox1);
            this.AboutPage.Controls.Add(this.GithubLinkLabel);
            this.AboutPage.Controls.Add(this.GithubLabel);
            this.AboutPage.Location = new System.Drawing.Point(8, 45);
            this.AboutPage.Margin = new System.Windows.Forms.Padding(5);
            this.AboutPage.Name = "AboutPage";
            this.AboutPage.Padding = new System.Windows.Forms.Padding(5);
            this.AboutPage.Size = new System.Drawing.Size(1442, 1045);
            this.AboutPage.TabIndex = 2;
            this.AboutPage.Text = "About";
            this.AboutPage.UseVisualStyleBackColor = true;
            //
            // SettingForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1488, 1123);
            this.Controls.Add(this.SettingTab);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WindowsGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SettingTab.ResumeLayout(false);
            this.ShortcutsPage.ResumeLayout(false);
            this.ShortcutsPage.PerformLayout();
            this.ConfigPage.ResumeLayout(false);
            this.AboutPage.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.CheckBox PortableModeCheckBox;

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox DisableInFullScreenCheckBox;
        private System.Windows.Forms.DataGridView WindowsGrid;
        private System.Windows.Forms.Button ConfigExportBtn;
        private System.Windows.Forms.Button ConfigImportBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel GithubLinkLabel;
        private System.Windows.Forms.Label GithubLabel;
        private System.Windows.Forms.TabControl SettingTab;
        private System.Windows.Forms.TabPage ShortcutsPage;
        private System.Windows.Forms.TabPage ConfigPage;
        private System.Windows.Forms.TabPage AboutPage;
        private System.Windows.Forms.Button SaveKeyBtn;
        private System.Windows.Forms.Label SaveKeyLabel;
        private System.Windows.Forms.Button RestoreKeyBtn;
        private System.Windows.Forms.Button RestoreAllKeyBtn;
        private System.Windows.Forms.Label RestoreKeyLabel;
        private System.Windows.Forms.Label RestoreAllKeyLabel;
    }
}
