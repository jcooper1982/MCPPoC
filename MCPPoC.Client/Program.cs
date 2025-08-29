using System;
using System.Text.Json;
using System.Threading.Tasks;
using ModelContextProtocol.Client;

namespace McpSpecDump
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Adjust this path to your Echo server project
            var projectPath = @"C:/repos/MCPPoC/MCPPoC.Server/MCPPoC.Server.csproj";

            var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
            {
                Name = "mcp-poc-tools",
                Command = "dotnet",
                Arguments = new[] { "run", "--project", projectPath }
            });

            var client = await McpClientFactory.CreateAsync(clientTransport);

            try
            {
                // Tools are part of the MCP spec
                var tools = await client.ListToolsAsync();

                var json = JsonSerializer.Serialize(tools, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Console.WriteLine("=== MCP Server Tools ===");
                Console.WriteLine(json);

                // If you want to see prompts/resources too:
                // var prompts = await client.ListPromptsAsync();
                // var resources = await client.ListResourcesAsync();
            }
            finally
            {
                // Explicit disposal to shut down the transport cleanly
                await client.DisposeAsync();
            }
        }
    }
}