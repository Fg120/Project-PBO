using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Project_PBO.Database;
using Project_PBO.Models;

namespace Project_PBO.Controller
{
    public class PenanamanController
    {
        public static List<PenanamanModel> GetAllPenanaman()
        {
            return PenanamanModel.GetAll();
        }

        public static List<PenanamanModel> GetAllActivePenanaman()
        {
            return PenanamanModel.GetAllActive();
        }

        public static PenanamanModel? GetPenanamanById(int idPenanaman)
        {
            return PenanamanModel.FindById(idPenanaman);
        }

        public static bool AddPenanaman(PenanamanModel penanaman)
        {
            return penanaman.Insert();
        }

        public static bool UpdatePenanaman(PenanamanModel penanaman)
        {
            return penanaman.Update();
        }

        public static int CountAllPenanaman()
        {
            return PenanamanModel.CountAll();
        }

        public static List<PenanamanModel> GetTopFive()
        {
            return PenanamanModel.GetTopFive();
        }

        public static List<PenanamanModel> GetPenanamanByPetani(int idPetani)
        {
            return PenanamanModel.GetByPetani(idPetani);
        }
    }
}
