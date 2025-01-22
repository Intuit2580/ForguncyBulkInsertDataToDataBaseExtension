using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace BulkInsertDataToDataBaseExtension.Helper
{
    public class ConvertListToDataTableHelper
    {
        public static DataTable ListToDataTable(List<Dictionary<string, object>> list, List<PropertyListDto> propertyList)
        {
            var dataTable = new DataTable();
            
            dataTable.Columns.AddRange(propertyList.Select(p => new DataColumn(p.PropertyName)).ToArray());
            
            foreach (var item in list)
            {
                var dataRow = dataTable.NewRow();
                
                foreach (var property in propertyList.Where(property => item.ContainsKey(property.PropertyName)))
                {
                    dataRow[property.PropertyName] = item[property.PropertyName];
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}