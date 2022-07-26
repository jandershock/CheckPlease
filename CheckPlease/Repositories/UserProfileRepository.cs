﻿using Microsoft.Data.SqlClient;
using CheckPlease.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CheckPlease.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public UserProfile GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Email, FirebaseUserId
                                    FROM UserProfiles
                                    WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Email, FirebaseUserId
                                    FROM UserProfiles
                                    WHERE FirebaseUserId = @FirebaseuserId";

                    cmd.Parameters.AddWithValue("@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        UserProfiles (Email, FirebaseUserId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@email, @firebaseUserId)";

                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@firebaseUserId", userProfile.FirebaseUserId);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<UserProfile> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT up.Id, up.Email, up.FirebaseUserId
                                        FROM UserProfiles up";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> users = new List<UserProfile>();
                        while (reader.Read())
                        {
                            users.Add(new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId"))
                            });
                        }
                        return users;
                    }
                }
            }
        }

        public void AddGroupOrder(GroupOrder groupOrder)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO GroupOrders (OwnerId, RestaurantId, IsReady)
                                        OUTPUT INSERTED.Id
                                        VALUES (@ownerId, @restaurantId, @isReady)";
                    cmd.Parameters.AddWithValue("@ownerId", groupOrder.OwnerId);
                    cmd.Parameters.AddWithValue("@restaurantId", groupOrder.RestaurantId);
                    cmd.Parameters.AddWithValue("@isReady", groupOrder.IsReady);
                    groupOrder.Id = (int) cmd.ExecuteScalar();
                }
            }
        }

        public void CreateGroupOrderUserEntry(GroupOrderUser gou)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO GroupOrdersUserProfiles (UserProfileId, GroupOrderId, HasOrdered)
                                        VALUES (@userProfileId, @groupOrderId, @hasOrdered)";
                    cmd.Parameters.AddWithValue("@userProfileId", gou.UserId);
                    cmd.Parameters.AddWithValue("@groupOrderId", gou.GroupOrderId);
                    cmd.Parameters.AddWithValue("@hasOrdered", gou.HasOrdered);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<GroupOrder> GetGroupOrdersByUser(int userId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [go].Id AS GroupOrderId, [go].IsReady, [go].OwnerId, [go].RestaurantId,

		                                        r.[Name] AS RestaurantName,

		                                        up.Email AS OwnerEmail
                                        FROM GroupOrdersUserProfiles goup
                                        JOIN GroupOrders [go] ON [go].Id = goup.GroupOrderId
                                        JOIN Restaurants r ON r.Id = [go].RestaurantId
                                        JOIN UserProfiles up ON up.Id = [go].OwnerId
                                        WHERE goup.UserProfileId = @userId";
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<GroupOrder> groupOrders = new List<GroupOrder>();
                        while (reader.Read())
                        {
                            groupOrders.Add(new GroupOrder()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("GroupOrderId")),
                                IsReady = reader.GetBoolean(reader.GetOrdinal("IsReady")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),

                                Restaurant = new Restaurant()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName"))
                                },

                                Owner = new UserProfile()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Email = reader.GetString(reader.GetOrdinal("OwnerEmail"))
                                }
                            });
                        }
                        return groupOrders;
                    }
                }
            }
        }

    }
}
