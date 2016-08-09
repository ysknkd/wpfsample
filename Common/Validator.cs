using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    }
}
