namespace Frends.Community.PdfSplitter.Definitions;

using System.Collections.Generic;

/// <summary>
/// Result of PDF splitting operation. Contains a list of byte arrays, where each byte array represents a single PDF page.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="bytesList">List of PDF page byte arrays.</param>
    internal Result(List<byte[]> bytesList)
    {
        this.Output = bytesList;
    }

    /// <summary>
    /// Gets a list of byte arrays. Each byte array contains one PDF page.
    /// </summary>
    /// <example> If the original PDF has 3 pages, the Output list will contain 3 byte arrays, each representing one page of the original PDF.</example>
    public List<byte[]> Output { get; private set; }
}
