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

namespace iText.IO.Util {
    /// <summary>
    /// This file is a helper class for internal usage only.
    /// Be aware that its API and functionality may be changed in future.
    /// </summary>
    public static class DateTimeUtil {
        public static double GetUtcMillisFromEpoch(DateTime? dateTime) {
            if (dateTime == null) {
                dateTime = DateTime.Now;
            }
            return ((DateTime) dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static DateTime GetCurrentTime() {
            return DateTime.Now;
        }

        public static DateTime GetCurrentUtcTime() {
            return DateTime.UtcNow;
        }

        public static DateTime ParseSimpleFormat(String date, String format) {
            return DateTime.ParseExact(date, format, null);
        }
    }
}
