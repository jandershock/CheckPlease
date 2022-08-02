using CheckPlease.Models;

namespace CheckPlease.Repositories
{
    public interface IGroupOrderRepository
    {
        public GroupOrder GetGroupOrderById(int id);
        void UpdateGroupOrderUserHasOrderedStatus(GroupOrderUser gou);
        void DeleteGroupOrderById(int id);
    }
}
