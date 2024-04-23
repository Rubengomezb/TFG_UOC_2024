﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.DB.Repository.Interfaces
{
    public interface IMenuRepository : IEntityRepository<Menu>
    {
        IEnumerable<Menu> GetMenu(DateTime start, DateTime end, Guid userId);

        void UpsertRange(List<Menu> list);
    }
}