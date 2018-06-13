namespace ServiceLayerTests
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;
    using DAL.Interfaces;
    using ServiceLayer;
    using ServiceLayer.Interfaces;
    using Common.CustomExceptions.UserExceptions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class TokenServiceTests
    {
        private Mock<IApplicationDbContext> applicationDbContextMock;
        private Mock<IAuthManager> authManagerMock;
        private ITokenService tokenService;

        [TestMethod]
        public void CreateUserRefreshToken_Should_Throw_UserNotFoundException_If_There_There_Is_No_User_With_Username_In_The_Database()
        {
            this.Arrange_User_Not_Found_Exception();

            bool isTestSuccess = false;

            try
            {
                var result = this.tokenService.CreateUserRefreshToken("Admin", "password", "client-id").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserNotFoundException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "CreateUserRefreshToken did not throw UserNotFoundException");
        }

        [TestMethod]
        public void CreateUserRefreshToken_Should_Throw_UserNotFoundException_If_There_There_Is_No_User_With_Email_In_The_Database()
        {
            this.Arrange_User_Not_Found_Exception();

            bool isTestSuccess = false;

            try
            {
                var result = this.tokenService.CreateUserRefreshToken("admin@yahoo.com", "password", "client-id").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserNotFoundException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "CreateUserRefreshToken did not throw UserNotFoundException");
        }

        private void Arrange_User_Not_Found_Exception()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            this.authManagerMock = new Mock<IAuthManager>();
            authManagerMock.Setup(prop => prop.FindByNameAsync("Admin")).Returns(Task.FromResult(NullUser()));
            authManagerMock.Setup(prop => prop.FindByEmailAsync("admin@yahoo.com")).Returns(Task.FromResult(NullUser()));

            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void CreateUserRefreshToken_Should_Throw_UserPasswordMissmatchException_If_The_Password_Parameter_Does_Not_Match_The_User_Actual_Password()
        {
            Arrange_Password_MissMatchException_Mocks();

            bool isTestSuccess = false;

            try
            {
                var result = this.tokenService.CreateUserRefreshToken("Admin", "password", "client-id").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserPasswordMissmatchException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "CreateUserRefreshToken did not throw UserPasswordMissmatchException");
        }

        private void Arrange_Password_MissMatchException_Mocks()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            this.authManagerMock = new Mock<IAuthManager>();
            authManagerMock.Setup(prop => prop.FindByNameAsync("Admin")).Returns(Task.FromResult(User()));
            authManagerMock.Setup(prop => prop.FindByEmailAsync("admin@yahoo.com")).Returns(Task.FromResult(User()));
            authManagerMock.Setup(prop => prop.CheckPasswordAsync(User(), "password")).Returns(Task.FromResult(true));

            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void CreateUserRefreshToken_Should_Add_Token_To_The_Tokens_DbSet()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            var mockSet = new Mock<DbSet<Token>>();
            mockSet.As<IQueryable<Token>>().Setup(m => m.Provider).Returns(new List<Token>().AsQueryable().Provider);
            applicationDbContextMock.Setup(prop => prop.Tokens).Returns(mockSet.Object);

            int addTokenCounter = 0;
            applicationDbContextMock.Setup(x => x.Tokens.Add(It.IsAny<Token>())).Callback(() => addTokenCounter++);

            this.authManagerMock = new Mock<IAuthManager>();
            authManagerMock.Setup(prop => prop.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(User()));
            authManagerMock.Setup(prop => prop.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(User()));
            authManagerMock.Setup(prop => prop.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);

            var result = this.tokenService.CreateUserRefreshToken("Admin", "123456", "client-id").Result;

            this.Annihilate();

            Assert.AreEqual(1, addTokenCounter);
        }

        [TestMethod]
        public void CreateUserRefreshToken_Should_Return_Token()
        {
            this.Arrange_Return_Valid_Token_Mocks();

            var result = this.tokenService.CreateUserRefreshToken("Admin", "123456", "client-id").Result;

            this.Annihilate();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Token));

            Assert.AreEqual("client-id", result.ClientId);
            Assert.AreEqual("user-id", result.UserId);
        }

        private void Arrange_Return_Valid_Token_Mocks()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            var mockSet = new Mock<DbSet<Token>>();
            mockSet.As<IQueryable<Token>>().Setup(m => m.Provider).Returns(new List<Token>().AsQueryable().Provider);
            applicationDbContextMock.Setup(prop => prop.Tokens).Returns(mockSet.Object);


            this.authManagerMock = new Mock<IAuthManager>();
            authManagerMock.Setup(prop => prop.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(User()));
            authManagerMock.Setup(prop => prop.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(User()));
            authManagerMock.Setup(prop => prop.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void ReplaceUserRefreshToken_Should_Throw_UserTokenNotFoundException_If_There_Is_No_Such_Token_In_The_Database()
        {
            Arrange_ReplaceUserRefreshToken_Token_Not_Found_Exception_Mocks();

            bool isTestSuccess = false;

            try
            {
                var result = this.tokenService.ReplaceUserRefreshToken("client-id", "token-value").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserTokenNotFoundException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "ReplaceUserRefreshToken did not throw UserTokenNotFoundException");
        }

        private void Arrange_ReplaceUserRefreshToken_Token_Not_Found_Exception_Mocks()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            DbSet<Token> tokensMock = this.GetQueryableMockDbSet
            (
                new Token { Value = "first-token-value", UserId = "user-id" },
                new Token { Value = "second-token-value", UserId = "second-user-id" }
            );

            applicationDbContextMock.Setup(prop => prop.Tokens).Returns(tokensMock);

            this.authManagerMock = new Mock<IAuthManager>();
            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void ReplaceUserRefreshToken_Should_Throw_UserNotFoundException_If_TokenUserId_Does_Not_Correspond_To_A_User_In_The_Database()
        {
            Arrange_ReplaceUserRefreshToken_Throws_UserNotFoundException_Mocks();

            bool isTestSuccess = false;

            try
            {
                var result = this.tokenService.ReplaceUserRefreshToken("client-id", "first-token-value").Result;
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is UserNotFoundException)
            {
                isTestSuccess = true;
            }

            this.Annihilate();

            Assert.IsTrue(isTestSuccess, "ReplaceUserRefreshToken did not throw UserNotFoundException");
        }

        private void Arrange_ReplaceUserRefreshToken_Throws_UserNotFoundException_Mocks()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            DbSet<Token> tokensMock = this.GetQueryableMockDbSet
            (
                new Token { Value = "first-token-value", UserId = "user-id", ClientId = "client-id" },
                new Token { Value = "second-token-value", UserId = "second-user-id", ClientId = "second-client-id" }
            );

            applicationDbContextMock.Setup(prop => prop.Tokens).Returns(tokensMock);

            this.authManagerMock = new Mock<IAuthManager>();
            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        public void ReplaceUserRefreshToken_Should_Return_Valid_Token()
        {
            Arrange_ReplaceUserRefreshToken_Returns_Valid_Token();

            var result = this.tokenService.ReplaceUserRefreshToken("client-id", "first-token-value").Result;

            this.Annihilate();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Token));

            Assert.AreEqual("user-id", result.UserId);
            Assert.AreEqual("client-id", result.ClientId);
            Assert.AreEqual(DateTime.Now.Date, result.CreatedDate.Date);
            Assert.AreNotEqual("first-token-value", result.Value);
        }

        private void Arrange_ReplaceUserRefreshToken_Returns_Valid_Token()
        {
            this.applicationDbContextMock = new Mock<IApplicationDbContext>();

            DbSet<Token> tokensMock = this.GetQueryableMockDbSet
            (
                new Token { Value = "first-token-value", UserId = "user-id", ClientId = "client-id" },
                new Token { Value = "second-token-value", UserId = "second-user-id", ClientId = "second-client-id" }
            );

            this.applicationDbContextMock.Setup(prop => prop.Tokens).Returns(tokensMock);

            this.authManagerMock = new Mock<IAuthManager>();
            this.authManagerMock.Setup(prop => prop.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(User()));

            this.tokenService = new TokenService(this.applicationDbContextMock.Object, this.authManagerMock.Object);
        }

        [TestMethod]
        [Ignore]
        public void ReplaceUserRefreshToken_Should_Call_Add_and_Remove_On_Tokens_DbSet_Once()
        {

        }

        private DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }

        private ApplicationUser User()
        {
            ApplicationUser user = new ApplicationUser();

            user.Id = "user-id";
            user.UserName = "username";
            user.Email = "email@yahoo.com";
            user.PasswordHash = "123456";

            return user;
        }

        private ApplicationUser NullUser()
        {
            return null;
        }

        private Token NullToken()
        {
            return null;
        }

        private void Annihilate()
        {
            this.applicationDbContextMock = null;
            this.authManagerMock = null;
            this.tokenService = null;
        }
    }
}