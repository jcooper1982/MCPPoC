using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [McpServerTool]
        [Description("Upserts an animal in the memory cache.")]
        public string UpsertAnimal(string id, AnimalType type, string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 30)
                return "Invalid name. Must be 2-30 characters.";
            // Allow letters and spaces, but no digits or special characters
            if (!name.All(c => char.IsLetter(c) || c == ' '))
                return "Invalid name. Only letters and spaces allowed.";
            // Prevent multiple consecutive spaces and leading/trailing spaces
            if (name.Contains("  ") || name.StartsWith(" ") || name.EndsWith(" "))
                return "Invalid name. No leading/trailing or consecutive spaces allowed.";
            if (age < 0 || age > 40)
                return "Invalid age. Must be between 0 and 40.";

            var animals = _cache.GetOrCreate(AnimalCacheKey, entry => new List<Animal>());
            var existing = animals.FirstOrDefault(a => a.Id == id);
            if (existing != null)
            {
                existing.Type = type;
                existing.Name = name;
                existing.Age = age;
            }
            else
            {
                animals.Add(new Animal { Id = id, Type = type, Name = name, Age = age });
            }
            _cache.Set(AnimalCacheKey, animals);
            return "Animal upserted successfully.";
        }

        [McpServerTool]
        [Description("Gets all animals from the memory cache.")]
        public List<Animal> GetAllAnimals()
        {
            return _cache.Get<List<Animal>>(AnimalCacheKey) ?? new List<Animal>();
        }

        [McpServerTool]
        [Description("Searches for an animal by name.")]
        public Animal? GetAnimalByName(string name)
        {
            var animals = _cache.Get<List<Animal>>(AnimalCacheKey) ?? new List<Animal>();
            return animals.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
