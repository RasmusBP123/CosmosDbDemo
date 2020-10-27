using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CosmosDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> GetAllPersonsAsync()
        {
            var container = Shared.Client.GetContainer(Shared.DbName, Shared.ContainerName);
            var sql = "SELECT * FROM c";

            var items = container.GetItemQueryIterator<Person>(sql);
            var results = await items.ReadNextAsync();

            return Ok(results);
        }

        [HttpGet("{key}/{value}")]
        public async Task<IActionResult> GetPersonByIdAsync(string key, string value)
        {
            var container = Shared.Client.GetContainer(Shared.DbName, Shared.ContainerName);
            var sql = $"SELECT * FROM c WHERE c.{key} = '{value}'";

            var item = container.GetItemQueryIterator<Person>(sql);
            var results = await item.ReadNextAsync();

            return Ok(results);
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateNewPersons()
        {
            var container = Shared.Client.GetContainer(Shared.DbName, Shared.ContainerName);

            dynamic person = new
            {
                id = Guid.NewGuid(),
                name = "Rasmus",
                status = "in a commited relationship",
                person = "male"
            };

            var myNewPerson = new Person
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Jakob",
                Status = "single and ready to mingle",
                Pets = new List<Pet>
                {
                    new Pet
                    {
                        Name = "Mao Mjavkat",
                        Age = 21,
                        IsCute = true,
                    },
                    new Pet
                    {
                        Name = "Princess Pickles",
                        Age = 5,
                        IsCute = false,
                    }
                }
            };

            await container.CreateItemAsync(person, new PartitionKey("in a commited relationship"));
            var result = await container.CreateItemAsync(myNewPerson, new PartitionKey("single and ready to mingle"));

            return Ok(result);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteItem()
        {
            var container = Shared.Client.GetContainer(Shared.DbName, Shared.ContainerName);
            var result = await container.DeleteItemAsync<dynamic>("a209863a-bd1e-4f9c-8120-bef4580402a9", new PartitionKey("single and ready to mingle"), null, CancellationToken.None);
            return Ok(result);
        }
    }
}
