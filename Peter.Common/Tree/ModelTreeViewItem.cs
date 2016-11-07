/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using Peter.Common.MainMenu;

namespace Peter.Common.Tree
{
   /// <summary>
   /// Logic for a TreeViewItemViewModel object that is used to display data in a treeview.
   /// </summary>
   public class ModelTreeViewItem : INotifyPropertyChanged
   {
      #region Fields

      /// <summary>
      /// Dummy tree item to dynamically load sub items.
      /// </summary>
      static readonly ModelTreeViewItem s_DummyChild = new ModelTreeViewItem { Text = "Loading..." };

      /// <summary>
      /// Tree item text.
      /// </summary>
      private string m_Text;

      /// <summary>
      /// Is this item expanded or not.
      /// </summary>
      private bool m_IsExpanded;

      /// <summary>
      /// Is this item selected or not.
      /// </summary>
      private bool m_IsSelected;

      /// <summary>
      /// Is this item enabled or not.
      /// </summary>
      private bool m_IsEnabled;

      /// <summary>
      /// Will this node dynamically load it's children?
      /// </summary>
      private bool m_IsDynamicLoad;
      
      /// <summary>
      /// Icon to display.
      /// </summary>
      private ImageSource m_IconSource;

      /// <summary>
      /// Additional data to attach.
      /// </summary>
      private object m_Tag;

      /// <summary>
      /// Parent tree view item container.
      /// </summary>
      private TreeViewItem m_TreeViewItem;

      /// <summary>
      /// Parent item.
      /// </summary>
      private readonly ModelTreeViewItem m_Parent;

      /// <summary>
      /// Children items.
      /// </summary>
      private readonly ObservableCollection<ModelTreeViewItem> m_Children;

      /// <summary>
      /// Main menu icon.
      /// </summary>
      private MainMenuIcon m_Icon = MainMenuIcon.None;

      private Brush m_IconBrush = Brushes.Black;

      #endregion

      #region Setup

      /// <summary>
      /// Initializes a new tree view item model.
      /// </summary>
      /// <param name="parent">Parent item.</param>
      public ModelTreeViewItem (ModelTreeViewItem parent)
         : this (parent, false)
      {
      }

      /// <summary>
      /// Initializes a new tree view item model.
      /// </summary>
      /// <param name="parent">Parent item.</param>
      /// <param name="dynamicLoad">True if this item will dynamically load it's children, otherwise false.</param>
      public ModelTreeViewItem (ModelTreeViewItem parent, bool dynamicLoad)
      {
         this.m_Text = string.Empty;
         this.IsEnabled = true;
         this.m_Parent = parent;
         this.m_IsDynamicLoad = dynamicLoad;
         this.m_Children = new ObservableCollection<ModelTreeViewItem>();
         if (this.m_IsDynamicLoad) this.m_Children.Add(s_DummyChild);
      }

      /// <summary>
      /// Private constructor for dummy item.
      /// </summary>
      private ModelTreeViewItem () { }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or Sets if the node will dynamically load it's children.
      /// </summary>
      public bool DynamicLoad
      {
         get { return this.m_IsDynamicLoad; }
         set
         {
            if (this.m_IsDynamicLoad != value)
            {
               this.m_IsDynamicLoad = value;
               // If children are present, clear children and collapse.
               if (this.Children.Count > 0)
               {
                  this.Children.Clear ();
                  this.IsExpanded = false;
               }
               this.m_Children.Add (s_DummyChild);
               OnPropertyChanged("DynamicLoad");
            }
         }
      }

      /// <summary>
      /// Gets or Sets the text to display.
      /// </summary>
      public string Text
      {
         get { return this.m_Text; }
         set
         {
            this.m_Text = value;
            this.OnPropertyChanged("Text");
         }
      }

      /// <summary>
      /// Gets or Sets additional data attached to the node.
      /// </summary>
      public object Tag
      {
         get { return this.m_Tag; }
         set
         {
            this.m_Tag = value;
            this.OnPropertyChanged("Data");
         }
      }

      /// <summary>
      /// Gets the collection of children.
      /// </summary>
      public ObservableCollection<ModelTreeViewItem> Children
      {
         get { return this.m_Children; }
      }

      /// <summary>
      /// Gets the parent item.
      /// </summary>
      public ModelTreeViewItem Parent
      {
         get { return this.m_Parent; }
      }

      /// <summary>
      /// Returns true if this object's Children have not yet been populated.
      /// </summary>
      public bool HasDummyChild
      {
         get { return this.Children.Count == 1 && this.Children[0] == s_DummyChild; }
      }

      /// <summary>
      /// Gets or Sets if this item is expanded or not.
      /// </summary>
      public bool IsExpanded
      {
         get { return this.m_IsExpanded; }
         set
         {
            // Expand parents...
            if (value && this.m_Parent != null)
               this.m_Parent.IsExpanded = true;

            // Expand this...
            if (value != this.m_IsExpanded)
            {
               this.m_IsExpanded = value;
               this.OnPropertyChanged("IsExpanded");
            }

            // Clear children when collapsing...
            if (!this.m_IsExpanded && this.DynamicLoad)
            {
               this.Children.Clear ();
               this.Children.Add (s_DummyChild);
            }

            // Change icon, if folder...
            this.CheckFolderIcon ();

            // Load children, if necessary...
            if (this.HasDummyChild)
            {
               this.Children.Remove(s_DummyChild);
               this.LoadChildren();
            }
         }
      }

      /// <summary>
      /// Checks for updating the folder icon.
      /// </summary>
      private void CheckFolderIcon ()
      {
         if (this.Icon == MainMenuIcon.FolderClose && this.m_IsExpanded)
            this.Icon = MainMenuIcon.FolderOpen;
         if (this.Icon == MainMenuIcon.FolderOpen && !this.m_IsExpanded)
            this.Icon = MainMenuIcon.FolderClose;
      }

      /// <summary>
      /// Gets or Sets if this item is selected or not.
      /// </summary>
      public bool IsSelected
      {
         get { return this.m_IsSelected; }
         set
         {
            if (value != this.m_IsSelected)
            {
               this.m_IsSelected = value;
               this.OnPropertyChanged("IsSelected");
               if (this.m_IsSelected)
                  this.OnSelected ();
            }
         }
      }

      /// <summary>
      /// Occurs when this item is selected.
      /// </summary>
      protected virtual void OnSelected () {}

      /// <summary>
      /// Gets or Sets if this item is enabled.
      /// </summary>
      public bool IsEnabled
      {
         get { return this.m_IsEnabled; }
         set
         {
            if (value != this.m_IsEnabled)
            {
               this.m_IsEnabled = value;
               this.OnPropertyChanged("IsEnabled");
            }
         }
      }

      /// <summary>
      /// Gets or Sets the path to the icon
      /// </summary>
      public ImageSource IconSource
      {
         get { return this.m_IconSource; }
         set
         {
            this.m_IconSource = value;
            this.OnPropertyChanged ("IconSource");
         }
      }

      /// <summary>
      /// Main menu icon for this item.
      /// </summary>
      public MainMenuIcon Icon
      {
         get { return this.m_Icon; }
         set
         {
            this.m_Icon = value;
            this.OnPropertyChanged ("Icon");
         }
      }

      /// <summary>
      /// Gets or Sets the brush for the icon.
      /// </summary>
      public Brush IconBrush
      {
         get { return this.m_IconBrush; }
         set
         {
            this.m_IconBrush = value;
            this.OnPropertyChanged ("IconBrush");
         }
      }
      
      /// <summary>
      /// Gets or Sets the container for this tree item.
      /// </summary>
      public TreeViewItem TreeViewItem
      {
         get { return this.m_TreeViewItem; }
         set
         {
            this.m_TreeViewItem = value;
            this.OnPropertyChanged ("TreeViewItem");
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Loads any children nodes.
      /// </summary>
      protected virtual void LoadChildren () { }

      /// <summary>
      /// Refreshes the dynamic load.
      /// </summary>
      public void RefreshDynamicLoad ()
      {
         this.IsExpanded = false;
         this.Children.Clear ();
         this.Children.Add (s_DummyChild);
         this.IsExpanded = true;
      }

      /// <summary>
      /// Resets the dynamic load.
      /// </summary>
      public void ResetDynamicLoad ()
      {
         this.IsExpanded = false;
         this.Children.Clear ();
         this.Children.Add (s_DummyChild);
      }

      /// <summary>
      /// Overriden to return text as string.
      /// </summary>
      /// <returns>Text for tree item.</returns>
      public override string ToString ()
      {
         return this.Text;
      }

      /// <summary>
      /// Overridden to make compiler happy.
      /// </summary>
      /// <returns>Base GetHasCode.</returns>
      public override int GetHashCode ()
      {
         int hash = 13;
         hash = (hash * 7) + this.Text.GetHashCode();
         if (this.Tag != null)
            hash = (hash * 7) + this.Tag.GetHashCode ();
         if (this.Parent != null)
            hash = (hash * 7) + this.Parent.GetHashCode ();
         return hash;
      }

      #endregion

      #region Property Changed

      /// <summary>
      /// Event used to notify when a property has changed.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Fires the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of property that changed.</param>
      protected void OnPropertyChanged (string propertyName)
      {
         if (this.PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }

      #endregion
   }
}
