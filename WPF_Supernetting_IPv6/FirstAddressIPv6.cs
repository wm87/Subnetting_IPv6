using System;

namespace IPV6
{
    public class FirstAddressIPv6
    {
        private string BinIPv6 { get; set; }
        private string Tmp { get; set; }
        private string Res { get; set; }
        public string InputIpv6 { get; set; }

        public FirstAddressIPv6()
        { }

        public void Calc(string inputIpv6)
        {
            InputIpv6 = inputIpv6;
            BinIPv6 = "";
            Tmp = "";
            Res = "";

            string[] input = inputIpv6.Split(':', '/');
            int startPos = Convert.ToInt32(input[input.Length - 1]);

            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                    BinIPv6 += Convert.ToString(Convert.ToInt32(input[i][j].ToString(), 16), 2).PadLeft(4, '0');
            }

            BinIPv6 = BinIPv6.Substring(0, startPos);

            for (int i = startPos + 1; i <= 128; i++)
                BinIPv6 += "0";

            for (int i = 0; i < 128; i++)
            {
                Tmp += Convert.ToString(Convert.ToInt32(BinIPv6.Substring(i, 4), 2), 16);
                i += 3;
            }

            Res = Tmp.Substring(0, 4) + ":";
            Res += Tmp.Substring(4, 4) + ":";
            Res += Tmp.Substring(8, 4) + ":";
            Res += Tmp.Substring(12, 4) + ":";
            Res += Tmp.Substring(16, 4) + ":";
            Res += Tmp.Substring(20, 4) + ":";
            Res += Tmp.Substring(24, 4) + ":";
            Res += Tmp.Substring(28, 4);
        }

        public override string ToString()
        {
            return $"{Res}";
        }
    }
}
