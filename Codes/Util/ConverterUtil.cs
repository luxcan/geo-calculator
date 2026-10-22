using GeoCalculator.Codes.Model;
using GeographicLib;

namespace GeoCalculator.Codes.Util {
    public static class ConverterUtil {
        public const double MinutesPerDegree = 60;
        public const double SecondsPerMinute = 60;
        public const double SecondsPerDegree = 3600;

        static readonly Geocentric earth = new Geocentric(Constants.WGS84_a, Constants.WGS84_f);

        public static void GeocentricToGeodetic(Coordinates coordinates) {
            (coordinates.Latitude, coordinates.Longitude, coordinates.Altitude) = earth.Reverse(coordinates.X, coordinates.Y, coordinates.Z);
        }

        public static void GeodeticToGeocentric(Coordinates coordinates) {
            (coordinates.X, coordinates.Y, coordinates.Z) = earth.Forward(coordinates.Latitude, coordinates.Longitude, coordinates.Altitude);
        }

        /// <summary>
        /// Converts degrees, minutes and seconds (all zero or positive) and a hemisphere to signed decimal degrees.
        /// </summary>
        /// <param name="isPositiveHemisphere">True for north or east, false for south or west.</param>
        public static double DmsToDegrees(double degree, double minute, double second, bool isPositiveHemisphere) {
            double degrees = degree + minute / MinutesPerDegree + second / SecondsPerDegree;
            return isPositiveHemisphere ? degrees : -degrees;
        }
    }
}
