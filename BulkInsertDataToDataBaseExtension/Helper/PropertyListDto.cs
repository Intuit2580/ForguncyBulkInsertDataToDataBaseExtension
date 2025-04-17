using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GrapeCity.Forguncy.Plugin;

namespace BulkInsertDataToDataBaseExtension.Helper
{
    public class PropertyListDto : ObjectPropertyBase
    {
        [Required]
        [DisplayName("数据库列名")]
        public string ColumnName { get; set; }
        
        [Required]
        [DisplayName("属性名")]
        public string PropertyName { get; set; }
        
        [DisplayName("是否为日期")]
        public bool IsDateTime { get; set; }
    }
}