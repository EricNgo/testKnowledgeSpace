 
using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModels;
//using KnowledgeSpace.ViewModels.System;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class UsersControllerTest
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly ApplicationDbContext _context;

        private List<User> _userSources = new List<User>(){
                             new User("1","test1","Te","Du1","test01@gmail.com","0933331",DateTime.Now),
                             new User("2","test2","Te","Du2","test02@gmail.com","0933332",DateTime.Now),
                             new User("3","test3","Te","Du3","test03@gmail.com","0933333",DateTime.Now),
                             new User("4","test4","Te","Du4","test04@gmail.com","0933334",DateTime.Now)
                        };

        public UsersControllerTest()
        {
            var UserStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(UserStore.Object, null, null, null, null,null,null,null,null);

            var RoleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(RoleStore.Object, null, null,  null,null);

            _context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var userController = new UserController(_mockUserManager.Object,_mockRoleManager.Object,_context);
            Assert.NotNull(userController);
        }

        [Fact]
        public async Task PostUser_ValidInput_Success()
        {
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    UserName = "test"
                });

            var usersController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await usersController.PostUser(new UserCreateRequest()
            {
                UserName = "test"
            });

            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task PostUser_ValidInput_Failed()
        {
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));

            var usersController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await usersController.PostUser(new UserCreateRequest()
            {
                UserName = "test"
            });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetUser_HasData_ReturnSuccess()
        {
            _mockUserManager.Setup(x => x.Users)
                .Returns(_userSources.AsQueryable().BuildMock().Object);
            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.GetUsers();
            var okResult = result as OkObjectResult;
            var userVms = okResult.Value as IEnumerable<UserVm>;
            Assert.True(userVms.Count() > 0);
        }

        [Fact]
        public async Task GetUser_ThrowException_Failed()
        {
            _mockUserManager.Setup(x => x.Users).Throws<Exception>();

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);

            await Assert.ThrowsAnyAsync<Exception>(async () => await userController.GetUsers());
        }

        [Fact]
        public async Task GetUserPaging_NoFilter_ReturnSuccess()
        {
            _mockUserManager.Setup(x => x.Users)
                .Returns(_userSources.AsQueryable().BuildMock().Object);

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.GetUsersPaging(null, 1, 2);
            var okResult = result as OkObjectResult;
            var userVms = okResult.Value as Pagination<UserVm>;
            Assert.Equal(4, userVms.TotalRecords);
            Assert.Equal(2, userVms.Items.Count);
        }

        [Fact]
        public async Task GetUserPaging_HasFilter_ReturnSuccess()
        {
            _mockUserManager.Setup(x => x.Users)
                .Returns(_userSources.AsQueryable().BuildMock().Object);

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.GetUsersPaging("test3", 1, 2);
            var okResult = result as OkObjectResult;
            var userVms = okResult.Value as Pagination<UserVm>;
            Assert.Equal(1, userVms.TotalRecords);
            Assert.Single(userVms.Items);
        }

        [Fact]
        public async Task GetUserPaging_ThrowException_Failed()
        {
            _mockUserManager.Setup(x => x.Users).Throws<Exception>();

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);

            await Assert.ThrowsAnyAsync<Exception>(async () => await userController.GetUsersPaging(null, 1, 1));
        }

        [Fact]
        public async Task GetById_HasData_ReturnSuccess()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                   UserName = "test1"
                });

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.GetById("test1");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var userVm = okResult.Value as UserVm;

            Assert.Equal("test1", userVm.UserName);
        }

        [Fact]
        public async Task GetById_ThrowException_Failed()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Throws<Exception>();

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);

            await Assert.ThrowsAnyAsync<Exception>(async () => await userController.GetById("test1"));
        }

        [Fact]
        public async Task PutUser_ValidInput_Success()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(new User()
               {UserName = "test1"
               });

            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PutUser("test", new UserCreateRequest()
            {FirstName = "test2"
            });

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutUser_ValidInput_Failed()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
             .ReturnsAsync(new User()
             {
                 UserName = "test" 
             });

            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PutUser("test", new UserCreateRequest()
            {
               UserName   = "test"
            });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ValidInput_Success()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(new User()
               {
                   UserName= "test"
               });

            _mockUserManager.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.DeleteUser("test");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ValidInput_Failed()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
             .ReturnsAsync(new User()
             {
                 UserName = "test"
             });

            _mockUserManager.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));

            var userController = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.DeleteUser("test");
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
