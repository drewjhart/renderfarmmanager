using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;

namespace RenderFarmDemo
{
    public partial class Form3 : Form
    {
        string domain = "";
        string username = "";
        public Form3()
        {
            InitializeComponent();
            textBox1.PasswordChar = '*';
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            userClosing = false;
            username = wp.Identity.Name;            
            int index = username.LastIndexOf("\\")+1;

            if (username.Length > index)
            {
                domain = username.Substring(0, index);
                username = username.Substring(index, username.Length - index);
                
            }
           
            label1.Text = username;
        }

        public string ReturnDomain { get; set; }
        public string ReturnUser { get; set; }
        public string ReturnPass { get; set; }
        public bool userClosing { get; set; }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReturnDomain = domain;
            this.ReturnUser = username;
            this.ReturnPass = textBox1.Text;
            Console.WriteLine(ReturnPass);
            userClosing = true;
            this.DialogResult = DialogResult.OK;            
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        
       

        private void Form3_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (userClosing == false)            
                Application.Exit();
        }
    }
}
