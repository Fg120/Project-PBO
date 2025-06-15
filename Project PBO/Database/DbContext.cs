using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Project_PBO.Database
{
    public class DbContext
    {
        private static string host = "localhost";
        private static string database = "project_pbo";
        private static string username = "postgres";
        private static string password = "postgres";
        private static int port = 5432;
        public static NpgsqlConnection GetConnection()
        {
            string conn = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
            return new NpgsqlConnection(conn);
        }
    }
}
