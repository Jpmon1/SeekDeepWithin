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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Peter.Common.Tree;

namespace Peter.Common.BindingConverters
{
   /// <summary>
   /// A value converter for the left margin of a tree view item, based on it's depth.
   /// </summary>
   public class LeftMarginMultiplierConverter : IValueConverter
   {
      /// <summary>
      /// Gets or Sets the default length to use.
      /// </summary>
      public double Length { get; set; }

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
         var item = value as TreeViewItem;
         if (item == null)
            return new Thickness (0);

         return new Thickness (Length * item.GetDepth (), 0, 0, 0);
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
         throw new NotImplementedException ();
      }
   }
}
