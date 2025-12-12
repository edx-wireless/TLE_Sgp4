using One_Sgp4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Windows.Forms;

namespace OneSGP4_Example
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            /* 
             * Parse OMM (Orbit Mean-Elements Message)
             * 
                string testpath = "path/to/omm.xml";
                Parse OMM (Orbit Mean-Elements Message)
                XmlDocument doc = new XmlDocument();
                doc.Load(testpath);
                List<Omm> OmmList = ParserOMM.Parse(doc);

            Calculate Satellite Position and Speed

                One_Sgp4.Sgp4 sgp4Propagator = new Sgp4(OmmList[0], Sgp4.wgsConstant.WGS_84);
            */
#if DEBUG
            //Code to check Latitude/Longitude for Satallite
            //Working TLE
            //string line1 = "1 25544U 98067A   19364.04305556 -.00001219  00000-0 -13621-4 0  9993";
            //string line2 = "2 25544  51.6441 110.3812 0005206  82.0414 249.9912 15.49519575205634";
            //Tle tle = ParserTLE.parseTle(line1, line2, "ISS 1");

            ////Working TLE
            //string line1 = "1 24793U 97020B   18291.94986041 +.00000123 +00000-0 +36812-4 0  9995";
            //string line2 = "2 24793 086.3924 114.7382 0002117 088.3447 271.7991 14.34266416123021";
            //Tle tle = ParserTLE.parseTle(line1, line2, "IRIDIUM 7");

            //// Choose WGS-84 (most common) for Earth model
            //One_Sgp4.Sgp4 sgp = new Sgp4(tle, Sgp4.wgsConstant.WGS_84);

            //// Set the epoch time you want
            //EpochTime when = new EpochTime(DateTime.UtcNow);

            //// Compute satellite position
            //Sgp4Data satPos = SatFunctions.getSatPositionAtTime(tle, when, Sgp4.wgsConstant.WGS_84);

            //// Compute the sub-point: lat, lon, alt (on ground, directly under satellite)
            //Coordinate subpoint = SatFunctions.calcSatSubPoint(when, satPos, Sgp4.wgsConstant.WGS_84);

            //Console.WriteLine($"Latitude:  {subpoint.getLatitude():F6}°");
            //Console.WriteLine($"Longitude: {subpoint.getLongitude():F6}°");
            //Console.WriteLine($"Height / Altitude:  {subpoint.getHeight():F3} km");


            ////Check file/Directory Exist to create BNA file
            //string filePath = @"C:\EDX_Wirless_BNAFile\SatInfo.bna";
            //string directory = Path.GetDirectoryName(filePath);

            //// Ensure directory exists
            //if (!Directory.Exists(directory))
            //{
            //    Directory.CreateDirectory(directory);
            //}
#endif

            //Parse three line element
            Tle tleISS = ParserTLE.parseTle(
                "1 25544U 98067A   19364.04305556 -.00001219  00000-0 -13621-4 0  9993",
                "2 25544  51.6441 110.3812 0005206  82.0414 249.9912 15.49519575205634",
                "ISS 1");

            //string file = "tleData.txt";//Path of the TLE file
            List<Tle> tleList = null;
            //List<string> satelliteNames = new List<string>();
            List<Tle> satelliteList = new List<Tle>();

            //Parse tle from file
            if (System.IO.File.Exists("tleData.txt"))//Always check TLE file in out file of the debug directory 
            {
                tleList = ParserTLE.ParseFile("tleData.txt");
            }

            // Create header text
            string textHeader = "\n\"*EDX_Polyline*\",\"\"," + "-" + tleList.Count + "\n";

            // Write header ONCE (overwrite existing file or create new)
            //File.WriteAllText(filePath, textHeader);

            //// Verify each TLE entry
            foreach (var tle in tleList)
            {
                Console.WriteLine("\n--- Satellite Loaded ---");
                Console.WriteLine($"Name : {tle.getName()}");
                Console.WriteLine($"L1   : {tle.Line1}");
                Console.WriteLine($"L2   : {tle.Line2}");
                //Choose WGS-84(most common) for Earth model
                One_Sgp4.Sgp4 sgp = new Sgp4(tle, Sgp4.wgsConstant.WGS_84);

                // Set the epoch time you want
                EpochTime when = new EpochTime(DateTime.UtcNow);

                // Compute satellite position
                Sgp4Data satPos = SatFunctions.getSatPositionAtTime(tle, when, Sgp4.wgsConstant.WGS_84);

                // Compute the sub-point: lat, lon, alt (on ground, directly under satellite)
                Coordinate subpoint = SatFunctions.calcSatSubPoint(when, satPos, Sgp4.wgsConstant.WGS_84);

                Console.WriteLine($"Latitude:  {subpoint.getLatitude():F6}°");
                Console.WriteLine($"Longitude: {subpoint.getLongitude():F6}°");
                Console.WriteLine($"Height / Altitude:  {subpoint.getHeight():F3} km");

                //satelliteNames.Add(tle.getName());
                satelliteList.Add(tle);

                //string text = textHeader + "\n" + $" {subpoint.getLatitude():F6}° " + "," + $"{subpoint.getLongitude():F6}°";
                //// Append to file
                //File.AppendAllText(filePath, text);

                // Build coordinate line (no header)
                //string coordinateLine = $"{subpoint.getLongitude():F6}°, {subpoint.getLatitude():F6}°\n";

                // Append only coordinates
                //File.AppendAllText(filePath, coordinateLine);
            }
#if DEBUG
            ////Get TLE from Space-Track.org
            ////list of satellites by their NORAD ID
            //string[] noradIDs = { "8709", "43572" };
            //try
            //{
            //    //One_Sgp4.SpaceTrack.GetSpaceTrack(noradIDs, "USERNAME", "PASSWORD");string username = "";
            //    string username = "";
            //    string password = "";
            //    if (username != "" && password != "")
            //        One_Sgp4.SpaceTrack.GetSpaceTrack(noradIDs, username, password);
            //    else
            //        Console.WriteLine("Skipping Space-Track download: no credentials provided.");
            //}
            //catch { Console.Out.WriteLine("Error could not retrive TLE's from Space-Track, Login credentials might be wrong"); }
#endif

            //Create Time points
            EpochTime startTime = new EpochTime(DateTime.UtcNow);
            EpochTime anotherTime = new EpochTime(2018, 100.5); //(Year 2018, 100 day at 12:00 HH)
            EpochTime stopTime = new EpochTime(DateTime.UtcNow.AddHours(1));

            //get time difference
            double daysSince = startTime - anotherTime;
            //throws exception if first time > second time
            //double daysSince = anotherTime - startTime;

            //compare Time points
            EpochTime compareTime = new EpochTime(2018, 100.5); //(Year 2018, 100 day at 12:00 HH)
            bool equals = anotherTime == compareTime;
            bool notequals = startTime != anotherTime;
            bool greater = startTime > anotherTime;
            bool smaler = anotherTime < startTime;

            //Add 15 Seconds to EpochTime
            anotherTime.addTick(15);
            //Add 20 Min to EpochTime
            anotherTime.addMinutes(15);
            //Add 1 hour to EpochTime
            anotherTime.addHours(1);
            //Add 2 Days to EpochTime
            anotherTime.addDays(2);
            Console.Out.WriteLine(anotherTime.ToString());

            //Calculate Satellite Position and Speed
            One_Sgp4.Sgp4 sgp4Propagator = new Sgp4(tleISS, Sgp4.wgsConstant.WGS_84);
            //set calculation parameters StartTime, EndTime and caclulation steps in minutes
            sgp4Propagator.runSgp4Cal(startTime, stopTime, 1 / 30.0); // 1/60 => caclulate sat points every 2 seconds
            List<One_Sgp4.Sgp4Data> resultDataList = new List<Sgp4Data>();
            //Return Results containing satellite Position x,y,z (ECI-Coordinates in Km) and Velocity x_d, y_d, z_d (ECI-Coordinates km/s) 
            resultDataList = sgp4Propagator.getResults();

            startTime = new EpochTime(DateTime.Now);
            //Coordinate of an observer on Ground lat, long, height(in meters)
            One_Sgp4.Coordinate observer = new Coordinate(35.00, 18, 0);

            //Convert to ECI coordinate system
            One_Sgp4.Point3d eci = observer.toECI(0.0);
            Console.Out.WriteLine("Sidereal Time: " + startTime.getLocalSiderealTime());
            Console.Out.WriteLine(String.Format("X: {0}, Y: {1}, Z: {2} ", eci.x, eci.y, eci.z));

            //Get Local SiderealTime for Observer
            double localSiderealTime = startTime.getLocalSiderealTime(observer.getLongitude());

            //Calculate if Satellite is Visible for a certain Observer on ground at certain timePoint
            bool satelliteIsVisible = One_Sgp4.SatFunctions.isSatVisible(observer, 0.0, startTime, resultDataList[0]);

            //Calculate Sperical Coordinates from an Observer to Satellite
            //returns 3D-Point with range(km), azimuth(radians), elevation(radians) to the Satellite
            One_Sgp4.Point3d spherical = One_Sgp4.SatFunctions.calcSphericalCoordinate(observer, startTime, resultDataList[0]);

            //Calculate the Next 5 Passes over a point
            //for a location, Satellite, StartTime, Accuracy in Seconds = 15sec, MaxNumber of Days = 5 Days, Wgs constant = WGS_84
            //Returns pass with Location, StartTime of Pass, EndTime Of Pass, Max Elevation in Degrees
            List<Pass> passes = One_Sgp4.SatFunctions.CalculatePasses(observer, tleISS, new EpochTime(DateTime.UtcNow));

            foreach (var p in passes)
            {
                //Amol_M Prient calculated latitude and Logitude values
                // Set the epoch time you want
                EpochTime passwhen = new EpochTime(DateTime.UtcNow);
                Sgp4Data passsatPos = SatFunctions.getSatPositionAtTime(tleISS, passwhen, Sgp4.wgsConstant.WGS_84); // Compute the sub-point: lat, lon, alt (on ground, directly under satellite)
                Coordinate getsubpoint = SatFunctions.calcSatSubPoint(passwhen, passsatPos, Sgp4.wgsConstant.WGS_84);
                One_Sgp4.Coordinate observer2 = new Coordinate(getsubpoint.getLatitude(), getsubpoint.getLongitude(), getsubpoint.getHeight());

                await Task.Delay(1000); // wait 1 seconds (non-blocking)

                Console.WriteLine("{0}  \nLatitude: {1} \nLongitude: {2} \nHeight/Altitude {3}", p, observer2.getLatitude(), observer2.getLongitude(), observer2.getHeight());
            }
            Console.Out.WriteLine("Done");

#if DEBUG
            //***********1st Part: WinForms Application Run **************//
            //// You can still read console args if needed
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SatelliteTrackingForm());

            //////******Part2: WinForms Application Run + Console Application Test i.e.If you want BOTH Console Output + WinForms**************//
            //Console.WriteLine("Console + WinForms test.");
            ////await Task.Run(() => Application.Run(new SatelliteTrackingForm(satelliteNames)));
            //await Task.Run(() => Application.Run(new SatelliteTrackingForm(satelliteList)));
            ////await Task.Run(() => Application.Run(new SatelliteTrackingForm()));
            //Console.WriteLine("WinForms running in background...");
            //Console.ReadLine();

            //**********************************************
#endif
        }
    }
}
