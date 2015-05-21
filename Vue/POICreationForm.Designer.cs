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
            this.nameSearchTextBox = new System.Windows.Forms.TextBox();
            this.listeBoxPersonnes = new System.Windows.Forms.ListBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.prenomLabel = new System.Windows.Forms.Label();
            this.dateNaissanceLabel = new System.Windows.Forms.Label();
            this.chargementPictureBox = new System.Windows.Forms.PictureBox();
            this.initialeLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.prenomTextBox = new System.Windows.Forms.TextBox();
            this.initialeTextBox = new System.Windows.Forms.TextBox();
            this.dateNaissanceTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chargementPictureBox)).BeginInit();
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
            // findNameLabel
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
            this.nameSearchTextBox.Location = new System.Drawing.Point(110, 6);
            this.nameSearchTextBox.Name = "nameTextBox";
            this.nameSearchTextBox.Size = new System.Drawing.Size(152, 20);
            this.nameSearchTextBox.TabIndex = 3;
            this.nameSearchTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // listeBoxPersonnes
            // 
            this.listeBoxPersonnes.FormattingEnabled = true;
            this.listeBoxPersonnes.Location = new System.Drawing.Point(299, 6);
            this.listeBoxPersonnes.Name = "listeBoxPersonnes";
            this.listeBoxPersonnes.Size = new System.Drawing.Size(165, 212);
            this.listeBoxPersonnes.TabIndex = 4;
            this.listeBoxPersonnes.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(18, 54);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Nom : ";
            // 
            // prenomLabel
            // 
            this.prenomLabel.AutoSize = true;
            this.prenomLabel.Location = new System.Drawing.Point(18, 88);
            this.prenomLabel.Name = "prenomLabel";
            this.prenomLabel.Size = new System.Drawing.Size(52, 13);
            this.prenomLabel.TabIndex = 6;
            this.prenomLabel.Text = "Prénom : ";
            // 
            // dateNaissanceLabel
            // 
            this.dateNaissanceLabel.AutoSize = true;
            this.dateNaissanceLabel.Location = new System.Drawing.Point(18, 154);
            this.dateNaissanceLabel.Name = "dateNaissanceLabel";
            this.dateNaissanceLabel.Size = new System.Drawing.Size(105, 13);
            this.dateNaissanceLabel.TabIndex = 7;
            this.dateNaissanceLabel.Text = "Date de naissance : ";
            // 
            // pictureBox1
            // 
            this.chargementPictureBox.Image = global::Vue.Properties.Resources.loading;
            this.chargementPictureBox.Location = new System.Drawing.Point(354, 67);
            this.chargementPictureBox.Name = "pictureBox1";
            this.chargementPictureBox.Size = new System.Drawing.Size(64, 64);
            this.chargementPictureBox.TabIndex = 8;
            this.chargementPictureBox.TabStop = false;
            this.chargementPictureBox.Visible = false;
            // 
            // initialeLabel
            // 
            this.initialeLabel.AutoSize = true;
            this.initialeLabel.Location = new System.Drawing.Point(18, 122);
            this.initialeLabel.Name = "initialeLabel";
            this.initialeLabel.Size = new System.Drawing.Size(81, 13);
            this.initialeLabel.TabIndex = 9;
            this.initialeLabel.Text = "Initiale prénom :";
            // 
            // textBox1
            // 
            this.nameTextBox.Location = new System.Drawing.Point(132, 51);
            this.nameTextBox.Name = "textBox1";
            this.nameTextBox.Size = new System.Drawing.Size(130, 20);
            this.nameTextBox.TabIndex = 10;
            // 
            // textBox2
            // 
            this.prenomTextBox.Location = new System.Drawing.Point(132, 85);
            this.prenomTextBox.Name = "textBox2";
            this.prenomTextBox.Size = new System.Drawing.Size(130, 20);
            this.prenomTextBox.TabIndex = 11;
            // 
            // textBox3
            // 
            this.initialeTextBox.Location = new System.Drawing.Point(132, 119);
            this.initialeTextBox.Name = "textBox3";
            this.initialeTextBox.Size = new System.Drawing.Size(130, 20);
            this.initialeTextBox.TabIndex = 12;
            // 
            // textBox4
            // 
            this.dateNaissanceTextBox.Location = new System.Drawing.Point(132, 151);
            this.dateNaissanceTextBox.Name = "textBox4";
            this.dateNaissanceTextBox.Size = new System.Drawing.Size(130, 20);
            this.dateNaissanceTextBox.TabIndex = 13;
            // 
            // POICreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 262);
            this.Controls.Add(this.dateNaissanceTextBox);
            this.Controls.Add(this.initialeTextBox);
            this.Controls.Add(this.prenomTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.initialeLabel);
            this.Controls.Add(this.chargementPictureBox);
            this.Controls.Add(this.dateNaissanceLabel);
            this.Controls.Add(this.prenomLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.listeBoxPersonnes);
            this.Controls.Add(this.nameSearchTextBox);
            this.Controls.Add(this.findNameLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "POICreationForm";
            this.Text = "Ajout d\'un lien";
            ((System.ComponentModel.ISupportInitialize)(this.chargementPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label findNameLabel;
        private System.Windows.Forms.TextBox nameSearchTextBox;
        private System.Windows.Forms.ListBox listeBoxPersonnes;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label prenomLabel;
        private System.Windows.Forms.Label dateNaissanceLabel;
        private System.Windows.Forms.PictureBox chargementPictureBox;
        private System.Windows.Forms.Label initialeLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox prenomTextBox;
        private System.Windows.Forms.TextBox initialeTextBox;
        private System.Windows.Forms.TextBox dateNaissanceTextBox;
    }

}