using System;
using System.Globalization;

namespace ISD.Extensions
{
    public class StringExtension
    {
        public static string RemoveSign4VietnameseString(string str)
        {
            string[] VietnameseSigns = new string[]
            {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
    }
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    //throw new ArgumentNullException(nameof(input));
                    return null;
                case "":
                    //throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                    return "";
                default:
                    var words = input.Trim().Replace("   ", " ").Replace("  ", " ").ToLower().Split(' ');
                    string[] retWords = new string[words.Length];
                    int i = 0;
                    foreach (string word in words)
                    {
                        char[] letters = word.ToCharArray();
                        letters[0] = char.ToUpper(letters[0]);
                        var str = new string(letters);
                        retWords[i] = str;
                        i = i + 1;
                    }
                    return string.Join(" ", retWords);
            }
        }
        //toLowerOtherChar = true: In hoa ký tự đầu tiên của chữ, in thường các ký tự còn lại 
        //toLowerOtherChar = false: In hoa ký tự đầu tiên của chữ
        public static string FirstCharToUpper(this string input, bool toLowerOtherChar = true)
        {
            switch (input)
            {
                case null:
                    //throw new ArgumentNullException(nameof(input));
                    return null;
                case "":
                    return "";
                //throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    if (toLowerOtherChar)
                    {
                        input = input.ToLower();
                    }
                    var words = input.Trim().Replace("   ", " ").Replace("  ", " ").Split(' ');
                    string[] retWords = new string[words.Length];
                    int i = 0;
                    foreach (string word in words)
                    {
                        char[] letters = word.ToCharArray();
                        letters[0] = char.ToUpper(letters[0]);
                        var str = new string(letters);
                        retWords[i] = str;
                        i = i + 1;
                    }
                    return string.Join(" ", retWords);
            }
        }
        public static string ToAbbreviation(this string input)
        {
            string ret = string.Empty;
            string[] strArr = input.FirstCharToUpper().Split();
            foreach (var str in strArr)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    ret = ret + str[0];
                }
            }
            return ret.ToLower();
        }

        /// <summary>
        /// Chuyển tên thành chữ hiển thị trên logo tên
        /// </summary>
        /// <param name="input">Vũ Hoài Nam</param>
        /// <returns>HN</returns>
        public static string GetCharacterForLogoName(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                input = "U";
            }
            string ret = string.Empty;
            string[] strArr = input.FirstCharToUpper().Split();
            foreach (var str in strArr)
            {
                ret = ret + StringExtension.RemoveSign4VietnameseString(str[0].ToString());
            }
            //Get 2 last characters of string
            if (ret.Length >= 2)
            {
                ret = ret.Substring(ret.Length - 2);
            }
            return ret.ToUpper();
        }
    }
}