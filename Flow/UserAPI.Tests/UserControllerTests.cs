using API.Controllers;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UserAPI.Tests;

[TestClass]
public sealed class UserControllerTests
{
    
    private Mock<IUserRepository> _mockRepository;
    private UsersController _controller;
    
    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _controller = new UsersController(_mockRepository.Object);
    }
    
    // Register endpoint tests
    
    [TestMethod]
    public void Valid_Registration_Returns_200_Code_And_Adds_User()
    {
        // Arrange
        var user = new UserDTO()
        {
            Email = "Umut@hjemmeværnet.dk",
            Name = "Umut",
            Password = "Umut123"
        };
        _mockRepository.Setup(r => r.Register(user))
            .Returns(0);

        // Act
        var result = _controller.RegisterUser(user) as CreatedResult;

        // Assert
        Assert.IsInstanceOfType<CreatedResult>(result);
        _mockRepository.Verify(r => r.Register(user), Times.Once);

    }

    [TestMethod]
    public void Username_Already_Exists_Returns_400_Bad_Request()
    {
        // Arrange
        _mockRepository.Setup(r => r.Register(It.IsAny<UserDTO>()))
            .Returns(2);

        // Act
        var result = _controller.RegisterUser(new UserDTO()) as  BadRequestResult;

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
        _mockRepository.Verify(r => r.Register(It.IsAny<UserDTO>()), Times.Once);
    }

    [TestMethod]
    public void NULL_DTO_Returns_400_Bad_Request()
    {
        // Arrange
        _mockRepository.Setup(r => r.Register(null))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(null) as BadRequestResult;
        
        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public void Empty_Username_Returns_400_Bad_Request()
    {
        // Arrange
        var userDTO = new UserDTO()
        {
            Email = "Somethinig@agag",
            Name = "",
            Password = "Erarararah"
        };
        _mockRepository.Setup(r => r.Register(userDTO))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(userDTO) as BadRequestResult;
        Assert.IsInstanceOfType<BadRequestResult>(result);
        _mockRepository.Verify(r => r.Register(It.IsAny<UserDTO>()), Times.Once);
    }

    [TestMethod]
    public void Empty_Password_Returns_400_Bad_Request()
    {
        // Arrange
        var userDTO = new UserDTO()
        {
            Email = "Somethinig@agag",
            Name = "Mortey",
            Password = ""
        };
        _mockRepository.Setup(r => r.Register(userDTO))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(userDTO) as BadRequestResult;
        Assert.IsInstanceOfType<BadRequestResult>(result);
        _mockRepository.Verify(r => r.Register(It.IsAny<UserDTO>()), Times.Once);
    }

    [TestMethod]
    public void WhiteSpace_Username_Returns_400_Bad_Request()
    {
        // Arrange
        var userDTO = new UserDTO()
        {
            Email = "Somethinig@agag",
            Name = "      ",
            Password = "Erarararah"
        };
        _mockRepository.Setup(r => r.Register(userDTO))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(userDTO) as BadRequestResult;
        Assert.IsInstanceOfType<BadRequestResult>(result);
        _mockRepository.Verify(r => r.Register(It.IsAny<UserDTO>()), Times.Once);
    }

    [TestMethod]
    public void Repository_Throws_Exception_During_Add_Returns_500_Server_Error()
    {
        // Arrange
        _mockRepository.Setup(r => r.Register(It.IsAny<UserDTO>()))
            .Throws<Exception>();
        
        // Act
        var result = _controller.RegisterUser(new UserDTO()) as StatusCodeResult;
        
        // Assert
        Assert.IsInstanceOfType<StatusCodeResult>(result);
        var expected = 500;
        Assert.AreEqual(expected, result.StatusCode);
        _mockRepository.Verify(r => r.Register(It.IsAny<UserDTO>()), Times.Once);
    }
    
    // Login endpoint tests

    [TestMethod]
    public void Valid_Credentials_Returns_200_Status_Code()
    {
        // Arrange
        _mockRepository.Setup(l => l.Login(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new User());
        var request = new UserLogin("Makaag", "egs2525");
        
        // Act
        var result = _controller.Login(request) as OkObjectResult;
        
        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(result);
        _mockRepository.Verify(r => r.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void Repository_Called_With_Correct_Data_Returns_200_Status_Code()
    {
        // Arrange
        User darryl = new User(){Id = 523, Email = "darryl@agag", Name = "Darryl", Password = "darryl123"};
        _mockRepository.Setup(r => r.Login("Darryl", "darryl123"))
            .Returns(darryl);
        UserLogin request = new UserLogin("Darryl", "darryl123");
        
        // Act
        var result = _controller.Login(request) as OkObjectResult;
        
        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(result);
        _mockRepository.Verify(r => r.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        Assert.AreEqual(result.Value, darryl);
    }

    [TestMethod]
    public void Wrong_Password_OR_User_Not_Found_Returns_401_Unauthorized()
    {
        // Arrange
        _mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((User?)null);
        var request = new UserLogin("Darryl", "wrongPassword");
        
        // Act
        var result = _controller.Login(request) as UnauthorizedResult;
        
        // Assert
        Assert.IsInstanceOfType<UnauthorizedResult>(result);
        _mockRepository.Verify(r => r.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
    
    [TestMethod]
    public void Null_DTO_Returns_400_Bad_Request()
    {
        // Arrange
        
        // Act
        var result = _controller.Login(null) as BadRequestResult;
        
        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public void Empty_Username_Login_Returns_400_Bad_Request()
    {
        // Arrange
        var request = new UserLogin("", "myPassword");
        
        // Act
        var result = _controller.Login(request) as BadRequestResult;
        
        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }
    
    [TestMethod]
    public void Empty_Password_Login_Returns_400_Bad_Request()
    {
        // Arrange
        var request = new UserLogin("MyUsername", "");
        
        // Act
        var result = _controller.Login(request) as BadRequestResult;
        
        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }
    
    // Don't know if this is doing anything meaningful
    [TestMethod]
    public void Invalid_ModelState_Returns_400_Bad_Request()
    {
        // Arrange
        _mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new User());
        var request = new UserLogin(null, null);
        
        // Act
        var result = _controller.Login(request) as BadRequestResult;
        
        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }
    
    [TestMethod]
    public void Repository_Throws_Exception_Returns_500_Server_Error()
    {
        // Arrange
        _mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
            .Throws<Exception>();
        var request = new UserLogin("Makaag", "egs2525");
        
        // Act
        var result = _controller.Login(request) as StatusCodeResult;
        
        // Assert
        Assert.IsInstanceOfType<StatusCodeResult>(result);
        var expected = 500;
        Assert.AreEqual(expected, result.StatusCode);
    }

    [TestMethod]
    public void Test_That_Fails()
    {
        Assert.Fail();
    }
    
}