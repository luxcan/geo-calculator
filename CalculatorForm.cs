using GeoCalculator.Codes.Model;
using GeoCalculator.Codes.Util;
using System.Globalization;

namespace GeoCalculator {
    public partial class CalculatorForm : Form {
        // Output precision: 1e-8 degree and 1e-5 arc-second are about 1 mm on the ground, 1e-4 m is 0.1 mm
        private const int DegreeDecimals = 8;
        private const int MetreDecimals = 4;
        private const int ArcSecondDecimals = 5;
        private const int InputDecimals = 12;
        private const string InputNumberFormat = "0.############";
        private const string RoundTripFormat = "R";
        private const double SecondsPerDegree = 3600;
        private const double SecondsPerMinute = 60;

        private const double MinLatitudeDegree = -90;
        private const double MaxLatitudeDegree = 90;
        private const double MinLongitudeDegree = -180;
        private const double MaxLongitudeDegree = 180;
        private const double MinAzimuthDegree = -360;
        private const double MaxAzimuthDegree = 360;
        private const double MinElevationDegree = -90;
        private const double MaxElevationDegree = 90;
        // Earth's equatorial circumference: a longer surface distance only wraps round the earth again
        private const double MaxDistanceKm = 40075;
        // From below the deepest ocean trench to far beyond geostationary orbit
        private const double MinAltitudeMetre = -12000;
        private const double MaxAltitudeMetre = 100000000;

        private const char AzimuthDelimiter = ',';
        private const string RunningNoPlaceholder = "${runningNo}";
        private static readonly string ResultSeparator = new string('-', 93);
        private const string InputErrorCaption = "Invalid Input";

        private Coordinates? lastConvertedCoordinates = null;
        private Coordinates? clipboardCoordinates = null;

        public CalculatorForm() {
            InitializeComponent();

            tipIconDFAzimuth.Image = SystemIcons.Information.ToBitmap();
            tipIconDFAzimuth.SizeMode = PictureBoxSizeMode.Zoom;
            tipDFAzimuth.SetToolTip(tipIconDFAzimuth,
                "Generate multiple coordinates with different azimuths by using a comma (,) as the delimiter.\n"
                + "For example, to generate coordinates at 0 degrees and 90 degrees from the target, enter 0,90.\n"
                + "Azimuth is measured clockwise from true north.");

            tipIconDFReplaceOutput.Image = SystemIcons.Information.ToBitmap();
            tipIconDFReplaceOutput.SizeMode = PictureBoxSizeMode.Zoom;
            tipDFReplaceOutput.SetToolTip(tipIconDFReplaceOutput, "Insert the following placeholders into the output where you want the\ncorresponding values to appear:\n - ${x}: X-coordinate\n - ${y}: Y-coordinate\n - ${z}: Z-coordinate\n - ${lat}: Latitude\n - ${lon}: Longitude\n - ${altitude}: Altitude\n - ${runningNo}: A sequence of running numbers");

            var distanceTip = "Without an elevation angle: distance along the WGS84 ellipsoid surface (geodesic).\n"
                + "The new coordinate keeps the target altitude.\n"
                + "With an elevation angle: straight-line (slant) range from the target along the azimuth\n"
                + "and elevation angle. Earth curvature is accounted for.\n"
                + $"From 0 to {FormatInput(MaxDistanceKm)} km.";
            tipDFInputs.SetToolTip(lblDFDistance, distanceTip);
            tipDFInputs.SetToolTip(txtDFDistance, distanceTip);

            var elevationTip = "Optional. Angle above (+) or below (-) the target's local horizontal plane,\n"
                + $"from {FormatInput(MinElevationDegree)} to {FormatInput(MaxElevationDegree)} degrees. "
                + "0 is a horizontal line of sight, which rises above the curved earth\n"
                + "with distance. Leave blank to move along the ellipsoid surface at the target altitude.";
            tipDFInputs.SetToolTip(lblDFElevation, elevationTip);
            tipDFInputs.SetToolTip(txtDFElevation, elevationTip);

            var altitudeTip = "Height above the WGS84 ellipsoid (HAE) in metres, not height above mean sea level.";
            tipDFInputs.SetToolTip(lblDFAltitude, altitudeTip);
            tipDFInputs.SetToolTip(txtDFAltitude, altitudeTip);
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
            // Keep the full-precision result for the clipboard; the text boxes below only show a rounded copy
            lastConvertedCoordinates = coordinates;

            txtConvertXOutput.Text = Math.Round(coordinates.X, 6).ToString();
            txtConvertYOutput.Text = Math.Round(coordinates.Y, 6).ToString();
            txtConvertZOutput.Text = Math.Round(coordinates.Z, 6).ToString();

            txtConvertLatOutput.Text = Math.Round(coordinates.Latitude, 6).ToString();
            txtConvertLngOutput.Text = Math.Round(coordinates.Longitude, 6).ToString();
            txtConvertAltOutput.Text = Math.Round(coordinates.Altitude, 6).ToString();

            txtConvertDMSLatOutput.Text = FormatDMS(coordinates.Latitude, "N", "S");
            txtConvertDMSLngOutput.Text = FormatDMS(coordinates.Longitude, "E", "W");
        }

        private void btnDFCalculate_Click(object sender, EventArgs e) {
            txtDFRichResult.Text = "";

            // Validate every input first, so that a typo is reported instead of being silently treated as 0
            var errors = new List<string>();
            double lat = ParseNumber(txtDFLat.Text, "Lat", MinLatitudeDegree, MaxLatitudeDegree, errors);
            double lng = ParseNumber(txtDFLng.Text, "Lng", MinLongitudeDegree, MaxLongitudeDegree, errors);
            double altitude = ParseNumber(txtDFAltitude.Text, "Altitude (m)", MinAltitudeMetre, MaxAltitudeMetre,
                errors);
            double distance = ParseNumber(txtDFDistance.Text, "Distance Apart (km)", 0, MaxDistanceKm, errors);
            List<double> azimuths = ParseAzimuthList(txtDFDeg.Text, errors);

            // A blank elevation angle means "move along the ellipsoid surface"
            double? elevation = null;
            if (txtDFElevation.Text.Trim() != "") {
                elevation = ParseNumber(txtDFElevation.Text, "Elevation Angle (Degree)",
                    MinElevationDegree, MaxElevationDegree, errors);
            }

            var replaceText = cbDFReplaceText.Checked;
            var replaceTextTemplate = txtDFReplaceText.Text;
            long runningNo = 0;
            if (replaceText && replaceTextTemplate.Contains(RunningNoPlaceholder)) {
                var runningNoText = txtDFReplaceRunningNo.Text.Trim();
                if (runningNoText == "") {
                    errors.Add($"Running No is required when the output text contains {RunningNoPlaceholder}.");
                } else if (!long.TryParse(runningNoText, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out runningNo)) {
                    errors.Add($"Running No: \"{runningNoText}\" is not a valid whole number.");
                } else if (runningNo > long.MaxValue - Math.Max(azimuths.Count - 1, 0)) {
                    errors.Add($"Running No: \"{runningNoText}\" is too large for {azimuths.Count} results.");
                }
            }

            if (errors.Count > 0) {
                ShowInputError(string.Join(Environment.NewLine, errors.Distinct()));
                return;
            }

            var baseCoordinates = new Coordinates();
            baseCoordinates.SetGeodetic(lat, lng, altitude);

            // Calculate every point first, so that an invalid result is reported before anything is shown
            var results = new List<(double Azimuth, Coordinates Coordinates)>();
            foreach (double azimuth in azimuths) {
                Coordinates newCoordinates = elevation.HasValue
                    ? DistanceUtil.CalculateSlantRangeFrom(baseCoordinates, azimuth, elevation.Value, distance)
                    : DistanceUtil.CalculateDistanceFrom(baseCoordinates, azimuth, distance);
                results.Add((azimuth, newCoordinates));
            }

            if (results.Any(result => !IsValidCoordinates(result.Coordinates))) {
                ShowInputError("The calculation produced an invalid coordinate. "
                    + "Check the target coordinate, the distance and the angles.");
                return;
            }

            using var boldFont = new Font(txtDFRichResult.Font, FontStyle.Bold);
            foreach (var (azimuth, newCoordinates) in results) {
                if (!replaceText) {
                    AppendDetailedResult(BuildResultHeader(azimuth, elevation, distance), newCoordinates, boldFont);
                } else {
                    var content = replaceTextTemplate.Replace("${x}", FormatMetre(newCoordinates.X))
                        .Replace("${y}", FormatMetre(newCoordinates.Y))
                        .Replace("${z}", FormatMetre(newCoordinates.Z))
                        .Replace("${lat}", FormatDegree(newCoordinates.Latitude))
                        .Replace("${lon}", FormatDegree(newCoordinates.Longitude))
                        .Replace("${altitude}", FormatMetre(newCoordinates.Altitude))
                        .Replace(RunningNoPlaceholder, runningNo.ToString(CultureInfo.InvariantCulture));
                    txtDFRichResult.AppendText(content + "\r\n");
                    runningNo++;
                }
            }
        }

        /// <summary>
        /// Builds the bold header line that describes how one result was calculated.
        /// </summary>
        private static string BuildResultHeader(double azimuth, double? elevation, double distance) {
            string header;
            if (elevation.HasValue) {
                header = $"Azimuth: {FormatInput(azimuth)}°  Elevation: {FormatInput(elevation.Value)}°  "
                    + $"Slant Range: {FormatInput(distance)} km";
            } else {
                header = $"Azimuth: {FormatInput(azimuth)}°  Surface Distance: {FormatInput(distance)} km";
            }
            return header;
        }

        /// <summary>
        /// Appends one calculated coordinate to the result box, with the header line in bold.
        /// </summary>
        private void AppendDetailedResult(string header, Coordinates coordinates, Font boldFont) {
            // RichTextBox stores each line break as a single '\n', so TextLength is the index where the header starts
            int headerStart = txtDFRichResult.TextLength;
            txtDFRichResult.AppendText(header + "\r\n");
            txtDFRichResult.Select(headerStart, header.Length);
            txtDFRichResult.SelectionFont = boldFont;
            txtDFRichResult.DeselectAll();

            txtDFRichResult.AppendText($"X: {FormatMetre(coordinates.X)}\r\n");
            txtDFRichResult.AppendText($"Y: {FormatMetre(coordinates.Y)}\r\n");
            txtDFRichResult.AppendText($"Z: {FormatMetre(coordinates.Z)}\r\n");
            txtDFRichResult.AppendText($"Latitude:  {FormatDegree(coordinates.Latitude)}\r\n");
            txtDFRichResult.AppendText($"Longitude: {FormatDegree(coordinates.Longitude)}\r\n");
            txtDFRichResult.AppendText($"Altitude (m):  {FormatMetre(coordinates.Altitude)}\r\n");
            txtDFRichResult.AppendText($"DMS Latitude:    {FormatDMS(coordinates.Latitude, "N", "S")}\r\n");
            txtDFRichResult.AppendText($"DMS Longitude: {FormatDMS(coordinates.Longitude, "E", "W")}\r\n");
            txtDFRichResult.AppendText(ResultSeparator + "\r\n");
        }

        /// <summary>
        /// Parses a number typed by the operator and checks that it lies within [minValue, maxValue].
        /// The invariant culture is used ('.' is the decimal point and thousands separators are rejected),
        /// so a mistyped comma is reported instead of being silently read as a thousands separator.
        /// Any problem is added to errors and 0 is returned.
        /// </summary>
        private static double ParseNumber(string text, string fieldName, double minValue, double maxValue,
            List<string> errors) {
            double value = 0;
            var trimmedText = text.Trim();
            if (trimmedText == "") {
                errors.Add($"{fieldName} is required.");
            } else if (!double.TryParse(trimmedText, NumberStyles.Float, CultureInfo.InvariantCulture,
                out double parsedValue) || !double.IsFinite(parsedValue)) {
                errors.Add($"{fieldName}: \"{trimmedText}\" is not a valid number.");
            } else if (parsedValue < minValue || parsedValue > maxValue) {
                errors.Add($"{fieldName}: {trimmedText} is outside the range "
                    + $"{FormatInput(minValue)} to {FormatInput(maxValue)}.");
            } else {
                value = parsedValue;
            }
            return value;
        }

        /// <summary>
        /// Parses the comma-separated azimuth list. Empty entries (e.g. a trailing comma) are reported as errors.
        /// </summary>
        private static List<double> ParseAzimuthList(string text, List<string> errors) {
            var azimuths = new List<double>();
            var azimuthTexts = text.Split(AzimuthDelimiter);
            foreach (var azimuthText in azimuthTexts) {
                if (azimuthTexts.Length > 1 && azimuthText.Trim() == "") {
                    errors.Add("Azimuth (Degree): the list contains an empty value. Remove the extra comma.");
                } else {
                    azimuths.Add(ParseNumber(azimuthText, "Azimuth (Degree)", MinAzimuthDegree, MaxAzimuthDegree,
                        errors));
                }
            }
            return azimuths;
        }

        private static void ShowInputError(string message) {
            MessageBox.Show(message, InputErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Echoes an operator input in plain decimal notation (no exponent, no "-0").
        /// </summary>
        private static string FormatInput(double value) {
            // Round to the decimals shown first, then add 0.0, so that a tiny negative value prints as 0 instead of -0
            double roundedValue = Math.Round(value, InputDecimals) + 0.0;
            return roundedValue.ToString(InputNumberFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a value with a fixed number of decimals in the invariant culture.
        /// A value that rounds to zero is printed as 0 rather than -0.
        /// </summary>
        private static string FormatFixed(double value, int decimals) {
            double roundedValue = Math.Round(value, decimals);
            if (roundedValue == 0) {
                roundedValue = 0;
            }
            return roundedValue.ToString("F" + decimals, CultureInfo.InvariantCulture);
        }

        private static string FormatDegree(double value) {
            return FormatFixed(value, DegreeDecimals);
        }

        private static string FormatMetre(double value) {
            return FormatFixed(value, MetreDecimals);
        }

        /// <summary>
        /// Formats decimal degrees as degrees, minutes and seconds. The total arc-seconds are rounded once before
        /// they are split, so that e.g. 103.8 prints as 103° 48' 0" instead of 103° 47' 60".
        /// </summary>
        private static string FormatDMS(double degreeValue, string positiveHemisphere, string negativeHemisphere) {
            string dms;
            // NaN, infinity or a value too large for whole degrees in an int cannot be shown as DMS
            if (!(Math.Abs(degreeValue) < int.MaxValue)) {
                dms = "Not a valid angle";
            } else {
                double totalSeconds = Math.Round(Math.Abs(degreeValue) * SecondsPerDegree, ArcSecondDecimals);

                // Mixed mode: whole degrees and whole minutes are taken from the rounded total as integers
                int degree = (int)(totalSeconds / SecondsPerDegree);
                int minute = (int)((totalSeconds - degree * SecondsPerDegree) / SecondsPerMinute);
                double second = totalSeconds - degree * SecondsPerDegree - minute * SecondsPerMinute;

                var hemisphere = degreeValue < 0 && totalSeconds > 0 ? negativeHemisphere : positiveHemisphere;
                dms = $"{degree}° {minute}' {FormatFixed(second, ArcSecondDecimals)}\" {hemisphere}";
            }
            return dms;
        }

        /// <summary>
        /// True when all values are finite numbers, the latitude is within -90 to 90 degrees and the longitude is
        /// within -180 to 180 degrees.
        /// </summary>
        private static bool IsValidCoordinates(Coordinates coordinates) {
            return double.IsFinite(coordinates.X) && double.IsFinite(coordinates.Y) && double.IsFinite(coordinates.Z)
                && double.IsFinite(coordinates.Altitude)
                && coordinates.Latitude >= MinLatitudeDegree && coordinates.Latitude <= MaxLatitudeDegree
                && coordinates.Longitude >= MinLongitudeDegree && coordinates.Longitude <= MaxLongitudeDegree;
        }

        private void btnClipboardCopy_Click(object sender, EventArgs e) {
            if (lastConvertedCoordinates == null) {
                ShowInputError("No converted coordinates to copy. Convert a coordinate first.");
                return;
            }

            if (!IsValidCoordinates(lastConvertedCoordinates)) {
                ShowInputError("The converted coordinates are not valid: every value must be a number, the latitude "
                    + $"must be within {FormatInput(MinLatitudeDegree)} to {FormatInput(MaxLatitudeDegree)} "
                    + $"and the longitude within {FormatInput(MinLongitudeDegree)} to "
                    + $"{FormatInput(MaxLongitudeDegree)}. Correct the input and convert again.");
                return;
            }

            // Copy the full-precision result, not the rounded text shown in the output boxes
            clipboardCoordinates = lastConvertedCoordinates;
            txtClipboard.Text = $"Lat: {FormatDegree(clipboardCoordinates.Latitude)}"
                + $" | Lng: {FormatDegree(clipboardCoordinates.Longitude)}"
                + $" | Altitude: {FormatMetre(clipboardCoordinates.Altitude)}";
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e) {
            if (clipboardCoordinates == null) {
                ShowInputError("No coordinates in clipboard. Convert a coordinate and press Copy first.");
                return;
            }

            // The round-trip format writes every digit needed to read back exactly the same value
            txtDFLat.Text = clipboardCoordinates.Latitude.ToString(RoundTripFormat, CultureInfo.InvariantCulture);
            txtDFLng.Text = clipboardCoordinates.Longitude.ToString(RoundTripFormat, CultureInfo.InvariantCulture);
            txtDFAltitude.Text = clipboardCoordinates.Altitude.ToString(RoundTripFormat, CultureInfo.InvariantCulture);
        }
    }
}
