using GeoCalculator.Codes.Util;

namespace GeoCalculator.Codes.Model {
    /// <summary>
    /// How the operator keys the azimuths on the Distance From tab.
    /// </summary>
    public enum AzimuthMode {
        List,
        Range
    }

    /// <summary>
    /// Azimuth input boxes that hold an invalid (not blank) value.
    /// </summary>
    [Flags]
    public enum AzimuthInputField {
        None = 0,
        List = 1,
        Start = 2,
        End = 4,
        Step = 8
    }

    /// <summary>
    /// One azimuth to calculate, in output order.
    /// </summary>
    public sealed class PlannedAzimuth {
        private readonly List<string> notes = new List<string>();

        public PlannedAzimuth(int index, double printedDegree, double directionDegree) {
            Index = index;
            PrintedDegree = printedDegree;
            DirectionDegree = directionDegree;
        }

        /// <summary>
        /// Position in the output order, starting at 0 (#1 is index 0).
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Azimuth used for the calculation and printed in the output: as typed in List mode, from 0 to below 360
        /// degrees in Range mode.
        /// </summary>
        public double PrintedDegree { get; }

        /// <summary>
        /// The same direction from 0 to below 360 degrees, used for drawing and for the compass point.
        /// </summary>
        public double DirectionDegree { get; }

        /// <summary>
        /// True when an earlier azimuth in the list points the same way.
        /// </summary>
        public bool IsDuplicate { get; internal set; }

        /// <summary>
        /// Short notes shown in the compass tooltip, e.g. "start", "end", "crosses north".
        /// </summary>
        public string Note => string.Join("; ", notes);

        /// <summary>
        /// The printed azimuth in plain decimal notation, without the degree sign.
        /// </summary>
        public string PrintedText => InputUtil.FormatInput(PrintedDegree);

        internal void AddNote(string note) {
            notes.Add(note);
        }
    }

    /// <summary>
    /// The ordered set of azimuths produced from the operator's List or Range input, with every problem, warning and
    /// note found while building it. The same plan drives the preview, the Calculate button and the output.
    /// </summary>
    public sealed class AzimuthPlan {
        // Above this many values the summary line shows only the first three and the last
        private const int SummaryValueLimit = 24;
        private const int SummaryLeadingValues = 3;

        private readonly List<PlannedAzimuth> items = new List<PlannedAzimuth>();
        private readonly List<string> problems = new List<string>();
        private readonly List<string> errors = new List<string>();
        private readonly List<string> missingFields = new List<string>();
        private readonly List<string> warnings = new List<string>();
        private readonly List<string> infos = new List<string>();

        public AzimuthPlan(AzimuthMode mode) {
            Mode = mode;
        }

        public AzimuthMode Mode { get; }

        /// <summary>
        /// The azimuths to calculate, in output order. Empty when the input has a problem.
        /// </summary>
        public IReadOnlyList<PlannedAzimuth> Items => items;

        /// <summary>
        /// Every problem in field order, as shown in the Invalid Input dialog (missing and invalid values).
        /// </summary>
        public IReadOnlyList<string> Problems => problems;

        /// <summary>
        /// Invalid values only (not blank boxes).
        /// </summary>
        public IReadOnlyList<string> Errors => errors;

        /// <summary>
        /// "... is required." messages for blank boxes.
        /// </summary>
        public IReadOnlyList<string> MissingFields => missingFields;

        /// <summary>
        /// Problems that do not block Calculate, e.g. a step that does not land on the end.
        /// </summary>
        public IReadOnlyList<string> Warnings => warnings;

        /// <summary>
        /// Plain information for the operator, e.g. where a range passes through north.
        /// </summary>
        public IReadOnlyList<string> Infos => infos;

        /// <summary>
        /// The input boxes that hold an invalid value, for the red box tint.
        /// </summary>
        public AzimuthInputField ErrorFields { get; private set; }

        /// <summary>
        /// True when there is at least one azimuth and no problem, so Calculate can run.
        /// </summary>
        public bool IsValid => problems.Count == 0 && items.Count > 0;

        // The range as keyed (Range mode only; set once all three values are valid)

        /// <summary>
        /// The range start as typed, in degrees.
        /// </summary>
        public double StartDegree { get; internal set; }

        /// <summary>
        /// The range end as typed, in degrees.
        /// </summary>
        public double EndDegree { get; internal set; }

        /// <summary>
        /// Degrees between neighbouring azimuths.
        /// </summary>
        public double StepDegree { get; internal set; }

        /// <summary>
        /// Degrees covered from the start to the end in the chosen direction (360 for a full circle).
        /// </summary>
        public double SweepDegree { get; internal set; }

        /// <summary>
        /// The start as a direction from 0 to below 360 degrees.
        /// </summary>
        public double StartDirectionDegree { get; internal set; }

        /// <summary>
        /// True when the azimuths increase from the start (clockwise), false when they decrease.
        /// </summary>
        public bool Clockwise { get; internal set; } = true;

        /// <summary>
        /// True when the range covers the whole circle (e.g. 0 to 360).
        /// </summary>
        public bool IsFullCircle { get; internal set; }

        /// <summary>
        /// True when an azimuth after the start reaches or passes north (not set for a full circle).
        /// </summary>
        public bool CrossesNorth { get; internal set; }

        /// <summary>
        /// True when the end falls on a step and is therefore the last azimuth.
        /// </summary>
        public bool EndIncluded { get; internal set; }

        /// <summary>
        /// Text added inside the brackets of the summary line, e.g. "; end not on a step, last 75°".
        /// </summary>
        public string SummaryNote { get; internal set; } = "";

        /// <summary>
        /// One line describing the whole set, printed at the top of the detailed output.
        /// </summary>
        public string SummaryText {
            get {
                var values = items.Select(item => item.PrintedText).ToList();
                var shownValues = values.Count > SummaryValueLimit
                    ? string.Join(", ", values.Take(SummaryLeadingValues)) + " … " + values[values.Count - 1]
                    : string.Join(", ", values);
                var noun = values.Count == 1 ? "1 azimuth" : $"{values.Count} azimuths";
                string description;
                if (Mode == AzimuthMode.Range) {
                    description = $"Range {InputUtil.FormatInput(StartDegree)}° to {InputUtil.FormatInput(EndDegree)}° "
                        + $"{DirectionWord}, every {InputUtil.FormatInput(StepDegree)}°";
                } else {
                    description = "List, as typed";
                }
                return $"{noun} ({description}{SummaryNote}): {shownValues}";
            }
        }

        /// <summary>
        /// One line describing the sweep for the preview, e.g. "0° to 90° clockwise, every 15° · covers 90°".
        /// Empty when the plan is not valid.
        /// </summary>
        public string SpanText {
            get {
                var text = "";
                if (IsValid && Mode == AzimuthMode.Range) {
                    text = $"{InputUtil.FormatInput(StartDegree)}° to {InputUtil.FormatInput(EndDegree)}° "
                    + $"{DirectionWord}, "
                        + $"every {InputUtil.FormatInput(StepDegree)}° · covers {InputUtil.FormatInput(SweepDegree)}°";
                    if (IsFullCircle) {
                        text += " · full circle";
                    } else if (CrossesNorth) {
                        text += " · crosses north";
                    }
                } else if (IsValid) {
                    text = "Keyed list · used in the order typed";
                }
                return text;
            }
        }

        private string DirectionWord => Clockwise ? "clockwise" : "anticlockwise";

        internal PlannedAzimuth AddItem(double printedDegree, double directionDegree) {
            var item = new PlannedAzimuth(items.Count, printedDegree, directionDegree);
            items.Add(item);
            return item;
        }

        internal void AddMissing(string message) {
            missingFields.Add(message);
            problems.Add(message);
        }

        internal void AddError(string message, AzimuthInputField field) {
            errors.Add(message);
            problems.Add(message);
            ErrorFields |= field;
        }

        internal void AddWarning(string message) {
            warnings.Add(message);
        }

        internal void AddInfo(string message) {
            infos.Add(message);
        }
    }
}
