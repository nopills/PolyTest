using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyTest
{
    // < ---- GeoJson ----> //
    public class GeoJson
    {
        public string type { get; set; }
        public string display_name { get; set; }
        public object geoJson { get; set; }
    }

    // < ---- Coordinates for single polygon ----> //
    public class Coordinates
    {
        public string type { get; set; }
        public object[][][] coordinates { get; set; }
    }

    // < ---- Coordinates for multi-poligon ----> //
    public class MultiPolygonCoordinates
    {
        public string type { get; set; }
        public object[][][][] coordinates { get; set; }
    }

}
