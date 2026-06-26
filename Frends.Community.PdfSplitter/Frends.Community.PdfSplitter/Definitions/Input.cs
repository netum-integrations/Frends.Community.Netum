namespace Frends.Community.PdfSplitter.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Required input parameters for PDF splitting operation.
/// </summary>
public class Input
{
    /// <summary>
    ///  Full path to the PDF file.
    /// </summary>
    /// <example>c:\temp\foo.pdf</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string Path { get; set; }
}