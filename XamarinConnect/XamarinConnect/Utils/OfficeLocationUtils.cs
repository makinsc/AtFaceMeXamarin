using System;
using System.Linq;

namespace ATFaceME.Xamarin.Core.Utils
{
    public class OfficeLocationUtils
    {
        public const string DEFAULT_RGB_COLOR = "#000000";
        private static string[] officeLocations = new string[]
        {
            "Todas",
            "Las Rozas",
            "Acanto",
            "Cádiz",
            "Barcelona",
            "A Coruña"
        };

        private static string ConvertToLocation(string _officeLocation)
        {
            if (officeLocations.Contains(_officeLocation))
            {
                return _officeLocation;
            }
            else
            {
                foreach (string office in officeLocations)
                {
                    if (_officeLocation.Contains(office))
                    {
                        return office;
                    }
                }
                return "";
            }
        }

        public static string[] OfficeLocations
        {
            get { return officeLocations; }
        }       

        public static string GetRGBColorByOfficeLocation(string _officeLocation)
        {
            switch (ConvertToLocation(_officeLocation))
            {
                case "Las Rozas":
                    return "#CC0000";
                case "Acanto":
                    return "#AAA000";
                case "Cádiz":
                    return "#0000CC";
                case "Barcelona":
                    return "#CC00CC";
                case "A Coruña":
                    return "#00AA00";
                default:
                    return DEFAULT_RGB_COLOR;
            }
        }
    }
}
