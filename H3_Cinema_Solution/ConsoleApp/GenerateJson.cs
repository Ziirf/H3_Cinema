using Cinema.Data;
using Cinema.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
