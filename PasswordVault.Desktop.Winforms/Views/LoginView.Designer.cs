namespace PasswordVault.Desktop.Winforms
{
    partial class LoginView
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loginResultLabel = new System.Windows.Forms.Label();
            this.loginPasswordTextBox = new System.Windows.Forms.TextBox();
            this.loginUsernameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.createEmailTextBox = new System.Windows.Forms.TextBox();
            this.createPhoneNumberTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.createLastNameTextBox = new System.Windows.Forms.TextBox();
            this.createFirstNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.createNewUserResultLabel = new System.Windows.Forms.Label();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this.createPasswordTextBox = new System.Windows.Forms.TextBox();
            this.createUsernameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.createLoginButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Label();
            this.moveWindowPanel = new System.Windows.Forms.Panel();
            this.newUserButton = new System.Windows.Forms.Button();
            this.newPasswordHelpLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.newUserButton);
            this.groupBox1.Controls.Add(this.loginResultLabel);
            this.groupBox1.Controls.Add(this.loginPasswordTextBox);
            this.groupBox1.Controls.Add(this.loginUsernameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.loginButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 208);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login";
            // 
            // loginResultLabel
            // 
            this.loginResultLabel.AutoSize = true;
            this.loginResultLabel.Location = new System.Drawing.Point(16, 187);
            this.loginResultLabel.Name = "loginResultLabel";
            this.loginResultLabel.Size = new System.Drawing.Size(69, 13);
            this.loginResultLabel.TabIndex = 52;
            this.loginResultLabel.Text = "Login Result:";
            // 
            // loginPasswordTextBox
            // 
            this.loginPasswordTextBox.Location = new System.Drawing.Point(19, 91);
            this.loginPasswordTextBox.Name = "loginPasswordTextBox";
            this.loginPasswordTextBox.PasswordChar = '•';
            this.loginPasswordTextBox.Size = new System.Drawing.Size(218, 20);
            this.loginPasswordTextBox.TabIndex = 1;
            // 
            // loginUsernameTextBox
            // 
            this.loginUsernameTextBox.Location = new System.Drawing.Point(19, 47);
            this.loginUsernameTextBox.Name = "loginUsernameTextBox";
            this.loginUsernameTextBox.Size = new System.Drawing.Size(218, 20);
            this.loginUsernameTextBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Username:";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(19, 120);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(218, 23);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.createEmailTextBox);
            this.groupBox2.Controls.Add(this.createPhoneNumberTextBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.createLastNameTextBox);
            this.groupBox2.Controls.Add(this.createFirstNameTextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.createNewUserResultLabel);
            this.groupBox2.Controls.Add(this.generatePasswordButton);
            this.groupBox2.Controls.Add(this.createPasswordTextBox);
            this.groupBox2.Controls.Add(this.createUsernameTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.createLoginButton);
            this.groupBox2.Location = new System.Drawing.Point(281, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 353);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create New User";
            // 
            // createEmailTextBox
            // 
            this.createEmailTextBox.Location = new System.Drawing.Point(15, 269);
            this.createEmailTextBox.Name = "createEmailTextBox";
            this.createEmailTextBox.Size = new System.Drawing.Size(218, 20);
            this.createEmailTextBox.TabIndex = 9;
            this.createEmailTextBox.TextChanged += new System.EventHandler(this.createEmailTextBox_TextChanged);
            this.createEmailTextBox.Enter += new System.EventHandler(this.createEmailTextBox_Enter);
            this.createEmailTextBox.Leave += new System.EventHandler(this.createEmailTextBox_Leave);
            // 
            // createPhoneNumberTextBox
            // 
            this.createPhoneNumberTextBox.Location = new System.Drawing.Point(15, 225);
            this.createPhoneNumberTextBox.Name = "createPhoneNumberTextBox";
            this.createPhoneNumberTextBox.Size = new System.Drawing.Size(218, 20);
            this.createPhoneNumberTextBox.TabIndex = 8;
            this.createPhoneNumberTextBox.TextChanged += new System.EventHandler(this.createPhoneNumberTextBox_TextChanged);
            this.createPhoneNumberTextBox.Enter += new System.EventHandler(this.createPhoneNumberTextBox_Enter);
            this.createPhoneNumberTextBox.Leave += new System.EventHandler(this.createPhoneNumberTextBox_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 59;
            this.label7.Text = "Email:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 207);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 58;
            this.label8.Text = "Phone Number:";
            // 
            // createLastNameTextBox
            // 
            this.createLastNameTextBox.Location = new System.Drawing.Point(15, 180);
            this.createLastNameTextBox.Name = "createLastNameTextBox";
            this.createLastNameTextBox.Size = new System.Drawing.Size(218, 20);
            this.createLastNameTextBox.TabIndex = 7;
            // 
            // createFirstNameTextBox
            // 
            this.createFirstNameTextBox.Location = new System.Drawing.Point(15, 136);
            this.createFirstNameTextBox.Name = "createFirstNameTextBox";
            this.createFirstNameTextBox.Size = new System.Drawing.Size(218, 20);
            this.createFirstNameTextBox.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 57;
            this.label5.Text = "Last Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "First Name:";
            // 
            // createNewUserResultLabel
            // 
            this.createNewUserResultLabel.AutoSize = true;
            this.createNewUserResultLabel.Location = new System.Drawing.Point(12, 297);
            this.createNewUserResultLabel.Name = "createNewUserResultLabel";
            this.createNewUserResultLabel.Size = new System.Drawing.Size(99, 13);
            this.createNewUserResultLabel.TabIndex = 60;
            this.createNewUserResultLabel.Text = "Create User Result:";
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.Location = new System.Drawing.Point(15, 322);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this.generatePasswordButton.Size = new System.Drawing.Size(137, 23);
            this.generatePasswordButton.TabIndex = 9;
            this.generatePasswordButton.Text = "Generate Password";
            this.generatePasswordButton.UseVisualStyleBackColor = true;
            this.generatePasswordButton.Click += new System.EventHandler(this.GeneratePasswordButton_Click);
            // 
            // createPasswordTextBox
            // 
            this.createPasswordTextBox.Location = new System.Drawing.Point(15, 91);
            this.createPasswordTextBox.Name = "createPasswordTextBox";
            this.createPasswordTextBox.Size = new System.Drawing.Size(218, 20);
            this.createPasswordTextBox.TabIndex = 5;
            this.createPasswordTextBox.TextChanged += new System.EventHandler(this.CreatePasswordTextBox_TextChanged);
            this.createPasswordTextBox.Enter += new System.EventHandler(this.createPasswordTextBox_Enter);
            this.createPasswordTextBox.Leave += new System.EventHandler(this.createPasswordTextBox_Leave);
            this.createPasswordTextBox.MouseEnter += new System.EventHandler(this.createPasswordTextBox_MouseEnter);
            this.createPasswordTextBox.MouseLeave += new System.EventHandler(this.createPasswordTextBox_MouseLeave);
            // 
            // createUsernameTextBox
            // 
            this.createUsernameTextBox.Location = new System.Drawing.Point(15, 47);
            this.createUsernameTextBox.Name = "createUsernameTextBox";
            this.createUsernameTextBox.Size = new System.Drawing.Size(218, 20);
            this.createUsernameTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Username:";
            // 
            // createLoginButton
            // 
            this.createLoginButton.Location = new System.Drawing.Point(158, 322);
            this.createLoginButton.Name = "createLoginButton";
            this.createLoginButton.Size = new System.Drawing.Size(75, 23);
            this.createLoginButton.TabIndex = 10;
            this.createLoginButton.Text = "Create";
            this.createLoginButton.UseVisualStyleBackColor = true;
            this.createLoginButton.Click += new System.EventHandler(this.CreateLoginButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(523, 9);
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
            this.moveWindowPanel.Location = new System.Drawing.Point(2, 1);
            this.moveWindowPanel.Name = "moveWindowPanel";
            this.moveWindowPanel.Size = new System.Drawing.Size(512, 27);
            this.moveWindowPanel.TabIndex = 29;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseDown);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseMove);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseUp);
            // 
            // newUserButton
            // 
            this.newUserButton.Location = new System.Drawing.Point(19, 149);
            this.newUserButton.Name = "newUserButton";
            this.newUserButton.Size = new System.Drawing.Size(218, 23);
            this.newUserButton.TabIndex = 3;
            this.newUserButton.Text = "New User";
            this.newUserButton.UseVisualStyleBackColor = true;
            this.newUserButton.Click += new System.EventHandler(this.newUserButton_Click);
            // 
            // newPasswordHelpLabel
            // 
            this.newPasswordHelpLabel.AutoSize = true;
            this.newPasswordHelpLabel.Location = new System.Drawing.Point(12, 259);
            this.newPasswordHelpLabel.MaximumSize = new System.Drawing.Size(250, 0);
            this.newPasswordHelpLabel.Name = "newPasswordHelpLabel";
            this.newPasswordHelpLabel.Size = new System.Drawing.Size(35, 13);
            this.newPasswordHelpLabel.TabIndex = 53;
            this.newPasswordHelpLabel.Text = "label9";
            // 
            // LoginView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(549, 400);
            this.Controls.Add(this.newPasswordHelpLabel);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginView";
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginView_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox loginUsernameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button createLoginButton;
        private System.Windows.Forms.TextBox loginPasswordTextBox;
        private System.Windows.Forms.Button generatePasswordButton;
        private System.Windows.Forms.TextBox createPasswordTextBox;
        private System.Windows.Forms.TextBox createUsernameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.Panel moveWindowPanel;
        private System.Windows.Forms.Label loginResultLabel;
        private System.Windows.Forms.Label createNewUserResultLabel;
        private System.Windows.Forms.TextBox createEmailTextBox;
        private System.Windows.Forms.TextBox createPhoneNumberTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox createLastNameTextBox;
        private System.Windows.Forms.TextBox createFirstNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button newUserButton;
        private System.Windows.Forms.Label newPasswordHelpLabel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}