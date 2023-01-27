namespace Export.Enums
{
    public enum BorderLineStyle
    {
        //
        // Summary:
        //     No border.
        Nil = -1,
        //
        // Summary:
        //     No border.
        None,
        //
        // Summary:
        //     A single solid line.
        Single,
        //
        // Summary:
        //     Single line.
        Thick,
        //
        // Summary:
        //     Double solid lines.
        Double,
        //
        // Summary:
        //     Dots.
        Dotted,
        //
        // Summary:
        //     Dashes.
        Dashed,
        //
        // Summary:
        //     A dash followed by a dot.
        DotDash,
        //
        // Summary:
        //     A dash followed by two dots.
        DotDotDash,
        //
        // Summary:
        //     Triple line.
        Triple,
        //
        // Summary:
        //     An internal single thin solid line surrounded by a single thick solid line with
        //     a small gap between them.
        ThinThickSmallGap,
        //
        // Summary:
        //     An internal single thick solid line surrounded by a single thin solid line with
        //     a small gap between them.
        ThickThinSmallGap,
        //
        // Summary:
        //     An internal single thin solid line surrounded by a single thick solid line surrounded
        //     by a single thin solid line with a small gap between all lines.
        ThinThickThinSmallGap,
        //
        // Summary:
        //     An internal single thin solid line surrounded by a single thick solid line with
        //     a medium gap between them.
        ThinThickMediumGap,
        //
        // Summary:
        //     An internal single thick solid line surrounded by a single thin solid line with
        //     a medium gap between them.
        ThickThinMediumGap,
        //
        // Summary:
        //     An internal single thin solid line surrounded by a single thick solid line surrounded
        //     by a single thin solid line with a medium gap between all lines.
        ThinThickThinMediumGap,
        //
        // Summary:
        //     An internal single thin solid line surrounded by a single thick solid line with
        //     a large gap between them.
        ThinThickLargeGap,
        //
        // Summary:
        //     An internal single thick solid line surrounded by a single thin solid line with
        //     a large gap between them.
        ThickThinLargeGap,
        //
        // Summary:
        //     An internal single thin solid line surrounded by a single thick solid line surrounded
        //     by a single thin solid line with a large gap between all lines.
        ThinThickThinLargeGap,
        //
        // Summary:
        //     Wavy line.
        Wave,
        //
        // Summary:
        //     Double wavy solid lines.
        DoubleWave,
        //
        // Summary:
        //     A dash followed by a small gap.
        DashSmallGap,
        //
        // Summary:
        //     A series of alternating thin and thick strokes, resembling a barber pole.
        DashDotStroked,
        //
        // Summary:
        //     A line border consisting of three staged gradient lines around the cell, getting
        //     darker towards the cell.
        ThreeDEmboss,
        //
        // Summary:
        //     A line border consisting of three staged gradient lines around the cell, getting
        //     darker away from the cell.
        ThreeDEngrave,
        //
        // Summary:
        //     The border appears to be outset.
        Outset,
        //
        // Summary:
        //     The border appears to be inset.
        Inset
    }
}
