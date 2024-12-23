
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FlightSearch.Gateway.Startup;
public class SwaggerEndpointFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Paths = new OpenApiPaths();

        // 1. Flight Search Endpoints
        AddPathWithRequestBody(swaggerDoc, "/api/flight/search", "Flight Search API",
            new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string> { "origin", "destination", "departureDate", "passengerCount" },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["origin"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("IST") },
                    ["destination"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("LHR") },
                    ["departureDate"] = new OpenApiSchema { Type = "string", Format = "date-time", Example = new OpenApiString("2024-11-14T00:00:00Z") },
                    ["returnDate"] = new OpenApiSchema { Type = "string", Format = "date-time", Nullable = true },
                    ["passengerCount"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) }
                }
            });

        // 2. Booking Endpoints
        AddPathWithRequestBody(swaggerDoc, "/api/booking/create", "Booking API",
            new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string> { "flightNumber", "passengers" },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["flightNumber"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("HH123") },
                    ["passengers"] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string> { "firstName", "lastName", "dateOfBirth" },
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["firstName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("John") },
                                ["lastName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Doe") },
                                ["dateOfBirth"] = new OpenApiSchema { Type = "string", Format = "date", Example = new OpenApiString("1990-01-01") },
                                ["passportNumber"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("AB123456"), Nullable = true }
                            }
                        }
                    }
                }
            });

        AddPathWithParameter(swaggerDoc, "/api/booking/{id}", "Booking API", OperationType.Get,
            new OpenApiParameter
            {
                Name = "id",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
            });

        AddPathWithParameter(swaggerDoc, "/api/booking/{id}/cancel", "Booking API", OperationType.Put,
            new OpenApiParameter
            {
                Name = "id",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
            }); 
    }

    private void AddPathWithRequestBody(OpenApiDocument document, string path, string tag, OpenApiSchema requestSchema)
    {
        document.Paths.Add(path, new OpenApiPathItem
        {
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [OperationType.Post] = new OpenApiOperation
                {
                    Tags = new List<OpenApiTag> { new OpenApiTag { Name = tag } },
                    RequestBody = new OpenApiRequestBody
                    {
                        Required = true,
                        Content =
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = requestSchema
                            }
                        }
                    },
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse { Description = "Success" },
                        ["400"] = new OpenApiResponse { Description = "Bad Request" },
                        ["404"] = new OpenApiResponse { Description = "Not Found" }
                    }
                }
            }
        });
    }

    private void AddPathWithParameter(OpenApiDocument document, string path, string tag, OperationType operation, OpenApiParameter parameter)
    {
        document.Paths.Add(path, new OpenApiPathItem
        {
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [operation] = new OpenApiOperation
                {
                    Tags = new List<OpenApiTag> { new OpenApiTag { Name = tag } },
                    Parameters = new List<OpenApiParameter> { parameter },
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse { Description = "Success" },
                        ["404"] = new OpenApiResponse { Description = "Not Found" }
                    }
                }
            }
        });
    }
}