using System;
using System.Globalization;

namespace IPV6
{
    public class TotalNetworks
    {
        public string IpV6 { get; set; }
        public string Res { get; set; }
        public string InputIpv6 { get; set; }

        public TotalNetworks()
        { }

        public void Calc(string inputIpv6)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
            InputIpv6 = inputIpv6;
            string[] input = inputIpv6.Split(':', '/');
            int prefix = Convert.ToInt16(input[input.Length - 1]);

            if (prefix == 64)
                Res = Math.Pow(2, 64).ToString("N0", culture);
            else
                Res = Math.Pow(2, 64 - prefix).ToString("N0", culture);
        }

        public override string ToString()
        {
            return $"{Res}";
        }
    }
}
