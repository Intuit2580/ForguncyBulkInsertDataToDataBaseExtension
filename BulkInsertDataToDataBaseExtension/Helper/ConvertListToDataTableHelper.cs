using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace BulkInsertDataToDataBaseExtension.Helper
{
    public static class ConvertListToDataTableHelper
    {
        public static DataTable ListToDataTable(object dataList, List<PropertyListDto> propertyList)
        {
            return dataList switch
            {
                List<Dictionary<string, object>> dictionaryList => ConvertFromDictionaryList(dictionaryList, propertyList),
                ArrayList arrayList => ConvertFromArrayList(arrayList, propertyList),
                JArray jArray => ConvertFromJArray(jArray, propertyList),
                _ => new DataTable()
            };
        }

        private static DataTable ConvertFromDictionaryList(List<Dictionary<string, object>> list, List<PropertyListDto> propertyList)
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
        
        private static DataTable ConvertFromArrayList(ArrayList list, List<PropertyListDto> propertyList)
        {
            

            var dataTable = new DataTable();
            
            dataTable.Columns.AddRange(propertyList.Select(p => new DataColumn(p.PropertyName)).ToArray());
            
            foreach (var item in list)
            {
                var dic = item as Dictionary<string, object>;

                var dataRow = dataTable.NewRow();
                
                foreach (var property in propertyList.Where(property => dic.ContainsKey(property.PropertyName)))
                {
                    dataRow[property.PropertyName] = dic[property.PropertyName];
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
        
        private static DataTable ConvertFromJArray(JArray jArray, List<PropertyListDto> propertyList)
        {
            var dataTable = new DataTable();
            
            dataTable.Columns.AddRange(propertyList.Select(p => new DataColumn(p.PropertyName)).ToArray());
            
            foreach (var item in jArray)
            {
                var dic = item.ToDictionary<JToken, string, object>(token => ((JProperty)token).Name, token => ((JProperty)token).Value);
                
                var dataRow = dataTable.NewRow();

                var existProperty = propertyList.Where(property => dic.ContainsKey(property.PropertyName));
                foreach (var property in existProperty)
                {
                    dataRow[property.PropertyName] = dic[property.PropertyName];
                }
                
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
        
    }
}