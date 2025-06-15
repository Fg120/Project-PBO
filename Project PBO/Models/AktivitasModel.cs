using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Npgsql;
using Project_PBO.Database;

namespace Project_PBO.Models
{
    public class AktivitasModel
    {
        private int _idAktivitas;
        private int _idPenanaman;
        private int _idJenisAktivitas;
        private int _idAkun;
        private DateTime _waktu;
        private string _catatan = string.Empty;

        private PenanamanModel? _penanaman;
        private JenisAktivitasModel? _jenisAktivitas;
        private AkunModel? _akun;

        public AktivitasModel() { }

        internal AktivitasModel(
            int idAktivitas,
            int idPenanaman,
            int idJenisAktivitas,
            int idAkun,
            DateTime waktu,
            string catatan
        )
        {
            _idAktivitas = idAktivitas;
            _idPenanaman = idPenanaman;
            _idJenisAktivitas = idJenisAktivitas;
            _idAkun = idAkun;
            _waktu = waktu;
            _catatan = catatan;
        }
        public int IdAktivitas
        {
            get { return _idAktivitas; }
            set { _idAktivitas = value; }
        }

        public int IdPenanaman
        {
            get { return _idPenanaman; }
            set { _idPenanaman = value; }
        }
        public int IdJenisAktivitas
        {
            get { return _idJenisAktivitas; }
            set { _idJenisAktivitas = value; }
        }
        public int IdAkun
        {
            get { return _idAkun; }
            set { _idAkun = value; }
        }
        public DateTime Waktu
        {
            get { return _waktu; }
            set { _waktu = value; }
        }

        public string Catatan
        {
            get { return _catatan; }
            set { _catatan = value ?? string.Empty; }
        }

        public string? NamaJenisAktivitas
        {
            get { return JenisAktivitas?.Nama; }
        }

        public string? NamaAkun
        {
            get { return Akun?.Username; }
        }

        public string WaktuDisplay => _waktu.ToString("dd-MM-yyyy HH:mm");

        public PenanamanModel? Penanaman
        {
            get
            {
                if (_penanaman == null && _idPenanaman > 0)
                {
                    _penanaman = PenanamanModel.FindById(_idPenanaman);
                }
                return _penanaman;
            }
            set { _penanaman = value; }
        }

        public JenisAktivitasModel? JenisAktivitas
        {
            get
            {
                if (_jenisAktivitas == null && _idJenisAktivitas > 0)
                {
                    _jenisAktivitas = JenisAktivitasModel.FindById(_idJenisAktivitas);
                }
                return _jenisAktivitas;
            }
            set { _jenisAktivitas = value; }
        }

        public AkunModel? Akun
        {
            get
            {
                if (_akun == null && _idAkun > 0)
                {
                    _akun = AkunModel.FindById(_idAkun);
                }
                return _akun;
            }
            set { _akun = value; }
        }

        public static List<AktivitasModel> GetByIdPenanaman(int idPenanaman)
        {
            List<AktivitasModel> aktivitasList = new List<AktivitasModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_aktivitas, id_penanaman, id_jenis_aktivitas, id_akun, waktu, catatan FROM aktivitas WHERE id_penanaman = @id_penanaman ORDER BY id_aktivitas ASC"; // Added id_akun
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_penanaman", idPenanaman);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AktivitasModel aktivitas = new AktivitasModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                );
                aktivitasList.Add(aktivitas);
            }
            return aktivitasList;
        }

        public static AktivitasModel? FindById(int idAktivitas)
        {
            AktivitasModel? aktivitas = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_aktivitas, id_penanaman, id_jenis_aktivitas, id_akun, waktu, catatan FROM aktivitas WHERE id_aktivitas = @id_aktivitas"; // Added id_akun
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_aktivitas", idAktivitas);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                aktivitas = new AktivitasModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                );
            }
            return aktivitas;
        }

        public static List<AktivitasModel> GetByIdAkun(int id_akun)
        {
            List<AktivitasModel> aktivitasList = new List<AktivitasModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_aktivitas, id_penanaman, id_jenis_aktivitas, id_akun, waktu, catatan FROM aktivitas WHERE id_akun = @id_akun ORDER BY id_aktivitas ASC"; // Added id_akun
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_akun", id_akun);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AktivitasModel aktivitas = new AktivitasModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                );
                aktivitasList.Add(aktivitas);
            }
            return aktivitasList;
        }

        public static List<AktivitasModel> GetByIdJenisAktivitas(int id_jenis_aktivitas)
        {
            List<AktivitasModel> aktivitasList = new List<AktivitasModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_aktivitas, id_penanaman, id_jenis_aktivitas, id_akun, waktu, catatan FROM aktivitas WHERE id_jenis_aktivitas = @id_jenis_aktivitas ORDER BY id_aktivitas ASC"; // Added id_akun
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_jenis_aktivitas", id_jenis_aktivitas);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AktivitasModel aktivitas = new AktivitasModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                );
                aktivitasList.Add(aktivitas);
            }
            return aktivitasList;
        }

        public bool Insert(AktivitasModel aktivitas)
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "INSERT INTO aktivitas (id_penanaman, id_jenis_aktivitas, id_akun, waktu, catatan) VALUES (@id_penanaman, @id_jenis_aktivitas, @id_akun, @waktu, @catatan) RETURNING id_aktivitas"; // Added id_akun
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_penanaman", aktivitas.IdPenanaman);
            cmd.Parameters.AddWithValue("id_jenis_aktivitas", aktivitas.IdJenisAktivitas);
            cmd.Parameters.AddWithValue("id_akun", aktivitas.IdAkun);
            cmd.Parameters.AddWithValue("waktu", aktivitas.Waktu);
            cmd.Parameters.AddWithValue("catatan", (object?)aktivitas.Catatan ?? DBNull.Value);

            var newId = cmd.ExecuteScalar();
            if (newId != null && newId != DBNull.Value)
            {
                _idAktivitas = Convert.ToInt32(newId);
                return true;
            }
            return false;
        }

        public bool Update(AktivitasModel Aktivitas)
        {
            if (_idAktivitas <= 0)
                return false;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "UPDATE aktivitas SET id_jenis_aktivitas = @id_jenis_aktivitas, waktu = @waktu, catatan = @catatan WHERE id_aktivitas = @id_aktivitas"; // Added id_akun
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_aktivitas", Aktivitas.IdAktivitas);
            cmd.Parameters.AddWithValue("id_jenis_aktivitas", Aktivitas.IdJenisAktivitas);
            cmd.Parameters.AddWithValue("waktu", Aktivitas.Waktu);
            cmd.Parameters.AddWithValue("catatan", (object?)Aktivitas.Catatan ?? DBNull.Value);

            int affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        public bool Delete(int _idAktivitas)
        {
            if (_idAktivitas <= 0)
                return false;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "DELETE FROM aktivitas WHERE id_aktivitas = @id_aktivitas";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_aktivitas", _idAktivitas);
            int affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        public static int CountAll()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT COUNT(*) FROM aktivitas";
            using var cmd = new NpgsqlCommand(query, conn);
            object? result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }
    }
}
