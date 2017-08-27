using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TestConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                
                string csvPath = ConfigurationManager.AppSettings["CsvPath"];
                string outputFolder = ConfigurationManager.AppSettings["OutputFolder"];

                DataTable dt = p.ReadCsvIntoDataTable(csvPath);

                List<string> list1 = p.GetList1(dt);
                p.WriteToFile(list1, string.Concat(outputFolder, "List1.txt"));

                List<string> list2 = p.GetList2(dt);
                p.WriteToFile(list2, string.Concat(outputFolder, "List2.txt"));

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
                Console.WriteLine("StackTrace : " + ex.StackTrace);
            }
        }

        public DataTable ReadCsvIntoDataTable(string filePath)
        {
            DataTable dtCsv = new DataTable();
            string fulltext;

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    fulltext = sr.ReadToEnd().ToString(); //read full file text  
                    string[] rows = fulltext.Split('\n'); //split full file text into rows  
                    for (int i = 0; i < rows.Count() - 1; i++)
                    {
                        string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < rowValues.Count(); j++)
                                {
                                    dtCsv.Columns.Add(rowValues[j]); //add headers  
                                }
                            }
                            else
                            {
                                DataRow dr = dtCsv.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString();
                                }
                                dtCsv.Rows.Add(dr); //add other rows  
                            }
                        }
                    }
                }
            }

            return dtCsv;
        }

        public List<string> GetList1(DataTable dt)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>(); //to hold firstname/lastname and count of it

            foreach (DataRow dr in dt.Rows)
            {
                string firstName = dr["FirstName"].ToString();
                if (dict.ContainsKey(firstName))
                    dict[firstName] = dict[firstName] + 1;
                else
                    dict.Add(firstName, 1);

                string lastName = dr["LastName"].ToString();
                if (dict.ContainsKey(lastName))
                    dict[lastName] = dict[lastName] + 1;
                else
                    dict.Add(lastName, 1);
            }
                        
            //sort by count descending and name ascending
            var sortedDict = dict.OrderByDescending(e => e.Value).ThenBy(e => e.Key);

            //convert to list and return it
            return sortedDict.Select(pair => string.Concat(pair.Key, ", ", pair.Value)).ToList();
        }

        public List<string> GetList2(DataTable dt)
        {
            //can't use dictionary here because multiple persons can have same address
            //so create a list of instances of an anonymous class which has 2 properties, Street and Address
            List<dynamic> tmpList = new List<object>(); 

            foreach (DataRow dr in dt.Rows)
            {
                string address = dr["Address"].ToString();
                string[] addressParts = address.Split(' '); //to extract street name from address
                tmpList.Add(new { Street = addressParts[1], Address = address });
            }

            tmpList.Sort((o1, o2) => o1.Street.CompareTo(o2.Street)); //sort list by street

            List<string> list = tmpList.Select(o => (string)o.Address).ToList(); //return new list containing only addresses
            return list;
        }

        public void WriteToFile(List<string> list, string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (string line in list)
                {
                    file.WriteLine(line);
                }
            }
        }
    }
}
