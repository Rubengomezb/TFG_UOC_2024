using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.DB.Repository
{
    public class ContactRepository : EntityRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
