
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;





namespace Drawing3d
{
    /// <summary>
    /// A typecoverter for <see cref="xy"/>
    /// </summary>
    [Serializable]
    public class xyEditor : TypeConverter
    {
        /// <summary>
        /// Checks whether the value can convert to xy
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {

            if (sourceType == typeof(string))
                return true;
            return false;
        }
       /// <summary>
       /// Converts the value to a xy point, if is possible
       /// </summary>
       /// <param name="context"></param>
       /// <param name="culture"></param>
       /// <param name="value"></param>
       /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is String)
                try
                {

                    xy Result = xy.FromString(value as String);
                    return Result;
                }
                catch (Exception)
                {

                    return false;
                }
            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>
        /// convert a value, if this is from type xy   to a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is xy)
                return ((xy)value).ToString();


            return base.ConvertTo(context, culture, value, destinationType);
        }
    

    }
}