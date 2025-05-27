using EmployeesApp.Application.Employees;
using EmployeesApp.Application.Employees.Interfaces;
using EmployeesApp.Application.Employees.Services;
using EmployeesApp.Domain.Entities;
using Moq;
using System.Xml.Linq;

namespace EmployeesApp.Application.Tests;

public class EmployeesServiceTests
{
    [Fact]
    public void Add_VerifyAddedEmployee()
    {
        //Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        var employeeService = new EmployeeService(employeeRepository.Object);
        var testEmployee = new Employee { Id = 1, Name = "Mock Mock", Email = "mock@mock.com" };
         
        //Act
        employeeService.Add(testEmployee);

        //Assert
        employeeRepository.Verify(o => o.Add(testEmployee), Times.Exactly(1));
    }


    [Fact]
    public void GetAll_ReturnsEmployeeArray()
    {
        //Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        
        employeeRepository
            .Setup(e => e.GetAll())
            .Returns([
                new Employee { Email = "mock@mock.com", Id = 1, Name = "Mock Mock" },
                new Employee { Email = "mock2@mock.com", Id = 2, Name = "Mocke Mocke" }]);
        var employeeService = new EmployeeService(employeeRepository.Object);

        //Act
        var result = employeeService.GetAll();

        //Assert
        Assert.Equal(2, result.Length);
        Assert.IsType<Employee[]>(result);
    }

    [Fact]
    public void GetById_InvalidId_ThrowsException()
    {
        //Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        employeeRepository
            .Setup(e => e.GetById(-1))
            .Throws<ArgumentException>();
        var employeeService = new EmployeeService(employeeRepository.Object);

        //Act
        var result = Record.Exception(() => employeeService.GetById(-1));

        //Assert
        Assert.IsType<ArgumentException>(result);
    }


    [Fact]
    public void GetById_ValidID_ReturnsEmployee()
    {
        //Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        employeeRepository
            .Setup(e => e.GetById(1))
            .Returns(new Employee { Id = 1, Name = "Mock Mock", Email = "mock@mock.com"});
        var employeeService = new EmployeeService(employeeRepository.Object);

        //Act
        var result = employeeService.GetById(1);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Employee>(result);
        employeeRepository.Verify(e => e.GetById(1), Times.Exactly(1));
    }
}
