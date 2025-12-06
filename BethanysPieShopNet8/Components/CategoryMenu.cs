using BethanysPieShopNet8.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Components
{
    public class CategoryMenu : ViewComponent
    {
        // Private field for the category repository
        private readonly ICategoryRepository _categoryRepository;

        // Constructor with dependency injection for ICategoryRepository
        public CategoryMenu(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        // Invoke method to render the category menu
        public IViewComponentResult Invoke()
        {
            var categories = _categoryRepository.AllCategories.OrderBy(c => c.CategoryName);

            return View(categories);
        }
    }
}
