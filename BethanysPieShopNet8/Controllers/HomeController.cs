using BethanysPieShopNet8.Models;
using BethanysPieShopNet8.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IPieRepository pieRepository, ILogger<HomeController> logger)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Loading Home page with Pies of the Week...");

            var piesOfTheWeek = _pieRepository.PiesOfTheWeek;
            var homeViewModel = new HomeViewModel(piesOfTheWeek);
            return View(homeViewModel);
        }
    }
}
