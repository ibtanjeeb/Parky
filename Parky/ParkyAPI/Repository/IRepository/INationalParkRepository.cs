using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);

        bool CreatedNatonalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNatonalPark(NationalPark nationalPark);

        bool save();

    }
}
