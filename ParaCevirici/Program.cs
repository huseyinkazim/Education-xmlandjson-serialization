using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace ParaCevirici
{
   
    public class Program
    {
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
            foreach (XmlNode node in xmlNodeList)
            {
                var item = TranslationXML.XMLToObject<Currency>(node.OuterXml);
                var x = item.Unit;
                //yazdir(item);
                //HEPSİ STRİNG ŞEKLİNDE TANIMLI ÇÜNKÜ SAYI DEĞERLERİNİN NULLABLE OLABİLİRLİĞİ İÇİN ÖNLEM ALINMAMIŞ
                Console.Write($"Unit:{item.Unit}");//< Unit > 1 </ Unit >
                Console.Write($"Isim:{item.Isim}");//< Isim > ABD DOLARI </ Isim >
                Console.Write($"CurrencyName:{item.CurrencyName}");//< CurrencyName > US DOLLAR </ CurrencyName >
                if (item.ForexBuying != string.Empty)
                    Console.Write($"ForexBuying:{float.Parse(item.ForexBuying)}");//< ForexBuying > 4.7908 </ ForexBuying >
                if (item.ForexSelling != string.Empty)
                    Console.Write($"ForexSelling:{float.Parse(item.ForexSelling)}");//< ForexSelling > 4.7994 </ ForexSelling >
                if (item.BanknoteBuying != string.Empty)
                    Console.Write($"BanknoteBuying:{float.Parse(item.BanknoteBuying)}"); //< BanknoteBuying > 4.7875 </ BanknoteBuying >
                if (item.BanknoteSelling != string.Empty)
                    Console.Write($"BanknoteSelling:{float.Parse(item.BanknoteSelling)}");//< BanknoteSelling > 4.8066 </ BanknoteSelling >
                if (item.CrossRateUSD != string.Empty)
                    Console.Write($"CrossRateUSD:{float.Parse(item.CrossRateUSD)}");//< CrossRateUSD />< CrossRateOther />
                if (item.CrossRateOther != string.Empty)
                    Console.Write($"CrossRateOther:{float.Parse(item.CrossRateOther)}");//< CrossRateUSD />< CrossRateOther />
                Console.WriteLine();
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

            var jsonData=GET("http://services.groupkt.com/country/get/all");
            RootJson countries = new JavaScriptSerializer().Deserialize<RootJson>(jsonData);
        }
        static void Main(string[] args)
        {
           // XmlConverter();
            JSONConverter();
            Console.ReadKey();
        }
    }
}
