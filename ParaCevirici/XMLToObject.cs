using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ParaCevirici
{
    public static class TranslationXML
    {
        public static T XMLToObject<T>(string xml)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            T obj ;
           
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(typeof(T));
                xmlReader = new XmlTextReader(strReader);
                obj = (T)serializer.Deserialize(xmlReader);
          
                    xmlReader.Close();
               
                    strReader.Close();
          
            return obj;
        }
    }
}
