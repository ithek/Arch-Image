namespace Vue
{
    partial class InscriptionForm
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
            this.identifiantLabel = new System.Windows.Forms.Label();
            this.mailLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.identifiantTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // identifiantLabel
            // 
            this.identifiantLabel.AutoSize = true;
            this.identifiantLabel.Location = new System.Drawing.Point(14, 15);
            this.identifiantLabel.Name = "identifiantLabel";
            this.identifiantLabel.Size = new System.Drawing.Size(53, 13);
            this.identifiantLabel.TabIndex = 0;
            this.identifiantLabel.Text = "Identifiant";
            this.identifiantLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // mailLabel
            // 
            this.mailLabel.AutoSize = true;
            this.mailLabel.Location = new System.Drawing.Point(14, 94);
            this.mailLabel.Name = "mailLabel";
            this.mailLabel.Size = new System.Drawing.Size(66, 13);
            this.mailLabel.TabIndex = 1;
            this.mailLabel.Text = "Adresse mail";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(14, 45);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(71, 13);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Mot de passe";
            // 
            // identifiantTextBox
            // 
            this.identifiantTextBox.Location = new System.Drawing.Point(110, 12);
            this.identifiantTextBox.Name = "identifiantTextBox";
            this.identifiantTextBox.Size = new System.Drawing.Size(166, 20);
            this.identifiantTextBox.TabIndex = 3;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(110, 42);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(166, 20);
            this.passwordTextBox.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(110, 91);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(166, 20);
            this.textBox2.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(32, 131);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(98, 25);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "Confirmer";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(178, 131);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(98, 25);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Annuler";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // InscriptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 174);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.identifiantTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.mailLabel);
            this.Controls.Add(this.identifiantLabel);
            this.Name = "InscriptionForm";
            this.Text = "Créer un compte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label identifiantLabel;
        private System.Windows.Forms.Label mailLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox identifiantTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}