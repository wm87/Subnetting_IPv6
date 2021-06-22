using System;
using System.Collections.Generic;

namespace IPV6
{
    public class IPv6
    {
        private SubnetInformation _SubnetInformation;
        private IPv6Information _IPv6Information;

        MinIPv6 minIP = new MinIPv6();
        MaxIPv6 maxIP = new MaxIPv6();
        BinaryIPv6 binaryIPv6 = new BinaryIPv6();
        NetworkIPv6 networkIPv6 = new NetworkIPv6();
        FirstAddressIPv6 firstIP = new FirstAddressIPv6();
        LastAddressIPv6 lastIP = new LastAddressIPv6();
        TotalNetworks totalNetworks = new TotalNetworks();
        TotalIPs totalIPs = new TotalIPs();
        Subnetting subnetting = new Subnetting();

        public string IP { set; get; }
        public int ActuellPage { set; get; }

        public SubnetInformation GivenSubnets(int anzahlSubNetze, int actuellPage)
        {
            maxIP.Calc(IP);
            IP = maxIP.ToString();

            minIP.calc(IP);
            maxIP.Calc(IP);
            networkIPv6.Calc(IP);
            firstIP.Calc(IP);
            binaryIPv6.Calc(IP);
            lastIP.Calc(IP);
            totalNetworks.Calc(IP);
            totalIPs.Calc(IP);

            subnetting.Subnets = anzahlSubNetze;
            subnetting.Actual = actuellPage;
            subnetting.Calc(subnetting.Actual, IP, maxIP, firstIP, lastIP, minIP);

            _SubnetInformation = new SubnetInformation(subnetting.GetList());
            OnSubnetFound(new SubnetFoundEventArgs(_SubnetInformation));

            return _SubnetInformation;
        }


        public IPv6Information Calc()
        {
            maxIP.Calc(IP);
            IP = maxIP.ToString();

            minIP.calc(IP);
            maxIP.Calc(IP);
            networkIPv6.Calc(IP);
            firstIP.Calc(IP);
            binaryIPv6.Calc(IP);
            lastIP.Calc(IP);
            totalNetworks.Calc(IP);
            totalIPs.Calc(IP);

            subnetting.Subnets = 0;
            subnetting.Actual = 1;
            subnetting.Calc(subnetting.Actual, IP, maxIP, firstIP, lastIP, minIP);

            _IPv6Information = new IPv6Information(minIP, maxIP, firstIP, lastIP, binaryIPv6, networkIPv6, totalNetworks, totalIPs);
            OnIPv6Found(new IPv6FoundEventArgs(_IPv6Information));

            return _IPv6Information;
        }

        // Events
        public delegate void SubnetFoundEventHandler(object sender, SubnetFoundEventArgs e);

        public event SubnetFoundEventHandler SubnetFound;

        protected void OnSubnetFound(SubnetFoundEventArgs e)
        {
            SubnetFound?.Invoke(this, e);
        }

        // Events
        public delegate void IPv6FoundEventHandler(object sender, IPv6FoundEventArgs e);

        public event IPv6FoundEventHandler IPv6Found;

        protected void OnIPv6Found(IPv6FoundEventArgs e)
        {
            IPv6Found?.Invoke(this, e);
        }
    }

    public class SubnetFoundEventArgs : EventArgs
    {
        public SubnetFoundEventArgs(SubnetInformation SubnetInformation)
        {
            this.SubnetInformation = SubnetInformation;
        }

        public SubnetInformation SubnetInformation { get; }
    }

    public class IPv6FoundEventArgs : EventArgs
    {
        public IPv6FoundEventArgs(IPv6Information IPv6Information)
        {
            this.IPv6Information = IPv6Information;
        }

        public IPv6Information IPv6Information { get; }
    }

    public class IPv6Information
    {
        public IPv6Information(
            MinIPv6 minIP, MaxIPv6 maxIP,
            FirstAddressIPv6 firstIP, LastAddressIPv6 lastIP,
            BinaryIPv6 binaryIP, NetworkIPv6 networkIPv6,
            TotalNetworks totalNetwork, TotalIPs totalIP)
        {
            MinIP = minIP;
            MaxIP = maxIP;
            FirstIP = firstIP;
            LastIP = lastIP;
            BinaryIP = binaryIP;
            NetworkIP = networkIPv6;
            TotalNetwork = totalNetwork;
            TotalIP = totalIP;
        }

        public MinIPv6 MinIP { get; }
        public MaxIPv6 MaxIP { get; }
        public FirstAddressIPv6 FirstIP { get; }
        public LastAddressIPv6 LastIP { get; }
        public BinaryIPv6 BinaryIP { get; }
        public NetworkIPv6 NetworkIP { get; }
        public TotalNetworks TotalNetwork { get; }
        public TotalIPs TotalIP { get; }
    }

    public class SubnetInformation
    {
        public SubnetInformation(List<string> subnetList)
        {
            SubnetList = subnetList;
        }

        public List<string> SubnetList;
    }
}