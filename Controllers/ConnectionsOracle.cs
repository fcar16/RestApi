using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using CajeroAPI.Controllers;
namespace Sepalo.WebApi.Admin.Repository
{
    /// <summary>
    /// Base repository class for DB connection and its type
    /// </summary>
    public class BaseRepository : IDisposable
    {
        //Data base connection
       public IDbConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
        //Data base connection type
        protected DBConnectionType _dbConnectionType;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repositoryName">Name of the entity repository using this base</param>
        /// <param name="configuration">Auto injected settigns configuration</param>
        public BaseRepository(string repositoryName, IConfiguration configuration)
        {
            string defaultConnectionId = configuration.GetValue($"Repositories:Default", "OracleCOROLA");
            string connectionId = configuration.GetValue($"Repositories:{repositoryName}:DBConnectionId", defaultConnectionId);
            string connectionString = configuration.GetValue<string>($"DBConnections:{connectionId}:ConnectionString");
            _dbConnectionType = configuration.GetValue<DBConnectionType>($"DBConnections:{connectionId}:Type");
            switch (_dbConnectionType)
            {
                case DBConnectionType.Oracle:
                    _dbConnection = new OracleConnection(connectionString);
                    break;
            }
        }
        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}