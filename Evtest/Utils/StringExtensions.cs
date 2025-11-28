// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Evtest.Utils
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Apply semantic color on this object.
        /// </summary>
        /// <returns>Markup string with the enabled color value.</returns>
        public static string Dye(this object obj, OutputStringType stringType)
        {
            string colorMarkup = stringType switch
            {
                OutputStringType.Path => ColorValue.PATH,
                OutputStringType.StringValue => ColorValue.STRING_VALUE,
                OutputStringType.IntegerValue => ColorValue.INTEGER_VALUE,
                OutputStringType.EnumValue => ColorValue.ENUM_VALUE,
                OutputStringType.Attention => ColorValue.ATTENTION,
                OutputStringType.Warning => ColorValue.WARNING,
                OutputStringType.Error => ColorValue.ERROR,
                OutputStringType.Good => ColorValue.GOOD,
                OutputStringType.Dimmed => ColorValue.DIMMED,
                _ => throw new ArgumentOutOfRangeException(nameof(stringType))
            };

            return $"[{colorMarkup}]{obj}[/]";
        }

        public static string DyePath(this object obj) => obj.Dye(OutputStringType.Path);
        public static string DyeStringValue(this object obj) => obj.Dye(OutputStringType.StringValue);
        public static string DyeIntegerValue(this object obj) => obj.Dye(OutputStringType.IntegerValue);
        public static string DyeEnumValue(this object obj) => obj.Dye(OutputStringType.EnumValue);
        public static string DyeAttention(this object obj) => obj.Dye(OutputStringType.Attention);
        public static string DyeWarning(this object obj) => obj.Dye(OutputStringType.Warning);
        public static string DyeError(this object obj) => obj.Dye(OutputStringType.Error);
        public static string DyeGood(this object obj) => obj.Dye(OutputStringType.Good);
        public static string DyeDimmed(this object obj) => obj.Dye(OutputStringType.Dimmed);
    }
}
