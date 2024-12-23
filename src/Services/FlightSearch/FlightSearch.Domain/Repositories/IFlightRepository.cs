using FlightSearch.Domain.Entities;
using FlightSearch.Domain.Specifications;
using FlightSearch.Shared.Domain;

namespace FlightSearch.Domain.Repositories;
public interface IFlightRepository : IRepository<Flight>
{
    Task<Flight?> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Flight>> SearchAsync(FlightSearchSpecification specification, CancellationToken cancellationToken = default);

}