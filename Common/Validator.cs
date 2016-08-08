using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    static public class Validator
    {
        const string REGEX_ALPHANUMERIC_ONLY = "^[0-9a-zA-Z]+$";
        const string REGEX_NOT_ALPHANUMERIC = "[^0-9a-zA-Z]";
        const string REGEX_NUMERIC_ONLY = "^[0-9]+$";
        const string REGEX_NOT_NUMERIC = "[^0-9]";

        /// <summary>
        /// 文字列が英数字のみで構成されているか検証します。
        /// 英数字以外の文字列が含まれている場合は、削除した上で検証後の文字列として返却します。
        /// </summary>
        /// <param name="input">検証値</param>
        /// <param name="output">検証済みの値</param>
        /// <returns>英数字のみで構成されている場合は、true。それ以外は、false</returns>
        static public bool IsAlphanumeric(string input, out string output)
        {
            bool result = false;

            result = new Regex(REGEX_ALPHANUMERIC_ONLY).IsMatch(input);
            output = Regex.Replace(input, REGEX_NOT_ALPHANUMERIC, "");

            return result;
        }

        /// <summary>
        /// 文字列が英数字のみで、かつ最大長以下であるかを検証します。
        /// 最大長以上の文字列である場合は最後の文字を削除します。
        /// 英数字以外の文字列が含まれている場合は、削除した上で検証後の文字列として返却します。
        /// </summary>
        /// <param name="input">検証値</param>
        /// <param name="maxLength">最大長</param>
        /// <param name="output">検証済みの値</param>
        /// <returns>文字列が英数字のみで、かつ最大長以下である場合は、true。それ以外は、false</returns>
        static public bool IsAlphanumericWithMaxLength(string input, int maxLength, out string output)
        {
            bool isOverflow = (input.Length > maxLength);

            return IsAlphanumeric(isOverflow ? input.Substring(0, maxLength) : input, out output)
                && ! isOverflow;
        }

        /// <summary>
        /// 文字列が数字のみで構成されているか検証します。
        /// 数字以外の文字列が含まれている場合は、削除した上で検証後の文字列として返却します。
        /// </summary>
        /// <param name="input">検証値</param>
        /// <param name="output">検証済みの値</param>
        /// <returns>数字のみで構成されている場合は、true。それ以外は、false</returns>
        static public bool IsNumeric(string input, out string output)
        {
            bool result = false;

            result = new Regex(REGEX_NUMERIC_ONLY).IsMatch(input);
            output = Regex.Replace(input, REGEX_NOT_NUMERIC, "");

            return result;
        }

        /// <summary>
        /// 文字列が数字のみで、かつ最大長以下であるかを検証します。
        /// 最大長以上の文字列である場合は最後の文字を削除します。
        /// 数字以外の文字列が含まれている場合は、削除した上で検証後の文字列として返却します。
        /// </summary>
        /// <param name="input">検証値</param>
        /// <param name="output">検証済みの値</param>
        /// <returns>文字列が数字のみで、かつ最大長以下である場合は、true。それ以外は、false</returns>
        static public bool IsNumericWithMaxLength(string input, int maxLength, out string output)
        {
            bool isOverflow = (input.Length > maxLength);

            return IsNumeric(isOverflow ? input.Substring(0, maxLength) : input, out output)
                && ! isOverflow;
        }

    }
}
