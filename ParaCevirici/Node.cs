using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ParaCevirici
{
    public class Friends
    {

        public List<FacebookFriend> data { get; set; }
    }

    public class FacebookFriend
    {

        public string id { get; set; }
        public string name { get; set; }
    }
    [Serializable]
    public class Currency
    {
        public int Unit { set; get; }
        
        public string Isim { set; get; }
        
        public string CurrencyName { set; get; }
        public string ForexBuying { set; get; }
        public string ForexSelling { set; get; }
        public string BanknoteBuying { set; get; }
        public string BanknoteSelling { set; get; }
      
        public string CrossRateUSD { set; get; }
        
        public string CrossRateOther { set; get; }


    }
   /* public class Result
    {
        public string name { get; set; }
        public string alpha2_code { get; set; }
        public string alpha3_code { get; set; }
    }

    public class RestResponse
    {
        public List<string> messages { get; set; }
        public List<Result> result { get; set; }
    }

    public class RootObject
    {
        public RestResponse RestResponse { get; set; }
    }*/
      public class RootJson
      {
          public Countries RestResponse { get; set; }
      }
      public class Countries
      {
          public List<string> messages { get; set; }
          public List<Country> result { get; set; }
      }

      public class Country
      {
          public string name { get; set; }
          public string alpha2_code { get; set; }
          public string alpha3_code { get; set; }
      }
}
