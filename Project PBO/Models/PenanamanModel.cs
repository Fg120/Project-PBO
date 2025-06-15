using System;
using System.Collections.Generic;
using Npgsql;
using Project_PBO.Database;

namespace Project_PBO.Models
{
    public class PenanamanModel
    {
        private int _idPenanaman;
        private int _idLahan;
        private int _idTanaman;
        private DateOnly _tanggalTanam;
        private DateOnly? _tanggalPanen;
        private decimal? _hasilPanen;
        private string _catatan = string.Empty;
        private string _status;
        private LahanModel? _lahan;
        private TanamanModel? _tanaman;
        private List<AktivitasModel>? _aktivitasList;

        public PenanamanModel()
        {
            _status = "Direncanakan";
            _aktivitasList = new List<AktivitasModel>();
        }

        internal PenanamanModel(
            int idPenanaman,
            int idLahan,
            int idTanaman,
            DateOnly tanggalTanam,
            DateOnly? tanggalPanen,
            decimal? hasilPanen,
            string? catatan,
            string status
        )
        {
            _idPenanaman = idPenanaman;
            _idLahan = idLahan;
            _idTanaman = idTanaman;
            _tanggalTanam = tanggalTanam;
            _tanggalPanen = tanggalPanen;
            _hasilPanen = hasilPanen;
            _catatan = catatan ?? string.Empty;
            _status = status ?? "Direncanakan";
            _aktivitasList = new List<AktivitasModel>();
        }

        public int IdPenanaman
        {
            get { return _idPenanaman; }
            set { _idPenanaman = value; }
        }
        public int IdLahan
        {
            get { return _idLahan; }
            set { _idLahan = value; }
        }
        public int IdTanaman
        {
            get { return _idTanaman; }
            set { _idTanaman = value; }
        }
        public DateOnly TanggalTanam
        {
            get { return _tanggalTanam; }
            set { _tanggalTanam = value; }
        }
        public DateOnly? TanggalPanen
        {
            get { return _tanggalPanen; }
            set { _tanggalPanen = value; }
        }
        public decimal? HasilPanen
        {
            get { return _hasilPanen; }
            set
            {
                if (value == null || value >= 0)
                    _hasilPanen = value;
                else
                    throw new ArgumentException("Hasil panen harus >= 0");
            }
        }
        public string Catatan
        {
            get { return _catatan; }
            set { _catatan = value ?? string.Empty; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value ?? "Direncanakan"; }
        }
        public string? NamaLahan => Lahan?.Nama;
        public string? NamaTanaman => Tanaman?.Nama;
        public LahanModel? Lahan
        {
            get
            {
                if (_lahan == null && _idLahan > 0)
                    _lahan = LahanModel.FindById(_idLahan);
                return _lahan;
            }
            set { _lahan = value; }
        }
        public TanamanModel? Tanaman
        {
            get
            {
                if (_tanaman == null && _idTanaman > 0)
                    _tanaman = TanamanModel.FindById(_idTanaman);
                return _tanaman;
            }
            set { _tanaman = value; }
        }

        public static List<PenanamanModel> GetAll()
        {
            List<PenanamanModel> penanamanList = new List<PenanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_penanaman, id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status FROM penanaman ORDER BY id_penanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string statusPenanaman = reader.IsDBNull(7) ? "Direncanakan" : reader.GetString(7);
                PenanamanModel p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    statusPenanaman
                );
                penanamanList.Add(p);
            }
            return penanamanList;
        }

        public static List<PenanamanModel> GetAllActive()
        {
            List<PenanamanModel> penanamanList = new List<PenanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_penanaman, id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status FROM penanaman WHERE status ILIKE 'Aktif' ORDER BY id_penanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PenanamanModel p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    reader.GetString(7)
                );
                penanamanList.Add(p);
            }
            return penanamanList;
        }

        public static PenanamanModel? FindById(int idPenanaman)
        {
            PenanamanModel? p = null;
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_penanaman, id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status FROM penanaman WHERE id_penanaman = @id_penanaman";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_penanaman", idPenanaman);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string statusPenanaman = reader.IsDBNull(7) ? "Direncanakan" : reader.GetString(7);
                p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    statusPenanaman
                );
            }
            return p;
        }

        public static List<PenanamanModel> GetByIdLahan(int idLahan)
        {
            List<PenanamanModel> penanamanList = new List<PenanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_penanaman, id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status FROM penanaman WHERE id_lahan = @id_lahan ORDER BY id_penanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_lahan", idLahan);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PenanamanModel p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    reader.GetString(7)
                );
                penanamanList.Add(p);
            }
            return penanamanList;
        }

        public static List<PenanamanModel> GetByIdTanaman(int idTanaman)
        {
            List<PenanamanModel> penanamanList = new List<PenanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_penanaman, id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status FROM penanaman WHERE id_tanaman = @id_tanaman ORDER BY id_penanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_tanaman", idTanaman);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PenanamanModel p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    reader.GetString(7)
                );
                penanamanList.Add(p);
            }
            return penanamanList;
        }
        public bool Insert()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "INSERT INTO penanaman (id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status) VALUES (@id_lahan, @id_tanaman, @tanggal_tanam, @tanggal_panen, @hasil_panen, @catatan, @status) RETURNING id_penanaman";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_lahan", _idLahan);
            cmd.Parameters.AddWithValue("id_tanaman", _idTanaman);
            cmd.Parameters.AddWithValue("tanggal_tanam", _tanggalTanam);
            cmd.Parameters.AddWithValue("tanggal_panen", (object?)_tanggalPanen ?? DBNull.Value);
            cmd.Parameters.AddWithValue("hasil_panen", (object?)_hasilPanen ?? DBNull.Value);
            cmd.Parameters.AddWithValue("catatan", (object?)_catatan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("status", _status);
            var newId = cmd.ExecuteScalar();
            if (newId != null && newId != DBNull.Value)
            {
                _idPenanaman = Convert.ToInt32(newId);
                return true;
            }
            return false;
        }

        public bool Update()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "UPDATE penanaman SET id_lahan = @id_lahan, id_tanaman = @id_tanaman, tanggal_tanam = @tanggal_tanam, tanggal_panen = @tanggal_panen, hasil_panen = @hasil_panen, catatan = @catatan, status = @status WHERE id_penanaman = @id_penanaman";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_penanaman", _idPenanaman);
            cmd.Parameters.AddWithValue("id_lahan", _idLahan);
            cmd.Parameters.AddWithValue("id_tanaman", _idTanaman);
            cmd.Parameters.AddWithValue("tanggal_tanam", _tanggalTanam);
            cmd.Parameters.AddWithValue("tanggal_panen", (object?)_tanggalPanen ?? DBNull.Value);
            cmd.Parameters.AddWithValue("hasil_panen", (object?)_hasilPanen ?? DBNull.Value);
            cmd.Parameters.AddWithValue("catatan", (object?)_catatan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("status", _status);
            int affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }

        public static int CountAll()
        {
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query = "SELECT COUNT(*) FROM penanaman";
            using var cmd = new NpgsqlCommand(query, conn);
            var result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }

        public static List<PenanamanModel> GetTopFive()
        {
            List<PenanamanModel> penanamanList = new List<PenanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            string query =
                "SELECT id_penanaman, id_lahan, id_tanaman, tanggal_tanam, tanggal_panen, hasil_panen, catatan, status FROM penanaman WHERE hasil_panen IS NOT NULL ORDER BY hasil_panen DESC LIMIT 5";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PenanamanModel p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    reader.GetString(7)
                );
                penanamanList.Add(p);
            }
            return penanamanList;
        }

        public static List<PenanamanModel> GetByPetani(int idPetani)
        {
            List<PenanamanModel> penanamanList = new List<PenanamanModel>();
            using var conn = DbContext.GetConnection();
            conn.Open();
            // Query to select penanaman based on id_akun (petani) from lahan, then join with penanaman
            string query = @"SELECT p.id_penanaman, p.id_lahan, p.id_tanaman, p.tanggal_tanam, 
                                    p.tanggal_panen, p.hasil_panen, p.catatan, p.status
                             FROM penanaman p
                             JOIN lahan l ON p.id_lahan = l.id_lahan
                             WHERE l.id_akun = @id_petani
                             ORDER BY p.id_penanaman ASC";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_petani", idPetani);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string statusPenanaman = reader.IsDBNull(7) ? "Direncanakan" : reader.GetString(7);
                PenanamanModel p = new PenanamanModel(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetFieldValue<DateOnly>(3),
                    reader.IsDBNull(4) ? (DateOnly?)null : reader.GetFieldValue<DateOnly>(4),
                    reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    reader.IsDBNull(6) ? null : reader.GetString(6),
                    statusPenanaman
                );
                penanamanList.Add(p);
            }
            return penanamanList;
        }
    }
}
