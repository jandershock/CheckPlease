using CheckPlease.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CheckPlease.Repositories
{
    public class FoodItemsRepository : BaseRepository, IFoodItemsRepository
    {
        public FoodItemsRepository(IConfiguration configuration) : base(configuration) { }

        public List<FoodItem> GetMenuByRestaurantId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT fi.Id, fi.[Description], fi.RestaurantId, fi.Price, fi.[Type]
                                        FROM FoodItems fi
                                        WHERE RestaurantId = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<FoodItem> menu = new List<FoodItem>();
                        while (reader.Read())
                        {
                            menu.Add(new FoodItem()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Type = reader.GetString(reader.GetOrdinal("Type"))
                            });
                        }
                        return menu;
                    }
                }
            }
        }

        public void CreateFoodItemsGoup(List<int> foodIds, int goupId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO FoodItemsGoup
                                        SELECT FoodItems.Id AS foodItemId, GroupOrdersUserProfiles.Id AS goupId
                                        FROM FoodItems, GroupOrdersUserProfiles
                                        WHERE FoodItems.Id = @foodId AND GroupOrdersUserProfiles.Id = @goupId
	                                        AND NOT EXISTS ( 
		                                        SELECT 1
		                                        FROM FoodItemsGoup fig
		                                        WHERE fig.FoodItemId = FoodItems.Id AND fig.GroupOrdersUserProfilesId = GroupOrdersUserProfiles.Id
		                                        )";
                    foreach(int foodId in foodIds)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@foodId", foodId);
                        cmd.Parameters.AddWithValue("@goupId", goupId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteFoodItemsByGoupId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM FoodItemsGoup
                                        WHERE GroupOrdersUserProfilesId = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
