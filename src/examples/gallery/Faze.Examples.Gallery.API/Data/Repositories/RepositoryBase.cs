﻿using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Faze.Examples.Gallery.API.Data
{
    public class RepositoryBase
    {
        protected readonly IDbConnection db;

        public RepositoryBase(DatabaseConfig databaseConfig)
        {
            db = new SqliteConnection(databaseConfig.Name);
        }

        public void Dispose()
        {
            db.Close();
        }
    }
}
