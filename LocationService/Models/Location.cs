using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationService.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string LocationCity { get; set; }
        public string LocationName { get; set; }
        public float FromLongitude { get; set; }
        public float ToLongitude { get; set; }
        public float FromLatitude { get; set; }
        public float ToLatitude { get; set; }
    }
}
