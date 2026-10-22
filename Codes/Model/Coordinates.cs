using GeoCalculator.Codes.Util;

namespace GeoCalculator.Codes.Model {
    /// <summary>
    /// One point on or above the WGS84 ellipsoid, kept in both geocentric (ECEF, metres) and geodetic form
    /// (degrees and height above the ellipsoid in metres).
    /// </summary>
    public class Coordinates {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public void SetGeocentric(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
            ConverterUtil.GeocentricToGeodetic(this);
        }

        public void SetGeodetic(double latitude, double longitude, double altitude) {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            ConverterUtil.GeodeticToGeocentric(this);
        }
    }
}
