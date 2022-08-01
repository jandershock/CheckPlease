using CheckPlease.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
    }
}
