using Core.Entities;
using Core.Entities.Interface;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepo(StoreDbContext context)
        {
            _context = context;
        }
        public  void Add(T entity)

        => _context.Set<T>().Add(entity);

        public void Delete(T entity)
         =>_context.Set<T>().Remove(entity);

        public async Task<T> GetByIdAsync(int id)

         => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetEntityWithSpecification(ISpecification<T> specification)
        =>await ApplySpecifications( specification).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> ListAllAsync()

         =>await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
         => await ApplySpecifications(specification).ToListAsync();

        private IQueryable<T> ApplySpecifications(ISpecification<T> specification)
            => SpecificationEvaluate<T>.GetQuery(_context.Set<T>().AsQueryable(),specification);
        public void Update(T entity)
       =>_context.Set<T>().Update(entity);

        public async Task<int> CountAsync(ISpecification<T> specification)
       => await ApplySpecifications(specification).CountAsync();
    }
}
