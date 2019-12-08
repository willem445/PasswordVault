namespace PasswordVault.Desktop.Winforms
{
    partial class ExportView
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
            this.closeButton = new System.Windows.Forms.Label();
            this.moveWindowPanel = new System.Windows.Forms.Panel();
            this.windowTitleLabel = new System.Windows.Forms.Label();
            this.filePathTextbox = new System.Windows.Forms.TextBox();
            this.browseFoldersButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.exportPasswordTextbox = new System.Windows.Forms.TextBox();
            this.encryptionEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.moveWindowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(359, 9);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(14, 13);
            this.closeButton.TabIndex = 28;
            this.closeButton.Text = "X";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            this.closeButton.MouseEnter += new System.EventHandler(this.CloseButton_MouseEnter);
            this.closeButton.MouseLeave += new System.EventHandler(this.CloseButton_MouseLeave);
            // 
            // moveWindowPanel
            // 
            this.moveWindowPanel.Controls.Add(this.windowTitleLabel);
            this.moveWindowPanel.Location = new System.Drawing.Point(1, 1);
            this.moveWindowPanel.Name = "moveWindowPanel";
            this.moveWindowPanel.Size = new System.Drawing.Size(352, 28);
            this.moveWindowPanel.TabIndex = 33;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseDown);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseMove);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseUp);
            // 
            // windowTitleLabel
            // 
            this.windowTitleLabel.AutoSize = true;
            this.windowTitleLabel.Location = new System.Drawing.Point(11, 8);
            this.windowTitleLabel.Name = "windowTitleLabel";
            this.windowTitleLabel.Size = new System.Drawing.Size(91, 13);
            this.windowTitleLabel.TabIndex = 32;
            this.windowTitleLabel.Text = "Export Passwords";
            this.windowTitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowTitle_MouseDown);
            this.windowTitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowTitle_MouseMove);
            this.windowTitleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WindowTitle_MouseUp);
            // 
            // filePathTextbox
            // 
            this.filePathTextbox.Location = new System.Drawing.Point(12, 50);
            this.filePathTextbox.Name = "filePathTextbox";
            this.filePathTextbox.Size = new System.Drawing.Size(328, 20);
            this.filePathTextbox.TabIndex = 34;
            // 
            // browseFoldersButton
            // 
            this.browseFoldersButton.Location = new System.Drawing.Point(346, 50);
            this.browseFoldersButton.Name = "browseFoldersButton";
            this.browseFoldersButton.Size = new System.Drawing.Size(27, 20);
            this.browseFoldersButton.TabIndex = 35;
            this.browseFoldersButton.Text = "...";
            this.browseFoldersButton.UseVisualStyleBackColor = true;
            this.browseFoldersButton.Click += new System.EventHandler(this.browseFoldersButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(298, 108);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 36;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // exportPasswordTextbox
            // 
            this.exportPasswordTextbox.Location = new System.Drawing.Point(12, 76);
            this.exportPasswordTextbox.Name = "exportPasswordTextbox";
            this.exportPasswordTextbox.Size = new System.Drawing.Size(361, 20);
            this.exportPasswordTextbox.TabIndex = 37;
            this.exportPasswordTextbox.TextChanged += new System.EventHandler(this.exportPasswordTextbox_TextChanged);
            this.exportPasswordTextbox.Enter += new System.EventHandler(this.exportPasswordTextbox_Enter);
            this.exportPasswordTextbox.Leave += new System.EventHandler(this.exportPasswordTextbox_Leave);
            // 
            // encryptionEnabledCheckbox
            // 
            this.encryptionEnabledCheckbox.AutoSize = true;
            this.encryptionEnabledCheckbox.Location = new System.Drawing.Point(12, 102);
            this.encryptionEnabledCheckbox.Name = "encryptionEnabledCheckbox";
            this.encryptionEnabledCheckbox.Size = new System.Drawing.Size(123, 17);
            this.encryptionEnabledCheckbox.TabIndex = 38;
            this.encryptionEnabledCheckbox.Text = "Password Protection";
            this.encryptionEnabledCheckbox.UseVisualStyleBackColor = true;
            this.encryptionEnabledCheckbox.CheckedChanged += new System.EventHandler(this.encryptionEnabledCheckbox_CheckedChanged);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 122);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 13);
            this.statusLabel.TabIndex = 39;
            this.statusLabel.Text = "status";
            // 
            // ExportView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(385, 143);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.encryptionEnabledCheckbox);
            this.Controls.Add(this.exportPasswordTextbox);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.browseFoldersButton);
            this.Controls.Add(this.filePathTextbox);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExportView";
            this.Text = "AboutView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportView_FormClosing);
            this.moveWindowPanel.ResumeLayout(false);
            this.moveWindowPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.Panel moveWindowPanel;
        private System.Windows.Forms.Label windowTitleLabel;
        private System.Windows.Forms.TextBox filePathTextbox;
        private System.Windows.Forms.Button browseFoldersButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.TextBox exportPasswordTextbox;
        private System.Windows.Forms.CheckBox encryptionEnabledCheckbox;
        private System.Windows.Forms.Label statusLabel;
    }
}