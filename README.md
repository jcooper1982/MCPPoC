MCP Server POC

This repo contains a PoC that demonstrates how MCP works.  

I've created an MCP Server app that performs some CRUD like operations against an in memory cache.  You will need to run this app, and then you can use the .http file to make requests against the MCP app such as checking available tools, adding/reading items in cache etc...

There is also an MCP Client app that demonstrates the .NET MCP Client SDK in action.  The SDK only appears to support the stdio protocol at present, which I've commented out in the server app but you can easily toggle off http support and enable the stdio support in the server if you want to test out the client.  Note I have an absolute file path in the client so if you want to run this on your local you will want to pay attention to this.

Most interestingly, you can make the MCP Server available to GitHub Copilot Agent via the .mcp.json file.  Since the binding in the file is http based you will need to run the server (ensuring http is enabled) first.  You will then need to enable the tools in GitHub Copilot agent and you can start making promps that operate against the MCP Server such as "Get me a list of animals", "Get me an animal named Paris", "Upsert an animal with Id 1, name Paris, type Cat, and Age 4".

Very cool stuff!  

Note that the MCP Server is not secure, so don't run this in a production environment or expose it to the internet.  It's just a PoC to demonstrate how MCP works.