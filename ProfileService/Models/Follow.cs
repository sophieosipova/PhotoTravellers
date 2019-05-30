using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class Follow
    {
        public int FollowId { get; set; }
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }
    }
}
