using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Project_PBO.Database;
using Project_PBO.Models;

namespace Project_PBO.Controller
{
    public class AkunController
    {
        public static AkunModel? Login(string username, string password)
        {
            return AkunModel.Auth(username, password);
        }

        public static List<AkunModel> GetAllAkun()
        {
            return AkunModel.GetAll();
        }

        public static List<AkunModel> GetAllActiveAkun()
        {
            return AkunModel.GetAllActive();
        }

        public static AkunModel? GetAkunById(int idAkun)
        {
            return AkunModel.FindById(idAkun);
        }

        public static AkunModel? GetAkunByUsername(string username)
        {
            return AkunModel.FindByUsername(username);
        }

        public static bool AddAkun(AkunModel akun)
        {
            return akun.Insert(akun);
        }

        public static bool UpdateAkun(AkunModel akun)
        {
            return akun.Update(akun);
        }

        public static bool DeleteAkun(int idAkun)
        {
            List<AktivitasModel> aktivitasList = AktivitasModel.GetByIdAkun(idAkun);
            if (aktivitasList != null && aktivitasList.Count > 0)
            {
                // throw new InvalidOperationException("Akun tidak dapat dihapus karena masih memiliki aktivitas terkait.");
                return false;
            }
            return new AkunModel().Delete(idAkun);
        }

        public static int CountAllAkun()
        {
            return AkunModel.CountAll();
        }
    }
}
