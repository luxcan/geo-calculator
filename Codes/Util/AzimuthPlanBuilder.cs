using GeoCalculator.Codes.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GeoCalculator.Codes.Util {
    /// <summary>
    /// Builds the ordered set of azimuths to calculate from the operator's List or Range input. The preview, the
    /// Calculate button and the output all use this one builder, so the screen never shows a different set from the
    /// one that is calculated.
    /// </summary>
    /// <remarks>
    /// A range runs from the start in the chosen direction. The start is always included; the end is included when it
    /// falls on a step. A full circle (e.g. 0 to 360) lists the start once. Range values are printed from 0 to
    /// below 360 degrees; list values are printed as typed.
    /// </remarks>
    public static class AzimuthPlanBuilder {
        public const int MaxAzimuthCount = 360;
        public const double MinAzimuthDegree = -360;
        public const double MaxAzimuthDegree = 360;
        public const double MaxStepDegree = 360;
        // Two azimuths closer than this (around the circle) point the same way
        public const double DuplicateToleranceDegree = 1e-9;

        private const double MinStepDegree = 0;
        private const decimal FullCircleDegree = 360m;
        private const double FullCircleDegreeAsDouble = 360;
        private const double LongWayThresholdDegree = 180;
        // Half of the last of the 12 decimals that FormatInput shows: a direction this close below 360 would print
        // as "360", so it is treated as north (0)
        private const double DisplayRoundingDegree = 5e-13;
        // Above this estimated count the exact count is not worked out, because the decimal division could overflow
        private const double CountEstimateLimit = 1e9;
        private const char ListDelimiter = ',';

        private const string ListFieldName = "Azimuth (Degree)";
        private const string StartFieldName = "Azimuth Range Start (Degree)";
        private const string EndFieldName = "Azimuth Range End (Degree)";
        private const string StepFieldName = "Azimuth Range Step (Degree)";

        // A failed list entry that looks like a range, e.g. "0-90", "0–90", "0:90", "0..90" or "0 to 90"
        private static readonly Regex RangeLikeText =
            new Regex(@"\d\s*(-|–|—|:|\.\.|to)\s*[-+]?\d", RegexOptions.IgnoreCase);

        /// <summary>
        /// Builds the plan from a comma-separated list. Values are used and printed as typed, in the order typed.
        /// </summary>
        public static AzimuthPlan BuildList(string text) {
            var plan = new AzimuthPlan(AzimuthMode.List);
            if (text.Trim() == "") {
                plan.AddMissing($"{ListFieldName} is required.");
            } else {
                var azimuths = ReadListValues(text, plan);
                if (plan.Problems.Count == 0 && azimuths.Count > MaxAzimuthCount) {
                    plan.AddError($"{ListFieldName}: the list has {azimuths.Count} values; the most allowed is "
                        + $"{MaxAzimuthCount}.", AzimuthInputField.List);
                }
                if (plan.Problems.Count == 0) {
                    AddListItems(plan, azimuths);
                }
            }
            return plan;
        }

        /// <summary>
        /// Builds the plan from a range keyed as "start to end every step" in the given direction.
        /// </summary>
        public static AzimuthPlan BuildRange(string startText, string endText, string stepText, bool clockwise) {
            var plan = new AzimuthPlan(AzimuthMode.Range) { Clockwise = clockwise };
            bool hasStart = TryReadRangeValue(startText, StartFieldName, AzimuthInputField.Start, plan,
                out decimal start);
            bool hasEnd = TryReadRangeValue(endText, EndFieldName, AzimuthInputField.End, plan, out decimal end);
            bool hasStep = TryReadStep(stepText, plan, out decimal step);
            if (hasStart && hasEnd && hasStep) {
                var typedRange = $"{startText.Trim()} to {endText.Trim()} every {stepText.Trim()}";
                GenerateRange(plan, start, end, step, typedRange);
            }
            return plan;
        }

        /// <summary>
        /// The same direction from 0 to below 360 degrees (360 becomes 0, -45 becomes 315).
        /// </summary>
        public static double Normalize(double degree) {
            double remainder = degree % FullCircleDegreeAsDouble;
            if (remainder < 0) {
                remainder += FullCircleDegreeAsDouble;
            }
            // A tiny negative remainder plus 360 can round up to exactly 360, which is north again
            if (remainder >= FullCircleDegreeAsDouble) {
                remainder = 0;
            }
            // Adding 0.0 turns -0 into 0
            return remainder + 0.0;
        }

        /// <summary>
        /// The angle between two directions, going the short way round (0 to 180 degrees).
        /// </summary>
        public static double AngleBetween(double firstDegree, double secondDegree) {
            double gap = Math.Abs(Normalize(firstDegree) - Normalize(secondDegree));
            return Math.Min(gap, FullCircleDegreeAsDouble - gap);
        }

        private static decimal Normalize(decimal degree) {
            decimal remainder = degree % FullCircleDegree;
            if (remainder < 0) {
                remainder += FullCircleDegree;
            }
            // A tiny negative remainder plus 360 can round up to exactly 360, which is north again
            if (remainder >= FullCircleDegree) {
                remainder = 0m;
            }
            return remainder;
        }

        /// <summary>
        /// Converts an exact decimal angle to the double used for display and for DistanceUtil.
        /// </summary>
        /// <remarks>
        /// Mixed mode (coding standard 2.1d): ranges are generated in decimal so that steps such as 0.1 add up
        /// exactly; every value leaves the builder through this one conversion.
        /// </remarks>
        private static double ToDegree(decimal degree) {
            return (double)degree;
        }

        /// <summary>
        /// Converts an exact direction (0 to below 360) to double, keeping it below 360 after rounding.
        /// </summary>
        private static double ToDirectionDegree(decimal direction) {
            double converted = ToDegree(Normalize(direction));
            return converted >= FullCircleDegreeAsDouble - DisplayRoundingDegree ? 0 : converted;
        }

        private static List<double> ReadListValues(string text, AzimuthPlan plan) {
            var azimuths = new List<double>();
            var entries = text.Split(ListDelimiter);
            foreach (var entry in entries) {
                if (entries.Length > 1 && entry.Trim() == "") {
                    AddUniqueError(plan, $"{ListFieldName}: the list contains an empty value. Remove the extra comma.",
                        AzimuthInputField.List);
                } else {
                    var check = InputUtil.CheckNumber(entry, ListFieldName, MinAzimuthDegree, MaxAzimuthDegree,
                        out double azimuth, out string message);
                    if (check == InputUtil.NumberCheck.Valid) {
                        azimuths.Add(azimuth);
                    } else {
                        if (check == InputUtil.NumberCheck.NotANumber && RangeLikeText.IsMatch(entry.Trim())) {
                            message += " For a range of azimuths, select Range and key in the start, end and step.";
                        }
                        AddUniqueError(plan, message, AzimuthInputField.List);
                    }
                }
            }
            return azimuths;
        }

        private static void AddListItems(AzimuthPlan plan, List<double> azimuths) {
            foreach (var azimuth in azimuths) {
                plan.AddItem(azimuth, Normalize(azimuth));
            }
            plan.Items[0].AddNote("first");

            // Two entries pointing the same way are both calculated, but the operator is told
            var hasDuplicate = false;
            for (int index = 1; index < plan.Items.Count; index++) {
                var earlier = FindSameDirection(plan, index);
                if (earlier != null) {
                    var item = plan.Items[index];
                    item.IsDuplicate = true;
                    item.AddNote($"same as #{earlier.Index + 1}");
                    if (!hasDuplicate) {
                        plan.AddWarning($"#{earlier.Index + 1} ({earlier.PrintedText}°) and #{index + 1} "
                            + $"({item.PrintedText}°) point the same way; both will be calculated.");
                        plan.SummaryNote = $"; #{earlier.Index + 1} and #{index + 1} point the same way";
                    }
                    hasDuplicate = true;
                }
            }

            PlannedAzimuth? firstOutside = null;
            foreach (var item in plan.Items) {
                if (item.PrintedDegree < 0 || item.PrintedDegree >= FullCircleDegreeAsDouble) {
                    item.AddNote($"points to {InputUtil.FormatInput(item.DirectionDegree)}°");
                    firstOutside ??= item;
                }
            }
            if (!hasDuplicate) {
                var count = plan.Items.Count;
                plan.AddInfo(count == 1 ? "1 direction. Used as typed."
                    : $"{count} directions, none repeated. Used in the order typed.");
            }
            if (firstOutside != null) {
                plan.AddInfo($"{firstOutside.PrintedText}° is printed as keyed; it points to "
                    + $"{InputUtil.FormatInput(firstOutside.DirectionDegree)}°.");
            }
        }

        private static PlannedAzimuth? FindSameDirection(AzimuthPlan plan, int index) {
            PlannedAzimuth? earlier = null;
            for (int other = 0; other < index && earlier == null; other++) {
                // Compared around the circle, so that 359.9999999999 and 0 count as the same direction
                var gap = AngleBetween(plan.Items[other].DirectionDegree, plan.Items[index].DirectionDegree);
                if (gap < DuplicateToleranceDegree) {
                    earlier = plan.Items[other];
                }
            }
            return earlier;
        }

        private static bool TryReadRangeValue(string text, string fieldName, AzimuthInputField field, AzimuthPlan plan,
            out decimal exactDegree) {
            exactDegree = 0m;
            var check = InputUtil.CheckNumber(text, fieldName, MinAzimuthDegree, MaxAzimuthDegree,
                out double approximateDegree, out string message);
            if (check == InputUtil.NumberCheck.Valid) {
                exactDegree = ToExactDecimal(text, approximateDegree);
                // The double check above can round e.g. 360.0000000000000001 down to 360; check the exact value too
                if (exactDegree < (decimal)MinAzimuthDegree || exactDegree > (decimal)MaxAzimuthDegree) {
                    check = InputUtil.NumberCheck.OutOfRange;
                    message = InputUtil.OutOfRangeMessage(fieldName, text.Trim(), MinAzimuthDegree, MaxAzimuthDegree);
                }
            }
            if (check == InputUtil.NumberCheck.Missing) {
                plan.AddMissing(message);
            } else if (check != InputUtil.NumberCheck.Valid) {
                plan.AddError(message, field);
            }
            return check == InputUtil.NumberCheck.Valid;
        }

        private static bool TryReadStep(string text, AzimuthPlan plan, out decimal exactStep) {
            exactStep = 0m;
            var check = InputUtil.CheckNumber(text, StepFieldName, MinStepDegree, MaxStepDegree,
                out double approximateStep, out string message);
            if (check == InputUtil.NumberCheck.Valid) {
                exactStep = ToExactDecimal(text, approximateStep);
                if (exactStep > (decimal)MaxStepDegree) {
                    check = InputUtil.NumberCheck.OutOfRange;
                    message = InputUtil.OutOfRangeMessage(StepFieldName, text.Trim(), MinStepDegree, MaxStepDegree);
                }
            }
            if (check == InputUtil.NumberCheck.Missing) {
                plan.AddMissing(message);
            } else if (check != InputUtil.NumberCheck.Valid) {
                if (check == InputUtil.NumberCheck.OutOfRange && InputUtil.TryParseInvariant(text, out double typedStep)
                    && typedStep < 0) {
                    var otherDirection = plan.Clockwise ? "Anticlockwise" : "Clockwise";
                    message += $" To go the other way round, choose {otherDirection}.";
                }
                plan.AddError(message, AzimuthInputField.Step);
            } else if (exactStep == 0m) {
                plan.AddError($"{StepFieldName} must be greater than 0.", AzimuthInputField.Step);
            }
            return check == InputUtil.NumberCheck.Valid && exactStep != 0m;
        }

        /// <summary>
        /// Reads the typed text as an exact decimal, so that steps such as 0.1 add up without rounding error.
        /// </summary>
        private static decimal ToExactDecimal(string text, double fallback) {
            if (!decimal.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal exact)) {
                exact = (decimal)fallback;
            }
            return exact;
        }

        private static void GenerateRange(AzimuthPlan plan, decimal start, decimal end, decimal step,
            string typedRange) {
            decimal startDirection = Normalize(start);
            decimal sweep = 0m;
            if (start != end) {
                decimal difference = plan.Clockwise ? Normalize(end - start) : Normalize(start - end);
                // Two different numbers that point the same way (0 and 360) mean a full circle
                sweep = difference == 0m ? FullCircleDegree : difference;
            }

            // Mixed mode: the estimate uses double only to reject huge counts before the exact decimal division
            double estimatedCount = ToDegree(sweep) / ToDegree(step) + 1;
            if (estimatedCount > CountEstimateLimit) {
                AddTooManyError(plan, typedRange, $"more than {MaxAzimuthCount}");
                return;
            }

            decimal stepCount = decimal.Floor(sweep / step);
            // The decimal quotient is rounded to 28 digits, so it can come out one step too far; never pass the end
            while (stepCount > 0 && stepCount * step > sweep) {
                stepCount--;
            }
            bool isFullCircle = sweep == FullCircleDegree;
            bool repeatsStart = isFullCircle && stepCount * step == FullCircleDegree;
            // Mixed mode: the step count is a whole decimal of at most 1e9 (checked above), so it fits in an int
            int count = (int)stepCount + 1 - (repeatsStart ? 1 : 0);
            if (count > MaxAzimuthCount) {
                AddTooManyError(plan, typedRange, count.ToString(CultureInfo.InvariantCulture));
                return;
            }

            for (int index = 0; index < count; index++) {
                // Each value is worked out by multiplication, never by adding the step repeatedly
                decimal raw = plan.Clockwise ? start + index * step : start - index * step;
                double direction = ToDirectionDegree(raw);
                plan.AddItem(direction, direction);
            }

            plan.StartDegree = ToDegree(start);
            plan.EndDegree = ToDegree(end);
            plan.StepDegree = ToDegree(step);
            plan.SweepDegree = ToDegree(sweep);
            plan.StartDirectionDegree = ToDirectionDegree(startDirection);
            plan.IsFullCircle = isFullCircle;
            plan.EndIncluded = !isFullCircle && stepCount * step == sweep;

            AddRangeNotes(plan, stepCount, end);
            AddRangeMessages(plan, start, end, step, stepCount);
        }

        private static void AddTooManyError(AzimuthPlan plan, string typedRange, string countText) {
            plan.AddError($"Azimuth Range: {typedRange} gives {countText} azimuths; the most allowed is "
                + $"{MaxAzimuthCount}. Use a larger step or a smaller range.", AzimuthInputField.Step);
        }

        private static void AddRangeNotes(AzimuthPlan plan, decimal stepCount, decimal end) {
            var items = plan.Items;
            var last = items[items.Count - 1];
            items[0].AddNote("start");
            if (items.Count > 1) {
                last.AddNote(plan.EndIncluded ? "end" : "last");
            }
            // A full circle always covers north; the "full circle" note says so instead
            var crossingText = plan.IsFullCircle ? "" : FindNorthCrossing(plan);
            plan.CrossesNorth = crossingText != "";
            if (plan.CrossesNorth) {
                plan.AddInfo($"Passes through north{crossingText}.");
            }
            if (plan.SweepDegree > 0 && !plan.IsFullCircle && stepCount > 0 && plan.EndIncluded) {
                plan.AddInfo($"End {FormatDecimal(end)}° falls on a step, so it is included.");
            }
        }

        /// <summary>
        /// Finds where the azimuths pass north (" at #3 (0°)" or " between #3 (350°) and #4 (15°)") and notes that
        /// row. Empty when no azimuth after the start reaches or passes north.
        /// </summary>
        private static string FindNorthCrossing(AzimuthPlan plan) {
            var crossingText = "";
            var items = plan.Items;
            for (int index = 1; index < items.Count && crossingText == ""; index++) {
                var previous = items[index - 1];
                var current = items[index];
                // Clockwise the direction drops when it passes north; anticlockwise it jumps up
                bool passesNorth = plan.Clockwise
                    ? current.DirectionDegree < previous.DirectionDegree
                    : current.DirectionDegree > previous.DirectionDegree && previous.DirectionDegree != 0;
                if (current.DirectionDegree == 0) {
                    crossingText = $" at #{index + 1} (0°)";
                    current.AddNote("crosses north");
                } else if (passesNorth) {
                    crossingText = $" between #{index} ({previous.PrintedText}°) and #{index + 1} "
                        + $"({current.PrintedText}°)";
                    current.AddNote("crosses north");
                }
            }
            return crossingText;
        }

        private static void AddRangeMessages(AzimuthPlan plan, decimal start, decimal end, decimal step,
            decimal stepCount) {
            var last = plan.Items[plan.Items.Count - 1];
            var stepText = FormatDecimal(step);
            if (plan.SweepDegree == 0) {
                plan.AddWarning("Start and end are the same: only 1 azimuth.");
            } else if (plan.IsFullCircle) {
                if (stepCount * step == FullCircleDegree) {
                    plan.AddInfo("Full circle: 360° is the same direction as 0°, so it is listed once.");
                } else {
                    var gap = FullCircleDegree - stepCount * step;
                    plan.AddWarning($"Step {stepText}° does not divide 360°: the gap from the last azimuth "
                        + $"({last.PrintedText}°) back to the start ({FormatDecimal(Normalize(start))}°) is "
                        + $"{FormatDecimal(gap)}°.");
                }
            } else if (stepCount == 0) {
                plan.AddWarning($"Step {stepText}° is larger than the range "
                    + $"({InputUtil.FormatInput(plan.SweepDegree)}°): only 1 azimuth.");
            } else if (!plan.EndIncluded) {
                plan.AddWarning($"Step {stepText}° does not land on the end {FormatDecimal(end)}°: the last azimuth "
                    + $"is {last.PrintedText}°.");
                plan.SummaryNote = $"; end not on a step, last {last.PrintedText}°";
            }

            AddLongWayWarning(plan, start, end);

            if (start < 0 || start >= FullCircleDegree) {
                plan.AddInfo($"Printed from 0° to below 360°: {FormatDecimal(start)}° is printed as "
                    + $"{FormatDecimal(Normalize(start))}°.");
            } else if (end < 0 || end >= FullCircleDegree) {
                plan.AddInfo($"Printed from 0° to below 360°: {FormatDecimal(end)}° is printed as "
                    + $"{FormatDecimal(Normalize(end))}°.");
            }
        }

        /// <summary>
        /// Warns when the range goes more than half way round in the chosen direction although the other direction
        /// is shorter, e.g. 90 to 0 clockwise.
        /// </summary>
        private static void AddLongWayWarning(AzimuthPlan plan, decimal start, decimal end) {
            if (!plan.IsFullCircle && plan.SweepDegree > LongWayThresholdDegree) {
                var startText = FormatDecimal(Normalize(start));
                var endText = FormatDecimal(Normalize(end));
                var sweepText = InputUtil.FormatInput(plan.SweepDegree);
                if (plan.Clockwise && Normalize(start) > Normalize(end)) {
                    plan.AddWarning($"This goes {sweepText}° clockwise (the long way round). For {startText}° down to "
                        + $"{endText}°, choose Anticlockwise.");
                } else if (!plan.Clockwise && Normalize(start) < Normalize(end)) {
                    plan.AddWarning($"This goes {sweepText}° anticlockwise (the long way round). "
                        + $"For {startText}° up to {endText}°, choose Clockwise.");
                }
            }
        }

        private static string FormatDecimal(decimal degree) {
            return InputUtil.FormatInput(ToDegree(degree));
        }

        private static void AddUniqueError(AzimuthPlan plan, string message, AzimuthInputField field) {
            if (!plan.Problems.Contains(message)) {
                plan.AddError(message, field);
            }
        }
    }
}
