﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Peter.Common.BindingConverters
{
   /// <summary>
   /// Check to see if a value is null, and returns false if null.
   /// </summary>
   public class NullNotVisibleConverter : IValueConverter
   {
      /// <summary>
      /// Initializes a new null not visible object.
      /// </summary>
      public NullNotVisibleConverter ()
      {
         this.NotVisible = Visibility.Collapsed;
      }

      /// <summary>
      /// Gets or Sets the not visible visibility.
      /// </summary>
      public Visibility NotVisible { get; set; }

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
         return value == null ? this.NotVisible : Visibility.Visible;
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
