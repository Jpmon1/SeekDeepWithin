/**
 * Peter
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
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Windows.Data;

namespace Peter.Common.BindingConverters
{
   /// <summary>
   /// Binding converter to pluralize the given string.
   /// </summary>
   public class PluralizeConverter : IValueConverter
   {
      private static readonly PluralizationService s_Service =
         PluralizationService.CreateService (CultureInfo.CurrentUICulture);

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
         var num = 0;
         var str = value.ToString ();
         if (parameter is int)
            num = (int)parameter;

         return (num != 1 && s_Service.IsSingular (str)) ? s_Service.Pluralize (str) : str;
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
         var str = value.ToString ();
         return (s_Service.IsPlural (str)) ? s_Service.Singularize (str) : str;
      }
   }
}
