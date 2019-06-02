using PhotoTravellers.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public interface IPhotoTravellersService
    {
        Task<List<PostModel>> GetFeed (string profileId);
        Task<PaginatedModel<PostModel>> GetFeed(string profileId, int pageSize, int pageIndex);
    }
}
