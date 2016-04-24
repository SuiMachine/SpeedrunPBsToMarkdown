using System;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;


namespace SpeedrunToMarkdown
{
    public partial class Form1 : Form
    {
        string file = "username.txt";
        Progress_Form _leaderboards;


        /*------------------
        -- INITIALIZATION --
        ------------------*/
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(file))
                loadtext();
        }

        private void loadtext()
        {
            StreamReader SR = new StreamReader(file);
            string line = "";
            while((line = SR.ReadLine()) != null)
            {
                if (line.StartsWith("TagWR:"))
                {
                    bool temp;
                    string[] helper = line.Split(new char[] { ':' }, 2);
                    if (bool.TryParse(helper[1], out temp))
                    {
                        CB_TagWR.Checked = temp;
                    }
                    else
                        CB_TagWR.Checked = true;
                }
                else if (line != String.Empty)
                    TB_Nickname.Text = line;
            }
            SR.Close();
            SR.Dispose();
        }

        private void b_Save_Click(object sender, EventArgs e)
        {
            string text = TB_Nickname.Text + "\nTagWR:" + CB_TagWR.Checked.ToString();
            File.WriteAllText(file, text);
        }

        private void b_GetPB_Click(object sender, EventArgs e)
        {
            _leaderboards = new Progress_Form(this);
            _leaderboards.Show();
            Thread lbThread = new Thread(new ThreadStart(_leaderboards.displayPBs));    //it's ugly, but it works
            _leaderboards.username = TB_Nickname.Text;
            _leaderboards.tagWR = CB_TagWR.Checked;
            lbThread.Start();
            RB_Result.Text = _leaderboards.output;
        }

        private void CB_TagWR_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
