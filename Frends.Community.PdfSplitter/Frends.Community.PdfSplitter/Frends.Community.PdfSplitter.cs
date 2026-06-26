namespace Frends.Community.PdfSplitter;

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Frends.Community.PdfSplitter.Definitions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class PDF
{
    /// <summary>
    /// Takes a PDF file path as input, and splits each page in the PDF into a separate PDF file. The output is a list of byte arrays, where each byte array represents a single PDF page.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Community.PdfSplitter).
    /// </summary>
    /// <param name="input">File to split.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { List&lt;byte[]&gt; Output }.</returns>
    public static Result SplitPages([PropertyTab] Input input, CancellationToken cancellationToken)
    {
        var filename = input.Path;
        using PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import);

        List<byte[]> bytesList = new List<byte[]>();

        for (int idx = 0; idx < inputDocument.PageCount; idx++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using MemoryStream stream = new MemoryStream();
            using PdfDocument outputDocument = new PdfDocument();
            outputDocument.Version = inputDocument.Version;
            outputDocument.Info.Title = string.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
            outputDocument.Info.Creator = inputDocument.Info.Creator;

            outputDocument.AddPage(inputDocument.Pages[idx]);
            outputDocument.Save(stream, false);

            byte[] bytes = stream.ToArray();
            bytesList.Add(bytes);
        }

        var output = new Result(bytesList);

        return output;
    }
}
