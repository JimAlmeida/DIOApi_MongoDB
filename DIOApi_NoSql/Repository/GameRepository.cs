using API.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> games;

        public GameRepository(IMongoClient mongoClient, IDatabaseConfigurations databaseConfigurations)
        {
            var database = mongoClient.GetDatabase(databaseConfigurations.DatabaseName);
            games = database.GetCollection<Game>(databaseConfigurations.TableName);
        }

        public async Task<string> Create(Game game)
        {
            await games.InsertOneAsync(game);

            return game.Id;
        }

        public async Task<bool> Delete(string objectId)
        {
            var deleteResult = await games.DeleteOneAsync(game => game.Id == objectId);
            return deleteResult.DeletedCount == 1;
        }

        public async Task<IEnumerable<Game>> Get()
        {
            var myGames = await games.Find(_ => true).ToListAsync();
            return myGames;
        }

        public async Task<Game> Get(string objectId)
        {
            var myGames = await games.Find(game => game.Id == objectId).FirstOrDefaultAsync();
            return myGames;
        }

        public async Task<IEnumerable<Game>> GetByName(string gameName)
        {
            var myGames = await games.Find(game => game.Name == gameName).ToListAsync();
            return myGames;
        }

        public async Task<bool> Update(string objectId, Game car)
        {
            var updateResult = await games.ReplaceOneAsync(game => game.Id == objectId, car);
            return updateResult.ModifiedCount == 1;
        }
    }
}
