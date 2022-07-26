using CheckPlease.Models;
using System.Collections.Generic;

namespace CheckPlease.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile userProfile);
        UserProfile GetByFirebaseUserId(string firebaseUserId);
        UserProfile GetById(int id);

        List<UserProfile> GetAll();
        int AddGroupOrder(GroupOrder groupOrder);
        void CreateGroupOrderUserEntry(GroupOrderUser gou);
    }
}