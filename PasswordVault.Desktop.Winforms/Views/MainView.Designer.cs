﻿using System.Drawing;
using System.Windows.Forms;

namespace PasswordVault.Desktop.Winforms
{
    partial class MainView
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

            if (passwordContextMenuStrip != null)
            {
                passwordContextMenuStrip.Dispose();
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
            this.menuStrip = new PasswordVault.Desktop.Winforms.AdvancedMenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportPasswordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPasswordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordDataGridView = new System.Windows.Forms.DataGridView();
            this.addButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.applicationTextBox = new System.Windows.Forms.TextBox();
            this.websiteTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.passphraseTextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Label();
            this.moveWindowPanel = new System.Windows.Forms.Panel();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.editCancelButton = new System.Windows.Forms.Button();
            this.filterComboBox = new PasswordVault.Desktop.Winforms.AdvancedComboBox();
            this.minimizeButton = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.clearFilterButton = new System.Windows.Forms.Button();
            this.userStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.label7 = new System.Windows.Forms.Label();
            this.passwordCountLabel = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordDataGridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.AllowMerge = false;
            this.menuStrip.AutoSize = false;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MenuItemBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.menuStrip.MenuItemSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(174, 30);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exportPasswordsToolStripMenuItem,
            this.importPasswordsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 26);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 6);
            // 
            // exportPasswordsToolStripMenuItem
            // 
            this.exportPasswordsToolStripMenuItem.Name = "exportPasswordsToolStripMenuItem";
            this.exportPasswordsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.exportPasswordsToolStripMenuItem.Text = "Export Passwords";
            this.exportPasswordsToolStripMenuItem.Click += new System.EventHandler(this.exportPasswordsToolStripMenuItem_Click);
            // 
            // importPasswordsToolStripMenuItem
            // 
            this.importPasswordsToolStripMenuItem.Name = "importPasswordsToolStripMenuItem";
            this.importPasswordsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.importPasswordsToolStripMenuItem.Text = "Import Passwords";
            this.importPasswordsToolStripMenuItem.Click += new System.EventHandler(this.importPasswordsToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 26);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // accountToolStripMenuItem
            // 
            this.accountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.changePasswordToolStripMenuItem});
            this.accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            this.accountToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.accountToolStripMenuItem.Text = "Account";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.changePasswordToolStripMenuItem.Text = "Change Password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click_1);
            // 
            // passwordDataGridView
            // 
            this.passwordDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.passwordDataGridView.Location = new System.Drawing.Point(12, 145);
            this.passwordDataGridView.Name = "passwordDataGridView";
            this.passwordDataGridView.Size = new System.Drawing.Size(730, 290);
            this.passwordDataGridView.TabIndex = 1;
            this.passwordDataGridView.SelectionChanged += new System.EventHandler(this.PasswordDataGridView_SelectionChanged);
            this.passwordDataGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PasswordDataGridView_MouseUp);
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(748, 98);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 26;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // editButton
            // 
            this.editButton.Enabled = false;
            this.editButton.Location = new System.Drawing.Point(748, 163);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(75, 23);
            this.editButton.TabIndex = 5;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // applicationTextBox
            // 
            this.applicationTextBox.Enabled = false;
            this.applicationTextBox.Location = new System.Drawing.Point(12, 72);
            this.applicationTextBox.Name = "applicationTextBox";
            this.applicationTextBox.Size = new System.Drawing.Size(129, 20);
            this.applicationTextBox.TabIndex = 20;
            // 
            // websiteTextBox
            // 
            this.websiteTextBox.Enabled = false;
            this.websiteTextBox.Location = new System.Drawing.Point(560, 72);
            this.websiteTextBox.Name = "websiteTextBox";
            this.websiteTextBox.Size = new System.Drawing.Size(116, 20);
            this.websiteTextBox.TabIndex = 24;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Enabled = false;
            this.descriptionTextBox.Location = new System.Drawing.Point(421, 72);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(133, 20);
            this.descriptionTextBox.TabIndex = 23;
            // 
            // passphraseTextBox
            // 
            this.passphraseTextBox.Enabled = false;
            this.passphraseTextBox.Location = new System.Drawing.Point(682, 72);
            this.passphraseTextBox.Name = "passphraseTextBox";
            this.passphraseTextBox.Size = new System.Drawing.Size(141, 20);
            this.passphraseTextBox.TabIndex = 25;
            this.passphraseTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passphraseTextBox_KeyDown);
            this.passphraseTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PassphraseTextBox_KeyUp);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(748, 192);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 15;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Enabled = false;
            this.usernameTextBox.Location = new System.Drawing.Point(147, 72);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(129, 20);
            this.usernameTextBox.TabIndex = 21;
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(809, 9);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(14, 13);
            this.closeButton.TabIndex = 27;
            this.closeButton.Text = "X";
            this.closeButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CloseButton_MouseClick);
            this.closeButton.MouseEnter += new System.EventHandler(this.CloseButton_MouseEnter);
            this.closeButton.MouseLeave += new System.EventHandler(this.CloseButton_MouseLeave);
            // 
            // moveWindowPanel
            // 
            this.moveWindowPanel.Location = new System.Drawing.Point(177, 0);
            this.moveWindowPanel.Name = "moveWindowPanel";
            this.moveWindowPanel.Size = new System.Drawing.Size(600, 53);
            this.moveWindowPanel.TabIndex = 28;
            this.moveWindowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseDown);
            this.moveWindowPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseMove);
            this.moveWindowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindowPanel_MouseUp);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Enabled = false;
            this.filterTextBox.Location = new System.Drawing.Point(12, 119);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(264, 20);
            this.filterTextBox.TabIndex = 30;
            this.filterTextBox.TextChanged += new System.EventHandler(this.filterChanged);
            // 
            // editCancelButton
            // 
            this.editCancelButton.Enabled = false;
            this.editCancelButton.Location = new System.Drawing.Point(667, 98);
            this.editCancelButton.Name = "editCancelButton";
            this.editCancelButton.Size = new System.Drawing.Size(75, 23);
            this.editCancelButton.TabIndex = 31;
            this.editCancelButton.Text = "Cancel";
            this.editCancelButton.UseVisualStyleBackColor = true;
            this.editCancelButton.Click += new System.EventHandler(this.EditCancelButton_Click);
            // 
            // filterComboBox
            // 
            this.filterComboBox.BorderColor = System.Drawing.Color.Black;
            this.filterComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.filterComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.HighlightColor = System.Drawing.Color.Gray;
            this.filterComboBox.Location = new System.Drawing.Point(282, 119);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(121, 21);
            this.filterComboBox.TabIndex = 0;
            this.filterComboBox.TextChanged += new System.EventHandler(this.filterChanged);
            // 
            // minimizeButton
            // 
            this.minimizeButton.AutoSize = true;
            this.minimizeButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.minimizeButton.Location = new System.Drawing.Point(789, 9);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(10, 13);
            this.minimizeButton.TabIndex = 32;
            this.minimizeButton.Text = "-";
            this.minimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            this.minimizeButton.MouseEnter += new System.EventHandler(this.MinimizeButton_MouseEnter);
            this.minimizeButton.MouseLeave += new System.EventHandler(this.MinimizeButton_MouseLeave);
            // 
            // emailTextBox
            // 
            this.emailTextBox.Enabled = false;
            this.emailTextBox.Location = new System.Drawing.Point(282, 72);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(133, 20);
            this.emailTextBox.TabIndex = 22;
            // 
            // clearFilterButton
            // 
            this.clearFilterButton.Enabled = false;
            this.clearFilterButton.Location = new System.Drawing.Point(409, 118);
            this.clearFilterButton.Name = "clearFilterButton";
            this.clearFilterButton.Size = new System.Drawing.Size(75, 23);
            this.clearFilterButton.TabIndex = 35;
            this.clearFilterButton.Text = "Clear Filter";
            this.clearFilterButton.UseVisualStyleBackColor = true;
            this.clearFilterButton.Click += new System.EventHandler(this.clearFilterButton_Click);
            // 
            // userStatusLabel
            // 
            this.userStatusLabel.Name = "userStatusLabel";
            this.userStatusLabel.Size = new System.Drawing.Size(61, 17);
            this.userStatusLabel.Text = "userStatus";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 462);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(835, 22);
            this.statusStrip1.TabIndex = 26;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 438);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Count:";
            // 
            // passwordCountLabel
            // 
            this.passwordCountLabel.AutoSize = true;
            this.passwordCountLabel.Location = new System.Drawing.Point(55, 438);
            this.passwordCountLabel.Name = "passwordCountLabel";
            this.passwordCountLabel.Size = new System.Drawing.Size(13, 13);
            this.passwordCountLabel.TabIndex = 37;
            this.passwordCountLabel.Text = "0";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(835, 484);
            this.Controls.Add(this.passwordCountLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.clearFilterButton);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.editCancelButton);
            this.Controls.Add(this.filterComboBox);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.moveWindowPanel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.passphraseTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.websiteTextBox);
            this.Controls.Add(this.applicationTextBox);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.passwordDataGridView);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainView";
            this.Text = "Password Vault";
            this.SizeChanged += new System.EventHandler(this.MainView_SizeChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordDataGridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AdvancedMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.DataGridView passwordDataGridView;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.TextBox applicationTextBox;
        private System.Windows.Forms.TextBox websiteTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox passphraseTextBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.Panel moveWindowPanel;
        private System.Windows.Forms.TextBox filterTextBox;
        private AdvancedComboBox filterComboBox;
        private Button editCancelButton;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Label minimizeButton;
        private TextBox emailTextBox;
        private ToolStripMenuItem accountToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem changePasswordToolStripMenuItem;
        private Button clearFilterButton;
        private ToolStripStatusLabel userStatusLabel;
        private StatusStrip statusStrip1;
        private Label label7;
        private Label passwordCountLabel;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exportPasswordsToolStripMenuItem;
        private ToolStripMenuItem importPasswordsToolStripMenuItem;
    }
}

