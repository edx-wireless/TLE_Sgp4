namespace SatelliteTrackingApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SatelliteTrackingForm));
            lblTLE = new Label();
            txtTLEFile = new TextBox();
            btnBrowseTLE = new Button();
            lblSatName = new Label();
            comboSatelliteList = new ComboBox();
            lblHours = new Label();
            numHours = new NumericUpDown();
            lblMinutes = new Label();
            numMinutes = new NumericUpDown();
            lblOutput = new Label();
            txtOutputFile = new TextBox();
            btnBrowseOutput = new Button();
            btnGenerate = new Button();
            lblDateTime = new Label();
            dateTimePickerPassTime = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)numHours).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinutes).BeginInit();
            SuspendLayout();
            // 
            // lblTLE
            // 
            lblTLE.AutoSize = true;
            lblTLE.Location = new Point(20, 20);
            lblTLE.Name = "lblTLE";
            lblTLE.Size = new Size(101, 18);
            lblTLE.TabIndex = 0;
            lblTLE.Text = "Input TLE File:";
            // 
            // txtTLEFile
            // 
            txtTLEFile.Location = new Point(20, 40);
            txtTLEFile.Name = "txtTLEFile";
            txtTLEFile.Size = new Size(400, 24);
            txtTLEFile.TabIndex = 1;
            // 
            // btnBrowseTLE
            // 
            btnBrowseTLE.Location = new Point(430, 38);
            btnBrowseTLE.Name = "btnBrowseTLE";
            btnBrowseTLE.Size = new Size(75, 29);
            btnBrowseTLE.TabIndex = 2;
            btnBrowseTLE.Text = "Browse...";
            btnBrowseTLE.Click += btnBrowseTLE_Click;
            // 
            // lblSatName
            // 
            lblSatName.AutoSize = true;
            lblSatName.Location = new Point(20, 80);
            lblSatName.Name = "lblSatName";
            lblSatName.Size = new Size(107, 18);
            lblSatName.TabIndex = 3;
            lblSatName.Text = "Satellite Name:";
            // 
            // comboSatelliteList
            // 
            comboSatelliteList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSatelliteList.Location = new Point(20, 101);
            comboSatelliteList.Name = "comboSatelliteList";
            comboSatelliteList.Size = new Size(300, 26);
            comboSatelliteList.TabIndex = 5;
            // 
            // lblHours
            // 
            lblHours.AutoSize = true;
            lblHours.Location = new Point(20, 202);
            lblHours.Name = "lblHours";
            lblHours.Size = new Size(134, 18);
            lblHours.TabIndex = 6;
            lblHours.Text = "Total Time (hours):";
            // 
            // numHours
            // 
            numHours.Location = new Point(200, 196);
            numHours.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numHours.Name = "numHours";
            numHours.Size = new Size(60, 24);
            numHours.TabIndex = 7;
            // 
            // lblMinutes
            // 
            lblMinutes.AutoSize = true;
            lblMinutes.Location = new Point(20, 245);
            lblMinutes.Name = "lblMinutes";
            lblMinutes.Size = new Size(161, 18);
            lblMinutes.TabIndex = 8;
            lblMinutes.Text = "Time Interval (minutes):";
            // 
            // numMinutes
            // 
            numMinutes.Location = new Point(200, 245);
            numMinutes.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMinutes.Name = "numMinutes";
            numMinutes.Size = new Size(60, 24);
            numMinutes.TabIndex = 9;
            // 
            // lblOutput
            // 
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(20, 299);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(112, 18);
            lblOutput.TabIndex = 10;
            lblOutput.Text = "Output CSV file:";
            // 
            // txtOutputFile
            // 
            txtOutputFile.Location = new Point(20, 320);
            txtOutputFile.Name = "txtOutputFile";
            txtOutputFile.Size = new Size(400, 24);
            txtOutputFile.TabIndex = 11;
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Location = new Point(430, 318);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(75, 29);
            btnBrowseOutput.TabIndex = 12;
            btnBrowseOutput.Text = "Browse...";
            btnBrowseOutput.Click += btnBrowseOutput_Click;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(150, 370);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(258, 34);
            btnGenerate.TabIndex = 13;
            btnGenerate.Text = "Generate Latitude/Longitude";
            btnGenerate.Click += btnGenerate_Click;
            // 
            // lblDateTime
            // 
            lblDateTime.AutoSize = true;
            lblDateTime.Location = new Point(20, 155);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(123, 18);
            lblDateTime.TabIndex = 6;
            lblDateTime.Text = "Start Date / Time:";
            // 
            // dateTimePickerPassTime
            // 
            dateTimePickerPassTime.CustomFormat = "dd-MM-yyyy HH:mm";
            dateTimePickerPassTime.Format = DateTimePickerFormat.Custom;
            dateTimePickerPassTime.Location = new Point(200, 150);
            dateTimePickerPassTime.Name = "dateTimePickerPassTime";
            dateTimePickerPassTime.Size = new Size(200, 24);
            dateTimePickerPassTime.TabIndex = 7;
            // 
            // SatelliteTrackingForm
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 430);
            Controls.Add(lblTLE);
            Controls.Add(txtTLEFile);
            Controls.Add(btnBrowseTLE);
            Controls.Add(lblSatName);
            Controls.Add(comboSatelliteList);
            Controls.Add(lblDateTime);
            Controls.Add(dateTimePickerPassTime);
            Controls.Add(lblHours);
            Controls.Add(numHours);
            Controls.Add(lblMinutes);
            Controls.Add(numMinutes);
            Controls.Add(lblOutput);
            Controls.Add(txtOutputFile);
            Controls.Add(btnBrowseOutput);
            Controls.Add(btnGenerate);
            Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "SatelliteTrackingForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Satellite Tracking";
            ((System.ComponentModel.ISupportInitialize)numHours).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinutes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTLE;
        private TextBox txtTLEFile;
        private Button btnBrowseTLE;
        private Label lblSatName;
        private ComboBox comboSatelliteList;
        private Label lblDateTime;
        private DateTimePicker dateTimePickerPassTime;

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