using GeoCalculator.Codes.Controls;
using GeoCalculator.Codes.Model;
using GeoCalculator.Codes.Util;
using System.Globalization;

namespace GeoCalculator {
    public partial class CalculatorForm : Form {
        // Output precision: 1e-8 degree and 1e-5 arc-second are about 1 mm on the ground, 1e-4 m is 0.1 mm
        private const int DegreeDecimals = 8;
        private const int MetreDecimals = 4;
        private const int ArcSecondDecimals = 5;
        private const string RoundTripFormat = "R";
        private const string TimeFormat = "HH:mm:ss";
        private const double SecondsPerDegree = ConverterUtil.SecondsPerDegree;
        private const double SecondsPerMinute = ConverterUtil.SecondsPerMinute;
        // DMS minutes and seconds must be from 0 to below this
        private const double DmsPartLimit = 60;

        private const double MinLatitudeDegree = -90;
        private const double MaxLatitudeDegree = 90;
        private const double MinLongitudeDegree = -180;
        private const double MaxLongitudeDegree = 180;
        private const double MinElevationDegree = -90;
        private const double MaxElevationDegree = 90;
        // Earth's equatorial circumference: a longer surface distance only wraps round the earth again
        private const double MaxDistanceKm = 40075;
        // From below the deepest ocean trench to far beyond geostationary orbit
        private const double MinAltitudeMetre = -12000;
        private const double MaxAltitudeMetre = 100000000;
        // The earth's radius plus the highest supported altitude, rounded up
        private const double MaxGeocentricMetre = 110000000;

        private const string RunningNoPlaceholder = "${runningNo}";
        private const string AzimuthPlaceholder = "${azimuth}";
        private static readonly string ResultSeparator = new string('-', 93);
        private const string InputErrorCaption = "Invalid Input";
        private const string CopyRangeCaption = "Copy Range to List";
        // Shown in the preview table while the target, distance or elevation cannot be calculated yet
        private const string NotAvailableText = "–";
        // The preview is rebuilt this long after the operator stops typing
        private const int PreviewDebounceMilliseconds = 200;
        // Joins the input values into one snapshot string; it cannot be typed into a text box
        private const char SnapshotSeparator = '\u001F';

        /// <summary>
        /// Coordinate types that the detailed output can show for each result.
        /// </summary>
        [Flags]
        private enum OutputParts {
            None = 0,
            Geocentric = 1,
            Geodetic = 2,
            Dms = 4
        }

        private readonly Font boldUiFont;
        private readonly Bitmap errorIcon = SystemIcons.Error.ToBitmap();
        private readonly Bitmap warningIcon = SystemIcons.Warning.ToBitmap();
        private readonly Bitmap infoIcon = SystemIcons.Information.ToBitmap();

        private Coordinates? lastConvertedCoordinates = null;
        private Coordinates? clipboardCoordinates = null;

        // The azimuth set built from the current input; the preview, Calculate and the output all use it
        private AzimuthPlan currentPlan = new AzimuthPlan(AzimuthMode.List);
        // Inputs and status line of the last successful Calculate, used to flag an out-of-date output
        private string? calculatedSnapshot;
        private string calculatedStatus = "";
        private bool outputIsDetailed;
        // Where each result block starts in the output, so that selecting an azimuth can scroll to it
        private readonly List<int> resultBlockStarts = new List<int>();
        private bool isRefreshingAzimuthTable;

        public CalculatorForm() {
            InitializeComponent();
            boldUiFont = new Font(Font, FontStyle.Bold);
            // Column widths are in pixels and do not follow the form's automatic scaling, so scale them to the screen
            foreach (ColumnHeader column in lvDFAzimuths.Columns) {
                // Mixed mode: whole-pixel widths are scaled by the dpi ratio and rounded back to whole pixels
                column.Width = (int)Math.Round(column.Width * DeviceDpi / PreviewStyle.DesignDpi);
            }
            Load += CalculatorForm_Load;
            Disposed += CalculatorForm_Disposed;

            tipIconDFAzimuth.Image = infoIcon;
            tipIconDFAzimuth.SizeMode = PictureBoxSizeMode.Zoom;
            tipDFAzimuth.SetToolTip(tipIconDFAzimuth,
                "List: one or more azimuths separated by commas, e.g. 0,90.\n"
                + "They are used as typed, in the order typed.\n"
                + "Range: start, end and step, e.g. 0 to 90 every 15 gives 0, 15, 30, 45, 60, 75, 90.\n"
                + "A range runs from the start in the chosen direction. The start is always included;\n"
                + "the end is included when it falls on a step.\n"
                + "A range can pass through north (300 to 60 clockwise). A full circle (0 to 360) lists 0 once.\n"
                + "Azimuth is measured clockwise from true north, "
                + $"{InputUtil.FormatInput(AzimuthPlanBuilder.MinAzimuthDegree)} to "
                + $"{InputUtil.FormatInput(AzimuthPlanBuilder.MaxAzimuthDegree)} degrees. "
                + $"At most {AzimuthPlanBuilder.MaxAzimuthCount} azimuths.");

            tipIconDFReplaceOutput.Image = infoIcon;
            tipIconDFReplaceOutput.SizeMode = PictureBoxSizeMode.Zoom;
            tipDFReplaceOutput.SetToolTip(tipIconDFReplaceOutput,
                "Insert the following placeholders into the output where you want the\n"
                + "corresponding values to appear:\n - ${x}: X-coordinate\n - ${y}: Y-coordinate\n"
                + " - ${z}: Z-coordinate\n - ${lat}: Latitude\n - ${lon}: Longitude\n - ${altitude}: Altitude\n"
                + " - ${azimuth}: Azimuth (degrees, as printed in the header)\n"
                + " - ${runningNo}: A sequence of running numbers");

            var distanceTip = "Without an elevation angle: distance along the WGS84 ellipsoid surface (geodesic).\n"
                + "The new coordinate keeps the target altitude.\n"
                + "With an elevation angle: straight-line (slant) range from the target along the azimuth\n"
                + "and elevation angle. Earth curvature is accounted for.\n"
                + $"From 0 to {InputUtil.FormatInput(MaxDistanceKm)} km.";
            tipInputs.SetToolTip(lblDFDistance, distanceTip);
            tipInputs.SetToolTip(txtDFDistance, distanceTip);

            var elevationTip = "Optional. Angle above (+) or below (-) the target's local horizontal plane,\n"
                + $"from {InputUtil.FormatInput(MinElevationDegree)} to "
                + $"{InputUtil.FormatInput(MaxElevationDegree)} degrees. "
                + "0 is a horizontal line of sight, which rises above the curved earth\n"
                + "with distance. Leave blank to move along the ellipsoid surface at the target altitude.";
            tipInputs.SetToolTip(lblDFElevation, elevationTip);
            tipInputs.SetToolTip(txtDFElevation, elevationTip);

            var altitudeTip = "Height above the WGS84 ellipsoid (HAE) in metres, not height above mean sea level.";
            tipInputs.SetToolTip(lblDFAltitude, altitudeTip);
            tipInputs.SetToolTip(txtDFAltitude, altitudeTip);

            var azimuthLimits = $"{InputUtil.FormatInput(AzimuthPlanBuilder.MinAzimuthDegree)} to "
                + $"{InputUtil.FormatInput(AzimuthPlanBuilder.MaxAzimuthDegree)} degrees";
            tipInputs.SetToolTip(txtDFAzimuthStart, $"First azimuth of the range, {azimuthLimits}. Always included.");
            tipInputs.SetToolTip(txtDFAzimuthEnd,
                $"Last azimuth of the range, {azimuthLimits}. Included when it falls on a step.");
            tipInputs.SetToolTip(txtDFAzimuthStep,
                "Degrees between azimuths: more than 0, up to "
                + $"{InputUtil.FormatInput(AzimuthPlanBuilder.MaxStepDegree)}.");
            tipInputs.SetToolTip(rbDFAzimuthClockwise, "Azimuth increases from the start, e.g. 300, 330, 0, 30.");
            tipInputs.SetToolTip(rbDFAzimuthAnticlockwise, "Azimuth decreases from the start, e.g. 60, 30, 0, 330.");
            tipInputs.SetToolTip(btnDFAzimuthCopyToList,
                "Copies the range into the List box so that single values can be removed or added.");

            var showTip = "Choose which coordinates each result shows. All are shown by default.\n"
                + "Replace Output uses its own placeholders instead.";
            tipInputs.SetToolTip(lblDFOutputShow, showTip);
            tipInputs.SetToolTip(cbDFShowGeocentric, "Geocentric (ECEF) X, Y, Z in metres.");
            tipInputs.SetToolTip(cbDFShowGeodetic,
                "Latitude and longitude in degrees, and altitude in metres (HAE).");
            tipInputs.SetToolTip(cbDFShowDMS, "Latitude and longitude in degrees, minutes and seconds.");

            // Converter tab: the hemisphere is picked, never typed, so a blank or mistyped letter cannot flip it
            cmbConvertDMSLatHemisphere.SelectedIndex = 0;
            cmbConvertDMSLngHemisphere.SelectedIndex = 0;
            tipInputs.SetToolTip(cmbConvertDMSLatHemisphere, "N for north of the equator, S for south.");
            tipInputs.SetToolTip(cmbConvertDMSLngHemisphere, "E for east of Greenwich, W for west.");
            var dmsTip = "Whole degrees; minutes and seconds from 0 to below 60 (blank = 0).";
            foreach (var dmsBox in new[] { txtConvertDMSLatDeg, txtConvertDMSLatMin, txtConvertDMSLatSec,
                txtConvertDMSLngDeg, txtConvertDMSLngMin, txtConvertDMSLngSec }) {
                tipInputs.SetToolTip(dmsBox, dmsTip);
            }
            tipInputs.SetToolTip(txtConvertAlt, altitudeTip);
            tipInputs.SetToolTip(txtConvertDMSAlt, altitudeTip);
            tipInputs.SetToolTip(btnClipboardCopy,
                "Copies the converted point to the Clipboard box below,\n"
                + "for Paste From Clipboard on the Distance From tab.");

            tmrDFAzimuthPreview.Interval = PreviewDebounceMilliseconds;
            UpdateAzimuthModeEnabled();
            UpdateOutputShowEnabled();
            RefreshAzimuthPreview();
        }

        private void CalculatorForm_Load(object? sender, EventArgs e) {
            // The window does not fit every screen (e.g. 1366 x 768 at 125 %): scroll instead of clipping it
            var workingArea = Screen.FromControl(this).WorkingArea;
            if (Width > workingArea.Width || Height > workingArea.Height) {
                AutoScroll = true;
                Size = new Size(Math.Min(Width, workingArea.Width), Math.Min(Height, workingArea.Height));
                Location = workingArea.Location;
            }
        }

        private void CalculatorForm_Disposed(object? sender, EventArgs e) {
            boldUiFont.Dispose();
            errorIcon.Dispose();
            warningIcon.Dispose();
            infoIcon.Dispose();
        }

        private void btnConvertGencentric_Click(object sender, EventArgs e) {
            var errors = new List<string>();
            double x = InputUtil.ParseNumber(txtConvertX.Text, "X (m)", -MaxGeocentricMetre, MaxGeocentricMetre,
                errors);
            double y = InputUtil.ParseNumber(txtConvertY.Text, "Y (m)", -MaxGeocentricMetre, MaxGeocentricMetre,
                errors);
            double z = InputUtil.ParseNumber(txtConvertZ.Text, "Z (m)", -MaxGeocentricMetre, MaxGeocentricMetre,
                errors);
            double addAltitude = ReadAddAltitude(errors);
            CompleteConversion("Geocentric", errors, () => {
                var coordinates = new Coordinates();
                coordinates.SetGeocentric(x, y, z);
                if (addAltitude != 0) {
                    coordinates.SetGeodetic(coordinates.Latitude, coordinates.Longitude,
                        coordinates.Altitude + addAltitude);
                }
                return coordinates;
            });
        }

        private void btnConvertLatLng_Click(object sender, EventArgs e) {
            var errors = new List<string>();
            double lat = InputUtil.ParseNumber(txtConvertLat.Text, "Lat", MinLatitudeDegree, MaxLatitudeDegree, errors);
            double lng = InputUtil.ParseNumber(txtConvertLng.Text, "Lng", MinLongitudeDegree, MaxLongitudeDegree,
                errors);
            double altitude = InputUtil.ParseNumber(txtConvertAlt.Text, "Altitude (m)", MinAltitudeMetre,
                MaxAltitudeMetre, errors);
            double addAltitude = ReadAddAltitude(errors);
            CompleteConversion("Lat/Lng", errors, () => {
                var coordinates = new Coordinates();
                coordinates.SetGeodetic(lat, lng, altitude + addAltitude);
                return coordinates;
            });
        }

        private void btnConvertDMS_Click(object sender, EventArgs e) {
            var errors = new List<string>();
            double lat = ReadDmsAngle("DMS Lat", txtConvertDMSLatDeg, txtConvertDMSLatMin, txtConvertDMSLatSec,
                MaxLatitudeDegree, cmbConvertDMSLatHemisphere.SelectedIndex == 0, errors);
            double lng = ReadDmsAngle("DMS Lng", txtConvertDMSLngDeg, txtConvertDMSLngMin, txtConvertDMSLngSec,
                MaxLongitudeDegree, cmbConvertDMSLngHemisphere.SelectedIndex == 0, errors);
            double altitude = InputUtil.ParseNumber(txtConvertDMSAlt.Text, "DMS Altitude (m)", MinAltitudeMetre,
                MaxAltitudeMetre, errors);
            double addAltitude = ReadAddAltitude(errors);
            CompleteConversion("DMS", errors, () => {
                var coordinates = new Coordinates();
                coordinates.SetGeodetic(lat, lng, altitude + addAltitude);
                return coordinates;
            });
        }

        /// <summary>
        /// Reads one DMS angle: whole degrees, then minutes and seconds from 0 to below 60 (a blank minute or second
        /// counts as 0). The sign comes from the hemisphere drop-down, never from the numbers.
        /// </summary>
        private static double ReadDmsAngle(string fieldName, TextBox degreeBox, TextBox minuteBox, TextBox secondBox,
            double maxDegree, bool isPositiveHemisphere, List<string> errors) {
            int problemsBefore = errors.Count;
            double degree = InputUtil.ParseNumber(degreeBox.Text, $"{fieldName} degrees", 0, maxDegree, errors);
            if (errors.Count == problemsBefore && degree != Math.Floor(degree)) {
                errors.Add($"{fieldName} degrees: {degreeBox.Text.Trim()} must be a whole number. "
                    + "Key the fraction into the minutes and seconds.");
            }
            double minute = ReadDmsPart(minuteBox.Text, $"{fieldName} minutes", errors);
            double second = ReadDmsPart(secondBox.Text, $"{fieldName} seconds", errors);

            double angle = 0;
            if (errors.Count == problemsBefore) {
                angle = ConverterUtil.DmsToDegrees(degree, minute, second, isPositiveHemisphere);
                if (Math.Abs(angle) > maxDegree) {
                    errors.Add($"{fieldName}: {InputUtil.FormatInput(degree)}° {InputUtil.FormatInput(minute)}' "
                        + $"{InputUtil.FormatInput(second)}\" is beyond {InputUtil.FormatInput(maxDegree)}°.");
                }
            }
            return angle;
        }

        /// <summary>
        /// Reads DMS minutes or seconds: blank means 0, otherwise a number from 0 to below 60.
        /// </summary>
        private static double ReadDmsPart(string text, string fieldName, List<string> errors) {
            double part = 0;
            if (text.Trim() != "") {
                var check = InputUtil.CheckNumber(text, fieldName, 0, DmsPartLimit, out part, out string message);
                if (check == InputUtil.NumberCheck.NotANumber) {
                    errors.Add(message);
                } else if (check == InputUtil.NumberCheck.OutOfRange || part >= DmsPartLimit) {
                    // One wording for every out-of-range minute or second, including exactly 60
                    errors.Add($"{fieldName}: {text.Trim()} must be from 0 to less than "
                        + $"{InputUtil.FormatInput(DmsPartLimit)}.");
                }
            }
            return part;
        }

        /// <summary>
        /// Reads the optional Add Altitude, which is added to every conversion; blank means 0.
        /// </summary>
        private double ReadAddAltitude(List<string> errors) {
            double addAltitude = 0;
            if (txtAddAltitude.Text.Trim() != "") {
                addAltitude = InputUtil.ParseNumber(txtAddAltitude.Text, "Add Altitude (m)", -MaxAltitudeMetre,
                    MaxAltitudeMetre, errors);
            }
            return addAltitude;
        }

        /// <summary>
        /// Converts when every input is valid and shows the result. Otherwise, or when the result is outside the
        /// supported range, it clears the outputs and shows one Invalid Input message listing every problem.
        /// </summary>
        private void CompleteConversion(string sourceName, List<string> errors, Func<Coordinates> convert) {
            Coordinates? coordinates = null;
            if (errors.Count == 0) {
                coordinates = convert();
                // Checked at the precision shown, so a point exactly at a limit converts the same from every input
                double shownAltitude = Math.Round(coordinates.Altitude, MetreDecimals);
                if (!IsValidCoordinates(coordinates)) {
                    errors.Add("The converted point is not valid: latitude must be within "
                        + $"{InputUtil.FormatInput(MinLatitudeDegree)} to {InputUtil.FormatInput(MaxLatitudeDegree)} "
                        + $"and longitude within {InputUtil.FormatInput(MinLongitudeDegree)} to "
                        + $"{InputUtil.FormatInput(MaxLongitudeDegree)}.");
                } else if (shownAltitude < MinAltitudeMetre || shownAltitude > MaxAltitudeMetre) {
                    errors.Add($"The converted altitude {FormatMetre(coordinates.Altitude)} m is outside the supported "
                        + $"range {InputUtil.FormatInput(MinAltitudeMetre)} to "
                        + $"{InputUtil.FormatInput(MaxAltitudeMetre)} "
                        + "m above the WGS84 ellipsoid.");
                }
            }
            if (errors.Count > 0) {
                ClearConversionOutput();
                ShowInputError(string.Join(Environment.NewLine, errors.Distinct()));
            } else {
                UpdateConversionOutput(coordinates!, sourceName);
            }
        }

        private void UpdateConversionOutput(Coordinates coordinates, string sourceName) {
            // Keep the full-precision result for the clipboard; the text boxes below only show a rounded copy
            lastConvertedCoordinates = coordinates;

            txtConvertXOutput.Text = FormatMetre(coordinates.X);
            txtConvertYOutput.Text = FormatMetre(coordinates.Y);
            txtConvertZOutput.Text = FormatMetre(coordinates.Z);

            txtConvertLatOutput.Text = FormatDegree(coordinates.Latitude);
            txtConvertLngOutput.Text = FormatDegree(coordinates.Longitude);
            txtConvertAltOutput.Text = FormatMetre(coordinates.Altitude);

            txtConvertDMSLatOutput.Text = FormatDMS(coordinates.Latitude, "N", "S");
            txtConvertDMSLngOutput.Text = FormatDMS(coordinates.Longitude, "E", "W");

            lblConvertStatus.Text = $"Converted from {sourceName} · "
                + DateTime.Now.ToString(TimeFormat, CultureInfo.InvariantCulture);
        }

        private void ClearConversionOutput() {
            lastConvertedCoordinates = null;
            foreach (var outputBox in new[] { txtConvertXOutput, txtConvertYOutput, txtConvertZOutput,
                txtConvertLatOutput, txtConvertLngOutput, txtConvertAltOutput, txtConvertDMSLatOutput,
                txtConvertDMSLngOutput }) {
                outputBox.Text = "";
            }
            lblConvertStatus.Text = "";
        }

        private void btnDFCalculate_Click(object sender, EventArgs e) {
            txtDFRichResult.Text = "";
            calculatedSnapshot = null;
            resultBlockStarts.Clear();
            // Build the plan from what is in the boxes now, even if the preview is still waiting for a pause in typing
            tmrDFAzimuthPreview.Stop();
            RefreshAzimuthPreview();
            var plan = currentPlan;

            // Validate every input first, so that a typo is reported instead of being silently treated as 0
            // Problems are listed in screen order: target, distance and elevation, azimuth, then the output options
            var errors = new List<string>();
            var baseCoordinates = ReadDistanceFromInputs(errors, out double distance, out double? elevation);
            errors.AddRange(plan.Problems);

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
                } else if (!RunningNoFits(runningNo, plan.Items.Count)) {
                    errors.Add($"Running No: \"{runningNoText}\" is too large for {plan.Items.Count} results.");
                }
            }

            var outputParts = GetSelectedOutputParts();
            if (!replaceText && outputParts == OutputParts.None) {
                errors.Add("Show: tick at least one coordinate type (X, Y, Z / Lat, Lng, Altitude / DMS).");
            }

            if (errors.Count > 0) {
                ShowInputError(string.Join(Environment.NewLine, errors.Distinct()));
                return;
            }

            // Calculate every point first, so that an invalid result is reported before anything is shown
            var results = new List<(PlannedAzimuth Azimuth, Coordinates Coordinates)>();
            foreach (var azimuth in plan.Items) {
                results.Add((azimuth, CalculatePoint(baseCoordinates, azimuth, elevation, distance)));
            }

            if (results.Any(result => !IsValidCoordinates(result.Coordinates))) {
                ShowInputError("The calculation produced an invalid coordinate. "
                    + "Check the target coordinate, the distance and the angles.");
                return;
            }

            using var boldFont = new Font(txtDFRichResult.Font, FontStyle.Bold);
            if (!replaceText) {
                // One summary line records exactly which azimuths were calculated
                AppendBoldLine(plan.SummaryText, boldFont);
                txtDFRichResult.AppendText(ResultSeparator + "\r\n");
            }
            foreach (var (azimuth, newCoordinates) in results) {
                if (!replaceText) {
                    resultBlockStarts.Add(txtDFRichResult.TextLength);
                    AppendDetailedResult(BuildResultHeader(azimuth, elevation, distance), newCoordinates, outputParts,
                        boldFont);
                } else {
                    var content = replaceTextTemplate.Replace("${x}", FormatMetre(newCoordinates.X))
                        .Replace("${y}", FormatMetre(newCoordinates.Y))
                        .Replace("${z}", FormatMetre(newCoordinates.Z))
                        .Replace("${lat}", FormatDegree(newCoordinates.Latitude))
                        .Replace("${lon}", FormatDegree(newCoordinates.Longitude))
                        .Replace("${altitude}", FormatMetre(newCoordinates.Altitude))
                        .Replace(AzimuthPlaceholder, azimuth.PrintedText)
                        .Replace(RunningNoPlaceholder, runningNo.ToString(CultureInfo.InvariantCulture));
                    txtDFRichResult.AppendText(content + "\r\n");
                    runningNo++;
                }
            }
            // Show the output from the top
            txtDFRichResult.Select(0, 0);
            txtDFRichResult.ScrollToCaret();

            calculatedSnapshot = BuildInputSnapshot();
            var resultWord = results.Count == 1 ? "result" : "results";
            calculatedStatus = $"{results.Count} {resultWord} · calculated "
                + DateTime.Now.ToString(TimeFormat, CultureInfo.InvariantCulture);
            outputIsDetailed = !replaceText;
            UpdateOutputStatus();
        }

        /// <summary>
        /// The coordinate at one azimuth from the target: along the ellipsoid surface when there is no elevation
        /// angle, otherwise along the line of sight (slant range).
        /// </summary>
        private static Coordinates CalculatePoint(Coordinates baseCoordinates, PlannedAzimuth azimuth,
            double? elevation, double distance) {
            return elevation.HasValue
                ? DistanceUtil.CalculateSlantRangeFrom(baseCoordinates, azimuth.PrintedDegree, elevation.Value,
                    distance)
                : DistanceUtil.CalculateDistanceFrom(baseCoordinates, azimuth.PrintedDegree, distance);
        }

        /// <summary>
        /// True when every running number from the first one up to the last azimuth fits in a long.
        /// </summary>
        private static bool RunningNoFits(long firstRunningNo, int azimuthCount) {
            return firstRunningNo <= long.MaxValue - Math.Max(azimuthCount - 1, 0);
        }

        /// <summary>
        /// Builds the bold header line that describes how one result was calculated. It starts with "#n", the same
        /// number as the azimuth's badge in the compass and its row in the preview table.
        /// </summary>
        private static string BuildResultHeader(PlannedAzimuth azimuth, double? elevation, double distance) {
            var header = $"#{azimuth.Index + 1}  Azimuth: {azimuth.PrintedText}°  ";
            if (elevation.HasValue) {
                header += $"Elevation: {InputUtil.FormatInput(elevation.Value)}°  "
                    + $"Slant Range: {InputUtil.FormatInput(distance)} km";
            } else {
                header += $"Surface Distance: {InputUtil.FormatInput(distance)} km";
            }
            return header;
        }

        /// <summary>
        /// Appends one calculated coordinate to the result box: the header line in bold, then only the coordinate
        /// types ticked under Show.
        /// </summary>
        private void AppendDetailedResult(string header, Coordinates coordinates, OutputParts outputParts,
            Font boldFont) {
            AppendBoldLine(header, boldFont);
            if (outputParts.HasFlag(OutputParts.Geocentric)) {
                txtDFRichResult.AppendText($"X: {FormatMetre(coordinates.X)}\r\n");
                txtDFRichResult.AppendText($"Y: {FormatMetre(coordinates.Y)}\r\n");
                txtDFRichResult.AppendText($"Z: {FormatMetre(coordinates.Z)}\r\n");
            }
            if (outputParts.HasFlag(OutputParts.Geodetic)) {
                txtDFRichResult.AppendText($"Latitude:  {FormatDegree(coordinates.Latitude)}\r\n");
                txtDFRichResult.AppendText($"Longitude: {FormatDegree(coordinates.Longitude)}\r\n");
                txtDFRichResult.AppendText($"Altitude (m):  {FormatMetre(coordinates.Altitude)}\r\n");
            }
            if (outputParts.HasFlag(OutputParts.Dms)) {
                txtDFRichResult.AppendText($"DMS Latitude:    {FormatDMS(coordinates.Latitude, "N", "S")}\r\n");
                txtDFRichResult.AppendText($"DMS Longitude: {FormatDMS(coordinates.Longitude, "E", "W")}\r\n");
            }
            txtDFRichResult.AppendText(ResultSeparator + "\r\n");
        }

        /// <summary>
        /// The coordinate types ticked under Show in the Output group.
        /// </summary>
        private OutputParts GetSelectedOutputParts() {
            var outputParts = OutputParts.None;
            if (cbDFShowGeocentric.Checked) {
                outputParts |= OutputParts.Geocentric;
            }
            if (cbDFShowGeodetic.Checked) {
                outputParts |= OutputParts.Geodetic;
            }
            if (cbDFShowDMS.Checked) {
                outputParts |= OutputParts.Dms;
            }
            return outputParts;
        }

        /// <summary>
        /// The Show choices apply to the detailed output only, so they are greyed out while Replace Output is ticked.
        /// </summary>
        private void UpdateOutputShowEnabled() {
            bool isDetailed = !cbDFReplaceText.Checked;
            lblDFOutputShow.Enabled = isDetailed;
            cbDFShowGeocentric.Enabled = isDetailed;
            cbDFShowGeodetic.Enabled = isDetailed;
            cbDFShowDMS.Enabled = isDetailed;
        }

        /// <summary>
        /// Appends one line to the result box in bold.
        /// </summary>
        private void AppendBoldLine(string text, Font boldFont) {
            // RichTextBox stores each line break as a single '\n', so TextLength is the index where the line starts
            int lineStart = txtDFRichResult.TextLength;
            txtDFRichResult.AppendText(text + "\r\n");
            txtDFRichResult.Select(lineStart, text.Length);
            txtDFRichResult.SelectionFont = boldFont;
            txtDFRichResult.DeselectAll();
        }

        private void txtDFInput_TextChanged(object? sender, EventArgs e) {
            // Restart the timer, so that the preview is rebuilt once, when typing pauses
            tmrDFAzimuthPreview.Stop();
            tmrDFAzimuthPreview.Start();
        }

        private void tmrDFAzimuthPreview_Tick(object? sender, EventArgs e) {
            tmrDFAzimuthPreview.Stop();
            RefreshAzimuthPreview();
        }

        private void rbDFAzimuthMode_CheckedChanged(object? sender, EventArgs e) {
            // Both mode buttons raise this event; act once, for the one that became checked
            if (sender is RadioButton modeButton && modeButton.Checked) {
                UpdateAzimuthModeEnabled();
                var firstBox = rbDFAzimuthRange.Checked ? txtDFAzimuthStart : txtDFDeg;
                // Moving the focus is a convenience; nothing depends on whether it succeeds
                _ = firstBox.Focus();
                firstBox.SelectAll();
                RefreshAzimuthPreview();
            }
        }

        private void rbDFAzimuthDirection_CheckedChanged(object? sender, EventArgs e) {
            if (sender is RadioButton directionButton && directionButton.Checked) {
                RefreshAzimuthPreview();
            }
        }

        private void cbDFReplaceText_CheckedChanged(object? sender, EventArgs e) {
            UpdateOutputShowEnabled();
            RefreshAzimuthPreview();
        }

        private void cbDFShowOutput_CheckedChanged(object? sender, EventArgs e) {
            // A different choice changes the output, so the current output is now out of date
            UpdateOutputStatus();
        }

        private void btnDFAzimuthCopyToList_Click(object? sender, EventArgs e) {
            var plan = AzimuthPlanBuilder.BuildRange(txtDFAzimuthStart.Text, txtDFAzimuthEnd.Text,
                txtDFAzimuthStep.Text, rbDFAzimuthClockwise.Checked);
            if (plan.IsValid) {
                var rangeText = string.Join(",", plan.Items.Select(item => item.PrintedText));
                var listText = txtDFDeg.Text.Trim();
                // Ask before replacing a different list the operator already typed (the default answer is No)
                var replace = listText == "" || listText == rangeText
                    || MessageBox.Show($"Replace the current list ({listText}) with the {plan.Items.Count} azimuths "
                        + "from the range?", CopyRangeCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes;
                if (replace) {
                    txtDFDeg.Text = rangeText;
                    rbDFAzimuthList.Checked = true;
                }
            }
        }

        private void lvDFAzimuths_SelectedIndexChanged(object? sender, EventArgs e) {
            var index = lvDFAzimuths.SelectedIndices.Count > 0 ? lvDFAzimuths.SelectedIndices[0] : -1;
            pnlDFAzimuthCompass.SelectedIndex = index;
            bool canScroll = !isRefreshingAzimuthTable && outputIsDetailed && IsOutputCurrent();
            if (canScroll && index >= 0 && index < resultBlockStarts.Count) {
                // Bring the matching output block into view
                txtDFRichResult.Select(resultBlockStarts[index], 0);
                txtDFRichResult.ScrollToCaret();
            }
        }

        private void pnlDFAzimuthCompass_RayClicked(object? sender, int index) {
            if (index >= 0 && index < lvDFAzimuths.Items.Count) {
                lvDFAzimuths.Items[index].Selected = true;
                lvDFAzimuths.Items[index].Focused = true;
                lvDFAzimuths.EnsureVisible(index);
            }
        }

        /// <summary>
        /// Rebuilds the azimuth plan and updates everything that shows it: the compass, count and sweep labels, the
        /// table, the status line, the box colours, the Calculate button and the out-of-date check of the output.
        /// </summary>
        private void RefreshAzimuthPreview() {
            currentPlan = BuildActiveAzimuthPlan();
            var firstRunningNo = GetPreviewRunningNo();
            pnlDFAzimuthCompass.SetPlan(currentPlan, BuildDistanceCaption(), firstRunningNo);
            UpdateAzimuthSummary();
            UpdateAzimuthTable();
            UpdateAzimuthStatus();
            UpdateAzimuthBoxColours();
            btnDFAzimuthCopyToList.Enabled = rbDFAzimuthRange.Checked && currentPlan.IsValid;
            btnDFCalculate.Text = BuildCalculateText(currentPlan);
            UpdateOutputStatus();
        }

        private AzimuthPlan BuildActiveAzimuthPlan() {
            return rbDFAzimuthRange.Checked
                ? AzimuthPlanBuilder.BuildRange(txtDFAzimuthStart.Text, txtDFAzimuthEnd.Text, txtDFAzimuthStep.Text,
                    rbDFAzimuthClockwise.Checked)
                : AzimuthPlanBuilder.BuildList(txtDFDeg.Text);
        }

        /// <summary>
        /// Greys out the inactive mode (its text is kept) and shows the checked mode in bold.
        /// </summary>
        private void UpdateAzimuthModeEnabled() {
            bool isRange = rbDFAzimuthRange.Checked;
            txtDFDeg.Enabled = !isRange;
            lblDFAzimuthListHint.Enabled = !isRange;
            txtDFAzimuthStart.Enabled = isRange;
            lblDFAzimuthTo.Enabled = isRange;
            txtDFAzimuthEnd.Enabled = isRange;
            lblDFAzimuthEvery.Enabled = isRange;
            txtDFAzimuthStep.Enabled = isRange;
            lblDFAzimuthDirection.Enabled = isRange;
            pnlDFAzimuthDirection.Enabled = isRange;
            lblDFAzimuthRangeHint.Enabled = isRange;
            rbDFAzimuthList.Font = isRange ? grpDFAzimuth.Font : boldUiFont;
            rbDFAzimuthRange.Font = isRange ? boldUiFont : grpDFAzimuth.Font;
        }

        /// <summary>
        /// The first running number when the compass badges should show running numbers, otherwise null.
        /// </summary>
        private long? GetPreviewRunningNo() {
            long? firstRunningNo = null;
            if (cbDFReplaceText.Checked && txtDFReplaceText.Text.Contains(RunningNoPlaceholder)
                && long.TryParse(txtDFReplaceRunningNo.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out long runningNo)
                && RunningNoFits(runningNo, currentPlan.Items.Count)) {
                firstRunningNo = runningNo;
            }
            return firstRunningNo;
        }

        /// <summary>
        /// Reads the target, distance and elevation for the preview table without reporting problems; false while
        /// any of them is missing or invalid (Calculate reports those).
        /// </summary>
        private bool TryReadPreviewInputs(out Coordinates baseCoordinates, out double distance, out double? elevation) {
            var silentErrors = new List<string>();
            baseCoordinates = ReadDistanceFromInputs(silentErrors, out distance, out elevation);
            return silentErrors.Count == 0;
        }

        /// <summary>
        /// Reads the target coordinate, the distance and the optional elevation angle of the Distance From tab, used
        /// by both Calculate and the preview table. Problems are added to errors.
        /// </summary>
        private Coordinates ReadDistanceFromInputs(List<string> errors, out double distance, out double? elevation) {
            double lat = InputUtil.ParseNumber(txtDFLat.Text, "Lat", MinLatitudeDegree, MaxLatitudeDegree, errors);
            double lng = InputUtil.ParseNumber(txtDFLng.Text, "Lng", MinLongitudeDegree, MaxLongitudeDegree, errors);
            double altitude = InputUtil.ParseNumber(txtDFAltitude.Text, "Altitude (m)", MinAltitudeMetre,
                MaxAltitudeMetre, errors);
            distance = InputUtil.ParseNumber(txtDFDistance.Text, "Distance Apart (km)", 0, MaxDistanceKm, errors);
            // A blank elevation angle means "move along the ellipsoid surface"
            elevation = null;
            if (txtDFElevation.Text.Trim() != "") {
                elevation = InputUtil.ParseNumber(txtDFElevation.Text, "Elevation Angle (Degree)",
                    MinElevationDegree, MaxElevationDegree, errors);
            }
            var baseCoordinates = new Coordinates();
            baseCoordinates.SetGeodetic(lat, lng, altitude);
            return baseCoordinates;
        }

        /// <summary>
        /// The compass corner caption, e.g. "10 km surface" or "10 km slant @ 30°"; empty while the inputs are invalid.
        /// </summary>
        private string BuildDistanceCaption() {
            var caption = "";
            var distanceCheck = InputUtil.CheckNumber(txtDFDistance.Text, "", 0, MaxDistanceKm, out double distance,
                out _);
            if (distanceCheck == InputUtil.NumberCheck.Valid) {
                var distanceText = InputUtil.FormatInput(distance);
                if (txtDFElevation.Text.Trim() == "") {
                    caption = $"{distanceText} km surface";
                } else if (InputUtil.CheckNumber(txtDFElevation.Text, "", MinElevationDegree, MaxElevationDegree,
                    out double elevation, out _) == InputUtil.NumberCheck.Valid) {
                    caption = $"{distanceText} km slant @ {InputUtil.FormatInput(elevation)}°";
                }
            }
            return caption;
        }

        private void UpdateAzimuthSummary() {
            var count = currentPlan.Items.Count;
            if (currentPlan.Errors.Count > 0) {
                lblDFAzimuthCount.Text = "No valid azimuths";
                lblDFAzimuthCount.ForeColor = PreviewStyle.ErrorText;
            } else if (!currentPlan.IsValid) {
                lblDFAzimuthCount.Text = "No azimuths yet";
                lblDFAzimuthCount.ForeColor = SystemColors.GrayText;
            } else {
                var noun = count == 1 ? "1 azimuth" : $"{count} azimuths";
                var mode = currentPlan.Mode == AzimuthMode.Range ? "Range" : "List";
                lblDFAzimuthCount.Text = $"{noun} · {mode}";
                lblDFAzimuthCount.ForeColor = currentPlan.Warnings.Count > 0
                    ? PreviewStyle.WarningText
                    : SystemColors.ControlText;
            }
            lblDFAzimuthSpan.Text = currentPlan.SpanText;
        }

        /// <summary>
        /// Lists every azimuth with the latitude and longitude it will give, worked out the same way as Calculate.
        /// </summary>
        private void UpdateAzimuthTable() {
            var selectedIndex = pnlDFAzimuthCompass.SelectedIndex;
            bool canCalculate = TryReadPreviewInputs(out Coordinates baseCoordinates, out double distance,
                out double? elevation);
            isRefreshingAzimuthTable = true;
            lvDFAzimuths.BeginUpdate();
            lvDFAzimuths.Items.Clear();
            if (currentPlan.IsValid) {
                foreach (var azimuth in currentPlan.Items) {
                    var row = new ListViewItem((azimuth.Index + 1).ToString(CultureInfo.InvariantCulture));
                    row.SubItems.Add(azimuth.PrintedText + "°");
                    var latitudeText = NotAvailableText;
                    var longitudeText = NotAvailableText;
                    if (canCalculate) {
                        var point = CalculatePoint(baseCoordinates, azimuth, elevation, distance);
                        if (IsValidCoordinates(point)) {
                            latitudeText = FormatDegree(point.Latitude);
                            longitudeText = FormatDegree(point.Longitude);
                        }
                    }
                    row.SubItems.Add(latitudeText);
                    row.SubItems.Add(longitudeText);
                    if (azimuth.IsDuplicate) {
                        row.BackColor = PreviewStyle.DuplicateRow;
                    }
                    lvDFAzimuths.Items.Add(row);
                }
            }
            lvDFAzimuths.EndUpdate();
            // Keep the highlighted azimuth when it still exists
            if (selectedIndex >= 0 && selectedIndex < lvDFAzimuths.Items.Count) {
                lvDFAzimuths.Items[selectedIndex].Selected = true;
            }
            pnlDFAzimuthCompass.SelectedIndex = selectedIndex < lvDFAzimuths.Items.Count ? selectedIndex : -1;
            isRefreshingAzimuthTable = false;
        }

        /// <summary>
        /// Shows the first error; otherwise every warning and note. A blank box shows a grey hint, not an error.
        /// </summary>
        private void UpdateAzimuthStatus() {
            string text;
            Color color;
            Bitmap icon;
            if (currentPlan.Errors.Count > 0) {
                var otherErrors = currentPlan.Errors.Count - 1;
                text = currentPlan.Errors[0] + (otherErrors > 0 ? $" (+{otherErrors} more)" : "");
                color = PreviewStyle.ErrorText;
                icon = errorIcon;
            } else if (currentPlan.MissingFields.Count > 0) {
                text = rbDFAzimuthRange.Checked ? "Key in the range start, end and step to see the azimuths."
                    : "Key in one or more azimuths, e.g. 0,90.";
                color = PreviewStyle.InfoText;
                icon = infoIcon;
            } else if (currentPlan.Warnings.Count > 0) {
                text = string.Join(" ", currentPlan.Warnings.Concat(currentPlan.Infos));
                color = PreviewStyle.WarningText;
                icon = warningIcon;
            } else {
                text = string.Join(" ", currentPlan.Infos);
                color = PreviewStyle.InfoText;
                icon = infoIcon;
            }
            picDFAzimuthStatus.Image = icon;
            lblDFAzimuthStatus.Text = text;
            lblDFAzimuthStatus.ForeColor = color;
            // The label can cut a long text short, so the whole text is also its tooltip
            tipInputs.SetToolTip(lblDFAzimuthStatus, text);
        }

        private void UpdateAzimuthBoxColours() {
            var errorFields = currentPlan.ErrorFields;
            SetErrorBackColor(txtDFDeg, errorFields.HasFlag(AzimuthInputField.List));
            SetErrorBackColor(txtDFAzimuthStart, errorFields.HasFlag(AzimuthInputField.Start));
            SetErrorBackColor(txtDFAzimuthEnd, errorFields.HasFlag(AzimuthInputField.End));
            SetErrorBackColor(txtDFAzimuthStep, errorFields.HasFlag(AzimuthInputField.Step));
        }

        private static void SetErrorBackColor(TextBox box, bool hasError) {
            if (hasError) {
                box.BackColor = PreviewStyle.ErrorBack;
            } else {
                box.ResetBackColor();
            }
        }

        private static string BuildCalculateText(AzimuthPlan plan) {
            var text = "&Calculate";
            if (plan.IsValid) {
                text += plan.Items.Count == 1 ? " 1 Point" : $" {plan.Items.Count} Points";
            }
            return text;
        }

        private void UpdateOutputStatus() {
            if (calculatedSnapshot == null) {
                lblDFOutputStatus.Text = "";
            } else if (IsOutputCurrent()) {
                lblDFOutputStatus.Text = calculatedStatus;
                lblDFOutputStatus.ForeColor = PreviewStyle.InfoText;
                lblDFOutputStatus.Font = grpDFOutput.Font;
            } else {
                lblDFOutputStatus.Text = "Inputs changed. Press Calculate to refresh.";
                lblDFOutputStatus.ForeColor = PreviewStyle.ErrorText;
                lblDFOutputStatus.Font = boldUiFont;
            }
        }

        private bool IsOutputCurrent() {
            return calculatedSnapshot != null && calculatedSnapshot == BuildInputSnapshot();
        }

        /// <summary>
        /// Every input that changes the output, joined into one string.
        /// </summary>
        private string BuildInputSnapshot() {
            return string.Join(SnapshotSeparator, new[] {
                txtDFLat.Text, txtDFLng.Text, txtDFAltitude.Text, txtDFDistance.Text, txtDFElevation.Text,
                rbDFAzimuthRange.Checked ? "range" : "list", txtDFDeg.Text,
                txtDFAzimuthStart.Text, txtDFAzimuthEnd.Text, txtDFAzimuthStep.Text,
                rbDFAzimuthClockwise.Checked ? "clockwise" : "anticlockwise",
                cbDFReplaceText.Checked ? "replace" : "detailed", txtDFReplaceRunningNo.Text, txtDFReplaceText.Text,
                GetSelectedOutputParts().ToString()
            });
        }

        private static void ShowInputError(string message) {
            MessageBox.Show(message, InputErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    + $"must be within {InputUtil.FormatInput(MinLatitudeDegree)} to "
                    + $"{InputUtil.FormatInput(MaxLatitudeDegree)} and the longitude within "
                    + $"{InputUtil.FormatInput(MinLongitudeDegree)} to {InputUtil.FormatInput(MaxLongitudeDegree)}. "
                    + "Correct the input and convert again.");
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
                ShowInputError("No coordinates in clipboard. Convert a coordinate on the Converter tab and press "
                    + "Copy to Clipboard first.");
                return;
            }

            // The round-trip format writes every digit needed to read back exactly the same value
            txtDFLat.Text = clipboardCoordinates.Latitude.ToString(RoundTripFormat, CultureInfo.InvariantCulture);
            txtDFLng.Text = clipboardCoordinates.Longitude.ToString(RoundTripFormat, CultureInfo.InvariantCulture);
            txtDFAltitude.Text = clipboardCoordinates.Altitude.ToString(RoundTripFormat, CultureInfo.InvariantCulture);
        }
    }
}
