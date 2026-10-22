namespace GeoCalculator.Codes.Controls {
    /// <summary>
    /// Colours shared by the azimuth preview (compass, labels, table) and the output status on the Distance From tab.
    /// </summary>
    public static class PreviewStyle {
        // Pixel sizes in the code are designed at this dpi and scaled to the screen's dpi
        public const float DesignDpi = 96;

        public static readonly Color ErrorText = Color.FromArgb(0xC4, 0x2B, 0x1C);
        public static readonly Color WarningText = Color.FromArgb(0x9D, 0x5D, 0x00);
        public static readonly Color InfoText = Color.FromArgb(0x5F, 0x5F, 0x5F);
        public static readonly Color ErrorBack = Color.FromArgb(0xFD, 0xE7, 0xE9);
        public static readonly Color DuplicateRow = Color.FromArgb(0xFF, 0xF4, 0xCE);

        public static readonly Color Ray = Color.FromArgb(0x00, 0x67, 0xC0);
        public static readonly Color FirstRay = Color.FromArgb(0x0F, 0x7B, 0x0F);
        public static readonly Color SelectedRay = Color.FromArgb(0xCA, 0x50, 0x10);
        public static readonly Color Ring = Color.FromArgb(0x8A, 0x8A, 0x8A);
        public static readonly Color HalfRing = Color.FromArgb(0xE0, 0xE0, 0xE0);
        public static readonly Color MinorTick = Color.FromArgb(0xB0, 0xB0, 0xB0);
        public static readonly Color MediumTick = Color.FromArgb(0x80, 0x80, 0x80);
        public static readonly Color MajorTick = Color.FromArgb(0x60, 0x60, 0x60);
        public static readonly Color CompassLetter = Color.FromArgb(0x40, 0x40, 0x40);

        // North and errors share the same red; duplicates share the warning amber
        public static Color North => ErrorText;
        public static Color DuplicateOutline => WarningText;
    }
}
