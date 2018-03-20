using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FixedAssetBarcodeUI.Dialogs
{
    public partial class frmPathSelector : Form
    {
        public frmPathSelector()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowseDocLoc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdlg = new FolderBrowserDialog();
            fdlg.ShowNewFolderButton = true;
            
            if(fdlg.ShowDialog() == DialogResult.OK)
            {
                txtDocumentLoc.Text = fdlg.SelectedPath + "\\";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Properties.Settings.Default.documentPath = txtDocumentLoc.Text;
            this.Close();
        }
    }
}
