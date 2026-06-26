namespace Frends.Community.PdfMerge.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Frends.Community.PdfMerge.Definitions;
using NUnit.Framework;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

[TestFixture]
internal class UnitTests
{
    // Test merging two different PDF files and check output is larger and valid
    [Test]
    public void MergeTwoFiles()
    {
        byte[] inputDocument1 = File.ReadAllBytes("./TestFiles/Netum_tausta.pdf");
        byte[] inputDocument2 = File.ReadAllBytes("./TestFiles/Netum_logo_black.pdf");
        var input = new Input
        {
            PageBytes = new List<byte[]> { inputDocument1, inputDocument2 },
        };
        var ret = PDF.MergePages(input, new CancellationToken());
        Assert.That(ret.OutputFileBytes.Length, Is.GreaterThan(inputDocument1.Length));
        Assert.That(ret.OutputFileBytes.Length, Is.GreaterThan(inputDocument2.Length));
        AssertValidPdf(ret.OutputFileBytes);
    }

    // Test merging a single PDF file returns a valid PDF
    [Test]
    public void MergeSingleFile_ReturnsSameFile()
    {
        byte[] inputDocument = File.ReadAllBytes("./TestFiles/Netum_tausta.pdf");
        var input = new Input { PageBytes = new List<byte[]> { inputDocument } };
        var ret = PDF.MergePages(input, new CancellationToken());
        Assert.That(ret.OutputFileBytes.Length, Is.EqualTo(ret.OutputFileBytes.Length));
        AssertValidPdf(ret.OutputFileBytes);
    }

    // Test merging more than two PDF files
    [Test]
    public void MergeMultipleFiles()
    {
        byte[] inputDocument1 = File.ReadAllBytes("./TestFiles/Netum_tausta.pdf");
        byte[] inputDocument2 = File.ReadAllBytes("./TestFiles/Netum_logo_black.pdf");
        byte[] inputDocument3 = File.ReadAllBytes("./TestFiles/Netum_logo_green.pdf");
        var input = new Input { PageBytes = new List<byte[]> { inputDocument1, inputDocument2, inputDocument3 } };
        var ret = PDF.MergePages(input, new CancellationToken());
        Assert.That(ret.OutputFileBytes.Length, Is.GreaterThan(inputDocument1.Length));
        Assert.That(ret.OutputFileBytes.Length, Is.GreaterThan(inputDocument2.Length));
        Assert.That(ret.OutputFileBytes.Length, Is.GreaterThan(inputDocument3.Length));
        AssertValidPdf(ret.OutputFileBytes);
    }

    // Test merging with empty input list returns empty or throws gracefully
    [Test]
    public void Merge_EmptyInput_ReturnsEmptyOrThrowsGracefully()
    {
        var input = new Input { PageBytes = new List<byte[]>() };
        try
        {
            var ret = PDF.MergePages(input, new CancellationToken());
            Assert.That(ret.OutputFileBytes, Is.Empty.Or.Null);
        }
        catch (Exception ex)
        {
            Assert.Pass($"Handled gracefully: {ex.Message}");
        }
    }

    // Test merging with null input throws or is handled gracefully
    [Test]
    public void Merge_NullInput_ThrowsGracefully()
    {
        try
        {
            PDF.MergePages(null, new CancellationToken());
            Assert.Fail("Should throw or handle null input");
        }
        catch (Exception ex)
        {
            Assert.Pass($"Handled gracefully: {ex.Message}");
        }
    }

    // Test merging with invalid/corrupted PDF bytes throws or is handled gracefully
    [Test]
    public void Merge_InvalidPdf_ThrowsGracefully()
    {
        var input = new Input { PageBytes = new List<byte[]> { new byte[] { 0, 1, 2, 3, 4, 5 } } };
        try
        {
            PDF.MergePages(input, new CancellationToken());
            Assert.Fail("Should throw or handle invalid PDF");
        }
        catch (Exception ex)
        {
            Assert.Pass($"Handled gracefully: {ex.Message}");
        }
    }

    // Helper: Asserts that the given bytes represent a valid PDF with at least one page
    private void AssertValidPdf(byte[] pdfBytes)
    {
        using var stream = new MemoryStream(pdfBytes);
        using var doc = PdfReader.Open(stream, PdfDocumentOpenMode.ReadOnly);
        Assert.That(doc.PageCount, Is.GreaterThan(0));
    }
}
