using ImageEncryptor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ImageEncryptorUI
{
    public partial class ImageEncryptorUI : Form
    {
        Bitmap current = null;
        public List<string> files = new List<string>();

        public ImageEncryptorUI()
        {
            InitializeComponent();
            AllowDrop = true;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "png files (*.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                current = new Bitmap(ofd.FileName);
                LoadImage();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "png files (*.png)|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImgEnc.Encrypt(current, files.ToArray(), sfd.FileName);
                    MessageBox.Show("Done!", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Something went wrong with saving the file!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                files.AddRange(ofd.FileNames);
                btnSave.Enabled = true;
            }
        }

        private void btnExtractFiles_Click(object sender, EventArgs e)
        {
            try
            {
                var decrypted = ImgEnc.Decrypt(current);
                FileList fl = new FileList(decrypted);
                fl.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something went wrong with decrypting the file!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadImage()
        {
            pbImg.Image = current;
            try
            {
                bool encrypted = ImgEnc.IsEncryptedImage(current);
                if (encrypted)
                {
                    btnExtractFiles.Enabled = true;
                    btnAddFile.Enabled = false;
                    btnSave.Enabled = false;
                    btnLoad.Enabled = true;
                }
                else
                {
                    btnExtractFiles.Enabled = false;
                    btnAddFile.Enabled = true;
                    btnSave.Enabled = false;
                    btnLoad.Enabled = true;
                    files = new List<string>();
                }
            }
            catch (Exception ex)
            {
                btnExtractFiles.Enabled = false;
                btnAddFile.Enabled = false;
                btnSave.Enabled = false;
                btnLoad.Enabled = true;
                MessageBox.Show("Something went wrong with loading the file!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImageEncryptorUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            string[] files = Directory.GetFiles(path).Where(x => x.Contains(FileList.TempFileName)).ToArray();
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}
