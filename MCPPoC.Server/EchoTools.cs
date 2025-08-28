using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPPoC.Server
{
    [McpServerToolType] // discoverable by the MCP server
    public class EchoTools
    {
        private readonly ILogger<EchoTools> _logger;

        // Dependencies flow in via DI (ILogger here as an example)
        public EchoTools(ILogger<EchoTools> logger)
        {
            _logger = logger;
        }

        [McpServerTool]
        [Description("Echoes the provided message. Useful for sanity checks and wiring tests.")]
        public string Echo(string message)
        {
            _logger.LogInformation("Echo invoked with: {Message}", message);
            return $"hello {message}";
        }
    }
}
