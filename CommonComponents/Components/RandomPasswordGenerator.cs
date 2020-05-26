using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordBoss
{
    public class RandomPasswordGenerator
    {
        [Flags]
        enum CharType
        {
            None = 0,
            Number = 1,
            Capital = 2,
            Lowercase = 4,
            Special = 8,
            All = 15
        };

        private static readonly Dictionary<byte, CharType> codes;

        static RandomPasswordGenerator()
        {
            codes = new Dictionary<byte, CharType>();
            for(byte i = 48; i <= 57; i++)
                codes.Add(i, CharType.Number);
            for(byte i = 65; i <= 90; i++)
                codes.Add(i, CharType.Capital);
            for(byte i = 97; i <= 122; i++)
                codes.Add(i, CharType.Lowercase);
            foreach(var b in new byte[] { 33, 35, 36, 37, 38, 42, 43, 45, 47, 61, 64, 94, 95 })
                codes.Add(b, CharType.Special);
        }

        private string GeneratePassword(int length, CharType mask)
        {
            if(length < 4) length = 4;
            if(length > 128) length = 128;
            if(mask == CharType.None) mask = CharType.All;
            CharType mustHave = mask;
            byte[] ba = null;
            var rng = RNGCryptoServiceProvider.Create();
            var pwd = new List<byte>();
            do
            {
                ba = new byte[length * 4];
                rng.GetNonZeroBytes(ba);
                var len = length - pwd.Count;
                if(len <= 0) break;
                if(mustHave != CharType.None)
                {
                    foreach(var b in ba)
                    {
                        if(codes.ContainsKey(b) && (codes[b] & mustHave) != 0)
                        {
                            pwd.Add(b);
                            mustHave ^= codes[b];
                            if(mustHave == CharType.None) break;
                        }
                    }
                }
                else
                {
                    ba = ba.Where(b => codes.ContainsKey(b) && (codes[b] & mask) != 0).Take(len).ToArray();
                    pwd.AddRange(ba);
                }
            } while(pwd.Count < length);
            return Encoding.UTF8.GetString(pwd.ToArray());
        }

        public string generatePswd(int length, bool capitals, bool numbers, bool specChars, bool letters)
        {
            if(length < 4) return null;
            CharType mask = CharType.None;
            if(numbers) mask |= CharType.Number;
            if(capitals) mask |= CharType.Capital;
            if(letters) mask |= CharType.Lowercase;
            if(specChars) mask |= CharType.Special;
            return GeneratePassword(length, mask);
        }
    }
}
