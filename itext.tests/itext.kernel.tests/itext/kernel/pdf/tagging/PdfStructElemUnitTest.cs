/*
This file is part of the iText (R) project.
Copyright (c) 1998-2025 Apryse Group NV
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
using iText.IO.Source;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Test;

namespace iText.Kernel.Pdf.Tagging {
    [NUnit.Framework.Category("UnitTest")]
    public class PdfStructElemUnitTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void NoParentObjectTest() {
            PdfDictionary parent = new PdfDictionary();
            PdfArray kid = new PdfArray();
            Exception exception = NUnit.Framework.Assert.Catch(typeof(PdfException), () => PdfStructElem.AddKidObject(
                parent, 1, kid));
            NUnit.Framework.Assert.AreEqual(KernelExceptionMessageConstant.STRUCTURE_ELEMENT_SHALL_CONTAIN_PARENT_OBJECT
                , exception.Message);
        }

        [NUnit.Framework.Test]
        public virtual void AnnotationHasNoReferenceToPageTest() {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            PdfName pdfName = new PdfName("test");
            PdfAnnotation annotation = new Pdf3DAnnotation(new Rectangle(100, 100), pdfName);
            Exception exception = NUnit.Framework.Assert.Catch(typeof(PdfException), () => new PdfStructElem(pdfDoc, pdfName
                , annotation));
            NUnit.Framework.Assert.AreEqual(KernelExceptionMessageConstant.ANNOTATION_SHALL_HAVE_REFERENCE_TO_PAGE, exception
                .Message);
        }

        [NUnit.Framework.Test]
        public virtual void AttributesAreNullTest() {
            IDictionary<PdfName, PdfObject> attributesMap = new Dictionary<PdfName, PdfObject>();
            PdfDictionary dictionary = new PdfDictionary(attributesMap);
            PdfStructElem pdfStructElem = new PdfStructElem(dictionary);
            IList<PdfStructureAttributes> actualAttributesList = pdfStructElem.GetAttributesList();
            IList<PdfStructureAttributes> expectedAttributesList = new List<PdfStructureAttributes>();
            NUnit.Framework.Assert.AreEqual(actualAttributesList, expectedAttributesList);
        }

        [NUnit.Framework.Test]
        public virtual void AttributesAreDictionaryTest() {
            IDictionary<PdfName, PdfObject> attributesMap = new Dictionary<PdfName, PdfObject>();
            IDictionary<PdfName, PdfObject> dictionaryMap = new Dictionary<PdfName, PdfObject>();
            dictionaryMap.Put(PdfName.A, new PdfName("value"));
            attributesMap.Put(PdfName.A, new PdfDictionary(dictionaryMap));
            PdfDictionary dictionary = new PdfDictionary(attributesMap);
            PdfStructElem pdfStructElem = new PdfStructElem(dictionary);
            IList<PdfStructureAttributes> actualAttributesList = pdfStructElem.GetAttributesList();
            IList<PdfStructureAttributes> expectedAttributesList = new List<PdfStructureAttributes>();
            expectedAttributesList.Add(new PdfStructureAttributes(new PdfDictionary(dictionaryMap)));
            NUnit.Framework.Assert.AreEqual(actualAttributesList[0].GetPdfObject().Get(PdfName.A), expectedAttributesList
                [0].GetPdfObject().Get(PdfName.A));
        }

        [NUnit.Framework.Test]
        public virtual void AttributesAreArrayTest() {
            IDictionary<PdfName, PdfObject> attributesMap = new Dictionary<PdfName, PdfObject>();
            IDictionary<PdfName, PdfObject> dictionaryMap = new Dictionary<PdfName, PdfObject>();
            dictionaryMap.Put(PdfName.A, new PdfName("value"));
            PdfDictionary pdfDictionary = new PdfDictionary(dictionaryMap);
            attributesMap.Put(PdfName.A, new PdfArray(pdfDictionary));
            PdfDictionary dictionary = new PdfDictionary(attributesMap);
            PdfStructElem pdfStructElem = new PdfStructElem(dictionary);
            IList<PdfStructureAttributes> actualAttributesList = pdfStructElem.GetAttributesList();
            IList<PdfStructureAttributes> expectedAttributesList = new List<PdfStructureAttributes>();
            expectedAttributesList.Add(new PdfStructureAttributes(new PdfDictionary(dictionaryMap)));
            NUnit.Framework.Assert.AreEqual(actualAttributesList[0].GetPdfObject().Get(PdfName.A), expectedAttributesList
                [0].GetPdfObject().Get(PdfName.A));
        }
    }
}
