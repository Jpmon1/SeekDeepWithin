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

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Peter.Common.Utilities;

namespace Peter.Common.Tests.Utilities
{
   /// <summary>
   /// Tests the focus extensions.
   /// </summary>
   [TestClass]
   public class FocusExtensionTests : ViewModelBase
   {
      private bool m_IsFocused;
      private int m_FoucsedEventCount;
      private FocusExtension.FocusEvent m_FocusEvent;

      /// <summary>
      /// Tests the set foucs attached property.
      /// </summary>
      [TestMethod]
      public void TestSetFocus ()
      {
         var textBox = new TextBox ();
         textBox.SetBinding (FocusExtension.IsFocusedProperty, new Binding ("IsFocused") { Source = this });
         Assert.IsNotNull (FocusExtension.GetIsFocused (textBox));
         Assert.IsFalse (textBox.IsFocused);
         this.IsFocused = true;
         Assert.IsTrue (textBox.IsFocused);
      }

      /// <summary>
      /// Tests the focus events attached property.
      /// </summary>
      [TestMethod]
      public void TestFocusEvents ()
      {
         var textBox = new TextBox ();
         textBox.SetBinding (FocusExtension.FocusEventsProperty, new Binding ("FocusCommand") { Source = this });
         this.OnPropertyChanged ("FocusCommand"); // This tests if we connect more than one event by re-binding.
         this.m_FoucsedEventCount = 0;
         textBox.Focus ();
         Assert.AreEqual (FocusExtension.FocusEvent.GotFocus, this.m_FocusEvent);
         Assert.AreEqual (1, this.m_FoucsedEventCount);
      }

      /// <summary>
      /// Gets or Sets if the text box is focused or not.
      /// Use with FocusExtension.
      /// </summary>
      public bool IsFocused
      {
         get { return this.m_IsFocused; }
         set
         {
            this.m_IsFocused = value;
            this.OnPropertyChanged ("IsFocused");
         }
      }

      /// <summary>
      /// Gets the command for focus events.
      /// </summary>
      public ICommand FocusCommand
      {
         get { return new RelayCommand (this.OnFocusEvent); }
      }

      /// <summary>
      /// Handles focus events.
      /// </summary>
      /// <param name="obj">Focus event.</param>
      private void OnFocusEvent (object obj)
      {
         this.m_FoucsedEventCount++;
         Assert.IsTrue (obj is FocusExtension.FocusEvent);
         this.m_FocusEvent = (FocusExtension.FocusEvent) obj;
      }
   }
}
