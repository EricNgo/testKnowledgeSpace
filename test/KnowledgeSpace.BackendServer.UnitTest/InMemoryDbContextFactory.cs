﻿using KnowledgeSpace.BackendServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.BackendServer.UnitTest
{
    public class InMemoryDbContextFactory  // cho phep tao ra Databse cho cac thao tac them,sua,xoa
    {

       
            public ApplicationDbContext GetApplicationDbContext()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseInMemoryDatabase(databaseName: "InMemoryApplicationDatabase")
                           .Options;
                var dbContext = new ApplicationDbContext(options);

                return dbContext;
            }
        }

    
}