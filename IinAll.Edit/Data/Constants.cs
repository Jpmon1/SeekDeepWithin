namespace IinAll.Edit.Data
{
   /// <summary>
   /// Constants for SDW
   /// </summary>
   public static class Constants
   {
      /// <summary>
      /// The login url.
      /// </summary>
      public const string URL_LOGIN_REQUEST = "api/api.php?request=Login";

      /// <summary>
      /// The search url.
      /// </summary>
      public const string URL_SEARCH = "api/api.php?request=Suggest&t=";

      /// <summary>
      /// Success response (success).
      /// </summary>
      public const string SUCCESS = "success";

      /// <summary>
      /// Failed response (fail).
      /// </summary>
      public const string FAIL = "fail";
   }
}
