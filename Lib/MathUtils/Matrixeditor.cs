using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;

using ClipperLib;
using System.Collections.Generic;

using LibTessDotNet;

namespace Drawing3d
{
    [Serializable]
    class Matrixeditor
    {
        [Serializable]
        internal class MatrixEditor : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {

                if (sourceType == typeof(string))
                    return true;
                return false;
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is String)
                    try
                    {

                        Matrix Result = Matrix.FromString(value as String);
                        return Result;
                    }
                    catch (Exception)
                    {

                        return false;
                    }
                return base.ConvertFrom(context, culture, value);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is Matrix)
                    return ((Matrix)value).ToString();


                return base.ConvertTo(context, culture, value, destinationType);
            }

        }
    }
}
