namespace Frends.Community.PdfMerge.Definitions;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Input
{
    /// <summary>
    /// List of PDF files to merge
    /// </summary>
    /// <example>new List&lt;byte[]&gt; { new byte[] { 0x25, 0x50 }, new byte[] { 0x44, 0x46 } }</example>
    [DisplayName("PDF file")]
    [DisplayFormat(DataFormatString = "Expression")]
    [DefaultValue("")]
    public List<byte[]> PageBytes { get; set; }
}