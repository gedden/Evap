using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

public static class EvapUtil
{
    /*
    private static float[,] LOOKUP = new float[,] 
    { 
        { 2,    5,  10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80 }, 
        { 3, 4 }, 
        { 5, 6 }, 
        { 7, 8 }
    };
    */
    // http://www.haveacoolday.com/docs/default-document-library/2011/04/26/Useful%20Evaporative%20Cooling%20Formulas%20ATEC%202011.pdf?Status=Master
    public static float GetEfficiency(float dbIn, float dbOut, float wbIn)
    {
        return (dbIn - dbOut) / (dbIn - wbIn);
        // e = direct evaporative cooling saturation efficiency (%)
        // {\displaystyle T_{ e,db} }  = entering air dry-bulb temperature(°C)
        // {\displaystyle T_{ l,db} }  = leaving air dry-bulb temperature(°C)
        // {\displaystyle T_{ e,wb} }  = entering air wet-bulb temperature(°C)
    }

    public static string Test()
    {
        //var url = "http://graphical.weather.gov/xml/DWMLgen/wsdl/ndfdXML.wsdl#LatLonListCityNames";
        var url = "http://graphical.weather.gov/xml/DWMLgen/wsdl/ndfdXML.wsdl#LatLonListZipCode";
        
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "text/xml";
        request.Method = "POST";

        var response = request.GetResponse();

        var dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        var reader = new StreamReader(dataStream);
        // Read the content.
        string responseFromServer = reader.ReadToEnd();

        Debug.Log(responseFromServer);
        return "ok";
    }
}
