using System;
using System.Globalization;
using System.Threading;

namespace Peter.Common.Controls
{
   /// <summary> 
   /// Helper class that validates text box input for double values. 
   /// </summary> 
   internal static class DoubleValidator
   {
      private static readonly ThreadLocal<NumberFormatInfo> s_NumbersFormat = new ThreadLocal<NumberFormatInfo> (
         () => Thread.CurrentThread.CurrentCulture.NumberFormat);

      /// <summary> 
      /// Returns true if input <param name="text"/> is accepted by IsDouble text box. 
      /// </summary> 
      public static bool IsValid (string text)
      {
         // First corner case: null or empty string is a valid text in our case 
         if (string.IsNullOrEmpty (text))
            return true;

         // '.', '+', '-', '+.' or '-.' - are invalid doubles, but we should accept them 
         // because user can continue typeing correct value (like .1, +1, -0.12, +.1, -.2) 
         if (text == s_NumbersFormat.Value.NumberDecimalSeparator ||
             text == s_NumbersFormat.Value.NegativeSign ||
             text == s_NumbersFormat.Value.PositiveSign ||
             text == s_NumbersFormat.Value.NegativeSign + s_NumbersFormat.Value.NumberDecimalSeparator ||
             text == s_NumbersFormat.Value.PositiveSign + s_NumbersFormat.Value.NumberDecimalSeparator)
            return true;

         double output;
         return Double.TryParse (text, out output);
         // This is for more additional checks that we prevent on input ...
         // Now, lets check, whether text is a valid double 
         /*bool isValidDouble = Double.TryParse (text, out output);

         // If text is a valid double - we're done 
         if (isValidDouble)
            return true;

         // Lets remove first separator from our input text 
         string textWithoutNumbersSeparator = RemoveFirstOccurrance (text, s_NumbersFormat.Value.NumberDecimalSeparator);

         // Second corner case: 
         // '.' is also valid text, because .1 is a valid double value and user may try to type this value 
         if (string.IsNullOrEmpty(textWithoutNumbersSeparator))
            return true;

         // Now, textWithoutNumbersSeparator should be valid if text contains only one 
         // numberic separator 
         bool isModifiedTextValid = Double.TryParse (textWithoutNumbersSeparator, out output);// StringEx.IsDouble (textWithoutNumbersSeparator);
         return isModifiedTextValid;*/
      }

      /*/// <summary> 
      /// Removes first occurance of valud from text. 
      /// </summary> 
      private static string RemoveFirstOccurrance (string text, string value)
      {
         if (string.IsNullOrEmpty (text))
            return String.Empty;
         if (string.IsNullOrEmpty (value))
            return text;

         int idx = text.IndexOf (value, StringComparison.InvariantCulture);
         if (idx == -1)
            return text;
         return text.Remove (idx, value.Length);
      }*/
   }
}
