using ImageEncryptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEncryptorUI
{
    public partial class FileList : Form
    {
        public const string TempFileName = "ImgEncTemp";

        public FileList(List<DecryptedFile> files)
        {
            InitializeComponent();
            foreach(DecryptedFile file in files)
            {
                lstFiles.Items.Add(file);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void lstFiles_DoubleClick(object sender, EventArgs e)
        {
            Open();
        }

        void Open()
        {
            if (lstFiles.SelectedItem != null)
            {
                RunFile((DecryptedFile)lstFiles.SelectedItem);
            }
        }

        void RunFile(DecryptedFile file)
        {
            string path = $"{TempFileName}.{file.Extention}";
            File.WriteAllBytes(path, file.Data);
            System.Diagnostics.Process.Start(path);
        }
    }
}
