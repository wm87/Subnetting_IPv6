using IPV6;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_Supernetting
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dt = new DataTable();
        private IPv6 ssc;

        Uri resourceUri1 = new Uri("true.png", UriKind.Relative);
        Uri resourceUri2 = new Uri("false.png", UriKind.Relative);

        private int counter = 0;

        public MainWindow()
        {
            InitializeComponent();
            ssc = new IPv6();
            ssc.IPv6Found += Ss_IPv6Found;
            ssc.SubnetFound += Ss_SubnetFound;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Columns.Add("Expanded", typeof(string));
            dt.Columns.Add("First", typeof(string));
            dt.Columns.Add("Last", typeof(string));
            dt.Columns.Add("Minimized", typeof(string));
            dgvISubnets.IsReadOnly = true;

            imgIPv6Check.Source = null;
            Txt_ipv6_TextChanged(null,null);
        }

        private void Ss_IPv6Found(object sender, IPv6FoundEventArgs e)
        {
            txtMin.Text = Convert.ToString(e.IPv6Information.MinIP);
            txtExpanded.Text = Convert.ToString(e.IPv6Information.MaxIP);
            txtFirst.Text = Convert.ToString(e.IPv6Information.FirstIP);
            txtLast.Text = Convert.ToString(e.IPv6Information.LastIP);
            txtNetwork.Text = Convert.ToString(e.IPv6Information.NetworkIP);
            txtTotalNetwork.Text = Convert.ToString(e.IPv6Information.TotalNetwork);
            txtAmountIPs.Text = Convert.ToString(e.IPv6Information.TotalIP);

            rtbBinIPv6.Document.Blocks.Clear();
            rtbBinIPv6_Inv.Document.Blocks.Clear();
            rtbBinIPv6.AppendText(Convert.ToString(e.IPv6Information.BinaryIP));
            rtbBinIPv6_Inv.AppendText(Convert.ToString(e.IPv6Information.BinaryIP));

            TextPointer pos1 = rtbBinIPv6.Document.ContentStart.GetPositionAtOffset(0);
            TextPointer pos2 = rtbBinIPv6.Document.ContentStart.GetPositionAtOffset(Convert.ToInt32(txtPrefix.Text) + (Convert.ToInt32(txtPrefix.Text) / 4 + 2));
            TextPointer pos3 = rtbBinIPv6_Inv.Document.ContentStart.GetPositionAtOffset(0);
            TextPointer pos4 = rtbBinIPv6_Inv.Document.ContentStart.GetPositionAtOffset(Convert.ToInt32(txtPrefix.Text) + (Convert.ToInt32(txtPrefix.Text) / 4 + 2));

            TextRange rng1 = new TextRange(pos1, pos2);
            rng1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Orange);
            rng1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            TextRange rng2 = new TextRange(pos2, rtbBinIPv6.Document.ContentEnd);
            rng2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.SeaGreen);
            rng2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);


            TextRange rng3 = new TextRange(pos3, pos4);
            rng3.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Orange);
            rng3.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            TextRange rng4 = new TextRange(pos4, rtbBinIPv6_Inv.Document.ContentEnd);
            rng4.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.SeaGreen);
            rng4.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void Ss_SubnetFound(object sender, SubnetFoundEventArgs e)
        {
            dgvISubnets.ItemsSource = null;
            dt.Clear();

            decimal upperCorner = (int)Math.Pow(2, Math.Ceiling(Math.Log(Convert.ToInt32(txtSubnets.Text), 2))) / 64;

            if (Convert.ToInt32(txtSubnets.Text) <= 64)
                txtMaxPages.Text = "1";
            else
                txtMaxPages.Text = $"{upperCorner}";

            if (Convert.ToInt32(txtActualPage.Text) > upperCorner)
                txtActualPage.Text = upperCorner.ToString();

            if (Convert.ToInt32(txtActualPage.Text) < 1)
                txtActualPage.Text = 1.ToString();

            foreach (var item in e.SubnetInformation.SubnetList)
            {
                string[] input = item.Split('|');
                dt.Rows.Add(input[0], input[1], input[2], input[3]);
            }

            dgvISubnets.ItemsSource = dt.DefaultView;
        }

        private void Btn_Run_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid_IPv6_IPAddress(txt_ipv6.Text))
            {
                ssc.IP = txt_ipv6.Text + "/" + txtPrefix.Text;
                ssc.Calc();
            }
        }

        private void GivenSubnets()
        {
            ssc.IP = txt_ipv6.Text + "/" + txtPrefix.Text;
            ssc.GivenSubnets(Convert.ToInt32(txtSubnets.Text), Convert.ToInt32(txtActualPage.Text));
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            counter = Convert.ToInt32(txtActualPage.Text);
            counter++;

            if (counter > Convert.ToInt32(txtMaxPages.Text))
                counter = Convert.ToInt32(txtMaxPages.Text);

            txtActualPage.Text = counter.ToString();

            GivenSubnets();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            counter = Convert.ToInt32(txtActualPage.Text);
            counter--;

            if (counter <= 0)
                counter = 1;

            txtActualPage.Text = counter.ToString();

            GivenSubnets();
        }

        private void TxtActualPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                counter = Convert.ToInt32(txtActualPage.Text);

                if (counter > Convert.ToInt32(txtMaxPages.Text))
                    counter = Convert.ToInt32(txtMaxPages.Text);

                if (counter <= 0)
                    counter = 1;

                txtActualPage.Text = counter.ToString();

                GivenSubnets();
            }
        }

        private void txtPrefix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtPrefix.Text != "")
                Btn_Run_Click(sender, e);
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            string expandedItem = ((DataRowView)dgvISubnets.SelectedItem).Row["Expanded"].ToString();
            string firstItem = ((DataRowView)dgvISubnets.SelectedItem).Row["Expanded"].ToString();
            string lastItem = ((DataRowView)dgvISubnets.SelectedItem).Row["Last"].ToString();
            string minimizedItem = ((DataRowView)dgvISubnets.SelectedItem).Row["Minimized"].ToString();

            Clipboard.SetText(string.Format("Expanded: {0}\nExpanded: {1}\nLast: {2}\nMinimized: {3}", expandedItem, firstItem, lastItem, minimizedItem));
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            string output = "";

            foreach (DataRowView dr in dgvISubnets.ItemsSource)
                output += string.Format("{0}|{1}|{2}|{3}\n", dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());

            Clipboard.SetText(string.Format("{0}", output));
        }

        private void NumberValidationPrefix(object sender, TextCompositionEventArgs e)
        {
            // RegExp Generator: https://codepen.io/weiyuan-lane/pen/NWPBLBG

            // Range from 1 to 64 as valid prefix
            Regex regex = new Regex("^((6[0-4])|([1-5][0-9]{1})|([1-9]))$");
            e.Handled = !regex.IsMatch(txtPrefix.Text + e.Text);
        }

        public static bool IsValid_IPv6_IPAddress(string IpAddress)
        {
            bool flag = false;
            if (!string.IsNullOrWhiteSpace(IpAddress))
            {
                IPAddress ip;
                if (IPAddress.TryParse(IpAddress, out ip))
                {
                    flag = ip.AddressFamily == AddressFamily.InterNetworkV6;
                }
            }

            return flag;
        }

        private void Txt_ipv6_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsValid_IPv6_IPAddress(txt_ipv6.Text))
            {
                if (imgIPv6Check != null)
                {
                    var bitmapImage = new BitmapImage();

                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = resourceUri1;
                    bitmapImage.EndInit();

                    imgIPv6Check.Source = bitmapImage;
                }
            }
            else
            {
                if (imgIPv6Check != null)
                {
                    var bitmapImage = new BitmapImage();

                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = resourceUri2;
                    bitmapImage.EndInit();

                    imgIPv6Check.Source = bitmapImage;
                }

            }
        }
    }
}
