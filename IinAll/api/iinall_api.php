<?php

$dir = dirname (__FILE__) . '/';
require_once ($dir . 'User.php');
require_once ($dir . 'api_base.php');
require_once ($dir . 'IinAllDb.php');
require_once ($dir . 'LoveController.php');
require_once ($dir . 'BodyController.php');
require_once ($dir . 'StyleController.php');
require_once ($dir . 'AliasController.php');
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
            // TODO:... (suggest)
         } elseif ($this->method == 'POST') {
            $light = $params["l"];
            if (empty ($light)) { throw new Exception ('No light given'); }
            $data['status'] = 'success';
            $data['id'] = $controller->Create ($light);
         } elseif ($this->method == 'PUT') {
            $text = $params["text"];
            $id = intval ($params["id"]);
            if (empty ($text)) { throw new Exception ('No light given'); }
            if (empty ($id)) { throw new Exception ('No id given'); }
            $data['status'] = 'success';
            $controller->Update ($id, $text);
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
            $format = array_key_exists ("f", $params);
            if (empty ($light) && empty ($love)) { throw new Exception ('No data given'); }
            $truths = $controller->Get ($light, $love, $format);
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
            $love = null;
            $format = array_key_exists ("f", $params);
            $token = array_key_exists ("t", $params) ? $params["t"] : null;
            $count = array_key_exists ("c", $params) ? $params["c"] : 25;
            $peace = array_key_exists ("p", $params) ? $params["p"] : null;
            if (!empty ($peace)) {
               $love = $controller->Get ($peace);
            } elseif (!empty ($token)) {
               $love = $controller->Search ($token, $format);
            } else {
               $love = $controller->Random ($count, $format);
            }
            $data['status'] = 'success';
            $data['love'] = $love;
         } elseif ($this->method == 'POST') {
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
            $loveId = $params["l"];
            if (empty ($loveId)) { throw new Exception('No love given'); }
            $body = $controller->Get ($loveId);
            $data["status"] = "success";
            $data["body"] = $body;
         } elseif ($this->method == 'POST') {
            $text = $params["t"];
            $loveId = intval ($params["l"]);
            $position = intval ($params["p"]);
            if (empty ($position)) { throw new Exception('No position given'); }
            if (empty ($text)) { throw new Exception('No text given'); }
            if (empty ($loveId)) { throw new Exception('No love given'); }
            $id = $controller->Create ($position, $text, $loveId);
            $data["status"] = "success";
            $data["id"] = $id;
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
    * Style operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Style ($params)
   {
      $data = array();
      $controller = new StyleController ();
      try {
         if ($this->method == 'GET') {
            $loveId = $params["l"];
            if (empty ($loveId)) { throw new Exception('No love given'); }
            $style = $controller->Get ($loveId);
            $data["status"] = "success";
            $data["style"] = $style;
         } elseif ($this->method == 'POST') {
            $css = $params["c"];
            $tag = $params["t"];
            $endIndex = intval ($params["e"]);
            $startIndex = intval ($params["s"]);
            $loveId = intval ($params["l"]);
            if (empty ($tag)) { throw new Exception('No tag given'); }
            if (empty ($loveId)) { throw new Exception('No love given'); }
            if ($startIndex < 0) { throw new Exception('No start index given'); }
            if ($endIndex <= $startIndex) { throw new Exception('End index cannot be less than start index.'); }
            $id = $controller->Create ($loveId, $tag, $startIndex, $endIndex, $css);
            $data["status"] = "success";
            $data["id"] = $id;
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
    * Alias operations.
    * @param $params array The passed in parameters.
    * @return array The results.
    * @throws Exception Error when unknown action is requested.
    */
   protected function Alias ($params)
   {
      $data = array();
      $controller = new AliasController ();
      try {
         if ($this->method == 'GET') {
            $loveId = intval ($params["l"]);
            if (empty ($loveId)) { throw new Exception('No love given'); }
            $alias = $controller->Get ($loveId);
            $data["status"] = "success";
            $data["alias"] = $alias;
         } elseif ($this->method == 'POST') {
            $loveId = intval ($params["l"]);
            $aliasId = intval ($params["a"]);
            if (empty ($loveId)) { throw new Exception('No love given'); }
            if (empty ($aliasId)) { throw new Exception('No alias given'); }
            $id = $controller->Create ($loveId, $aliasId);
            $data["status"] = "success";
         } elseif ($this->method == 'DELETE') {
            $loveId = $params["l"];
            if (empty ($loveId)) { throw new Exception('No love given'); }
            $controller->Delete ($loveId);
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