using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QuizApp.Controllers;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.Services;

namespace QuizApp.Test.Controllers;

public class FlashCardControllerTests
{
    [Fact]
    public async Task TestFlashCards()
    {
        // arrange
        var mockFlashCards = new List<FlashCard>()
        {
            new FlashCard
            {
                FlashCardId = 1,
                Question = "What is the capital of Norway?",
                Answer = "Oslo",
                QuizId = 1,
                QuizQuestionNum = 1
            },
            new FlashCard
            {
                FlashCardId = 2,
                Question = "What is the capital of Sweden?",
                Answer = "Stockholm",
                QuizId = 1,
                QuizQuestionNum = 2
            }
        };
        var mockQuizId = 1;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.GetAll(mockQuizId)).ReturnsAsync(mockFlashCards);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.FlashCards(mockQuizId);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var flashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(2, flashCardsViewModel.FlashCards.Count());
        Assert.Equal(mockFlashCards, flashCardsViewModel.FlashCards);
    }

    [Fact]
    public void TestRevealFlashCardAnswer()
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
        var mockFlashCardsViewModel = new FlashCardsViewModel
        {
            FlashCards = new List<FlashCard>
            {
                mockFlashCard
            }
        };
        var originalShowAnswer = mockFlashCardsViewModel.FlashCards.ElementAt(mockFlashCardsViewModel.CurrentFlashCardNum).ShowAnswer;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.RevealFlashCardAnswer(mockFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(mockFlashCardsViewModel.FlashCards.Count(), viewFlashCardsViewModel.FlashCards.Count());
        Assert.NotEqual(originalShowAnswer, viewFlashCardsViewModel.FlashCards.ElementAt(viewFlashCardsViewModel.CurrentFlashCardNum).ShowAnswer);
    }

    [Fact]
    public void TestNextFlashCardShouldIncrement()
    {
        // arrange
        var mockFlashCardsViewModel = new FlashCardsViewModel
        {
            FlashCards = new List<FlashCard>
            {
                new FlashCard
                {
                    FlashCardId = 1,
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    QuizId = 1,
                    QuizQuestionNum = 1
                },
                new FlashCard
                {
                    FlashCardId = 2,
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm",
                    QuizId = 1,
                    QuizQuestionNum = 2
                }
            }
        };
        var originalCurrentFlashCardNum = mockFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.NextFlashCard(mockFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum + 1, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestNextFlashCardShouldNotIncrement()
    {
        // arrange
        var mockFlashCardsViewModel = new FlashCardsViewModel
        {
            FlashCards = new List<FlashCard>
            {
                new FlashCard
                {
                    FlashCardId = 1,
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    QuizId = 1,
                    QuizQuestionNum = 1
                }
            }
        };
        var originalCurrentFlashCardNum = mockFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.NextFlashCard(mockFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestPrevFlashCardShouldDecrement()
    {
        // arrange
        var mockFlashCardsViewModel = new FlashCardsViewModel
        {
            CurrentFlashCardNum = 1,
            FlashCards = new List<FlashCard>
            {
                new FlashCard
                {
                    FlashCardId = 1,
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    QuizId = 1,
                    QuizQuestionNum = 1
                },
                new FlashCard
                {
                    FlashCardId = 2,
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm",
                    QuizId = 1,
                    QuizQuestionNum = 2
                }
            }
        };
        var originalCurrentFlashCardNum = mockFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.PrevFlashCard(mockFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum - 1, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestPrevFlashCardShouldNotDecrement()
    {
        // arrange
        var mockFlashCardsViewModel = new FlashCardsViewModel
        {
            FlashCards = new List<FlashCard>
            {
                new FlashCard
                {
                    FlashCardId = 1,
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    QuizId = 1,
                    QuizQuestionNum = 1
                }
            }
        };
        var originalCurrentFlashCardNum = mockFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.PrevFlashCard(mockFlashCardsViewModel);
        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestCreateGet()
    {
        // arrange
        int mockQuizId = 1;
        int mockNumOfQuestions = 0;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.Create(mockQuizId, mockNumOfQuestions);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCard = Assert.IsAssignableFrom<FlashCard>(viewResult.ViewData.Model);
        Assert.Equal(mockQuizId, viewFlashCard.QuizId);
        Assert.Equal(mockNumOfQuestions + 1, viewFlashCard.QuizQuestionNum);
    }

    [Fact]
    public async Task TestCreatePost()
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
        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.CreateFlashCard(mockFlashCard)).ReturnsAsync(false);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act 
        var result = await flashCardController.Create(mockFlashCard);

        // assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("ManageQuiz", redirectResult.ActionName);
        Assert.Equal("FlashCardQuiz", redirectResult.ControllerName);
        Assert.Equal(mockFlashCard.QuizId, redirectResult.RouteValues!["quizId"]);
    }

    [Fact]
    public async Task TestEditGet()
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

        var mockFlashCardId = 1;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.GetFlashCardById(mockFlashCardId)).ReturnsAsync(mockFlashCard);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.Edit(mockFlashCardId);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCard = Assert.IsAssignableFrom<FlashCard>(viewResult.ViewData.Model);
        Assert.Equal(mockFlashCardId, viewFlashCard.FlashCardId);
    }

    [Fact]
    public async Task TestEditPost()
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

        int mockQuizId = 1;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.UpdateFlashCard(mockFlashCard)).ReturnsAsync(true);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.Edit(mockFlashCard);

        // assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("ManageQuiz", redirectResult.ActionName);
        Assert.Equal("FlashCardQuiz", redirectResult.ControllerName);
        Assert.Equal(mockQuizId, redirectResult.RouteValues!["quizId"]);
    }

    [Fact]
    public async Task TestDelete()
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

        int mockFlashCardId = 1;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.GetFlashCardById(mockFlashCardId)).ReturnsAsync(mockFlashCard);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.Delete(mockFlashCardId);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCard = Assert.IsAssignableFrom<FlashCard>(viewResult.ViewData.Model);
        Assert.Equal(mockFlashCardId, viewFlashCard.FlashCardId);
    }

    [Fact]
    public async Task TestDeleteConfirmed()
    {
        int mockFlashCardId = 1;
        int mockQNum = 1;
        int mockQuizId = 1;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.DeleteFlashCard(mockFlashCardId)).ReturnsAsync(true);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = await flashCardController.DeleteConfirmed(mockFlashCardId, mockQNum, mockQuizId);

        // assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("ManageQuiz", redirectResult.ActionName);
        Assert.Equal("FlashCardQuiz", redirectResult.ControllerName);
        Assert.Equal(mockQuizId, redirectResult.RouteValues!["quizId"]);
    }
}