using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace JiraWorkLogUploader.Ui
{
    /// <summary>
    /// based on: https://bytes.com/topic/c-sharp/answers/596701-propertygrid-dynamic-dropdown-list
    /// </summary>
    public class GenericEnumerableConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var collectionPropertyName = "ItemsFor_" + context.PropertyDescriptor.Name;

            var type = context.Instance.GetType();
            var itemsProperty = type.GetProperty(collectionPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var items = itemsProperty.GetValue(context.Instance, null);
            var collection = new StandardValuesCollection((ICollection) items);
            return collection;
        }
    }
}
