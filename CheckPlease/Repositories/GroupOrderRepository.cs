using CheckPlease.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CheckPlease.Repositories
{
    public class GroupOrderRepository : BaseRepository, IGroupOrderRepository
    {
        public GroupOrderRepository(IConfiguration config) : base(config) { }

        public void DeleteGroupOrderById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM GroupOrders
                                        WHERE Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

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

		                                        goup.Id AS GoupId, goup.HasOrdered, goup.UserProfileId AS GoupUserProfileId,

		                                        up.Email AS GroupMemberEmail,

		                                        fig.FoodItemId,

		                                        fi.Id AS FoodItemId, fi.[Description], fi.Price, fi.[Type]

                                        FROM GroupOrders [go]
                                        JOIN Restaurants r ON r.Id = [go].RestaurantId
                                        JOIN GroupOrdersUserProfiles goup ON goup.GroupOrderId = [go].Id
                                        JOIN [UserProfiles] up ON up.Id = UserProfileId
                                        JOIN [UserProfiles] up2 ON up2.Id = OwnerId
                                        LEFT JOIN FoodItemsGoup fig ON fig.GroupOrdersUserProfilesId = goup.Id
                                        LEFT JOIN FoodItems fi ON fi.Id = fig.FoodItemId
                                        WHERE [go].Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
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

                            int userId = reader.GetInt32(reader.GetOrdinal("GoupUserProfileId"));
                            GroupOrderUser gou = groupOrder.GroupMembers.Where(gm => gm.UserId == userId).FirstOrDefault();
                            if (gou == null)
                            {
                                gou = new GroupOrderUser()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("GoupId")),
                                    GroupOrderId = reader.GetInt32(reader.GetOrdinal("GroupOrderId")),
                                    HasOrdered = reader.GetBoolean(reader.GetOrdinal("HasOrdered")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("GoupUserProfileId")),
                                    UserProfile = new UserProfile()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("GoupUserProfileId")),
                                        Email = reader.GetString(reader.GetOrdinal("GroupMemberEmail"))
                                    },
                                    FoodItems = new List<FoodItem>()
                                };
                                groupOrder.GroupMembers.Add(gou);
                            }


                            if (!reader.IsDBNull(reader.GetOrdinal("FoodItemId"))){
                                gou.FoodItems.Add(new FoodItem()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("FoodItemId")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Type = reader.GetString(reader.GetOrdinal("Type"))
                                });
                            }
                        }

                        return groupOrder;
                    }
                }
            }
        }
        public void UpdateGroupOrderUserHasOrderedStatus(GroupOrderUser gou)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE GroupOrdersUserProfiles
                                        SET HasOrdered = @hasOrdered
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@hasOrdered", gou.HasOrdered);
                    cmd.Parameters.AddWithValue("@id", gou.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
