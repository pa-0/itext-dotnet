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
using iText.Test;

namespace iText.Kernel.Pdf {
    public class PdfPagesTreeTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void NullUnlimitedListAddTest() {
            PdfPagesTree.NullUnlimitedList<String> list = new PdfPagesTree.NullUnlimitedList<String>();
            list.Add("hey");
            list.Add("bye");
            NUnit.Framework.Assert.AreEqual(2, list.Size());
            list.Add(-1, "hello");
            list.Add(3, "goodbye");
            NUnit.Framework.Assert.AreEqual(2, list.Size());
        }

        [NUnit.Framework.Test]
        public virtual void NullUnlimitedListIndexOfTest() {
            PdfPagesTree.NullUnlimitedList<String> list = new PdfPagesTree.NullUnlimitedList<String>();
            list.Add("hey");
            list.Add(null);
            list.Add("bye");
            list.Add(null);
            NUnit.Framework.Assert.AreEqual(4, list.Size());
            NUnit.Framework.Assert.AreEqual(1, list.IndexOf(null));
        }

        [NUnit.Framework.Test]
        public virtual void NullUnlimitedListRemoveTest() {
            PdfPagesTree.NullUnlimitedList<String> list = new PdfPagesTree.NullUnlimitedList<String>();
            list.Add("hey");
            list.Add("bye");
            NUnit.Framework.Assert.AreEqual(2, list.Size());
            list.Remove(-1);
            list.Remove(2);
            NUnit.Framework.Assert.AreEqual(2, list.Size());
        }
    }
}
