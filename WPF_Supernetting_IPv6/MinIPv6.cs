using System.Collections.Generic;
using System.Linq;

namespace IPV6
{
    public class MinIPv6
    {
        public string OutIpV6 { get; set; }
        public string InputIpv6 { get; set; }

        private Dictionary<int, int> res = new Dictionary<int, int>();
        private List<string> resIpv6 = new List<string>();
        private int[] indexOfIpv6 = new int[8];
        private string[] input;

        public MinIPv6()
        { }

        public void calc(string inputIpv6)
        {
            int cntZ = 0;
            InputIpv6 = inputIpv6;
            input = InputIpv6.Split(':', '/');
            indexOfIpv6 = null;
            indexOfIpv6 = new int[8];

            OutIpV6 = "";
            res.Clear();
            resIpv6.Clear();

            for (int i = 0; i < 8; i++)
            {
                if (input[i] == "0" || input[i] == "0000")
                {
                    indexOfIpv6[i] = 0;
                    resIpv6.Add("0");
                }
                else if (input[i] != "0" || input[i] != "0000")
                {
                    indexOfIpv6[i] = 1;
                    resIpv6.Add(input[i].TrimStart('0'));
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (indexOfIpv6[i] == 0)
                {
                    cntZ = 1;

                    for (int j = i + 1; j < input.Length - 1; j++)
                    {
                        if (indexOfIpv6[j] == 0)
                            cntZ++;
                        else
                            break;
                    }
                    res.Add(i, cntZ);
                    i += cntZ;
                }
            }

            var hlpList = res.ToList();
            hlpList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            int index = hlpList[0].Key;
            int length = hlpList[0].Value;

            if (length > 0)
                for (int i = index; i < index + length; i++)
                    resIpv6.RemoveAt(index);

            for (int i = 0; i < resIpv6.Count; i++)
            {
                if (i != index)
                    OutIpV6 += ":" + resIpv6[i];
                if (i == index && length > 0)
                    OutIpV6 += "::" + resIpv6[i];
            }

            if (resIpv6.Count == index)
                OutIpV6 += "::";

            OutIpV6 = OutIpV6.Remove(0, 1);
            OutIpV6 += "/" + input[input.Length - 1];
        }

        public override string ToString()
        {
            return $"{OutIpV6}";
        }
    }
}
