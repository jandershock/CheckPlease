using CheckPlease.Models;

namespace CheckPlease.Repositories
{
    public interface IGroupOrderRepository
    {
        public GroupOrder GetGroupOrderById(int id);
        void UpdateGroupOrderUserHasOrderedStatus(GroupOrderUser gou);
        void UpdateGroupOrderIsReadyStatus(GroupOrder go);
        void DeleteGroupOrderById(int id);
    }
}
