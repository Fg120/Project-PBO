using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Npgsql;
using Project_PBO.Database;

namespace Project_PBO.Models
{
    public class AkunModel
    {
        private int _idAkun;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _role = string.Empty;
        private bool _isActive = true;

        public AkunModel() { }

        internal AkunModel(
            int idAkun,
            string username,
            string password,
            string role,
            bool isActive = true
        )
        {
            _idAkun = idAkun;
            _username = username;
            _password = password;
            _role = role;
            _isActive = isActive;
        }

        public int IdAkun
        {
            get { return _idAkun; }
            set { _idAkun = value; }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length >= 3)
                    _username = value;
                else
                    throw new ArgumentException("Username harus setidaknya 3 karakter");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length >= 6)
                    _password = value;
                else
                    throw new ArgumentException("Password harus setidaknya 6 karakter");
            }
        }

        public string Role
        {
            get { return _role; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _role = value;
                else
                    throw new ArgumentException("Role tidak boleh kosong");
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public static AkunModel Auth(string username, string password)
        {
            AkunModel? akun = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM akun WHERE username = @username AND password = @password";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                akun = new AkunModel(
                    reader.GetInt32(0), // IdAkun
                    reader.GetString(1), // Username
                    reader.GetString(2), // Password
                    reader.GetString(3), // Role
                    reader.GetBoolean(4) // IsActive
                );
            }
            return akun;
        }

        public static List<AkunModel> GetAll()
        {
            List<AkunModel> akunList = new List<AkunModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM akun ORDER BY id_akun ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AkunModel akun = new AkunModel(
                    reader.GetInt32(0), // IdAkun
                    reader.GetString(1), // Username
                    reader.GetString(2), // Password
                    reader.GetString(3), // Role
                    reader.GetBoolean(4) // IsActive
                );
                akunList.Add(akun);
            }
            return akunList;
        }

        public static List<AkunModel> GetAllActive()
        {
            List<AkunModel> akunList = new List<AkunModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM akun WHERE is_active = true ORDER BY id_akun ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AkunModel akun = new AkunModel(
                    reader.GetInt32(0), // IdAkun
                    reader.GetString(1), // Username
                    reader.GetString(2), // Password
                    reader.GetString(3), // Role
                    reader.GetBoolean(4) // IsActive
                );
                akunList.Add(akun);
            }
            return akunList;
        }

        public static AkunModel? FindById(int IdAkun)
        {
            AkunModel? akun = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT * FROM akun WHERE id_akun = @id_akun";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_akun", IdAkun);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                akun = new AkunModel(
                    reader.GetInt32(0), // IdAkun
                    reader.GetString(1), // Username
                    reader.GetString(2), // Password
                    reader.GetString(3), // Role
                    reader.GetBoolean(4) // IsActive
                );
            }
            return akun;
        }

        public bool Insert(AkunModel akun)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "INSERT INTO akun (username, password, role, is_active) VALUES (@Username, @Password, @Role, @IsActive)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("Username", akun.Username);
                    cmd.Parameters.AddWithValue("Password", akun.Password);
                    cmd.Parameters.AddWithValue("Role", akun.Role);
                    cmd.Parameters.AddWithValue("IsActive", akun.IsActive);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(AkunModel akun)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query =
                    "UPDATE akun SET username = @Username, password = @Password, role = @Role, is_active = @IsActive WHERE id_akun = @IdAkun";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("IdAkun", akun.IdAkun);
                    cmd.Parameters.AddWithValue("Username", akun.Username);
                    cmd.Parameters.AddWithValue("Password", akun.Password);
                    cmd.Parameters.AddWithValue("Role", akun.Role);
                    cmd.Parameters.AddWithValue("IsActive", akun.IsActive);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int idAkun)
        {
            using (var conn = DbContext.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM akun WHERE id_akun = @id_akun";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id_akun", idAkun);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static int CountAll()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT COUNT(*) FROM akun";
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
