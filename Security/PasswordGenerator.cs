using System;
using System.Linq;

namespace PassKeeper.Security
{
    public static class PasswordGenerator
    {
        /// <summary> 
        /// 产生随机密码
        /// </summary> 
        /// <param name="genlen">密码长度</param> 
        /// <returns>产生的随机密码</returns> 
        public static string GeneratePassword(int genlen)
        {
            return GeneratePassword(genlen, true, true, true, true);
        }
        /// <summary> 
        /// 产生随机密码
        /// </summary> 
        /// <param name="genlen">密码长度</param> 
        /// <param name="usenumbers">是否使用数字</param> 
        /// <param name="uselowalphabets">是否使用小写字母</param> 
        /// <param name="usehighalphabets">是否使用大写字母</param> 
        /// <param name="usesymbols">是否用符号</param> 
        /// <returns>产生的随机密码</returns> 
        public static string GeneratePassword(int genlen = 8, bool usenumbers = true, bool uselowalphabets = true, bool usehighalphabets = true, bool usesymbols = true)
        {
            var upperCase = new char[]
                {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
                };
            var lowerCase = new char[]
                {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z'
                };
            var numerals = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var symbols = new char[]
                {
                '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '{', '[', '}', ']', '-', '_', '=', '+', ':',
                ';', '|', '/', '?', ',', '<', '.', '>'
                };
            char[] total = (Array.Empty<char>())
                            .Concat(usehighalphabets ? upperCase : Array.Empty<char>())
                            .Concat(uselowalphabets ? lowerCase : Array.Empty<char>())
                            .Concat(usenumbers ? numerals : Array.Empty<char>())
                            .Concat(usesymbols ? symbols : Array.Empty<char>())
                            .ToArray();
            var rnd = new Random();

            var chars = Enumerable
                .Repeat<int>(0, genlen)
                .Select(i => total[rnd.Next(total.Length)])
                .ToArray();
            return new string(chars);
        }
    }
}
