using System;
using System.Collections.Generic;
using System.Linq;
using SpeedrunComSharp;
using System.Diagnostics;

namespace SpeedrunToMarkdown
{

    class Leaderboards
    {
        string output = "";

        public Leaderboards()
        {
        }

        public string displayPBs(string username)
        {
            try
            {
                var srlClient = new SpeedrunComClient();
                var playersPBs = srlClient.Users.GetPersonalBests(username, null, null, null);
                List<Record> filteredPBs = new List<Record>();

                if (playersPBs.Count > 0)
                {
                    foreach(var element in playersPBs)
                    {
                        if (!filteredPBs.Any(comp => comp.GameID == element.GameID && comp.CategoryID == element.CategoryID))//.Contains(comp => comp.Game == element.Game))
                        {
                            if(element.Level == null)
                                filteredPBs.Add(element);
                        }
                    }

                    List<Record> SortedList = filteredPBs.OrderBy(o => o.Game.Name).ToList();

                    foreach (var element in SortedList)
                    {
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
                    return output;
                }
                else
                    return "Nothing";
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION ERROR: " + ex.ToString());
                return ex.ToString();
            }
        }
    }
}