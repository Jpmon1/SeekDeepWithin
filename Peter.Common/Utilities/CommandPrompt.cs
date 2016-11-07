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
using System.Diagnostics;
using System.IO;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Represents a windows command prompt utility.
   /// </summary>
   public class CommandPrompt : ViewModelBase
   {
      private int m_CheckingDir;
      private Process m_CmdProcess;
      private StreamWriter m_Writer;
      private string m_WorkingDirectory;

      /// <summary>
      /// Handler for any output messages for the command prompt
      /// </summary>
      /// <param name="message">Output message.</param>
      /// <param name="isError">True if message is an error, otherwise false.</param>
      public delegate void OutputHandler (string message, bool isError);

      /// <summary>
      /// Occurs when output is received from the command line.
      /// </summary>
      public event OutputHandler OutputRecieved;

      /// <summary>
      /// Initializes a command prompt.
      /// </summary>
      public CommandPrompt ()
      {
         this.m_CheckingDir = 0;
      }

      /// <summary>
      /// Gets the current working directory.
      /// </summary>
      public string WorkingDirectory
      {
         get { return m_WorkingDirectory; }
         private set
         {
            this.m_WorkingDirectory = value;
            this.OnPropertyChanged ("WorkingDirectory");
         }
      }

      /// <summary>
      /// Initializes this command prompt.
      /// </summary>
      public void Initialize ()
      {
         this.m_CmdProcess = new Process
         {
            StartInfo =
            {
               FileName = "cmd.exe",
               WindowStyle = ProcessWindowStyle.Hidden,
               CreateNoWindow = true,
               UseShellExecute = false,
               RedirectStandardError = true,
               RedirectStandardInput = true,
               RedirectStandardOutput = true
            }
         };

         this.m_CmdProcess.OutputDataReceived += HandleOutput;
         this.m_CmdProcess.ErrorDataReceived += HandleError;

         this.m_CmdProcess.Start ();
         this.m_Writer = this.m_CmdProcess.StandardInput;
         this.m_CmdProcess.BeginErrorReadLine ();
         this.m_CmdProcess.BeginOutputReadLine ();
         this.RefreshCurrentDirectory ();
      }

      /// <summary>
      /// Gets the handle to the process.
      /// </summary>
      public IntPtr Handle
      {
         get
         {
            return this.m_CmdProcess == null ? IntPtr.Zero : this.m_CmdProcess.Handle;
         }
      }

      /// <summary>
      /// Checks for the current directory.
      /// </summary>
      public void RefreshCurrentDirectory ()
      {
         this.m_CheckingDir = 1;
         this.m_Writer.WriteLine ("cd");
      }

      /// <summary>
      /// Handles any error coming from the command line.
      /// </summary>
      /// <param name="sender">Command prompt process.</param>
      /// <param name="e">DataReceivedEventArgs</param>
      void HandleError (object sender, DataReceivedEventArgs e)
      {
         if (!string.IsNullOrEmpty (e.Data))
         {
            this.GuiDispatcher.Invoke (new Action<string, bool> (this.UpdateOutput), e.Data, true);
         }
      }

      /// <summary>
      /// Handles normal output coming from the command line.
      /// </summary>
      /// <param name="sender">Command prompt process.</param>
      /// <param name="e">DataReceivedEventArgs</param>
      void HandleOutput (object sender, DataReceivedEventArgs e)
      {
         if (!string.IsNullOrEmpty (e.Data))
         {
            if (this.m_CheckingDir == 1 && e.Data.EndsWith (">cd"))
            {
               this.m_CheckingDir = 2;
            }
            else if (this.m_CheckingDir == 2 && Directory.Exists (e.Data))
            {
               this.m_CheckingDir = 0;
               this.WorkingDirectory = e.Data;
            }
            else
            {
               this.GuiDispatcher.Invoke (new Action<string, bool> (this.UpdateOutput), e.Data, false);
            }
         }
      }

      /// <summary>
      /// Changes the current working directory.
      /// </summary>
      /// <param name="workingDirectory">Working directory to change to.</param>
      public void ChangeDirectory (string workingDirectory)
      {
         int index = workingDirectory.IndexOf (":\\", StringComparison.Ordinal);
         if (index > 0)
         {
            this.m_Writer.WriteLine (workingDirectory.Substring (0, index + 1));
         }
         this.m_Writer.WriteLine ("cd " + workingDirectory);
         this.RefreshCurrentDirectory ();
      }

      /// <summary>
      /// Updates the displayed output.
      /// </summary>
      /// <param name="text">Text to add to output.</param>
      /// <param name="isError">True if text is an error, otherwise false.</param>
      private void UpdateOutput (string text, bool isError = false)
      {
         if (this.OutputRecieved != null)
            this.OutputRecieved (text, isError);
         else if (isError)
            Console.Error.WriteLine (text);
         else
            Console.WriteLine (text);
      }

      /// <summary>
      /// Runs the given command...
      /// </summary>
      /// <param name="command">Command to run.</param>
      public void RunCommand (string command)
      {
         this.m_Writer.WriteLine (command);
         this.RefreshCurrentDirectory ();
      }

      /// <summary>
      /// Closes this command prompt.
      /// </summary>
      public void Close ()
      {
         this.m_Writer.WriteLine ("exit");
         this.m_Writer.Dispose ();
         this.m_CmdProcess.Dispose ();
      }
   }
}
