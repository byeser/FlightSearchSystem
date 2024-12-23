using Prometheus;
namespace FlightSearch.Shared.Metrics;
public static class MetricsRegistry
{
    public static readonly Counter SearchRequests =Prometheus.Metrics.CreateCounter("flight_search_total", "Total number of flight searches");

    public static readonly Histogram SearchDuration = Prometheus.Metrics.CreateHistogram("flight_search_duration_seconds", "Flight search duration");

    public static readonly Gauge ActiveSearches = Prometheus.Metrics.CreateGauge("active_searches", "Number of active flight searches");
}