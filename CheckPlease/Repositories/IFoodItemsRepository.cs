using CheckPlease.Models;
using System.Collections.Generic;

namespace CheckPlease.Repositories
{
    public interface IFoodItemsRepository
    {
        List<FoodItem> GetMenuByRestaurantId(int id);
        void CreateFoodItemsGoup(List<int> foodIds, int userId);
    }
}
