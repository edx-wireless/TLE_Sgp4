using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteTrackingApp
{
    public static class LeoSatelliteService
    {
        const double EarthRadiusKm = 6378.137;
        const double Mu = 398600.4418; // km^3/s^2

        // ============================
        // 📦 Vector
        // ============================
        public class Vector3
        {
            public double X, Y, Z;

            public Vector3(double x, double y, double z)
            {
                X = x; Y = y; Z = z;
            }

            public static Vector3 operator -(Vector3 a, Vector3 b)
            {
                return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            }
        }

        // ============================
        // 📄 TLE STRUCTURE
        // ============================
        public class TleData
        {
            public double Inclination;
            public double RAAN;
            public double Eccentricity;
            public double ArgumentPerigee;
            public double MeanAnomaly;
            public double MeanMotion;
            public DateTime Epoch;
        }

        // ============================
        // 📥 Parse TLE
        // ============================
        public static TleData ParseTle(string line1, string line2)
        {
            TleData t = new TleData();

            // Epoch
            int year = int.Parse(line1.Substring(18, 2));
            double day = double.Parse(line1.Substring(20, 12));

            year += (year < 57) ? 2000 : 1900;
            t.Epoch = new DateTime(year, 1, 1).AddDays(day - 1);

            // Line 2 values
            t.Inclination = double.Parse(line2.Substring(8, 8));
            t.RAAN = double.Parse(line2.Substring(17, 8));
            t.Eccentricity = double.Parse("0." + line2.Substring(26, 7));
            t.ArgumentPerigee = double.Parse(line2.Substring(34, 8));
            t.MeanAnomaly = double.Parse(line2.Substring(43, 8));
            t.MeanMotion = double.Parse(line2.Substring(52, 11));

            return t;
        }

        // ============================
        // 🛰️ Simplified SGP4 → ECI
        // ============================
        public static Vector3 GetEciPosition(TleData tle, DateTime utc)
        {
            double dt = (utc - tle.Epoch).TotalSeconds;

            double n = tle.MeanMotion * 2.0 * Math.PI / 86400.0; // rad/sec
            double a = Math.Pow(Mu / (n * n), 1.0 / 3.0);

            double M = (tle.MeanAnomaly * Math.PI / 180.0) + n * dt;

            // Solve Kepler (iterative)
            double E = M;
            for (int i = 0; i < 10; i++)
                E = M + tle.Eccentricity * Math.Sin(E);

            double v = 2 * Math.Atan2(
                Math.Sqrt(1 + tle.Eccentricity) * Math.Sin(E / 2),
                Math.Sqrt(1 - tle.Eccentricity) * Math.Cos(E / 2));

            double r = a * (1 - tle.Eccentricity * Math.Cos(E));

            // Orbital plane
            double xOrb = r * Math.Cos(v);
            double yOrb = r * Math.Sin(v);

            // Convert angles to radians
            double iRad = tle.Inclination * Math.PI / 180.0;
            double raanRad = tle.RAAN * Math.PI / 180.0;
            double argPerRad = tle.ArgumentPerigee * Math.PI / 180.0;

            double cosRAAN = Math.Cos(raanRad);
            double sinRAAN = Math.Sin(raanRad);
            double cosI = Math.Cos(iRad);
            double sinI = Math.Sin(iRad);
            double cosArg = Math.Cos(argPerRad);
            double sinArg = Math.Sin(argPerRad);

            // Rotation to ECI
            double x = (cosRAAN * cosArg - sinRAAN * sinArg * cosI) * xOrb
                     + (-cosRAAN * sinArg - sinRAAN * cosArg * cosI) * yOrb;

            double y = (sinRAAN * cosArg + cosRAAN * sinArg * cosI) * xOrb
                     + (-sinRAAN * sinArg + cosRAAN * cosArg * cosI) * yOrb;

            double z = (sinArg * sinI) * xOrb + (cosArg * sinI) * yOrb;

            return new Vector3(x, y, z);
        }

        // ============================
        // 🕒 GMST
        // ============================
        public static double GetGMST(DateTime utc)
        {
            double jd = ToJulianDate(utc);
            double T = (jd - 2451545.0) / 36525.0;

            double gmst = 280.46061837
                        + 360.98564736629 * (jd - 2451545.0)
                        + 0.000387933 * T * T
                        - T * T * T / 38710000.0;

            gmst = gmst % 360.0;
            if (gmst < 0) gmst += 360.0;

            return gmst * Math.PI / 180.0;
        }

        public static double ToJulianDate(DateTime utc)
        {
            int y = utc.Year;
            int m = utc.Month;
            double d = utc.Day + utc.Hour / 24.0 + utc.Minute / 1440.0 + utc.Second / 86400.0;

            if (m <= 2) { y--; m += 12; }

            int A = y / 100;
            int B = 2 - A + A / 4;

            return Math.Floor(365.25 * (y + 4716))
                 + Math.Floor(30.6001 * (m + 1))
                 + d + B - 1524.5;
        }

        // ============================
        // 🌍 ECI → ECEF
        // ============================
        public static Vector3 EciToEcef(Vector3 eci, DateTime utc)
        {
            double gmst = GetGMST(utc);

            double cos = Math.Cos(gmst);
            double sin = Math.Sin(gmst);

            return new Vector3(
                eci.X * cos + eci.Y * sin,
               -eci.X * sin + eci.Y * cos,
                eci.Z
            );
        }

        // ============================
        // 🌍 Observer → ECEF
        // ============================
        public static Vector3 ObserverToEcef(double latDeg, double lonDeg)
        {
            double lat = latDeg * Math.PI / 180.0;
            double lon = lonDeg * Math.PI / 180.0;

            double x = EarthRadiusKm * Math.Cos(lat) * Math.Cos(lon);
            double y = EarthRadiusKm * Math.Cos(lat) * Math.Sin(lon);
            double z = EarthRadiusKm * Math.Sin(lat);

            return new Vector3(x, y, z);
        }


        public static bool GetTleByName(string filePath, string satelliteName, out string line1, out string line2)
        {
            line1 = null;
            line2 = null;

            if (!File.Exists(filePath))
                return false;

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length - 2; i++)
            {
                string name = lines[i].Trim();

                if (name.Equals(satelliteName, StringComparison.OrdinalIgnoreCase))
                {
                    line1 = lines[i + 1].Trim();
                    line2 = lines[i + 2].Trim();
                    return true;
                }
            }

            return false;
        }


        // ============================
        // 📡 Elevation
        // ============================
        public static double GetElevation(Vector3 sat, Vector3 obs, double latDeg, double lonDeg)
        {
            double lat = latDeg * Math.PI / 180.0;
            double lon = lonDeg * Math.PI / 180.0;

            Vector3 d = sat - obs;

            double sinLat = Math.Sin(lat);
            double cosLat = Math.Cos(lat);
            double sinLon = Math.Sin(lon);
            double cosLon = Math.Cos(lon);

            double east = -sinLon * d.X + cosLon * d.Y;
            double north = -sinLat * cosLon * d.X - sinLat * sinLon * d.Y + cosLat * d.Z;
            double up = cosLat * cosLon * d.X + cosLat * sinLon * d.Y + sinLat * d.Z;

            return Math.Atan2(up, Math.Sqrt(east * east + north * north)) * 180.0 / Math.PI;
        }
    }
}
