namespace FlightSearch.Shared.Exceptions;
public class FlightSearchException : Exception
{
    public string ErrorCode { get; }

    public FlightSearchException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}