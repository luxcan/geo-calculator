using GeoCalculator.Codes.Util;

namespace GeoCalculator.Codes.Model {
    public class Coordinates {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public int DMSLatitudeDegree { get; set; }
        public int DMSLatitudeMinute { get; set; }
        public double DMSLatitudeSecond { get; set; }
        public bool DMSLatitudeIsNorth { get; set; }

        public int DMSLongitudeDegree { get; set; }
        public int DMSLongitudeMinute { get; set; }
        public double DMSLongitudeSecond { get; set; }
        public bool DMSLongitudeIsEast { get; set; }

        public void SetGeocentric(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
            ConverterUtil.GeocentricToGeodetic(this);
            ConverterUtil.GeodeticToDMS(this);
        }

        public void SetGeodetic(double latitude, double longitude, double altitude) {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            ConverterUtil.GeodeticToGeocentric(this);
            ConverterUtil.GeodeticToDMS(this);
        }

        public void SetDMS(int latDegree, int latMinute, double latSecond, bool latIsNorth, int lngDegree, int lngMinute, double lngSecond, bool lngIsEast, double altitude = 0) {
            DMSLatitudeDegree = latDegree;
            DMSLatitudeMinute = latMinute;
            DMSLatitudeSecond = latSecond;
            DMSLatitudeIsNorth = latIsNorth;

            DMSLongitudeDegree = lngDegree;
            DMSLongitudeMinute = lngMinute;
            DMSLongitudeSecond = lngSecond;
            DMSLongitudeIsEast = lngIsEast;

            Altitude = altitude;

            ConverterUtil.DMSToGeodetic(this);
            ConverterUtil.GeodeticToGeocentric(this);
        }

        public void UpdateAltitude(double newAltitude) {
            Altitude = newAltitude;
            ConverterUtil.GeodeticToGeocentric(this);
            ConverterUtil.GeodeticToDMS(this);
        }
    }
}