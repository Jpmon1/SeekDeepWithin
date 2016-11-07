/**
 * Peter
 * 
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 *    
 *  This code is provided on an AS IS basis, with no WARRANTIES,
 *  CONDITIONS or GUARANTEES of any kind.
 *  
 **/

using System;
using System.Globalization;
using System.Windows.Data;
using Peter.Common.Utilities;

namespace Peter.Common.BindingConverters
{
   /// <summary>
   /// Converts the given data model to a view if registered.
   /// </summary>
   public class ViewRegistryConverter : IValueConverter
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
         var view = ViewRegistry.CreateView (value);
         return view ?? value;
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
   }
}
