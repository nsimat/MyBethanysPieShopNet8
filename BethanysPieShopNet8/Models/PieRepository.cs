
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopNet8.Models
{
    public class PieRepository : IPieRepository
    {
        // Private field for the database context
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        // Constructor with dependency injection for BethanysPieShopDbContext
        public PieRepository(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        // Property to get all pies including their categories
        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _bethanysPieShopDbContext.Pies.Include(c => c.Category);
            }
        }

        // Property to get pies that are marked as "Pie of the Week" including their categories
        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _bethanysPieShopDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        // Method to get a pie by its ID including its category
        public Pie? GetPieById(int pieId)
        {
            return _bethanysPieShopDbContext.Pies.Include(c => c.Category).FirstOrDefault(p => p.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            return _bethanysPieShopDbContext.Pies.Include(c => c.Category).Where(p => p.Name.Contains(searchQuery));
        }
    }
}
