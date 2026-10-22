using GeoCalculator.Codes.Model;
using GeographicLib;

namespace GeoCalculator.Codes.Util {
    /// <summary>
    /// Calculates a new coordinate from a base (target) coordinate on the WGS84 ellipsoid.
    /// Azimuths are in degrees, clockwise from true (geodetic) north.
    /// Altitudes are heights above the WGS84 ellipsoid (HAE).
    /// </summary>
    public static class DistanceUtil {
        private const double MetresPerKilometre = 1000.0;
        private const double RadiansPerDegree = Math.PI / 180.0;

        /// <summary>
        /// Moves along the surface geodesic of the WGS84 ellipsoid from the base coordinate.
        /// The new coordinate keeps the altitude of the base coordinate.
        /// </summary>
        /// <param name="baseCoordinates">Starting (target) coordinate.</param>
        /// <param name="azimuthDegree">Azimuth in degrees, clockwise from true north.</param>
        /// <param name="distanceKm">Distance along the ellipsoid surface in kilometres.</param>
        public static Coordinates CalculateDistanceFrom(Coordinates baseCoordinates, double azimuthDegree,
            double distanceKm) {
            _ = Geodesic.WGS84.Direct(baseCoordinates.Latitude, baseCoordinates.Longitude, azimuthDegree,
                distanceKm * MetresPerKilometre, out double newLatitude, out double newLongitude);

            var coordinates = new Coordinates();
            coordinates.SetGeodetic(newLatitude, newLongitude, baseCoordinates.Altitude);
            return coordinates;
        }

        /// <summary>
        /// Moves along a straight line of sight from the base coordinate (azimuth / elevation / slant range, AER).
        /// The elevation angle is measured from the local horizontal plane of the base coordinate, so the earth
        /// curvature is accounted for: the straight-line (3D) distance between the two coordinates equals the
        /// slant range.
        /// </summary>
        /// <param name="baseCoordinates">Starting (target) coordinate.</param>
        /// <param name="azimuthDegree">Azimuth in degrees, clockwise from true north.</param>
        /// <param name="elevationDegree">
        /// Elevation angle in degrees above (+) or below (-) the local horizontal plane.
        /// </param>
        /// <param name="slantRangeKm">Straight-line distance from the base coordinate in kilometres.</param>
        public static Coordinates CalculateSlantRangeFrom(Coordinates baseCoordinates, double azimuthDegree,
            double elevationDegree, double slantRangeKm) {
            double slantRangeMetre = slantRangeKm * MetresPerKilometre;
            double azimuthRadian = azimuthDegree * RadiansPerDegree;
            double elevationRadian = elevationDegree * RadiansPerDegree;

            // Split the line of sight into East / North / Up components of the base coordinate's local tangent plane
            double horizontalMetre = slantRangeMetre * Math.Cos(elevationRadian);
            double eastMetre = horizontalMetre * Math.Sin(azimuthRadian);
            double northMetre = horizontalMetre * Math.Cos(azimuthRadian);
            double upMetre = slantRangeMetre * Math.Sin(elevationRadian);

            // Convert the local East / North / Up offset back to WGS84 geodetic coordinates
            var localFrame = new LocalCartesian(baseCoordinates.Latitude, baseCoordinates.Longitude,
                baseCoordinates.Altitude);
            var (newLatitude, newLongitude, newAltitude) = localFrame.Reverse(eastMetre, northMetre, upMetre);

            var coordinates = new Coordinates();
            coordinates.SetGeodetic(newLatitude, newLongitude, newAltitude);
            return coordinates;
        }
    }
}
