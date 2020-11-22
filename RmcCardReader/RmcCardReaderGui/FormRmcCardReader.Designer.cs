namespace RmcCardReaderGui {
    partial class FormRmcCardReader {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRmcCardReader));
            this.PictureBoxCard = new System.Windows.Forms.PictureBox();
            this.ButtonGo = new System.Windows.Forms.Button();
            this.TextBoxResult = new System.Windows.Forms.TextBox();
            this.TextBoxBitString = new System.Windows.Forms.TextBox();
            this.pbHeader = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxBitStringText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxCard
            // 
            this.PictureBoxCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxCard.BackColor = System.Drawing.Color.Silver;
            this.PictureBoxCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBoxCard.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxCard.Image")));
            this.PictureBoxCard.Location = new System.Drawing.Point(12, 162);
            this.PictureBoxCard.Name = "PictureBoxCard";
            this.PictureBoxCard.Size = new System.Drawing.Size(472, 652);
            this.PictureBoxCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxCard.TabIndex = 0;
            this.PictureBoxCard.TabStop = false;
            this.PictureBoxCard.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBoxCard_DragDrop);
            this.PictureBoxCard.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxCard_DragEnter);
            // 
            // ButtonGo
            // 
            this.ButtonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonGo.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.ButtonGo.Location = new System.Drawing.Point(502, 162);
            this.ButtonGo.Name = "ButtonGo";
            this.ButtonGo.Size = new System.Drawing.Size(600, 54);
            this.ButtonGo.TabIndex = 1;
            this.ButtonGo.Text = "Run OCR using Azure Cognitive Services";
            this.ButtonGo.UseVisualStyleBackColor = true;
            this.ButtonGo.Click += new System.EventHandler(this.ButtonGo_Click);
            // 
            // TextBoxResult
            // 
            this.TextBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxResult.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.TextBoxResult.Location = new System.Drawing.Point(502, 656);
            this.TextBoxResult.Multiline = true;
            this.TextBoxResult.Name = "TextBoxResult";
            this.TextBoxResult.ReadOnly = true;
            this.TextBoxResult.Size = new System.Drawing.Size(600, 158);
            this.TextBoxResult.TabIndex = 2;
            // 
            // TextBoxBitString
            // 
            this.TextBoxBitString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxBitString.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxBitString.Location = new System.Drawing.Point(502, 257);
            this.TextBoxBitString.Multiline = true;
            this.TextBoxBitString.Name = "TextBoxBitString";
            this.TextBoxBitString.Size = new System.Drawing.Size(485, 362);
            this.TextBoxBitString.TabIndex = 3;
            this.TextBoxBitString.TextChanged += new System.EventHandler(this.TextBoxBitString_TextChanged);
            // 
            // pbHeader
            // 
            this.pbHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbHeader.Image = ((System.Drawing.Image)(resources.GetObject("pbHeader.Image")));
            this.pbHeader.Location = new System.Drawing.Point(-1, -2);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(1116, 152);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHeader.TabIndex = 4;
            this.pbHeader.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(499, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(428, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "OCR Result (modify manually to fix errors)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(499, 635);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Text Result";
            // 
            // TextBoxBitStringText
            // 
            this.TextBoxBitStringText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxBitStringText.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxBitStringText.Location = new System.Drawing.Point(1004, 257);
            this.TextBoxBitStringText.Multiline = true;
            this.TextBoxBitStringText.Name = "TextBoxBitStringText";
            this.TextBoxBitStringText.ReadOnly = true;
            this.TextBoxBitStringText.Size = new System.Drawing.Size(98, 362);
            this.TextBoxBitStringText.TabIndex = 7;
            // 
            // FormRmcCardReader
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 826);
            this.Controls.Add(this.TextBoxBitStringText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PictureBoxCard);
            this.Controls.Add(this.pbHeader);
            this.Controls.Add(this.TextBoxBitString);
            this.Controls.Add(this.TextBoxResult);
            this.Controls.Add(this.ButtonGo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormRmcCardReader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[RMC - The Cave] Retro Tea Breaks Collectors Card Decode Tool by HAG\'S LAB";
            this.Load += new System.EventHandler(this.FormRmcCardReader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxCard;
        private System.Windows.Forms.Button ButtonGo;
        private System.Windows.Forms.TextBox TextBoxResult;
        private System.Windows.Forms.TextBox TextBoxBitString;
        private System.Windows.Forms.PictureBox pbHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxBitStringText;
    }
}

