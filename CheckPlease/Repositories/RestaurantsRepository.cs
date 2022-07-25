using CheckPlease.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CheckPlease.Repositories
{
    public class RestaurantsRepository : BaseRepository, IRestaurantsRepository
    {
        public RestaurantsRepository(IConfiguration config) : base (config) { }
        public List<Restaurant> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT r.Id, r.[Name]
                            FROM Restaurants r
                            ";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Restaurant> restaurants = new List<Restaurant>();
                        while (reader.Read())
                        {
                            restaurants.Add(new Restaurant()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            });
                        }
                        return restaurants;
                    }
                }
            }
        }
    }
}
