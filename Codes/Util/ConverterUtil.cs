using GeoCalculator.Codes.Model;
using GeographicLib;

namespace GeoCalculator.Codes.Util {
    public static class ConverterUtil {
        static readonly Geocentric earth = new Geocentric(Constants.WGS84_a, Constants.WGS84_f);

        public static void GeocentricToGeodetic(Coordinates coordinates) {
            (coordinates.Latitude, coordinates.Longitude, coordinates.Altitude) = earth.Reverse(coordinates.X, coordinates.Y, coordinates.Z);
        }

        public static void GeodeticToGeocentric(Coordinates coordinates) {
            (coordinates.X, coordinates.Y, coordinates.Z) = earth.Forward(coordinates.Latitude, coordinates.Longitude, coordinates.Altitude);
        }

        public static void GeodeticToDMS(Coordinates coordinates) {
            // Process DMS Latitude
            coordinates.DMSLatitudeIsNorth = coordinates.Latitude >= 0;

            var absLatDegree = Math.Abs(coordinates.Latitude);
            coordinates.DMSLatitudeDegree = (int)absLatDegree;

            var absLatMin = (absLatDegree - coordinates.DMSLatitudeDegree) * 60;
            coordinates.DMSLatitudeMinute = (int)absLatMin;
            coordinates.DMSLatitudeSecond = (absLatMin - coordinates.DMSLatitudeMinute) * 60;

            // Process DMS Longtitude
            coordinates.DMSLongitudeIsEast = coordinates.Longitude >= 0;

            var absLonDegree = Math.Abs(coordinates.Longitude);
            coordinates.DMSLongitudeDegree = (int)absLonDegree;

            var absLonMin = (absLonDegree - coordinates.DMSLongitudeDegree) * 60;
            coordinates.DMSLongitudeMinute =(int)absLonMin;
            coordinates.DMSLongitudeSecond = (absLonMin - coordinates.DMSLongitudeMinute)*60;
        }

        public static void DMSToGeodetic(Coordinates coordinates) {
            coordinates.Latitude = coordinates.DMSLatitudeDegree + coordinates.DMSLatitudeMinute / 60.0 + coordinates.DMSLatitudeSecond / 3600.0;
            coordinates.Latitude *= coordinates.DMSLatitudeIsNorth ? 1 : -1;

            coordinates.Longitude = coordinates.DMSLongitudeDegree + coordinates.DMSLongitudeMinute / 60.0 + coordinates.DMSLongitudeSecond / 3600.0;
            coordinates.Longitude *= coordinates.DMSLongitudeIsEast ? 1 : -1;
        }
    }
}