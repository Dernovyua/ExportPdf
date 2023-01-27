namespace Export.Enums
{
    public enum HorizontalAlignment
    {
        //
        // Summary:
        //     The exact position is specified by the DevExpress.XtraRichEdit.API.Native.Table.OffsetXRelative
        //     property.
        None = 0,
        //
        // Summary:
        //     The table is left aligned relative to its DevExpress.XtraRichEdit.API.Native.Table.RelativeHorizontalPosition.
        Left = 3,
        //
        // Summary:
        //     The table is centered relative to its DevExpress.XtraRichEdit.API.Native.Table.RelativeHorizontalPosition.
        Center = 1,
        //
        // Summary:
        //     The table is right aligned relative to its DevExpress.XtraRichEdit.API.Native.Table.RelativeHorizontalPosition.
        Right = 5,
        //
        // Summary:
        //     The table is inside relative to the element specified by the DevExpress.XtraRichEdit.API.Native.Table.RelativeHorizontalPosition.
        //     Not currently supported.
        Inside = 2,
        //
        // Summary:
        //     The table is outside relative to the element specified by the DevExpress.XtraRichEdit.API.Native.Table.RelativeHorizontalPosition.
        //     Not currently supported.
        Outside = 4
    }
}
