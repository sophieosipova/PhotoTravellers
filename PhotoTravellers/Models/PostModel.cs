using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Models
{
    public class PostModel
    {
        // [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int PostId { get; set; }
        public string ProfileId { get; set; }
        public string UserName { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        //   public string LastName { get; set; }

        /* public bool Equals(Profile  other)
         {
             return other != null &&
                 CommentId == other.CommentId &&
                 CommentText == other.CommentText &&
                 UserId == other.UserId &&
                 DishId == other.DishId;
         }
         public override int GetHashCode()
         {
             return this.CommentId.GetHashCode();
         } */
    }
}
