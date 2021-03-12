using Cinema.Data;
using Cinema.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    class SeedData
    {
        private CinemaContext _context;
        public SeedData(CinemaContext context)
        {
            _context = context;
        }

        public void PopulateDatabase()
        {
            // Populates all the tables with data from json files.
            _context.AddRange(ReadJsonToList<Crew>("Crew"));
            _context.AddRange(ReadJsonToList<Customer>("Customers"));
            _context.AddRange(ReadJsonToList<Postcode>("Postcodes"));
            _context.AddRange(ReadJsonToList<Genre>("Genres"));
            _context.AddRange(ReadJsonToList<AgeRating>("AgeRatings"));
            _context.AddRange(ReadJsonToList<SeatLocation>("SeatLocation"));
            _context.AddRange(ReadJsonToList<Movie>("Movies"));
            _context.AddRange(ReadJsonToList<Theater>("Theaters"));
            //_context.AddRange(GenerateSeatLocation(20, 30));
            //_context.AddRange(GenerateTheater());
            _context.SaveChanges();

            // Populate the roles in order
            PopulateRoles();
        }

        private void PopulateRoles()
        {
            string[] roles = { "Director", "Screen Writer", "Script Writer", "Actor" };
            foreach (var role in roles)
            {
                _context.Add(new Role() { Title = role });
                _context.SaveChanges();
            }
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


            return seatLocations;
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


        //private List<Seat> GenerateSeats(List<SeatLocation> seatLocations, int rows, int seats)
        //{
        //    var result = seatLocations.Where(x => x.Seat <= seats && x.Row <= rows).ToList();

        //    return result.Select(sl => new Seat() { SeatLocation = sl }).ToList();
        //}

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
    }
}
