using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluate<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> specification)
        {
            var Query = inputQuery;
            if(specification.Criteria != null) 
            Query= Query.Where(specification.Criteria);

            if(specification.OrderBy != null)
                Query= Query.OrderBy(specification.OrderBy);

            if (specification.OrderByDesc != null)
                Query = Query.OrderByDescending(specification.OrderByDesc);
            if(specification.IsPagingEnable)
                Query=Query.Skip(specification.Skip).Take(specification.Take);

            Query = specification.Includes.Aggregate(Query, (current, include) => current.Include(include));
            return Query;
        }
    }
}
