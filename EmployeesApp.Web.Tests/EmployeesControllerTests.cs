using EmployeesApp.Application.Employees.Interfaces;
using EmployeesApp.Domain.Entities;
using EmployeesApp.Web.Controllers;
using EmployeesApp.Web.Views.Employees;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;


namespace EmployeesApp.Web.Tests;

public class EmployeesControllerTests
{
    [Fact]
    public void IndexTest()
    {
        //Arrange
        var employeeService = new Mock<IEmployeeService>();
        employeeService
            .Setup(o => o.GetAll())
            .Returns([
                new Employee { Id = 1, Email = "mock@mock.com", Name = "Mock Mock" },
                new Employee { Id = 2, Email = "mock2@mock.com", Name = "Mocke Mocke" }]);
        var controller = new EmployeesController(employeeService.Object);

        //Act
        var result = controller.Index();

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void DetailsTest()
    {
        var employeeService = new Mock<IEmployeeService>();
        employeeService
            .Setup(o => o.GetById(1))
            .Returns(new Employee { Id = 1, Email = "mock@mock.com", Name = "Mock Mock" });
        var controller = new EmployeesController(employeeService.Object);

        //Act
        var result = controller.Details(1);

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void CreateTest_NotValid()
    {
        //Arrange
        var model = new CreateVM
        {
            Name = "Mock Mock",
            Email = "mock@mock.com",
            BotCheck = 2
        }; 

        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        //Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        //Assert
        Assert.False(isValid);
    }
}