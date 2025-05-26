using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Jan Pobi
    public class ProblemRepository : Repository<Problem>
    {
        public ProblemRepository() : base(new CityInfoModel())
        {
        }

        public void AddProblem(Problem problem)
        {
            Add(problem);
        }

        public Problem GetProblemById(int id)
        {
            return Entities.FirstOrDefault(p => p.ID_Problem == id);
        }

        public IQueryable<Problem> GetAllProblems()
        {
            return GetAll();
        }
        public void UpdateProblem(Problem problem)
        {
            var existingProblem = Entities.FirstOrDefault(p => p.ID_Problem == problem.ID_Problem);
            if (existingProblem != null)
            {
                existingProblem.ProblemReply = problem.ProblemReply;
                existingProblem.Status = problem.Status;
                existingProblem.ID_Employee = problem.ID_Employee; 
                SaveChanges();
            }
        }
        public (List<Problem>, int) GetPaginatedProblems(int page, int pageSize)
        {
            var query = GetAllProblems()
                        .Include(p => p.User)
                        .OrderByDescending(p => p.ReportDate);

            int totalRecords = query.Count();
            var paginatedProblems = query.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToList();

            return (paginatedProblems, totalRecords);
        }


        private void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
