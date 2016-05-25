<?php

$dir = dirname (__FILE__) . '/';
require_once($dir . 'include/Params.php');
require_once($dir . 'api_base.php');
require_once($dir . 'include/IinAllDb.php');

class IinAllApi extends Api
{
   public function __construct ($request, $origin)
   {
      parent::__construct($request);
      /*// Abstracted out for example
      $APIKey = new Models\APIKey();
      $User = new Models\User();

      if (!array_key_exists('apiKey', $this->request)) {
            throw new Exception('No API Key provided');
      } else if (!$APIKey->verifyKey($this->request['apiKey'], $origin)) {
            throw new Exception('Invalid API Key');
      } else if (array_key_exists('token', $this->request) &&
             !$User->get('token', $this->request['token'])) {
            throw new Exception('Invalid User Token');
      }
      $this->User = $User;*/
   }

   /**
    * Get a random number of lights.
    * @param $params The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Light ($params)
   {
      if ($this->method == 'GET') {
         $db = new IinAllDb ();
         $light = $db->getRandomLight (25);
         $db->close ();
         return $light;
      } elseif ($this->method == 'POST') {
         $light = $params["l"];
         $key = $params["m"];
         if ($key != "GodisAllLove") { throw new Exception('An unknown error occurred.'); }
         if (empty ($light)) { throw new Exception('No light given'); }
         $db = new IinAllDb ();
         $id = $db->createLight ($light);
         $db->close ();
         return Array('status' => 'success', 'id' => $id);
      } elseif ($this->method == 'PUT') {

      } elseif ($this->method == 'DEL') {

      }
      throw new Exception('An unknown error occurred.');
   }
   
   /**
    * Gets the truth for the given light.
    */
   protected function truth ($params)
   {
      if ($this->method == 'GET') {
         $light = $params["l"];
         if (empty ($light)) {
            throw new Exception('No light given');
         }
         $db = new IinAllDb ();
         $truths = $db->getTruth($light);
         $db->close();
         return Array('status' => 'success', 'truth' => $truths);
      } elseif ($this->method == 'POST') {

      } elseif ($this->method == 'PUT') {

      } elseif ($this->method == 'DEL') {

      }
      throw new Exception('An unknown error occurred.');
   }

   /**
    * Creates a new light.
    */
   protected function createLight ($params)
   {
      if ($this->method == 'GET'){ throw new Exception('An unknown error occurred.'); }
      $light = $params["l"];
      $key = $params["m"];
      if ($key != "GodisAllLove") { throw new Exception('An unknown error occurred.'); }
      if (empty ($light)) { throw new Exception('No light given'); }      
      $db = new IinAllDb ();
      $id = $db->createLight ($light);
      $db->close ();
      return Array('status' => 'success', 'id' => $id);
   }
   
   /**
    * Creates a new love.
    */
   protected function createLove ($params)
   {
      if ($this->method == 'GET'){
         throw new Exception('An unknown error occured.');
      }
      $key = $params["m"];
      if ($key != "GodisAllLove") { throw new Exception('An unknown error occured.'); }
      $light = $params["light"];
      if (empty ($light)) throw new Exception('No light given');
      $db = new IinAllDb ();
      $id = $db->createLove ($light);
      $db->close ();
      return Array('status' => 'success', 'id' => $id);
   }
   
   /**
    * Creates truth.
    */
   protected function createTruth ($params)
   {
      if ($this->method == 'GET') {
         throw new Exception('An unknown error occurred.');
      }
      $key = $params["m"];
      if ($key != "GodisAllLove") { throw new Exception('An unknown error occurred.'); }
      $data = $params["d"];
      $db = new IinAllDb ();
      $log = $db->createTruth ($data);
      $db->close ();
      return Array('status' => 'success', 'log' => $log);
   }
   
   public function format ($params)
   {
      if ($this->method == 'GET') {
         throw new Exception('An unknown error occurred.');
      }      
      $love = $params["l"];
      $data = $params["d"];
      $regex = $params["r"];
      $order = $params["o"];
      $number = $params["n"];
      if (empty ($data)) throw new Exception('No data given');
      if (empty ($regex)) throw new Exception('Cannot parse data without instructions');
      $db = new IinAllDb ();
      $formattedData = $db->format ($love, $data, $regex, $order, $number);
      $db->close ();
      return Array('status' => 'success', 'data' => $formattedData);
   }
   
   /**
    * Get suggestions for the given token.
    */
   protected function suggest ($params)
   {
      $light = Array();
      $token = $params["t"];
      if (!empty ($token)) {
         $db = new IinAllDb ();
         $light = $db->suggest ($token);
         $db->close ();
      }
      return Array('suggestions' => $light);
   }
}
?>