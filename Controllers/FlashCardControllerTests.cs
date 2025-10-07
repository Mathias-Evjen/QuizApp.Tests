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
    

    // [Fact]
    // public async Task TestCreateNotOk() 
    // {

    // }
}