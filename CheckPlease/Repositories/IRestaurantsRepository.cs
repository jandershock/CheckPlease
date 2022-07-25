using CheckPlease.Models;
using System.Collections.Generic;

namespace CheckPlease.Repositories
{
    public interface IRestaurantsRepository
    {
        public List<Restaurant> GetAll();
    }
}
