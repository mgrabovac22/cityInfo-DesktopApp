using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Lucija Polak, Marin Grabovac
    public class UserRepository : Repository<User>
    {
        public UserRepository() : base(new CityInfoModel())
        {
        }

        public void AddUser (User user)
        {
            Add(user);
        }

        public void RemoveUser(User user)
        {
            Remove(user);
        }

        public IQueryable<User> GetAllUsers()
        {
            return GetAll().Include("Institution").Include("UserType");
        }

        public User GetOneById(int id) {
             var query = from u in Entities where id == u.ID_User select u;
            return query.FirstOrDefault();
        }

        public User GetOneByName(string name)
        {
            var query = from u in Entities where name == u.Username select u;
            return query.FirstOrDefault();
        }

        public User GetUserWithRole(string username, string password)
        {
            var query = from u in Entities
                        join ut in Context.Set<UserType>() on u.ID_UserType equals ut.ID_UserType
                        where u.Username == username && u.Password == password
                        select u;

            return query.Include("UserType").FirstOrDefault();
        }

        public bool UsernameExists(string username)
        {
            return Entities.Any(u => u.Username == username);
        }

        public bool EmailExists(string email)
        {
            return Entities.Any(u => u.Email == email);
        }

        public bool UpdateSubscribedStatus(int id, short status)
        {
            var user = Entities.FirstOrDefault(u => u.ID_User == id);
            user.IsSubscribed = status;
            if (user != null) {
                Update(user);
                return true;
            }
            return false;
        }
        public bool GetSubscriptionStatus(int userId)
        {
            var user = Entities.FirstOrDefault(u => u.ID_User == userId);
            return user?.IsSubscribed == 1;
        }

        public List<User> GetSubsribedUsers()
        {
            var users = from u in Entities where u.IsSubscribed == 1 select u;
            return users.ToList();
        }
        public IQueryable<User> GetEmployees()
        {
            var query = from u in Entities join ut in Context.Set<UserType>() on u.ID_UserType equals ut.ID_UserType where ut.TypeName == "Admin" || ut.TypeName == "Employee" select u;
            return query.Include("UserType").Include("Institution");
        }

        public IQueryable<User> GetEmployeesFromInstitution(int id)
        {
            var query = from u in Entities join ins in Context.Set<Institution>() on u.ID_Institution equals ins.ID_Institution where ins.ID_Institution == id select u;
            return query.Include("Institution").Include("UserType");
        }

        public IQueryable<UserType> GetRoles()
        {
            var query = from u in Context.Set<UserType>() select u;
            return query;
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }

        public IQueryable<UserType> GetUserTypeById(int selectedValue)
        {
            var query = from u in Context.Set<UserType>() where u.ID_UserType == selectedValue select u;
            return query;
        }

        public IQueryable<User> GetUsersByRole(string role)
        {
            var query = from u in Entities join ut in Context.Set<UserType>() on u.ID_UserType equals ut.ID_UserType where ut.TypeName == role select u;
            return query.Include("UserType").Include("Institution");
        }
    }
}
