using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss
{
    public class PasswordScanner
    {
        public enum Strength { VERYWEAK, WEAK, GOOD, STRONG, VERY_STRONG }

        private static readonly Dictionary<Strength, Tuple<int, int>> ranges;

        static PasswordScanner()
        {
            ranges = new Dictionary<Strength, Tuple<int, int>>();
            ranges.Add(Strength.VERYWEAK, new Tuple<int, int>(0, 15));
            ranges.Add(Strength.WEAK, new Tuple<int, int>(16, 30));
            ranges.Add(Strength.GOOD, new Tuple<int, int>(31, 55));
            ranges.Add(Strength.STRONG, new Tuple<int, int>(56, 80));
            ranges.Add(Strength.VERY_STRONG, new Tuple<int, int>(81, 100));
        }

        // Scans password calculates strength and set true values to
        // letters,capitals,special chars,digits, eight chars if it contains.
        public Strength scanPassword(String password)
        {

            if(string.IsNullOrWhiteSpace(password) || password.Length <= 4) return Strength.WEAK;
            var arr = password.ToCharArray();
            HashSet<char> unique = new HashSet<char>(arr);
            int score = unique.Count * 4;
            int cntUpper = arr.Where(c => char.IsUpper(c)).Count();
            if(cntUpper > 0) score += (password.Length - cntUpper) * 2;
            int cntLower = arr.Where(c => char.IsLower(c)).Count();
            if(cntLower > 0) score += (password.Length - cntLower) * 2;
            int res = 0, seq = 0;
            var numbers = arr.Where(c => char.IsNumber(c)).Select((c) => int.Parse(new string(c, 1)));
            if(numbers.Any())
            {
                numbers.Aggregate((prev, next) =>
                {
                    if(Math.Abs(prev - next) > 1)
                    {
                        res += 4;
                        seq = 0;
                    }
                    else
                    {
                        seq++;
                    }
                    if(seq > 3) res -= 4;
                    return next;
                });
            }
            score += res;
            score += arr.Where(c => !char.IsLetterOrDigit(c)).Count() * 6;
            if(score > 100) score = 100;
            return ranges.Where(kv => kv.Value.Item1 <= score && kv.Value.Item2 >= score).Single().Key;
        }
    }
}