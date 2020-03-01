<<<<<<< HEAD
﻿namespace ImageEncryptorUI
{
    partial class ImageEncryptorUI
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
            this.pbImg = new System.Windows.Forms.PictureBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnExtractFiles = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImg
            // 
            this.pbImg.Location = new System.Drawing.Point(12, 12);
            this.pbImg.Name = "pbImg";
            this.pbImg.Size = new System.Drawing.Size(156, 156);
            this.pbImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImg.TabIndex = 0;
            this.pbImg.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(11, 178);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load Image";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Enabled = false;
            this.btnAddFile.Location = new System.Drawing.Point(11, 207);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(156, 23);
            this.btnAddFile.TabIndex = 2;
            this.btnAddFile.Text = "Add File To Image";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnExtractFiles
            // 
            this.btnExtractFiles.Enabled = false;
            this.btnExtractFiles.Location = new System.Drawing.Point(11, 236);
            this.btnExtractFiles.Name = "btnExtractFiles";
            this.btnExtractFiles.Size = new System.Drawing.Size(156, 23);
            this.btnExtractFiles.TabIndex = 3;
            this.btnExtractFiles.Text = "Extract Image Files";
            this.btnExtractFiles.UseVisualStyleBackColor = true;
            this.btnExtractFiles.Click += new System.EventHandler(this.btnExtractFiles_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(92, 178);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save Image";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ImageEncryptorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 269);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExtractFiles);
            this.Controls.Add(this.btnAddFile);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pbImg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ImageEncryptorUI";
            this.Text = "Image Enc/Dec";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageEncryptorUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImg;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnExtractFiles;
        private System.Windows.Forms.Button btnSave;
    }
}

=======
﻿namespace ImageEncryptorUI
{
    partial class ImageEncryptorUI
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
            this.pbImg = new System.Windows.Forms.PictureBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnExtractFiles = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImg
            // 
            this.pbImg.Location = new System.Drawing.Point(12, 12);
            this.pbImg.Name = "pbImg";
            this.pbImg.Size = new System.Drawing.Size(156, 156);
            this.pbImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImg.TabIndex = 0;
            this.pbImg.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(11, 178);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load Image";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Enabled = false;
            this.btnAddFile.Location = new System.Drawing.Point(11, 207);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(156, 23);
            this.btnAddFile.TabIndex = 2;
            this.btnAddFile.Text = "Add File To Image";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnExtractFiles
            // 
            this.btnExtractFiles.Enabled = false;
            this.btnExtractFiles.Location = new System.Drawing.Point(11, 236);
            this.btnExtractFiles.Name = "btnExtractFiles";
            this.btnExtractFiles.Size = new System.Drawing.Size(156, 23);
            this.btnExtractFiles.TabIndex = 3;
            this.btnExtractFiles.Text = "Extract Image Files";
            this.btnExtractFiles.UseVisualStyleBackColor = true;
            this.btnExtractFiles.Click += new System.EventHandler(this.btnExtractFiles_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(92, 178);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save Image";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ImageEncryptorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 269);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExtractFiles);
            this.Controls.Add(this.btnAddFile);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pbImg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ImageEncryptorUI";
            this.Text = "Image Encryptor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageEncryptorUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImg;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnExtractFiles;
        private System.Windows.Forms.Button btnSave;
    }
}

>>>>>>> 80e04306c79b1eb4deca022bd33980fd985d4a9a
