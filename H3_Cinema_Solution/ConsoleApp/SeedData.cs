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
