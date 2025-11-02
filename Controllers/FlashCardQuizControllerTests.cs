using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using QuizApp.Controllers;
using QuizApp.DAL;
using QuizApp.DTOs;
using QuizApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace QuizApp.Test.Controllers
{
    public class FlashCardQuizControllerTests
    {
        [Fact]
        public async Task GetQuizzesOk()
        {
            // arrange
            var mockFlashCardQuizzes = new List<FlashCardQuiz>()
            {
                new ()
                {
                    FlashCardQuizId = 1,
                    Name = "Capitals of Scandinavia",
                    Description = "What are the capitals of Scandinavia?",
                    NumOfQuestions = 3
                },
                new()
                {
                    FlashCardQuizId = 2,
                    Name = "Who wrote?",
                    Description = "Which band wrote the song?",
                    NumOfQuestions = 5
                }
            };
            var mockFlashCardQuizDtos = new List<FlashCardQuizDto>()
            {
                new ()
                {
                    FlashCardQuizId = 1,
                    Name = "Capitals of Scandinavia",
                    Description = "What are the capitals of Scandinavia?",
                    NumOfQuestions = 3
                },
                new()
                {
                    FlashCardQuizId = 2,
                    Name = "Who wrote?",
                    Description = "Which band wrote the song?",
                    NumOfQuestions = 5
                }
            };

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.GetAll()).ReturnsAsync(mockFlashCardQuizzes);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.GetQuizzes();

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var quizDtos = Assert.IsAssignableFrom<IEnumerable<FlashCardQuizDto>>(okResult.Value);
            Assert.Equal(2, quizDtos.Count());
            quizDtos.Should().BeEquivalentTo(mockFlashCardQuizDtos);
        }

        [Fact]
        public async Task GetQuizzesNotFound()
        {
            // arrange
            var mockFlashCardQuizzes = new List<FlashCardQuiz>()
            {
                new ()
                {
                    FlashCardQuizId = 1,
                    Name = "Capitals of Scandinavia",
                    Description = "What are the capitals of Scandinavia?",
                    NumOfQuestions = 3
                },
                new()
                {
                    FlashCardQuizId = 2,
                    Name = "Who wrote?",
                    Description = "Which band wrote the song?",
                    NumOfQuestions = 5
                }
            };
            var mockFlashCardQuizDtos = new List<FlashCardQuizDto>()
            {
                new ()
                {
                    FlashCardQuizId = 1,
                    Name = "Capitals of Scandinavia",
                    Description = "What are the capitals of Scandinavia?",
                    NumOfQuestions = 3
                },
                new()
                {
                    FlashCardQuizId = 2,
                    Name = "Who wrote?",
                    Description = "Which band wrote the song?",
                    NumOfQuestions = 5
                }
            };

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.GetAll()).ReturnsAsync((FlashCardQuiz[]?)null);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.GetQuizzes();

            // assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("FlashCardQuizzes not found", notFoundResult.Value);
        }

        [Fact]
        public async Task TestGetQuizOk()
        {
            // arrange
            var mockFlashCardQuizzes = new List<FlashCardQuiz>()
            {
                new ()
                {
                    FlashCardQuizId = 1,
                    Name = "Capitals of Scandinavia",
                    Description = "What are the capitals of Scandinavia?",
                    NumOfQuestions = 3
                },
                new()
                {
                    FlashCardQuizId = 2,
                    Name = "Who wrote?",
                    Description = "Which band wrote the song?",
                    NumOfQuestions = 5
                }
            };

            var mockFlashCardQuizId = 2;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync(mockFlashCardQuizzes[1]);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.GetQuiz(mockFlashCardQuizId);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var quiz = Assert.IsAssignableFrom<FlashCardQuiz>(okResult.Value);
            quiz.Should().BeEquivalentTo(mockFlashCardQuizzes[1]);
        }

        [Fact]
        public async Task TestGetQuizNotfound()
        {
            // arrange
            var mockFlashCardQuizzes = new List<FlashCardQuiz>()
            {
                new ()
                {
                    FlashCardQuizId = 1,
                    Name = "Capitals of Scandinavia",
                    Description = "What are the capitals of Scandinavia?",
                    NumOfQuestions = 3
                },
                new()
                {
                    FlashCardQuizId = 2,
                    Name = "Who wrote?",
                    Description = "Which band wrote the song?",
                    NumOfQuestions = 5
                }
            };

            var mockFlashCardQuizId = 2;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync((FlashCardQuiz?)null);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.GetQuiz(mockFlashCardQuizId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("FlashCardQuiz not found", notFoundResult.Value);
        }

        [Fact]
        public async Task TestCreateOk()
        {
            // arrange 
            var mockNewQuiz = new FlashCardQuiz
            {
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };
            var mockQuizDto = new FlashCardQuizDto
            {
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Create(It.IsAny<FlashCardQuiz>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Create(mockQuizDto);

            // assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(flashCardQuizController.GetQuizzes), createdResult.ActionName);

            var returnedQuiz = Assert.IsType<FlashCardQuiz>(createdResult.Value);
            returnedQuiz.Should().BeEquivalentTo(mockNewQuiz);
        }

        [Fact]
        public async Task TestCreateDtoIsNull()
        {
            // arrange 
            var mockQuizDto = new FlashCardQuizDto
            {
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Create(It.IsAny<FlashCardQuiz>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Create(null);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Flash card quiz cannot be null", badRequestResult.Value);
        }

        [Fact]
        public async Task TestCreateFailed()
        {
            // arrange 
            var mockQuizDto = new FlashCardQuizDto
            {
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Create(It.IsAny<FlashCardQuiz>())).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Create(mockQuizDto);

            // assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public async Task TestUpdateOk()
        {
            // arrange 
            var mockQuiz = new FlashCardQuiz
            {
                FlashCardQuizId = 1,
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };
            var mockReturnedQuiz = new FlashCardQuiz
            {
                FlashCardQuizId = 1,
                Name = "Who wrote the song?",
                Description = "Which band/artist wrote the song?",
            };
            var mockQuizDto = new FlashCardQuizDto
            {
                FlashCardQuizId = 1,
                Name = "Who wrote the song?",
                Description = "Which band/artist wrote the song?",
            };

            var mockFlashCardQuizId = 1;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Update(It.IsAny<FlashCardQuiz>())).ReturnsAsync(true);
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync(mockQuiz);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Update(mockFlashCardQuizId, mockQuizDto);

            // assert
            var notFoundResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuiz = Assert.IsAssignableFrom<FlashCardQuiz>(notFoundResult.Value);
            returnedQuiz.Should().BeEquivalentTo(mockReturnedQuiz);
        }

        [Fact]
        public async Task TestUpdateDtoIsNull()
        {
            // arrange 
            var mockQuiz = new FlashCardQuiz
            {
                FlashCardQuizId = 1,
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };

            var mockFlashCardQuizId = 1;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Update(It.IsAny<FlashCardQuiz>())).ReturnsAsync(true);
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync(mockQuiz);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Update(mockFlashCardQuizId, null);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Flash card quiz cannot be null", badRequestResult.Value);
        }

        [Fact]
        public async Task TestUpdateExistingIsNull()
        {
            // arrange 
            var mockQuizDto = new FlashCardQuizDto
            {
                FlashCardQuizId = 1,
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };

            var mockFlashCardQuizId = 1;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Update(It.IsAny<FlashCardQuiz>())).ReturnsAsync(true);
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync((FlashCardQuiz?)null);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Update(mockFlashCardQuizId, mockQuizDto);

            // assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("FlashCardQuiz not found for the FlashCardQuizId", notFoundResult.Value);
        }

        [Fact]
        public async Task TestUpdateFailed()
        {
            // arrange 
            var mockQuiz = new FlashCardQuiz
            {
                FlashCardQuizId = 1,
                Name = "Who wrote?",
                Description = "Which band wrote the song?",
            };
            var mockQuizDto = new FlashCardQuizDto
            {
                FlashCardQuizId = 1,
                Name = "Who wrote the song?",
                Description = "Which band/artist wrote the song?",
            };

            var mockFlashCardQuizId = 1;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Update(It.IsAny<FlashCardQuiz>())).ReturnsAsync(false);
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync(mockQuiz);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Update(mockFlashCardQuizId, mockQuizDto);

            // assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public async Task TestUpdateIdNotEqual()
        {
            // arrange 
            var mockQuiz = new FlashCardQuiz
            {
                FlashCardQuizId = 2,
                Name = "Capitals of Scandinavia",
                Description = "What are the capitals of Scandinavia?",
            };
            var mockQuizDto = new FlashCardQuizDto
            {
                FlashCardQuizId = 1,
                Name = "Who wrote the song?",
                Description = "Which band/artist wrote the song?",
            };

            var mockFlashCardQuizId = 2;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Update(It.IsAny<FlashCardQuiz>())).ReturnsAsync(false);
            mockFlashCardQuizRepository.Setup(repo => repo.GetById(mockFlashCardQuizId)).ReturnsAsync(mockQuiz);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var result = await flashCardQuizController.Update(mockFlashCardQuizId, mockQuizDto);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Ids must match", badRequestResult.Value);
        }

        [Fact]
        public async Task TestDeleteOk()
        {
            // arrange
            int mockFlahCardQuizId = 1;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Delete(mockFlahCardQuizId)).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var restult = await flashCardQuizController.Delete(mockFlahCardQuizId);

            // assert
            var noContentResult = Assert.IsType<NoContentResult>(restult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task TestDeleteFailed()
        {
            // arrange
            int mockFlahCardQuizId = 1;

            var mockFlashCardQuizRepository = new Mock<IQuizRepository<FlashCardQuiz>>();
            mockFlashCardQuizRepository.Setup(repo => repo.Delete(mockFlahCardQuizId)).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<FlashCardQuizAPIController>>();
            var flashCardQuizController = new FlashCardQuizAPIController(
                mockFlashCardQuizRepository.Object,
                mockLogger.Object);

            // act
            var restult = await flashCardQuizController.Delete(mockFlahCardQuizId);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(restult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("FlashCardQuiz deletion failed", badRequestResult.Value);
        }
    }
}