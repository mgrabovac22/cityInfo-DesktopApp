using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    //Marin Grabovac, Jan Pobi
    public class InstitutionService
    {

        public List<Institution> GetAllInstitutions()
        {
            using (var ir = new InstitutionRepository())
            {
                return ir.GetAllInstitutions().ToList();
            }
        }
        public void RemoveInstitution(Institution institution)
        {
            using (var ir = new InstitutionRepository())
            {
                ir.RemoveInstitution(institution);
            }
        }
        public void AddInstitution(Institution institution)
        {
            using (var ir = new InstitutionRepository())
            {
                ir.AddInstitution(institution);
            }
        }
        public void UpdateInstitution(Institution institution)
        {
            using (var ir = new InstitutionRepository())
            {
                ir.UpdateInstitution(institution);
            }
        }

        public Institution GetInstitutionById(int institutionId)
        {
            using (var ir = new InstitutionRepository())
            {
                return ir.GetInstitutionById(institutionId);
            }
        }

        public List<InstitutionType> GetInstitutionTypes()
        {
            using (var ir = new InstitutionRepository())
            {
                return ir.GetInstitutionTypes().ToList();
            }
        }

        public InstitutionType GetInstitutionTypeById(int institutionTypeId)
        {
            using (var ir = new InstitutionRepository())
            {
                return ir.GetInstitutionTypeById(institutionTypeId);
            }
        }
        public List<Institution> GetInstitutionsByType(int typeId)
        {
            using (var ir = new InstitutionRepository())
            {
                return ir.GetInstitutionsByType(typeId);
            }
        }

    }
}
