using System.Text.RegularExpressions;

namespace DevJobs.Infrastructure.Utilities
{
    public static class BanglaInteger
    {
        public static int ParseSafe(this string s)
        {
            s = s.Replace(",", "").Trim();

            if (!string.IsNullOrEmpty(s) && s.All(char.IsDigit))
            {
                return int.Parse(s.Trim());
            }
            else
            {
                int place = 1;
                int sum = 0;
                foreach (var digit in GetDigits(s))
                {
                    sum = sum + place * digit;
                    place *= 10;
                }
                return sum;
            } 
        }

        private static List<int> GetDigits(string number)
        {
            Regex regex = new Regex(@"\d{4}?");
            List<int> digits = new List<int>();

            foreach (var match in regex.Matches(number))
            {
                digits.Add(int.Parse(match.ToString()) - 2534);
            }

            digits.Reverse();
            return digits;
        }
    }
}
