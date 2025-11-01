using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using QuizApp.Controllers;
using QuizApp.DAL;
using QuizApp.DTOs;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Test.Controllers;

public class FlashCardControllerTests
{
    [Fact]
    public async Task TestGetFlashCards()
    {
        // arrange
        var mockFlashCards = new List<FlashCard>()
        {
            new() {
                FlashCardId = 1,
                Question = "What is the capital of Norway?",
                Answer = "Oslo",
                QuizId = 1,
                QuizQuestionNum = 1
            },
            new() {
                FlashCardId = 2,
                Question = "What is the capital of Sweden?",
                Answer = "Stockholm",
                QuizId = 1,
                QuizQuestionNum = 2
            }
        };
        var mockFlashCardDtos = new List<FlashCardDto>()
        {
            new()
            {
                FlashCardId = 1,
                Question = "What is the capital of Norway?",
                Answer = "Oslo",
                QuizId = 1,
                QuizQuestionNum = 1,
                Color = "blue"
            },
            new()
            {
                FlashCardId = 2,
                Question = "What is the capital of Sweden?",
                Answer = "Stockholm",
                QuizId = 1,
                QuizQuestionNum = 2,
                Color = "blue"
            }
        };
        var mockQuizId = 1;

        var mockFlashCardRepository = new Mock<IQuestionRepository<FlashCard>>();
        mockFlashCardRepository.Setup(repo => repo.GetAll(fc => fc.QuizId == mockQuizId)).ReturnsAsync(mockFlashCards);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        mockSerivce.Setup(service => service.PickRandomFlashCardColor()).Returns("blue");
        var mockLogger = new Mock<ILogger<FlashCardAPIController>>();
        var flashCardController = new FlashCardAPIController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.GetFlashCards(mockQuizId);

        // assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flashCardDtos = Assert.IsAssignableFrom<IEnumerable<FlashCardDto>>(okResult.Value);
        Assert.Equal(2, flashCardDtos.Count());
        flashCardDtos.Should().BeEquivalentTo(mockFlashCardDtos);
    }

    [Fact]
    public async Task TestCreate()
    {
        // arrange
        var mockFlashCard = new FlashCard
        {
            Question = "What is the capital of Norway?",
            Answer = "Oslo",
            QuizId = 1,
            QuizQuestionNum = 1
        };
        var mockFlashCardDto = new FlashCardDto
        {
            FlashCardId = 1,
            Question = "What is the capital of Norway?",
            Answer = "Oslo",
            QuizId = 1,
            QuizQuestionNum = 1
        };
        var mockFlashCardRepository = new Mock<IQuestionRepository<FlashCard>>();
        mockFlashCardRepository.Setup(repo => repo.Create(It.IsAny<FlashCard>())).ReturnsAsync(true);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardAPIController>>();
        var flashCardController = new FlashCardAPIController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act 
        var result = await flashCardController.Create(mockFlashCardDto);

        // assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(flashCardController.GetFlashCards), createdResult.ActionName);
        Assert.Equal(mockFlashCard.QuizId, createdResult.RouteValues!["quizId"]);

        var returnedFlashCard = Assert.IsType<FlashCard>(createdResult.Value);
        returnedFlashCard.Should().BeEquivalentTo(mockFlashCard);
    }

    [Fact]
    public async Task TestEdit()
    {
        // arrange
        var mockFlashCard = new FlashCard
        {
            FlashCardId = 1,
            Question = "What is the capital of Norway?",
            Answer = "Oslo",
            QuizId = 1,
            QuizQuestionNum = 1
        };
        var mockFlashCardDto = new FlashCardDto
        {
            FlashCardId = 1,
            Question = "What is the capital of Norway?",
            Answer = "Oslo",
            QuizId = 1,
            QuizQuestionNum = 1
        };

        int mockFlashCardId = 1;

        var mockFlashCardRepository = new Mock<IQuestionRepository<FlashCard>>();
        mockFlashCardRepository.Setup(repo => repo.Update(It.IsAny<FlashCard>())).ReturnsAsync(true);
        mockFlashCardRepository.Setup(repo => repo.GetById(mockFlashCardId)).ReturnsAsync(mockFlashCard);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardAPIController>>();
        var flashCardController = new FlashCardAPIController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.Edit(mockFlashCardId, mockFlashCardDto);

        // assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedFlashCard = Assert.IsAssignableFrom<FlashCard>(okResult.Value);
        returnedFlashCard.Should().BeEquivalentTo(mockFlashCard);
    }

    [Fact]
    public async Task TestDelete()
    {
        int mockFlashCardId = 1;
        int mockQNum = 1;
        int mockQuizId = 1;

        var mockFlashCardRepository = new Mock<IQuestionRepository<FlashCard>>();
        mockFlashCardRepository.Setup(repo => repo.Delete(mockFlashCardId)).ReturnsAsync(true);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardAPIController>>();
        var flashCardController = new FlashCardAPIController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.Delete(mockFlashCardId, mockQNum, mockQuizId);

        // assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }
}