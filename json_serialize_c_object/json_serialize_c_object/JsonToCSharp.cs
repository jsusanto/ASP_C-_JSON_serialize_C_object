using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace json_serialize_c_object
{
    public class Prices_TC
    {
        public double small { get; set; }
        public double? medium { get; set; }
        public double? large { get; set; }
        public double? huge { get; set; }
        public double? mega { get; set; }
        public double? ultra { get; set; }
    }

    public class RootObject
    {
        public string drink_name { get; set; }
        public Prices_TC prices { get; set; }
    }

    public class MyTightlyCoupledClass
    {
        public List<RootObject> data;
    }

    /*********************************************************************************************************/
    public class Price
    {
        public string Size { get; set; }

        public float PriceValue { get; set; }
    }

    public class Drink
    {
        public string drink_name { get; set; }

        public List<Price> Prices { get; set; }
    }

    class JsonToCSharp
    {
        static void Main(string[] args)
        {
            string json = @"{
                         'data': [
                              {
                                'drink_name': 'short espresso',
                                'prices': { 'small': '3.0' }
                              },
                              {
                                'drink_name': 'latte',
                                'prices': {
                                  'small': '3.5',
                                  'medium': '4.0',
                                  'large': '4.5'
                                }
                              },
                              {
                                'drink_name': 'flat white',
                                'prices': {
                                  'small': '3.5',
                                  'medium': '4.0',
                                  'large': '4.5'
                                }
                              }
                            ]
                            }";

            MyTightlyCoupledClass ss2 = JsonConvert.DeserializeObject<MyTightlyCoupledClass>(json);
            Console.WriteLine(ss2.data[0].drink_name);
            Console.WriteLine(ss2.data[0].prices.small);

            //******************************************************************************************

            string jsonText = File.ReadAllText(@"E:\ASP_C-_JSON_serialize_C_object\json_serialize_c_object\json_serialize_c_object\data\prices.json");
            JArray jsonArray = JArray.Parse(jsonText);

            List<Drink> drinks = new List<Drink>();

            foreach (JObject jsonArr in jsonArray)
            {
                string drinkName = jsonArr["drink_name"].ToString();
                Drink drink = new Drink() { drink_name = drinkName };
                List<Price> prices = new List<Price>();

                JToken token = jsonArr["prices"];
                foreach (JToken priceToken in token.Children())
                {
                    string size = ((JProperty)priceToken).Name;
                    string price = ((JProperty)priceToken).Value.ToString();
                    Price priceObj = new Price() { Size = size, PriceValue = float.Parse(price) };
                    prices.Add(priceObj);
                }
                drink.Prices = prices;
                drinks.Add(drink);
            }

            Console.WriteLine("############################################################");
            Console.WriteLine("##                   ENVATO ESPRESSO MENU                 ##");
            Console.WriteLine("############################################################");
            Console.WriteLine();
            foreach (var drink in drinks)
            {
                Console.WriteLine();
                Console.WriteLine(drink.drink_name);
                foreach (var drinkPrice in drink.Prices)
                {
                    Console.WriteLine("{0}: {1}", drinkPrice.Size, drinkPrice.PriceValue);

                }
            }
        }
    }
}
