namespace Export.Enums
{
    public enum ParagraphAlignment
    {
        //
        // Summary:
        //     Text is aligned to the left of the paragraph.
        Left = 0,
        //
        // Summary:
        //     Text is aligned to the right of the paragraph.
        Right = 1,
        //
        // Summary:
        //     Text is aligned to the center of the paragraph.
        Center = 2,
        //
        // Summary:
        //     Text is justified to the entire width of the paragraph.
        Justify = 3,
        //
        // Summary:
        //     If the language is Arabic, the paragraph uses small length Kashida. In other
        //     languages, text is justified with short inter-word spacing.
        JustifyLow = 6,
        //
        // Summary:
        //     If the language is Arabic, the paragraph uses medium-length Kashida. In other
        //     languages, text is justified with wider inter-word spacing.
        JustifyMedium = 4,
        //
        // Summary:
        //     If the text is Arabic, the paragraph uses the kashida with the widest length.
        //     For other languages, paragraph text is justified with the widest inter-word spacing.
        JustifyHigh = 5,
        //
        // Summary:
        //     The text is evenly distributed between right and left margins. If the last line
        //     is short, extra space is added between the characters so the line matches the
        //     paragraph’s width.
        Distribute = 7,
        //
        // Summary:
        //     Text is aligned between right and left margins by adding an extra space between
        //     each two characters. Affects languages with tone marks and vowel marks.
        ThaiDistribute = 8
    }
}
