using System;
using System.Globalization;

namespace IPV6
{
    public class TotalIPs
    {
        public string IpV6 { get; set; }
        public string Res { get; set; }
        public string InputIpv6 { get; set; }

        public TotalIPs()
        { }

        public void Calc(string inputIpv6)
        {
            InputIpv6 = inputIpv6;
            string[] input = inputIpv6.Split(':', '/');
            int prefix = Convert.ToInt16(input[input.Length - 1]);

            CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
            Res = Math.Pow(2, 128 - prefix).ToString("N0", culture);
        }

        public override string ToString()
        {
            return $"{Res}";
        }
    }
}
