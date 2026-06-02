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
                Log("Opening SaveFileDialog for CSV output path...");
                SaveFileDialog dlg = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFile.Text = dlg.FileName;
                    Log($"Output CSV path selected: {dlg.FileName}");
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

        const double WGS84_A = 6378.137;      // Semi-major axis (equatorial radius, km)
        const double WGS84_B = 6356.752314;   // Semi-minor axis (polar radius, km)


        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            Log("========== Satellite Tracking Started ==========");

            try
            {
                // ------------------------------
                // ** BASIC FIELD VALIDATION **
                // ------------------------------
                Log("Validating input fields...");

                if (string.IsNullOrWhiteSpace(txtTLEFile.Text))
                {
                    Log("Validation failed: Missing TLE.");
                    MessageBox.Show("Please select a TLE file.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                {
                    Log("Validation failed: Missing output file.");
                    MessageBox.Show("Please select an output file.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ------------------------------
                // ** NUMERIC VALIDATION **
                // ------------------------------
                int totalHours = (int)numHours.Value;
                int intervalMinutes = (int)numMinutes.Value;

                if (totalHours <= 0)
                {
                    Log("Validation failed: Total Hours value is 0.");
                    MessageBox.Show("Total Time (hours) must be greater than 0.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (intervalMinutes <= 0)
                {
                    Log("Validation failed: Time Interval value is 0.");
                    MessageBox.Show("Time Interval (minutes) must be greater than 0.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ------------------------------
                // ** DATE VALIDATION **
                // ------------------------------
                DateTime startDate = dateTimePickerPassTime.Value;

                // ------------------------------
                //   INPUTS ARE VALID - CONTINUE
                // ------------------------------
                Tle selectedTle = comboSatelliteList.SelectedItem as Tle;
                string satName = selectedTle.getName();
                Log($"Selected satellite: {satName}");
                Log($"Tracking for {totalHours} hours, interval = {intervalMinutes} minutes");

                // ------------------------------
                // ** OUTPUT PATH SETUP **
                // ------------------------------
                string csvOutputPath = txtOutputFile.Text;
                string? directory = Path.GetDirectoryName(csvOutputPath);

                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Log($"Created output directory: {directory}");
                }

                // Parse TLE
                Log("Parsing TLE...");
                Tle tle = ParserTLE.parseTle(selectedTle.Line1, selectedTle.Line2, satName);

                int steps = (totalHours * 60) / intervalMinutes;
                int totalPoints = steps + 1;
                Log($"Total points to calculate: {totalPoints}");

                // Write CSV header
                string csvHeader = "DateTime,Latitude,Longitude,Altitude_km," +
                   "v_sat_X_kms,v_sat_Y_kms,v_sat_Z_kms\n";
                File.WriteAllText(csvOutputPath, csvHeader);
                Log($"Wrote CSV header to: {csvOutputPath}");

                // Initialize SGP4
                Log("Initializing SGP4...");
                Sgp4 sgp4 = new Sgp4(tle, Sgp4.wgsConstant.WGS_84);

                // ------------------------------
                // ** MAIN LOOP **
                // ------------------------------
                Log("Beginning satellite position generation loop...");
                for (int i = 0; i <= steps; i++)
                {
                    try
                    {
                        DateTime currentDateTime = startDate.AddMinutes(i * intervalMinutes);
                        EpochTime currentTime = new EpochTime(currentDateTime);

                        // Step 1: SGP4 - get ECI position from TLE
                        Sgp4Data state = SatFunctions.getSatPositionAtTime(
                            tle, currentTime, Sgp4.wgsConstant.WGS_84);

                        // Step 2: Sub-satellite point (lat, lon)
                        Coordinate subPoint = SatFunctions.calcSatSubPoint(
                            currentTime, state, Sgp4.wgsConstant.WGS_84);

                        double lon = subPoint.getLongitude();
                        double lat = subPoint.getLatitude();

                        // Step 3: Earth radius at satellite's current latitude (WGS-84)
                        double earthRadiusKm = GetEarthRadiusAtLatitude(lat);

                        // Step 4: Altitude from ECI vector magnitude minus WGS-84 Earth radius
                        double eciMagnitude = Math.Sqrt(
                            state.getX() * state.getX() +
                            state.getY() * state.getY() +
                            state.getZ() * state.getZ()
                        );
                        double altKm = eciMagnitude - earthRadiusKm;

                        double r_sat_X = state.getX();
                        double r_sat_Y = state.getY();
                        double r_sat_Z = state.getZ();
                        double v_sat_X = state.getXDot();
                        double v_sat_Y = state.getYDot();
                        double v_sat_Z = state.getZDot();
                        
                        // Write CSV line                        
                        string csvLine = $"{currentDateTime:yyyy-MM-dd HH:mm:ss}," +
                                         $"{lat:F6},{lon:F6},{altKm:F3}," +                                        
                                         $"{v_sat_X:F6},{v_sat_Y:F6},{v_sat_Z:F6}\n";
                        File.AppendAllText(csvOutputPath, csvLine);

                        Log($"Point [{i}]: {currentDateTime:yyyy-MM-dd HH:mm:ss}, Lat={lat:F6}, Lon={lon:F6}, Alt={altKm:F3} km");
                    }
                    catch (Exception exInner)
                    {
                        Log($"ERROR at step {i}: {exInner.Message} | {exInner.StackTrace}");
                    }

                    await Task.Delay(20);
                }

                Log("Satellite tracking completed successfully.");
                MessageBox.Show(
                    $"CSV created successfully!\n\n{csvOutputPath}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("********** FATAL ERROR **********");
                Log($"Exception: {ex.GetType().Name}");
                Log($"Message:   {ex.Message}");
                Log($"StackTrace:{ex.StackTrace}");

                MessageBox.Show(
                    "An error occurred while generating the file.\n" +
                    "Please check the log file:\n" + LogFilePath,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log("========== Satellite Tracking Finished ==========\n\n");
            }
        }


        /// <summary>
        /// Returns Earth's radius (km) at a given geodetic latitude
        /// using the WGS-84 ellipsoid model.
        /// </summary>
        private double GetEarthRadiusAtLatitude(double latitudeDeg)
        {
            double latRad = latitudeDeg * Math.PI / 180.0;
            double cosLat = Math.Cos(latRad);
            double sinLat = Math.Sin(latRad);

            double radius = Math.Sqrt(
                (Math.Pow(WGS84_A * WGS84_A * cosLat, 2) + Math.Pow(WGS84_B * WGS84_B * sinLat, 2)) /
                (Math.Pow(WGS84_A * cosLat, 2) + Math.Pow(WGS84_B * sinLat, 2))
            );

            return radius;
        }
    }
}