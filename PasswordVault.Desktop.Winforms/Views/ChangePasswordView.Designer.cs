namespace PasswordVault.Desktop.Winforms
{
    partial class ChangePasswordView
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
            this.moveWindowPanel = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.changePasswordButton = new System.Windows.Forms.Button();
            this.currentPasswordTextbox = new System.Windows.Forms.TextBox();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.confirmPasswordTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.passwordStrengthLabel = new System.Windows.Forms.Label();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this.passwordStrengthProgressBar = new PasswordVault.Desktop.Winforms.AdvancedProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // moveWindowPanel
            // 
            this.moveWindowPanel.Location = new System.Drawing.Point(3, 1);
            this.moveWindowPanel.Name = "moveWindowPanel";
            this.moveWindowPanel.Size = new System.Drawing.Size(244, 27);
            this.moveWindowPanel.TabIndex = 31;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveWindowPanel_MouseDown_1);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.moveWindowPanel_MouseMove_1);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.moveWindowPanel_MouseUp_1);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(253, 9);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(14, 13);
            this.closeButton.TabIndex = 30;
            this.closeButton.Text = "X";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            this.closeButton.MouseEnter += new System.EventHandler(this.closeButton_MouseEnter);
            this.closeButton.MouseLeave += new System.EventHandler(this.closeButton_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Current Password:";
            // 
            // changePasswordButton
            // 
            this.changePasswordButton.Location = new System.Drawing.Point(156, 235);
            this.changePasswordButton.Name = "changePasswordButton";
            this.changePasswordButton.Size = new System.Drawing.Size(104, 23);
            this.changePasswordButton.TabIndex = 33;
            this.changePasswordButton.Text = "Change Password";
            this.changePasswordButton.UseVisualStyleBackColor = true;
            this.changePasswordButton.Click += new System.EventHandler(this.changePasswordButton_Click);
            // 
            // currentPasswordTextbox
            // 
            this.currentPasswordTextbox.Location = new System.Drawing.Point(12, 47);
            this.currentPasswordTextbox.Name = "currentPasswordTextbox";
            this.currentPasswordTextbox.Size = new System.Drawing.Size(248, 20);
            this.currentPasswordTextbox.TabIndex = 34;
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(12, 93);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(248, 20);
            this.passwordTextbox.TabIndex = 36;
            this.passwordTextbox.TextChanged += new System.EventHandler(this.passwordTextbox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Password:";
            // 
            // confirmPasswordTextbox
            // 
            this.confirmPasswordTextbox.Location = new System.Drawing.Point(12, 139);
            this.confirmPasswordTextbox.Name = "confirmPasswordTextbox";
            this.confirmPasswordTextbox.Size = new System.Drawing.Size(248, 20);
            this.confirmPasswordTextbox.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Confirm Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Password Strength:";
            // 
            // passwordStrengthLabel
            // 
            this.passwordStrengthLabel.AutoSize = true;
            this.passwordStrengthLabel.Location = new System.Drawing.Point(123, 172);
            this.passwordStrengthLabel.Name = "passwordStrengthLabel";
            this.passwordStrengthLabel.Size = new System.Drawing.Size(38, 13);
            this.passwordStrengthLabel.TabIndex = 41;
            this.passwordStrengthLabel.Text = "Strong";
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.Location = new System.Drawing.Point(12, 235);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this.generatePasswordButton.Size = new System.Drawing.Size(138, 23);
            this.generatePasswordButton.TabIndex = 42;
            this.generatePasswordButton.Text = "Generate Password";
            this.generatePasswordButton.UseVisualStyleBackColor = true;
            this.generatePasswordButton.Click += new System.EventHandler(this.generatePasswordButton_Click);
            // 
            // passwordStrengthProgressBar
            // 
            this.passwordStrengthProgressBar.Location = new System.Drawing.Point(12, 191);
            this.passwordStrengthProgressBar.Name = "passwordStrengthProgressBar";
            this.passwordStrengthProgressBar.Size = new System.Drawing.Size(248, 16);
            this.passwordStrengthProgressBar.TabIndex = 39;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 219);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 43;
            this.statusLabel.Text = "Status";
            // 
            // ChangePasswordView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(276, 292);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.generatePasswordButton);
            this.Controls.Add(this.passwordStrengthLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.passwordStrengthProgressBar);
            this.Controls.Add(this.confirmPasswordTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.currentPasswordTextbox);
            this.Controls.Add(this.changePasswordButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChangePasswordView";
            this.Text = "ChangePasswordView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChangePasswordView_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel moveWindowPanel;
        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button changePasswordButton;
        private System.Windows.Forms.TextBox currentPasswordTextbox;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox confirmPasswordTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label passwordStrengthLabel;
        private AdvancedProgressBar passwordStrengthProgressBar;
        private System.Windows.Forms.Button generatePasswordButton;
        private System.Windows.Forms.Label statusLabel;
    }
}