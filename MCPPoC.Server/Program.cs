using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MCPPoC.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            // Verbose console logging to STDERR (handy for MCP STDIO debugging)
            builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

            // Register MCP server + STDIO transport
            // Auto-discovers tools/prompts/resources in this assembly via attributes
            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithToolsFromAssembly();

            builder.Services.AddMemoryCache();

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}
