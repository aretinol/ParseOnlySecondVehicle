using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;


namespace ParseOnlySecondVehicle
{
    class Program
    {
        /// <summary>
        /// Ссылка на страницу сайта
        /// </summary>
        static string m_Link = "https://www.kellysubaru.com/used-inventory/index.htm";


        /// <summary>
        /// Находит во входной строке вторую подстроку, совпадающую с шаблоном регулярного выражения
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="pattern">Шаблон регулярного выражения</param>
        /// <returns></returns>
        static string GetSecondMatches(string input, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(input);
            if (matches.Count >= 2)
                return matches[1].Groups[1].Value;
            else
                return null;
        }


        /// <summary>
        /// Отображаю в консоли VIN, Price и ImgSrc
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WindowWidth = 140;

            string ResultPage = GetHTML(m_Link);

            string VIN = GetSecondMatches(ResultPage, "<dt>VIN: </dt><dd>([\\d\\w]*)</dd>");
            string Price = GetSecondMatches(ResultPage, "<span class='value'\\s?>\\$([\\d,]*)");
            string ImgSrc = GetSecondMatches(ResultPage, "<div class=\"media\">.*?<img src=\"(.+?)\"");

            Console.WriteLine($"VIN = {VIN}");
            Console.WriteLine($"Price = {Price}");
            Console.WriteLine($"ImgSrc = {ImgSrc}");
            Console.Read();
        }


        /// <summary>
        /// Получить исходный HTML код страницы
        /// </summary>
        /// <param name="link">Ссылка на страницу</param>
        /// <returns></returns>
        static string GetHTML(string link)
        {

            string ResultPage;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            request.UserAgent = "UserAgent";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ResultPage = reader.ReadToEnd();
                }
            }
            response.Close();
            return ResultPage;
        }
    }
}
