using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Http;
using System.Management;
using System.Management.Instrumentation;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.WindowsAPICodePack.Dialogs;



namespace RenderFarmDemo
{
    public partial class Form1 : Form
    {        
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        ColumnHeader columnHeader2 = new ColumnHeader();

        public Form1()
        {
            InitializeComponent();
            SetDoubleBuffered(dataGridView1);

            List <string[]> items = new List<string[]>();
            watch.Start();          
            using (var stream = File.OpenRead(@"I:\VIZ\Viz_Tools\farm.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                   
                    String[] entry = new String[5];                    
                    entry[0] = line;
                    entry[1] = "";
                    entry[2] = "";
                    entry[3] = "LOADING";
                    items.Add(entry);                    

                    CheckServiceStatusAsync(line);


                    /* leaving this in in case we want to accomodate mental ray in the future
                    sc.ServiceName = "mi-raysat_3dsmax2016_64";
                    if (sc.Status.Equals(ServiceControllerStatus.Running))
                        entry[3] = "ON";
                    else
                        entry[3] = "OFF";
                    */
                    //var listViewItem = new ListViewItem(entry);
                    //listView1.Items.Add(listViewItem);
                    this.dataGridView1.AllowUserToAddRows = false;
                    this.dataGridView1.Columns[1].DefaultCellStyle.NullValue = null;
                    this.dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    this.dataGridView1.Columns[2].DefaultCellStyle.NullValue = null;
                    this.dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    this.dataGridView1.Rows.Add(entry[0], null, null, entry[3]);                    
                }

            }
            
            columnHeader2.Text = "VRay Sprawner";
            columnHeader2.Width = 100;
            columnHeader2.TextAlign = HorizontalAlignment.Left;

       
        }
    
   

      

        private async Task CheckServiceStatusAsync(string line)
        {
            string[] status = await Task.Run(() => CheckStatus(line));            
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {                
                if (row.Cells[0].Value.Equals(line)) //for each comp
                {                    
                    if (status[0].Equals("Access Denied."))
                    {
                        row.Cells[1].Value = null; 
                        row.Cells[2].Value = null;
                        row.Cells[3].Value = "Access Denied.";
                    }               
                    else
                    {
                        row.Cells[9].Value = status[0];
                        row.Cells[4].Value = Convert.ToString(status[5]);
                        if (checkBox1.Checked == false)
                        {
                            if (status[1].Equals("1"))
                            {
                                row.Cells[2].Value = status[1];
                                row.Cells[2].Value = imageList1.Images[10];
                            }
                            else if (status[1].Equals("0"))
                            {
                                row.Cells[2].Value = status[1];
                                row.Cells[2].Value = imageList1.Images[11];
                            }
                            else
                            {
                                row.Cells[3].Value = status[1];
                            }
                            if (status[3].Equals("1"))
                            {
                                row.Cells[1].Value = status[3];
                                row.Cells[1].Value = imageList1.Images[10];
                            }
                            else if (status[3].Equals("0"))
                            {
                                row.Cells[1].Value = status[3];
                                row.Cells[1].Value = imageList1.Images[11];
                            }
                            else
                            {
                                row.Cells[3].Value = status[3];
                            }
                        }
                        else
                        {
                            double vrayAff = Convert.ToDouble(status[4]) / Convert.ToDouble(status[0]);
                            double backAff = Convert.ToDouble(status[2]) / Convert.ToDouble(status[0]);

                            
                            row.Cells[5].Value = Convert.ToString(vrayAff);
                            row.Cells[6].Value = Convert.ToString(backAff);
                            row.Cells[7].Value = Convert.ToString(status[7]);
                            row.Cells[8].Value = Convert.ToString(status[8]);

                            if (Convert.ToDouble(status[3]) == 0)
                            {
                                row.Cells[1].Value = "0";
                                row.Cells[1].Value = imageList1.Images[12];
                            }
                            else
                            {
                                if (status[3].Equals("1"))
                                {
                                    row.Cells[1].Value = Convert.ToString(vrayAff);
                                    
                                    if (vrayAff == 1)
                                    {
                                        row.Cells[1].Value = imageList1.Images[10];
                                      //  item.SubItems[1].ImageIndex = 0;
                                    }
                                    else if (vrayAff >= 0.9 && vrayAff < 1)
                                        row.Cells[1].Value = imageList1.Images[9];
                                    else if (vrayAff >= 0.8 && vrayAff < 0.9)
                                        row.Cells[1].Value = imageList1.Images[8];
                                    else if (vrayAff >= 0.7 && vrayAff < 0.8)
                                        row.Cells[1].Value = imageList1.Images[7];
                                    else if (vrayAff >= 0.6 && vrayAff < 0.7)
                                        row.Cells[1].Value = imageList1.Images[6];
                                    else if (vrayAff >= 0.5 && vrayAff < 0.6)
                                        row.Cells[1].Value = imageList1.Images[5];
                                    else if (vrayAff >= 0.4 && vrayAff < 0.5)
                                        row.Cells[1].Value = imageList1.Images[4];
                                    else if (vrayAff >= 0.3 && vrayAff < 0.4)
                                        row.Cells[1].Value = imageList1.Images[3];
                                    else if (vrayAff >= 0.2 && vrayAff < 0.3)
                                        row.Cells[1].Value = imageList1.Images[2];
                                    else if (vrayAff >= 0.1 && vrayAff < 0.2)
                                        row.Cells[1].Value = imageList1.Images[1];
                                    else if (vrayAff >= 0.01 && vrayAff < 0.1)
                                        row.Cells[1].Value = imageList1.Images[0];
                                    else if (status[1].Equals("Access Denied."))
                                    {
                                        row.Cells[3].Value = status[1];
                                    }
                                    row.Cells[5].Value = Convert.ToString(vrayAff);
                                    row.Cells[7].Value = status[7];
                                    
                                }
                                else if (status[4].Equals("Access Denied."))
                                {
                                    row.Cells[3].Value = status[1];
                                }
                            }
                            if (Convert.ToDouble(status[1]) == 0)
                            {
                                row.Cells[2].Value = "0";
                                row.Cells[2].Value = imageList1.Images[12];
                            }
                            else
                            {
                                if (status[1].Equals("1"))
                                {
                                    row.Cells[1].Value = Convert.ToString(backAff);
                                    
                                    if (backAff == 1)
                                    {
                                        row.Cells[2].Value = imageList1.Images[10];
                                       // item.SubItems[1].ImageIndex = 0;
                                    }
                                    else if (backAff >= 0.9 && backAff < 1)
                                        row.Cells[2].Value = imageList1.Images[9];
                                    else if (backAff >= 0.8 && backAff < 0.9)
                                        row.Cells[2].Value = imageList1.Images[8];
                                    else if (backAff >= 0.7 && backAff < 0.8)
                                        row.Cells[2].Value = imageList1.Images[7];
                                    else if (backAff >= 0.6 && backAff < 0.7)
                                        row.Cells[2].Value = imageList1.Images[6];
                                    else if (backAff >= 0.5 && backAff < 0.6)
                                        row.Cells[2].Value = imageList1.Images[5];
                                    else if (backAff >= 0.4 && backAff < 0.5)
                                        row.Cells[2].Value = imageList1.Images[4];
                                    else if (backAff >= 0.3 && backAff < 0.4)
                                        row.Cells[2].Value = imageList1.Images[3];
                                    else if (backAff >= 0.2 && backAff < 0.3)
                                        row.Cells[2].Value = imageList1.Images[2];
                                    else if (backAff >= 0 && backAff < 0.2)
                                        row.Cells[2].Value = imageList1.Images[1];
                                    else if (backAff >= 0.01 && backAff < 0.1)
                                        row.Cells[2].Value = imageList1.Images[0];
                                    else if (status[1].Equals("Access Denied."))
                                    {
                                        row.Cells[3].Value = status[1];
                                    }
                                    row.Cells[6].Value = Convert.ToString(backAff);
                                    row.Cells[8].Value = status[8];
                                    
                                }
                            }
                        }
                        row.Cells[3].Value = status[6];
                    }
                }               
            }
            dataGridView1.Refresh();
        }
      
    
        private string[] CheckStatus(string line)
        {
            
            string[] access = new string[9];
            int totalCoreNum = 0;
            int backRun = 0;
            int vrayRun = 0;
            int backCores = 666;
            int vrayCores = 666;
            int maxNum = 0;
            string result = "";
            string vrayPID = "";
            string backPID = "";
            double counter = 1;
            double affinity = 0;
            string compName = @"\\" + line;
            string fileName = compName + @"\C$\IBI\RFM_ProgramFiles\ProcessCheckRunner.bat";            
            try //try catch is new and access needs to be rewrittent so that it provides what is needed in function above 
            {
                ConnectionOptions connOptions = new ConnectionOptions();
                connOptions.Authority = "NTLMDOMAIN:CanEast.IBIGROUP.com";                
                connOptions.Username = authUser;
                connOptions.Password = authPass;
                ManagementScope scope = new ManagementScope(compName+@"\root\cimv2", connOptions);                
                scope.Connect();

                //get Vray version
                using (StreamReader reader = new StreamReader(compName + @"\c$\Program Files\Chaos Group\V-Ray\3dsmax 2016 for x64\docs\vray_changelog.txt"))
                {
                    string firstLine = reader.ReadLine() ?? "";
                    char delimit = ' ';
                    result = firstLine.Split(delimit)[1];                    
                }

                if (checkBox1.Checked)
                { 
                    foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                    {
                        totalCoreNum += int.Parse(item["NumberOfCores"].ToString());
                    }
                }

                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Process WHERE Name='3dsmax.exe'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection objectCollection = searcher.Get();

                foreach (ManagementObject managementObject in objectCollection)
                {
                    string[] argList = new string[] { string.Empty, string.Empty };
                    int parentPID = Convert.ToInt32(managementObject["ParentProcessId"]);
                    int processID = Convert.ToInt32(managementObject["ProcessId"]);

                    if (checkBox1.Checked)
                    {

                        //trigger bat that checks processor affinity for the current process
                        ManagementClass wmiProcess = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                        ManagementBaseObject inParams = wmiProcess.GetMethodParameters("Create");
                        inParams["CommandLine"] = fileName + " \"" + processID + "\"";  //passes the adjustment value as a parameter down through the bat to the powershell script

                        ManagementBaseObject outParams = wmiProcess.InvokeMethod("Create", inParams, null);
                        try
                        {
                            using (StreamReader reader = new StreamReader(compName + @"\C$\IBI\RFM_ProgramFiles\temptext.txt"))
                            {
                                string line2 = reader.ReadLine();
                                affinity = Convert.ToDouble(line2);
                                double convertNum = Math.Pow(2, 1);
                                counter = 1;
                                while ((affinity / convertNum) > 1)
                                {
                                    convertNum = Math.Pow(2, counter);
                                    counter++;
                                }
                                counter--;
                            }
                        }
                        catch (IOException ex)
                        {
                            access[0] = "Check Install.";
                            return access;
                        }
                    }

                    ObjectQuery queryParent = new ObjectQuery("SELECT * FROM Win32_Process WHERE ProcessID=" + parentPID + "");
                    ManagementObjectSearcher searcherParent = new ManagementObjectSearcher(scope, queryParent);
                    ManagementObjectCollection objectCollectionParent = searcherParent.Get();
                    foreach (ManagementObject managementObjectParent in objectCollectionParent)
                    {                 
                        string parentProcess = managementObjectParent["Name"].ToString();                        

                        if (parentProcess.Equals("vrayspawner2016.exe"))
                        {
                            
                            vrayRun = 1;                            
                            vrayCores = Convert.ToInt32(counter);
                            vrayPID = Convert.ToString(processID);
                                
                        }
                        else if (parentProcess.Equals("maxadapter.adp.exe"))
                        {
                            backRun = 1;
                            backCores = Convert.ToInt32(counter);
                            backPID = Convert.ToString(processID);
                        }

                    }
                    maxNum++;
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.ToString());
                access[0] = "Access Denied.";
                return access;
            }            


            access[0] = Convert.ToString(totalCoreNum);
            access[1] = Convert.ToString(backRun);
            access[2] = Convert.ToString(backCores);
            access[3] = Convert.ToString(vrayRun);
            access[4] = Convert.ToString(vrayCores);
            access[5] = Convert.ToString(maxNum);
            access[6] = result;
            access[7] = vrayPID;
            access[8] = backPID;

            return access;

         
            
        }

        private bool statusCheck(string line)
        {
            bool access = false;
            ServiceController sc = new ServiceController();
            sc.MachineName = line;
            sc.ServiceName = "VRaySpawner 2016";
            
            access = sc.Status.Equals(ServiceControllerStatus.Running);         
            return access;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                await Task.Run(() =>compRestarter(strItem));
            }
            refreshList();
        }

        public void compRestarter(string compName)
        {
            
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = "shutdown.exe";
            startinfo.Arguments = "/r /m " + @"\\" + compName;
            startinfo.RedirectStandardError = true;
            startinfo.UseShellExecute = false;

            Process p = Process.Start(startinfo);
            string errstring = p.StandardError.ReadToEnd();
            p.WaitForExit();
            Debug.WriteLine(errstring);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                await Task.Run(() => compShutdown(strItem));
            }
            refreshList();
        }

        public void compShutdown(string compName)
        {
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = "shutdown.exe";
            startinfo.Arguments = "/s /m " + @"\\" + compName;
            startinfo.RedirectStandardError = true;
            startinfo.UseShellExecute = false;

            Process p = Process.Start(startinfo);
            string errstring = p.StandardError.ReadToEnd();
            p.WaitForExit();
            Debug.WriteLine(errstring);
        }
        
        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Only interested in 2nd column.
           if (e.Header != columnHeader2)
           {
            e.DrawDefault = true;
            return;
           }

           // e.DrawBackground();
            SolidBrush greenBrush = new SolidBrush(Color.DarkOliveGreen);
            var imageRect = new Rectangle(e.Bounds.X, e.Bounds.Y,100, 10);
            e.Graphics.FillRectangle(greenBrush, imageRect);
            //e.Graphics.DrawImage(SystemIcons.Information.ToBitmap(), imageRect);
        }
   

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
                dataGridView1.CurrentRow.Selected = false;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string ServiceName = "";
        
            if (radioButton1.Checked)
            {
                ServiceName = "VRaySpawner 2016";
            }
            else if (radioButton2.Checked)
            {
                ServiceName = "BACKBURNER_SRV_200";
            }
          


            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                await Task.Run(() => processStarter(strItem, ServiceName));
            }
            refreshList();          
        }

        public void processStarter(string compName, string serviceName)
        {
            ServiceController sc = new ServiceController();
            sc.MachineName = compName;
            sc.ServiceName = serviceName;
            if (sc.Status.Equals(ServiceControllerStatus.Stopped) || sc.Status.Equals(ServiceControllerStatus.StopPending))
                sc.Start();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string ServiceName = "";
            if (radioButton1.Checked)
            {
                ServiceName = "VRaySpawner 2016";
            }
            else if (radioButton2.Checked)
            {
                ServiceName = "BACKBURNER_SRV_200";
            }
         

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                await Task.Run(() => processStopper(strItem, ServiceName));
            }
            refreshList();            
        }

        public void processStopper(string compName, string serviceName)
        {
            ServiceController sc = new ServiceController();
            sc.MachineName = compName;
            sc.ServiceName = serviceName;
            if (sc.Status.Equals(ServiceControllerStatus.Running))
                sc.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
            refreshList();
        }

        public void refreshList2 ()
        {
            ServiceController sc = new ServiceController();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {

                String strItem = row.Cells[0].Value.ToString();
                if (row.Cells[3].Value.ToString() != "Access Denied.")
                    CheckServiceStatusAsync(strItem);

            }
          
        }


        public void refreshList ()
        {           
            ServiceController sc = new ServiceController();
            if (dataGridView1.SelectedRows.Count == 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                    String strItem = row.Cells[0].Value.ToString();                    
                    if (row.Cells[3].Value.ToString() != "Access Denied.") 
                        CheckServiceStatusAsync(strItem);
                }
            }
            else
            {                
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    
                    String strItem = row.Cells[0].Value.ToString();                    
                    if (row.Cells[3].Value.ToString() != "Access Denied.")
                        CheckServiceStatusAsync(strItem);

                }
            }   
        }

        public void entryRefresher(string compName, string serviceName)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = true;           
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {        
            bool dup = false;
            var folderSelectorDialog = new CommonOpenFileDialog();
            folderSelectorDialog.Title = "Select Directory";
            folderSelectorDialog.IsFolderPicker = true;
            folderSelectorDialog.EnsureReadOnly = true;
            if (folderSelectorDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string copyFolder = folderSelectorDialog.FileName;
                textBox1.Text = copyFolder;               
            }
            else
            {
                return;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {            
            OpenFileDialog openFDialog = new OpenFileDialog();
            openFDialog.Title = "Select Install File";            
            if (openFDialog.ShowDialog() == DialogResult.OK)
            {
                string installLocation = openFDialog.FileName;
                textBox2.Text = installLocation;                
            }
            else
            {
                return;
            }
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                string destPath = @"C$\Program Files\Autodesk\3ds Max 2016\plugins";
                //string filePath = Path.GetFullPath(textBox2.Text);
                string dirPath = Path.GetFullPath(textBox1.Text);
                string compName = @"\\" + strItem;
                dirPath = dirPath + @"\";
                //filePath = filePath.Replace(dirPath, "");
                string destDirFolder = new DirectoryInfo(dirPath).Name;
                string destFinal = destPath; //Path.Combine(destPath, destDirFolder);
                
                destFinal = Path.Combine(compName, destFinal);
                //string filePathFinal = Path.Combine(destFinal, filePath);                                                      
                Console.WriteLine("destination: ========> " + destFinal);

                label8.Text = "Copying Required Files";
                await Task.Run(() =>fileCopier(compName, dirPath, destFinal, authUser, authPass, domain));
                label8.Text = "Running Installation File";                
               

            }
      

        }

        private static void fileCopier(string compName, string copyDir, string destDir, string authUser, string authPass, string domain)            
        {
            // errorCode = WNetAddConnection2(nr, authPass, @"CANEAST\andrew.hart", 0); //<------- put these in a text file, to be loaded on initialize   
            try
            {
              
            NETRESOURCE nr = new NETRESOURCE();
            nr.dwType = RESOURCETYPE_DISK;
            nr.lpRemoteName = compName;
            int errorCode = WNetCancelConnection2(nr.lpRemoteName, 0, true);
            errorCode = WNetAddConnection2(nr, authPass, @"CANEAST\andrew.hart", 0);//<------- put these in a text file, to be loaded on initialize   
                Console.WriteLine("errorcode: ======>" + errorCode);
                Console.WriteLine("copyDir: ======>" + copyDir);
                DirectoryInfo dir = new DirectoryInfo(copyDir);
            DirectoryInfo[] dirs = dir.GetDirectories();
            
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }            
            FileInfo[] files = dir.GetFiles();
            
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDir, file.Name);
                string origPath = Path.Combine(copyDir, file.Name);
               
                    // Console.WriteLine(temppath);        
                    try
                {
                        Console.WriteLine("Copy paths: " + origPath + " " + temppath);
                        System.IO.File.Copy(origPath, temppath, true); //check file full name????? TODAY<----------------------
                        Console.WriteLine("DONE");
                    }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
            foreach (DirectoryInfo subdir in dirs)
            {    
                string temppath = Path.Combine(destDir, subdir.Name);                 
                fileCopier(compName, subdir.FullName, temppath, authUser, authPass, domain);
                
            }

            errorCode = WNetCancelConnection2(nr.lpRemoteName, CONNECT_UPDATE_PROFILE, true);//**            
            }
            catch (System.IO.IOException e)
            {
                //Console.WriteLine(e.Message);
            }
        }


        
        private static void fileExecute (string compName, string fileName, string authUser, string authPass) 
        {
            try
            {
                string scopeString = compName + @"\root\cimv2";                
                ConnectionOptions options = new ConnectionOptions();
                options.Authority = "NTLMDOMAIN:CanEast.IBIGROUP.com";
                options.Username = authUser;
                options.Password = authPass;
                ManagementScope scope = new ManagementScope(scopeString, options);
                scope.Connect();

                
                var processToRun = new[] { fileName };

                ObjectQuery query = new ObjectQuery(
                  "SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
               

                var wmiProcess = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                wmiProcess.InvokeMethod("Create", processToRun);

            }
            catch (Exception ex)
            {
              //  Console.WriteLine(ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + ex.StackTrace);                
            }
               
        }

        private void button11_Click(object sender, EventArgs e)
        {            
                
         

          //  fileExecuter(filePath, dirPath, destPath);

            
        }
        private static void fileExecuter(string filePath, string dirPath, string destPath)
        {
                        
            if (filePath.StartsWith(dirPath, StringComparison.OrdinalIgnoreCase))
            {             
                dirPath = dirPath+@"\";
                filePath = filePath.Replace(dirPath,"");                
                string destDirFolder = new DirectoryInfo(dirPath).Name;
                string destFinal = Path.Combine(destPath, destDirFolder);                
                string filePathFinal = Path.Combine(destFinal,filePath);       //filepaths converte
            }
        }

        private void flowLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        const int RESOURCETYPE_DISK = 1;
        const int CONNECT_UPDATE_PROFILE = 0x00000001;        

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope = 0;
            public int dwType = 0;
            public int dwDisplayType = 0;
            public int dwUseage = 0;
            public string lpLocalName = null;
            public string lpRemoteName = "";
            public string lpComment = "";
            public string lpProvider = null;
        }
        [DllImport("Mpr.dll")]
        private static extern int WNetAddConnection2(
        NETRESOURCE lpNetResource,
        string lpPassword,
        string lpUserID,
        int dwFlags
        );
        [DllImport("Mpr.dll")]
        private static extern int WNetCancelConnection2(
        string lpName,
        int dwFlags,
        bool fForce
        );

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        private int getProcessInfo (string reqProcess, string compName)
        {
            try
            {
                ConnectionOptions connOptions = new ConnectionOptions();
                connOptions.Username = authUser;
                connOptions.Password = authPass;
                ManagementScope scope = new ManagementScope(@"\\dto302453\root\cimv2", connOptions);
                scope.Connect();


                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Process WHERE Name='3dsmax.exe'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection objectCollection = searcher.Get();
                foreach (ManagementObject managementObject in objectCollection)
                {
                    string[] argList = new string[] { string.Empty, string.Empty };
                    int parentPID = Convert.ToInt32(managementObject["ParentProcessId"]);
                    int processID = Convert.ToInt32(managementObject["ProcessId"]);
             

                    ObjectQuery queryParent = new ObjectQuery("SELECT * FROM Win32_Process WHERE ProcessID=" + parentPID + "");
                    ManagementObjectSearcher searcherParent = new ManagementObjectSearcher(scope, queryParent);
                    ManagementObjectCollection objectCollectionParent = searcherParent.Get();
                    foreach (ManagementObject managementObjectParent in objectCollectionParent)
                    {
                 
                        string parentProcess = managementObjectParent["Name"].ToString();
                        if (parentProcess.Equals(reqProcess) )
                        {
                            return processID;
                        }
                        else if (parentProcess.Equals("maxadapter.adp.exe"))
                        {
                            Console.WriteLine("It's Backburner!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
               
            }
            return 0;
        }
        private async void button11_Click_1(object sender, EventArgs e)
        {            
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                string compName = @"\\" + strItem;
                string PID = row.Cells[7].Value.ToString();

                int affinity = 0;
                int cores = Convert.ToInt32(row.Cells[9].Value);
                double slideValue = Convert.ToDouble(trackBar1.Value) / 100;                             
                int affinityNum = Convert.ToInt32(cores * slideValue);
                
                for (int i = 0; i < affinityNum; i++)
                {
                    double powNum = Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i));                  
                    affinity = Convert.ToInt32(powNum) + affinity;
                }
                   
                string fileName = compName + @"\C$\IBI\RFM_ProgramFiles\ProcessRunner.bat";         
                await Task.Run(() => affinityAdjuster(compName, fileName, affinity, PID, authUser, authPass));                
             
            }
            refreshList2();
        }


     
        private static string affinityAdjuster(string compName, string fileName, int adjustNum, string PID, string authUser, string authPass)
        {           
           try
           {
                string scopeString = compName + @"\root\cimv2";                
                ConnectionOptions options = new ConnectionOptions();
                options.Authority = "NTLMDOMAIN:CanEast.IBIGROUP.com";
                options.Username = authUser;
                options.Password = authPass;
                ManagementScope scope = new ManagementScope(scopeString, options);
                scope.Connect();
                
                var processToRun = new[] { fileName };

                ObjectQuery query = new ObjectQuery(
                  "SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

         
                ManagementClass wmiProcess = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                ManagementBaseObject inParams = wmiProcess.GetMethodParameters("Create");

                inParams["CommandLine"] = fileName + " " + adjustNum + " "+ PID;  //passes the adjustment value as a parameter down through the bat to the powershell script
                ManagementBaseObject outParams = wmiProcess.InvokeMethod("Create", inParams, null);    
                
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + ex.StackTrace);
            }
            return null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }  

        private void label10_Click(object sender, EventArgs e)
        {

        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {            
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }


        private async void button12_Click(object sender, EventArgs e)
        {            
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String strItem = row.Cells[0].Value.ToString();
                string compName = @"\\" + strItem;
                string PID = row.Cells[8].Value.ToString();

                int affinity = 0;
                int cores = Convert.ToInt32(row.Cells[9].Value);
                double slideValue = Convert.ToDouble(trackBar2.Value) / 100;
                int affinityNum = Convert.ToInt32(cores * slideValue);

                for (int i = 0; i < affinityNum; i++)
                {
                    double powNum = Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i));
                    affinity = Convert.ToInt32(powNum) + affinity;
                }
                //label13.Text = Convert.ToString(affinity);                
                string fileName = compName + @"\C$\IBI\RFM_ProgramFiles\ProcessRunner.bat";         
                await Task.Run(() => affinityAdjuster(compName, fileName, affinity, PID, authUser, authPass));

            }
            refreshList2();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }
    }
}
