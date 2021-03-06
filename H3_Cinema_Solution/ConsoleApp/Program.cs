using Cinema.Data;
using Cinema.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static CinemaContext _context;

        static void Main(string[] args)
        {
            // Creates a new instance of the context
            _context = new CinemaContext();

            // Drops the database.
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Populate the database
            new SeedData(_context).PopulateDatabase();
            new SeedRelations(_context).PopulateDatabaseRelation();
        }
    }
}