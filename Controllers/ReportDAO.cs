using System.Collections;
using CajeroAPI.Models;
using Oracle.ManagedDataAccess.Client;

namespace CajeroAPI.Controllers
{
    public class ReportDAO
    {
        

        public ArrayList GetReportsddbb(string query)
        {
            OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
            ArrayList ArrayReports = new ArrayList();
            try
            {
                _dbConnection.Open();

                OracleCommand command = _dbConnection.CreateCommand();
                command.CommandText = query;

                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string description = reader["DESCRIPTION"].ToString();
                    string name_report = reader["NAME_REPORT"].ToString();
                    string url_template = reader["URL_TEMPLATE"].ToString();
                    string sql_criteria = reader["SQL_CRITERIA"].ToString();

                    Report report = new Report(id, description, name_report, url_template, sql_criteria);
                    // Populate the report object with data from the reader
                    ArrayReports.Add(report);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
            }
            finally
            {
                _dbConnection.Close();
                
            }
            return ArrayReports;
        }
        
        public int GetProfileId(string query)
        {
            OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
            int profile_id = 1;
            try
            {
                _dbConnection.Open();

                OracleCommand command = _dbConnection.CreateCommand();
                command.CommandText = query;

                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    profile_id = Convert.ToInt32(reader["PROFILE_ID"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
            }
            finally
            {
                _dbConnection.Close();
                
            }
            return profile_id;
        }
        public ArrayList GetReportCritddbb(String query)
        {
            OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
            ArrayList ArrayReportCrit = new ArrayList();
            try
            {
                _dbConnection.Open();

                OracleCommand command = _dbConnection.CreateCommand();
                command.CommandText = query;

                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string description = reader["DESCRIPTION"].ToString();
                    string name_report = reader["NAME_REPORT"].ToString();
                    string url_template = reader["URL_TEMPLATE"].ToString();
                    string sql_criteria = reader["SQL_CRITERIA"].ToString();
                    int Idc = Convert.ToInt32(reader["ID"]);
                    string type = reader["TYPE"].ToString();
                    string @operator = reader["OPERATOR"].ToString();
                    string sql = reader["SQL"].ToString();
                    string sQL_SOURCE = reader["SQL_SOURCE"].ToString();
                    int order = Convert.ToInt32(reader["ORDER"]);

                    Report report = new Report(id, description, name_report, url_template, sql_criteria);
                    Report_Criteria reportC = new Report_Criteria(Idc, type, @operator, description, sql, sQL_SOURCE, id, order);
                    ArrayReportCrit.Add(report);
                    ArrayReportCrit.Add(reportC);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
            }
            finally
            {
                _dbConnection.Close();

            }
            return ArrayReportCrit;

        }
    }
}