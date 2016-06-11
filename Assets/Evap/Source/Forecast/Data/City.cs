using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evap
{
    public class City
    {
        public City(string name, string coords)
        {
            this.Name = name;
            this.GPSCoords = coords;
        }
        public string GPSCoords { get; private set; }
        public string Name { get; private set; }
    }


}
