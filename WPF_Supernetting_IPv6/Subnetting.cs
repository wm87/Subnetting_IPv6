using System;
using System.Collections.Generic;

namespace IPV6
{
    public class Subnetting
    {
        public string IpV6 { get; set; }
        private int subnet;
        public int Actual { get; set; }
        public string InputIpv6 { get; set; }
        public string Output { get; set; }
        private List<string> subList = new List<string>();

        public Subnetting()
        { }

        public int Subnets
        {
            set => subnet = value;
            get => (int)Math.Pow(2, Math.Ceiling(Math.Log(subnet, 2)));
        }

        public void Calc(int inputPage, string inputIpv6, MaxIPv6 maxIP, FirstAddressIPv6 firstIP, LastAddressIPv6 lastIP, MinIPv6 minIP)
        {
            InputIpv6 = inputIpv6;
            inputPage--;

            subList.Clear();

            string[] prefix = InputIpv6.Split(':', '/');
            int pref = Convert.ToInt32(prefix[prefix.Length - 1]);
            int len = (int)Math.Log(this.Subnets, 2);
            int iter = 0;
            string ip = "";
            string[] ipArr = InputIpv6.Split(':');

            string hlp;
            string tmp;
            string subNet;

            for (int i = 0; i < ipArr.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                    ip += Convert.ToString(Convert.ToInt32(ipArr[i][j].ToString(), 16), 2).PadLeft(4, '0');
            }

            int lessThan64 = 64;

            if (Subnets < 64)
                lessThan64 = Subnets % 64;

            //  for (int i = 0; i < subnets; i++)
            for (int i = inputPage * 64; i < (inputPage * 64) + lessThan64; i++)
            {
                hlp = Convert.ToString(i, 2).PadLeft((int)Math.Log(this.Subnets, 2), '0');
                tmp = (ip.Substring(0, pref) + hlp).PadRight(64, '0');

                subNet = Convert.ToString(Convert.ToInt32(tmp.Substring(0, 16), 2), 16) + ":";
                subNet += Convert.ToString(Convert.ToInt32(tmp.Substring(16, 16), 2), 16) + ":";
                subNet += Convert.ToString(Convert.ToInt32(tmp.Substring(32, 16), 2), 16) + ":";
                subNet += Convert.ToString(Convert.ToInt32(tmp.Substring(48, 16), 2), 16) + "/" + (pref + len);

                maxIP.Calc(subNet);
                firstIP.Calc(maxIP.ToString());
                lastIP.Calc(maxIP.ToString());
                minIP.calc(maxIP.ToString());

                subList.Add(maxIP + "|" + firstIP + "|" + lastIP + "|" + minIP);
                iter++;
            }
        }

        public List<string> GetList()
        {
            return subList;
        }
    }
}
