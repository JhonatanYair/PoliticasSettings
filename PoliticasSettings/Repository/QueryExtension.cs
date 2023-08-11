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

        public static Expression<Func<T, bool>> BuildDynamicFilterExpression<T>(T filterModel)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var filterProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var conditions = new List<Expression>();
            foreach (var property in filterProperties)
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    continue; // Ignorar propiedades de navegación y virtuales
                }

                var value = property.GetValue(filterModel);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    var propertyAccess = Expression.Property(parameter, property);
                    var propertyValue = Expression.Constant(value);
                    var equality = Expression.Equal(propertyAccess, propertyValue);
                    conditions.Add(equality);
                }
            }

            if (conditions.Count == 0)
            {
                return null; // No hay condiciones para aplicar
            }

            var combinedConditions = conditions.Aggregate(Expression.And);
            var filterExpression = Expression.Lambda<Func<T, bool>>(combinedConditions, parameter);
            return filterExpression;
        }

    }

}
