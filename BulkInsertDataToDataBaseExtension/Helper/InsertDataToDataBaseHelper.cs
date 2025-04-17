using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;

namespace BulkInsertDataToDataBaseExtension.Helper
{
    public static class InsertDataToDataBaseHelper
    {
        public static async Task InsertDataToSqlServerAsync(string connectionString, string tableName, DataTable dataTable, List<PropertyListDto> propertyList)
        {
            await using var connection = new SqlConnection(connectionString);
            var bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = tableName; 
            bulkCopy.BatchSize = dataTable.Rows.Count;
            
            foreach (var property in propertyList)
            {
                bulkCopy.ColumnMappings.Add(property.PropertyName, property.ColumnName);
            }
            
            await connection.OpenAsync();
            await bulkCopy.WriteToServerAsync(dataTable);
            await connection.CloseAsync();
        }
        
        public static async Task InsertDataToMySqlAsync(string connectionString, string tableName, DataTable dataTable, List<PropertyListDto> propertyList)
        {
            connectionString += ";AllowLoadLocalInfile=true;";
            var connection = new MySqlConnection(connectionString);
            
            var bulkCopy = new MySqlBulkCopy(connection)
            {
                DestinationTableName = tableName
            };
            
            foreach (var property in propertyList)
            {
                bulkCopy.ColumnMappings.Add(new MySqlBulkCopyColumnMapping(dataTable.Columns.IndexOf(property.PropertyName), property.ColumnName));
            }
            
            await connection.OpenAsync();
            await bulkCopy.WriteToServerAsync(dataTable);
            await connection.CloseAsync();
        }

        public static async Task InsertDataToOracleAsync(string connectionString, string tableName, DataTable dataTable, List<PropertyListDto> propertyList)
        {
            var connection = new OracleConnection(connectionString);
            
            await connection.OpenAsync();
            
            var bulkCopy = new OracleBulkCopy(connection)
            {
                DestinationTableName = tableName
            };
            
            foreach (var property in propertyList)
            {
                bulkCopy.ColumnMappings.Add(property.PropertyName, property.ColumnName);
            }
            
            bulkCopy.WriteToServer(dataTable);
            await connection.CloseAsync();
        }
    }
}