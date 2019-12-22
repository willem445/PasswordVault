namespace PasswordVault.Desktop.Winforms
{
    partial class AboutView
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
            this.aboutLabel = new System.Windows.Forms.Label();
            this.iconLinkLabel = new System.Windows.Forms.LinkLabel();
            this.versionLabel = new System.Windows.Forms.Label();
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
            this.moveWindowPanel.Controls.Add(this.versionLabel);
            this.moveWindowPanel.Location = new System.Drawing.Point(1, 1);
            this.moveWindowPanel.Name = "moveWindowPanel";
            this.moveWindowPanel.Size = new System.Drawing.Size(352, 28);
            this.moveWindowPanel.TabIndex = 33;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseDown);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseMove);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseUp);
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.Location = new System.Drawing.Point(12, 37);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(35, 13);
            this.aboutLabel.TabIndex = 29;
            this.aboutLabel.Text = "label1";
            // 
            // iconLinkLabel
            // 
            this.iconLinkLabel.AutoSize = true;
            this.iconLinkLabel.Location = new System.Drawing.Point(12, 84);
            this.iconLinkLabel.Name = "iconLinkLabel";
            this.iconLinkLabel.Size = new System.Drawing.Size(55, 13);
            this.iconLinkLabel.TabIndex = 30;
            this.iconLinkLabel.TabStop = true;
            this.iconLinkLabel.Text = "linkLabel1";
            this.iconLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.IconLinkLabel_LinkClicked);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(11, 8);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(35, 13);
            this.versionLabel.TabIndex = 32;
            this.versionLabel.Text = "label3";
            this.versionLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VersionLabel_MouseDown);
            this.versionLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VersionLabel_MouseMove);
            this.versionLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VersionLabel_MouseUp);
            // 
            // AboutView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(385, 115);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.iconLinkLabel);
            this.Controls.Add(this.aboutLabel);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AboutView";
            this.Text = "AboutView";
            this.moveWindowPanel.ResumeLayout(false);
            this.moveWindowPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.Panel moveWindowPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.LinkLabel iconLinkLabel;
    }
}