namespace PasswordVault.Desktop.Winforms
{
    partial class AddPasswordView
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
            this.components = new System.ComponentModel.Container();
            this.closeButton = new System.Windows.Forms.Label();
            this.moveWindowPanel = new System.Windows.Forms.Panel();
            this.windowLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.applicationTextbox = new System.Windows.Forms.TextBox();
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.emailTextbox = new System.Windows.Forms.TextBox();
            this.descriptionTextbox = new System.Windows.Forms.TextBox();
            this.websiteTextbox = new System.Windows.Forms.TextBox();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.categoryCombobox = new PasswordVault.Desktop.Winforms.AdvancedComboBox();
            this.moveWindowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(225, 9);
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
            this.moveWindowPanel.Controls.Add(this.windowLabel);
            this.moveWindowPanel.Location = new System.Drawing.Point(2, 1);
            this.moveWindowPanel.Name = "moveWindowPanel";
            this.moveWindowPanel.Size = new System.Drawing.Size(205, 27);
            this.moveWindowPanel.TabIndex = 29;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseDown);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseMove);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseUp);
            // 
            // windowLabel
            // 
            this.windowLabel.AutoSize = true;
            this.windowLabel.Location = new System.Drawing.Point(10, 8);
            this.windowLabel.Name = "windowLabel";
            this.windowLabel.Size = new System.Drawing.Size(75, 13);
            this.windowLabel.TabIndex = 30;
            this.windowLabel.Text = "Add Password";
            // 
            // applicationTextbox
            // 
            this.applicationTextbox.Location = new System.Drawing.Point(24, 48);
            this.applicationTextbox.Name = "applicationTextbox";
            this.applicationTextbox.Size = new System.Drawing.Size(198, 20);
            this.applicationTextbox.TabIndex = 0;
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(24, 74);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(198, 20);
            this.usernameTextbox.TabIndex = 1;
            // 
            // emailTextbox
            // 
            this.emailTextbox.Location = new System.Drawing.Point(24, 100);
            this.emailTextbox.Name = "emailTextbox";
            this.emailTextbox.Size = new System.Drawing.Size(198, 20);
            this.emailTextbox.TabIndex = 2;
            // 
            // descriptionTextbox
            // 
            this.descriptionTextbox.Location = new System.Drawing.Point(24, 152);
            this.descriptionTextbox.Multiline = true;
            this.descriptionTextbox.Name = "descriptionTextbox";
            this.descriptionTextbox.Size = new System.Drawing.Size(198, 71);
            this.descriptionTextbox.TabIndex = 4;
            // 
            // websiteTextbox
            // 
            this.websiteTextbox.Location = new System.Drawing.Point(24, 229);
            this.websiteTextbox.Name = "websiteTextbox";
            this.websiteTextbox.Size = new System.Drawing.Size(198, 20);
            this.websiteTextbox.TabIndex = 5;
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(24, 255);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(198, 20);
            this.passwordTextbox.TabIndex = 6;
            this.passwordTextbox.TextChanged += new System.EventHandler(this.passwordTextbox_TextChanged);
            this.passwordTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passwordTextbox_KeyUp);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(147, 334);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(66, 334);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.Location = new System.Drawing.Point(24, 281);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this.generatePasswordButton.Size = new System.Drawing.Size(198, 23);
            this.generatePasswordButton.TabIndex = 10;
            this.generatePasswordButton.Text = "Generate Password";
            this.generatePasswordButton.UseVisualStyleBackColor = true;
            this.generatePasswordButton.Click += new System.EventHandler(this.generatePasswordButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(21, 311);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 13);
            this.statusLabel.TabIndex = 30;
            this.statusLabel.Text = "status";
            // 
            // categoryCombobox
            // 
            this.categoryCombobox.BorderColor = System.Drawing.Color.Black;
            this.categoryCombobox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.categoryCombobox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoryCombobox.FormattingEnabled = true;
            this.categoryCombobox.HighlightColor = System.Drawing.Color.Gray;
            this.categoryCombobox.Location = new System.Drawing.Point(24, 125);
            this.categoryCombobox.Name = "categoryCombobox";
            this.categoryCombobox.Size = new System.Drawing.Size(198, 21);
            this.categoryCombobox.TabIndex = 0;
            this.categoryCombobox.SelectedIndexChanged += new System.EventHandler(this.categoryCombobox_SelectedIndexChanged);
            // 
            // AddPasswordView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(251, 370);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.generatePasswordButton);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.categoryCombobox);
            this.Controls.Add(this.applicationTextbox);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Controls.Add(this.websiteTextbox);
            this.Controls.Add(this.emailTextbox);
            this.Controls.Add(this.descriptionTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddPasswordView";
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginView_FormClosing);
            this.moveWindowPanel.ResumeLayout(false);
            this.moveWindowPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.Panel moveWindowPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label windowLabel;
        private System.Windows.Forms.TextBox applicationTextbox;
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.TextBox emailTextbox;
        private System.Windows.Forms.TextBox descriptionTextbox;
        private System.Windows.Forms.TextBox websiteTextbox;
        private System.Windows.Forms.TextBox passwordTextbox;
        private AdvancedComboBox categoryCombobox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button generatePasswordButton;
        private System.Windows.Forms.Label statusLabel;
    }
}