using System;
using System.Net;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// A web client that is aware of cookies.
   /// </summary>
   public class CookieAwareWebClient : WebClient
   {
      /// <summary>
      /// Initializes a new cookie aware web client.
      /// </summary>
      public CookieAwareWebClient ()
      {
         this.CookieContainer = new CookieContainer();
      }

      /// <summary>
      /// Gets the cookie container.
      /// </summary>
      public CookieContainer CookieContainer { get; private set; }

      /// <summary>
      /// Returns a <see cref="T:System.Net.WebRequest"/> object for the specified resource.
      /// </summary>
      /// <returns>
      /// A new <see cref="T:System.Net.WebRequest"/> object for the specified resource.
      /// </returns>
      /// <param name="address">A <see cref="T:System.Uri"/> that identifies the resource to request.</param>
      protected override WebRequest GetWebRequest (Uri address)
      {
         //Grabs the base request being made
         var request = (HttpWebRequest)base.GetWebRequest (address);
         //Adds the existing cookie container to the Request
         if (request != null) {
            request.CookieContainer = CookieContainer;
         }
         return request;
      }
   }
}
