namespace BethanysPieShopNet8.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        // Private field for the database context
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        // Constructor with dependency injection for BethanysPieShopDbContext
        public CategoryRepository(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        // Property to get all categories ordered by category name
        public IEnumerable<Category> AllCategories => _bethanysPieShopDbContext.Categories.OrderBy(c => c.CategoryName);
    }
}
