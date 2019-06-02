using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Models
{
    public class FollowModel
    {
        public int FollowId { get; set; }
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }
    }
}
