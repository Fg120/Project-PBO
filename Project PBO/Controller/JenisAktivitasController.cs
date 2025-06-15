using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Project_PBO.Database;
using Project_PBO.Models;

namespace Project_PBO.Controller
{
    public class JenisAktivitasController
    {
        public static List<JenisAktivitasModel> GetAllJenisAktivitas()
        {
            return JenisAktivitasModel.GetAll();
        }

        public static JenisAktivitasModel? GetJenisAktivitasById(int idJenisAktivitas)
        {
            return JenisAktivitasModel.FindById(idJenisAktivitas);
        }

        public static bool AddJenisAktivitas(JenisAktivitasModel jenisAktivitas)
        {
            return jenisAktivitas.Insert(jenisAktivitas);
        }

        public static bool UpdateJenisAktivitas(JenisAktivitasModel jenisAktivitas)
        {
            return jenisAktivitas.Update(jenisAktivitas);
        }

        public static bool DeleteJenisAktivitas(int idJenisAktivitas)
        {
            List<AktivitasModel> aktivitasList = AktivitasModel.GetByIdJenisAktivitas(idJenisAktivitas);
            if (aktivitasList != null && aktivitasList.Count > 0)
            {
                // throw new InvalidOperationException("Jenis Aktivitas tidak dapat dihapus karena masih memiliki aktivitas terkait.");
                return false;
            }
            return new JenisAktivitasModel().Delete(idJenisAktivitas);
        }

        public static int CountAllJenisAktivitas()
        {
            return JenisAktivitasModel.CountAll();
        }
    }
}
