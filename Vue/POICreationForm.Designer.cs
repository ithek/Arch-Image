namespace Vue
{
    public partial class POICreationForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.findNameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.listeBoxPersonnes = new System.Windows.Forms.ListBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.prenomLabel = new System.Windows.Forms.Label();
            this.dateNaissanceLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(180, 224);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(92, 26);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 224);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(92, 26);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Annuler";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // nameLabel
            // 
            this.findNameLabel.AutoSize = true;
            this.findNameLabel.Location = new System.Drawing.Point(18, 9);
            this.findNameLabel.Name = "findNameLabel";
            this.findNameLabel.Size = new System.Drawing.Size(86, 13);
            this.findNameLabel.TabIndex = 2;
            this.findNameLabel.Text = "Rechercher nom";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(110, 6);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(120, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // listeBoxPersonnes
            // 
            this.listeBoxPersonnes.FormattingEnabled = true;
            this.listeBoxPersonnes.Location = new System.Drawing.Point(349, 6);
            this.listeBoxPersonnes.Size = new System.Drawing.Size(165, 212);
            this.listeBoxPersonnes.TabIndex = 4;
            this.listeBoxPersonnes.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(21, 50);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Nom : ";
            // 
            // prenomLabel
            // 
            this.prenomLabel.AutoSize = true;
            this.prenomLabel.Location = new System.Drawing.Point(21, 67);
            this.prenomLabel.Name = "prenomLabel";
            this.prenomLabel.Size = new System.Drawing.Size(52, 13);
            this.prenomLabel.TabIndex = 6;
            this.prenomLabel.Text = "Prénom : ";
            // 
            // dateNaissanceLabel
            // 
            this.dateNaissanceLabel.AutoSize = true;
            this.dateNaissanceLabel.Location = new System.Drawing.Point(21, 84);
            this.dateNaissanceLabel.Name = "dateNaissanceLabel";
            this.dateNaissanceLabel.Size = new System.Drawing.Size(105, 13);
            this.dateNaissanceLabel.TabIndex = 7;
            this.dateNaissanceLabel.Text = "Date de naissance : ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(246, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Chercher";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // POICreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateNaissanceLabel);
            this.Controls.Add(this.prenomLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.listeBoxPersonnes);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.findNameLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "POICreationForm";
            this.Text = "Ajout d\'un lien";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label findNameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ListBox listeBoxPersonnes;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label prenomLabel;
        private System.Windows.Forms.Label dateNaissanceLabel;
        private System.Windows.Forms.Button button1;
    }
}