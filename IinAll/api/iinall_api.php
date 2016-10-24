<?php

$dir = dirname (__FILE__) . '/';
require_once ($dir . 'User.php');
require_once ($dir . 'api_base.php');
require_once ($dir . 'IinAllDb.php');
require_once ($dir . 'LoveController.php');
require_once ($dir . 'BodyController.php');
require_once ($dir . 'LightController.php');
require_once ($dir . 'TruthController.php');

class IinAllApi extends Api
{
   public function __construct ($request, $origin)
   {
      parent::__construct ($request);
      if ($this->method !== 'GET' && $this->endpoint !== 'Login' && $this->endpoint !== 'Register') {
         $userDb = new UserDb ();
         try {
            if (!$userDb->verifyToken ($this->params['user'], $this->params['token'])) {
               throw new Exception ('Invalid User.');
            }
         } finally {
            $userDb->disconnect ();
         }
      }
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
    * Light operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Light ($params)
   {
      $data = array();
      $controller = new LightController ();
      try {
         if ($this->method == 'GET') {
            $data = $controller->Get (25);
         } elseif ($this->method == 'POST') {
            $light = $params["l"];
            if (empty ($light)) { throw new Exception ('No light given'); }
            $data['status'] = 'success';
            $data['id'] = $controller->Create ($light);
         } elseif ($this->method == 'PUT') {
            $lightId = $params["text"];
            $id = $params["id"];
            if (empty ($lightId)) {
               throw new Exception ('No light given');
            } elseif (empty ($id)) {
               throw new Exception ('No id given');
            }
            $data['status'] = 'success';
            $controller->Update ($id, $lightId);
         } elseif ($this->method == 'DELETE') {
            throw new Exception ('The delete has not been implemented yet.');
         }
      } catch (Exception $ex) {
         throw $ex;
      } finally {
         $controller->close ();
      }
      if (!empty($data)) {
         return $data;
      }
      throw new Exception('An unknown error occurred.');
   }

   /**
    * Truth operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Truth ($params)
   {
      $data = array();
      $controller = new TruthController ();
      try {
         if ($this->method == 'GET') {
            $light = null;
            if (array_key_exists ("light", $params)) {
               $light = $params["light"];
            }
            $love = null;
            if (array_key_exists ("love", $params)) {
               $love = $params["love"];
            }
            if (empty ($light) && empty ($love)) { throw new Exception ('No data given'); }
            $truths = $controller->Get ($light, $love);
            $data["status"] = "success";
            $data = array_merge($data, $truths);
         } elseif ($this->method == 'POST') {
            $tData = $params["d"];
            if (empty ($tData)) { throw new Exception ('No data given'); }
            $controller->Create ($tData);
            $data["status"] = "success";
         } elseif ($this->method == 'PUT') {
            $tData = $params["d"];
            if (empty ($tData)) { throw new Exception ('No data given'); }
            $controller->Update ($tData);
            $data["status"] = "success";
         } elseif ($this->method == 'DELETE') {
   
         }
      } catch (Exception $ex) {
         throw $ex;
      } finally {
         $controller->close ();
      }
      if (!empty($data)) {
         return $data;
      }
      throw new Exception('An unknown error occurred.');
   }

   /**
    * Love operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Love ($params)
   {
      $data = array();
      $controller = new LoveController ();
      try {
         if ($this->method == 'GET') {
            $peaceIds = $params["p"];
            if (empty ($peaceIds)) { throw new Exception('No peace given'); }
            $love = $controller->Get ($peaceIds);
            $data["status"] = "success";
            $data["love"] = $love;
         } elseif ($this->method == 'POST') {
            $data = $params["d"];
            //$log = $controller->Create ($data);
            $data["status"] = "success";
            $data["log"] = $log;
         } elseif ($this->method == 'PUT') {
   
         } elseif ($this->method == 'DELETE') {
   
         }
      } catch (Exception $ex) {
         throw $ex;
      } finally {
         $controller->close ();
      }
      if (!empty($data)) {
         return $data;
      }
      throw new Exception('An unknown error occurred.');
   }

   /**
    * Body operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Body ($params)
   {
      $data = array();
      $controller = new BodyController ();
      try {
         if ($this->method == 'GET') {
         } elseif ($this->method == 'POST') {
            $position = $params["p"];
            $text = $params["t"];
            $loveId = $params["l"];
            $id = $controller->Create ($position, $text, $loveId);
            $data["status"] = "success";
            $data["id"] = $id;
         } elseif ($this->method == 'PUT') {
   
         } elseif ($this->method == 'DELETE') {
            $id = $params["id"];
            $controller->Delete ($id);
            $data["status"] = "success";
         }
      } catch (Exception $ex) {
         throw $ex;
      } finally {
         $controller->close ();
      }
      if (!empty($data)) {
         return $data;
      }
      throw new Exception('An unknown error occurred.');
   }

   /**
    * Random operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Random ($params)
   {
      $data = array();
      $controller = new LoveController ();
      $love = $controller->Get ("");
      $data['status'] = 'success';
      $data['love'] = $love;
      return $data;
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
      $lightId = $params["light"];
      if (empty ($lightId)) throw new Exception('No light given');
      $id = $db->createLove ($lightId);
      $db->close ();
      return Array('status' => 'success', 'id' => $id);
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
      $lightId = Array();
      $token = $params["t"];
      if (!empty ($token)) {
         $db = new IinAllDb ();
         $lightId = $db->suggest ($token);
         $db->close ();
      }
      return Array('suggestions' => $lightId);
   }
}
?>