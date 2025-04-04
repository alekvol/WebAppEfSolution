using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppEF.Controllers;
using WebAppEF.Models;

namespace WebAppEfTests
{
    public class SimpleSearchTest
    {
        [Fact]
        public async Task Search()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_WithSeed")
                // Отключаем вызов базового OnConfiguring
                .UseApplicationServiceProvider(null)
                .Options;

            // Модифицируем контекст для тестов
            using (var context = new TestApplicationContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new TestApplicationContext(options))
            {
                var controller = new StudentController(context);

                // Act - ищем студентов с буквой "e" в имени
                var result = await controller.Search("e");

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Student>>(viewResult.Model);

                var expectedNames = new[] { "Alex", "Coper", "Serega", "Endik"};
                Assert.Equal(expectedNames.Length, model.Count);
                Assert.All(model, s => Assert.Contains("e", s.Name.ToLower()));
            }
        }

        // Тестовый контекст без SQL Server конфигурации
        private class TestApplicationContext : ApplicationContext
        {
            public TestApplicationContext(DbContextOptions<ApplicationContext> options)
                : base(options) { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // Не вызываем базовый метод с SQL Server конфигурацией
                // Оставляем пустым для InMemory
            }
        }
    }
}