using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySqlConnector;

namespace BulkInsertDataToDataBaseExtension.Helper
{
    public class InsertDataToDataBaseHelper
    {
        public static async Task InsertDataToSqlServerAsync(string connectionString, string tableName, DataTable dataTable, List<PropertyListDto> propertyList)
        {
            await using var connection = new SqlConnection(connectionString);
            var bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = tableName; 
            bulkCopy.BatchSize = dataTable.Rows.Count;
                
            await connection.OpenAsync();
            foreach (var property in propertyList)
            {
                bulkCopy.ColumnMappings.Add(property.PropertyName, property.ColumnName);
            }
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

            await connection.OpenAsync();
            foreach (var property in propertyList)
            {
                bulkCopy.ColumnMappings.Add(new MySqlBulkCopyColumnMapping(dataTable.Columns.IndexOf(property.PropertyName), property.ColumnName));
            }
            await bulkCopy.WriteToServerAsync(dataTable);
            await connection.CloseAsync();
        }
    }
}