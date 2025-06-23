using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Project_PBO.Database;
using Project_PBO.Helpers;
using Project_PBO.Models;

namespace Project_PBO.Controller
{
    public class LahanController
    {
        public static List<LahanModel> GetAllLahan()
        {
            return LahanModel.GetAll();
        }

        public static List<LahanModel> GetAllActiveLahan()
        {
            return LahanModel.GetAllActive();
        }

        public static List<LahanModel> GetAllAvailableLahan()
        {
            return LahanModel.GetAllAvailable();
        }

        public static LahanModel? GetLahanById(int idLahan)
        {
            return LahanModel.FindById(idLahan);
        }

        public static bool AddLahan(LahanModel lahan)
        {
            return lahan.Insert(lahan);
        }

        public static bool UpdateLahan(LahanModel lahan)
        {
            return lahan.Update(lahan);
        }

        public static bool DeleteLahan(int idLahan)
        {
            List<PenanamanModel> penanamanList = PenanamanModel.GetByIdLahan(idLahan);
            if (penanamanList != null && penanamanList.Count > 0)
            {
                // throw new InvalidOperationException("Lahan tidak dapat dihapus karena masih memiliki penanaman terkait.");
                return false;
            }
            return new LahanModel().Delete(idLahan);
        }

        public static int CountAllLahan()
        {
            return LahanModel.CountAll();
        }
    }
}
