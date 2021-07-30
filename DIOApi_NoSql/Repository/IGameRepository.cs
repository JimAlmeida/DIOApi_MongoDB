using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using MongoDB.Bson;

namespace API.Repository
{
    public interface IGameRepository
    {
        // Create
        Task<string> Create(Game car);

        // Read
        Task<IEnumerable<Game>> Get();
        Task<Game> Get(string objectId);
        Task<IEnumerable<Game>> GetByName(string gameName);

        // Update
        Task<bool> Update(string objectId, Game car);

        // Delete
        Task<bool> Delete(string objectId);
    }
}
