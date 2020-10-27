using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDemo
{
    public class Person
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "pets")]
        public List<Pet> Pets { get; set; }
    }

    public class Pet
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isCute")]
        public bool IsCute { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
    }
}
