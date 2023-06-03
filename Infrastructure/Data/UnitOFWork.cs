using Core.Entities;
using Core.Entities.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOFWork : IUnitOFWork

    {
        private readonly StoreDbContext _context;
        private Hashtable _Repos;
        public UnitOFWork(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
       =>await _context.SaveChangesAsync();

        public IGenericRepo<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_Repos is null)
                _Repos = new Hashtable();
            var Type=typeof(TEntity).Name;
            if(!_Repos.ContainsKey(Type))
            {
                var RepoType=typeof(GenericRepo<>);
                var RepoInstance=Activator.CreateInstance(RepoType.MakeGenericType(typeof(TEntity)),_context);

                _Repos.Add(Type, RepoInstance); 
            }
            return (IGenericRepo<TEntity>)_Repos[Type];
        }
    }
}
