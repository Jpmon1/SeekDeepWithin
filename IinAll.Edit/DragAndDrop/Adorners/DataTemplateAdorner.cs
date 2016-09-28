/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows;
using System.Windows.Controls;

namespace IinAll.Edit.DragAndDrop.Adorners
{
   /// <summary>
   /// Adorner for a given data template.
   /// </summary>
   public class DataTemplateAdorner : DragNDropAdorner
   {
      /// <summary>
      /// Content presenter for adornment.
      /// </summary>
      private readonly ContentPresenter m_Content;

      /// <summary>
      /// Initializes a new drag adroner.
      /// </summary>
      /// <param name="adornedElement">Element being adorned.</param>
      /// <param name="template">Template used for adornment.</param>
      /// <param name="data">Bound data for template.</param>
      public DataTemplateAdorner (UIElement adornedElement, DataTemplate template, object data)
         : base (adornedElement)
      {
         this.m_Content = new ContentPresenter { Content = data, ContentTemplate = template };
      }

      /// <summary>
      /// Gets the adornment.
      /// </summary>
      protected override UIElement Adornment
      {
         get { return this.m_Content; }
      }
   }
}
