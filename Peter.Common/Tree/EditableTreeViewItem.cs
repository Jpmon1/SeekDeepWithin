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

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Peter.Common.Icons;

namespace Peter.Common.Tree
{
   /// <summary>
   /// Represents an editable tree item.
   /// </summary>
   public abstract class EditableTreeViewItem : ModelTreeViewItem
   {
      private RelayCommand m_CmdEdit;
      private Brush m_BorderBrush = Brushes.Black;
      private IconSize m_IconSize = IconSize.MediumLarge;
      private Thickness m_IconPadding = new Thickness (0);
      private Thickness m_BorderThickness = new Thickness (0);
      private Thickness m_IconMargin = new Thickness (0, 0, 3, 0);

      /// <summary>
      /// Initializes a new editable tree view item.
      /// </summary>
      /// <param name="parent">Parent of item.</param>
      /// <param name="dynamicLoad">True to load dynamically</param>
      protected EditableTreeViewItem (ModelTreeViewItem parent, bool dynamicLoad)
         : base (parent, dynamicLoad)
      {
      }

      /// <summary>
      /// Gets the edit command.
      /// </summary>
      public ICommand EditCommand
      {
         get { return this.m_CmdEdit ?? (this.m_CmdEdit = new RelayCommand (this.OnEdit, this.CanEdit)); }
      }

      /// <summary>
      /// Gets or Sets the brush for the border of the icon.
      /// </summary>
      public Brush BorderBrush
      {
         get { return this.m_BorderBrush; }
         protected set
         {
            this.m_BorderBrush = value;
            this.OnPropertyChanged ("BorderBrush");
         }
      }

      /// <summary>
      /// Gets or Sets the thickness of the border around the icon.
      /// </summary>
      public Thickness BorderThickness
      {
         get { return this.m_BorderThickness; }
         protected set
         {
            this.m_BorderThickness = value;
            this.OnPropertyChanged ("BorderThickness");
         }
      }

      /// <summary>
      /// Gets or Sets the size of the icon.
      /// </summary>
      public IconSize IconSize
      {
         get { return this.m_IconSize; }
         protected set
         {
            this.m_IconSize = value;
            this.OnPropertyChanged ("IconSize");
         }
      }

      /// <summary>
      /// Gets or Sets the margin of the border around the icon.
      /// </summary>
      public Thickness IconMargin
      {
         get { return this.m_IconMargin; }
         protected set
         {
            this.m_IconMargin = value;
            this.OnPropertyChanged ("IconMargin");
         }
      }

      /// <summary>
      /// Gets or Sets the padding for the border around the icon.
      /// </summary>
      public Thickness IconPadding
      {
         get { return this.m_IconPadding; }
         protected set
         {
            this.m_IconPadding = value;
            this.OnPropertyChanged ("IconPadding");
         }
      }

      /// <summary>
      /// Checks whether an edit actions can be performed or not.
      /// </summary>
      /// <param name="obj">Edit action</param>
      /// <returns>True if action can be performed, otherwise false.</returns>
      protected virtual bool CanEdit (object obj)
      {
         var editAction = (EditAction)obj;
         switch (editAction)
         {
            case EditAction.Copy:
               return true;
            case EditAction.Cut:
               return false;
            case EditAction.Paste:
               return false;
            default:
               return false;
         }
      }

      /// <summary>
      /// Performs the edit action.
      /// </summary>
      /// <param name="obj">Edit action.</param>
      protected virtual void OnEdit (object obj)
      {
         var editAction = (EditAction)obj;
         switch (editAction)
         {
            case EditAction.Copy:
               break;
            case EditAction.Cut:
               break;
            case EditAction.Paste:
               break;
         }
      }
   }
}
