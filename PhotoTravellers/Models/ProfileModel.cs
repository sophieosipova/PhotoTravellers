using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Models
{
    public class ProfileModel //: IEquatable<Profile>
    {
        // [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public string ProfileId { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public string ImageUrl { get; set; }
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
