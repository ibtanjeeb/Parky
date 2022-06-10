using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public bool CreatedTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return save();
        }

        public Trail GetTrail(int trailId)
        {
            var id =_db.Trails.Include(c=>c.NationalPark).FirstOrDefault(i => i.Id == trailId);
            return id;
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.OrderBy(a => a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
           bool value= _db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(a => a.Id == id);
        }

        public bool save()
        {
           return _db.SaveChanges()>=0 ? true:false;

        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);

            return save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
        }
    }
}
