namespace PasswordVault
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
            this.createNewUserResultLabel = new System.Windows.Forms.Label();
            this.generatePasswordButton = new System.Windows.Forms.Button();
            this.createPasswordTextBox = new System.Windows.Forms.TextBox();
            this.createUsernameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.createLoginButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Label();
            this.moveWindowPanel = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.loginResultLabel);
            this.groupBox1.Controls.Add(this.loginPasswordTextBox);
            this.groupBox1.Controls.Add(this.loginUsernameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.loginButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login";
            // 
            // loginResultLabel
            // 
            this.loginResultLabel.AutoSize = true;
            this.loginResultLabel.Location = new System.Drawing.Point(16, 150);
            this.loginResultLabel.Name = "loginResultLabel";
            this.loginResultLabel.Size = new System.Drawing.Size(69, 13);
            this.loginResultLabel.TabIndex = 5;
            this.loginResultLabel.Text = "Login Result:";
            // 
            // loginPasswordTextBox
            // 
            this.loginPasswordTextBox.Location = new System.Drawing.Point(19, 100);
            this.loginPasswordTextBox.Name = "loginPasswordTextBox";
            this.loginPasswordTextBox.PasswordChar = '•';
            this.loginPasswordTextBox.Size = new System.Drawing.Size(218, 20);
            this.loginPasswordTextBox.TabIndex = 4;
            // 
            // loginUsernameTextBox
            // 
            this.loginUsernameTextBox.Location = new System.Drawing.Point(19, 47);
            this.loginUsernameTextBox.Name = "loginUsernameTextBox";
            this.loginUsernameTextBox.Size = new System.Drawing.Size(218, 20);
            this.loginUsernameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username:";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(162, 145);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.createNewUserResultLabel);
            this.groupBox2.Controls.Add(this.generatePasswordButton);
            this.groupBox2.Controls.Add(this.createPasswordTextBox);
            this.groupBox2.Controls.Add(this.createUsernameTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.createLoginButton);
            this.groupBox2.Location = new System.Drawing.Point(274, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 190);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create New User";
            // 
            // createNewUserResultLabel
            // 
            this.createNewUserResultLabel.AutoSize = true;
            this.createNewUserResultLabel.Location = new System.Drawing.Point(12, 126);
            this.createNewUserResultLabel.Name = "createNewUserResultLabel";
            this.createNewUserResultLabel.Size = new System.Drawing.Size(99, 13);
            this.createNewUserResultLabel.TabIndex = 10;
            this.createNewUserResultLabel.Text = "Create User Result:";
            // 
            // generatePasswordButton
            // 
            this.generatePasswordButton.Location = new System.Drawing.Point(15, 145);
            this.generatePasswordButton.Name = "generatePasswordButton";
            this.generatePasswordButton.Size = new System.Drawing.Size(137, 23);
            this.generatePasswordButton.TabIndex = 9;
            this.generatePasswordButton.Text = "Generate Password";
            this.generatePasswordButton.UseVisualStyleBackColor = true;
            this.generatePasswordButton.Click += new System.EventHandler(this.GeneratePasswordButton_Click);
            // 
            // createPasswordTextBox
            // 
            this.createPasswordTextBox.Location = new System.Drawing.Point(15, 100);
            this.createPasswordTextBox.Name = "createPasswordTextBox";
            this.createPasswordTextBox.Size = new System.Drawing.Size(218, 20);
            this.createPasswordTextBox.TabIndex = 8;
            this.createPasswordTextBox.TextChanged += new System.EventHandler(this.CreatePasswordTextBox_TextChanged);
            // 
            // createUsernameTextBox
            // 
            this.createUsernameTextBox.Location = new System.Drawing.Point(15, 47);
            this.createUsernameTextBox.Name = "createUsernameTextBox";
            this.createUsernameTextBox.Size = new System.Drawing.Size(218, 20);
            this.createUsernameTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Username:";
            // 
            // createLoginButton
            // 
            this.createLoginButton.Location = new System.Drawing.Point(158, 145);
            this.createLoginButton.Name = "createLoginButton";
            this.createLoginButton.Size = new System.Drawing.Size(75, 23);
            this.createLoginButton.TabIndex = 1;
            this.createLoginButton.Text = "Create";
            this.createLoginButton.UseVisualStyleBackColor = true;
            this.createLoginButton.Click += new System.EventHandler(this.CreateLoginButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(524, 9);
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
            this.moveWindowPanel.Size = new System.Drawing.Size(516, 27);
            this.moveWindowPanel.TabIndex = 29;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseDown);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseMove);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseUp);
            // 
            // LoginView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(550, 236);
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
    }
}