﻿namespace Vue
{
    partial class ConnexionForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.identifiantTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.identifiantLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(178, 82);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(98, 25);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "Annuler";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(32, 82);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(98, 25);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "Se connecter";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(110, 42);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(166, 20);
            this.passwordTextBox.TabIndex = 12;
            // 
            // identifiantTextBox
            // 
            this.identifiantTextBox.Location = new System.Drawing.Point(110, 12);
            this.identifiantTextBox.Name = "identifiantTextBox";
            this.identifiantTextBox.Size = new System.Drawing.Size(166, 20);
            this.identifiantTextBox.TabIndex = 11;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(14, 45);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(71, 13);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Mot de passe";
            // 
            // identifiantLabel
            // 
            this.identifiantLabel.AutoSize = true;
            this.identifiantLabel.Location = new System.Drawing.Point(14, 15);
            this.identifiantLabel.Name = "identifiantLabel";
            this.identifiantLabel.Size = new System.Drawing.Size(53, 13);
            this.identifiantLabel.TabIndex = 8;
            this.identifiantLabel.Text = "Identifiant";
            // 
            // ConnexionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 130);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.identifiantTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.identifiantLabel);
            this.Name = "ConnexionForm";
            this.Text = "Connexion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox identifiantTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label identifiantLabel;
    }
}