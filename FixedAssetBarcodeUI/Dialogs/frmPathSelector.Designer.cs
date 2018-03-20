namespace FixedAssetBarcodeUI.Dialogs
{
    partial class frmPathSelector
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnlose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDocumentLoc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseDocLoc = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.panel1.Controls.Add(this.btnlose);
            this.panel1.Location = new System.Drawing.Point(-2, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 33);
            this.panel1.TabIndex = 0;
            // 
            // btnlose
            // 
            this.btnlose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnlose.BackColor = System.Drawing.Color.Crimson;
            this.btnlose.FlatAppearance.BorderSize = 0;
            this.btnlose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnlose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnlose.Location = new System.Drawing.Point(193, 0);
            this.btnlose.Name = "btnlose";
            this.btnlose.Size = new System.Drawing.Size(27, 25);
            this.btnlose.TabIndex = 9;
            this.btnlose.Text = "x";
            this.btnlose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnlose.UseVisualStyleBackColor = false;
            this.btnlose.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.panel2.Location = new System.Drawing.Point(-2, 157);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 25);
            this.panel2.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.Location = new System.Drawing.Point(12, 118);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(203, 25);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDocumentLoc
            // 
            this.txtDocumentLoc.Enabled = false;
            this.txtDocumentLoc.Location = new System.Drawing.Point(12, 63);
            this.txtDocumentLoc.Name = "txtDocumentLoc";
            this.txtDocumentLoc.ReadOnly = true;
            this.txtDocumentLoc.Size = new System.Drawing.Size(203, 20);
            this.txtDocumentLoc.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Document Location";
            // 
            // btnBrowseDocLoc
            // 
            this.btnBrowseDocLoc.Location = new System.Drawing.Point(12, 89);
            this.btnBrowseDocLoc.Name = "btnBrowseDocLoc";
            this.btnBrowseDocLoc.Size = new System.Drawing.Size(203, 23);
            this.btnBrowseDocLoc.TabIndex = 7;
            this.btnBrowseDocLoc.Text = "browse";
            this.btnBrowseDocLoc.UseVisualStyleBackColor = true;
            this.btnBrowseDocLoc.Click += new System.EventHandler(this.btnBrowseDocLoc_Click);
            // 
            // frmPathSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 169);
            this.Controls.Add(this.btnBrowseDocLoc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDocumentLoc);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPathSelector";
            this.Text = "Select Path";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnlose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtDocumentLoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseDocLoc;
    }
}