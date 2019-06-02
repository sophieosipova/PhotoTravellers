using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Models
{
    public class LocationModel
    {
        public int LocationId { get; set; }
        public string LocationCountry { get; set; }
        public string LocationCity { get; set; }
        public string LocationName { get; set; }
        public int Raiting { get; set; }
        public float FromLongitude { get; set; }
        public float ToLongitude { get; set; }
        public float FromLatitude { get; set; }
        public float ToLatitude { get; set; }
    }
}
