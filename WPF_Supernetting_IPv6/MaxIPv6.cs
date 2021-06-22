namespace IPV6
{
    public class MaxIPv6
    {
        public string IpV6 { get; set; }
        public string InputIpv6 { get; set; }
        public string Output { get; set; }

        public MaxIPv6()
        { }

        public void Calc(string inputIpv6)
        {
            InputIpv6 = inputIpv6;
            IpV6 = "";
            string[] input = inputIpv6.Split(':', '/');

            char pad = '0';

            for (int i = 0; i < input.Length - 1; i++)
            {
                input[i] = input[i].PadLeft(4, pad);
                IpV6 += input[i];
            }

            IpV6 = IpV6.PadRight(64, pad);

            Output = IpV6.Substring(0, 4) + ":";
            Output += IpV6.Substring(4, 4) + ":";
            Output += IpV6.Substring(8, 4) + ":";
            Output += IpV6.Substring(12, 4) + ":";
            Output += IpV6.Substring(16, 4) + ":";
            Output += IpV6.Substring(20, 4) + ":";
            Output += IpV6.Substring(24, 4) + ":";
            Output += IpV6.Substring(28, 4) + "/" + input[input.Length - 1];
        }

        public override string ToString()
        {
            return $"{Output}";
        }
    }
}
