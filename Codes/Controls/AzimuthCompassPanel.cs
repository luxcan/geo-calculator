using GeoCalculator.Codes.Model;
using GeoCalculator.Codes.Util;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;

namespace GeoCalculator.Codes.Controls {
    /// <summary>
    /// Owner-drawn, north-up compass that previews the azimuths to calculate around the target: one numbered ray per
    /// direction, plus the sweep wedge and direction arc of a range. It only draws; the plan comes from
    /// <see cref="AzimuthPlanBuilder"/>.
    /// </summary>
    public class AzimuthCompassPanel : Panel {
        // Geometry, in pixels at 96 dpi; Px() scales them to the screen (the ring radius follows the control size)
        private const float RingMargin = 28;
        private const float RayInnerRadius = 5;
        private const float RayGapToBadge = 8;
        private const float BadgeHeight = 16;
        private const float BadgeMinWidth = 16;
        private const float BadgeTextPadding = 8;
        private const float BadgeMinGapPixels = 18;
        private const float DotRadius = 2;
        private const float TargetDotRadius = 3;
        private const float ArcRadius = 30;
        private const float LetterOffset = 16;
        private const float TagRadiusRatio = 0.62f;
        private const float TagOffsetPixels = 6;
        private const float MessageOffset = 22;
        private const float CaptionMargin = 4;
        private const float HoverInnerRadius = 12;
        private const float HoverOuterMargin = 10;
        private const float MinorTickLength = 3;
        private const float MediumTickLength = 6;
        private const float MajorTickLength = 9;
        private const float ArrowCapSize = 4;
        private const float ArcPenWidth = 2;
        private const float RayPenWidth = 2;
        private const float ThinRayPenWidth = 1;
        private const float SelectedRayPenWidth = 3;
        private const float DuplicateOutlineWidth = 2;
        private const double HoverToleranceDegree = 5;
        private const int ThinRayThreshold = 72;
        private const int MinorTickDegree = 10;
        private const int MediumTickDegree = 30;
        private const int MajorTickDegree = 90;
        private const float FullCircleArcDegree = 359;
        private const double CompassPointWidthDegree = 22.5;
        private const int WedgeAlpha = 38;

        private static readonly string[] CompassPoints = {
            "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"
        };

        private readonly ToolTip rayToolTip = new ToolTip();
        private readonly List<RayGroup> rayGroups = new List<RayGroup>();
        private AzimuthPlan? plan;
        private string distanceCaption = "";
        private long? firstRunningNo;
        private int selectedIndex = -1;
        private int hoverIndex = -1;

        public AzimuthCompassPanel() {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true);
            TabStop = false;
            BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Raised with the azimuth index when the operator clicks a ray.
        /// </summary>
        public event EventHandler<int>? RayClicked;

        /// <summary>
        /// Index of the highlighted azimuth, or -1 for none. Runtime state only, so the designer never saves it.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex {
            get => selectedIndex;
            set {
                if (selectedIndex != value) {
                    selectedIndex = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The 16-point compass name of an azimuth, e.g. 30 gives "NNE". A boundary (e.g. 11.25) goes to the next
        /// point clockwise.
        /// </summary>
        public static string CompassPointOf(double degree) {
            var point = Math.Round(AzimuthPlanBuilder.Normalize(degree) / CompassPointWidthDegree,
                MidpointRounding.AwayFromZero);
            // Mixed mode: the rounded point number (0 to 16) is used as a whole-number index
            return CompassPoints[(int)point % CompassPoints.Length];
        }

        /// <summary>
        /// Shows a new plan. The distance caption (e.g. "10 km surface") is drawn in the bottom-right corner; when a
        /// running number is given, the badges show running numbers instead of #1, #2, ...
        /// </summary>
        public void SetPlan(AzimuthPlan? newPlan, string newDistanceCaption, long? newFirstRunningNo) {
            plan = newPlan;
            distanceCaption = newDistanceCaption;
            firstRunningNo = newFirstRunningNo;
            hoverIndex = -1;
            BuildRayGroups();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            bool hasErrors = plan != null && plan.Errors.Count > 0;
            bool isValid = plan != null && plan.IsValid;
            if (isValid && plan!.Mode == AzimuthMode.Range && plan.SweepDegree > 0) {
                DrawWedge(graphics);
            }
            DrawFace(graphics, hasErrors);
            if (isValid && plan!.Mode == AzimuthMode.Range) {
                DrawDirectionArc(graphics);
            }
            if (isValid) {
                DrawRays(graphics);
            }
            DrawTarget(graphics);
            if (!isValid) {
                DrawMessage(graphics, hasErrors ? "No valid azimuths" : "Key in azimuths",
                    hasErrors ? PreviewStyle.ErrorText : SystemColors.GrayText);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            var index = FindRayAt(e.Location);
            if (index != hoverIndex) {
                hoverIndex = index;
                rayToolTip.SetToolTip(this, index >= 0 ? DescribeRayGroup(index) : "");
            }
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            hoverIndex = -1;
            rayToolTip.SetToolTip(this, "");
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            var index = FindRayAt(e.Location);
            if (index >= 0) {
                RayClicked?.Invoke(this, index);
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                rayToolTip.Dispose();
            }
            base.Dispose(disposing);
        }

        private PointF Center => new PointF(ClientSize.Width / 2f, ClientSize.Height / 2f);

        private float RingRadius => Math.Min(ClientSize.Width, ClientSize.Height) / 2f - Px(RingMargin);

        /// <summary>
        /// Scales a length designed at 96 dpi to the screen's dpi.
        /// </summary>
        private float Px(float designPixels) {
            // Mixed mode: the whole-number DeviceDpi is used as a float ratio
            return designPixels * DeviceDpi / PreviewStyle.DesignDpi;
        }

        /// <summary>
        /// Screen point for an azimuth (clockwise from north, north up) at a radius from the centre.
        /// </summary>
        private PointF PointAt(double azimuthDegree, float radius) {
            double radian = azimuthDegree * Math.PI / 180.0;
            return new PointF(Center.X + radius * (float)Math.Sin(radian), Center.Y - radius * (float)Math.Cos(radian));
        }

        private RectangleF CircleBounds(float radius) {
            return new RectangleF(Center.X - radius, Center.Y - radius, radius * 2, radius * 2);
        }

        private void BuildRayGroups() {
            rayGroups.Clear();
            if (plan != null && plan.IsValid) {
                foreach (var item in plan.Items) {
                    var rayGroup = rayGroups.Find(candidate =>
                        AzimuthPlanBuilder.AngleBetween(candidate.DirectionDegree, item.DirectionDegree)
                        < AzimuthPlanBuilder.DuplicateToleranceDegree);
                    if (rayGroup == null) {
                        rayGroup = new RayGroup(item.DirectionDegree);
                        rayGroups.Add(rayGroup);
                    }
                    rayGroup.Indexes.Add(item.Index);
                }
            }
        }

        private void DrawWedge(Graphics graphics) {
            using var brush = new SolidBrush(Color.FromArgb(WedgeAlpha, PreviewStyle.Ray));
            var bounds = CircleBounds(RingRadius);
            if (plan!.IsFullCircle) {
                graphics.FillEllipse(brush, bounds);
            } else {
                // GDI+ measures angles from 3 o'clock, clockwise; azimuth is measured from 12 o'clock
                float sweep = (float)(plan.Clockwise ? plan.SweepDegree : -plan.SweepDegree);
                graphics.FillPie(brush, bounds.X, bounds.Y, bounds.Width, bounds.Height,
                    (float)plan.StartDirectionDegree - 90, sweep);
            }
        }

        private void DrawFace(Graphics graphics, bool hasErrors) {
            var radius = RingRadius;
            using (var ringPen = new Pen(hasErrors ? SystemColors.GrayText : PreviewStyle.Ring, 1))
            using (var halfRingPen = new Pen(PreviewStyle.HalfRing, 1) { DashStyle = DashStyle.Dot }) {
                graphics.DrawEllipse(ringPen, CircleBounds(radius));
                graphics.DrawEllipse(halfRingPen, CircleBounds(radius / 2));
            }

            for (int degree = 0; degree < 360; degree += MinorTickDegree) {
                bool isMajor = degree % MajorTickDegree == 0;
                bool isMedium = degree % MediumTickDegree == 0;
                var length = Px(isMajor ? MajorTickLength : (isMedium ? MediumTickLength : MinorTickLength));
                var color = isMajor ? PreviewStyle.MajorTick
                    : (isMedium ? PreviewStyle.MediumTick : PreviewStyle.MinorTick);
                using var tickPen = new Pen(color, 1);
                graphics.DrawLine(tickPen, PointAt(degree, radius), PointAt(degree, radius - length));
            }

            var letterRadius = radius + Px(LetterOffset);
            using (var northFont = new Font(Font.FontFamily, 9f, FontStyle.Bold))
            using (var letterFont = new Font(Font.FontFamily, 8.25f, FontStyle.Bold))
            using (var northBrush = new SolidBrush(PreviewStyle.North))
            using (var letterBrush = new SolidBrush(PreviewStyle.CompassLetter)) {
                DrawCentredText(graphics, "N", northFont, northBrush, PointAt(0, letterRadius));
                DrawCentredText(graphics, "E", letterFont, letterBrush, PointAt(90, letterRadius));
                DrawCentredText(graphics, "S", letterFont, letterBrush, PointAt(180, letterRadius));
                DrawCentredText(graphics, "W", letterFont, letterBrush, PointAt(270, letterRadius));
            }

            var margin = Px(CaptionMargin);
            using var captionFont = new Font(Font.FontFamily, 7.5f, FontStyle.Regular);
            using var captionBrush = new SolidBrush(PreviewStyle.InfoText);
            graphics.DrawString("● Target", captionFont, captionBrush, margin, margin);
            var bottom = ClientSize.Height - margin - captionFont.GetHeight(graphics);
            graphics.DrawString("Not to scale", captionFont, captionBrush, margin, bottom);
            if (distanceCaption != "") {
                var width = graphics.MeasureString(distanceCaption, captionFont).Width;
                var left = ClientSize.Width - margin - width;
                graphics.DrawString(distanceCaption, captionFont, captionBrush, left, bottom);
            }
        }

        private void DrawDirectionArc(Graphics graphics) {
            var count = plan!.Items.Count;
            float sweep = plan.IsFullCircle ? FullCircleArcDegree : (float)((count - 1) * plan.StepDegree);
            if (sweep > 0) {
                using var arrowCap = new AdjustableArrowCap(ArrowCapSize, ArrowCapSize);
                using var arcPen = new Pen(PreviewStyle.Ray, Px(ArcPenWidth)) { CustomEndCap = arrowCap };
                var bounds = CircleBounds(Px(ArcRadius));
                graphics.DrawArc(arcPen, bounds, (float)plan.StartDirectionDegree - 90,
                    plan.Clockwise ? sweep : -sweep);
            }
        }

        private void DrawRays(Graphics graphics) {
            var radius = RingRadius;
            var lastIndex = plan!.Items.Count - 1;
            bool thin = plan.Items.Count > ThinRayThreshold;

            using var badgeFont = new Font(Font.FontFamily, 7f, FontStyle.Bold);
            using var tagFont = new Font(Font.FontFamily, 7.5f, FontStyle.Bold);
            using var whiteBrush = new SolidBrush(Color.White);
            float neededPixels = BadgeSpacingNeeded(graphics, badgeFont);
            int showEvery = BadgeInterval(radius, neededPixels);
            var firstGroup = rayGroups.Find(rayGroup => rayGroup.Indexes.Contains(0));

            // #1 is drawn last but one and the selected azimuth last, so that neither is hidden by a neighbour
            var drawOrder = rayGroups.OrderBy(rayGroup => rayGroup.Indexes.Contains(selectedIndex) ? 2
                : (rayGroup.Indexes.Contains(0) ? 1 : 0)).ToList();
            foreach (var rayGroup in drawOrder) {
                var first = rayGroup.Indexes[0];
                bool hasFirst = rayGroup.Indexes.Contains(0);
                bool hasLast = rayGroup.Indexes.Contains(lastIndex);
                bool selected = rayGroup.Indexes.Contains(selectedIndex);
                // #N gives way to #1 when the azimuths almost close the circle; every n-th badge gives way to #N
                bool lastCrowdsFirst = hasLast && !hasFirst && firstGroup != null
                    && ArcPixels(rayGroup, firstGroup, radius) < neededPixels;
                bool regularBadge = first % showEvery == 0 && (showEvery == 1 || lastIndex - first >= showEvery);
                bool showBadge = hasFirst || (hasLast && !lastCrowdsFirst) || (!thin && regularBadge);
                var color = selected ? PreviewStyle.SelectedRay : (hasFirst ? PreviewStyle.FirstRay : PreviewStyle.Ray);
                float width = Px(selected ? SelectedRayPenWidth : (thin ? ThinRayPenWidth : RayPenWidth));

                using (var rayPen = new Pen(color, width) { StartCap = LineCap.Round, EndCap = LineCap.Round }) {
                    graphics.DrawLine(rayPen, PointAt(rayGroup.DirectionDegree, Px(RayInnerRadius)),
                        PointAt(rayGroup.DirectionDegree, radius - Px(RayGapToBadge)));
                }
                var badgeCentre = PointAt(rayGroup.DirectionDegree, radius);
                if (showBadge) {
                    var label = BadgeLabel(rayGroup);
                    DrawBadge(graphics, badgeCentre, label, color, rayGroup.Indexes.Count > 1, badgeFont, whiteBrush,
                        Px(BadgeHeight), Px(BadgeMinWidth), Px(BadgeTextPadding), Px(DuplicateOutlineWidth));
                } else if (!thin) {
                    var dotRadius = Px(DotRadius);
                    using var dotBrush = new SolidBrush(color);
                    graphics.FillEllipse(dotBrush, badgeCentre.X - dotRadius, badgeCentre.Y - dotRadius,
                        dotRadius * 2, dotRadius * 2);
                }
                if (selected) {
                    DrawValueTag(graphics, rayGroup, radius, tagFont);
                }
            }
        }

        /// <summary>
        /// Writes the selected azimuth's value beside its ray, part way out, on a small plain background so that the
        /// rays never strike through the digits.
        /// </summary>
        private void DrawValueTag(Graphics graphics, RayGroup rayGroup, float radius, Font tagFont) {
            var tagText = plan!.Items[selectedIndex].PrintedText + "°";
            var tagSize = graphics.MeasureString(tagText, tagFont);
            var tagRadius = radius * TagRadiusRatio;
            // Clockwise of the ray by half the text width plus a small gap
            var offsetDegree = (Px(TagOffsetPixels) + tagSize.Width / 2) / tagRadius * 180 / Math.PI;
            var tagCentre = PointAt(rayGroup.DirectionDegree + offsetDegree, tagRadius);
            using var backgroundBrush = new SolidBrush(BackColor);
            using var tagBrush = new SolidBrush(PreviewStyle.SelectedRay);
            graphics.FillRectangle(backgroundBrush, tagCentre.X - tagSize.Width / 2, tagCentre.Y - tagSize.Height / 2,
                tagSize.Width, tagSize.Height);
            DrawCentredText(graphics, tagText, tagFont, tagBrush, tagCentre);
        }

        /// <summary>
        /// The distance between two badge centres along the ring, in pixels.
        /// </summary>
        private static double ArcPixels(RayGroup firstRay, RayGroup secondRay, float radius) {
            return AzimuthPlanBuilder.AngleBetween(firstRay.DirectionDegree, secondRay.DirectionDegree) * Math.PI / 180
                * radius;
        }

        /// <summary>
        /// The centre-to-centre spacing two neighbouring badges need: the widest label (e.g. a running number like
        /// "103") or the minimum gap, whichever is larger. Neighbouring badges may touch but never overlap.
        /// </summary>
        private float BadgeSpacingNeeded(Graphics graphics, Font badgeFont) {
            var widestLabel = rayGroups.Select(rayGroup => graphics.MeasureString(BadgeLabel(rayGroup), badgeFont).Width
                + Px(BadgeTextPadding)).DefaultIfEmpty(0).Max();
            return Math.Max(Px(BadgeMinGapPixels), widestLabel);
        }

        /// <summary>
        /// Draws every badge when they fit; otherwise every n-th badge, so that neighbouring badges do not overlap.
        /// </summary>
        private int BadgeInterval(float radius, float neededPixels) {
            var directions = rayGroups.Select(rayGroup => rayGroup.DirectionDegree).OrderBy(direction => direction)
                .ToList();
            double smallestGap = double.MaxValue;
            for (int index = 0; index < directions.Count && directions.Count > 1; index++) {
                var next = index + 1 < directions.Count ? directions[index + 1] : directions[0] + 360;
                smallestGap = Math.Min(smallestGap, next - directions[index]);
            }
            double gapPixels = smallestGap * Math.PI / 180 * radius;
            // Mixed mode: the interval is a whole number of rays, rounded up from the ratio of pixel lengths
            return gapPixels >= neededPixels ? 1 : (int)Math.Ceiling(neededPixels / gapPixels);
        }

        private static void DrawBadge(Graphics graphics, PointF centre, string label, Color color, bool isDuplicate,
            Font badgeFont, Brush textBrush, float height, float minWidth, float padding, float outlineWidth) {
            var textWidth = graphics.MeasureString(label, badgeFont).Width;
            var width = Math.Max(minWidth, textWidth + padding);
            var bounds = new RectangleF(centre.X - width / 2, centre.Y - height / 2, width, height);
            using (var path = PillPath(bounds))
            using (var fill = new SolidBrush(color)) {
                graphics.FillPath(fill, path);
                if (isDuplicate) {
                    using var outline = new Pen(PreviewStyle.DuplicateOutline, outlineWidth);
                    graphics.DrawPath(outline, path);
                }
            }
            DrawCentredText(graphics, label, badgeFont, textBrush, centre);
        }

        private static GraphicsPath PillPath(RectangleF bounds) {
            var path = new GraphicsPath();
            var diameter = bounds.Height;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 90, 180);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 180);
            path.CloseFigure();
            return path;
        }

        private void DrawTarget(Graphics graphics) {
            using var targetBrush = new SolidBrush(Color.Black);
            using var outline = new Pen(Color.White, 1);
            var bounds = CircleBounds(Px(TargetDotRadius));
            graphics.FillEllipse(targetBrush, bounds);
            graphics.DrawEllipse(outline, bounds);
        }

        private void DrawMessage(Graphics graphics, string message, Color color) {
            using var messageFont = new Font(Font.FontFamily, 8.25f, FontStyle.Regular);
            using var messageBrush = new SolidBrush(color);
            var position = new PointF(Center.X, Center.Y + Px(MessageOffset));
            DrawCentredText(graphics, message, messageFont, messageBrush, position);
        }

        private static void DrawCentredText(Graphics graphics, string text, Font font, Brush brush, PointF centre) {
            using var format = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            graphics.DrawString(text, font, brush, centre, format);
        }

        private string BadgeLabel(RayGroup rayGroup) {
            return string.Join(",", rayGroup.Indexes.Select(BadgeNumber));
        }

        private string BadgeNumber(int index) {
            return firstRunningNo.HasValue
                ? (firstRunningNo.Value + index).ToString(CultureInfo.InvariantCulture)
                : (index + 1).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Describes every azimuth on the ray that holds the given azimuth, one line each (list duplicates share a
        /// ray).
        /// </summary>
        private string DescribeRayGroup(int index) {
            var rayGroup = rayGroups.Find(candidate => candidate.Indexes.Contains(index));
            var indexes = rayGroup != null ? rayGroup.Indexes : new List<int> { index };
            return string.Join("\n", indexes.Select(DescribeRay));
        }

        private string DescribeRay(int index) {
            var item = plan!.Items[index];
            var text = $"#{index + 1}  {item.PrintedText}° ({CompassPointOf(item.DirectionDegree)})";
            if (firstRunningNo.HasValue) {
                text += "  Running No " + (firstRunningNo.Value + index).ToString(CultureInfo.InvariantCulture);
            }
            // e.g. "start", "end", "crosses north" or "same as #1"
            if (item.Note != "") {
                text += $"  ({item.Note})";
            }
            return text;
        }

        /// <summary>
        /// The azimuth index of the ray nearest the mouse (within a few degrees), or -1.
        /// </summary>
        private int FindRayAt(Point location) {
            var index = -1;
            var dx = location.X - Center.X;
            var dy = location.Y - Center.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            bool inRing = distance >= Px(HoverInnerRadius) && distance <= RingRadius + Px(HoverOuterMargin);
            if (rayGroups.Count > 0 && inRing) {
                var mouseDegree = AzimuthPlanBuilder.Normalize(Math.Atan2(dx, -dy) * 180 / Math.PI);
                var bestGap = HoverToleranceDegree;
                foreach (var rayGroup in rayGroups) {
                    var gap = AzimuthPlanBuilder.AngleBetween(mouseDegree, rayGroup.DirectionDegree);
                    if (gap <= bestGap) {
                        bestGap = gap;
                        index = rayGroup.Indexes[0];
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// Azimuths that point the same way share one ray.
        /// </summary>
        private sealed class RayGroup {
            public RayGroup(double directionDegree) {
                DirectionDegree = directionDegree;
            }

            public double DirectionDegree { get; }

            public List<int> Indexes { get; } = new List<int>();
        }
    }
}
