<?php
$incDir = dirname(__FILE__) . '/';
require_once ($incDir."Constants.php");

class UserDb
{

   private $m_Nonce;
   private $m_Db = null;

   const DB_SERVER = "localhost";
   const DB_USER = "iinallco_user";
   const DB_PASSWORD = "I.tA32)3f1Ca";
   const DB_MAIN_DB = "iinallco_data";
   const DB_USER_DB = "iinallco_user";

   /**
    * Connects to the database.
    */
   function __construct ()
   {
      $this->m_Db = new mysqli (self::DB_SERVER, self::DB_USER, self::DB_PASSWORD, self::DB_USER_DB);
      if ($this->m_Db->connect_errno) {
         printf ("Connect failed: %s\n", mysqli_connect_error ());
         exit ();
      }
      $this->m_Db->set_charset ("utf8");
   }

   /**
    * Disconnects from the database.
    */
   public function disconnect ()
   {
      $this->m_Db->close ();
   }

   /**
    * Gets the user's nonce.
    * @return string The user's nonce.
    */
   public function getNonce ()
   {
      return $this->m_Nonce;
   }

   /**
    * Log's a user into IinAll.
    * @param string $email The user's email.
    * @param string $password The user's password.
    * @return string The login result (OK if everything went well).
    */
   public function login ($email, $password)
   {
      if (!$this->isEmailInDb ($email)) {
         return "The user or password is incorrect.";
      }
      $statement = $this->m_Db->prepare ("SELECT id, name, password, salt FROM users WHERE email=?");
      $statement->bind_param ("s", $email);
      $statement->execute ();
      $statement->bind_result ($userId, $userName, $userPassword, $salt);
      $userFound = $statement->fetch ();
      $statement->free_result ();
      $statement->close ();
      if ($userFound) {
         if ($this->checkBrute ($userId)) {
            return "Too many failed login attempts for $email.";
         }
         $password = hash ("sha256", $password . $salt);
         if ($userPassword === $password) {
            // Create a nonce...
            $nonce = $this->createNonce ();
            $statement = $this->m_Db->prepare ("UPDATE users SET nonce=? where id=?");
            $statement->bind_param ("si", $nonce, $userId);
            $statement->execute ();
            $statement->close ();

            $this->m_Nonce = $nonce;
            $this->setSessionData ($userName, $email, $nonce, $salt);
            $this->checkEditor ($userId, $nonce);
            return true;
         } else {
            $now = time ();
            $statement = $this->m_Db->prepare ("INSERT INTO login_attempts (id, time) VALUES (?, ?);");
            $statement->bind_param ("ii", $userId, $now);
            $statement->execute ();
            $statement->close ();
            return "The user or password is incorrect.";
         }
      }
      return "The user or password is incorrect.";
   }

   /**
    * Checks if the given user id is an editor, and if so, get the access level.
    * @param int $userId The id of the user to check.
    */
   private function checkEditor ($userId, $nonce)
   {
      $statement = $this->m_Db->prepare ("SELECT level FROM editors WHERE id=?");
      $statement->bind_param ("i", $userId);
      $statement->execute ();
      $statement->bind_result ($editLevel);
      $isEditor = $statement->fetch ();
      $statement->free_result ();
      $statement->close ();

      if ($isEditor) {
         $privKey = "";
         $openSsl = openssl_pkey_new ();
         openssl_pkey_export ($openSsl, $privKey);
         $keyDetails = openssl_pkey_get_details ($openSsl);
         $pubKey = $keyDetails ["key"];

         $motto = IINALL_MOTTO;
         $statement = $this->m_Db->prepare ("UPDATE editors SET edit_key=AES_ENCRYPT(?, ?) WHERE id=?;");
         $statement->bind_param ("ssi", $privKey, $motto, $userId);
         $statement->execute ();
         $statement->close ();
         $crypted = "";
         openssl_public_encrypt ($nonce, $crypted, $pubKey);
         $_SESSION ['iinAlluser'] ['level'] = $editLevel;
         $_SESSION ['iinAlluser'] ['token'] = base64_encode ($crypted);
         $_SESSION ['iinAlluser'] ['id'] = $userId;
      }
   }

   /**
    * Log's a user into iinall with an open id.
    * @param string $nonce The nonce of the open id login.
    * @param string $email The user's email.
    * @param string $name The user's name.
    * @param string $locale The user's language setting.
    * @return string The login result (OK if everything went well).
    */
   public function loginOpenId ($nonce, $email, $name, $locale)
   {
      $statement = $this->m_Db->prepare ("SELECT openid_id, salt FROM users_openid WHERE email=?");
      $statement->bind_param ("s", $email);
      $statement->execute ();
      $statement->bind_result ($userId, $salt);
      $userFound = $statement->fetch ();
      $statement->free_result ();
      $statement->close ();

      if ($userFound) {
         $statement = $this->m_Db->prepare ("UPDATE users_openid SET nonce=? where openid_id=?");
         $statement->bind_param ("si", $nonce, $userId);
         $statement->execute ();
         $statement->close ();
      } else {
         $salt = dechex (mt_rand (0, 2147483647)) . dechex (mt_rand (0,
                                 2147483647));
         $statement = $this->m_Db->prepare ("INSERT INTO users_openid (name, email, nonce, local, salt) VALUES (?,?,?,?,?)");
         $statement->bind_param ("ssss", $name, $email, $nonce, $locale, $salt);
         $statement->execute ();
         $statement->close ();
      }

      $this->setSessionData ($name, $email, $nonce, $salt);
   }

   /**
    * Creates a new nonce.
    * @return string a New nonce.
    */
   private function createNonce ()
   {
      return dechex (mt_rand (0, 2147483647)) . dechex (mt_rand (0, 2147483647)) . time ();
   }

   /**
    * Check to see if a brute login attack is happening.
    * @param int $id ID of the user to check.
    * @return boolean True if a brute attack is sensed.
    */
   private function checkBrute ($id)
   {
      // Get timestamp of current time
      $now = time ();
      // All login attempts are counted from the past 2 hours.
      $valid_attempts = $now - (2 * 60 * 60);

      $statement = $this->m_Db->prepare ("SELECT time FROM login_attempts WHERE id = ? AND time > '$valid_attempts'");
      $statement->bind_param ("i", $id);
      $statement->execute ();
      $statement->store_result ();
      $isBrute = ($statement->num_rows > 5);
      $statement->free_result ();
      $statement->close ();
      return $isBrute;
   }

   /**
    * Sets user session data and cookies.
    * @param string $userName The name of the user.
    * @param string $userEmail The email address of the user.
    * @param string $nonce The user's nonce.
    * @param string $salt The user' salt.
    */
   private function setSessionData ($userName, $userEmail, $nonce, $salt)
   {
      $_SESSION ['iinAlluser'] ['name'] = $userName;
      $_SESSION ['iinAlluser'] ['email'] = $userEmail;

      $nonce = hash ("sha256", $nonce . $salt);
      $expire = time () + 60 * 60 * 24 * 14; // two weeks...
      setcookie ("iinAllNonce", $nonce, $expire, "/", ".iinall.com");
      setcookie ("iinAllEmail", $userEmail, $expire, "/", ".iinall.com");
   }

   /**
    * Regiserts a user with the given informaiton.
    * @param string $name The user's name.
    * @param string $email The user's email.
    * @param string $password The user's password.
    * @return string The outcome of the register process.
    */
   public function register ($name, $email, $password)
   {
      if ($this->isEmailInDb ($email)) {
         return "A user with the same email is already registered.";
      }
      $nonce = $this->createNonce ();
      $statement = $this->m_Db->prepare ("INSERT INTO `users` (name, email, password, salt, nonce) VALUES (?,?,?,?,?)");
      if ($statement === false) {
         var_dump($this->m_Db->error);
      }
      $salt = dechex (mt_rand (0, 2147483647)) . dechex (mt_rand (0, 2147483647));
      $password = hash ("sha256", $password . $salt);
      $statement->bind_param('sssss', $name, $email, $password, $salt, $nonce);
      $statement->execute ();
      $statement->close ();

      $this->setSessionData ($name, $email, $nonce, $salt);

      return true;
   }

   /**
    * Checks to see if the given email is in the database.
    * @param string $email The email to check.
    * @return boolean True if the given email is in the database, otherwise false.
    */
   private function isEmailInDb ($email)
   {
      $statement = $this->m_Db->prepare ("SELECT 1 FROM users WHERE email=?");
      $statement->bind_param ("s", $email);
      $statement->execute ();
      $userExists = $statement->fetch ();
      $statement->free_result ();
      $statement->close ();
      return $userExists;
   }

   /**
    * Gets the currently known nonce for the given user.
    * @param int $userId The user ID to get the nonce for.
    * @return string The current nonce of the user, 'unknown' if non-existance.
    */
   private function getCurrentNonce ($userId)
   {
      $statement = $this->m_Db->prepare ("SELECT nonce FROM users WHERE id=?");
      $statement->bind_param ("i", $userId);
      $statement->execute ();
      $statement->bind_result ($nonce);
      if (!$statement->fetch ()) {
         $nonce = "unknown";
      }
      $statement->free_result ();
      $statement->close ();
      return $nonce;
   }

   /**
    * Gets the current private key for the given editor.
    * @param int $userId The user ID to get the private key for.
    * @return string The current private key of the user, 'unknown' if non-existance.
    */
   private function getPrivateKey ($userId)
   {
      $motto = IINALL_MOTTO;
      $statement = $this->m_Db->prepare ("SELECT AES_DECRYPT(edit_key, ?) FROM editors WHERE id=?");
      $statement->bind_param ("si", $motto, $userId);
      $statement->execute ();
      $statement->bind_result ($privKey);
      if (!$statement->fetch ()) {
         $privKey = "unknown";
      }
      $statement->free_result ();
      $statement->close ();
      return $privKey;
   }

   /**
    * Verifies the edit token.
    * @param int $userId The ID of the user to verify the token for.
    * @param string $token The edit token to verify.
    * @return boolean True if the token is verified, otherwise false.
    */
   public function verifyToken ($userId, $token)
   {
      $nonce = $this->getCurrentNonce ($userId);
      if ($nonce === "unknown") return false;
      $privKey = $this->getPrivateKey ($userId);
      if ($privKey === "unknown") return false;

      $decrypted = "";
      $encryptedData = base64_decode ($token);
      openssl_private_decrypt ($encryptedData, $decrypted, $privKey);
      if ($nonce === $decrypted) return true;

      return false;
   }

   /**
    * Verifies that the given information is for a known user.
    * @param string $userEmail The email address of the user.
    * @param string $nonce The user's nonce.
    * @return boolean True if user is verified, false if not verified.
    */
   public function verifyUser ($userEmail, $nonce)
   {
      $statement = $this->m_Db->prepare ("SELECT name, id, nonce, salt FROM users WHERE email=?;");
      $statement->bind_param ("s", $userEmail);
      $statement->execute ();
      $statement->bind_result ($userName, $userId, $dbNonce, $salt);
      $verified = $statement->fetch ();
      $statement->free_result ();
      $statement->close ();
      if ($verified) {
         $hashNonce = hash ("sha256", $dbNonce . $salt);
         if ($hashNonce === $nonce) {
            $this->m_Nonce = $nonce;
            $this->setSessionData ($userName, $userEmail, $dbNonce, $salt);
            $this->checkEditor ($userId, $dbNonce);
         } else {
            $verified = false;
         }
      } else {
         // Check for open id login...
         $statement = $this->m_Db->prepare ("SELECT name, nonce, salt FROM users_openid WHERE email=?;");
         $statement->bind_param ("s", $userEmail);
         $statement->execute ();
         $statement->bind_result ($userName, $dbNonce, $salt);
         if ($statement->fetch ()) {
            $hashNonce = hash ("sha256", $dbNonce . $salt);
            if ($hashNonce === $nonce) {
               $verified = true;
               $this->setSessionData ($userName, $userEmail, $dbNonce, $salt);
            } else {
               $verified = false;
            }
         }
         $statement->free_result ();
         $statement->close ();
      }
      return $verified;
   }

   /**
    * Logs an active guest for the system.
    * @param string $ip The IP address of the guest.
    * @param int $time The time the guest was here.
    */
   public function addActiveGuest ($ip, $time)
   {
      $this->m_Db->query ("REPLACE INTO active_guests (ip, timestamp) VALUES ('$ip', '$time');");
   }

   /**
    * Logs an active user for the system.
    * @param string $ip The Email address of the user.
    * @param int $time The time the user was here.
    */
   public function addActiveUser ($email, $time)
   {
      $this->m_Db->query ("REPLACE INTO active_users (email, timestamp) VALUES ('$email', '$time');");
   }

   /**
    * Removes inactive users and guests from the active tables.
    */
   public function removeInactive ()
   {
      // 30 minute timeout.
      $timeout = time () - 30 * 60;
      $this->m_Db->query ("DELETE FROM active_users WHERE timestamp < $timeout;");
      $this->m_Db->query ("DELETE FROM active_guests WHERE timestamp < $timeout;");
   }

}
