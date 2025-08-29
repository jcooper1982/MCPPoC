using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MCPPoC.Server;

namespace MCPPoC.Server
{
    [McpServerToolType] // discoverable by the MCP server
    public class AnimalTools
    {
        private readonly ILogger<AnimalTools> _logger;
        private readonly IMemoryCache _cache;
        private const string AnimalCacheKey = "Animals";

        public AnimalTools(ILogger<AnimalTools> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        /// <summary>
        /// Upserts an animal in the memory cache.
        /// </summary>
        /// <param name="animal">The animal to upsert.</param>
        /// <returns>A status message indicating success or validation errors.</returns>
        [McpServerTool]
        [Description("Upserts an animal in the memory cache.")]
        public string UpsertAnimal(Animal animal)
        {
            var context = new ValidationContext(animal);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(animal, context, results, true))
            {
                return string.Join("; ", results.Select(r => r.ErrorMessage));
            }

            var animals = _cache.GetOrCreate(AnimalCacheKey, entry => new List<Animal>());
            var existing = animals.FirstOrDefault(a => a.Id == animal.Id);
            if (existing != null)
            {
                existing.Type = animal.Type;
                existing.Name = animal.Name;
                existing.Age = animal.Age;
            }
            else
            {
                animals.Add(animal);
            }
            _cache.Set(AnimalCacheKey, animals);
            return "Animal upserted successfully.";
        }

        /// <summary>
        /// Gets all animals from the memory cache.
        /// </summary>
        /// <returns>A list of all animals currently in the memory cache.</returns>
        [McpServerTool]
        [Description("Gets all animals from the memory cache.")]
        public List<Animal> GetAllAnimals()
        {
            return _cache.Get<List<Animal>>(AnimalCacheKey) ?? new List<Animal>();
        }

        /// <summary>
        /// Searches for an animal by name.
        /// </summary>
        /// <param name="name">The name of the animal to search for.</param>
        /// <returns>The animal with the specified name, or null if not found.</returns>
        [McpServerTool]
        [Description("Searches for an animal by name.")]
        public Animal? GetAnimalByName(string name)
        {
            var animals = _cache.Get<List<Animal>>(AnimalCacheKey) ?? new List<Animal>();
            return animals.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
