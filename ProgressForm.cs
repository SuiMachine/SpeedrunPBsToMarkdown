using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeedrunComSharp;
using System.Diagnostics;
using System.Threading;


namespace SpeedrunToMarkdown
{
    public partial class Progress_Form : Form
    {
        public string output = "";
        public string username = "";
        Form1 _parent;

        public Progress_Form(Form1 parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        public void displayPBs()
        {
            try
            {
                output = "";
                var srlClient = new SpeedrunComClient();
                lb_Progress.Text = "Getting PBs";
                var playersPBs = srlClient.Users.GetPersonalBests(username, null, null, null);
                List<Record> filteredPBs = new List<Record>();
                int progressHelper = 0;
                int currentProgress = 0;
                if (playersPBs.Count > 0)
                {
                    progressHelper = playersPBs.Count * 2;
                    progressBar1.Maximum = progressHelper;

                    foreach (var element in playersPBs)
                    {
                        lb_Progress.Text = String.Format("Filtering out obsolete runs: {0} / {1}", currentProgress, progressHelper/2);
                        currentProgress++;
                        progressBar1.Value = currentProgress;
                        if (!filteredPBs.Any(comp => comp.GameID == element.GameID && comp.CategoryID == element.CategoryID))//.Contains(comp => comp.Game == element.Game))
                        {
                            if (element.Level == null)
                                filteredPBs.Add(element);
                        }
                    }

                    lb_Progress.Text = String.Format("Sorting by name");
                    List<Record> SortedList = filteredPBs.OrderBy(o => o.Game.Name).ToList();

                    foreach (var element in SortedList)
                    {
                        lb_Progress.Text = String.Format("Getting times and prepering final text: {0} / {1}", currentProgress, progressHelper);
                        currentProgress++;
                        progressBar1.Value = currentProgress;
                        output += "+ **" + element.Game.Name + "** (" + element.Category.Name + ") - [";
                        if (element.Times.Primary > TimeSpan.FromHours(1))
                        {
                            output += element.Times.Primary.Value.Hours + ":";
                            if (element.Times.Primary.Value.Minutes < 10)
                                output += "0";
                            output += element.Times.Primary.Value.Minutes + ":";
                            if (element.Times.Primary.Value.Seconds < 10)
                                output += "0";
                            output += element.Times.Primary.Value.Seconds;
                        }
                        else
                        {
                            if (element.Times.Primary.Value.Minutes < 10)
                                output += "0";
                            output += element.Times.Primary.Value.Minutes + ":";
                            if (element.Times.Primary.Value.Seconds < 10)
                                output += "0";
                            output += element.Times.Primary.Value.Seconds;
                        }
                        output += "](" + element.WebLink.AbsoluteUri + ")\n";
                    }
                    _parent.RB_Result.Text = output;
                    this.Close();
                }
                else
                {
                    output = "Nothing";
                    _parent.RB_Result.Text = output;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION ERROR: " + ex.ToString());
                output = ex.ToString();
                _parent.RB_Result.Text = output;
                this.Close();
            }
        }
    }
}
