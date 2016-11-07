/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Globalization;
using System.Windows.Data;

namespace Peter.Common.Icons
{
   /// <summary>
   /// Class used to convert the icon size to the proper font size.
   /// </summary>
   public class IconSizeConverter : IValueConverter
   {
      /// <summary>
      /// Converts a value. 
      /// </summary>
      /// <param name="value">The value produced by the binding source.</param>
      /// <param name="targetType">The type of the binding target property.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
      public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value == null) return String.Empty;
         var size = (IconSize)value;
         return GetIconSize (size);
      }

      /// <summary>
      /// Converts a value. 
      /// </summary>
      /// <param name="value">The value that is produced by the binding target.</param>
      /// <param name="targetType">The type to convert to.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
      public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
      {
         return null;
      }

      /// <summary>
      /// Gets the font size for the given icon size.
      /// </summary>
      /// <param name="size"></param>
      /// <returns></returns>
      public static double GetIconSize (IconSize size)
      {
         switch (size)
         {
            case IconSize.Smallest:
               return 8;
            case IconSize.Smaller:
               return 10;
            case IconSize.Small:
               return 12;
            case IconSize.MediumLarge:
               return 16;
            case IconSize.Large:
               return 18;
            case IconSize.Larger:
               return 20;
            case IconSize.XLarge:
               return 24;
            case IconSize.Largest:
               return 32;
            default:
               return 14;
         }
      }
   }
}