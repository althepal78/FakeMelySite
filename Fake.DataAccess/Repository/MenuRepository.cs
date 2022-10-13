using Fake.DataAccess.Repository.IRepository;
using Fake.Models;

namespace Fake.DataAccess.Repository
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
   
        public void Update(Menu obj)
        {
            _db.Menus.Update(obj);
        }
    }
}
