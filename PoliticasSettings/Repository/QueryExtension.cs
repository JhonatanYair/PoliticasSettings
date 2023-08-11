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
        
        public static Expression<Func<T, bool>> BuildDynamicFilterExpression<T>(T filterModel)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var filterProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var conditions = new List<Expression>();
            foreach (var property in filterProperties)
            {
               
                //Ignora las propiedades virtuales.
                if (property.GetMethod.IsVirtual == true)
                {
                    continue;
                }

                //Si el campo es un llave primaria o foranea y esta en 0, se ignora.
                var value = property.GetValue(filterModel);
                if (property.Name.ToLower().Contains("id") == true && Equals(value, 0)) 
                { 
                    continue; 
                }

                if (value != null)
                {
                        var propertyAccess = Expression.Property(parameter, property);
                        var propertyValue = Expression.Constant(value);

                        if (property.PropertyType == typeof(int?))
                        {
                            int? nullableValue = (int?)value;
                            propertyValue = Expression.Constant(nullableValue, typeof(int?));

                        } 
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
            Console.WriteLine();
            return filterExpression;
        }

    }

}
