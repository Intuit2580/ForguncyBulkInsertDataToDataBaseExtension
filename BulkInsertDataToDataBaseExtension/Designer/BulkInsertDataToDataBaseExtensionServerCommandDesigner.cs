using GrapeCity.Forguncy.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BulkInsertDataToDataBaseExtension.Helper;
using GrapeCity.Forguncy.Plugin;
using EnumsNET;

namespace BulkInsertDataToDataBaseExtension.Designer
{
    public class BulkInsertDataToDataBaseExtensionServerCommandDesigner : CommandDesigner<BulkInsertDataToDataBaseExtensionServerCommand>
    {
        public override EditorSetting GetEditorSetting(PropertyDescriptor property, IBuilderCommandContext builderContext)
        {
            if (property.Name == nameof(BulkInsertDataToDataBaseExtensionServerCommand.DataBaseType))
            {
                return new ComboEditorSetting(
                    Enum.GetValues<DataBaseTypeEnum>().Select(e => new ComboItem((int)e, e.GetName())), 
                    nameof(ComboItem.Display), 
                    nameof(ComboItem.Value));
            }

            if (property.Name == nameof(BulkInsertDataToDataBaseExtensionServerCommand.TableName))
            {
                return new TableComboTreeSelectorEditorSetting();
            }

            if (property.Name == nameof(BulkInsertDataToDataBaseExtensionServerCommand.CustomPropertyList))
            {
                return new ColumnListEditorSetting(builderContext);
            }
            
            return base.GetEditorSetting(property, builderContext);
        }

        public override void OnPropertyEditorChanged(string propertyName, object propertyValue, Dictionary<string, IEditorSettingsDataContext> properties)
        {
            if (propertyName == nameof(BulkInsertDataToDataBaseExtensionServerCommand.TableName))
            {
                if (properties.TryGetValue(nameof(BulkInsertDataToDataBaseExtensionServerCommand.CustomPropertyList), out var setting)
                    && setting.EditorSetting is ColumnListEditorSetting columnListSetting)
                {
                    setting.Value = GetPropertyListValue(columnListSetting.BuilderContext);
                }
            }
            base.OnPropertyEditorChanged(propertyName, propertyValue, properties);
        }

        private List<PropertyListDto> GetPropertyListValue(IBuilderCommandContext context)
        {
            var tableInfo = context.EnumAllTableInfos().FirstOrDefault(i => i.TableName == Command.TableName);

            return tableInfo?.Columns.Select(c => new PropertyListDto
            {
                ColumnName = c.ColumnName,
                PropertyName = c.ColumnName
            }).ToList() ?? new List<PropertyListDto>();
        }
        
    }
    
    public class ComboItem
    {
        public ComboItem(int value, string display)
        {
            Value = value;
            Display = display;
        }
        public int Value { get; set; }
        public string Display { get; set; }
    }
    
    public class ColumnListEditorSetting : FlatListPropertyAttributeEditorSetting
    {
        public IBuilderCommandContext BuilderContext { get; set; }
        public ColumnListEditorSetting(IBuilderCommandContext builderContext)
        {
            BuilderContext = builderContext;
            ListType = typeof(List<PropertyListDto>);
        }
    }
}
