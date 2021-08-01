using SGICInfoSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReplaceSymbols
{
    class Program
    {
        static string conString = File.ReadAllText("constring.txt");
        static GeologiyaContext db = new GeologiyaContext(conString);
        static void Main(string[] args)
        {
            ReplaceSymbols();
        }
        static void ReplaceSymbols()
        {
            //SqlParameter param1 = new SqlParameter("@param1", "%[j-l|к]-4[0-3]%");
            //var essays = db.Essays.SqlQuery("select * from Essays where slova like @param1", param1);
            //Console.WriteLine(essays.Count());

            //Regex k_4 = new Regex("(к-4[0-3]-[^\\d])", RegexOptions.IgnoreCase);

            Regex romanDigits = new Regex(@"([j-l|к]-4[0-3]-\w+)", RegexOptions.IgnoreCase);
            //Regex arabDigits = new Regex(@"([k]-4[1-2]-\d{1,3})", RegexOptions.IgnoreCase);
            Regex arabDigits = new Regex(@"([k|к]-4[1-2]-[0-9])", RegexOptions.IgnoreCase);
            Regex all = new Regex(@"([j-l|к]-4[0-3])", RegexOptions.IgnoreCase);
            Regex exDefis = new Regex(@"([к]-4[0-3])([^-])", RegexOptions.IgnoreCase);
            Regex exDefisEnd = new Regex(@"([к]-4[0-3]$)", RegexOptions.IgnoreCase);
            var essaysRegex = db.Essays.ToList().Where(e => !string.IsNullOrEmpty(e.Slova)).Where(e => all.IsMatch(e.Slova));

            //foreach (Essay essay in essaysRegex)
            //{
            //    Debug.WriteLine("{0}\t{1}\t{2}", essay.Id, essay.nomer, essay.Slova);
            //    if (exDefis.IsMatch(essay.Slova))
            //    {
            //        foreach (Match match in exDefis.Matches(essay.Slova))
            //        {
            //            Debug.WriteLine(match.Groups[0].Value);
            //        }
            //    }

            //}
            //Debug.WriteLine(essaysRegex.Count());
            int count = 0;
            foreach (Essay essay in essaysRegex)
            {
                string modified = essay.Slova;
                Debug.WriteLine("{0}\t{1}\t{2}", essay.Id, essay.nomer, essay.Slova);
                string temp = "";
                if (modified != null)
                if (romanDigits.IsMatch(modified))
                {
                   
                    foreach (Match match in romanDigits.Matches(modified))
                    {
                        if (arabDigits.IsMatch(match.Groups[0].Value))
                        {
                            Debug.WriteLine("Arab");
                            temp = match.Groups[0].Value;
                            Debug.WriteLine(match.Groups[0].Value);
                            temp = Regex.Replace(temp, "(k)", "К", RegexOptions.IgnoreCase);

                            modified = Regex.Replace(modified, match.Groups[0].Value, temp, RegexOptions.IgnoreCase);

                        }
                        else
                        {
                            Debug.WriteLine("Roman");
                            temp = match.Groups[0].Value;
                            Debug.WriteLine(match.Groups[0].Value);

                            temp = Regex.Replace(temp, "(к)", "K", RegexOptions.IgnoreCase);
                            temp = Regex.Replace(temp, "(х)", "X", RegexOptions.IgnoreCase);
                            temp = Regex.Replace(temp, "(П)", "II", RegexOptions.IgnoreCase);
                            temp = Regex.Replace(temp, "(У)", "V", RegexOptions.IgnoreCase);
                            temp = Regex.Replace(temp, "(Ш)", "III", RegexOptions.IgnoreCase);

                            modified = Regex.Replace(modified, match.Groups[0].Value, temp, RegexOptions.IgnoreCase);
                        }
                    }
                }
                //if (arabDigits.IsMatch(modified))
                //{
                //    Debug.WriteLine("Arab");
                //    foreach (Match match in arabDigits.Matches(modified))
                //    {
                //        temp = match.Groups[0].Value;
                //        Debug.WriteLine(match.Groups[0].Value);
                //        temp = Regex.Replace(temp, "(k)", "К", RegexOptions.IgnoreCase);

                //        modified = Regex.Replace(modified, match.Groups[0].Value, temp, RegexOptions.IgnoreCase);
                //    }
                //}
                if (exDefis.IsMatch(modified))
                {
                    Debug.WriteLine("Defis");
                    foreach (Match match in exDefis.Matches(modified))
                    {
                        Debug.WriteLine(match.Groups[1].Value);

                        temp = match.Groups[1].Value;

                        temp = Regex.Replace(temp, "(к)", "K", RegexOptions.IgnoreCase);

                        modified = Regex.Replace(modified, match.Groups[1].Value, temp, RegexOptions.IgnoreCase);
                    }
                }
                if (exDefisEnd.IsMatch(modified))
                {
                    Debug.WriteLine("End Defis");
                    Debug.WriteLine(exDefisEnd.Matches(modified)[0].Groups[1].Value);
                    temp = exDefisEnd.Matches(modified)[0].Groups[1].Value;
                    temp = Regex.Replace(temp, "(к)", "K", RegexOptions.IgnoreCase);

                    modified = Regex.Replace(modified, exDefisEnd.Matches(modified)[0].Groups[1].Value, temp, RegexOptions.IgnoreCase);
                }
                count++;
                Console.WriteLine(essay.Id + "\t" + count + "\t" + essay.nomer);
                essay.Slova = modified;
                db.Entry(essay).State = System.Data.Entity.EntityState.Modified;

            }

            db.SaveChanges();
            Console.WriteLine(essaysRegex.Count());
            Console.ReadLine();
        }
        
    }
}
