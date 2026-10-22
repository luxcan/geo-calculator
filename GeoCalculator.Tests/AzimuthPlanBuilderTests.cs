using GeoCalculator.Codes.Controls;
using GeoCalculator.Codes.Model;
using GeoCalculator.Codes.Util;
using Xunit;

namespace GeoCalculator.Tests {
    /// <summary>
    /// The range and list rules of the Distance From azimuth input. Each case is a worked example from the design.
    /// </summary>
    public class AzimuthPlanBuilderTests {
        private static double[] Printed(AzimuthPlan plan) {
            return plan.Items.Select(item => item.PrintedDegree).ToArray();
        }

        private static AzimuthPlan Range(string start, string end, string step, bool clockwise = true) {
            return AzimuthPlanBuilder.BuildRange(start, end, step, clockwise);
        }

        [Fact]
        public void RangeIncludesStartAndEndWhenEndFallsOnStep() {
            var plan = Range("0", "90", "15");

            Assert.True(plan.IsValid);
            Assert.Equal(new double[] { 0, 15, 30, 45, 60, 75, 90 }, Printed(plan));
            Assert.True(plan.EndIncluded);
            Assert.False(plan.CrossesNorth);
            Assert.Empty(plan.Warnings);
            Assert.Equal(new[] { "End 90° falls on a step, so it is included." }, plan.Infos);
            Assert.Equal("start", plan.Items[0].Note);
            Assert.Equal("end", plan.Items[6].Note);
            Assert.Equal("7 azimuths (Range 0° to 90° clockwise, every 15°): 0, 15, 30, 45, 60, 75, 90",
                plan.SummaryText);
            Assert.Equal("0° to 90° clockwise, every 15° · covers 90°", plan.SpanText);
        }

        [Fact]
        public void ClockwiseRangePassesThroughNorth() {
            var plan = Range("300", "60", "30");

            Assert.Equal(new double[] { 300, 330, 0, 30, 60 }, Printed(plan));
            Assert.True(plan.CrossesNorth);
            Assert.Equal(new[] {
                "Passes through north at #3 (0°).",
                "End 60° falls on a step, so it is included."
            }, plan.Infos);
            Assert.Equal("crosses north", plan.Items[2].Note);
            Assert.Equal("300° to 60° clockwise, every 30° · covers 120° · crosses north", plan.SpanText);
        }

        [Fact]
        public void AnticlockwiseRangeGoesTheOtherWayRound() {
            var plan = Range("300", "60", "30", clockwise: false);

            Assert.Equal(new double[] { 300, 270, 240, 210, 180, 150, 120, 90, 60 }, Printed(plan));
            Assert.False(plan.CrossesNorth);
            Assert.Equal(new double[] { 90, 60, 30, 0 }, Printed(Range("90", "0", "30", clockwise: false)));
        }

        [Fact]
        public void LongWayRoundIsWarned() {
            var plan = Range("90", "0", "30");

            Assert.Equal(new double[] { 90, 120, 150, 180, 210, 240, 270, 300, 330, 0 }, Printed(plan));
            Assert.Contains("This goes 270° clockwise (the long way round). For 90° down to 0°, choose Anticlockwise.",
                plan.Warnings);
        }

        [Fact]
        public void NegativeStartIsPrintedFromZeroToBelow360() {
            var plan = Range("-45", "45", "15");

            Assert.Equal(new double[] { 315, 330, 345, 0, 15, 30, 45 }, Printed(plan));
            Assert.Contains("Printed from 0° to below 360°: -45° is printed as 315°.", plan.Infos);
        }

        [Fact]
        public void FullCircleListsTheStartOnce() {
            var clockwise = Range("0", "360", "90");
            Assert.Equal(new double[] { 0, 90, 180, 270 }, Printed(clockwise));
            Assert.True(clockwise.IsFullCircle);
            Assert.Contains("Full circle: 360° is the same direction as 0°, so it is listed once.", clockwise.Infos);
            Assert.Equal("0° to 360° clockwise, every 90° · covers 360° · full circle", clockwise.SpanText);

            Assert.Equal(new double[] { 0, 270, 180, 90 }, Printed(Range("360", "0", "90", clockwise: false)));
        }

        [Fact]
        public void FullCircleWithStepThatDoesNotDivide360WarnsAboutTheGap() {
            var plan = Range("0", "360", "7");

            Assert.Equal(52, plan.Items.Count);
            Assert.Contains("Step 7° does not divide 360°: the gap from the last azimuth (357°) back to the start (0°) "
                + "is 3°.", plan.Warnings);
        }

        [Fact]
        public void EndNotOnStepStopsAtLastStepAndIsWarned() {
            var plan = Range("0", "100", "15");

            Assert.Equal(new double[] { 0, 15, 30, 45, 60, 75, 90 }, Printed(plan));
            Assert.False(plan.EndIncluded);
            Assert.Equal("last", plan.Items[6].Note);
            Assert.Contains("Step 15° does not land on the end 100°: the last azimuth is 90°.", plan.Warnings);
            Assert.Equal("7 azimuths (Range 0° to 100° clockwise, every 15°; end not on a step, last 90°): "
                + "0, 15, 30, 45, 60, 75, 90", plan.SummaryText);
        }

        [Fact]
        public void StepLargerThanRangeGivesOneAzimuth() {
            var plan = Range("0", "90", "120");

            Assert.Equal(new double[] { 0 }, Printed(plan));
            Assert.Contains("Step 120° is larger than the range (90°): only 1 azimuth.", plan.Warnings);
        }

        [Fact]
        public void SameStartAndEndGivesOneAzimuth() {
            var plan = Range("45", "45", "15");

            Assert.Equal(new double[] { 45 }, Printed(plan));
            Assert.Contains("Start and end are the same: only 1 azimuth.", plan.Warnings);
        }

        [Fact]
        public void DecimalStepsAddUpExactly() {
            var plan = Range("0", "1", "0.1");

            Assert.Equal(11, plan.Items.Count);
            Assert.Equal(1.0, plan.Items[10].PrintedDegree);
            Assert.Equal(0.3, plan.Items[3].PrintedDegree);
            Assert.True(plan.EndIncluded);
        }

        [Fact]
        public void TooManyAzimuthsIsAnError() {
            var plan = Range("0", "360", "0.5");

            Assert.False(plan.IsValid);
            Assert.Equal(new[] {
                "Azimuth Range: 0 to 360 every 0.5 gives 720 azimuths; the most allowed is 360. "
                + "Use a larger step or a smaller range."
            }, plan.Errors);
            Assert.Equal(AzimuthInputField.Step, plan.ErrorFields);
        }

        [Theory]
        [InlineData("0", "Azimuth Range Step (Degree) must be greater than 0.")]
        [InlineData("1e-30", "Azimuth Range Step (Degree) must be greater than 0.")]
        [InlineData("-15", "Azimuth Range Step (Degree): -15 is outside the range 0 to 360. "
            + "To go the other way round, choose Anticlockwise.")]
        [InlineData("400", "Azimuth Range Step (Degree): 400 is outside the range 0 to 360.")]
        [InlineData("x", "Azimuth Range Step (Degree): \"x\" is not a valid number.")]
        public void InvalidStepIsReported(string step, string message) {
            var plan = Range("0", "90", step);

            Assert.False(plan.IsValid);
            Assert.Equal(new[] { message }, plan.Errors);
            Assert.Empty(plan.Items);
        }

        [Fact]
        public void InvalidStartAndEndAreReportedInFieldOrder() {
            var plan = Range("abc", "400", "15");

            Assert.Equal(new[] {
                "Azimuth Range Start (Degree): \"abc\" is not a valid number.",
                "Azimuth Range End (Degree): 400 is outside the range -360 to 360."
            }, plan.Problems);
            Assert.Equal(AzimuthInputField.Start | AzimuthInputField.End, plan.ErrorFields);
        }

        [Fact]
        public void BlankRangeBoxesAreMissingNotErrors() {
            var plan = Range("", " ", "");

            Assert.False(plan.IsValid);
            Assert.Empty(plan.Errors);
            Assert.Equal(new[] {
                "Azimuth Range Start (Degree) is required.",
                "Azimuth Range End (Degree) is required.",
                "Azimuth Range Step (Degree) is required."
            }, plan.MissingFields);
            Assert.Equal(AzimuthInputField.None, plan.ErrorFields);
        }

        [Fact]
        public void CommaDecimalIsNotAccepted() {
            var plan = Range("0", "90", "1,5");

            Assert.Equal(new[] { "Azimuth Range Step (Degree): \"1,5\" is not a valid number." }, plan.Errors);
        }

        [Fact]
        public void LongSummaryShowsFirstThreeAndLast() {
            var plan = Range("0", "350", "10");

            Assert.Equal(36, plan.Items.Count);
            Assert.Equal("36 azimuths (Range 0° to 350° clockwise, every 10°): 0, 10, 20 … 350", plan.SummaryText);
        }

        [Fact]
        public void ListIsUsedAsTypedInOrder() {
            var plan = AzimuthPlanBuilder.BuildList("45,135,225,315");

            Assert.True(plan.IsValid);
            Assert.Equal(new double[] { 45, 135, 225, 315 }, Printed(plan));
            Assert.Equal(new[] { "4 directions, none repeated. Used in the order typed." }, plan.Infos);
            Assert.Equal("first", plan.Items[0].Note);
            Assert.Equal("4 azimuths (List, as typed): 45, 135, 225, 315", plan.SummaryText);
            Assert.Equal("Keyed list · used in the order typed", plan.SpanText);
        }

        [Fact]
        public void ListDuplicatesAreWarnedButKept() {
            var plan = AzimuthPlanBuilder.BuildList("0, 90, 360");

            Assert.True(plan.IsValid);
            Assert.Equal(new double[] { 0, 90, 360 }, Printed(plan));
            Assert.True(plan.Items[2].IsDuplicate);
            Assert.Equal("same as #1; points to 0°", plan.Items[2].Note);
            Assert.Contains("#1 (0°) and #3 (360°) point the same way; both will be calculated.", plan.Warnings);
            Assert.Equal("3 azimuths (List, as typed; #1 and #3 point the same way): 0, 90, 360", plan.SummaryText);
        }

        [Fact]
        public void ListNegativeValueIsPrintedAsTyped() {
            var plan = AzimuthPlanBuilder.BuildList("-45");

            Assert.Equal(-45, plan.Items[0].PrintedDegree);
            Assert.Equal(315, plan.Items[0].DirectionDegree);
            Assert.Contains("-45° is printed as keyed; it points to 315°.", plan.Infos);
        }

        [Theory]
        [InlineData("", "Azimuth (Degree) is required.")]
        [InlineData("0,90,", "Azimuth (Degree): the list contains an empty value. Remove the extra comma.")]
        [InlineData("0;90", "Azimuth (Degree): \"0;90\" is not a valid number.")]
        [InlineData("400", "Azimuth (Degree): 400 is outside the range -360 to 360.")]
        [InlineData("0-90", "Azimuth (Degree): \"0-90\" is not a valid number. For a range of azimuths, select Range "
            + "and key in the start, end and step.")]
        public void InvalidListIsReported(string text, string message) {
            var plan = AzimuthPlanBuilder.BuildList(text);

            Assert.False(plan.IsValid);
            Assert.Equal(new[] { message }, plan.Problems);
        }

        [Fact]
        public void ListLongerThanLimitIsAnError() {
            var text = string.Join(",", Enumerable.Range(0, 361));
            var plan = AzimuthPlanBuilder.BuildList(text);

            Assert.Equal(new[] { "Azimuth (Degree): the list has 361 values; the most allowed is 360." }, plan.Errors);
        }

        [Theory]
        [InlineData(-45, 315)]
        [InlineData(360, 0)]
        [InlineData(-360, 0)]
        [InlineData(375, 15)]
        public void NormalizeGivesZeroToBelow360(double degree, double expected) {
            var normalized = AzimuthPlanBuilder.Normalize(degree);

            Assert.Equal(expected, normalized);
            Assert.False(double.IsNegative(normalized));
        }

        [Theory]
        [InlineData(0, "N")]
        [InlineData(30, "NNE")]
        [InlineData(45, "NE")]
        [InlineData(300, "WNW")]
        [InlineData(350, "N")]
        public void CompassPointNamesSixteenPoints(double degree, string expected) {
            Assert.Equal(expected, AzimuthCompassPanel.CompassPointOf(degree));
        }

        [Fact]
        public void AnticlockwiseRangeLandingOnNorthSaysWhere() {
            var plan = Range("30", "350", "15", clockwise: false);

            Assert.Equal(new double[] { 30, 15, 0 }, Printed(plan));
            Assert.True(plan.CrossesNorth);
            Assert.Contains("Passes through north at #3 (0°).", plan.Infos);
            Assert.Equal("last; crosses north", plan.Items[2].Note);
        }

        [Fact]
        public void NorthIsOnlyClaimedWhenAnAzimuthReachesIt() {
            // The sweep covers north, but the last azimuth (350) stops short of it
            var plan = Range("300", "10", "25");

            Assert.Equal(new double[] { 300, 325, 350 }, Printed(plan));
            Assert.False(plan.CrossesNorth);
            Assert.DoesNotContain(plan.Infos, info => info.StartsWith("Passes through north"));
            Assert.Equal("300° to 10° clockwise, every 25° · covers 70°", plan.SpanText);
        }

        [Fact]
        public void TinyNegativeValuesNeverPrintAs360() {
            var plan = Range("-0.0000000000000000000000000001", "90", "15");

            Assert.Equal(0, plan.Items[0].PrintedDegree);
            // Within the 12 decimals shown, so it would otherwise print as "360"
            Assert.Equal("0", Range("-0.0000000000001", "90", "15").Items[0].PrintedText);
            Assert.Equal(0, AzimuthPlanBuilder.Normalize(-1e-14));
            Assert.Equal(0, AzimuthPlanBuilder.Normalize(-1e-300));
        }

        [Fact]
        public void DecimalRoundingNeverAddsAnAzimuthPastTheEnd() {
            var plan = Range("0", "7", "7.0000000000000000000000000001");

            Assert.Equal(new double[] { 0 }, Printed(plan));
            Assert.Contains("Step 7° is larger than the range (7°): only 1 azimuth.", plan.Warnings);
        }

        [Fact]
        public void EndIsEchoedInPlainDecimalNotation() {
            var plan = Range("0", "1e2", "15");

            Assert.Contains("Step 15° does not land on the end 100°: the last azimuth is 90°.", plan.Warnings);
            Assert.Contains("End 90° falls on a step, so it is included.", Range("0", "+90", "15").Infos);
        }

        [Fact]
        public void TooManyErrorEchoesTheStepAsTyped() {
            var plan = Range("0", "90", "1e-28");

            Assert.Equal(new[] {
                "Azimuth Range: 0 to 90 every 1e-28 gives more than 360 azimuths; the most allowed is 360. "
                + "Use a larger step or a smaller range."
            }, plan.Errors);
        }

        [Fact]
        public void NegativeStepSuggestsTheOtherDirection() {
            var plan = Range("0", "90", "-15", clockwise: false);

            Assert.Equal(new[] {
                "Azimuth Range Step (Degree): -15 is outside the range 0 to 360. "
                + "To go the other way round, choose Clockwise."
            }, plan.Errors);
        }

        [Fact]
        public void StartOrEndJustAbove360IsRejected() {
            var plan = Range("0", "360.0000000000000001", "15");

            Assert.Equal(new[] {
                "Azimuth Range End (Degree): 360.0000000000000001 is outside the range -360 to 360."
            }, plan.Errors);
        }

        [Fact]
        public void ListDuplicatesAreFoundAcrossNorth() {
            var plan = AzimuthPlanBuilder.BuildList("0,359.9999999999");

            Assert.True(plan.Items[1].IsDuplicate);
            Assert.Contains("#1 (0°) and #2 (359.9999999999°) point the same way; both will be calculated.",
                plan.Warnings);
        }

        [Theory]
        [InlineData("0–90", true)]
        [InlineData("0..90", true)]
        [InlineData("0 to 90", true)]
        [InlineData("auto", false)]
        [InlineData("abc", false)]
        public void RangeLikeListEntryGetsTheRangeHint(string text, bool expectHint) {
            var plan = AzimuthPlanBuilder.BuildList(text);

            Assert.Equal(expectHint, plan.Errors[0].Contains("select Range"));
        }

        [Theory]
        [InlineData(11.25, "NNE")]
        [InlineData(33.75, "NE")]
        [InlineData(348.75, "N")]
        public void CompassPointBoundariesGoClockwise(double degree, string expected) {
            Assert.Equal(expected, AzimuthCompassPanel.CompassPointOf(degree));
        }

        [Fact]
        public void FormatInputHasNoExponentOrNegativeZero() {
            Assert.Equal("0", InputUtil.FormatInput(-1e-13));
            Assert.Equal("0.00001", InputUtil.FormatInput(0.00001));
            Assert.Equal("45.5", InputUtil.FormatInput(45.5));
        }
    }
}
