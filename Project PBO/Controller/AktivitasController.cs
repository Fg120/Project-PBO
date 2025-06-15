using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Project_PBO.Database;
using Project_PBO.Models;

namespace Project_PBO.Controller
{
    public class AktivitasController
    {
        public static List<AktivitasModel> GetAllByIdPenanaman(int idPenanaman)
        {
            return AktivitasModel.GetByIdPenanaman(idPenanaman);
        }

        public static AktivitasModel? GetAktivitasById(int idAktivitas)
        {
            return AktivitasModel.FindById(idAktivitas);
        }

        public static bool AddAktivitas(AktivitasModel aktivitas)
        {
            return aktivitas.Insert(aktivitas);
        }

        public static bool UpdateAktivitas(AktivitasModel aktivitas)
        {
            return aktivitas.Update(aktivitas);
        }

        public static bool DeleteAktivitas(int idAktivitas)
        {
            return new AktivitasModel().Delete(idAktivitas);
        }

        public static int CountAllAktivitas()
        {
            return AktivitasModel.CountAll();
        }
    }
}
