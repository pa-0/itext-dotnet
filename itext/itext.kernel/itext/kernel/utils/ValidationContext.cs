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
using System.Collections.Generic;
using iText.Kernel.Font;
using iText.Kernel.Pdf;

namespace iText.Kernel.Utils {
    /// <summary>
    /// This class is used to pass additional information to the
    /// <see cref="IValidationChecker"/>
    /// implementations.
    /// </summary>
    public class ValidationContext {
        private PdfDocument PdfDocument = null;

        private ICollection<PdfFont> fonts = null;

        public ValidationContext() {
        }

        public virtual iText.Kernel.Utils.ValidationContext WithPdfDocument(PdfDocument pdfDocument) {
            this.PdfDocument = pdfDocument;
            return this;
        }

        public virtual iText.Kernel.Utils.ValidationContext WithFonts(ICollection<PdfFont> fonts) {
            this.fonts = fonts;
            return this;
        }

        public virtual PdfDocument GetPdfDocument() {
            return PdfDocument;
        }

        public virtual ICollection<PdfFont> GetFonts() {
            return fonts;
        }
    }
}