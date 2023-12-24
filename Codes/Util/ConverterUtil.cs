using GeoCalculator.Codes.Model;
using GeographicLib;

namespace GeoCalculator.Codes.Util {
    public static class ConverterUtil {
        static Geocentric earth = new Geocentric(Constants.WGS84_a, Constants.WGS84_f);

        public static void GeocentricToGeodetic(Coordinates coordinates) {
            (coordinates.Latitude, coordinates.Longitude, coordinates.Altitude) = earth.Reverse(coordinates.X, coordinates.Y, coordinates.Z);
        }

        public static void GeodeticToGeocentric(Coordinates coordinates) {
            (coordinates.X, coordinates.Y, coordinates.Z) = earth.Forward(coordinates.Latitude, coordinates.Longitude, coordinates.Altitude);
        }

        public static void GeodeticToDMS(Coordinates coordinates) {
            coordinates.DMSLatitudeIsNorth = coordinates.Latitude >= 0;
            coordinates.DMSLatitudeDegree = (int)coordinates.Latitude;
            coordinates.DMSLatitudeMinute = (int)((coordinates.Latitude - coordinates.DMSLatitudeDegree) * 60);
            coordinates.DMSLatitudeSecond = ((coordinates.Latitude - coordinates.DMSLatitudeDegree) * 60 - coordinates.DMSLatitudeMinute) * 60;

            coordinates.DMSLongitudeIsEast = coordinates.Longitude >= 0;
            coordinates.DMSLongitudeDegree = (int)coordinates.Longitude;
            coordinates.DMSLongitudeMinute = (int)((coordinates.Longitude - coordinates.DMSLongitudeDegree) * 60);
            coordinates.DMSLongitudeSecond = ((coordinates.Longitude - coordinates.DMSLongitudeDegree) * 60 - coordinates.DMSLongitudeMinute) * 60;
        }

        public static void DMSToGeodetic(Coordinates coordinates) {
            coordinates.Latitude = coordinates.DMSLatitudeDegree + coordinates.DMSLatitudeMinute / 60.0 + coordinates.DMSLatitudeSecond / 3600.0;
            coordinates.Latitude *= coordinates.DMSLatitudeIsNorth ? 1 : -1;

            coordinates.Longitude = coordinates.DMSLongitudeDegree + coordinates.DMSLongitudeMinute / 60.0 + coordinates.DMSLongitudeSecond / 3600.0;
            coordinates.Longitude *= coordinates.DMSLongitudeIsEast ? 1 : -1;
        }
    }
}