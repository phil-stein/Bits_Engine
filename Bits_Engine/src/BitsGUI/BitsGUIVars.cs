
namespace BitsCore.BitsGUI
{
    #region FORMATING_ENUMS
    /// <summary> Element alignment in the Window. </summary>
    public enum Alignment
    {
        /// <summary></summary>
        TopLeft,

        /// <summary></summary>
        TopCenter,

        /// <summary></summary>
        TopRight,

        /// <summary></summary>
        MiddleLeft,
        
        /// <summary></summary>
        MiddleCenter,

        /// <summary></summary>
        MiddleRight,

        /// <summary></summary>
        BottomLeft,

        /// <summary></summary>
        BottomCenter,

        /// <summary></summary>
        BottomRight,
    };

    /// <summary> Item alignment in an Element. </summary>
    public enum Order 
    {
        /// <summary></summary>
        Fill,
        
        /// <summary></summary>
        FreeFloat,

        /// <summary></summary>
        VerticalAscending,

        /// <summary></summary>
        VerticalCenter,

        /// <summary></summary>
        VerticalDescending,

        /// <summary></summary>
        HorizontalAscending,

        /// <summary></summary>
        HorizontalCenter,

        /// <summary></summary>
        HorizontalDescending,
    };

    /// <summary> Showing/Masking items in an Element when overflowing. </summary>
    public enum Overflow 
    {
        /// <summary></summary>
        Show,

        /// <summary></summary>
        Mask,
    };
    #endregion
}
