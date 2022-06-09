using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;
        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public bool CreatedNatonalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Add(nationalPark);
            return save();
        }

        public bool DeleteNatonalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Remove(nationalPark);
            return save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            var id =_db.NationalPark.FirstOrDefault(i => i.Id == nationalParkId);
            return id;
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalPark.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
           bool value= _db.NationalPark.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalPark.Any(a => a.Id == id);
        }

        public bool save()
        {
           return _db.SaveChanges()>=0 ? true:false;

        }

        public bool UpdateNatonalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Update(nationalPark);

            return save();
        }
    }
}
