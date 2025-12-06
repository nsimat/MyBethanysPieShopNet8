using BethanysPieShopNet8.Models;
using BethanysPieShopNet8.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers
{
    public class PieController : Controller
    {
        // Private fields for the pie repository, category repository, and logger
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<PieController> _logger;

        // Constructor with dependency injection for IPieRepository, ICategoryRepository, and ILogger
        public PieController(IPieRepository pieRepository,
                             ICategoryRepository categoryRepository,
                             ILogger<PieController> logger)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // Action method for listing pies, optionally filtered by category
        public IActionResult List(string category)
        {
            _logger.LogInformation("Loading pies list...");

            IEnumerable<Pie> pies;
            string? currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
                return View(new PieListViewModel(pies, currentCategory));

            }

            pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category)
                .OrderBy(p => p.PieId);

            currentCategory = _categoryRepository.AllCategories
                .FirstOrDefault(c => c.CategoryName == category)?.CategoryName;

            return View(new PieListViewModel(pies, currentCategory));
        }

        // Action method for displaying details of a specific pie by ID
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Loading pie details with ID: {id}...", id);

            var pie = _pieRepository.GetPieById(id);

            if (pie == null)
                return NotFound();

            return View(pie);
        }

        // Action method for displaying the search page
        public IActionResult Search()
        {
            return View();
        }
    }
}
