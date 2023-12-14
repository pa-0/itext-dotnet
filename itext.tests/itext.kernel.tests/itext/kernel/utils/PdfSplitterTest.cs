/*
This file is part of the iText (R) project.
Copyright (c) 1998-2023 Apryse Group NV
Authors: Apryse Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Kernel.Utils {
    public class PdfSplitterTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/utils/PdfSplitterTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/utils/PdfSplitterTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.SOURCE_DOCUMENT_HAS_ACROFORM_DICTIONARY, Count = 3)]
        public virtual void SplitDocumentTest01() {
            String inputFileName = sourceFolder + "iphone_user_guide.pdf";
            PdfDocument inputPdfDoc = new PdfDocument(new PdfReader(inputFileName));
            IList<int> pageNumbers = JavaUtil.ArraysAsList(30, 100);
            IList<PdfDocument> splitDocuments = new _PdfSplitter_85(inputPdfDoc).SplitByPageNumbers(pageNumbers);
            foreach (PdfDocument doc in splitDocuments) {
                doc.Close();
            }
            for (int i = 1; i <= 3; i++) {
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "splitDocument1_" + i
                    .ToString() + ".pdf", sourceFolder + "cmp/" + "cmp_splitDocument1_" + i.ToString() + ".pdf", destinationFolder
                    , "diff_"));
            }
        }

        private sealed class _PdfSplitter_85 : PdfSplitter {
            public _PdfSplitter_85(PdfDocument baseArg1)
                : base(baseArg1) {
                this.partNumber = 1;
            }

            internal int partNumber;

            protected internal override PdfWriter GetNextPdfWriter(PageRange documentPageRange) {
                try {
                    return new PdfWriter(PdfSplitterTest.destinationFolder + "splitDocument1_" + (this.partNumber++).ToString(
                        ) + ".pdf");
                }
                catch (FileNotFoundException) {
                    throw new Exception();
                }
            }
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.SOURCE_DOCUMENT_HAS_ACROFORM_DICTIONARY, Count = 3)]
        public virtual void SplitDocumentTest02() {
            String inputFileName = sourceFolder + "iphone_user_guide.pdf";
            PdfDocument inputPdfDoc = new PdfDocument(new PdfReader(inputFileName));
            new _PdfSplitter_115(inputPdfDoc).SplitByPageCount(60, new _IDocumentReadyListener_126());
            for (int i = 1; i <= 3; i++) {
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "splitDocument2_" + i
                    .ToString() + ".pdf", sourceFolder + "cmp/" + "cmp_splitDocument2_" + i.ToString() + ".pdf", destinationFolder
                    , "diff_"));
            }
        }

        private sealed class _PdfSplitter_115 : PdfSplitter {
            public _PdfSplitter_115(PdfDocument baseArg1)
                : base(baseArg1) {
                this.partNumber = 1;
            }

            internal int partNumber;

            protected internal override PdfWriter GetNextPdfWriter(PageRange documentPageRange) {
                try {
                    return new PdfWriter(PdfSplitterTest.destinationFolder + "splitDocument2_" + (this.partNumber++).ToString(
                        ) + ".pdf");
                }
                catch (FileNotFoundException) {
                    throw new Exception();
                }
            }
        }

        private sealed class _IDocumentReadyListener_126 : PdfSplitter.IDocumentReadyListener {
            public _IDocumentReadyListener_126() {
            }

            public void DocumentReady(PdfDocument pdfDocument, PageRange pageRange) {
                if (new PageRange("61-120").Equals(pageRange)) {
                    pdfDocument.GetDocumentInfo().SetAuthor("Modified Author");
                }
                pdfDocument.Close();
            }
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.SOURCE_DOCUMENT_HAS_ACROFORM_DICTIONARY, Count = 2)]
        public virtual void SplitDocumentTest03() {
            String inputFileName = sourceFolder + "iphone_user_guide.pdf";
            PdfDocument inputPdfDoc = new PdfDocument(new PdfReader(inputFileName));
            PageRange pageRange1 = new PageRange().AddPageSequence(4, 15).AddSinglePage(18).AddPageSequence(1, 2);
            PageRange pageRange2 = new PageRange().AddSinglePage(99).AddSinglePage(98).AddPageSequence(70, 99);
            IList<PdfDocument> splitDocuments = new _PdfSplitter_154(inputPdfDoc).ExtractPageRanges(JavaUtil.ArraysAsList
                (pageRange1, pageRange2));
            foreach (PdfDocument pdfDocument in splitDocuments) {
                pdfDocument.Close();
            }
            for (int i = 1; i <= 2; i++) {
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "splitDocument3_" + i
                     + ".pdf", sourceFolder + "cmp/" + "cmp_splitDocument3_" + i.ToString() + ".pdf", destinationFolder, "diff_"
                    ));
            }
        }

        private sealed class _PdfSplitter_154 : PdfSplitter {
            public _PdfSplitter_154(PdfDocument baseArg1)
                : base(baseArg1) {
                this.partNumber = 1;
            }

            internal int partNumber;

            protected internal override PdfWriter GetNextPdfWriter(PageRange documentPageRange) {
                try {
                    return new PdfWriter(PdfSplitterTest.destinationFolder + "splitDocument3_" + (this.partNumber++).ToString(
                        ) + ".pdf");
                }
                catch (FileNotFoundException) {
                    throw new Exception();
                }
            }
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.SOURCE_DOCUMENT_HAS_ACROFORM_DICTIONARY, Count = 2)]
        public virtual void SplitDocumentTest04() {
            String inputFileName = sourceFolder + "iphone_user_guide.pdf";
            PdfDocument inputPdfDoc = new PdfDocument(new PdfReader(inputFileName));
            PageRange pageRange1 = new PageRange("even & 80-").AddPageSequence(4, 15).AddSinglePage(18).AddPageSequence
                (1, 2);
            PageRange pageRange2 = new PageRange("99,98").AddPageSequence(70, 99);
            IList<PdfDocument> splitDocuments = new _PdfSplitter_188(inputPdfDoc).ExtractPageRanges(JavaUtil.ArraysAsList
                (pageRange1, pageRange2));
            foreach (PdfDocument pdfDocument in splitDocuments) {
                pdfDocument.Close();
            }
            for (int i = 1; i <= 2; i++) {
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "splitDocument4_" + i
                     + ".pdf", sourceFolder + "cmp/" + "cmp_splitDocument4_" + i.ToString() + ".pdf", destinationFolder, "diff_"
                    ));
            }
        }

        private sealed class _PdfSplitter_188 : PdfSplitter {
            public _PdfSplitter_188(PdfDocument baseArg1)
                : base(baseArg1) {
                this.partNumber = 1;
            }

            internal int partNumber;

            protected internal override PdfWriter GetNextPdfWriter(PageRange documentPageRange) {
                try {
                    return new PdfWriter(PdfSplitterTest.destinationFolder + "splitDocument4_" + (this.partNumber++).ToString(
                        ) + ".pdf");
                }
                catch (FileNotFoundException) {
                    throw new Exception();
                }
            }
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.SOURCE_DOCUMENT_HAS_ACROFORM_DICTIONARY, Count = 2)]
        public virtual void SplitDocumentByOutlineTest() {
            String inputFileName = sourceFolder + "iphone_user_guide.pdf";
            PdfDocument inputPdfDoc = new PdfDocument(new PdfReader(inputFileName));
            PdfSplitter splitter = new PdfSplitter(inputPdfDoc);
            IList<String> listTitles = new List<String>();
            listTitles.Add("Syncing iPod Content from Your iTunes Library");
            listTitles.Add("Restoring or Transferring Your iPhone Settings");
            IList<PdfDocument> list = splitter.SplitByOutlines(listTitles);
            NUnit.Framework.Assert.AreEqual(1, list[0].GetNumberOfPages());
            NUnit.Framework.Assert.AreEqual(2, list[1].GetNumberOfPages());
            list[0].Close();
            list[1].Close();
        }

        [NUnit.Framework.Test]
        public virtual void SplitDocumentBySize() {
            String inputFileName = sourceFolder + "splitBySize.pdf";
            PdfDocument inputPdfDoc = new PdfDocument(new PdfReader(inputFileName));
            PdfSplitter splitter = new _PdfSplitter_234(inputPdfDoc);
            IList<PdfDocument> documents = splitter.SplitBySize(100000);
            foreach (PdfDocument doc in documents) {
                doc.Close();
            }
            for (int i = 1; i <= 4; ++i) {
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "splitBySize_part" + 
                    i + ".pdf", sourceFolder + "cmp/" + "cmp_splitBySize_part" + i + ".pdf", destinationFolder, "diff_"));
            }
        }

        private sealed class _PdfSplitter_234 : PdfSplitter {
            public _PdfSplitter_234(PdfDocument baseArg1)
                : base(baseArg1) {
                this.partNumber = 1;
            }

            internal int partNumber;

            protected internal override PdfWriter GetNextPdfWriter(PageRange documentPageRange) {
                try {
                    return new PdfWriter(PdfSplitterTest.destinationFolder + "splitBySize_part" + (this.partNumber++).ToString
                        () + ".pdf");
                }
                catch (FileNotFoundException) {
                    throw new Exception();
                }
            }
        }
    }
}
