using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPortfolio.Api.Controllers;
using Moq; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockPortfolio.Data.Entities;
using StockPortfolio.Api.Models;
using StockPortfolio.Api.Converters;
using StockPortfolio.Data.Interfaces;
using  StockPortfolio.Api.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;



namespace StockPortfolio.Test.Controllers
{
    [TestClass()]
    public class AuthControllerTest
    {
        AuthController _controller;
        CredentialModel _credentialModel;
        User _returnedUser;
        UserModel _userModel;
        Mock<IStockPortfolioRepository> mockRepository;
        Mock<ILogger<AuthController>> mockLogger;
        Mock<ITokenGenerator> mockTokenGenerator;
        Mock<IPasswordHasher> mockPasswordHasher;
        Mock<IMapper> mockMapper;
        

        [TestInitialize()]
        public void Initialize(){
            //Arrange
            mockRepository = new Mock<IStockPortfolioRepository>();
            mockLogger = new Mock<ILogger<AuthController>>();
            mockTokenGenerator = new Mock<ITokenGenerator>();
            mockPasswordHasher = new Mock<IPasswordHasher>();
            mockMapper = new Mock<IMapper>();
            _controller = new AuthController(
                mockRepository.Object,
                mockLogger.Object,
                mockTokenGenerator.Object,
                mockPasswordHasher.Object,
                mockMapper.Object);
            
            _credentialModel = new CredentialModel{
                UserName = "aacister",
                Password = "password"
            };

            _returnedUser = new User { userName = "aacister",
                                firstName = "Andrew",
                                lastName = "Cisternino",
                                hash = new byte[50],
                                salt = new byte[10],
                                stocks = new List<Stock>(),
                                newsSources = new List<NewsSource>()
                              };
            _userModel = new UserModel{
                UserName = "aacister",
                Token = "",
                FirstName = "Andrew",
                LastName = "Cisternino",
                Stocks = new List<StockModel>(),
                News = new List<NewsSourceModel>()
            };
        
            mockRepository.Setup(x => x.GetUser(_credentialModel.UserName))
            .Returns(Task.FromResult(_returnedUser));


            
            mockPasswordHasher.Setup(x => x.Hash(_credentialModel.Password, It.IsAny<byte[]>()))
            .Returns(_returnedUser.hash);

            mockTokenGenerator.Setup(x => x.CreateToken(_credentialModel.UserName))
            .Returns(Task.FromResult("mockToken"));

            mockMapper.Setup(x => x.Map<UserModel>(It.IsAny<User>()))
            .Returns(_userModel);

        }

        

        [TestMethod()]
        public void Login_GivenValid_ReturnsOk()
        {
            // Act
            var actionResult = _controller.Login(_credentialModel);
            var result = actionResult.Result as OkObjectResult;
            var resultUser = result.Value as UserModel;
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultUser, typeof(UserModel));
            Assert.AreEqual("mockToken", resultUser.Token);
            Assert.AreEqual("aacister", resultUser.UserName);
        }

        [TestMethod()]
        public void Login_Exception_ReturnsBadRequest()
        {
            //Arrange
             mockRepository.Setup(x => x.GetUser(It.IsAny<string>()))
            .Throws(new Exception());
 
            //Act
            var actionResult = _controller.Login(_credentialModel);
             var result = actionResult.Result as BadRequestObjectResult;
            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
 
        }
        
    }
}