using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class DbInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
