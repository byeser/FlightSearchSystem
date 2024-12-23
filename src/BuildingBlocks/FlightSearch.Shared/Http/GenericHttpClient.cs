using System.Text; 
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using FlightSearch.Shared.Http;
using System.Runtime.CompilerServices;

public class GenericHttpClient : IGenericHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GenericHttpClient> _logger;

    public GenericHttpClient(HttpClient httpClient, ILogger<GenericHttpClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async IAsyncEnumerable<T> StreamGetAsync<T>(
        string endpoint,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(
            endpoint,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var streamReader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(streamReader);

        var serializer = new JsonSerializer();

        while (await jsonReader.ReadAsync(cancellationToken))
        {
            if (jsonReader.TokenType == JsonToken.StartArray)
            {
                while (await jsonReader.ReadAsync(cancellationToken) &&
                       jsonReader.TokenType != JsonToken.EndArray)
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;

                    if (jsonReader.TokenType == JsonToken.StartObject)
                    {
                        var item = serializer.Deserialize<T>(jsonReader);
                        if (item != null)
                            yield return item;
                    }
                }
            }
        }
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken)
    {
        var jsonContent = JsonConvert.SerializeObject(request);
        using var content = new StringContent(jsonContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        using var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponse>(responseContent)
            ?? throw new InvalidOperationException("Failed to deserialize response");
    }
}