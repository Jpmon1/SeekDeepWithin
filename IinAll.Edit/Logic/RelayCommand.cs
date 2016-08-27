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
using System.Windows.Input;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// A class for a generic command that can check for execution and execute.
   /// </summary>
   public class RelayCommand : ICommand
   {
      /// <summary>
      /// Execute action.
      /// </summary>
      private readonly Action<object> m_Execute;

      /// <summary>
      /// Can execute logic.
      /// </summary>
      private readonly Predicate<object> m_CanExecute;

      /// <summary>
      /// Creates a new command that can alway execute.
      /// </summary>
      /// <param name="execute">Execution logic.</param>
      public RelayCommand (Action<object> execute) : this (execute, null) { }

      /// <summary>
      /// Creates a new command.
      /// </summary>
      /// <param name="execute">Execution logic.</param>
      /// <param name="canExecute">Can execute logic.</param>
      public  RelayCommand (Action<object> execute, Predicate<object> canExecute)
      {
         if (execute == null)
            throw new ArgumentNullException("execute");

         this.m_Execute = execute;
         this.m_CanExecute = canExecute;
      }

      /// <summary>
      /// Occurs to check if the command can execute.
      /// </summary>
      public event EventHandler CanExecuteChanged
      {
         add { CommandManager.RequerySuggested += value; }
         remove { CommandManager.RequerySuggested -= value; }
      }

      /// <summary>
      /// Gets if the command can execute.
      /// </summary>
      /// <param name="parameter">Command paremeter.</param>
      /// <returns>True if able to execute, otherwise false.</returns>
      public bool CanExecute (object parameter)
      {
         return this.m_CanExecute == null || this.m_CanExecute(parameter);
      }

      /// <summary>
      /// Executes the command.
      /// </summary>
      /// <param name="parameter">Command paremeter.</param>
      public void Execute (object parameter)
      {
         this.m_Execute (parameter);
      }
   }
}
