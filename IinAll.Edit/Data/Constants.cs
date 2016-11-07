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
      /// The truth url.
      /// </summary>
      public const string URL_GET_TRUTH = "api/api.php?request=Truth&light=";

      /// <summary>
      /// The love url.
      /// </summary>
      public const string URL_GET_LOVE = "api/api.php?request=Love&p=";

      /// <summary>
      /// The truth url.
      /// </summary>
      public const string URL_TRUTH = "api/api.php?request=Truth";

      /// <summary>
      /// The light url.
      /// </summary>
      public const string URL_LIGHT = "api/api.php?request=Light";

      /// <summary>
      /// The body url.
      /// </summary>
      public const string URL_BODY = "api/api.php?request=Body";

      /// <summary>
      /// The body url.
      /// </summary>
      public const string URL_GET_BODY = "api/api.php?request=Body&l=";

      /// <summary>
      /// The body url.
      /// </summary>
      public const string URL_STYLE = "api/api.php?request=Style";

      /// <summary>
      /// The body url.
      /// </summary>
      public const string URL_GET_STYLE = "api/api.php?request=Style&l=";

      /// <summary>
      /// The alias url.
      /// </summary>
      public const string URL_ALIAS = "api/api.php?request=Alias";

      /// <summary>
      /// The alias url.
      /// </summary>
      public const string URL_GET_ALIAS = "api/api.php?request=Alias&l=";

      /// <summary>
      /// Success response (success).
      /// </summary>
      public const string SUCCESS = "success";

      /// <summary>
      /// Failed response (fail).
      /// </summary>
      public const string FAIL = "fail";

      /// <summary>
      /// The save data file.
      /// </summary>
      public const string SAVE_FILE = "IinAll.dat";
   }
}
