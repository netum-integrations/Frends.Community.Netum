namespace Frends.Community.PdfMerge;

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Frends.Community.PdfMerge.Definitions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class PDF
{
    /// <summary>
    /// Task for merging PDF pages into a single document
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Community.PdfMerge).
    /// </summary>
    /// <param name="input">A list of page byte arrays to merge</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { byte[] OutputFileBytes }.</returns>
    public static Result MergePages([PropertyTab] Input input, CancellationToken cancellationToken)
    {
        List<byte[]> pageBytes = input.PageBytes;
        PdfDocument outputPDFDocument = new PdfDocument();
        foreach (byte[] pdfBytes in pageBytes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using MemoryStream stream = new MemoryStream(pdfBytes);
            using PdfDocument inputPDFDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
            outputPDFDocument.Version = inputPDFDocument.Version;
            foreach (PdfPage page in inputPDFDocument.Pages)
            {
                outputPDFDocument.AddPage(page);
            }
        }

        using MemoryStream outputStream = new ();
        outputPDFDocument.Save(outputStream, false);
        byte[] outputBytes = outputStream.ToArray();

        return new Result(outputBytes);
    }
}
