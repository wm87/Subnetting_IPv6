using System;

namespace IPV6
{
    public class BinaryIPv6
    {
        public string BinIPv6 { get; set; }
        public string InputIpv6 { get; set; }

        public BinaryIPv6()
        { }

        public void Calc(string inputIpv6)
        {
            InputIpv6 = inputIpv6;
            BinIPv6 = "";
            string[] input = inputIpv6.Split(':', '/');

            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    BinIPv6 += Convert.ToString(Convert.ToInt32(input[i][j].ToString(), 16), 2).PadLeft(4, '0') + " ";
                }
                //BinIPv6 = BinIPv6.Remove(BinIPv6.Length - 1, 1);
                //BinIPv6 += "\n";
            }
            BinIPv6 = BinIPv6.Remove(BinIPv6.Length - 1, 1);
        }

        public override string ToString()
        {
            return $"{BinIPv6}";
        }
    }
}
