using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Evap
{
    /// <summary>
    /// http://visualwebservice.com/wsdl/www.weather.gov/ndfdXML.wsdl
    /// 
    /// http://graphical.weather.gov/xml/SOAP_server/ndfdXML.htm
    /// </summary>
    public static class ForecastService
    {
        private static readonly string SERVICE_URL = "http://graphical.weather.gov/xml/SOAP_server/ndfdXMLserver.php";

        public static void GetCities(Action<List<City>> callback)
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <LatLonListCityNames >
                            <displayLevel>1</displayLevel>
                        </LatLonListCityNames >
                    </soap:Body>
                </soap:Envelope>");


            var reply = SoapUtil.CallWebService(SERVICE_URL, "http://graphical.weather.gov/xml/DWMLgen/wsdl/ndfdXML.wsdl#LatLonListCityNames", soapEnvelop);
            var result = ParceCities(reply);
            callback(result);
        }

        private static List<City> ParceCities(string data)
        {
            List<City> result = new List<City>();

            using (XmlReader reader = XmlReader.Create(new StringReader(data)))
            {
                reader.MoveToContent();

                Debug.Log(reader.Name);

                var r= reader.ReadToDescendant("listLatLonOut");

                
                var content = reader.ReadElementContentAsString();

                var coordsList = GetContent("latLonList", content).Split(' ');
                var nameList = GetContent("cityNameList", content).Split('|');

                for( int x=0;x<coordsList.Length;x++ )
                {
                    var city = new City(nameList[x], coordsList[x]);
                    result.Add(city);

                    Debug.Log(city.Name + " / " + city.GPSCoords);
                }
            }

            return result;
        }

        private static string GetContent(string tag, string data)
        {
            var start = data.IndexOf(tag) + tag.Length + 1;
            var end = data.LastIndexOf("</" + tag);

            return data.Substring(start, end - start);
        }


        public static void GetGPSByZip()
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <LatLonListZipCode >
                            <zipCodeList>85716</zipCodeList>
                        </LatLonListZipCode >
                    </soap:Body>
                </soap:Envelope>");


            var reply = SoapUtil.CallWebService(SERVICE_URL, "http://graphical.weather.gov/xml/DWMLgen/wsdl/ndfdXML.wsdl#LatLonListCityNames", soapEnvelop);
        }
    }
}
