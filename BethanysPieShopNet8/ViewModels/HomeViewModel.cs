using BethanysPieShopNet8.Models;

namespace BethanysPieShopNet8.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Pie> PiesOfTheWeek { get; }

        public HomeViewModel(IEnumerable<Pie> piesOfTheWeek)
        {
            PiesOfTheWeek = piesOfTheWeek ?? throw new ArgumentNullException(nameof(piesOfTheWeek));
        }
    }
}
