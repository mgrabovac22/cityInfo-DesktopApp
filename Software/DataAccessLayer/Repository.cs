using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Marin Grabovac, Lucija Polak
    public abstract class Repository<T> : IDisposable where T : class
    {
        public DbSet<T> Entities { get; set; }
        protected CityInfoModel Context { get; set; }

        protected Repository(CityInfoModel context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public IQueryable<T> GetAll()
        {
            var query = from e in Entities select e;
            return query;
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
            Context.SaveChanges();
        }

        public void Remove(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Entities.Attach(entity);
            }

            Entities.Remove(entity);
            Context.SaveChanges();
        }


        public void Update(T entity) {
            Entities.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
