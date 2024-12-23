using Prometheus;

namespace FlightSearch.Api.Metrics;
public static class FlightSearchMetrics
{
    // Prometheus.Metrics sınıfından CreateCounter'ı kullanıyoruz
    public static readonly Counter SearchRequests = Prometheus.Metrics
        .CreateCounter("flight_search_requests_total", "Total number of flight search requests");

    public static readonly Histogram SearchDuration = Prometheus.Metrics
        .CreateHistogram("flight_search_duration_seconds", "Duration of flight search requests");

    public static readonly Gauge ActiveSearches = Prometheus.Metrics
        .CreateGauge("flight_search_active", "Number of active flight searches");
}