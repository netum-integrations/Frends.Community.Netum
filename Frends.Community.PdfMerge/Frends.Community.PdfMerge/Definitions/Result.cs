namespace Frends.Community.PdfMerge.Definitions;

/// <summary>
/// Result of PDF merge operation containing the merged PDF file.
/// </summary>
public class Result
{
    internal Result(byte[] output)
    {
        this.OutputFileBytes = output;
    }

    /// <summary>
    /// The merged PDF file as a byte array.
    /// </summary>
    /// <example>new byte[] { 0x25, 0x50, 0x44, 0x46 }</example>
    public byte[] OutputFileBytes { get; private set; }
}
