﻿using CheckPlease.Models;

namespace CheckPlease.Repositories
{
    public interface IGroupOrderRepository
    {
        public GroupOrder GetGroupOrderById(int id);
    }
}
