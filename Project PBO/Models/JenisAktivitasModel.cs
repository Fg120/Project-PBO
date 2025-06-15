using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Npgsql;
using Project_PBO.Database;

namespace Project_PBO.Models
{
    public class JenisAktivitasModel
    {
        private int _idJenisAktivitas;
        private string _nama = string.Empty;

        public JenisAktivitasModel() { }

        internal JenisAktivitasModel(int idJenisAktivitas, string nama)
        {
            _idJenisAktivitas = idJenisAktivitas;
            _nama = nama;
        }

        public int IdJenisAktivitas
        {
            get { return _idJenisAktivitas; }
            set { _idJenisAktivitas = value; }
        }

        public string Nama
        {
            get { return _nama; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _nama = value;
                else
                    throw new ArgumentException("Nama tidak boleh kosong");
            }
        }

        public static List<JenisAktivitasModel> GetAll()
        {
            List<JenisAktivitasModel> jenisAktivitasList = new List<JenisAktivitasModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM jenis_aktivitas ORDER BY id_jenis_aktivitas ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                JenisAktivitasModel jenisAktivitas = new JenisAktivitasModel(
                    reader.GetInt32(0), // IdJenisAktivitas
                    reader.GetString(1) // Nama
                );
                jenisAktivitasList.Add(jenisAktivitas);
            }
            return jenisAktivitasList;
        }

        public static JenisAktivitasModel? FindById(int IdJenisAktivitas)
        {
            JenisAktivitasModel? jenisAktivitas = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT * FROM jenis_aktivitas WHERE id_jenis_aktivitas = @id_jenis_aktivitas";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_jenis_aktivitas", IdJenisAktivitas);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                jenisAktivitas = new JenisAktivitasModel(
                    reader.GetInt32(0), // IdJenisAktivitas
                    reader.GetString(1) // Nama
                );
            }
            return jenisAktivitas;
        }

        public bool Insert(JenisAktivitasModel jenisAktivitas)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO jenis_aktivitas (nama) VALUES (@Nama)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("Nama", jenisAktivitas.Nama);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(JenisAktivitasModel jenisAktivitas)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "UPDATE jenis_aktivitas SET nama = @Nama WHERE id_jenis_aktivitas = @IdJenisAktivitas";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "IdJenisAktivitas",
                        jenisAktivitas.IdJenisAktivitas
                    );
                    cmd.Parameters.AddWithValue("Nama", jenisAktivitas.Nama);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int idJenisAktivitas)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "DELETE FROM jenis_aktivitas WHERE id_jenis_aktivitas = @id_jenis_aktivitas";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id_jenis_aktivitas", idJenisAktivitas);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static int CountAll()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT COUNT(*) FROM jenis_aktivitas";
            using var cmd = new NpgsqlCommand(query, conn);
            var result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }
    }
}
