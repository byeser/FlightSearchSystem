using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.ValueObjects;
public record Duration
{
    public TimeSpan Value { get; }

    private Duration(TimeSpan value) => Value = value;

    public static Duration FromTimeSpan(TimeSpan timeSpan) => new(timeSpan);
    public static Duration FromString(string duration) => new(TimeSpan.Parse(duration));
}