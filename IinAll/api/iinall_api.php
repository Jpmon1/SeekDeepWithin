<?php

$dir = dirname (__FILE__) . '/';
require_once ($dir . 'User.php');
require_once ($dir . 'api_base.php');
require_once ($dir . 'IinAllDb.php');

class IinAllApi extends Api
{
   public function __construct ($request, $origin)
   {
      parent::__construct ($request);
   }

   /**
    * Logs a user into the server.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Login ($params)
   {
      if ($this->method == 'POST') {
         $email = $params["email"];
         if (empty ($email)) {
            throw new Exception ("You must provide an email address.");
         }
         $data = array();
         $user = new User ();
         if ($user->isUserLoggedIn ()) {
            if ($user->getUserEmail () == $email) {
               return array('status' => 'error', 'message' => 'User is already logged in.');
            } else {
               $user->logOut ();
            }
         }
         $hash = $params["hash"];
         if (empty ($hash)) {
            throw new Exception ("You must provide a password.");
         }
         $result = $user->logIn ($email, $hash);
         if ($result === true) {
            return $this->CheckUser ($params, $user);
         }
         $data['status'] = 'fail';
         $data['message'] = $result;
         return $data;
      }
      throw new Exception ("An unknown error occurred.");
   }

   /**
    * Registers a new user.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Register ($params)
   {
      if ($this->method == 'POST') {
         $email = $params["email"];
         $name = $params["name"];
         $hash = $params["hash"];
         if (empty ($email)) { throw new Exception ("You must provide an email address."); }
         if (empty ($name))  { throw new Exception ("You must provide a name."); }
         if (empty ($hash))  { throw new Exception ("You must provide a password."); }
         $data = array();
         $user = new User ();
         $result = $user->register ($name, $email, $hash);
         if ($result === true){
            return $this->CheckUser ($params, $user);
         }
         $data['status'] = 'fail';
         $data['message'] = $result;
         return $data;
      }
      throw new Exception ("An unknown error occurred.");
   }

   /**
    * Checks if there is a current user.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function CheckUser ($params, $user = null)
   {
      if ($user == null) {
         $user = new User ();
      }
      if ($user->isUserLoggedIn ()) {
         return array('status' => 'success',
                      'id' => $user->getUserId (),
                      'name' => $user->getUserName (),
                      'email' => $user->getUserEmail (),
                      'token' => $user->getEditToken (),
                      'level' => $user->getLevel ());
      }
      return array('status' => 'fail', 'message' => 'The user is not logged in.');
   }

   /**
    * Logs a user out of the server.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Logout ()
   {
      $user = new User ();
      $user->logout ();
      return array('status' => 'success');
   }

   /**
    * Get a random number of lights.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Light ($params)
   {
      $data = array();
      $db = new IinAllDb ();
      try {
         if ($this->method == 'GET') {
            $data = $db->getRandomLight(25);
         } else {
            $db = new UserDb ();
            //$verify = $db->verifyToken ($params['user'], $params['token']);
            if (!$db->verifyToken ($params['user'], $params['token'])) {
                  throw new Exception ('Invalid User.');
            }
            if ($this->method == 'POST') {
               $light = $params["text"];
               if (empty ($light)) {
                  throw new Exception('No light given');
               }
               $data['status'] = 'success';
               $data['id'] = $db->createLight($light);
            } elseif ($this->method == 'PUT') {
               $db = new IinAllDb ();
               $light = $params["text"];
               $id = $params["id"];
               if (empty ($light)) {
                  throw new Exception('No light given');
               } elseif (empty ($id)) {
                  throw new Exception('No id given');
               }
               //$success = $db->editLight ($light);
               throw new Exception('The put has not been implemented yet.');
            } elseif ($this->method == 'DEL') {
               throw new Exception('The del has not been implemented yet.');
            }
            if (!empty($data)) {
               return $data;
            }
         }
      } catch (Exception $ex) {
         throw $ex;
      } finally {
         $db->close ();
      }
      throw new Exception('An unknown error occurred.');
   }

   /**
    * Gets the truth for the given light.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
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
    * Creates a new love.
    */
   protected function createLove ($params)
   {
      if ($this->method == 'GET'){
         throw new Exception('An unknown error occured.');
      }
      $userDb = new UserDb ();
      if (!$userDb->verifyToken ($params['user'], $params['token'])) {
         throw new Exception ('Invalid User.');
      }
      $db = new IinAllDb ();
      $light = $params["light"];
      if (empty ($light)) throw new Exception('No light given');
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
      $db = new IinAllDb ();
      $key = $params["m"];
      $motto = $db->getMotto();
      if ($key != $motto) { throw new Exception('An unknown error occurred.'); }
      $data = $params["d"];
      $log = $db->createTruth ($data);
      $db->close ();
      return Array('status' => 'success', 'log' => $log);
   }
   
   public function format ($params)
   {
      if ($this->method == 'GET') {
         throw new Exception('An unknown error occurred.');
      }
      $userDb = new UserDb ();
      if (!$userDb->verifyToken ($params['user'], $params['token'])) {
         throw new Exception ('Invalid User.');
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
   protected function Suggest ($params)
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