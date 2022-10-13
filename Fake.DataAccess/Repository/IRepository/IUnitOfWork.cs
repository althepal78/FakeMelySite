namespace Fake.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }        
        IProductRepository Product { get; }      
        IApplicationUserRepository ApplicationUser { get; }
        IMenuRepository Menu { get; }
        void Save();
    }
}
