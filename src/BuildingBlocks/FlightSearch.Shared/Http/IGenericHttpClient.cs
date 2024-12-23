using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Shared.Http;
public interface IGenericHttpClient
{
    IAsyncEnumerable<T> StreamGetAsync<T>(string endpoint, CancellationToken cancellationToken);
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken);
}