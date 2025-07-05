# region Menu Started Here

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using static System.Net.WebRequestMethods;

Console.WriteLine("Welcome to API Consume Process!");
Console.WriteLine();
Console.WriteLine("### Select the action you want to take ###");
Console.WriteLine();
Console.WriteLine("1 - Show the City List");
Console.WriteLine("2 - Show the City and Weather List");
Console.WriteLine("3 - Add a new City to List");
Console.WriteLine("4 - Delete City from List");
Console.WriteLine("5 - Update City from List");
Console.WriteLine("6 - List Cities by ID");
Console.WriteLine();

#endregion


string number;

Console.WriteLine("Your Choice: ");
number = Console.ReadLine();
Console.WriteLine();

if (number == "1") // List Cities
{
    string url = "http://localhost:5111/api/Weathers";
    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        JArray jArray = JArray.Parse(responseBody);
        foreach (var item in jArray)
        {
            string cityName = item["cityName"].ToString();
            Console.WriteLine($"City: {cityName}");
        }
    }
}
if (number == "2") // List Cities with Country and Weather
{
    string url = "http://localhost:5111/api/Weathers";
    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        JArray jArray = JArray.Parse(responseBody);
        foreach (var item in jArray)
        {
            string cityName = item["cityName"].ToString();
            string temp = item["temp"].ToString();
            string country = item["country"].ToString();
            Console.WriteLine(cityName + " - " + country + " --> " + temp + " C Degrees");
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}
if (number == "3") // Add a new City
{
    Console.WriteLine("### Add a Data ###");
    Console.WriteLine();
    string cityName, country, detail;
    decimal temp;

    Console.Write("City Name: ");
    cityName = Console.ReadLine();

    Console.Write("CountryName: ");
    country = Console.ReadLine();

    Console.Write("Weather Detail: ");
    detail = Console.ReadLine();

    Console.Write("Temptuare: ");
    temp = decimal.Parse(Console.ReadLine());

    string url = "http://localhost:5111/api/Weathers";

    var newWeatherCity = new
    {
        CityName = cityName,
        Country = country,
        Detail = detail,
        Temp = temp
    };

    using (HttpClient client = new HttpClient()) // Yeni veriyi JSON formatından stringe donusturduk.
    {
        string json = JsonConvert.SerializeObject(newWeatherCity); // yukarıdan gelen degerleri JSON donusturduk
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }
}
if (number == "4") // Id ye göre Sehir Silme
{
    string url = "http://localhost:5111/api/Weathers?id=";

    Console.Write("ID Value of the city to be deleted: ");
    int id = int.Parse(Console.ReadLine());

    using(HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.DeleteAsync(url + id);
        response.EnsureSuccessStatusCode();
    }
}
if(number == "5") // Veri Guncelleme islemi
{
    Console.WriteLine("### Update a Data ###");
    Console.WriteLine();
    string cityName, country, detail;
    decimal temp;
    int cityId;

    Console.Write("City Name: ");
    cityName = Console.ReadLine();

    Console.Write("CountryName: ");
    country = Console.ReadLine();

    Console.Write("Weather Detail: ");
    detail = Console.ReadLine();

    Console.Write("Temptuare: ");
    temp = decimal.Parse(Console.ReadLine());

    Console.Write("City ID: ");
    cityId = int.Parse(Console.ReadLine());

    string url = "http://localhost:5111/api/Weathers";

    var updateWeatherValues = new 
    {
        CityId = cityId,
        cityName = cityName,
        Country = country,
        Detail = detail,
        Temp = temp
    };

    using(HttpClient client = new HttpClient())
    {
        string json = JsonConvert.SerializeObject(updateWeatherValues);
        StringContent content = new StringContent(json,Encoding.UTF8,"application/json");
        HttpResponseMessage response = await client.PutAsync(url,content);
        response.EnsureSuccessStatusCode();
    }
}
if(number == "6") // ID si girilen sehrin bilgilerini getirme
{
    string url = "http://localhost:5111/api/Weathers/GetByIdWeatherCity?id=";

    Console.Write("List City by ID: ");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine();

    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync(url + id);
        response.EnsureSuccessStatusCode(); // Başarılı bir yanıt döndüğünden emin olduk.
        string responseBody = await response.Content.ReadAsStringAsync(); // Gelen değeri string olarak okuyup ona göre atama yaparız.
        JObject weatherCityObject = JObject.Parse(responseBody);// şimdi JSON verisini Parse etmemiz gerekiyor. 

        // şimdi atamalarını yaparız. ID okumaya gerek yok çünkü zaten biz gönderdik. diğer sütunları okuruz.
        string cityName = weatherCityObject["cityName"].ToString();
        string detail = weatherCityObject["detail"].ToString();
        string country = weatherCityObject["country"].ToString();
        decimal temp = decimal.Parse(weatherCityObject["temp"].ToString());

        Console.WriteLine("Info's about the entered ID: ");
        Console.WriteLine();
        Console.WriteLine("City: " + cityName + " Country: " + country + " Temp: " + temp + " Details: " + detail);

    }
}

Console.Read();