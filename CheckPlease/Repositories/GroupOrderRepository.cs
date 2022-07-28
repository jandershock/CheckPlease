using CheckPlease.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CheckPlease.Repositories
{
    public class GroupOrderRepository : BaseRepository, IGroupOrderRepository
    {
        public GroupOrderRepository(IConfiguration config) : base(config) { }

        public GroupOrder GetGroupOrderById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [go].Id AS GroupOrderId, [go].OwnerId, [go].IsReady, [go].RestaurantId,

                                                r.[Name] AS RestaurantName,

		                                        up2.Email AS OwnerEmail,

		                                        goup.Id AS GoupId, goup.UserProfileId, goup.HasOrdered, goup.UserProfileId AS GoupUserProfileId,

		                                        up.Email AS GroupMemberEmail
                                        FROM GroupOrders [go]
                                        JOIN Restaurants r ON r.Id = [go].RestaurantId
                                        JOIN GroupOrdersUserProfiles goup ON goup.GroupOrderId = [go].Id
                                        JOIN [UserProfiles] up ON up.Id = UserProfileId
                                        JOIN [UserProfiles] up2 ON up2.Id = OwnerId
                                        WHERE [go].Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        GroupOrder groupOrder = new GroupOrder()
                        {
                            Restaurant = null,
                            Owner = null,
                            GroupMembers = new List<GroupOrderUser>()
                        };

                        while (reader.Read())
                        {
                            if (groupOrder.Restaurant == null)
                            {
                                groupOrder.Id = reader.GetInt32(reader.GetOrdinal("GroupOrderId"));
                                groupOrder.IsReady = reader.GetBoolean(reader.GetOrdinal("IsReady"));
                                groupOrder.RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId"));
                                groupOrder.OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"));
                                groupOrder.Restaurant = new Restaurant()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName")),
                                };
                            }
                            if (groupOrder.Owner == null)
                            {
                                groupOrder.Owner = new UserProfile() 
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Email = reader.GetString(reader.GetOrdinal("OwnerEmail"))
                                };
                            }
                            groupOrder.GroupMembers.Add(new GroupOrderUser()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("GoupId")),
                                GroupOrderId = reader.GetInt32(reader.GetOrdinal("GroupOrderId")),
                                HasOrdered = reader.GetBoolean(reader.GetOrdinal("HasOrdered")),
                                UserId = reader.GetInt32(reader.GetOrdinal("GoupUserProfileId"))
                            });
                        }

                        return groupOrder;
                    }
                }
            }
        }
    }
}
