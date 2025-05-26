using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    //Jan Pobi
    public class ProblemService
    {
        public void AddProblem(string problemName, string description, DateTime dateOccured, int problemCategoryId, int userId, string problemReply = null)
        {
            try
            {
                var problem = new Problem
                {
                    ProblemName = problemName,
                    Description = description,
                    Solved = 0,
                    ReportDate = DateTime.Now,
                    DateOccured = dateOccured, 
                    ID_ProblemCategory = problemCategoryId,
                    ID_User = userId,
                    ProblemReply = problemReply
                };

                using (var repo = new ProblemRepository())
                {
                    repo.AddProblem(problem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }



        public List<Problem> GetAllProblemsWithUsernames()
        {
            using (var repo = new ProblemRepository())
            {
                var problems = repo.GetAllProblems()
                           .Include(p => p.User)
                               .ToList();


                var result = problems.Select(p => new Problem
                {
                    ID_Problem = p.ID_Problem,
                    ProblemName = p.ProblemName,
                    Description = p.Description,
                    Solved = p.Solved,
                    ReportDate = p.ReportDate,
                    DateOccured = p.DateOccured,
                    Status=p.Status,
                    ID_ProblemCategory = p.ID_ProblemCategory,
                    ID_User = p.ID_User,
                    ProblemReply = p.ProblemReply,
                    User = new User
                    {
                        Username = p.User?.Username
                    }
                }).ToList();

                return result;
            }
        }

        public List<Problem> GetMyProblems(int userId)
        {
            using (var repo = new ProblemRepository())
            {
                var problems = repo.GetAllProblems()
                                   .Include("User")
                                   .Where(p => p.ID_User == userId)
                                   .ToList();

                var result = problems.Select(p => new Problem
                {
                    ID_Problem = p.ID_Problem,
                    ProblemName = p.ProblemName,
                    Description = p.Description,
                    Solved = p.Solved,
                    ReportDate = p.ReportDate,
                    DateOccured = p.DateOccured,
                    ID_ProblemCategory = p.ID_ProblemCategory,
                    ID_User = p.ID_User,
                    ProblemReply = p.ProblemReply,
                    User = new User
                    {
                        Username = p.User?.Username
                    }
                }).ToList();

                return result;
            }
        }
        public List<Problem> SearchProblems(string problemName, int? categoryId, int? sortOrder)
        {
            using (var repo = new ProblemRepository())
            {
                var query = repo.GetAllProblems()
                                .Include(p => p.User)
                                .AsQueryable();

              
                if (!string.IsNullOrWhiteSpace(problemName))
                {
                    query = query.Where(p => p.ProblemName.Contains(problemName));
                }

              
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query = query.Where(p => p.ID_ProblemCategory == categoryId.Value);
                }

             
                if (sortOrder.HasValue)
                {
                    if (sortOrder.Value == 1) 
                    {
                        query = query.OrderByDescending(p => p.ReportDate);
                    }
                    else if (sortOrder.Value == 2) 
                    {
                        query = query.OrderBy(p => p.ReportDate);
                    }
                }

                return query.ToList();
            }
        }

        public void UpdateProblem(int problemId, string reply, string status, int userId)
        {
            try
            {
                using (var repo = new ProblemRepository())
                {
                    var problem = repo.GetProblemById(problemId);
                    if (problem != null)
                    {
                        problem.ProblemReply = reply;
                        problem.Status = status;
                        problem.ID_Employee = userId; 
                        repo.UpdateProblem(problem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating problem: {ex.Message}");
                throw;
            }
        }
        public (List<Problem>, int) GetPaginatedProblems(int page, int pageSize)
        {
            using (var repo = new ProblemRepository())
            {
                return repo.GetPaginatedProblems(page, pageSize);
            }
        }




    }
}
