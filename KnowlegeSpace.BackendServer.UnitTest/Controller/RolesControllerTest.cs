using System;
using System.Threading.Tasks;
using Moq;
using KnowledgeSpace.BackendServer.Controllers;
using Xunit;
using Microsoft.AspNetCore.Identity;
using KnowledgeSpace.ViewModels.System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using KnowlegeSpace.BackendServer.UnitTest.Suggestions;
using Confluent.Kafka;

namespace KnowlegeSpace.BackendServer.UnitTest.Controller
{
    public class RolesControllerTest
    {
       
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

        private List<IdentityRole> _roles = new List<IdentityRole>() {
                new IdentityRole("123"),
                new IdentityRole("456"),
                new IdentityRole("789"),
                new IdentityRole("test01"),
                new IdentityRole("test02")
            };

        public RolesControllerTest()
        {

            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        }


        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var rolesController = new RolesController(_mockRoleManager.Object);

            Assert.NotNull(rolesController);
        }



        [Fact]
        public async Task PostRole_ValidInput_Success()
        {
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PostRole(new RoleViewmodel()
            {
                Id = "113",
                Name= "binhthanh",

            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);   

        }
        [Fact]
        public async Task PostRole_ValidInput_Error()
        {
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { } ));
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PostRole(new RoleViewmodel()
            {
                Id = "113",
                Name = "binhthanh",

            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

        }



        [Fact]
        public async Task PutRole_ValidData_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole()        
                {
                    Id = "1111",
                    Name ="1111"
                
                 });
            _mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PutRole("1111",new RoleViewmodel()
            {
                Id = "1111",
                Name = "1111",

            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);

        }
        [Fact]
        public async Task PutRole_ValidData_Error()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole()
            {
                Id = "1111",
                Name = "1111"

            });
            _mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));

            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PutRole("1111",new RoleViewmodel()
            {
                Id = "1111",
                Name = "1111",

            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

        }


        [Fact]
        public async Task DeleteRole_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole()
            {
                Id = "1111",
                Name = "1111"

            });
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.DeleteRole("1111");
       
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task DeleteRole_Error()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole()
            {
                Id = "1111",
                Name = "1111"

            });
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));

            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.DeleteRole("1111");
      
            Assert.IsType<BadRequestObjectResult>(result);

        }



        [Fact]
        public async Task GetRolePaging_NoFilter_Success()
        {

            _mockRoleManager.Setup(x => x.Roles).Returns(_roles.AsAsyncQueryable);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetRolesPaging(null, 1, 2);
            var okeObjectResult = result as OkObjectResult;

            var roleVm = okeObjectResult.Value as Pagination<RoleViewmodel>;
            Assert.Equal(4, roleVm.PageCount);
            Assert.Equal(2, roleVm.Items.Count);
        }
        [Fact]
        public async Task GetRolePaging_AvailableFilter_Success()
        {

            _mockRoleManager.Setup(x => x.Roles).Returns(_roles.AsAsyncQueryable);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetRolesPaging("test01", 1, 2);
            var okeObjectResult = result as OkObjectResult;
            var roleVm = okeObjectResult.Value as Pagination<RoleViewmodel>;
            Assert.Equal(1, roleVm.PageCount);
            Assert.Single(roleVm.Items);
        }
        [Fact]
        public async Task GetRolePaging_ThrowException_Error()
        {

            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object);


            await Assert.ThrowsAsync<Exception>(() => rolesController.GetRolesPaging(null,1,1));
        }
  






        [Fact]
        public async Task GetRoleById_ValidData_ReturnSuccess()
        {

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole()
            {
                Id = "test01",
                Name = "test01"
            }) ;
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetById("test01");
            var okObjectResult = result as OkObjectResult;  
            Assert.NotNull(result);

            var roleVm = okObjectResult.Value as RoleViewmodel;
            Assert.Equal("test01", roleVm.Name);
        }
        [Fact]
        public async Task GetRoleById_ThrowException_Error()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object);


            await Assert.ThrowsAsync<Exception>(() => rolesController.GetById("tets01"));
        }




        [Fact]
        public async Task GetRole_ValidData_Success()
        {
           
            _mockRoleManager.Setup(x => x.Roles).Returns(_roles.AsAsyncQueryable);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetRoles();
            var okeObjectResult = result as OkObjectResult;
            var roleVm = okeObjectResult.Value as IEnumerable<RoleViewmodel>;
             
            Assert.True(roleVm.Count() > 0);
        }
        [Fact]
        public async Task GetRole_ThrowException_Error()
        {
            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object);
      

            await Assert.ThrowsAsync<Exception>(()=> rolesController.GetRoles());
        }

    }
}

