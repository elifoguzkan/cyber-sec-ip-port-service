using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.ServiceProcess;
using System.Net.NetworkInformation;
namespace finding_ip_address
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Columns.Add("Servis Adı", 180);
            listView1.Columns.Add("Görünen Adı", 180);
            listView1.Columns.Add("Servis Tipi", 180);
            listView1.Columns.Add("Durum", 70);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName();
            string myIp = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            textBox1.Text = myIp;
            textBox2.Text = hostName;
            foreach(ServiceController service in ServiceController.GetServices())
            {
                string serviceName = service.ServiceName;
                string serviceDisplayName = service.DisplayName;
                string serviceType = service.ServiceType.ToString();
                string status = service.Status.ToString();
                listBox1.Items.Add(serviceName + " " + serviceDisplayName + serviceType + " " + status);
                ListViewItem item = new ListViewItem(serviceName);
                item.SubItems.Add(serviceDisplayName);
                item.SubItems.Add(serviceType);
                item.SubItems.Add(status);
                listView1.Items.Add(item);
                ListTcpPorts();
            }
        }
        private void ListTcpPorts()
        {
            // Mevcut TCP bağlantılarını ve dinleyicilerini al
            var tcpConnections = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
            var tcpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

            // TCP bağlantılarını listele
            foreach (var connection in tcpConnections)
            {
                string localEndpoint = $"{connection.LocalEndPoint.Address}:{connection.LocalEndPoint.Port}";
                string remoteEndpoint = $"{connection.RemoteEndPoint.Address}:{connection.RemoteEndPoint.Port}";
                string state = connection.State.ToString();
                listBox2.Items.Add($"Bağlantı: {localEndpoint} -> {remoteEndpoint} ({state})");
            }
            // TCP dinleyicilerini listele
            foreach (var listener in tcpListeners)
            {
                string localEndpoint = $"{listener.Address}:{listener.Port}";
                listBox2.Items.Add($"Dinleyici: {localEndpoint}");
            }
        }
    }
}
