namespace ServiceLayerTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;
    using ServiceLayer;
    using ServiceLayer.Interfaces;
    using DAL.Interfaces;
    using Common.CustomExceptions.UserExceptions;

    using Moq;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Common.ValidationConstants;

    [TestClass]
    public class UserServiceTests
    {
        private Mock<IApplicationDbContext> applicationDbContextMock;
        private Mock<IAuthManager> authManagerMock;
        private IUserService userService;

        [TestMethod]
        public void RegisterNewUser_Should_Throw_Exception_If_There_Is_Already_Such_A_Username_In_The_Database()
        {
            this.Arrange_Register_New_User_Exception_If_Username_Already_Exists();

            bool isTestSuccess = false;

            try
            {
                var result = this.userService.RegisterNewUser("Admin", "displayname", "email", "Password123").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserAlreadyExistsException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "RegisterNewUser did not throw UserAlreadyExistsException");
        }

        private void Arrange_Register_New_User_Exception_If_Username_Already_Exists()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            this.authManagerMock = new Mock<IAuthManager>();
            authManagerMock.Setup(prop => prop.FindByNameAsync("Admin")).Returns(Task.FromResult(new ApplicationUser() { UserName = "Admin" }));

            this.userService = new UserService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void RegisterNewUser_Should_Throw_Exception_If_There_Is_Already_Such_An_Email_In_The_Database()
        {
            this.Arrange_Register_New_User_Exception_If_Email_Already_Exists();

            bool isTestSuccess = false;

            try
            {
                var result = this.userService.RegisterNewUser("unexisting", "displayname", "admin@yahoo.com", "Password123").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserAlreadyExistsException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "RegisterNewUser did not throw UserAlreadyExistsException");
        }

        private void Arrange_Register_New_User_Exception_If_Email_Already_Exists()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            this.authManagerMock = new Mock<IAuthManager>();
            authManagerMock.Setup(prop => prop.FindByNameAsync("Admin")).Returns(Task.FromResult(new ApplicationUser()));
            authManagerMock.Setup(prop => prop.FindByEmailAsync("admin@yahoo.com")).Returns(Task.FromResult(new ApplicationUser() { UserName = "Admin", Email = "admin@yahoo.com" }));

            this.userService = new UserService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void RegisterNewUser_Should_Throw_Exception_If_The_Password_Does_Not_Meet_DoesUserPasswordRequiresDigit_Rule()
        {
            this.Arrange_Register_New_User_Password_Validation_Mocks();

            bool isTestSuccess = false;

            try
            {
                var result = this.userService.RegisterNewUser("does-not-matter", "does-not-matter", "does-not-matter", "password").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserPasswordValidationException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "RegisterNewUser did not throw UserPasswordValidationException");
        }

        [TestMethod]
        public void RegisterNewUser_Should_Throw_Exception_If_The_Password_Does_Not_Meet_DoesUserPasswordRequiresLowercase_Rule()
        {
            this.Arrange_Register_New_User_Password_Validation_Mocks();

            bool isTestSuccess = false;

            try
            {
                var result = this.userService.RegisterNewUser("does-not-matter", "does-not-matter", "does-not-matter", "PASSWORD").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserPasswordValidationException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "RegisterNewUser did not throw UserPasswordValidationException");
        }

        [TestMethod]
        public void RegisterNewUser_Should_Throw_Exception_If_The_Password_Does_Not_Meet_DoesUserPasswordRequiresUppercase_Rule()
        {
            this.Arrange_Register_New_User_Password_Validation_Mocks();

            bool isTestSuccess = false;

            try
            {
                var result = this.userService.RegisterNewUser("does-not-matter", "does-not-matter", "does-not-matter", "password1").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserPasswordValidationException)
            {
                if (aggregateException.InnerException.Message == UserValidationConstants.UserPasswordRequiresUpercaseMessage)
                {
                    isTestSuccess = true;
                }
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "RegisterNewUser did not throw UserPasswordValidationException");
        }

        private void Arrange_Register_New_User_Password_Validation_Mocks()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();
            this.authManagerMock = new Mock<IAuthManager>();

            this.userService = new UserService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        private void Annihilate()
        {
            this.applicationDbContextMock = null;
            this.authManagerMock = null;
            this.userService = null;
        }
    }
}
