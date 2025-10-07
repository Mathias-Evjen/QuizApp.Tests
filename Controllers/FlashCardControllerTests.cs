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
        var flashCards = new List<FlashCard>()
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
        mockFlashCardRepository.Setup(repo => repo.GetAll(mockQuizId)).ReturnsAsync(flashCards);
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
        Assert.Equal(flashCards, flashCardsViewModel.FlashCards);
    }

    [Fact]
    public void TestRevealFlashCardAnswer()
    {
        // arrange
        var testFlashCard = new FlashCard
        {
            FlashCardId = 1,
            Question = "What is the capital of Norway?",
            Answer = "Oslo",
            QuizId = 1,
            QuizQuestionNum = 1
        };
        var testFlashCardsViewModel = new FlashCardsViewModel
        {
            FlashCards = new List<FlashCard>
            {
                testFlashCard
            }
        };
        var originalShowAnswer = testFlashCardsViewModel.FlashCards.ElementAt(testFlashCardsViewModel.CurrentFlashCardNum).ShowAnswer;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.RevealFlashCardAnswer(testFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(testFlashCardsViewModel.FlashCards.Count(), viewFlashCardsViewModel.FlashCards.Count());
        Assert.NotEqual(originalShowAnswer, viewFlashCardsViewModel.FlashCards.ElementAt(viewFlashCardsViewModel.CurrentFlashCardNum).ShowAnswer);
    }

    [Fact]
    public void TestNextFlashCardShouldIncrement()
    {
        // arrange
        var testFlashCardsViewModel = new FlashCardsViewModel
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
        var originalCurrentFlashCardNum = testFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.NextFlashCard(testFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum + 1, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestNextFlashCardShouldNotIncrement()
    {
        // arrange
        var testFlashCardsViewModel = new FlashCardsViewModel
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
        var originalCurrentFlashCardNum = testFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.NextFlashCard(testFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestPrevFlashCardShouldDecrement()
    {
        // arrange
        var testFlashCardsViewModel = new FlashCardsViewModel
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
        var originalCurrentFlashCardNum = testFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.PrevFlashCard(testFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum - 1, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestPrevFlashCardShouldNotDecrement()
    {
        // arrange
        var testFlashCardsViewModel = new FlashCardsViewModel
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
        var originalCurrentFlashCardNum = testFlashCardsViewModel.CurrentFlashCardNum;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.PrevFlashCard(testFlashCardsViewModel);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCardsViewModel = Assert.IsAssignableFrom<FlashCardsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(originalCurrentFlashCardNum, viewFlashCardsViewModel.CurrentFlashCardNum);
    }

    [Fact]
    public void TestCreateGet()
    {
        // arrange
        int quizId = 1;
        int numOfQuestions = 0;

        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act
        var result = flashCardController.Create(quizId, numOfQuestions);

        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFlashCard = Assert.IsAssignableFrom<FlashCard>(viewResult.ViewData.Model);
        Assert.Equal(quizId, viewFlashCard.QuizId);
        Assert.Equal(numOfQuestions + 1, viewFlashCard.QuizQuestionNum);
    }

    [Fact]
    public async Task TestCreatePost()
    {
        // arrange
        var testFlashCard = new FlashCard
        {
            FlashCardId = 1,
            Question = "What is the capital of Norway?",
            Answer = "Oslo",
            QuizId = 1,
            QuizQuestionNum = 1
        };
        var mockFlashCardRepository = new Mock<IFlashCardRepository>();
        mockFlashCardRepository.Setup(repo => repo.CreateFlashCard(testFlashCard)).ReturnsAsync(false);
        var mockSerivce = new Mock<IFlashCardQuizService>();
        var mockLogger = new Mock<ILogger<FlashCardController>>();
        var flashCardController = new FlashCardController(
            mockFlashCardRepository.Object,
            mockSerivce.Object,
            mockLogger.Object);

        // act 
        var result = await flashCardController.Create(testFlashCard);

        // assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("ManageQuiz", redirectResult.ActionName);
        Assert.Equal("FlashCardQuiz", redirectResult.ControllerName);
        Assert.Equal(testFlashCard.QuizId, redirectResult.RouteValues!["quizId"]);
    }
}