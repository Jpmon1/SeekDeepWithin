using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IinAll.Edit.Data;
using Newtonsoft.Json.Linq;

namespace IinAll.Edit.Logic
{
   /// <summary>
   /// A queue for web requests.
   /// </summary>
   public class WebQueue : IDisposable
   {
      private bool m_IsGetting;
      private bool m_IsPosting;
      private static WebQueue s_Instance;
      private readonly CookieAwareWebClient m_WebClient;
      private readonly Queue<RequestData> m_GetQueue = new Queue<RequestData> ();
      private readonly Queue<RequestData> m_PostQueue = new Queue<RequestData> ();
      private const string BASE_ADDRESS = "http://localhost/IinAllDev/";

      /// <summary>
      /// Initializes a new web queue.
      /// </summary>
      public WebQueue ()
      {
         this.m_IsGetting = false;
         this.m_IsPosting = false;
         this.m_WebClient = new CookieAwareWebClient ();
      }

      /// <summary>
      /// Gets the one and only instance of the web queue.
      /// </summary>
      public static WebQueue Instance
      {
         get { return s_Instance ?? (s_Instance = new WebQueue ()); }
      }

      /// <summary>
      /// Gets or Sets the request token to append to posts.
      /// </summary>
      public string Token { get; private set; }

      /// <summary>
      /// Gets if the web client has been authenticated or not.
      /// </summary>
      public bool IsAuthenticated { get; private set; }

      /// <summary>
      /// Gets or Sets the user id.
      /// </summary>
      public int UserId { get; private set; }

      /// <summary>
      /// Gets or Sets the user name.
      /// </summary>
      public string UserName { get; private set; }

      /// <summary>
      /// Gets or Sets the user email.
      /// </summary>
      public string UserEmail { get; private set; }

      /// <summary>
      /// Gets or Sets the user's edit level.
      /// </summary>
      public int EditLevel { get; private set; }

      /// <summary>
      /// Queues the given url for data retrieval.
      /// </summary>
      /// <param name="url">Url to request data from.</param>
      /// <param name="success">An action to perform on successful return.</param>
      public void Get (string url, Action<NameValueCollection, dynamic> success = null)
      {
         this.m_GetQueue.Enqueue (new RequestData { Url = url, Success = success });
         if (!this.m_IsGetting)
            this.GetAll ();
      }

      /// <summary>
      /// Queues the given url for data retrieval.
      /// </summary>
      /// <param name="url">Url to request data from.</param>
      /// <param name="parameters">The list of parameters to send.</param>
      /// <param name="success">An action to perform on successful return.</param>
      public void Post (string url, NameValueCollection parameters, 
         Action<NameValueCollection, dynamic> success = null)
      {
         this.m_PostQueue.Enqueue (new RequestData {Url = url, Parameters = parameters, Success = success});
         if (!this.m_IsPosting)
            this.PostAll ();
      }

      /// <summary>
      /// Performs all requests in the queue.
      /// </summary>
      private async void GetAll ()
      {
         this.m_IsGetting = true;
         while (this.m_IsPosting) { await Task.Run (() => Thread.Sleep (10)); }
         while (true) {
            dynamic responseJObj;
            string responseText = string.Empty;
            var data = this.m_GetQueue.Dequeue ();
            try {
               responseText = await this.m_WebClient.DownloadStringTaskAsync (new Uri (BASE_ADDRESS + data.Url));
               responseJObj = JObject.Parse (responseText);
            } catch (Exception ex) {
               MessageBox.Show (Application.Current.MainWindow, ex.Message + "\n" + responseText, "I in All",
                  MessageBoxButton.OK, MessageBoxImage.Error);
               if (this.m_GetQueue.Count > 0)
                  continue;
               break;
            }
            if (responseJObj.status == Constants.FAIL) {
               string message = responseJObj.message ?? string.Empty;
               MessageBox.Show (Application.Current.MainWindow, message, "I in All",
                  MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
               if (data.Success != null)
                  data.Success (data.Parameters, responseJObj);
            }
            if (this.m_GetQueue.Count > 0)
               continue;
            break;
         }
         this.m_IsGetting = false;
      }

      /// <summary>
      /// Performs all requests in the queue.
      /// </summary>
      private async void PostAll ()
      {
         this.m_IsPosting = true;
         while (this.m_IsGetting) { await Task.Run (() => Thread.Sleep (10)); }
         while (true)
         {
            dynamic responseJObj;
            string responseText = string.Empty;
            var postData = this.m_PostQueue.Dequeue ();
            try {
               byte[] response = await this.m_WebClient.UploadValuesTaskAsync (new Uri (BASE_ADDRESS + postData.Url),
                  "POST", postData.Parameters);
               responseText = Encoding.UTF8.GetString (response);
               responseJObj = JObject.Parse (responseText);
            } catch (Exception ex) {
               MessageBox.Show (Application.Current.MainWindow, ex.Message + "\n" + responseText, "I in All",
                  MessageBoxButton.OK, MessageBoxImage.Error);
               if (this.m_PostQueue.Count > 0)
                  continue;
               break;
            }
            if (postData.Url == Constants.URL_LOGIN_REQUEST && responseJObj.status == Constants.SUCCESS) {
               this.UserId = responseJObj.id;
               this.Token = responseJObj.token;
               this.UserName = responseJObj.name;
               this.UserEmail = responseJObj.email;
               this.EditLevel = responseJObj.level;
               this.IsAuthenticated = true;
               CommandManager.InvalidateRequerySuggested ();
            }
            if (responseJObj.status == Constants.FAIL) {
               string message = responseJObj.message ?? string.Empty;
               MessageBox.Show (Application.Current.MainWindow, message, "I in All",
                  MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
               if (postData.Success != null)
                  postData.Success (postData.Parameters, responseJObj);
            }
            if (this.m_PostQueue.Count > 0)
               continue;
            break;
         }
         this.m_IsPosting = false;
      }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose ()
      {
         this.m_WebClient.Dispose();
      }

      /// <summary>
      /// Data for posting.
      /// </summary>
      private class RequestData
      {
         /// <summary>
         /// The url to post data to.
         /// </summary>
         public string Url { get; set; }

         /// <summary>
         /// The parameters to post.
         /// </summary>
         public NameValueCollection Parameters { get; set; }
         
         /// <summary>
         /// Gets or Sets a action to call on success.
         /// </summary>
         public Action<NameValueCollection, dynamic> Success { get; set; }
      }
   }
}
