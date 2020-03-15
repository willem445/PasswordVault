﻿namespace PasswordVault.Desktop.Winforms
{
    partial class ImportView
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
            this.importButton = new System.Windows.Forms.Button();
            this.importPasswordTextbox = new System.Windows.Forms.TextBox();
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
            this.windowTitleLabel.Size = new System.Drawing.Size(90, 13);
            this.windowTitleLabel.TabIndex = 32;
            this.windowTitleLabel.Text = "Import Passwords";
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
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(298, 108);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 36;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // importPasswordTextbox
            // 
            this.importPasswordTextbox.Location = new System.Drawing.Point(12, 76);
            this.importPasswordTextbox.Name = "importPasswordTextbox";
            this.importPasswordTextbox.Size = new System.Drawing.Size(361, 20);
            this.importPasswordTextbox.TabIndex = 37;
            this.importPasswordTextbox.TextChanged += new System.EventHandler(this.importPasswordTextbox_TextChanged);
            this.importPasswordTextbox.Enter += new System.EventHandler(this.importPasswordTextbox_Enter);
            this.importPasswordTextbox.Leave += new System.EventHandler(this.importPasswordTextbox_Leave);
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
            // ImportView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(385, 143);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.importPasswordTextbox);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.browseFoldersButton);
            this.Controls.Add(this.filePathTextbox);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImportView";
            this.Text = "AboutView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportView_FormClosing);
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
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.TextBox importPasswordTextbox;
        private System.Windows.Forms.Label statusLabel;
    }
}