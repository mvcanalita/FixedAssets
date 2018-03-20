using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FixedAssetBarcodeUI.Properties;

namespace FixedAssetBarcodeUI.Dialogs
{
    public partial class frmSettings : Form
    {
        #region Constants
        // Dropshadow
        private const int CS_DROPSHADOW = 0x00020000;
        // End Dropshadow
        #endregion
        #region Overrides
        //Drop Shadow Create Parameters
        protected override CreateParams CreateParams
        {
            get
            {
                // add the drop shadow flag for automatically drawing
                // a drop shadow around the form
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion
        public frmSettings()
        {
            InitializeComponent();
        }

        private void chkDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDefault.Checked)
            {
                txtPrinterName.Enabled = true;
            }else
            {
                txtPrinterName.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            loadAppSettings();
        }

        protected void loadAppSettings()
        {
            chkDefault.Checked = Properties.Settings.Default.useDefaultPrinter;
            txtDocumentLocation.Text = Properties.Settings.Default.documentPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowseDoclocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdlg = new FolderBrowserDialog();
            fdlg.ShowNewFolderButton = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                txtDocumentLocation.Text = fdlg.SelectedPath + "\\";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default["printerName"] = txtPrinterName.Text == string.Empty && chkDefault.Checked == true ? Properties.Settings.Default.printerName : txtPrinterName.Text;
                Properties.Settings.Default["documentPath"] = txtDocumentLocation.Text;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Reload();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("I Think it was saved..","mvc");
            this.Close();
            this.Dispose();
        }

        private void frmSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmMain n = new frmMain();
            n.Refresh();
        }
    }
}
