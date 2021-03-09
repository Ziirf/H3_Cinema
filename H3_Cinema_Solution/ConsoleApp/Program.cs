using Cinema.Data;

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

            // Used temporarily for generating the Json
            //new GenerateJson(_context).GenerateJsonFiles();

            // Populate the database
            new SeedData(_context).PopulateDatabase();
            new SeedRelations(_context).PopulateDatabaseRelation();

        }

    }
}