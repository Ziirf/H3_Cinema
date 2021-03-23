using System;
using Cinema.Data;
using Cinema.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class GenerateJson
    {
        private CinemaContext _context;
        public GenerateJson(CinemaContext context)
        {
            _context = context;
        }

        public void GenerateJsonFiles()
        {
            //Generate Json files for Database

            ConvertToJsonFile(GenerateTheater(), "Theaters");
            ConvertToJsonFile(GenerateSeatLocation(20, 30), "SeatLocation");

        }

        private async static Task<RootObject> GetMovies(string movieNavn)
        {
            var http = new HttpClient();
            var url = String.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&amp;amp;lon={1}&amp;amp;units=metric", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }


        private List<Movie> MakeMoviesFromAPI(List<Movie> jsonMovies)
        {
            var movies = new List<Movie>();




            return movies;
        }

        private List<T> ReadJsonToList<T>(string file)
        {
            var outputList = new List<T>();

            //using (StreamReader r = new StreamReader($"../../../data/{ file }.json"))
            using (StreamReader r = new StreamReader($"data/{ file }.json"))
            {
                string json = r.ReadToEnd();
                outputList = JsonConvert.DeserializeObject<List<T>>(json);
            }

            return outputList;
        }


        private static bool ConvertToJsonFile<T>(List<T> list, string fileName)
        {
            var json = JsonConvert.SerializeObject(list.ToArray());

            File.WriteAllText($"data/{ fileName }.json", json);

            return true;
        }

        private List<Theater> GenerateTheater()
        {
            List<SeatLocation> seatLocationList = _context.SeatLocations.ToList();

            var theaters = new List<Theater>
            {
                new Theater() { TheaterName = "MovieZilla", Row = 20, SeatNumber = 30},
                new Theater() { TheaterName = "Wax on, wax off", Row = 15, SeatNumber = 30 },
                new Theater() { TheaterName = "Yippee ki-yay", Row = 10, SeatNumber = 20 },
                new Theater() { TheaterName = "To infinity and beyond!", Row = 5, SeatNumber = 20 }
            };

            return theaters;
        }

        private List<SeatLocation> GenerateSeatLocation(int rows, int seats)
        {
            var seatLocations = new List<SeatLocation>();

            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= seats; j++)
                {
                    seatLocations.Add(new SeatLocation() { Row = i, SeatNumber = j });
                }
            }

            seatLocations = seatLocations.OrderBy(x => x.Row).ThenBy(x => x.SeatNumber).ToList();

            return seatLocations;
        }

    }
}
