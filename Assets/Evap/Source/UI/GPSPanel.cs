using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Evap
{
    public class GPSPanel : MonoBehaviour
    {
        public Dropdown CityDropdown;
        public Text GPSLabel;
        private List<City> cities;

        public void Start()
        {
            // Debug.text = EvapUtil.Test();
            // SoapUtil.CallWebService();
            ForecastService.GetCities(OnRecieveCities);
        }

        private void PopulateDropdown()
        {
            CityDropdown.ClearOptions();

            var options = new List<string>();
            foreach (var city in cities)
                options.Add(city.Name);
            CityDropdown.AddOptions(options);
            UpdateGPSLabel(); 
        }

        public City Selected
        {
            get
            {
                return cities[CityDropdown.value];
            }
            set
            {
                for( var i = 0;i<cities.Count; i++ )
                {
                    if (cities[i] == value && CityDropdown.value != i)
                    {
                        CityDropdown.value = i;
                    }
                }

                UpdateGPSLabel();
            }
        }

        public void UpdateGPSLabel()
        {
            // Update the GPS Display
            GPSLabel.text = Selected.GPSCoords;
        }


        public void OnRecieveCities(List<City> cities)
        {
            this.cities = cities;
            PopulateDropdown();
        }
    }
}
