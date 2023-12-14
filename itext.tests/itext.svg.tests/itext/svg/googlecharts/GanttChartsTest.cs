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
using iText.Kernel.Geom;
using iText.Kernel.Utils;
using iText.Svg.Renderers;
using iText.Svg.Utils;
using iText.Test;

namespace iText.Svg.Googlecharts {
    public class GanttChartsTest : SvgIntegrationTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/svg/googlecharts/GanttChartsTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/svg/googlecharts/GanttChartsTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            ITextTest.CreateDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void GanttChart() {
            PageSize pageSize = PageSize.A4;
            TestUtils.ConvertSVGtoPDF(destinationFolder + "ganttChart.pdf", sourceFolder + "ganttChart.svg", 1, pageSize
                );
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "ganttChart.pdf", sourceFolder
                 + "cmp_ganttChart.pdf", destinationFolder, "diff_"));
        }

        [NUnit.Framework.Test]
        public virtual void Gantt2Chart() {
            PageSize pageSize = PageSize.A4;
            TestUtils.ConvertSVGtoPDF(destinationFolder + "gantt2Chart.pdf", sourceFolder + "gantt2Chart.svg", 1, pageSize
                );
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gantt2Chart.pdf", sourceFolder
                 + "cmp_gantt2Chart.pdf", destinationFolder, "diff_"));
        }

        [NUnit.Framework.Test]
        public virtual void Gantt3Chart() {
            PageSize pageSize = PageSize.A4;
            TestUtils.ConvertSVGtoPDF(destinationFolder + "gantt3Chart.pdf", sourceFolder + "gantt3Chart.svg", 1, pageSize
                );
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gantt3Chart.pdf", sourceFolder
                 + "cmp_gantt3Chart.pdf", destinationFolder, "diff_"));
        }

        [NUnit.Framework.Test]
        public virtual void Gantt4Chart() {
            PageSize pageSize = PageSize.A4;
            TestUtils.ConvertSVGtoPDF(destinationFolder + "gantt4Chart.pdf", sourceFolder + "gantt4Chart.svg", 1, pageSize
                );
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gantt4Chart.pdf", sourceFolder
                 + "cmp_gantt4Chart.pdf", destinationFolder, "diff_"));
        }
    }
}
