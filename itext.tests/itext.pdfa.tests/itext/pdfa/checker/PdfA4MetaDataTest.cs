using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.XMP;
using iText.Pdfa;
using iText.Pdfa.Exceptions;
using iText.Test;

namespace iText.Pdfa.Checker {
    [NUnit.Framework.Category("IntegrationTest")]
    public class PdfA4MetaDataTest : ExtendedITextTest {
        private static readonly String DESTINATION_FOLDER = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/pdfa/PdfA4ActionCheckTest/";

        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfa/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(DESTINATION_FOLDER);
        }

        [NUnit.Framework.Test]
        public virtual void PdfA4DocumentShallContainMetaDataKey() {
            PdfDictionary dictionary = new PdfDictionary();
            PdfA4Checker checker = new PdfA4Checker(PdfAConformanceLevel.PDF_A_4);
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(PdfAConformanceLevel.PDF_A_4).CheckMetaData(dictionary);
            }
            );
            NUnit.Framework.Assert.AreEqual(e.Message, PdfAConformanceException.A_CATALOG_DICTIONARY_SHALL_CONTAIN_METADATA_ENTRY
                );
        }

        [NUnit.Framework.Test]
        public virtual void PdfA4DocumentMetaDataDocumentShallNotContainBytes() {
            String startHeader = "<?xpacket begin=\"\" id=\"W5M0MpCehiHzreSzNTczkc9d\" bytes=\"1234567890\"?>\n";
            byte[] bytes = startHeader.GetBytes();
            PdfA4Checker checker = new PdfA4Checker(PdfAConformanceLevel.PDF_A_4);
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.Metadata, new PdfStream(bytes));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                checker.CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_PACKET_MAY_NOT_CONTAIN_BYTES_OR_ENCODING_ATTRIBUTE
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void PdfA4DocumentMetaDataDocumentShallNotContainEncoding() {
            String startHeader = "<?xpacket begin=\"\" id=\"W5M0MpCehiHzreSzNTczkc9d\" encoding=\"UTF-8\"?>\n";
            byte[] bytes = startHeader.GetBytes();
            PdfA4Checker checker = new PdfA4Checker(PdfAConformanceLevel.PDF_A_4);
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.Metadata, new PdfStream(bytes));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                checker.CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_PACKET_MAY_NOT_CONTAIN_BYTES_OR_ENCODING_ATTRIBUTE
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void PdfA4DocumentMetaDataDocumentShallNotContainEncodingInAnyPacket() {
            String startHeader = "<?xpacket begin=\"\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?>\n";
            startHeader += "<?xpacket begin=\"\" id=\"W5M0MpCehiHzreSzNTczkc9d\" encoding=\"UTF-8\"?>\n";
            byte[] bytes = startHeader.GetBytes();
            PdfA4Checker checker = new PdfA4Checker(PdfAConformanceLevel.PDF_A_4);
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.Metadata, new PdfStream(bytes));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                checker.CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_PACKET_MAY_NOT_CONTAIN_BYTES_OR_ENCODING_ATTRIBUTE
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void PdfA4DocumentMetaDataDocumentShallNotThrowInAnyPacket() {
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(SOURCE_FOLDER + "xmp/xmpWithMultipleXmpHeaders.xmp"
                ));
            PdfA4Checker checker = new PdfA4Checker(PdfAConformanceLevel.PDF_A_4);
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.Metadata, new PdfStream(bytes));
            NUnit.Framework.Assert.DoesNotThrow(() => {
                checker.CheckMetaData(catalog);
            }
            );
        }

        [NUnit.Framework.Test]
        public virtual void PdfA4DocumentMetaDataDocumentShallThrowInSecondPacket() {
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(SOURCE_FOLDER + "xmp/xmpWithMultipleXmpHeadersWithEnconding.xmp"
                ));
            PdfA4Checker checker = new PdfA4Checker(PdfAConformanceLevel.PDF_A_4);
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.Metadata, new PdfStream(bytes));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                checker.CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_PACKET_MAY_NOT_CONTAIN_BYTES_OR_ENCODING_ATTRIBUTE
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestAbsentPartPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testAbsentPartPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                xmpMeta.DeleteProperty(XMPConst.NS_PDFA_ID, XMPConst.PART);
            }
            ));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_PART
                , "4"), e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestInvalidPartPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testInvalidPartPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.PART, "1");
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_PART
                , "4"), e_1.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestNullPartPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testNullPartPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.PART, null);
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_PART
                , "4"), e_1.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestAbsentRevisionPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testNullRevisionPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                xmpMeta.DeleteProperty(XMPConst.NS_PDFA_ID, XMPConst.REV);
            }
            ));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_REV
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestInvalidValueNotNumberRevisionPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testInvalidValueNotNumberRevisionPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.REV, "test");
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_REV
                , e_1.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestInvalidValueNotLength4RevisionPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testInvalidValueNotLength4RevisionPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.REV, "200");
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_REV
                , e_1.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestInvalidValueLength4ButContainsLettersRevisionPropertyPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testInvalidValueLength4ButContainsLettersRevisionPropertyPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.REV, "200A");
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_REV
                , e_1.Message);
        }

        [NUnit.Framework.Test]
        public virtual void TestValidPropertiesPDFA4() {
            String outPdf = DESTINATION_FOLDER + "testValidPropertiesPDFA4.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
            }
            ));
            NUnit.Framework.Assert.DoesNotThrow(() => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
        }

        [NUnit.Framework.Test]
        public virtual void TestValidPropertiesPDFA4F() {
            String outPdf = DESTINATION_FOLDER + "testValidPropertiesPDFA4F.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4F;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
                PdfFileSpec fs = PdfFileSpec.CreateEmbeddedFileSpec(doc, "file".GetBytes(), "description", "file.txt", null
                    , null, null);
                doc.AddFileAttachment("file.txt", fs);
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
            }
            ));
            NUnit.Framework.Assert.DoesNotThrow(() => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
        }

        [NUnit.Framework.Test]
        public virtual void TestValidPropertiesPDFA4E() {
            String outPdf = DESTINATION_FOLDER + "testValidPropertiesPDFA4E.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4E;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
            }
            ));
            NUnit.Framework.Assert.DoesNotThrow(() => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
        }

        [NUnit.Framework.Test]
        public virtual void TestAbsentConformancePropertyPDFA4F() {
            String outPdf = DESTINATION_FOLDER + "testAbsentConformancePropertyPDFA4F.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4F;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
                PdfFileSpec fs = PdfFileSpec.CreateEmbeddedFileSpec(doc, "file".GetBytes(), "description", "file.txt", null
                    , null, null);
                doc.AddFileAttachment("file.txt", fs);
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                xmpMeta.DeleteProperty(XMPConst.NS_PDFA_ID, XMPConst.CONFORMANCE);
            }
            ));
            NUnit.Framework.Assert.DoesNotThrow(() => new PdfA4Checker(conformanceLevel).CheckMetaData(catalog));
        }

        [NUnit.Framework.Test]
        public virtual void TestInvalidConformancePropertyPDFA4F() {
            String outPdf = DESTINATION_FOLDER + "testInvalidConformancePropertyPDFA4F.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4F;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
                PdfFileSpec fs = PdfFileSpec.CreateEmbeddedFileSpec(doc, "file".GetBytes(), "description", "file.txt", null
                    , null, null);
                doc.AddFileAttachment("file.txt", fs);
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.CONFORMANCE, "1");
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_CONFORMANCE
                , e_1.Message);
        }

        [NUnit.Framework.Test]
        public virtual void HistoryWithXmpMetaData() {
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(SOURCE_FOLDER + "xmp/xmpWithCorrectHistory.xmp"));
            XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            PdfDictionary catalog = new PdfDictionary();
            MemoryStream baos = new MemoryStream();
            XMPMetaFactory.Serialize(xmpMeta, baos);
            catalog.Put(PdfName.Metadata, new PdfStream(baos.ToArray()));
            NUnit.Framework.Assert.DoesNotThrow(() => new PdfA4Checker(conformanceLevel).CheckMetaData(catalog));
        }

        [NUnit.Framework.Test]
        public virtual void HistoryWithInvalidWhenXmpMetaData() {
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(SOURCE_FOLDER + "xmp/xmpWithInvalidWhen.xmp"));
            XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            PdfDictionary catalog = new PdfDictionary();
            MemoryStream baos = new MemoryStream();
            XMPMetaFactory.Serialize(xmpMeta, baos);
            catalog.Put(PdfName.Metadata, new PdfStream(baos.ToArray()));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => new PdfA4Checker(conformanceLevel
                ).CheckMetaData(catalog));
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(PdfaExceptionMessageConstant.XMP_METADATA_HISTORY_ENTRY_SHALL_CONTAIN_KEY
                , "stEvt:when"), e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void HistoryWithInvalidActionXmpMetaData() {
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(SOURCE_FOLDER + "xmp/xmpWithInvalidAction.xmp"));
            XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            PdfDictionary catalog = new PdfDictionary();
            MemoryStream baos = new MemoryStream();
            XMPMetaFactory.Serialize(xmpMeta, baos);
            catalog.Put(PdfName.Metadata, new PdfStream(baos.ToArray()));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => new PdfA4Checker(conformanceLevel
                ).CheckMetaData(catalog));
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(PdfaExceptionMessageConstant.XMP_METADATA_HISTORY_ENTRY_SHALL_CONTAIN_KEY
                , "stEvt:action"), e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void HistoryWithEmptyEntryXmpMetaData() {
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4;
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(SOURCE_FOLDER + "xmp/xmpWithEmpty.xmp"));
            XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            PdfDictionary catalog = new PdfDictionary();
            MemoryStream baos = new MemoryStream();
            XMPMetaFactory.Serialize(xmpMeta, baos);
            catalog.Put(PdfName.Metadata, new PdfStream(baos.ToArray()));
            new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            NUnit.Framework.Assert.IsTrue(true);
        }

        [NUnit.Framework.Test]
        public virtual void TestNullConformancePropertyPDFA4F() {
            String outPdf = DESTINATION_FOLDER + "testNullConformancePropertyPDFA4F.pdf";
            PdfAConformanceLevel conformanceLevel = PdfAConformanceLevel.PDF_A_4F;
            GeneratePdfADocument(conformanceLevel, outPdf, (doc) => {
                PdfFileSpec fs = PdfFileSpec.CreateEmbeddedFileSpec(doc, "file".GetBytes(), "description", "file.txt", null
                    , null, null);
                doc.AddFileAttachment("file.txt", fs);
            }
            );
            PdfADocument pdfADocument = new PdfADocument(new PdfReader(outPdf), new PdfWriter(new MemoryStream()));
            PdfDictionary catalog = GenerateCustomXmpCatalog(pdfADocument, ((xmpMeta) => {
                try {
                    xmpMeta.SetProperty(XMPConst.NS_PDFA_ID, XMPConst.CONFORMANCE, null);
                }
                catch (XMPException e) {
                    throw new PdfException(e);
                }
            }
            ));
            Exception e_1 = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => {
                new PdfA4Checker(conformanceLevel).CheckMetaData(catalog);
            }
            );
            NUnit.Framework.Assert.AreEqual(PdfaExceptionMessageConstant.XMP_METADATA_HEADER_SHALL_CONTAIN_VERSION_IDENTIFIER_CONFORMANCE
                , e_1.Message);
        }

        private void GeneratePdfADocument(PdfAConformanceLevel conformanceLevel, String outPdf, Action<PdfDocument
            > consumer) {
            if (outPdf == null) {
                NUnit.Framework.Assert.Fail();
            }
            PdfWriter writer = new PdfWriter(outPdf, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfADocument doc = new PdfADocument(writer, conformanceLevel, new PdfOutputIntent("Custom", "", "http://www.color.org"
                , "sRGB IEC61966-2.1", new FileStream(SOURCE_FOLDER + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read
                )));
            doc.AddNewPage();
            consumer(doc);
            doc.Close();
        }

        private static PdfDictionary GenerateCustomXmpCatalog(PdfADocument pdfADocument, Action<XMPMeta> action) {
            XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(pdfADocument.GetXmpMetadata()));
            PdfDictionary catalog = pdfADocument.GetCatalog().GetPdfObject();
            MemoryStream baos = new MemoryStream();
            action(xmpMeta);
            XMPMetaFactory.Serialize(xmpMeta, baos);
            catalog.Put(PdfName.Metadata, new PdfStream(baos.ToArray()));
            return catalog;
        }
    }
}
