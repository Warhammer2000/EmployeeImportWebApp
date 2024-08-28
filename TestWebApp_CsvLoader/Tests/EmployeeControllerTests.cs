using Moq;
using Xunit;
using EmployeeImportApp.Services;
using EmployeeImportApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using EmployeeImportApp.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using EmployeeImportApp.Models;

namespace EmployeeImportApp.Tests
{
    public class EmployeeControllerTests
    {
        private readonly EmployeeController _controller;
        private readonly Mock<IEmployeeRepository> _mockRepository;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<ILogger<EmployeeService>> _mockServiceLogger;
        private readonly Mock<ILogger<EmployeeController>> _mockControllerLogger;

        public EmployeeControllerTests()
        {
            // Create mocks for all dependencies
            _mockRepository = new Mock<IEmployeeRepository>();
            _mockCache = new Mock<IMemoryCache>();
            _mockServiceLogger = new Mock<ILogger<EmployeeService>>();
            _mockControllerLogger = new Mock<ILogger<EmployeeController>>();

            // Initialize the service
            var employeeService = new EmployeeService(_mockRepository.Object, _mockCache.Object, _mockServiceLogger.Object);

            // Initialize the controller
            _controller = new EmployeeController(employeeService, _mockControllerLogger.Object);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                ["ImportResult"] = null
            };
        }

        [Fact]
        public async Task UploadFile_ShouldReturnRedirectToAction()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = "This is a dummy file content";
            var fileName = "dummy.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.Length).Returns(ms.Length);

            // Act
            var result = await _controller.Import(mockFile.Object) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
        [Fact]
        public async Task CreateEmployee_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _controller.Create(employee) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task EditEmployee_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(employee.EmployeeId))
                           .ReturnsAsync(employee);

            // Act
            var result = await _controller.Edit(employee.EmployeeId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(employee.EmployeeId))
                           .ReturnsAsync(employee);

            // Act
            var result = await _controller.Delete(employee.EmployeeId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
