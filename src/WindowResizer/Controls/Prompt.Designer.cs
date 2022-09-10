using System.ComponentModel;

namespace WindowResizer.Controls
{
    partial class Prompt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Prompt));
            this.PromptText = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PromptText
            // 
            this.PromptText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PromptText.Location = new System.Drawing.Point(3, 16);
            this.PromptText.Name = "PromptText";
            this.PromptText.Size = new System.Drawing.Size(640, 40);
            this.PromptText.TabIndex = 0;
            this.PromptText.Text = "PromptText";
            this.PromptText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OkBtn.BackColor = System.Drawing.SystemColors.Highlight;
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OkBtn.Location = new System.Drawing.Point(269, 173);
            this.OkBtn.Margin = new System.Windows.Forms.Padding(5);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(192, 64);
            this.OkBtn.TabIndex = 11;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = false;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.InputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputTextBox.Location = new System.Drawing.Point(11, 108);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(708, 24);
            this.InputTextBox.TabIndex = 12;
            this.InputTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PromptText, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.OkBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.InputTextBox, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(33, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(730, 242);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // Prompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(800, 297);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Prompt";
            this.ShowInTaskbar = false;
            this.Text = "PromptForm";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.TextBox InputTextBox;

        private System.Windows.Forms.Button OkBtn;

        private System.Windows.Forms.Label PromptText;

        #endregion
    }
}

