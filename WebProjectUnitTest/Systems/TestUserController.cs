using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using App.Controllers;
using App.Services;

namespace WebProjectUnitTest;

public class TestUserController
{
    [Fact]
    public async Task Get_OnSuccess_StatusCode200()
    {
        var sut = new UserController();

        var result = (OkObjectResult)await sut.Get();

        result.StatusCode.Should().Be(200);
    }

    public async Task Get_OnSuccess_InvokesUserService()
    {
        var mockUsersService = new Mock<IUserService>();
        mockUsersService.Setup(service => service.GetAllUsers());

        var sut = new UserController(mockUserService.Object);

        await sut.Get();

        mockUserService.Verify(userService => userService.Get(), Times.Once);
    }
}
