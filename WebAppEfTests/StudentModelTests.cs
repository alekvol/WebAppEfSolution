
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppEF.Controllers;
using WebAppEF.Models;
using Xunit;

namespace WebAppEfTests
{
    public class StudentModelTests
    {
            /// <summary>
            /// Проверка для инцилизации 
            /// </summary>
            [Fact]
            public void StudentPropertiesSetCorrectly()
            {
                // Arrange & Act
                var student = new Student(1, "John Doe", 20, "123 Main St");

                // Assert
                Assert.Equal(1, student.Id);
                Assert.Equal("John Doe", student.Name);
                Assert.Equal(20, student.Age);
                Assert.Equal("123 Main St", student.Adress);
            }

            /// <summary>
            /// Проверка конструктора
            /// </summary>
            [Fact]
            public void StudentDefaultConstructorInitializesCorrectly()
            {
                // Arrange & Act
                var student = new Student();

                // Assert
                Assert.Equal(0, student.Id);
                Assert.Null(student.Name);
                Assert.Null(student.Age);
                Assert.Null(student.Adress);
            }

            /// <summary>
            /// Проверка на Null свойства Age
            /// </summary>
            [Fact]
            public void StudentAgeCanBeNullable()
            {
                // Arrange & Act
                var student = new Student { Age = null };

                // Assert
                Assert.Null(student.Age);
            }
    }
}
