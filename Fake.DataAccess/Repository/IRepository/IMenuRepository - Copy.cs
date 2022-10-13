using Fake.Models;

namespace Fake.DataAccess.Repository.IRepository
{
    public interface IMenuRepository : IRepository<Menu>
    {
        void Update(Menu obj);
    }

}
