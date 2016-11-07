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
/*
Copyright (c) 2010, Daniel De Sousa (daniel@dandesousa.com)
http://code.google.com/p/phoenix-control-library/

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE
*/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Peter.Common.Controls
{
   [TemplatePart (Name = ELEMENT_INCREMENT_BUTTON, Type = typeof (RepeatButton))]
   [TemplatePart (Name = ELEMENT_DECREMENT_BUTTON, Type = typeof (RepeatButton))]
   public class NumericUpDown : TextBox
   {
      private const string ELEMENT_INCREMENT_BUTTON = "PART_IncrementButton";
      private const string ELEMENT_DECREMENT_BUTTON = "PART_DecrementButton";

      private const double DEFAULT_INTERVAL = 1.0;
      private RepeatButton m_IncrementButton;
      private RepeatButton m_DecrementButton;
      private bool m_ValueUpdating;

      /// <summary>
      /// Static constructor.
      /// </summary>
      static NumericUpDown ()
      {
         DefaultStyleKeyProperty.OverrideMetadata (typeof (NumericUpDown),
            new FrameworkPropertyMetadata (typeof (NumericUpDown)));
      }

      public NumericUpDown ()
      {
         this.HorizontalContentAlignment = HorizontalAlignment.Right;
         this.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
         TextBoxProperties.SetIsNumeric (this, true);
      }

      /// <summary>
      /// Dependency property for value.
      /// </summary>
      public static readonly DependencyProperty ValueProperty = DependencyProperty.Register (
         "Value", typeof (double), typeof (NumericUpDown), new FrameworkPropertyMetadata (default (double),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnValueChanged));

      /// <summary>
      /// Gets or Sets the value.
      /// </summary>
      public double Value
      {
         get { return (double) GetValue (ValueProperty); }
         set { SetValue (ValueProperty, value); }
      }

      /// <summary>
      /// Dependency property for the unit label.
      /// </summary>
      public static readonly DependencyProperty LabelProperty = DependencyProperty.Register (
         "Label", typeof (string), typeof (NumericUpDown),
         new PropertyMetadata (default (string), OnUnitLabelChanged));

      /// <summary>
      /// Gets or Sets the unit label.
      /// </summary>
      public string Label
      {
         get { return (string) GetValue (LabelProperty); }
         set { SetValue (LabelProperty, value); }
      }

      /// <summary>
      /// Occurs when the unit label changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnUnitLabelChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var spinner = d as NumericUpDown;
         if (spinner != null)
            spinner.HasLabel = !string.IsNullOrWhiteSpace (spinner.Label);
      }

      /// <summary>
      /// Dependency property for Precision.
      /// </summary>
      public static readonly DependencyProperty PrecisionProperty =
          DependencyProperty.Register ("Precision", typeof (int), typeof (NumericUpDown),
             new PropertyMetadata (2));

      /// <summary>
      /// Gets or Sets the precision for number being displayed.
      /// </summary>
      public int Precision
      {
         get { return (int) GetValue (PrecisionProperty); }
         set { SetValue (PrecisionProperty, value); }
      }

      /// <summary>
      /// Dependency property for if the spinner is shown or not.
      /// </summary>
      public static readonly DependencyProperty IsSpinnerShownProperty = DependencyProperty.Register (
         "IsSpinnerShown", typeof (bool), typeof (NumericUpDown),
         new PropertyMetadata (true));

      /// <summary>
      /// Gets or Sets whether or not to show the spin buttons.
      /// </summary>
      public bool IsSpinnerShown
      {
         get { return (bool) GetValue (IsSpinnerShownProperty); }
         set { SetValue (IsSpinnerShownProperty, value); }
      }

      /// <summary>
      /// Dependency property for updating the value binding.
      /// </summary>
      public static readonly DependencyProperty IsValueUpdateImmediateProperty = DependencyProperty.Register (
         "IsValueUpdateImmediate", typeof (bool), typeof (NumericUpDown),
         new PropertyMetadata (default (bool)));

      /// <summary>
      /// Gets or Sets if the value update is immediate as opposed to focus lost.
      /// </summary>
      public bool IsValueUpdateImmediate
      {
         get { return (bool) GetValue (IsValueUpdateImmediateProperty); }
         set { SetValue (IsValueUpdateImmediateProperty, value); }
      }

      /// <summary>
      /// Read only dependency property key for has unit.
      /// </summary>
      private static readonly DependencyPropertyKey s_HasLabelPropertyKey
        = DependencyProperty.RegisterReadOnly ("HasLabel", typeof (bool), typeof (NumericUpDown),
            new FrameworkPropertyMetadata (default (bool), FrameworkPropertyMetadataOptions.None));

      /// <summary>
      /// Dependency property for has unit.
      /// </summary>
      public static readonly DependencyProperty HasLabelProperty = s_HasLabelPropertyKey.DependencyProperty;

      /// <summary>
      /// Gets if this control has a unit or not.
      /// </summary>
      public bool HasLabel
      {
         get { return (bool) GetValue (HasLabelProperty); }
         protected set { SetValue (s_HasLabelPropertyKey, value); }
      }

      /// <summary>
      /// Dependency property for Increment Value.
      /// </summary>
      public static readonly DependencyProperty IncrementValueProperty =
          DependencyProperty.Register ("IncrementValue", typeof (double), typeof (NumericUpDown),
             new FrameworkPropertyMetadata (DEFAULT_INTERVAL));

      /// <summary>
      /// Gets or Sets Value to increment or decrement the spinner by.
      /// </summary>
      public double IncrementValue
      {
         get { return (double) GetValue (IncrementValueProperty); }
         set { SetValue (IncrementValueProperty, value); }
      }

      /// <summary>
      /// When applying the template, load the parts into the data fields 
      /// of this Custom Control
      /// </summary>
      public override void OnApplyTemplate ()
      {
         base.OnApplyTemplate ();

         this.m_IncrementButton = GetTemplateChild (ELEMENT_INCREMENT_BUTTON) as RepeatButton;
         if (this.m_IncrementButton != null) {
            this.m_IncrementButton.Click += IncrementButtonClick;
         }

         this.m_DecrementButton = GetTemplateChild (ELEMENT_DECREMENT_BUTTON) as RepeatButton;
         if (this.m_DecrementButton != null) {
            this.m_DecrementButton.Click += DecrementButtonClick;
         }
      }

      private static void OnValueChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var spinner = d as NumericUpDown;
         if (spinner != null) {
            if (spinner.m_ValueUpdating) return;
            var value = Math.Round (spinner.Value, spinner.Precision);
            var text = value.ToString (CultureInfo.InvariantCulture);
            if (spinner.Text != text)
               spinner.Text = text;
         }
      }

      /// <summary>Is called when content in this editing control changes.</summary>
      /// <param name="e">The arguments that are associated with the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</param>
      protected override void OnTextChanged (TextChangedEventArgs e)
      {
         base.OnTextChanged (e);
         if (this.IsValueUpdateImmediate && !string.IsNullOrEmpty (this.Text)) {
            var value = Math.Round (double.Parse (this.Text), this.Precision);
            if (!this.Value.Equals (value)) {
               this.m_ValueUpdating = true;
               this.Value = value;
               this.m_ValueUpdating = false;
            }
         }
      }

      /// <summary>Raises the <see cref="E:System.Windows.UIElement.LostFocus" /> event (using the provided arguments).</summary>
      /// <param name="e">Provides data about the event.</param>
      protected override void OnLostFocus (RoutedEventArgs e)
      {
         base.OnLostFocus (e);
         var hasValue = !string.IsNullOrEmpty (this.Text);
         if (!this.IsValueUpdateImmediate && hasValue) {
            var value = Math.Round (double.Parse (this.Text), this.Precision);
            if (!this.Value.Equals (value)) {
               this.m_ValueUpdating = true;
               this.Value = value;
               this.m_ValueUpdating = false;
            }
         } else if (hasValue) {
            var value = Math.Round (double.Parse (this.Text), this.Precision);
            var text = value.ToString (CultureInfo.InvariantCulture);
            if (this.Text != text)
               this.Text = text;
         }
      }

      /// <summary>
      /// Occurs when the increment button is pressed.
      /// </summary>
      /// <param name="sender">Increment button</param>
      /// <param name="e">RoutedEventArgs</param>
      void IncrementButtonClick (object sender, RoutedEventArgs e)
      {
         this.Value += this.IncrementValue;
      }

      /// <summary>
      /// Occurs when the decrement button is pressed.
      /// </summary>
      /// <param name="sender">Decrement button</param>
      /// <param name="e">RoutedEventArgs</param>
      void DecrementButtonClick (object sender, RoutedEventArgs e)
      {
         this.Value -= this.IncrementValue;
      }
   }
}
