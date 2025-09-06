using BethanysPieShopNet8.Models;
using BethanysPieShopNet8.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<PieController> _logger;

        public PieController(IPieRepository pieRepository,
                             ICategoryRepository categoryRepository,
                             ILogger<PieController> logger)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public IActionResult List(string category)
        {
            _logger.LogInformation("Loading pies list...");

            IEnumerable<Pie> pies;
            string? currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category)
                    .OrderBy(p => p.PieId);

                currentCategory = _categoryRepository.AllCategories
                    .FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            return View(new PieListViewModel(pies, currentCategory));
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Loading pie details with ID: {id}...", id);

            var pie = _pieRepository.GetPieById(id);

            if (pie == null)
                return NotFound();

            return View(pie);
        }

        public IActionResult Search()
        {
            return View();
        }
    }
}
