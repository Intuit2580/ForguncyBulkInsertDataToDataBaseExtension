using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using GrapeCity.Forguncy.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Threading.Tasks;
using BulkInsertDataToDataBaseExtension.Helper;

namespace BulkInsertDataToDataBaseExtension
{
    [Icon("pack://application:,,,/BulkInsertDataToDataBaseExtension;component/Resources/Icon.png")]
    [Designer("BulkInsertDataToDataBaseExtension.Designer.BulkInsertDataToDataBaseExtensionServerCommandDesigner, BulkInsertDataToDataBaseExtension")]
    public class BulkInsertDataToDataBaseExtensionServerCommand : Command, ICommandExecutableInServerSideAsync
    {
        [Required]
        [DisplayName("数据库类型")]
        public int DataBaseType { get; set; }
        
        [DatabaseConnectionSelectorProperty(IncludeBuiltInDatabase = true)]
        [Required]
        [DisplayName("连接字符串")]
        public string Connection { get; set; }
        
        [Required]
        [DisplayName("数据表名")]
        public string TableName { get; set; }

        [Required]
        [FormulaProperty] 
        [DisplayName("Json数组")] 
        public object DataList { get; set; }
        
        [DisplayName("属性名称对照列表")]
        [FlatListProperty]
        public List<PropertyListDto> CustomPropertyList { get; set; }
        
        [ResultToProperty]
        [DisplayName("成功插入行数")]
        public string ResultTo { get; set; } = "结果";
        
        
        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            if (CustomPropertyList == null || CustomPropertyList.Count == 0) return null;
            
            var dataAccess = dataContext.DataAccess;
            var connectionString = dataAccess.GetConnectionStringByID(Connection);

            var dataList = await dataContext.EvaluateFormulaAsync(DataList);
            if (!(dataList is List<Dictionary<string, object>> list)) return null;
            
            var tableName = (await dataContext.EvaluateFormulaAsync(TableName)).ToString();
            if (string.IsNullOrWhiteSpace(tableName)) return null;

            var dataTable = ConvertListToDataTableHelper.ListToDataTable(list, CustomPropertyList);
            
            
            await ImportDataToDataBaseAsync(connectionString, tableName, dataTable);
            
            dataContext.Parameters[ResultTo] = dataTable.Rows.Count;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            return "批量插入数据命令";
        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
        
        private async Task ImportDataToDataBaseAsync(string connectionString, string tableName, DataTable dataTable)
        {
            switch (DataBaseType)
            {
                case (int)DataBaseTypeEnum.SqlServer:
                    await InsertDataToDataBaseHelper.InsertDataToSqlServerAsync(connectionString, tableName, dataTable, CustomPropertyList);
                    break;
                case (int)DataBaseTypeEnum.MySql:
                    await InsertDataToDataBaseHelper.InsertDataToMySqlAsync(connectionString, tableName, dataTable, CustomPropertyList);
                    break;
            }
        }
    }
}
