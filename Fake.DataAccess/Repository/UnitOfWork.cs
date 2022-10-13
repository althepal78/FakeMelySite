using Fake.DataAccess.Repository.IRepository;
using Fake.Models;

namespace Fake.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
      
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);           
            ApplicationUser = new ApplicationUserRepository(_db);          
         
            Menu = new MenuRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }

        public IMenuRepository Menu { get; set; }

        public void Save()
        {
            _db.SaveChanges();  
        }
    }
}
