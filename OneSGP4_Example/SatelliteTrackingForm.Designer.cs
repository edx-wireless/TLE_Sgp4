using System.Windows.Forms;

namespace OneSGP4_Example
{
    partial class SatelliteTrackingForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTLE = new System.Windows.Forms.Label();
            this.txtTLEFile = new System.Windows.Forms.TextBox();
            this.btnBrowseTLE = new System.Windows.Forms.Button();
            this.lblSatName = new System.Windows.Forms.Label();
            this.txtSatName = new System.Windows.Forms.TextBox();
            this.comboSatelliteList = new System.Windows.Forms.ComboBox();
            this.lblHours = new System.Windows.Forms.Label();
            this.numHours = new System.Windows.Forms.NumericUpDown();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.numMinutes = new System.Windows.Forms.NumericUpDown();
            this.lblOutput = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTLE
            // 
            this.lblTLE.AutoSize = true;
            this.lblTLE.Location = new System.Drawing.Point(20, 20);
            this.lblTLE.Name = "lblTLE";
            this.lblTLE.Size = new System.Drawing.Size(73, 15);
            this.lblTLE.Text = "Input TLE File:";
            // 
            // txtTLEFile
            // 
            this.txtTLEFile.Location = new System.Drawing.Point(20, 40);
            this.txtTLEFile.Width = 400;
            // 
            // btnBrowseTLE
            // 
            this.btnBrowseTLE.Location = new System.Drawing.Point(430, 38);
            this.btnBrowseTLE.Text = "Browse...";
            this.btnBrowseTLE.Click += new System.EventHandler(this.btnBrowseTLE_Click);
            // 
            // lblSatName
            // 
            this.lblSatName.AutoSize = true;
            this.lblSatName.Location = new System.Drawing.Point(20, 70);
            this.lblSatName.Text = "Satellite Name:";
            // 
            // txtSatName
            // 
            this.txtSatName.Location = new System.Drawing.Point(20, 90);
            this.txtSatName.Width = 300;

            // comboSatelliteList 
            this.comboSatelliteList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSatelliteList.Location = new System.Drawing.Point(20, 110);
            this.comboSatelliteList.Name = "comboSatelliteList";
            this.comboSatelliteList.Size = new System.Drawing.Size(300, 23);
            // 
            // lblHours
            // 
            this.lblHours.AutoSize = true;
            this.lblHours.Location = new System.Drawing.Point(20, 140);
            this.lblHours.Text = "Total Time (hours):";
            // 
            // numHours
            // 
            this.numHours.Location = new System.Drawing.Point(20, 160);
            this.numHours.Maximum = 1000;
            this.numHours.Width = 60;
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(150, 140);
            this.lblMinutes.Text = "Time Interval (minutes):";
            // 
            // numMinutes
            // 
            this.numMinutes.Location = new System.Drawing.Point(150, 160);
            this.numMinutes.Maximum = 1000;
            this.numMinutes.Width = 60;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(20, 200);
            this.lblOutput.Text = "Output BNA file:";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(20, 220);
            this.txtOutputFile.Width = 400;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(430, 218);
            this.btnBrowseOutput.Text = "Browse...";
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(150, 270);
            this.btnGenerate.Width = 250;
            this.btnGenerate.Text = "Generate Latitude/Longitude";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // SatelliteTrackingForm
            // 
            this.ClientSize = new System.Drawing.Size(550, 330);
            this.Controls.Add(this.lblTLE);
            this.Controls.Add(this.txtTLEFile);
            this.Controls.Add(this.btnBrowseTLE);
            this.Controls.Add(this.lblSatName);
            this.Controls.Add(this.txtSatName);
            this.Controls.Add(this.comboSatelliteList);
            this.Controls.Add(this.lblHours);
            this.Controls.Add(this.numHours);
            this.Controls.Add(this.lblMinutes);
            this.Controls.Add(this.numMinutes);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.btnGenerate);
            this.Text = "Satellite Tracking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.numHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTLE;
        private TextBox txtTLEFile;
        private Button btnBrowseTLE;
        private Label lblSatName;
        private TextBox txtSatName;
        private ComboBox comboSatelliteList;
        private Label lblHours;
        private NumericUpDown numHours;
        private Label lblMinutes;
        private NumericUpDown numMinutes;
        private Label lblOutput;
        private TextBox txtOutputFile;
        private Button btnBrowseOutput;
        private Button btnGenerate;
    }
}