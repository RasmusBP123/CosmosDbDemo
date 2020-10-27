using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CosmosDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> GetAllPersonsAsync()
        {
            var container = Shared.Client.GetContainer(Shared.DbName, Shared.ContainerName);
            var sql = "SELECT * FROM c";

            var items = container.GetItemQueryIterator<dynamic>(sql);
            var results = await items.ReadNextAsync();

            var persons = new List<Person>();

            foreach (var result in results)
            {
                persons.Add(new Person
                {
                    Id = result.id,
                    Name = result.name,
                    Status = result.status
                });
            }

            return Ok(persons);
        }

        [HttpGet("container")]
        public async Task<IActionResult> CreateNewItem()
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
                        Name = "FluffyBalllz",
                        Age = 21,
                        IsCute = true,
                    },
                    new Pet
                    {
                        Name = "Snowflake",
                        Age = 5,
                        IsCute = false,
                    }
                }
            };

            await container.CreateItemAsync(person, new PartitionKey("in a commited relationship"));
            var result = await container.CreateItemAsync(myNewPerson, new PartitionKey("single and ready to mingle"));
            return Ok(result);
        }
    }
}
