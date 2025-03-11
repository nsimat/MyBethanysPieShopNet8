using BethanysPieShopNet8.Controllers;
using BethanysPieShopNet8.ViewModels;
using BethanysPieShopNet8Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8Tests.Controllers
{
    public class PieControllerTests
    {
        [Fact]
        public void list_EmptyCategory_ReturnsAllPies()
        {
            // Arrange
            var mockPieRepository = RepositoryMocks.GetPieRepository();
            var mockCategoryRepository = RepositoryMocks.GetCategoryRepository();

            var pieController = new PieController(mockPieRepository.Object, mockCategoryRepository.Object);

            // Act
            var result = pieController.List("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var pieListViewModel = Assert.IsAssignableFrom<PieListViewModel>(viewResult.ViewData.Model);
            Assert.Equal(10, pieListViewModel.Pies.Count());
        }
    }
}
