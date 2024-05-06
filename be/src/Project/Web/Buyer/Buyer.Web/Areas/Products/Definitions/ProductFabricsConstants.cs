using System.Globalization;

namespace Buyer.Web.Areas.Products.Definitions
{
    public static class ProductFabricsConstants
    {
        public static string GetPrimaryFabircKey(string language)
        {
            string key;

            if (language == "pl")
            {
                key = "tkaninaPodstawowa";
            }
            else if (language == "en")
            {
                key = "primaryFabrics";
            }
            else
            {
                key = "primarstoffe";
            }

            return key;
        }

        public static string GetSecondaryFabircKey(string language)
        {
            string key;

            if (language == "pl")
            {
                key = "tkaninaDodatkowa";
            }
            else if (language == "en")
            {
                key = "secondaryFabrics";
            }
            else
            {
                key = "sekundarefarbe";
            }

            return key;
        }
    }
}