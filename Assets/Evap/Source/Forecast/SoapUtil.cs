using System;
using System.Xml;
using System.Net;
using System.IO;
using UnityEngine;

namespace Evap
{
    /// <summary>
    /// http://visualwebservice.com/wsdl/www.weather.gov/ndfdXML.wsdl
    /// 
    /// http://graphical.weather.gov/xml/SOAP_server/ndfdXML.htm
    /// </summary>
    public static class SoapUtil
    {

        public static void CallWebService()
        {
            var _url = "http://graphical.weather.gov/xml/SOAP_server/ndfdXMLserver.php";
            //var _action = "http://graphical.weather.gov/xml/DWMLgen/wsdl/ndfdXML.wsdl#LatLonListCityNames";
            var _action = "http://graphical.weather.gov/xml/DWMLgen/wsdl/ndfdXML.wsdl#LatLonListZipCode";
            //var _action = "SOAP:Action";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Debug.Log(soapResult);
            }
        }

        public static string CallWebService(string serviceUrl, string action, XmlDocument soapEnvelopeXml)
        {
            HttpWebRequest webRequest = CreateWebRequest(serviceUrl, action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                return soapResult;
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);
            //webRequest.Headers.Add(@"SOAP:Action");
            webRequest.Headers.Add(String.Format("SOAPAction: \"{0}\"", action));
            webRequest.ContentType = "text/xml;charset=\"UTF-8\"";
            //webRequest.ContentType = "text/xml;charset=\"UTF-8\";action=\""+action+"\"";
            //webRequest.ContentType = "application/soap+xml;charset=UTF-8;action =\""+action + "\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelop = new XmlDocument();

            /*
            soapEnvelop.LoadXml(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <LatLonListCityNames >
                            <displayLevel>1</displayLevel>
                        </LatLonListCityNames >
                    </soap:Body>
                </soap:Envelope>");
            */
            soapEnvelop.LoadXml(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <LatLonListZipCode >
                            <zipCodeList>85716</zipCodeList>
                        </LatLonListZipCode >
                    </soap:Body>
                </soap:Envelope>");
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
