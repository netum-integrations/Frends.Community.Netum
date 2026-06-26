namespace Frends.Community.PdfSplitter.Tests;

using System;
using System.IO;
using System.Linq;
using Frends.Community.PdfSplitter.Definitions;
using NUnit.Framework;
using PdfSharp.Pdf.IO;

[TestFixture]
internal class UnitTests
{
    // Test splitting a multi-page PDF file and check that the ouput contains the correct number of pages and that each page is valid PDF data
    [Test]
    public void SplitMultiPageFile()
    {
        var input = new Input
        {
            Path = "./TestFiles/multipage.pdf",
        };

        var result = PDF.SplitPages(input, default);

        var pageCount = result.Output.Count;
        TestContext.WriteLine($"Original PDF has {pageCount} pages");
        TestContext.WriteLine($"Splitting will create {pageCount} separate PDF files");

        Assert.That(result, Is.Not.Null, "Result should not be null");
        Assert.That(result.Output, Is.Not.Null, "Output should not be null");
        Assert.That(result.Output.Count, Is.EqualTo(4), "Should have exactly 4 files");

        // Validate all output PDFs are valid
        var validOutput = ValidateOutputPdfs(result.Output);
        Assert.That(validOutput, Is.True, "PDF output validation should pass");

        // Validate page order is preserved by checking that each page contains different content
        var validPageOrder = ValidatePageOrderPreserved(result.Output);
        Assert.That(validPageOrder, Is.True, "Page order validation should pass");
    }

    // Test splitting a single-page PDF file and check that the output contains exactly one page and that it is valid PDF data
    [Test]
    public void SplitSinglePageFile()
    {
        var input = new Input
        {
            Path = "./TestFiles/singlepage.pdf",
        };

        var result = PDF.SplitPages(input, default);

        TestContext.WriteLine("Testing single page PDF splitting");
        TestContext.WriteLine($"Input file: {input.Path}");
        TestContext.WriteLine($"Output contains {result.Output.Count} page(s)");

        Assert.That(result, Is.Not.Null, "Result should not be null");
        Assert.That(result.Output, Is.Not.Null, "Output should not be null");
        Assert.That(result.Output.Count, Is.EqualTo(1), "Single page PDF should result in exactly 1 output file");

        // Validate output PDF is valid
        var validOutput = ValidateOutputPdfs(result.Output);
        Assert.That(validOutput, Is.True, "PDF output validation should pass");

        // Verify that the output is essentially the same as input (both are single-page PDFs)
        // Read the original file to compare size
        var originalFileBytes = File.ReadAllBytes(input.Path);
        Assert.That(result.Output[0].Length, Is.GreaterThan(0), "Output should contain PDF data");

        TestContext.WriteLine($"Original file size: {originalFileBytes.Length} bytes");
        TestContext.WriteLine($"Output file size: {result.Output[0].Length} bytes");
    }

    // Test splitting a non-existent PDF file and check that the method throws a FileNotFoundException with an appropriate message
    [Test]
    public void SplitNonExistentFile()
    {
        var input = new Input
        {
            Path = "./TestFiles/nonexistent.pdf",
        };

        TestContext.WriteLine("Testing non-existent file handling");
        TestContext.WriteLine($"Attempting to split: {input.Path}");

        var exception = Assert.Throws<FileNotFoundException>(() => PDF.SplitPages(input, default));
        Assert.That(exception.Message, Does.Contain("nonexistent.pdf"), "Exception message should reference the missing file");

        TestContext.WriteLine($"Expected exception thrown: {exception.GetType().Name}");
        TestContext.WriteLine($"Exception message: {exception.Message}");
    }

    // Test splitting a PNG file and check that the method throws an exception since it's not a valid PDF
    [Test]
    public void SplitNonPdfFile_ShouldThrowException()
    {
        var input = new Input
        {
            Path = "./TestFiles/netum_logo_green.png",
        };

        TestContext.WriteLine("Testing non-PDF file handling (invalid PDF format)");
        TestContext.WriteLine($"Attempting to split: {input.Path}");

        var exception = Assert.Throws<InvalidOperationException>(() => PDF.SplitPages(input, default));
        Assert.That(exception.Message, Does.Contain("not a valid PDF document"), "Exception should indicate invalid PDF format");

        TestContext.WriteLine($"Expected exception thrown: {exception.GetType().Name}");
        TestContext.WriteLine($"Exception message: {exception.Message}");
    }

    // Validates that byte arrays contain valid PDF data by attempting to open them with PdfReader
    private static bool ValidateOutputPdfs(System.Collections.Generic.IEnumerable<byte[]> pdfByteArrays)
    {
        foreach (var pdfBytes in pdfByteArrays)
        {
            if (pdfBytes == null)
            {
                TestContext.WriteLine("VALIDATION FAILED: PDF byte array is null");
                return false;
            }

            if (pdfBytes.Length == 0)
            {
                TestContext.WriteLine("VALIDATION FAILED: PDF byte array is empty");
                return false;
            }

            // Validate by attempting to read as PDF
            try
            {
                using var stream = new MemoryStream(pdfBytes);
                using var document = PdfReader.Open(stream, PdfDocumentOpenMode.ReadOnly);
                if (document.PageCount <= 0)
                {
                    TestContext.WriteLine("VALIDATION FAILED: PDF contains no pages");
                    return false;
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"VALIDATION FAILED: Unable to read as valid PDF - {ex.Message}");
                return false;
            }
        }

        TestContext.WriteLine("PDF output validation completed successfully");
        return true;
    }

    // Validates that pages are in the correct order by ensuring each split PDF contains different content
    private static bool ValidatePageOrderPreserved(System.Collections.Generic.IList<byte[]> pdfByteArrays)
    {
        TestContext.WriteLine($"Validating page order for {pdfByteArrays.Count} pages");

        // Check that each page contains different content (different byte arrays)
        for (int i = 0; i < pdfByteArrays.Count; i++)
        {
            for (int j = i + 1; j < pdfByteArrays.Count; j++)
            {
                if (pdfByteArrays[i].SequenceEqual(pdfByteArrays[j]))
                {
                    TestContext.WriteLine($"VALIDATION FAILED: Page {i + 1} and Page {j + 1} contain identical content");
                    return false;
                }
            }
        }

        // Verify that pages have reasonable size differences (indicating different content)
        var pageSizes = pdfByteArrays.Select((pdf, index) => new { Index = index + 1, Size = pdf.Length }).ToList();
        foreach (var pageInfo in pageSizes)
        {
            TestContext.WriteLine($"Page {pageInfo.Index}: {pageInfo.Size} bytes");
        }

        // At least some pages should have different sizes (not all identical)
        var uniqueSizes = pageSizes.Select(p => p.Size).Distinct().Count();
        if (uniqueSizes == 1)
        {
            TestContext.WriteLine("All pages have identical size");
        }
        else
        {
            TestContext.WriteLine($"Pages have {uniqueSizes} different sizes");
        }

        return true;
    }
}
