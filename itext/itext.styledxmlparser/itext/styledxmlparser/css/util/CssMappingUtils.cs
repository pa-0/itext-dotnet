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
using iText.Layout.Properties;

namespace iText.StyledXmlParser.Css.Util {
    /// <summary>Utilities class for CSS mapping operations.</summary>
    [System.ObsoleteAttribute(@"will be removed in 7.2, use CssBackgroundUtils instead")]
    public sealed class CssMappingUtils {
        /// <summary>
        /// Creates a new
        /// <see cref="CssMappingUtils"/>
        /// instance.
        /// </summary>
        private CssMappingUtils() {
        }

        /// <summary>Parses the background repeat string value.</summary>
        /// <param name="value">the string which stores the background repeat value</param>
        /// <returns>
        /// the background repeat as a
        /// <see cref="iText.Layout.Properties.BackgroundRepeat.BackgroundRepeatValue"/>
        /// instance
        /// </returns>
        [System.ObsoleteAttribute(@"will be removed in 7.2, use CssBackgroundUtils.ParseBackgroundRepeat(System.String) instead"
            )]
        public static BackgroundRepeat.BackgroundRepeatValue ParseBackgroundRepeat(String value) {
            return CssBackgroundUtils.ParseBackgroundRepeat(value);
        }
    }
}
