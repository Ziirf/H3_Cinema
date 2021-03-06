using Cinema.Data;
using Cinema.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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
            _context.AddRange(ReadJsonToList<Theater>("Theater"));
            _context.AddRange(ReadJsonToList<SeatLocation>("SeatLocation"));
            _context.AddRange(ReadJsonToList<Movie>("Movies"));
            _context.AddRange(GenerateSeatLocation(20, 30));
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
            var list = new List<SeatLocation>();

            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= seats; j++)
                {
                    list.Add(new SeatLocation() { Row = i, Seat = j });
                }
            }

            return list;
        }

        private List<T> ReadJsonToList<T>(string file)
        {
            List<T> list = new List<T>();

            using (StreamReader r = new StreamReader($"../../../data/{ file }.json"))
            {
                string json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<List<T>>(json);
            }

            return list;
        }
    }
}
