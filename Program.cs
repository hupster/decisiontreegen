using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;

namespace decisiontreegen
{
    class tree
    {
        public Dictionary<string, tree_entry> data { get; set; }
        public Dictionary<string, string> index { get; set; }
        public string start_ID { get; set; }
        public string version { get; set; }
    }

    class tree_entry
    {
        public string question { get; set; }
        public string type { get; set; }
        public string subtext { get; set; }
        public string textlink { get; set; }
        public string info { get; set; }
        public List<tree_entry_choice> choices { get; set; }

        public tree_entry(string text, string stext)
        {

            question = text;
            type = "answer";
            subtext = stext;
            textlink = "";
            info = "";
            choices = new List<tree_entry_choice>();
        }

        public void addchoice(string choice, int refnr)
        {
            choices.Add(new tree_entry_choice() { choice = choice, next = refnr.ToString() });
            if (type == "answer")
            {
                type = "question";
            }
        }
    }

    class tree_entry_choice
    {
        public string choice { get; set; }
        public string next { get; set; }
    }

    class Program
    {
        const string separator_choice = " -> ";
        const string separator_text = "*";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please provide path to text file input.");
                return;
            }
            //string path = Environment.CurrentDirectory;
            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist: " + filePath);
                return;
            }
            string filePathOut = filePath + ".json";

            // debug option
            bool debug = false;
            if (args.Length > 1)
            {
                if (args[1] == "debug")
                {
                    debug = true;
                }
            }

            // create tree instance
            tree tree_obj = new tree()
            {
                data = new Dictionary<string, tree_entry>(),
                index = new Dictionary<string, string>(),
                start_ID = "1",
                version = "1.1.0"
            };

            // read file
            StreamReader file = new StreamReader(filePath);
            string line, str, choice, value, text, subtext;
            int nr = 0, refnr = 0, tabcnt, idx;
            Dictionary<int, int> refdict = new Dictionary<int, int>();
            while ((line = file.ReadLine()) != null)
            {
                // check line
                if (line.Contains("\""))
                {
                    Console.WriteLine("Line " + nr + " contains invalid characters");
                    return;
                }
                
                // get tab index
                str = line.Trim('\t');
                tabcnt = line.Length - str.Length;
                if (tabcnt > 0)
                {
                    // split the line on the special marker
                    idx = str.IndexOf(separator_choice);
                    if (idx == -1)
                    {
                        Console.WriteLine("Error parsing line " + nr);
                        return;
                    }
                    choice = str.Substring(0, idx);
                    value = str.Substring(idx + separator_choice.Length);
                }
                else
                {
                    // first line
                    choice = "";
                    value = str;
                    if (nr > 0)
                    {
                        Console.WriteLine("Missing tab at line " + nr);
                        return;
                    }
                }

                // split the text on the special marker
                idx = value.IndexOf(separator_text);
                if (idx == -1)
                {
                    text = value;
                    subtext = "";
                }
                else
                {
                    text = value.Substring(0, idx);
                    subtext = value.Substring(idx + separator_text.Length);
                }

                // add entry
                tree_obj.data.Add((nr + 1).ToString(), new tree_entry(text, subtext));
                tree_obj.index.Add(nr.ToString(), (nr + 1).ToString());
                refdict[tabcnt] = nr;

                if (tabcnt > 0)
                {
                    // install reference
                    refnr = refdict[tabcnt - 1];
                    tree_obj.data[(refnr + 1).ToString()].addchoice(choice, nr + 1);

                    if (debug)
                    {
                        Console.WriteLine(String.Format("{0}: {1} tabs, answer {2} for line {3}: {4} text + {5} subtext", nr, tabcnt, choice, refnr, text.Length, subtext.Length));
                    }
                }

                nr++;
            }
            file.Close();

            // Serializing object to json data  
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(tree_obj);

            using (StreamWriter outfile = new StreamWriter(filePathOut))
            {
                outfile.Write(jsonData);
            }
        }
    }
}
