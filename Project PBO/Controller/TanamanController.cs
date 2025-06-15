using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Project_PBO.Database;
using Project_PBO.Models;

namespace Project_PBO.Controller
{
    public class TanamanController
    {
        public static List<TanamanModel> GetAllTanaman()
        {
            return TanamanModel.GetAll();
        }

        public static List<TanamanModel> GetAllActiveTanaman()
        {
            return TanamanModel.GetAllActive();
        }

        public static TanamanModel? GetTanamanById(int idTanaman)
        {
            return TanamanModel.FindById(idTanaman);
        }

        public static bool AddTanaman(TanamanModel tanaman)
        {
            return tanaman.Insert(tanaman);
        }

        public static bool UpdateTanaman(TanamanModel tanaman)
        {
            return tanaman.Update(tanaman);
        }

        public static bool DeleteTanaman(int idTanaman)
        {
            List<PenanamanModel> penanamanList = PenanamanModel.GetByIdTanaman(idTanaman);
            if (penanamanList != null && penanamanList.Count > 0)
            {
                // throw new InvalidOperationException("Tanaman tidak dapat dihapus karena masih memiliki lahan terkait.");
                return false;
            }
            return new TanamanModel().Delete(idTanaman);
        }

        public static int CountAllTanaman()
        {
            return TanamanModel.CountAll();
        }
    }
}
