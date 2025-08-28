namespace MCPPoC.Server
{
    public class Animal
    {
        public string Id { get; set; } = string.Empty;
        public AnimalType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
