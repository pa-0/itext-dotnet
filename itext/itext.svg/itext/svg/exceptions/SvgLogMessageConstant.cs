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

namespace iText.Svg.Exceptions {
    /// <summary>Class that holds the logging and exception messages.</summary>
    public sealed class SvgLogMessageConstant {
        public const String ARC_TO_EXPECTS_FOLLOWING_PARAMETERS_GOT_0 = "(rx ry rot largearc sweep x y)+ parameters are expected for elliptical arcs. Got: {0}";

        public const String ATTRIBUTES_NULL = "The attributes of this element are null.";

        public const String COORDINATE_VALUE_ABSENT = "The coordinate value is empty or null.";

        public const String COULDNOTINSTANTIATE = "Could not instantiate Renderer for tag {0}";

        public const String CUSTOM_ABSTRACT_CSS_CONTEXT_NOT_SUPPORTED = "Custom AbstractCssContext implementations are not supported yet";

        public const String DRAW_NO_DRAW = "Can't draw current SvgNodeRenderer.";

        [Obsolete]
        public const String ERROR_CLOSING_CSS_STREAM = "An error occured when trying to close the InputStream of the default CSS.";

        public const String ERROR_INITIALIZING_DEFAULT_CSS = "Error loading the default CSS. Initializing an empty style sheet.";

        public const String FAILED_TO_PARSE_INPUTSTREAM = "Failed to parse InputStream.";

        [Obsolete]
        public const String FLOAT_PARSING_NAN = "The passed value is not a number.";

        public const String FONT_NOT_FOUND = "The font wasn't found.";

        /// <summary>Message in case the font provider doesn't know about any fonts.</summary>
        [Obsolete]
        public const String FONT_PROVIDER_CONTAINS_ZERO_FONTS = "Font Provider contains zero fonts. At least one font shall be present";

        public const String GRADIENT_INVALID_GRADIENT_UNITS_LOG = "Could not recognize gradient units value {0}";

        public const String GRADIENT_INVALID_SPREAD_METHOD_LOG = "Could not recognize gradient spread method value {0}";

        public const String INODEROOTISNULL = "Input root value is null";

        public const String INVALID_CLOSEPATH_OPERATOR_USE = "The close path operator (Z) may not be used before a move to operation (M)";

        public const String INVALID_PATH_D_ATTRIBUTE_OPERATORS = "Invalid operators found in path data attribute: {0}";

        public const String INVALID_TRANSFORM_DECLARATION = "Transformation declaration is not formed correctly.";

        public const String LOOP = "Loop detected";

        public const String MARKER_HEIGHT_IS_NEGATIVE_VALUE = "markerHeight has negative value. Marker will not be rendered.";

        public const String MARKER_HEIGHT_IS_ZERO_VALUE = "markerHeight has zero value. Marker will not be rendered.";

        public const String MARKER_WIDTH_IS_NEGATIVE_VALUE = "markerWidth has negative value. Marker will not be rendered.";

        public const String MARKER_WIDTH_IS_ZERO_VALUE = "markerWidth has zero value. Marker will not be rendered.";

        public const String MISSING_WIDTH = "Top Svg tag has no defined width attribute and viewbox width is not present, so browser default of 300px is used";

        public const String MISSING_HEIGHT = "Top Svg tag has no defined height attribute and viewbox height is not present, so browser default of 150px is used";

        public const String NAMED_OBJECT_NAME_NULL_OR_EMPTY = "The name of the named object can't be null or empty.";

        public const String NAMED_OBJECT_NULL = "A named object can't be null.";

        public const String NONINVERTIBLE_TRANSFORMATION_MATRIX_USED_IN_CLIP_PATH = "Non-invertible transformation matrix was used in a clipping path context. Clipped elements may show undefined behavior.";

        public const String NOROOT = "No root found";

        public const String PATTERN_INVALID_PATTERN_UNITS_LOG = "Could not recognize patternUnits value {0}";

        public const String PATTERN_INVALID_PATTERN_CONTENT_UNITS_LOG = "Could not recognize patternContentUnits value {0}";

        public const String PATTERN_WIDTH_OR_HEIGHT_IS_ZERO = "Pattern width or height is zero. This pattern will not be rendered.";

        public const String PATTERN_WIDTH_OR_HEIGHT_IS_NEGATIVE = "Pattern width or height is negative value. This pattern will not be rendered.";

        public const String PATH_WRONG_NUMBER_OF_ARGUMENTS = "Path operator {0} has received {1} arguments, but expects between {2} and {3} arguments. \n Resulting SVG will be incorrect.";

        public const String PARAMETER_CANNOT_BE_NULL = "Parameters for this method cannot be null.";

        public const String POINTS_ATTRIBUTE_INVALID_LIST = "Points attribute {0} on polyline tag does not contain a valid set of points";

        public const String ROOT_SVG_NO_BBOX = "The root svg tag needs to have a bounding box defined.";

        public const String TAGPARAMETERNULL = "Tag parameter must not be null";

        public const String TRANSFORM_EMPTY = "The transformation value is empty.";

        public const String TRANSFORM_INCORRECT_NUMBER_OF_VALUES = "Transformation doesn't contain the right number of values.";

        [Obsolete]
        public const String TRANSFORM_INCORRECT_VALUE_TYPE = "The transformation value is not a number.";

        public const String TRANSFORM_NULL = "The transformation value is null.";

        public const String UNABLE_TO_GET_INVERSE_MATRIX_DUE_TO_ZERO_DETERMINANT = "Unable to get inverse transformation matrix and thus calculate a viewport for the element because some of the transformation matrices, which are written to document, have a determinant of zero value. A bbox of zero values will be used as a viewport for this element.";

        public const String UNABLE_TO_RETRIEVE_STREAM_WITH_GIVEN_BASE_URI = "Unable to retrieve stream with given base URI ({0}) and source path ({1})";

        public const String UNABLE_TO_RETRIEVE_FONT = "Unable to retrieve font:\n {0}";

        public const String UNMAPPEDTAG = "Could not find implementation for tag {0}";

        public const String UNKNOWN_TRANSFORMATION_TYPE = "Unsupported type of transformation.";

        public const String VIEWBOX_VALUE_MUST_BE_FOUR_NUMBERS = "The viewBox value must be 4 numbers. This viewBox=\"{0}\" will not be processed.";

        public const String VIEWBOX_WIDTH_AND_HEIGHT_CANNOT_BE_NEGATIVE = "The viewBox width and height cannot be negative. This viewBox=\"{0}\" will not be processed.";

        public const String VIEWBOX_WIDTH_OR_HEIGHT_IS_ZERO = "The viewBox width or height is zero. The element with this viewBox will not be rendered.";

        private SvgLogMessageConstant() {
        }
    }
}
