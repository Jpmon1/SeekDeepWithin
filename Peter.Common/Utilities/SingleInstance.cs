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
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Helper for a single instance application.
   /// </summary>
   public static class SingleInstance
   {
      private static readonly bool s_OwnsMutex;
      private static Mutex s_Mutex;
      private const string GUID = "{2fd4eae2-d630-4075-87ec-34fe6f604324}";

      /// <summary>
      /// Occurs when [arguments received].
      /// </summary>
      public static event EventHandler<GenericEventArgs<string>> ArgumentsReceived;

      /// <summary>
      /// Initializes a new instance of the <see cref="SingleInstance"/> class.
      /// </summary>
      static SingleInstance ()
      {
         s_Mutex = new Mutex (true, GUID, out s_OwnsMutex);
      }

      /// <summary>
      /// Gets a value indicating whether this instance is first instance.
      /// </summary>
      /// <value>
      /// 	<c>true</c> if this instance is first instance; otherwise, <c>false</c>.
      /// </value>
      public static bool IsFirstInstance { get { return s_OwnsMutex; } }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public static void Dispose ()
      {
         if (s_Mutex != null && s_OwnsMutex)
         {
            s_Mutex.ReleaseMutex ();
            s_Mutex = null;
         }
      }

      /// <summary>
      /// Passes the arguments to first instance.
      /// </summary>
      /// <param name="argument">The argument to pass.</param>
      public static void PassArgumentsToFirstInstance (string argument = "")
      {
         using (var client = new NamedPipeClientStream (GUID))
         {
            using (var writer = new StreamWriter (client))
            {
               client.Connect (200);
               writer.WriteLine (argument);
            }
         }
      }

      /// <summary>
      /// Listens for arguments from successive instances.
      /// </summary>
      public static void ListenForArgumentsFromSuccessiveInstances ()
      {
         Action action = StartArgumentServer;
         action.BeginInvoke (null, null);
      }

      /// <summary>
      /// Listens for arguments from successive instances on a separate thread.
      /// </summary>
      private static void StartArgumentServer ()
      {
         using (var server = new NamedPipeServerStream (GUID))
         {
            using (var reader = new StreamReader (server))
            {
               while (true)
               {
                  server.WaitForConnection ();

                  var argument = string.Empty;
                  while (server.IsConnected)
                  {
                     argument += reader.ReadLine ();
                  }

                  CallOnArgumentsReceived (argument);
                  server.Disconnect ();
               }
            }
         }
      }

      /// <summary>
      /// Calls the on arguments received.
      /// </summary>
      /// <param name="state">The state.</param>
      public static void CallOnArgumentsReceived (object state)
      {
         if (ArgumentsReceived != null)
         {
            if (state == null)
            {
               state = string.Empty;
            }

            ArgumentsReceived (null, new GenericEventArgs<string> { Data = state.ToString () });
         }
      }
   }

   /// <summary>
   /// Generic event arguments.
   /// </summary>
   /// <typeparam name="T">Type of data for event args.</typeparam>
   public class GenericEventArgs<T> : EventArgs
   {
      /// <summary>
      /// Gets or sets the data.
      /// </summary>
      /// <value>The data.</value>
      public T Data { get; set; }
   }
}
