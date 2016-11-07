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
using Peter.Common.Icons;

namespace Peter.Common.MainMenu
{
   /// <summary>
   /// A converter for the height of the main menu.
   /// </summary>
   public class MainMenuHeightConverter : IValueConverter
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
         var height = 30;
         var size = (IconSize) value;
         if (size == IconSize.Large)
            height = 32;
         if (size == IconSize.Small)
            height = 20;
         if (parameter != null)
         {
            height = height - System.Convert.ToInt32 (parameter);
         }

         return height;
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
         var size = (double)value;
         if (size.Equals (32))
            return IconSize.Large;
         if (size.Equals (20))
            return IconSize.Small;

         return IconSize.Medium;
      }
   }
}
