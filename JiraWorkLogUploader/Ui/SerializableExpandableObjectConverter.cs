using System.ComponentModel;

namespace JiraWorkLogUploader.Ui
{
    /// <summary>
    /// expandable collections: http://stackoverflow.com/questions/17560085/problems-using-json-net-with-expandableobjectconverter
    /// </summary>
    public class SerializableExpandableObjectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((destinationType == typeof(string)))
            {
                return false;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if ((sourceType == typeof(string)))
            {
                return false;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }
    }
}
