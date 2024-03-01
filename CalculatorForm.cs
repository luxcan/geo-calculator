using GeoCalculator.Codes.Model;
using GeoCalculator.Codes.Util;
using GeographicLib;

namespace GeoCalculator {
    public partial class CalculatorForm : Form {
        private Coordinates? clipboardCoordinates = null;

        public CalculatorForm() {
            InitializeComponent();
        }

        private void btnConvertGencentric_Click(object sender, EventArgs e) {
            _ = double.TryParse(txtConvertX.Text, out double x);
            _ = double.TryParse(txtConvertY.Text, out double y);
            _ = double.TryParse(txtConvertZ.Text, out double z);
            _ = double.TryParse(txtAddAltitude.Text, out double addAltitude);

            var coordinates = new Coordinates();
            coordinates.SetGeocentric(x, y, z);

            if (addAltitude != 0) {
                coordinates.SetGeodetic(coordinates.Latitude, coordinates.Longitude, coordinates.Altitude + addAltitude);
            }

            UpdateConversionOutput(coordinates);
        }

        private void btnConvertLatLng_Click(object sender, EventArgs e) {
            _ = double.TryParse(txtConvertLat.Text, out double lat);
            _ = double.TryParse(txtConvertLng.Text, out double lng);
            _ = double.TryParse(txtConvertAlt.Text, out double altitude);
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

            _ = double.TryParse(txtConvertAlt.Text, out double altitude);

            var coordinates = new Coordinates();
            coordinates.SetDMS(latDegree, latMinute, latSecond, txtConvertDMSLatDirection.Text.Equals("N", StringComparison.InvariantCultureIgnoreCase),
                lngDegree, lngMinute, lngSecond, txtConvertDMSLngDirection.Text.Equals("E", StringComparison.InvariantCultureIgnoreCase),
                altitude);

            UpdateConversionOutput(coordinates);
        }

        private void UpdateConversionOutput(Coordinates coordinates) {
            txtConvertXOutput.Text = Math.Round(coordinates.X, 6).ToString();
            txtConvertYOutput.Text = Math.Round(coordinates.Y, 6).ToString();
            txtConvertZOutput.Text = Math.Round(coordinates.Z, 6).ToString();

            txtConvertLatOutput.Text = Math.Round(coordinates.Latitude, 6).ToString();
            txtConvertLngOutput.Text = Math.Round(coordinates.Longitude, 6).ToString();
            txtConvertAltOutput.Text = Math.Round(coordinates.Altitude, 6).ToString();

            txtConvertDMSLatOutput.Text = $"{coordinates.DMSLatitudeDegree}° {coordinates.DMSLatitudeMinute}' {Math.Round(coordinates.DMSLatitudeSecond, 6)}\" {(coordinates.DMSLatitudeIsNorth ? "N" : "S")}";
            txtConvertDMSLngOutput.Text = $"{coordinates.DMSLongitudeDegree}° {coordinates.DMSLongitudeMinute}' {Math.Round(coordinates.DMSLongitudeSecond, 6)}\" {(coordinates.DMSLongitudeIsEast ? "E" : "W")}";
        }

        private void btnDFCalculate_Click(object sender, EventArgs e) {
            txtDFResult.Text = "";

            _ = double.TryParse(txtDFLat.Text, out double lat);
            _ = double.TryParse(txtDFLng.Text, out double lng);
            _ = double.TryParse(txtDFAltitude.Text, out double altitude);
            _ = double.TryParse(txtDFDistance.Text, out double distance);
            var baseCoordinates = new Coordinates();
            baseCoordinates.SetGeodetic(lat, lng, altitude);

            var degreeList = txtDFDeg.Text.Split(',');
            foreach (var degreeStr in degreeList) {
                _ = double.TryParse(degreeStr, out double degree);
                var newCoordinates = CalculateDistanceFrom(baseCoordinates, degree, distance);

                // Display result
                txtDFResult.Text += $"Degree: {degree}°\r\n";
                txtDFResult.Text += $"X: {Round(newCoordinates.X)}\r\n";
                txtDFResult.Text += $"Y: {Round(newCoordinates.Y)}\r\n";
                txtDFResult.Text += $"Z: {Round(newCoordinates.Z)}\r\n";
                txtDFResult.Text += $"Latitude: {Round(newCoordinates.Latitude)}\r\n";
                txtDFResult.Text += $"Longitude: {Round(newCoordinates.Longitude)}\r\n";
                txtDFResult.Text += $"Altitude: {Round(newCoordinates.Altitude)}\r\n";
                txtDFResult.Text += $"DMS Latitude: {newCoordinates.DMSLatitudeDegree}° {newCoordinates.DMSLatitudeMinute}' {Round(newCoordinates.DMSLatitudeSecond)}\" {(newCoordinates.DMSLatitudeIsNorth ? "N" : "S")}\r\n";
                txtDFResult.Text += $"DMS Longitude: {newCoordinates.DMSLongitudeDegree}° {newCoordinates.DMSLongitudeMinute}' {Round(newCoordinates.DMSLongitudeSecond)}\" {(newCoordinates.DMSLongitudeIsEast ? "E" : "W")}\r\n";
                txtDFResult.Text += $"----------------------------------------\r\n";
            }
        }

        private Coordinates CalculateDistanceFrom(Coordinates baseCoordinates, double degree, double distanceInKM) {
            var geod = Geodesic.WGS84;
            geod.Direct(baseCoordinates.Latitude, baseCoordinates.Longitude, degree, distanceInKM * 1000, out double newLat, out double newLon);

            var coordinates = new Coordinates();
            coordinates.SetGeodetic(newLat, newLon, baseCoordinates.Altitude);
            return coordinates;
        }

        private double Round(double value) {
            return Math.Round(value, 4);
        }

        private void btnClipboardCopy_Click(object sender, EventArgs e) {
            var strLat = txtConvertLatOutput.Text;
            var strLng = txtConvertLngOutput.Text;
            var strAlt = txtConvertAltOutput.Text;

            if (strLat == "" || strLng == "" || strAlt == "") {
                MessageBox.Show("Empty coordinates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clipboardCoordinates = new Coordinates();
            clipboardCoordinates.SetGeodetic(double.Parse(strLat), double.Parse(strLng), double.Parse(strAlt));
            txtClipboard.Text = "Lat: " + clipboardCoordinates.Latitude + " | Lng: " + clipboardCoordinates.Longitude + " | Altitude: " + clipboardCoordinates.Altitude;
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e) {
            if (clipboardCoordinates == null) {
                MessageBox.Show("No coordinates in clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtDFLat.Text = clipboardCoordinates.Latitude.ToString();
            txtDFLng.Text = clipboardCoordinates.Longitude.ToString();
            txtDFAltitude.Text = clipboardCoordinates.Altitude.ToString();
        }
    }
}