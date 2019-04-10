using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Data;
using NsExcel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace ParaCevirici
{
    public class Car
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int MaximumSpeed { get; set; }
    }
    public class Program
    {
        public static List<Currency> Currencies { get; set; }
        public static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
        static void XmlConverter()
        {
            XmlDocument myxml = new XmlDocument();
            try
            {
                XmlTextReader rdr = new XmlTextReader("http://www.tcmb.gov.tr/kurlar/today.xml");


                // XmlDocument nesnesini yaratıyoruz.
                myxml.Load(rdr);
                // Load metodu ile xml yüklüyoruz

            }
            catch (Exception ex)
            {

            }
            var tarih = myxml.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value.Split(new char[] { '.' });
            XmlNodeList xmlNodeList = myxml.GetElementsByTagName("Currency");
            //Console.Write("_________________________________________________________________");
            //Console.Write("|    Unit        |");
            //Console.Write("     Isim        |");
            //Console.Write("  CurrencyName    |");
            //Console.Write("   ForexBuying    |");
            //Console.Write("   ForexSelling    |");
            //Console.Write(" BanknoteBuying   |");
            //Console.Write("BanknoteSellingling|");
            //Console.Write("   CrossRateUSD   |");
            //Console.Write(" CrossRateOther   |");
            //Console.Write("_________________________________________________________________");
            Currencies = new List<Currency>();
            foreach (XmlNode node in xmlNodeList)
            {
                var item = TranslationXML.XMLToObject<Currency>(node.OuterXml);
                var x = item.Unit;

                Currencies.Add(item);

                //yazdir(item);
                //HEPSİ STRİNG ŞEKLİNDE TANIMLI ÇÜNKÜ SAYI DEĞERLERİNİN NULLABLE OLABİLİRLİĞİ İÇİN ÖNLEM ALINMAMIŞ
                //Console.Write($"    {item.Unit}        ");//< Unit > 1 </ Unit >
                //Console.Write($"     {item.Isim}        ");//< Isim > ABD DOLARI </ Isim >
                //Console.Write($"{item.CurrencyName}");//< CurrencyName > US DOLLAR </ CurrencyName >
                //if (item.ForexBuying != string.Empty)
                //    Console.Write($"        {string.Format("{0:0.00}", float.Parse(item.ForexBuying))}");//< ForexBuying > 4.7908 </ ForexBuying >
                //if (item.ForexSelling != string.Empty)
                //    Console.Write($"        {string.Format("{0:0.00}", float.Parse(item.ForexSelling))}");//< ForexSelling > 4.7994 </ ForexSelling >
                //if (item.BanknoteBuying != string.Empty)
                //    Console.Write($"        {string.Format("{0:0.00}", float.Parse(item.BanknoteBuying))}"); //< BanknoteBuying > 4.7875 </ BanknoteBuying >
                //if (item.BanknoteSelling != string.Empty)
                //    Console.Write($"        {string.Format("{0:0.00}", float.Parse(item.BanknoteSelling))}");//< BanknoteSelling > 4.8066 </ BanknoteSelling >
                //if (item.CrossRateUSD != string.Empty)
                //    Console.Write($"        {string.Format("{0:0.00}", float.Parse(item.CrossRateUSD))}");//< CrossRateUSD />< CrossRateOther />
                //if (item.CrossRateOther != string.Empty)
                //    Console.Write($"        {string.Format("{0:0.00}", float.Parse(item.CrossRateOther))}");//< CrossRateUSD />< CrossRateOther />
                //Console.WriteLine();
            }
        }
        static void JSONConverter()
        {
            string json =
              @"{""data"":[{""id"":""518523721"",""name"":""ftyft""}, {""id"":""527032438"",""name"":""ftyftyf""}, {""id"":""527572047"",""name"":""ftgft""}, {""id"":""531141884"",""name"":""ftftft""}]}";


            Friends facebookFriends = new JavaScriptSerializer().Deserialize<Friends>(json);

            foreach (var item in facebookFriends.data)
            {
                Console.WriteLine("id: {0}, name: {1}", item.id, item.name);
            }

            var jsonData = GET("http://services.groupkt.com/country/get/all");
            RootJson countries = new JavaScriptSerializer().Deserialize<RootJson>(jsonData);
        }

      
        static void Main(string[] args)
        {
            
            //ExportToExcel();
            XmlConverter();
            ExcelUtility.ExportToExcel(Currencies);

            JSONConverter();
            Console.ReadKey();
        }

        public static void ExportToExcel()
        {
            List<Car> cars = new List<Car>()
            {
                new Car {Name = "Toyota", Color = "Red", MaximumSpeed = 195},
                new Car {Name = "Honda", Color = "Blue", MaximumSpeed = 224},
                new Car {Name = "Mazda", Color = "Green", MaximumSpeed = 205}
            };
            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel.Workbooks.Add();

            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;

            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet.Cells[1, "A"] = "Name";
                workSheet.Cells[1, "B"] = "Color";
                workSheet.Cells[1, "C"] = "Maximum speed";

                // ------------------------------------------------
                // Populate sheet with some real data from "cars" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                foreach (Car car in cars)
                {
                    workSheet.Cells[row, "A"] = car.Name;
                    workSheet.Cells[row, "B"] = car.Color;
                    workSheet.Cells[row, "C"] = string.Format("{0} km/h", car.MaximumSpeed);

                    row++;
                }

                // Apply some predefined styles for data to look nicely :)
                workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // Define filename
                string fileName = string.Format(@"{0}\Currency{1}.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DateTime.Now.Month);

                // Save this data as a file
                workSheet.SaveAs(fileName);

                // Display SUCCESS message
                Console.WriteLine(string.Format("The file '{0}' is saved successfully!", fileName));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception",
                    "There was a PROBLEM saving Excel file!\n" + exception.Message);
            }
            finally
            {
                // Quit Excel application
                excel.Quit();

                // Release COM objects (very important!)
                if (excel != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                if (workSheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

                // Empty variables
                excel = null;
                workSheet = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
        }
    }
}
