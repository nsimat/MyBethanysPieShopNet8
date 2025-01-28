using BethanysPieShopNet8.Models;
using BethanysPieShopNet8.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }


        public IActionResult List()
        {
            var pies = _pieRepository.AllPies;
            PieListViewModel pieListViewModel = new PieListViewModel(pies, "Cheese cakes");
            return View(pieListViewModel);
        }

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);

            if (pie == null)
                return NotFound();

            return View(pie);
        }
    }
}
