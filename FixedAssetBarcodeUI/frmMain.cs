using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FixedAssetBarcodeUI.Dialogs;
using Seagull.BarTender.Print;
using Seagull.Services.PrintScheduler;
using System.Drawing.Printing;
using System.IO;
using System.Data.OleDb;
using System.Management;

namespace FixedAssetBarcodeUI
{
    public partial class frmMain : Form
    {
        #region Constants
        //moveable panel
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //end moveable panel
        // Dropshadow
        private const int CS_DROPSHADOW = 0x00020000;
        // End Dropshadow

        //Document Location
        public string documentDirectoryPath = "";
        public string docName = "";
        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        #region window btns
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void lblApplicationHeader_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Author: MVCANALITA" + Environment.NewLine + Environment.NewLine + "Version: 1.0.2" + Environment.NewLine + Environment.NewLine + "Language: C#");
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //On panel header mouse down
        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
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

        #region CSV File Handling
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgF = new OpenFileDialog();
            dlgF.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            DialogResult dr = dlgF.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string[] filename = dlgF.FileName.ToString().Split('\\');
                txtFilename.Text = filename[filename.Length - 1].ToString();
                txtPath.Text = dlgF.FileName.ToString();
            }
        }
        #endregion

        #region Settings Icon animation
        private void pbSettings_MouseHover(object sender, EventArgs e)
        {
            pbSettings.BackColor = Color.DimGray;
        }

        private void pbSettings_MouseLeave(object sender, EventArgs e)
        {
            pbSettings.BackColor = Color.Transparent;
        }
        #endregion

        #region methods
        //Load printers to combo box
        protected void loadPrinters()
        {
            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            foreach (var printer in printerQuery.Get())
            {

                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var printloc = printer.GetPropertyValue("Location");
                cmbPrinters.Items.Add(name);
            }
        }

        //printer name trimmer
        private string trimPrinterName(string printerName)
        {
            if (printerName.Contains(@"\"))
            {
                string[] slice = printerName.Split('\\');
                return slice[slice.Length - 1].Trim();
            }
            else
            {
                return printerName.Trim();
            }
        }

        //loading of Template from default folder
        protected void loadDocumentTemplates()
        {

            if (!Properties.Settings.Default.useDefaultDocLoc)
            {
                try
                {
                    documentDirectoryPath = Path.Combine(Application.StartupPath, @"\Templates\");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Path Not Found." + Environment.NewLine + Environment.NewLine + ex.ToString());
                }

            }
            else
            {
                if (!Directory.Exists(Properties.Settings.Default.documentPath.ToString()))
                {
                    MessageBox.Show("Path Not Found.");
                }
                else
                {
                    try
                    {
                        documentDirectoryPath = Properties.Settings.Default.documentPath.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }

        protected void loadCmbTemplate()
        {
            try
            {
                DirectoryInfo dr = new DirectoryInfo(documentDirectoryPath);
                FileInfo[] doclist = dr.GetFiles("*.btw");
                foreach (FileInfo fi in doclist)
                {
                    cmbDocument.Items.Add(fi.Name);
                }
                cmbDocument.SelectedIndex = cmbDocument.FindString(Properties.Settings.Default.defaultDocument.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //summary of all defaults
        protected void loadAppDefaults()
        {
            if (Properties.Settings.Default.useDefaultPrinter)
            {
                PrinterSettings ps = new PrinterSettings();

                txtActivePrinter.Text = trimPrinterName(ps.PrinterName.ToString());
            }
            else
            {
                txtActivePrinter.Text = trimPrinterName(Properties.Settings.Default.printerName.ToString());
            }
            //load printers
            loadPrinters();
            //load templates from default folder
            loadDocumentTemplates();
            //load documents in cmb
            loadCmbTemplate();
            //always check default printer
            chkUseDefault.Checked = true;

        }

        //create a preview Image
        protected void previewTemplate(string documentFile)
        {
            if (cmbDocument.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("No Template Selected");
            }
            else
            {
                pbPreview.ImageLocation = "";
                //generate new file
                using (Engine btEngine = new Engine(true))
                {
                    // Start the BarTender print engine. 
                    btEngine.Start();
                    // Open the specified format. 
                    LabelFormatDocument btFormat = btEngine.Documents.Open(Path.Combine(documentDirectoryPath, documentFile));
                    // Export the label format's thumbnail image to a file. 
                    btFormat.ExportImageToFile(Path.Combine(Path.GetTempPath(), "PreviewImage.jpg"), ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth256, new Resolution(96), Seagull.BarTender.Print.OverwriteOptions.Overwrite);
                    // Stop the BarTender print engine. 
                    pbPreview.ImageLocation = Path.Combine(Path.GetTempPath(), "PreviewImage.jpg");
                    btEngine.Stop();
                }
                //Change Preview

            }
        }
        //printing time
        protected void printFinalLayout(string btLayoutPath, string prtName, string filepath)
        {
            using (Engine btEngine = new Engine(true))
            {
                lbEvents.Items.Add("Print Job Started");
                btnPrint.Enabled = false;
                btEngine.Start();
                LabelFormatDocument ldoc = btEngine.Documents.Open(btLayoutPath);
                ldoc.PrintSetup.PrinterName = trimPrinterName(prtName);
                using (StreamReader ioFile = new StreamReader(@filepath))
                {
                    ioFile.ReadLine();
                    int counter = 0;
                    string line;
                    while ((line = ioFile.ReadLine()) != null)
                    {
                        string[] filSpliced = line.Split(',');
                        ldoc.SubStrings["lblBarcode"].Value = filSpliced[1];
                        ldoc.SubStrings["lblDescription"].Value = filSpliced[2];
                        ldoc.SubStrings["lblDate"].Value = filSpliced[3].ToString() != string.Empty ? Convert.ToDateTime(filSpliced[3]).ToShortDateString() : "";

                        ldoc.Print();
                        counter++;
                    }
                    ioFile.Close();
                    lbEvents.Items.Add("Print Job Completed");
                }
                ldoc.Close(SaveOptions.DoNotSaveChanges);
                btEngine.Stop();
            }
            btnPrint.Enabled = true;
        }
        public delegate void DoUpdateOutputDelegate(string output);
        private void DoUpdateOutput(string output)
        {
            lbEvents.Items.Add(output);
        }
        
        //Using text file as database
        protected void hooktextdb(string btLayoutPath, string prtName, string filepath)
        {
            using (Engine btEngine = new Engine(true))
            {
                lbEvents.Items.Add("Print Job Started");
                btnPrint.Enabled = false;
                btEngine.Start();
                LabelFormatDocument ldoc = btEngine.Documents.Open(btLayoutPath);
                ldoc.PrintSetup.PrinterName = trimPrinterName(prtName);
                Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(ldoc.DatabaseConnections[0].Name);
                tf.FileName = @filepath;
                ldoc.DatabaseConnections.SetDatabaseConnection(tf);
                ldoc.PrintSetup.ReloadTextDatabaseFields = true; // Fix when field order is different from design time
                //print using the database
                ldoc.Print();
                //Close the document
                ldoc.Close(SaveOptions.DoNotSaveChanges);
                btEngine.Stop();
            }
            btnPrint.Enabled = true;
        }

        #endregion
        private void pbSettings_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings();
            settings.ShowDialog();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            loadAppDefaults();
        }

        private void cmbDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            docName = cmbDocument.SelectedItem.ToString();
            previewTemplate(cmbDocument.SelectedItem.ToString());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == string.Empty)
            {
                MessageBox.Show("No File selected to print", "MVC");
            }
            else if (txtActivePrinter.Text == string.Empty)
            {
                MessageBox.Show("Printer Not Set", "MVC");
            }
            else if (cmbDocument.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("No Layout Selected", "MVC");
            }
            else
            {

                switch (cmbDocument.SelectedItem.ToString())
                {
                    case "FixedAssetTemplateV1.btw":
                        printFinalLayout(Path.Combine(documentDirectoryPath, cmbDocument.SelectedItem.ToString()), txtActivePrinter.Text, txtPath.Text);
                        break;

                    case "FixedAssetTemplateV2.btw":
                        printFinalLayout(Path.Combine(documentDirectoryPath, cmbDocument.SelectedItem.ToString()), txtActivePrinter.Text, txtPath.Text);
                        break;

                    case "FixedAssetTemplateV3.btw":
                        hooktextdb(Path.Combine(documentDirectoryPath, cmbDocument.SelectedItem.ToString()), txtActivePrinter.Text, txtPath.Text);
                        break;

                    default:
                        printFinalLayout(Path.Combine(documentDirectoryPath, cmbDocument.SelectedItem.ToString()), txtActivePrinter.Text, txtPath.Text);
                        break;
                }

            }
        }

        private void cmbPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtActivePrinter.Text = trimPrinterName(cmbPrinters.SelectedItem.ToString());
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Mvc");
            }
        }

        private void chkUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkUseDefault.Checked)
            {
                cmbPrinters.Enabled = true;
            }
            else
            {
                cmbPrinters.Enabled = false;
                PrinterSettings ps = new PrinterSettings();
                txtActivePrinter.Text = trimPrinterName(ps.PrinterName.ToString());
            }
        }
    }
}
