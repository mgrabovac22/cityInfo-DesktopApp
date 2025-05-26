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
    public class CommunalRepository : Repository<PeriodLocation>
    {
        public CommunalRepository() : base(new CityInfoModel())
        {
        }

        public void AddPL(PeriodLocation data)
        {
            Add(data);
        }

        public PeriodLocation GetOneByLocation(string location)
        {
            var query = from pl in Entities where location == pl.Location select pl;
            return query.FirstOrDefault();
        }

        public PeriodLocation GetOneByLocationArea(string locationArea)
        {
            var query = from pl in Entities where locationArea == pl.Location_area select pl;
            return query.FirstOrDefault();
        }
    }
}
