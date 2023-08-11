using System.Linq.Expressions;
using System.Reflection;

namespace PoliticasSettings.Repository
{
    public static class QueryExtension
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Expression<Func<T, bool>> filter)
        {
            return query.Where(filter);
        }

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, T filterModel)
        {
            var filterProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Expression<Func<T, bool>> combinedFilter = null;

            foreach (var property in filterProperties)
            {
                var value = property.GetValue(filterModel);

                if (value != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var propertyAccess = Expression.Property(parameter, property);
                    var propertyValue = Expression.Constant(value);
                    var equality = Expression.Equal(propertyAccess, propertyValue);
                    var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

                    if (combinedFilter == null)
                    {
                        combinedFilter = lambda;
                    }
                    else
                    {
                        var body = Expression.AndAlso(combinedFilter.Body, lambda.Body);
                        combinedFilter = Expression.Lambda<Func<T, bool>>(body, combinedFilter.Parameters);
                    }
                }
            }

            if (combinedFilter != null)
            {
                return query.Where(combinedFilter);
            }

            return query;
        }
    }

}
