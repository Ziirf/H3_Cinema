using System;
using System.Linq;
using Cinema.Domain.Models;
using Xunit;

namespace Cinema.Test.Models
{
    public class MovieModelTest
    {
        [Theory]
        [InlineData(null, 0, 0, 3)]         // Error from title, runtime and rating missing.
        [InlineData("Title", 0, 0, 2)]      // Error from runtime and rating missing.
        [InlineData("Title", 120, 0, 1)]    // Error from rating missing.
        [InlineData("Title", 120, 5, 0)]    // No errors, all required fields have been filled.
        public void CheckForRequirements(string title, int runtime, float rating, int errors)
        {
            var movie = new Movie()
            {
                Title = title,
                Runtime = runtime,
                Rating = rating,
            };

            var results = Helper.Validate(movie);

            Assert.Equal(errors, results.Count);
        }

        [Theory]
        [InlineData(0, false)]      // Too low
        [InlineData(601, false)]    // Too high
        [InlineData(1, true)]       // Minimum
        [InlineData(600, true)]     // Maximum
        public void CheckingRangeOfRuntime(int runtime, bool success)
        {
            var movie = new Movie()
            {
                Runtime = runtime,
            };

            var result = Helper.HasError(movie, "Runtime");

            Assert.NotEqual(success, result);
        }

        [Theory]
        [InlineData(0, false)]      // Too low
        [InlineData(0.9, false)]    // Just below minimum
        [InlineData(10.1, false)]   // Just above maximum
        [InlineData(1, true)]       // Minimum
        [InlineData(10, true)]      // Maximum
        public void CheckingRangeOfRating(float rating, bool success)
        {
            var movie = new Movie()
            {
                Rating = rating,
            };

            var result = Helper.HasError(movie, "Rating");

            Assert.NotEqual(success, result);
        }
    }
}
