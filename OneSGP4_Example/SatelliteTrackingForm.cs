using One_Sgp4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneSGP4_Example
{
    public partial class SatelliteTrackingForm : Form
    {
        private List<Tle> _tleList;
        private List<Tle> _loadedTleList;//TLE browse button

        //public SatelliteTrackingForm(List<string> satelliteNames)
        public SatelliteTrackingForm(List<Tle> tleList)
        //public SatelliteTrackingForm()
        {
            InitializeComponent();

            // Bind list to combo box
            //comboSatelliteList.DataSource = satelliteNames;

            _tleList = tleList;
            comboSatelliteList.DataSource = _tleList;
            comboSatelliteList.DisplayMember = "satName";   // Property in your Tle class
            //comboSatelliteList.ValueMember = null;       // Optional
        }

        private void btnBrowseTLE_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "TLE files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
                txtTLEFile.Text = dlg.FileName;

            //******************new code
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Filter = "TLE files (*.txt)|*.txt|All files (*.*)|*.*";

            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    txtTLEFile.Text = dlg.FileName;

            //    // Load TLE data
            //    _loadedTleList = ParserTLE.ParseFile(dlg.FileName);

            //    if (_loadedTleList.Count > 0)
            //    {
            //        // Bind satellite names to ComboBox
            //        comboSatelliteList.DataSource = _loadedTleList;
            //        comboSatelliteList.DisplayMember = "satName";
            //    }
            //}


        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "BNA files (*.bna)|*.bna|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
                txtOutputFile.Text = dlg.FileName;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string tlePath = txtTLEFile.Text;
            string satName = txtSatName.Text;
            int totalHours = (int)numHours.Value;
            int intervalMinutes = (int)numMinutes.Value;
            string outputPath = txtOutputFile.Text;

            if (string.IsNullOrWhiteSpace(tlePath) ||
                string.IsNullOrWhiteSpace(satName) ||
                string.IsNullOrWhiteSpace(outputPath))
            {
                MessageBox.Show("Please fill all required fields.", 
                                "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Replace with your actual satellite calculation logic
            MessageBox.Show(
                $"Generating latitude/longitude...\n\n" +
                $"TLE: {tlePath}\n" +
                $"Satellite: {satName}\n" +
                $"Total Time: {totalHours} hours\n" +
                $"Interval: {intervalMinutes} minutes\n" +
                $"Output: {outputPath}",
                "Processing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
