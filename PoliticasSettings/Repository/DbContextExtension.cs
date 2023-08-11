using Microsoft.EntityFrameworkCore;
using PoliticasSettings.Models;
using PoliticasSettings.Datos;
using PoliticasSettings.Repository;
using System;
using System.Threading.Tasks; // Agrega esta línea
using System.Linq.Expressions;

namespace PoliticasSettings.Repository
{
    public class DbContextExtension : IDbContextExtension
    {

        private readonly DBPersonaContext context;

        public DbContextExtension(DBPersonaContext _context)
        {
            context = _context;
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsyncInclude<T>() where T : class
        {
            var includePaths = GetIncludePaths(typeof(T));
            IQueryable<T> query = context.Set<T>(); // Cambio DbSet a IQueryable

            foreach (var includePath in includePaths)
            {
                query = query.Include(includePath);
            }

            Console.WriteLine(query);
            Console.WriteLine();
            return await query.ToListAsync();
        }

        private List<string> GetIncludePaths(Type type)
        {
            var properties = type.GetProperties();
            var includePaths = new List<string>();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    // Solo incluir la propiedad de navegación si es una colección (ICollection)
                    includePaths.Add(property.Name);
                }
            }

            return includePaths;
        }

        //public async Task<IEnumerable<T>> GetFilter<T>() where T : class
        //{
        //    var Query = context.Set<T>().AsQueryable();

        //    var filterModel = new Product
        //    {
        //        Price = 20.0m
        //    };

        //    var filteredProducts = productsQuery.ApplyFilter(filterModel).ToList();
        //}

        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(int id) where T : class
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }
 
        public async Task<IEnumerable<T>> GetFilteredAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var query = context.Set<T>().AsQueryable();
            query = query.ApplyFilter(filter);
            return await query.ToListAsync();
        }

    }

    public interface IDbContextExtension
    {
        Task<T> GetByIdAsync<T>(int id) where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<IEnumerable<T>> GetAllAsyncInclude<T>() where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(int id) where T : class;
        Task<IEnumerable<T>> GetFilteredAsync<T>(Expression<Func<T, bool>> filter) where T : class;

    }

}
