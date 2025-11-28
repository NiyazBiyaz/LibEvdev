// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Evtest.Utils
{
    public static class StringExtensions
    {
        /// <summary>
        /// Apply semantic color on this string.
        /// </summary>
        /// <returns>Markup string with the enabled color value.</returns>
        public static string Dye(this string str, OutputStringType stringType)
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

            return $"[{colorMarkup}]{str}[/]";
        }
    }
}
