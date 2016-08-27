using System;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// Event handler for web requests.
   /// </summary>
   /// <param name="e">Web Request events.</param>
   public delegate void WebRequestEventHandler (object sender, WebRequestEventArgs e);

   /// <summary>
   /// Event arguments for web requests.
   /// </summary>
   public class WebRequestEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new web request event args.
      /// </summary>
      /// <param name="url">The url that requested data.</param>
      /// <param name="response">The JSON parsed response given.</param>
      /// <param name="data">Any additional data.</param>
      public WebRequestEventArgs (string url, dynamic response, object data = null)
      {
         this.Url = url;
         this.Data = data;
         this.Response = response;
      }

      /// <summary>
      /// Gets any additional data attached to the action
      /// </summary>
      public object Data { get; private set; }

      /// <summary>
      /// Gets the url requested.
      /// </summary>
      public string Url { get; private set; }

      /// <summary>
      /// Gets the response of the request.
      /// </summary>
      public dynamic Response { get; private set; }
   }
}
