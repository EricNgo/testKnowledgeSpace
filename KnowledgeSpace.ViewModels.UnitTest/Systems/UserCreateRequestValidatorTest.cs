using KnowledgeSpace.ViewModels.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.ViewModels.UnitTest.Systems
{
    public class UserCreateRequestValidatorTest
    {
        private UserCreateRequestValidator validator;
        private UserCreateRequest request;

        public UserCreateRequestValidatorTest()
        {
            request = new UserCreateRequest()
            {
                Dob = DateTime.Now,
                Email = "test01@gmail.com",
                FirstName = "test",
                LastName = "test",
                Password = "admin@123",
                PhoneNumber = "09129321",
                UserName ="admin",
            };
            validator = new UserCreateRequestValidator();
        }

        [Fact]
        public void Should_Valid_Result_When_Valid_Request()
        {
            var result = validator.Validate(request);
            Assert.True(result.IsValid);
        }

 
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Request_Miss_UserName(string userName)
        {
            request.UserName = userName;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Request_Miss_FirstName(string firstname)
        
        {
            request.FirstName = firstname;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Request_Miss_LastName(string lastname)

        {
          request.LastName = lastname;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }



        [Theory]
        [InlineData("sasasasa")]
        [InlineData("123123123")]
        [InlineData("Admin123")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Request_Miss_PassWord(string password)

        {
            request.Password = password;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Request_Miss_PhoneNumber(string phonenumber)

        {
            request.PhoneNumber = phonenumber;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }




        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Miss_Email(string email)
        {
            request.Email = email;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}
