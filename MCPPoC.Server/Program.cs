using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace McpHttpEcho
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Log to stderr (don’t pollute stdout if you ever embed this)
            builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

            // MCP server with attribute discovery
            builder.Services
                .AddMcpServer()
                .WithHttpTransport()
            // NOTE: GitHub Copilot seems to get upset if Stdio is also enabled alongside http
                //.WithStdioServerTransport()
                .WithToolsFromAssembly();

            // (Optional) CORS if you’ll call from browser-hosted clients
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("mcp", p => p
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)); // or specify exact origins
            });

            builder.Services.AddMemoryCache();

            var app = builder.Build();

            app.UseCors("mcp");

            // Expose MCP over HTTP/SSE
            app.MapMcp("/mcp"); // provided by ModelContextProtocol.AspNetCore

            // Standard ASP.NET Core hosting
            app.Run();
        }
    }
}
