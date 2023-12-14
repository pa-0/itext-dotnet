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

namespace iText.Kernel.Log {
    /// <summary>
    /// <see cref="ICounterFactory"/>
    /// implementation that always returns counter instance passed to it in constructor
    /// </summary>
    [System.ObsoleteAttribute(@"will be removed in next major release, please use iText.Kernel.Counter.SimpleEventCounterFactory instead."
        )]
    public class SimpleCounterFactory : ICounterFactory {
        private ICounter counter;

        public SimpleCounterFactory(ICounter counter) {
            this.counter = counter;
        }

        public virtual ICounter GetCounter(Type cls) {
            return counter;
        }
    }
}
