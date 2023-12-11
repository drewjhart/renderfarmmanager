using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenderFarmDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var items = new List<string>();

            using (var stream = File.OpenRead(@"I:\VIZ\Viz_Tools\farm2.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] entry = new String[4];
                    items.Add(line);
                    entry[0] = line;

                    ServiceController sc = new ServiceController();
                    sc.MachineName = line;
                    sc.ServiceName = "BACKBURNER_SRV_200";

                    if (sc.Status.Equals(ServiceControllerStatus.Running))
                        entry[1] = "ON";
                    else
                        entry[1] = "OFF";

                    sc.ServiceName = "VRaySpawner 2016";
                    if (sc.Status.Equals(ServiceControllerStatus.Running))
                        entry[2] = "ON";
                    else
                        entry[2] = "OFF";

                    sc.ServiceName = "mi-raysat_3dsmax2016_64";
                    if (sc.Status.Equals(ServiceControllerStatus.Running))
                        entry[3] = "ON";
                    else
                        entry[3] = "OFF";

                    var listViewItem = new ListViewItem(entry);
                    listView1.Items.Add(listViewItem);
                }
            }
            
            this.listBox1.DataSource = items;
            listView1.Columns.Add("Computer", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Vray Spawner", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Backburner", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Mental Ray", 100, HorizontalAlignment.Left);

            

           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Object selecteditem in listBox1.SelectedItems)
            {
                String strItem = selecteditem as String;
                            
                ProcessStartInfo startinfo = new ProcessStartInfo();           
                startinfo.FileName = "shutdown.exe";
                startinfo.Arguments = "/r /m " + @"\\" + strItem;
                startinfo.RedirectStandardError = true;
                startinfo.UseShellExecute = false;

                Process p = Process.Start(startinfo);
                string errstring = p.StandardError.ReadToEnd();
                p.WaitForExit();
                Debug.WriteLine(errstring);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Object selecteditem in listBox1.SelectedItems)
            {
                String strItem = selecteditem as String;

                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = "shutdown.exe";
                startinfo.Arguments = "/s /m " + @"\\" + strItem;
                startinfo.RedirectStandardError = true;
                startinfo.UseShellExecute = false;

                Process p = Process.Start(startinfo);
                string errstring = p.StandardError.ReadToEnd();
                p.WaitForExit();
                Debug.WriteLine(errstring);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
