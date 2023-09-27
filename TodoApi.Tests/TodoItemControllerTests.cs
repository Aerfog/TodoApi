using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Tests
{
    [TestFixture]
    public class TodoItemsControllerTests
    {
        private Mock<ITodoItemService> _mockService;
        private TodoItemsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITodoItemService>();
            _controller = new TodoItemsController(_mockService.Object);
        }

        [Test]
        public async Task GetTodoItems_ReturnsAllTodoItems()
        {
            // Arrange
            var testData = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test1", Description = "Description1" },
                new TodoItem { Id = 2, Title = "Test2", Description = "Description2" },
                new TodoItem { Id = 3, Title = "Test3", Description = "Description3" }
            };
            _mockService.Setup(s => s.GetTodoItems()).ReturnsAsync(testData);

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<TodoItem>>>());
            var actionResult = result as ActionResult<IEnumerable<TodoItem>>;
            Assert.That(actionResult.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = actionResult.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(testData));
        }

        [Test]
        public async Task GetTodoItem_WhenExists_ReturnsMatchingTodoItem()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockService.Setup(s => s.GetTodoItem(1)).ReturnsAsync(testData);

            // Act
            var result = await _controller.GetTodoItem(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<TodoItem>>());
            var actionResult = result as ActionResult<TodoItem>;
            Assert.That(actionResult.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = actionResult.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(testData));
        }

        [Test]
        public async Task GetTodoItem_WhenNotExists_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetTodoItem(1)).ReturnsAsync(null as TodoItem);

            // Act
            var result = await _controller.GetTodoItem(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<TodoItem>>());
            var actionResult = result as ActionResult<TodoItem>;
            Assert.That(actionResult.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostTodoItem_CreatesNewTodoItemAndReturnsCreatedAtAction()
        {
            // Arrange
            var testData = new TodoItem { Title = "Test", Description = "Description" };

            // Act
            var result = await _controller.PostTodoItem(testData);

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<TodoItem>>());
            var actionResult = result as ActionResult<TodoItem>;
            Assert.That(actionResult.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = actionResult.Result as CreatedAtActionResult;
            Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetTodoItem)));
            Assert.That(createdResult.RouteValues["id"], Is.EqualTo(testData.Id));
            Assert.That(createdResult.Value, Is.EqualTo(testData));
        }

        [Test]
        public async Task PutTodoItem_WhenItemExists_UpdatesTodoItemAndReturnsOk()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockService.Setup(s => s.UpdateTodoItem(testData)).ReturnsAsync(testData);

            // Act
            var result = await _controller.PutTodoItem(1, testData);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(testData));
        }

        [Test]
        public async Task PutTodoItem_WhenIdInPathDoesNotMatchIdInRequestBody_ReturnsBadRequest()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };

            // Act
            var result = await _controller.PutTodoItem(2, testData);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task PutTodoItem_WhenItemDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockService.Setup(s => s.UpdateTodoItem(testData)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.PutTodoItem(1, testData);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteTodoItem_WhenItemExists_DeletesTodoItemAndReturnsNoContent()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockService.Setup(s => s.DeleteTodoItem(1)).ReturnsAsync(testData);

            // Act
            var result = await _controller.DeleteTodoItem(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteTodoItem_WhenItemDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteTodoItem(1)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.DeleteTodoItem(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}