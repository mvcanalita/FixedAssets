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
        //private void statusUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    if (e.UserState.GetType() == typeof(Messages))
        //    {
        //        Messages messages = (Messages)e.UserState;
        //        string text;
        //        foreach (Seagull.BarTender.Print.Message message in messages)
        //        {
        //            // Let's remove any carriage returns and linefeeds since
        //            // we are putting each message on a single line.
        //            text = message.Text.Replace('\n', ' ');
        //            text = text.Replace('\r', ' ');
        //            lbEvents.Items.Add(text);
        //        }
        //    }

        //    // If we are finished printing, re-enable the print button.
        //    if (e.ProgressPercentage == 100)
        //        btnPrint.Enabled = true;
        //}
        //printer name trimmer
        private string trimPrinterName(string printerName)
        {
            if (printerName.Contains(@"\"))
            {
                string[] slice = printerName.Split('\\');
                return slice[slice.Length - 1];
            }
            else
            {
                return printerName;
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
            loadDocumentTemplates();
            loadCmbTemplate();

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
                ldoc.PrintSetup.PrinterName = prtName.Trim();
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
        //private void Engine_JobSent(object sender, JobSentEventArgs printJob)
        //{
        //    DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
        //    if (printJob.JobPrintingVerified)
        //        lbEvents.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Sent/Print Verified on {1}.", printJob.Name, printJob.PrinterInfo.Name) });
        //    else
        //        lbEvents.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Sent to {1}.", printJob.Name, printJob.PrinterInfo.Name) });
        //}

        //Reading Excel File
        //protected void readExcelFile(string btLayoutPath, string prtName, string filepath)
        //{
        //    string constr = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=" + @filepath + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"";
        //    string sCommand = "SELECT * FROM [Sheet1$]";


        //        using (OleDbConnection con = new OleDbConnection(constr))
        //        {
        //            using (Engine btEngine = new Engine(true))
        //            {
        //                btEngine.Start();
        //                LabelFormatDocument ldoc = btEngine.Documents.Open(btLayoutPath);
        //                ldoc.PrintSetup.PrinterName = prtName.Trim();
        //                OleDbCommand command = new OleDbCommand(sCommand, con);
        //                con.Open();

        //                OleDbDataReader rdr = command.ExecuteReader();
        //                while (rdr.Read())
        //                {
        //                    ldoc.SubStrings["lblBarcode"].Value = rdr["ID"].ToString();
        //                    ldoc.SubStrings["lblDescription"].Value = rdr["Print Name"].ToString();
        //                    ldoc.SubStrings["lblDate"].Value = rdr["Purchase Date"].ToString() != string.Empty ? Convert.ToDateTime(rdr["Purchase Date"]).ToShortDateString() : "";
        //                    ldoc.Print();
        //                }
        //                ldoc.Close(SaveOptions.DoNotSaveChanges);
        //                btEngine.Stop();
        //            }
        //        }

        //}


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
                //readExcelFile(Path.Combine(documentDirectoryPath, cmbDocument.SelectedItem.ToString()), txtActivePrinter.Text, txtPath.Text);
                printFinalLayout(Path.Combine(documentDirectoryPath, cmbDocument.SelectedItem.ToString()), txtActivePrinter.Text, txtPath.Text);
                //statusUpdater.RunWorkerAsync();
            }
        }

        //private void statusUpdater_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    using (Engine btEngine = new Engine(true))
        //    {

        //        btEngine.Start();

        //        LabelFormatDocument ldoc = btEngine.Documents.Open(Path.Combine(documentDirectoryPath, docName));
        //        Messages messages;
        //        btEngine.JobSent += new EventHandler<JobSentEventArgs>(Engine_JobSent);
        //        ldoc.PrintSetup.PrinterName = txtActivePrinter.Text.Trim();
        //        using (StreamReader ioFile = new StreamReader(@txtPath.Text))
        //        {
        //            ioFile.ReadLine();
        //            int counter = 0;
        //            string line;
        //            while ((line = ioFile.ReadLine()) != null)
        //            {
        //                string[] filSpliced = line.Split(',');
        //                ldoc.SubStrings["lblBarcode"].Value = filSpliced[1];
        //                ldoc.SubStrings["lblDescription"].Value = filSpliced[2];
        //                ldoc.SubStrings["lblDate"].Value = filSpliced[3].ToString() != string.Empty ? Convert.ToDateTime(filSpliced[3]).ToShortDateString() : "";

        //                ldoc.Print("FixedAsset App - ", System.Threading.Timeout.Infinite, out messages);
        //                statusUpdater.ReportProgress(100, messages);
        //                counter++;
        //            }
        //            ioFile.Close();

        //        }
        //        ldoc.Close(SaveOptions.DoNotSaveChanges);
        //        btEngine.Stop();
        //    }
        //}
    }
}
