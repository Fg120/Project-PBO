using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Npgsql;
using Project_PBO.Database;

namespace Project_PBO.Models
{
    public class LahanModel
    {
        private int _idLahan;
        private string _nama = string.Empty;
        private int _luas = 0;
        private string _lokasi = string.Empty;
        private bool _isActive = true;

        public LahanModel() { }

        internal LahanModel(int idLahan, string nama, int luas, string lokasi, bool isActive = true)
        {
            _idLahan = idLahan;
            _nama = nama;
            _luas = luas;
            _lokasi = lokasi;
            _isActive = isActive;
        }

        public int IdLahan
        {
            get { return _idLahan; }
            set { _idLahan = value; }
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
        public int Luas
        {
            get { return _luas; }
            set
            {
                if (value > 0)
                    _luas = value;
                else
                    throw new ArgumentException("Luas harus lebih dari 0");
            }
        }

        public string Lokasi
        {
            get { return _lokasi; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _lokasi = value;
                else
                    throw new ArgumentException("Lokasi tidak boleh kosong");
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public static List<LahanModel> GetAll()
        {
            List<LahanModel> lahanList = new List<LahanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM lahan ORDER BY id_lahan ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LahanModel lahan = new LahanModel(
                    reader.GetInt32(0), // IdLahan
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // Luas
                    reader.GetString(3), // Lokasi
                    reader.GetBoolean(4) // IsActive
                );
                lahanList.Add(lahan);
            }
            return lahanList;
        }

        public static List<LahanModel> GetAllActive()
        {
            List<LahanModel> lahanList = new List<LahanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM lahan WHERE is_active = true ORDER BY id_lahan ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LahanModel lahan = new LahanModel(
                    reader.GetInt32(0), // IdLahan
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // Luas
                    reader.GetString(3), // Lokasi
                    reader.GetBoolean(4) // IsActive
                );
                lahanList.Add(lahan);
            }
            return lahanList;
        }

        public static LahanModel? FindById(int IdLahan)
        {
            LahanModel? lahan = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM lahan WHERE id_lahan = @id_lahan";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_lahan", IdLahan);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                lahan = new LahanModel(
                    reader.GetInt32(0), // IdLahan
                    reader.GetString(1), // Nama
                    reader.GetInt32(2), // Luas
                    reader.GetString(3), // Lokasi
                    reader.GetBoolean(4) // IsActive
                );
            }
            return lahan;
        }

        public bool Insert(LahanModel lahan)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "INSERT INTO lahan (nama, luas, lokasi, is_active) VALUES (@Nama, @Luas, @Lokasi, @IsActive)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("Nama", lahan.Nama);
                    cmd.Parameters.AddWithValue("Luas", lahan.Luas);
                    cmd.Parameters.AddWithValue("Lokasi", lahan.Lokasi);
                    cmd.Parameters.AddWithValue("IsActive", lahan.IsActive);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(LahanModel lahan)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "UPDATE lahan SET nama = @Nama, luas = @Luas, lokasi = @Lokasi, is_active = @IsActive WHERE id_lahan = @IdLahan";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("IdLahan", lahan.IdLahan);
                    cmd.Parameters.AddWithValue("Nama", lahan.Nama);
                    cmd.Parameters.AddWithValue("Luas", lahan.Luas);
                    cmd.Parameters.AddWithValue("Lokasi", lahan.Lokasi);
                    cmd.Parameters.AddWithValue("IsActive", lahan.IsActive);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int idLahan)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM lahan WHERE id_lahan = @id_lahan";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id_lahan", idLahan);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static int CountAll()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT COUNT(*) FROM lahan";
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
