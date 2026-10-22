using System.Globalization;

namespace GeoCalculator.Codes.Util {
    /// <summary>
    /// Parses and echoes numbers typed by the operator. The invariant culture is used ('.' is the decimal point and
    /// thousands separators are rejected), so a mistyped comma is reported instead of being silently misread.
    /// </summary>
    public static class InputUtil {
        private const int InputDecimals = 12;
        private const string InputNumberFormat = "0.############";

        /// <summary>
        /// Outcome of checking one typed number.
        /// </summary>
        public enum NumberCheck {
            Valid,
            Missing,
            NotANumber,
            OutOfRange
        }

        /// <summary>
        /// Checks a typed number against [minValue, maxValue] and builds the operator message for any problem.
        /// The number is only set when the result is <see cref="NumberCheck.Valid"/>.
        /// </summary>
        public static NumberCheck CheckNumber(string text, string fieldName, double minValue, double maxValue,
            out double number, out string message) {
            var check = NumberCheck.Valid;
            number = 0;
            message = "";
            var trimmedText = text.Trim();
            if (trimmedText == "") {
                check = NumberCheck.Missing;
                message = $"{fieldName} is required.";
            } else if (!TryParseInvariant(trimmedText, out double parsedNumber)) {
                check = NumberCheck.NotANumber;
                message = $"{fieldName}: \"{trimmedText}\" is not a valid number.";
            } else if (parsedNumber < minValue || parsedNumber > maxValue) {
                check = NumberCheck.OutOfRange;
                message = OutOfRangeMessage(fieldName, trimmedText, minValue, maxValue);
            } else {
                number = parsedNumber;
            }
            return check;
        }

        /// <summary>
        /// The message for a typed number outside [minValue, maxValue]; the number is echoed as typed.
        /// </summary>
        public static string OutOfRangeMessage(string fieldName, string trimmedText, double minValue, double maxValue) {
            return $"{fieldName}: {trimmedText} is outside the range {FormatInput(minValue)} to "
                + $"{FormatInput(maxValue)}.";
        }

        /// <summary>
        /// Parses a number typed by the operator and checks that it lies within [minValue, maxValue].
        /// Any problem is added to errors and 0 is returned.
        /// </summary>
        public static double ParseNumber(string text, string fieldName, double minValue, double maxValue,
            List<string> errors) {
            var check = CheckNumber(text, fieldName, minValue, maxValue, out double number, out string message);
            if (check != NumberCheck.Valid) {
                errors.Add(message);
            }
            return number;
        }

        /// <summary>
        /// True when the text is a finite number in the invariant culture.
        /// </summary>
        public static bool TryParseInvariant(string text, out double number) {
            return double.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out number)
                && double.IsFinite(number);
        }

        /// <summary>
        /// Echoes an operator input in plain decimal notation (no exponent, no "-0").
        /// </summary>
        public static string FormatInput(double number) {
            // Round to the decimals shown first, then add 0.0, so that a tiny negative value prints as 0 instead of -0
            double roundedNumber = Math.Round(number, InputDecimals) + 0.0;
            return roundedNumber.ToString(InputNumberFormat, CultureInfo.InvariantCulture);
        }
    }
}
