using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;
using System.Text;

namespace FlightSearch.Gateway.Startup;

public class SwaggerMergeService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWebHostEnvironment _environment;
    private readonly string _swaggerPath;

    public SwaggerMergeService(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _environment = environment;
        _swaggerPath = Path.Combine(_environment.WebRootPath, "swagger", "v1", "swagger.json");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var swaggerUrls = new Dictionary<string, string>
        {
            { "FlightSearch", "http://localhost:5001/swagger/v1/swagger.json" },
            { "Booking", "http://localhost:5004/swagger/v1/swagger.json" }
        };

        var openApiDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Title = "Gateway API",
                Version = "v1"
            },
            Paths = new OpenApiPaths(),
            Components = new OpenApiComponents()
        };

        using var client = _httpClientFactory.CreateClient();

        foreach (var url in swaggerUrls)
        {
            try
            {
                var response = await client.GetStringAsync(url.Value, cancellationToken);
                var serviceDoc = new OpenApiStringReader().Read(response, out var diagnostic);

                foreach (var path in serviceDoc.Paths)
                {
                    openApiDoc.Paths.Add($"/{url.Key.ToLower()}{path.Key}", path.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error merging swagger from {url.Key}: {ex.Message}");
            }
        }

        // JSON olarak serialize et
        var json = SerializeOpenApiDocToJson(openApiDoc);
        Directory.CreateDirectory(Path.GetDirectoryName(_swaggerPath));
        await File.WriteAllTextAsync(_swaggerPath, json, cancellationToken);
    }

    private string SerializeOpenApiDocToJson(OpenApiDocument document)
    {
        using var stringWriter = new StringWriter();
        var jsonWriter = new OpenApiJsonWriter(stringWriter);
        document.SerializeAsV3(jsonWriter);
        return stringWriter.ToString();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}