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
using iText.Test;

namespace iText.Kernel.Geom {
    public class MatrixTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void TestMultiply() {
            Matrix m1 = new Matrix(2, 3, 4, 5, 6, 7);
            Matrix m2 = new Matrix(8, 9, 10, 11, 12, 13);
            Matrix shouldBe = new Matrix(46, 51, 82, 91, 130, 144);
            Matrix rslt = m1.Multiply(m2);
            NUnit.Framework.Assert.AreEqual(shouldBe, rslt);
        }

        [NUnit.Framework.Test]
        public virtual void TestDeterminant() {
            Matrix m = new Matrix(2, 3, 4, 5, 6, 7);
            NUnit.Framework.Assert.AreEqual(-2f, m.GetDeterminant(), .001f);
        }

        [NUnit.Framework.Test]
        public virtual void TestSubtract() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 5, 6);
            Matrix m2 = new Matrix(6, 5, 4, 3, 2, 1);
            Matrix shouldBe = new Matrix(-5, -3, 0, -1, 1, 0, 3, 5, 0);
            Matrix rslt = m1.Subtract(m2);
            NUnit.Framework.Assert.AreEqual(shouldBe, rslt);
        }

        [NUnit.Framework.Test]
        public virtual void TestAdd() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 5, 6);
            Matrix m2 = new Matrix(6, 5, 4, 3, 2, 1);
            Matrix shouldBe = new Matrix(7, 7, 0, 7, 7, 0, 7, 7, 2);
            Matrix rslt = m1.Add(m2);
            NUnit.Framework.Assert.AreEqual(shouldBe, rslt);
        }
    }
}
