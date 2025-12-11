using System;
using System.Windows.Forms;
using One_Sgp4;
using System;
using System.Collections.Generic;

namespace SatelliteTrackingApp
{
    public partial class SatelliteTrackingForm : Form
    {
        private static readonly string LogDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\EDX";
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "SatelliteLog.txt");
        private List<Tle> _loadedTleList;//TLE browse button

        public SatelliteTrackingForm()
        {
            InitializeComponent();
        }
        private void Log(string message)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);

                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  {message}\n";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // Silent catch - logging should never crash the app
            }
        }
        
        private void btnBrowseTLE_Click(object sender, EventArgs e)
        {
            Log("---- btnBrowseTLE_Click invoked ----");

            try
            {
                Log("Opening file dialog for TLE file selection...");
                OpenFileDialog dlg = new OpenFileDialog
                {
                    Filter = "TLE files (*.txt)|*.txt|All files (*.*)|*.*"
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string filePath = dlg.FileName;
                    txtTLEFile.Text = filePath;

                    Log($"TLE file selected: {filePath}");
                    Log("Parsing TLE file...");

                    _loadedTleList = ParserTLE.ParseFile(filePath);

                    Log($"TLE parsing completed. Total satellites found: {_loadedTleList.Count}");

                    if (_loadedTleList.Count > 0)
                    {
                        comboSatelliteList.DataSource = _loadedTleList;
                        comboSatelliteList.DisplayMember = "satName";

                        Log("Satellite list successfully bound to ComboBox.");
                    }
                    else
                    {
                        Log("WARNING: No valid TLE entries found in file.");
                    }
                }
                else
                {
                    Log("TLE selection canceled by user.");
                }
            }
            catch (Exception ex)
            {
                Log("ERROR in btnBrowseTLE_Click");
                Log($"Exception: {ex.GetType().Name}");
                Log($"Message: {ex.Message}");
                Log($"StackTrace: {ex.StackTrace}");

                MessageBox.Show("Error loading TLE file. Please check the log file.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Log("---- btnBrowseTLE_Click completed ----\n");
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            Log("---- btnBrowseOutput_Click invoked ----");

            try
            {
                Log("Opening SaveFileDialog for BNA output path...");
                SaveFileDialog dlg = new SaveFileDialog
                {
                    Filter = "BNA files (*.bna)|*.bna|All files (*.*)|*.*"
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFile.Text = dlg.FileName;
                    Log($"Output BNA path selected: {dlg.FileName}");
                }
                else
                {
                    Log("Output file selection cancelled by user.");
                }
            }
            catch (Exception ex)
            {
                Log("ERROR in btnBrowseOutput_Click");
                Log($"Exception: {ex.GetType().Name}");
                Log($"Message: {ex.Message}");
                Log($"StackTrace: {ex.StackTrace}");

                MessageBox.Show("Error selecting output file. Please check logs.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Log("---- btnBrowseOutput_Click completed ----\n");
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            Log("========== Satellite Tracking Started ==========");

            try
            {
                // ------------------------------
                // ** BASIC FIELD VALIDATION **
                // ------------------------------
                Log("Validating input fields...");

                //if (string.IsNullOrWhiteSpace(txtTLEFile.Text) ||
                //    string.IsNullOrWhiteSpace(txtOutputFile.Text) ||
                //    comboSatelliteList.SelectedItem == null)
                //{
                //    Log("Validation failed: Missing TLE, satellite selection, or output file.");
                //    MessageBox.Show("Please select a TLE file, satellite, and output file.",
                //                     "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(txtTLEFile.Text))
                {
                    Log("Validation failed: Missing TLE.");
                    MessageBox.Show("Please select a TLE file.","Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                {
                    Log("Validation failed: satellite selection.");
                    MessageBox.Show("Please select a satellite Name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                {
                    Log("Validation failed: output file.");
                    MessageBox.Show("Please select a output file.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ------------------------------
                // ** NUMERIC VALIDATION **
                // ------------------------------
                int totalHours = (int)numHours.Value;
                int intervalMinutes = (int)numMinutes.Value;

                // Hours > 0
                if (totalHours <= 0)
                {
                    Log("Validation failed: Total Hours value is 0.");
                    MessageBox.Show("Total Time (hours) must be greater than 0.",
                                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Interval Minutes > 0
                if (intervalMinutes <= 0)
                {
                    Log("Validation failed: Time Interval value is 0.");
                    MessageBox.Show("Time Interval (minutes) must be greater than 0.",
                                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // ------------------------------
                // ** DATE VALIDATION **
                // ------------------------------
                DateTime startDate = dateTimePickerPassTime.Value;

                //if (startDate < DateTime.Now)
                //{
                //    Log("Validation failed: Start date/time is in the past.");
                //    MessageBox.Show("Start Date/Time cannot be in the past.",
                //                    "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                // ------------------------------
                //   INPUTS ARE VALID – CONTINUE
                // ------------------------------

                // Selected TLE
                Tle selectedTle = comboSatelliteList.SelectedItem as Tle;
                string satName = selectedTle.getName();
                Log($"Selected satellite: {satName}");

                // Convert to EpochTime
                EpochTime startEpoch = new EpochTime(startDate);
                EpochTime endEpoch = new EpochTime(startDate.AddHours(totalHours)
                                                              .AddMinutes(intervalMinutes));

                Log($"Tracking for {totalHours} hours, interval = {intervalMinutes} minutes");

                // Output path setup
                string outputPath = txtOutputFile.Text;
                string? directory = Path.GetDirectoryName(outputPath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Log($"Created output directory: {directory}");
                }

                // Parse TLE
                Log("Parsing TLE...");
                Tle tle = ParserTLE.parseTle(selectedTle.Line1, selectedTle.Line2, satName);

                // Time steps
                int steps = (totalHours * 60) / intervalMinutes;
                int totalPoints = steps + 1;
                Log($"Total points to calculate: {totalPoints}");

                // Write BNA header
                string header = $"\"*EDX_Polyline*\", \"\", -{totalPoints}, Start Date Time(hh:mm:ss):{startDate}\n";
                File.WriteAllText(outputPath, header);
                Log("Wrote BNA header successfully.");

                // Initialize SGP4
                Log("Initializing SGP4...");
                Sgp4 sgp4 = new Sgp4(tle, Sgp4.wgsConstant.WGS_84);

                // LOOP
                Log("Beginning satellite position generation loop...");
                for (int i = 0; i <= steps; i++)
                {
                    try
                    {
                        EpochTime currentTime =
                            new EpochTime(startDate.AddMinutes(i * intervalMinutes));

                        Log($"Calculating satellite position for t+{i * intervalMinutes} minutes");

                        // Calc ECI
                        Sgp4Data state = SatFunctions.getSatPositionAtTime(
                            tle, currentTime, Sgp4.wgsConstant.WGS_84);

                        // Sub-point
                        Coordinate subPoint =
                            SatFunctions.calcSatSubPoint(currentTime, state, Sgp4.wgsConstant.WGS_84);

                        string line = $"{subPoint.getLatitude():F6},{subPoint.getLongitude():F6}\n";
                        File.AppendAllText(outputPath, line);

                        Log($"Wrote point: Lat={subPoint.getLatitude():F6} , Lon={subPoint.getLongitude():F6}");
                    }
                    catch (Exception exInner)
                    {
                        Log($"ERROR inside interval loop: {exInner.Message} | {exInner.StackTrace}");
                    }

                    await Task.Delay(20);
                }

                Log("Satellite tracking completed successfully.");
                MessageBox.Show("BNA file created successfully!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("********** FATAL ERROR **********");
                Log($"Exception: {ex.GetType().Name}");
                Log($"Message: {ex.Message}");
                Log($"StackTrace: {ex.StackTrace}");

                MessageBox.Show(
                    "An error occurred while generating the BNA file.\n" +
                    "Please check the log file:\n" + LogFilePath,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Log("========== Satellite Tracking Finished ==========\n\n");
            }
        }
    }
}
