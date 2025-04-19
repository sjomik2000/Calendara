using System;

namespace Calendara.Application.Models
{
    public class GeoCoordinate
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        // Overriding = to allow for precision accuracy relaxation when updating coordinates
        public override bool Equals(object obj)
        {
            if (obj is GeoCoordinate other)
            {
                return Math.Abs(Latitude - other.Latitude) < 0.0000001 &&
                       Math.Abs(Longitude - other.Longitude) < 0.0000001;
            }
            return false;
        }
        // Combining hashcode to store on same reference heap in dictionary.
        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }
    }
}
