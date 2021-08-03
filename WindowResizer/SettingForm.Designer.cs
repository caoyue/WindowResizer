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
            this.SaveKeysBox = new System.Windows.Forms.TextBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.RestoreKeysBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.WindowsGrid = new System.Windows.Forms.DataGridView();
            this.ConfigExportBtn = new System.Windows.Forms.Button();
            this.ConfigImportBtn = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.WindowsGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveKeysBox
            // 
            this.SaveKeysBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SaveKeysBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveKeysBox.Location = new System.Drawing.Point(105, 33);
            this.SaveKeysBox.Margin = new System.Windows.Forms.Padding(2);
            this.SaveKeysBox.Name = "SaveKeysBox";
            this.SaveKeysBox.Size = new System.Drawing.Size(160, 33);
            this.SaveKeysBox.TabIndex = 0;
            this.SaveKeysBox.TabStop = false;
            // 
            // SaveBtn
            // 
            this.SaveBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveBtn.Location = new System.Drawing.Point(743, 590);
            this.SaveBtn.Margin = new System.Windows.Forms.Padding(2);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(145, 68);
            this.SaveBtn.TabIndex = 1;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(27, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Save";
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelBtn.Location = new System.Drawing.Point(552, 590);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(2);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(145, 68);
            this.CancelBtn.TabIndex = 0;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // RestoreKeysBox
            // 
            this.RestoreKeysBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RestoreKeysBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RestoreKeysBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.RestoreKeysBox.Location = new System.Drawing.Point(396, 33);
            this.RestoreKeysBox.Margin = new System.Windows.Forms.Padding(2);
            this.RestoreKeysBox.Name = "RestoreKeysBox";
            this.RestoreKeysBox.Size = new System.Drawing.Size(160, 33);
            this.RestoreKeysBox.TabIndex = 4;
            this.RestoreKeysBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(308, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "Restore";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(596, 33);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(292, 31);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.TabStop = false;
            this.checkBox1.Text = "Disable in FullScreen Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // WindowsGrid
            // 
            this.WindowsGrid.AllowUserToResizeRows = false;
            this.WindowsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WindowsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.WindowsGrid.Location = new System.Drawing.Point(7, 82);
            this.WindowsGrid.Margin = new System.Windows.Forms.Padding(4);
            this.WindowsGrid.MultiSelect = false;
            this.WindowsGrid.Name = "WindowsGrid";
            this.WindowsGrid.RowHeadersVisible = false;
            this.WindowsGrid.RowHeadersWidth = 51;
            this.WindowsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.WindowsGrid.ShowCellToolTips = false;
            this.WindowsGrid.Size = new System.Drawing.Size(881, 487);
            this.WindowsGrid.TabIndex = 7;
            this.WindowsGrid.TabStop = false;
            // 
            // ConfigExportBtn
            // 
            this.ConfigExportBtn.Location = new System.Drawing.Point(197, 103);
            this.ConfigExportBtn.Margin = new System.Windows.Forms.Padding(4);
            this.ConfigExportBtn.Name = "ConfigExportBtn";
            this.ConfigExportBtn.Size = new System.Drawing.Size(149, 39);
            this.ConfigExportBtn.TabIndex = 9;
            this.ConfigExportBtn.Text = "Export Config";
            this.ConfigExportBtn.UseVisualStyleBackColor = true;
            this.ConfigExportBtn.Click += new System.EventHandler(this.ConfigExportBtn_Click);
            // 
            // ConfigImportBtn
            // 
            this.ConfigImportBtn.Location = new System.Drawing.Point(26, 103);
            this.ConfigImportBtn.Margin = new System.Windows.Forms.Padding(4);
            this.ConfigImportBtn.Name = "ConfigImportBtn";
            this.ConfigImportBtn.Size = new System.Drawing.Size(149, 39);
            this.ConfigImportBtn.TabIndex = 10;
            this.ConfigImportBtn.Text = "Import Config";
            this.ConfigImportBtn.UseVisualStyleBackColor = true;
            this.ConfigImportBtn.Click += new System.EventHandler(this.ConfigImportBtn_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(26, 45);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(154, 27);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "Portable Mode";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.ConfigImportBtn);
            this.groupBox1.Controls.Add(this.ConfigExportBtn);
            this.groupBox1.Location = new System.Drawing.Point(15, 740);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(906, 164);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import/Export Config";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.SaveKeysBox);
            this.groupBox2.Controls.Add(this.SaveBtn);
            this.groupBox2.Controls.Add(this.CancelBtn);
            this.groupBox2.Controls.Add(this.WindowsGrid);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.RestoreKeysBox);
            this.groupBox2.Location = new System.Drawing.Point(15, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(906, 691);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Config";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(940, 925);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WindowsGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox SaveKeysBox;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox RestoreKeysBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView WindowsGrid;
        private System.Windows.Forms.Button ConfigExportBtn;
        private System.Windows.Forms.Button ConfigImportBtn;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
