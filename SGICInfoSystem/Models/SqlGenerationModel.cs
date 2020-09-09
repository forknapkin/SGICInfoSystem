using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace SGICInfoSystem.Models
{
    public class SqlGenerationModel
    {
        private NameValueCollection form;
        public bool CbNumber { get; set; }
        public string TbNumber { get; set; }
        public bool CbToNumber { get; set; }
        public string TbToNumber { get; set; }
        public List<SqlParameter> SqlParams { get; set; }
        public SqlGenerationModel(NameValueCollection form)
        {
            this.form = form;
            SqlParams = new List<SqlParameter>();
        }
        public string GenerateSqlQuery(string tableName)
        {
            StringBuilder sqlQuery = new StringBuilder("select * from ").Append(tableName).Append(" where ");//27 символов
            
            if (form["CbNumber"].Split(',')[0] == "true")
                sqlQuery.Append(this.NumberChecked());
            if(form["cbrubrika"].Split(',')[0] == "true")
                sqlQuery.Append(this.RubricChecked());      
            if (form["cbauthor"].Split(',')[0] == "true")
                sqlQuery.Append(this.AuthorChecked());
            if (form["cbnaz"].Split(',')[0] == "true")
                sqlQuery.Append(this.TitleChecked());
            if (form["cborg"].Split(',')[0] == "true")
                sqlQuery.Append(this.OrganizationChecked());
            if (form["cbgorod"].Split(',')[0] == "true")
                sqlQuery.Append(this.CityChecked());
            if (form["cbgod"].Split(',')[0] == "true")
                sqlQuery.Append(this.YearChecked());
            if (form["cbslova"].Split(',')[0] == "true")
                sqlQuery.Append(this.KeyWords(form["rbslova"], form["tbslova"], " in ", bool.Parse(form["cbaccuracy"].Split(',')[0])));
            if (form["cbexslova"].Split(',')[0] == "true")
                sqlQuery.Append(this.KeyWords(form["rbexslova"], form["tbexslova"], " not in ", bool.Parse(form["cbaccuracy"].Split(',')[0])));
            if (form["cbreferat"].Split(',')[0] == "true")
                sqlQuery.Append(this.EssayChecked());
            if (form["cbplace"].Split(',')[0] == "true")
                sqlQuery.Append(this.PlaceChecked());

            sqlQuery.Append(" id > 0");
            return sqlQuery.ToString();
        }
        private string NumberChecked()
        {
            StringBuilder sqlPart = new StringBuilder();
            if (form["CbToNumber"].Split(',')[0] != "true")
            {
                string[] numbers = form["TbNumber"].Split(", ".ToCharArray()).Where(n => !string.IsNullOrEmpty(n) && !string.IsNullOrWhiteSpace(n) && !n.Equals(",")).ToArray();

                if (numbers != null && numbers.Length > 0)
                {
                    sqlPart.Append("(NOMER IN(");
                    List<int> intNumbers = new List<int>();
                    for (int i = 0, temp = 0; i < numbers.Length; i++)
                    {
                        if (int.TryParse(numbers[i], out temp))
                            intNumbers.Add(temp);
                    }
                    for (int i = 0; i < intNumbers.Count; i++)
                    {
                        sqlPart.Append("@num").Append(i).Append(",");
                        SqlParams.Add(new SqlParameter(new StringBuilder("num").Append(i).ToString(), intNumbers[i]));
                    }
                    sqlPart.Remove(sqlPart.Length - 1, 1).Append(")) and");
                }
            }
            else
            {
                int from, to;
                if (int.TryParse(form["TbNumber"].Split(", ".ToCharArray())[0], out from) && int.TryParse(form["TbToNumber"].Split(", ".ToCharArray())[0], out to))
                {
                    sqlPart.Append("(nomer between ");
                    sqlPart.Append("@numfrom and @numto) and");
                    SqlParams.Add(new SqlParameter("@numfrom", from));
                    SqlParams.Add(new SqlParameter("@numto", to));
                }
            }
            return sqlPart.ToString();
        }
        private string RubricChecked()
        {
            string sqlPart = "";
            if(!string.IsNullOrEmpty(form["cbrubrika"]))
            {
                //(([t0].[tema1] LIKE @p0) OR ([t0].[tema2] LIKE @p1))             
                //new StringBuilder("%").Append(form["tbgorod"]).Append("%").ToString()));
                sqlPart = "((tema1 like @tema) or (tema2 like @tema)) and";
                SqlParams.Add(new SqlParameter("@tema", new StringBuilder("%").Append(form["tbrubrika"]).Append("%").ToString()));
            }
            return sqlPart;
        }
        private string AuthorChecked()
        {
            string author = form["tbauthor"];
            string sqlPart = "";
            // без параметров
            /*string sqlPart = string.Format("(Aftor LIKE '{0}' OR Aftor " + 
                "LIKE '{0} %' OR Aftor LIKE '% {0} %' OR Aftor LIKE '% {0}' OR Aftor LIKE '{0},' OR " + 
                "Aftor LIKE '{0}, %' OR Aftor LIKE '% {0}, %' OR Aftor LIKE '% {0},') and", author);*/
            //([t0].[Aftor] LIKE 'панченко' OR [Aftor] LIKE 'панченко %' OR [Aftor] LIKE '% панченко %' OR [Aftor] LIKE '% панченко'OR [Aftor] LIKE 'панченко,' OR [Aftor] LIKE 'панченко, %' OR [Aftor] LIKE '% панченко, %' OR [Aftor] LIKE '% панченко,')
            if (!string.IsNullOrEmpty(form["tbauthor"]))
            {
                // параметризованный запрос
                sqlPart = "(Aftor LIKE @author1 OR Aftor LIKE @author2 "
                + "OR Aftor LIKE @author3 OR Aftor LIKE @author4 OR Aftor LIKE @author5 OR "
                + "Aftor LIKE @author6 OR Aftor LIKE @author7 OR Aftor LIKE @author8) and";
                SqlParams.Add(new SqlParameter("@author1", form["tbauthor"]));
                SqlParams.Add(new SqlParameter("@author2", new StringBuilder(form["tbauthor"]).Append(" %").ToString()));
                SqlParams.Add(new SqlParameter("@author3", new StringBuilder("% ").Append(form["tbauthor"]).Append(" %").ToString()));
                SqlParams.Add(new SqlParameter("@author4", new StringBuilder("% ").Append(form["tbauthor"]).ToString()));
                SqlParams.Add(new SqlParameter("@author5", new StringBuilder(form["tbauthor"]).Append(",").ToString()));
                SqlParams.Add(new SqlParameter("@author6", new StringBuilder(form["tbauthor"]).Append(", %").ToString()));
                SqlParams.Add(new SqlParameter("@author7", new StringBuilder("% ").Append(form["tbauthor"]).Append(", %").ToString()));
                SqlParams.Add(new SqlParameter("@author8", new StringBuilder("% ").Append(form["tbauthor"]).Append(",").ToString()));
            }

            return sqlPart;
        }
        private string TitleChecked()
        {
            string title = form["tbnaz"];
            string sqlPart = "";
            if (!string.IsNullOrEmpty(title))
            {
                sqlPart = "(naz LIKE @title) and";
                SqlParams.Add(new SqlParameter("@title", new StringBuilder("%").Append(form["tbnaz"]).Append("%").ToString()));
            }
                return sqlPart;
        }
        private string OrganizationChecked()
        {
            string organization = form["tborg"];
            string sqlPart = "";
            if (!string.IsNullOrEmpty(organization))
            {
                sqlPart = "(org LIKE @org) and";
                SqlParams.Add(new SqlParameter("@org", new StringBuilder("%").Append(form["tborg"]).Append("%").ToString()));
            }
            return sqlPart;
        }
        private string CityChecked()
        {
            string city = form["tbgorod"];
            string sqlPart = "";
            if (!string.IsNullOrEmpty(city))
            {
                sqlPart = "(gorod LIKE @city) and";
                SqlParams.Add(new SqlParameter("@city", new StringBuilder("%").Append(form["tbgorod"]).Append("%").ToString()));
            }
            return sqlPart;
        }
        private string YearChecked()
        {
            string sqlPart = "";

            if (form["cbtogod"].Split(',')[0] != "true")
            {
                int year;
                if(int.TryParse(form["tbgod"], out year))
                {
                    sqlPart = "(god = @year) and";
                    SqlParams.Add(new SqlParameter("@year", year));
                }
            }
            else
            {
                int from, to;
                if (int.TryParse(form["tbgod"], out from) && int.TryParse(form["tbtogod"], out to))
                {
                    sqlPart = "(god between @yearfrom and @yearto) and";
                    SqlParams.Add(new SqlParameter("@yearfrom", from));
                    SqlParams.Add(new SqlParameter("@yearto", to));
                }
            }
            return sqlPart;
        }
        private string KeyWords(string operation, string keyWords, string predicate, bool isAccuracy)
        {
            StringBuilder sqlPart = new StringBuilder(" id ").Append(predicate).Append(" (select id from essays where ( ");
            
            if(operation == "complex")
            {
                keyWords = keyWords.Replace("(", " ( ");
                keyWords = keyWords.Replace(")", " ) ");
                keyWords = keyWords.Replace("&", " and ");
                keyWords = keyWords.Replace("|", " or ");
                string[] array = keyWords.Split(' ');
                array = array.Where(s => s != string.Empty).ToArray();

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == "(" || array[i] == ")" || (array[i] == "and" || array[i] == "or"))
                        sqlPart.Append(" ").Append(array[i]).Append(" ");
                    else if (isAccuracy)
                        sqlPart.Append(" ((slova like '").Append(array[i]).Append(",%') or (slova like '")
                            .Append(array[i]).Append(" %') or (slova like '").Append(array[i]).Append("') or (slova like '% ")
                            .Append(array[i]).Append(",%') or (slova like '% ").Append(array[i]).Append(" %') or (slova like '% ")
                            .Append(array[i]).Append("')) ");
                    else
                        sqlPart.Append(" ((slova like '% ").Append(array[i]).Append("%') OR (slova LIKE '%.")
                            .Append(array[i]).Append("%') or (slova LIKE '").Append(array[i]).Append("%')) ");
                }
            }
            else
            {
                string[] array = ((IEnumerable<string>)keyWords.Split(new char[2] { ' ', ',' }))
                    .Where<string>((Func<string, bool>)(s => s != string.Empty)).ToArray<string>();
                if (isAccuracy)
                {
                    sqlPart.Append("((slova like '").Append(array[0]).Append(",%') or (slova like '")
                            .Append(array[0]).Append(" %') or (slova like '").Append(array[0]).Append("') or (slova like '% ")
                            .Append(array[0]).Append(",%') or (slova like '% ").Append(array[0]).Append(" %') or (slova like '% ")
                            .Append(array[0]).Append("')) ");
                    for (int i = 1; i < array.Length; i++)
                        sqlPart.Append(operation).Append(" ((slova like '").Append(array[i]).Append(",%') or (slova like '")
                            .Append(array[i]).Append(" %') or (slova like '").Append(array[i]).Append("') or (slova like '% ")
                            .Append(array[i]).Append(",%') or (slova like '% ").Append(array[i]).Append(" %') or (slova like '% ")
                            .Append(array[i]).Append("')) ");                   
                }
                else
                {
                    sqlPart.Append("((slova like '% ").Append(array[0]).Append("%') or (slova like '%.")
                            .Append(array[0]).Append("%') or (slova like '").Append(array[0]).Append("%')) ");
                    for (int i = 1; i < array.Length; i++)
                        sqlPart.Append(operation).Append(" ((slova like '% ").Append(array[i]).Append("%') or (slova like '%.")
                            .Append(array[i]).Append("%') or (slova like '").Append(array[i]).Append("%')) ");                    
                }               
            }

            sqlPart.Append(")) and");
            return sqlPart.ToString();
        }

        private string EssayChecked()
        {
            string sqlPart = "";
            if (!string.IsNullOrEmpty(form["cbreferat"]))
            {
                sqlPart = "(referat like @essay) and";
                SqlParams.Add(new SqlParameter("@essay", new StringBuilder("%").Append(form["tbreferat"]).Append("%").ToString()));
            }

            return sqlPart;
        }
        private string PlaceChecked()
        {
            string sqlPart = "";
            if (!string.IsNullOrEmpty(form["cbplace"]))
            {
                sqlPart = "(mesto like @place) and";
                SqlParams.Add(new SqlParameter("@place", new StringBuilder("%").Append(form["tbplace"]).Append("%").ToString()));
            }
            return sqlPart;
        }
    }
}