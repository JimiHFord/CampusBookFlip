using CampusBookFlip.Domain.Concrete;
using CampusBookFlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using WebMatrix.WebData;

namespace CampusBookFlip.WebUI.Infrastructure
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<EFDbContext>
    {
        
        protected override void Seed(EFDbContext context)
        {
            //auto-generated
            base.Seed(context);
            Constants.SeedAdmins();
            context.SaveChanges();
        }
    }
}
