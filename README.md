# Frends.Community.Netum

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

## Installing

You can install either task via Frends UI Task View or NuGet Package Manager

# Tasks

## MergePages Task

Merge multiple PDF files into a single document.

### Parameters

| Property    | Type         | Description                              | Example                       |
|-------------|--------------|------------------------------------------|-------------------------------|
| PageBytes   | List<byte[]> | List of PDF file contents as byte arrays | File.ReadAllBytes("file1.pdf") |

### Result

| Property        | Type     | Description                      | Example                       |
|-----------------|----------|----------------------------------|-------------------------------|
| OutputFileBytes | byte[]   | Merged PDF file as byte array    | result.OutputFileBytes         |

---

## SplitPages Task

Split PDF files into individual pages as separate documents.

### Parameters

| Property | Type   | Description                | Example           |
|----------|--------|----------------------------|-------------------|
| Path     | string | Full path to PDF file      | "C:\\temp\\doc.pdf" |

### Result

| Property | Type          | Description                           | Example           |
|----------|---------------|---------------------------------------|-------------------|
| Output   | List<byte[]>  | List of split pages as byte arrays    | result.Output[0]  |

---

## Building

### Clone the repository

```bash
git clone https://github.com/FrendsPlatform/Frends.Community.Netum.git
cd Frends.Community.Netum
```


### Run tests

```bash
dotnet test
```

### Create NuGet packages

```bash
# Build both packages
dotnet pack Frends.Community.PdfMerge --configuration Release
dotnet pack Frends.Community.PdfSplitter --configuration Release
```

## Third Party Licenses

- This project is licensed under the MIT License - see the LICENSE file for details
- Uses [PdfSharp](https://github.com/empira/PDFsharp) library for PDF manipulation

## Changelog

| Version | Date       | Task      | Description                    |
|---------|------------|-----------|--------------------------------|
| 1.0.0   | 2025-06-11 | PdfMerge  | Initial version of MergePDF    |
| 1.1.0   | 2026-06-26 | PdfMerge  | Cleanup                        |
| 1.0.0   | 2026-04-17 | PdfSplitter | Initial version of SplitPages |