/*
This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: iText Software.

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
using iText.Kernel.Counter;
using iText.Kernel.Counter.Context;

namespace iText.Kernel.Actions {
    /// <summary>Base class for events handling depending on the context.</summary>
    public abstract class AbstractContextBasedEventHandler : IBaseEventHandler {
        private readonly IContext defaultContext;

        /// <summary>
        /// Creates a new instance of the handler with the defined fallback for events within unknown
        /// contexts.
        /// </summary>
        /// <param name="onUnknownContext">is a fallback for events within unknown context</param>
        public AbstractContextBasedEventHandler(IContext onUnknownContext)
            : base() {
            this.defaultContext = onUnknownContext;
        }

        /// <summary>
        /// Performs context validation and if event is allowed to be processed passes it to
        /// <see cref="OnAcceptedEvent(ITextEvent)"/>.
        /// </summary>
        /// <param name="event">to handle</param>
        public void OnEvent(IBaseEvent @event) {
            if (!(@event is ITextEvent)) {
                return;
            }
            ITextEvent iTextEvent = (ITextEvent)@event;
            IContext context = null;
            if (@event is AbstractContextBasedITextEvent) {
                AbstractContextBasedITextEvent iTextProductEvent = (AbstractContextBasedITextEvent)@event;
                if (iTextProductEvent.GetMetaInfo() != null) {
                    context = ContextManager.GetInstance().GetContext(iTextProductEvent.GetMetaInfo().GetType());
                }
                if (context == null) {
                    context = ContextManager.GetInstance().GetContext(@event.GetType());
                }
            }
            if (context == null) {
                context = this.defaultContext;
            }
            if (context.IsAllowed(iTextEvent)) {
                OnAcceptedEvent(iTextEvent);
            }
        }

        /// <summary>Handles the accepted event.</summary>
        /// <param name="event">to handle</param>
        protected internal abstract void OnAcceptedEvent(ITextEvent @event);
    }
}