using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Tests
{
    [TestFixture]
    public class TodoItemServiceTests
    {
        private TodoItemService _service;
        private Mock<DataContext> _mockContext;

        [SetUp]
        public void SetUp()
        {
            // Create a new mock DataContext and TodoItemService for each test
            _mockContext = new Mock<DataContext>();
            _service = new TodoItemService(_mockContext.Object);
        }

        [Test]
        public async Task GetTodoItems_ReturnsAllTodoItems()
        {
            var testData = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test1", Description = "Description1" },
                new TodoItem { Id = 2, Title = "Test2", Description = "Description2" },
                new TodoItem { Id = 3, Title = "Test3", Description = "Description3" }
            };
            var mockSet = new Mock<DbSet<TodoItem>>();
            mockSet.SetupData(testData);
            _mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

// Act
            var result = await _service.GetTodoItems();

// Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task GetTodoItem_WhenExists_ReturnsMatchingTodoItem()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test1", Description = "Description1" };
            _mockContext.Setup(c => c.TodoItems.FindAsync(1)).ReturnsAsync(testData);

            // Act
            var result = await _service.GetTodoItem(1);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task GetTodoItem_WhenNotExists_ReturnsNull()
        {
            // Arrange
            _mockContext.Setup(c => c.TodoItems.FindAsync(1)).ReturnsAsync(null as TodoItem);

            // Act
            var result = await _service.GetTodoItem(1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateTodoItem_CreatesNewTodoItem()
        {
            // Arrange
            var testData = new TodoItem { Title = "Test", Description = "Description" };
            _mockContext.Setup(c => c.TodoItems.Add(testData));
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _service.CreateTodoItem(testData);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task UpdateTodoItem_WhenExists_UpdatesTodoItem()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockContext.Setup(c => c.TodoItems.FindAsync(1)).ReturnsAsync(testData);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _service.UpdateTodoItem(testData);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task UpdateTodoItem_WhenNotExists_ReturnsNull()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockContext.Setup(c => c.TodoItems.FindAsync(1)).ReturnsAsync(null as TodoItem);

            // Act
            var result = await _service.UpdateTodoItem(testData);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteTodoItem_WhenExists_DeletesTodoItem()
        {
            // Arrange
            var testData = new TodoItem { Id = 1, Title = "Test", Description = "Description" };
            _mockContext.Setup(c => c.TodoItems.FindAsync(1)).ReturnsAsync(testData);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteTodoItem(1);

            // Assert
            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public async Task DeleteTodoItem_WhenNotExists_ReturnsNull()
        {
            // Arrange
            _mockContext.Setup(c => c.TodoItems.FindAsync(1)).ReturnsAsync(null as TodoItem);

            // Act
            var result = await _service.DeleteTodoItem(1);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
    
    public static class DbSetExtensions
    {
        public static void SetupData<TEntity>(this Mock<DbSet<TEntity>> mockSet, IList<TEntity> data) where TEntity : class
        {
            var queryable = data.AsQueryable();
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        }
    }
}