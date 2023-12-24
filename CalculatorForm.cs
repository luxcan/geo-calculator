using GeoCalculator.Codes.Model;
using GeographicLib;

namespace GeoCalculator {
    public partial class CalculatorForm : Form {
        public CalculatorForm() {
            InitializeComponent();
        }

        private void btnConvertGencentric_Click(object sender, EventArgs e) {
            _ = double.TryParse(txtConvertXParam.Text, out double x);
            _ = double.TryParse(txtConvertYParam.Text, out double y);
            _ = double.TryParse(txtConvertZParam.Text, out double z);
            _ = double.TryParse(txtAddAltitude.Text, out double addAltitude);

            var coordinates = new Coordinates();
            coordinates.SetGeocentric(x, y, z);

            if (addAltitude != 0) {
                coordinates.SetGeodetic(coordinates.Latitude, coordinates.Longitude, coordinates.Altitude + addAltitude);
            }

            UpdateConversionOutput(coordinates);
        }

        private void btnConvertLatLng_Click(object sender, EventArgs e) {
            _ = double.TryParse(txtConvertLatParam.Text, out double lat);
            _ = double.TryParse(txtConvertLngParam.Text, out double lng);
            _ = double.TryParse(txtConvertAltParam.Text, out double altitude);
            _ = double.TryParse(txtAddAltitude.Text, out double addAltitude);
            altitude += addAltitude;

            var coordinates = new Coordinates();
            coordinates.SetGeodetic(lat, lng, altitude);

            UpdateConversionOutput(coordinates);
        }

        private void btnConvertDMS_Click(object sender, EventArgs e) {
            _ = int.TryParse(txtConvertDMSLatDeg.Text, out int latDegree);
            _ = int.TryParse(txtConvertDMSLatMin.Text, out int latMinute);
            _ = double.TryParse(txtConvertDMSLatSec.Text, out double latSecond);

            _ = int.TryParse(txtConvertDMSLngDeg.Text, out int lngDegree);
            _ = int.TryParse(txtConvertDMSLngMin.Text, out int lngMinute);
            _ = double.TryParse(txtConvertDMSLngSec.Text, out double lngSecond);

            _ = double.TryParse(txtConvertAltParam.Text, out double altitude);

            var coordinates = new Coordinates();
            coordinates.SetDMS(latDegree, latMinute, latSecond, txtConvertDMSLatDirection.Text.Equals("N", StringComparison.InvariantCultureIgnoreCase),
                lngDegree, lngMinute, lngSecond, txtConvertDMSLngDirection.Text.Equals("E", StringComparison.InvariantCultureIgnoreCase), 
                altitude);

            UpdateConversionOutput(coordinates);
        }

        private void UpdateConversionOutput(Coordinates coordinates) {
            txtConvertXOutput.Text = coordinates.X.ToString();
            txtConvertYOutput.Text = coordinates.Y.ToString();
            txtConvertZOutput.Text = coordinates.Z.ToString();

            txtConvertLatOutput.Text = coordinates.Latitude.ToString();
            txtConvertLngOutput.Text = coordinates.Longitude.ToString();
            txtConvertAltOutput.Text = coordinates.Altitude.ToString();

            txtConvertDMSLatOutput.Text = $"{coordinates.DMSLatitudeDegree}° {coordinates.DMSLatitudeMinute}' {coordinates.DMSLatitudeSecond}\" {(coordinates.DMSLatitudeIsNorth ? "N" : "S")}";
            txtConvertDMSLngOutput.Text = $"{coordinates.DMSLongitudeDegree}° {coordinates.DMSLongitudeMinute}' {coordinates.DMSLongitudeSecond}\" {(coordinates.DMSLongitudeIsEast ? "E" : "W")}";
        }
    }
}