using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Npgsql;
using Project_PBO.Database;

namespace Project_PBO.Models
{
    public class TanamanModel
    {
        private int _idTanaman;
        private string _nama = string.Empty;
        private int _masaTanam = 0;
        private bool _isActive = true;

        public TanamanModel() { }

        internal TanamanModel(int idTanaman, string nama, int masaTanam, bool isActive = true)
        {
            _idTanaman = idTanaman;
            _nama = nama;
            _masaTanam = masaTanam;
            _isActive = isActive;
        }

        public int IdTanaman
        {
            get { return _idTanaman; }
            set { _idTanaman = value; }
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
        public int MasaTanam
        {
            get { return _masaTanam; }
            set
            {
                if (value > 0)
                    _masaTanam = value;
                else
                    throw new ArgumentException("Masa Tanam harus lebih dari 0");
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public static List<TanamanModel> GetAll()
        {
            List<TanamanModel> tanamanList = new List<TanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM tanaman ORDER BY id_tanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TanamanModel tanaman = new TanamanModel(
                    reader.GetInt32(0), // IdTanaman
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // MasaTanam
                    reader.GetBoolean(3) // IsActive
                );
                tanamanList.Add(tanaman);
            }
            return tanamanList;
        }

        public static List<TanamanModel> GetAllActive()
        {
            List<TanamanModel> tanamanList = new List<TanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM tanaman WHERE is_active = true ORDER BY id_tanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TanamanModel tanaman = new TanamanModel(
                    reader.GetInt32(0), // IdTanaman
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // MasaTanam
                    reader.GetBoolean(3) // IsActive
                );
                tanamanList.Add(tanaman);
            }
            return tanamanList;
        }

        public static TanamanModel? FindById(int IdTanaman)
        {
            TanamanModel? tanaman = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM tanaman WHERE id_tanaman = @id_tanaman";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_tanaman", IdTanaman);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                tanaman = new TanamanModel(
                    reader.GetInt32(0), // IdTanaman
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // MasaTanam
                    reader.GetBoolean(3) // IsActive
                );
            }
            return tanaman;
        }

        public static List<TanamanModel> GetByIdLahan(int idLahan)
        {
            List<TanamanModel> tanamanList = new List<TanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM tanaman WHERE id_lahan = @id_lahan ORDER BY id_tanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_lahan", idLahan);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TanamanModel tanaman = new TanamanModel(
                    reader.GetInt32(0), // IdTanaman
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // MasaTanam
                    reader.GetBoolean(3) // IsActive
                );
                tanamanList.Add(tanaman);
            }
            return tanamanList;
        }
        public bool Insert(TanamanModel tanaman)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "INSERT INTO tanaman (nama, masa_tanam, is_active) VALUES (@Nama, @MasaTanam, @IsActive)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("Nama", tanaman.Nama);
                    cmd.Parameters.AddWithValue("MasaTanam", tanaman.MasaTanam);
                    cmd.Parameters.AddWithValue("IsActive", tanaman.IsActive);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(TanamanModel tanaman)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "UPDATE tanaman SET nama = @Nama, masa_tanam = @MasaTanam, is_active = @IsActive WHERE id_tanaman = @IdTanaman";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("IdTanaman", tanaman.IdTanaman);
                    cmd.Parameters.AddWithValue("Nama", tanaman.Nama);
                    cmd.Parameters.AddWithValue("MasaTanam", tanaman.MasaTanam);
                    cmd.Parameters.AddWithValue("IsActive", tanaman.IsActive);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int idTanaman)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM tanaman WHERE id_tanaman = @id_tanaman";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id_tanaman", idTanaman);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static int CountAll()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT COUNT(*) FROM tanaman";
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
