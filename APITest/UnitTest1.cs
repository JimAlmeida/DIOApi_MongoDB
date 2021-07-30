using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using API;
using System.Net.Http;
using API.DTO;
using Newtonsoft.Json;
using System.Text.Encodings;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using APITest.Orderers;

namespace APITest
{
    [TestCaseOrderer("APITest.Orderers.AlphabeticalOrderer", "APITest")]
    public class APIControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        private string _docId;
        public APIControllerTest(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _factory = webApplicationFactory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public void CRUDTest()
        {
            //Arrange
            Game testGame = new()
            {
                Name = "Squad",
                Description = "MILSIM Tac Shooter",
                Genre = "FPS",
                Year = 2020,
                Publisher = "Paradox",
                Platforms = "PC"
            };

            Game updateGame = new()
            {
                Name = "Squad",
                Description = "MILSIM Tac Shooter with elements of PTSD",
                Genre = "FPS",
                Year = 2021,
                Publisher = "Paradox",
                Platforms = "PC"
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(testGame), Encoding.UTF8, "application/json");


            //Act
            //Create
            var postResponse = _client.PostAsync("/api/v1/Game", content).GetAwaiter().GetResult();
            //Read
            var getResponse = _client.GetAsync("/api/v1/Game/search?name=Squad").GetAwaiter().GetResult();
            var queriedGame = processGetResponse(getResponse);
            var gameId = queriedGame.Id;
            //Update
            updateGame.Id = gameId;
            HttpContent payload = new StringContent(JsonConvert.SerializeObject(updateGame), Encoding.UTF8, "application/json");
            var putResponse = _client.PutAsync($"/api/v1/Game/{gameId}", payload).GetAwaiter().GetResult();
            //Delete
            var deleteResponse = _client.DeleteAsync($"/api/v1/Game/{gameId}").GetAwaiter().GetResult();
            var searchDeletedGameResponse = _client.GetAsync($"/api/v1/Game/{gameId}").GetAwaiter().GetResult();


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, searchDeletedGameResponse.StatusCode);

            Assert.Equal(testGame.Name, queriedGame.Name);
            Assert.Equal(testGame.Description, queriedGame.Description);
            Assert.Equal(testGame.Genre, queriedGame.Genre);
            Assert.Equal(testGame.Year, queriedGame.Year);
            Assert.Equal(testGame.Publisher, queriedGame.Publisher);
            Assert.Equal(testGame.Platforms, queriedGame.Platforms);

        }

        private Game processGetResponse(HttpResponseMessage responseMessage)
        {
            var jsonContent = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var queryResults = JsonConvert.DeserializeObject<List<Game>>(jsonContent);
            return queryResults.FirstOrDefault();
        }
    }
}
