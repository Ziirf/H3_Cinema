using AutoFixture;
using Cinema.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Cinema.Domain.Models;

namespace Cinema.Test.API
{
    public class CustomerAPI
    {
        [Fact]
        public async Task Get_Should_Return_OK_String()
        {
            var builder = new DbContextOptionsBuilder<CinemaContext>();
            builder.UseInMemoryDatabase("CinemaDBCustomerTesting");
            var context = new CinemaContext(builder.Options);

            var customer = new Customer()
            {
                FirstName = "Nicolai",
                LastName = "Friis"
            };
            context.Add(customer);
            await context.SaveChangesAsync();

            // Arrange  
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var fixture = new Fixture();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fixture.Create<String>()),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            client.BaseAddress = fixture.Create<Uri>();
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
            var controller = new CustomersController(context);
            var result = await controller.GetCustomer(1);

            // Assert  
            httpClientFactory.Verify(f => f.CreateClient(It.IsAny<String>()), Times.Once);

            Assert.NotNull(result);
            //Assert.IsAssignableFrom<OkObjectResult>(result);
            //Assert.IsAssignableFrom<String>((result as OkObjectResult)?.Value);
            //Assert.False(String.IsNullOrWhiteSpace((result as OkObjectResult)?.Value as String));
        }
    }
}
