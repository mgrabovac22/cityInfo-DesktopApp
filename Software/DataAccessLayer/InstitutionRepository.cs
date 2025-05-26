using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Marin Grabovac
    public class InstitutionRepository : Repository<Institution>
    {
        public InstitutionRepository() : base(new CityInfoModel())
        {
        }

        public void AddInstitution(Institution institution)
        {
            Add(institution);
        }

        public void RemoveInstitution(Institution institution)
        {
            Remove(institution);
        }

        public IQueryable<Institution> GetAllInstitutions()
        {
            return GetAll().Include("InstitutionType");
        }

        public void UpdateInstitution(Institution institution)
        {
            Update(institution);
        }

        public Institution GetInstitutionById(int institutionId)
        {
            var query = from i in Entities where i.ID_Institution == institutionId select i;
            return query.FirstOrDefault();
        }

        public IQueryable<InstitutionType> GetInstitutionTypes()
        {
            var query = from i in Context.Set<InstitutionType>() select i;
            return query;
        }

        public InstitutionType GetInstitutionTypeById(int institutionTypeId)
        {
            var query = from i in Context.Set<InstitutionType>() where i.ID_Type == institutionTypeId select i;
            return query.FirstOrDefault();
        }
        public List<Institution> GetInstitutionsByType(int typeId)
        {
            return Entities.Where(i => i.ID_Type == typeId).ToList();
        }
    }
}
