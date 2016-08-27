<?php

/**
 * Database connection utitlity class.
 */
class Database
{

   const DB_SERVER = "localhost";
   const DB_USER = "iinallco_user";
   const DB_PASSWORD = "I.tA32)3f1Ca";
   const DB_MAIN_DB = "iinallco_data";
   const DB_USER_DB = "iinallco_user";

   private $m_Db = null;
   private $m_DbType;
   private $m_IsConnected = false;

   /**
    * Connects to the database.
    */
   public function connect ($database = DatabaseType::Main)
   {
      $this->m_DbType = $database;
      $this->m_Db = new mysqli (self::DB_SERVER, self::DB_USER,
              self::DB_PASSWORD,
              ($database == DatabaseType::Main) ? self::DB_MAIN_DB : self::DB_USER_DB);
      if ($this->m_Db->connect_errno > 0) {
         printf ("Connect failed: %s\n", mysqli_connect_error ());
         exit ();
      }
      $this->m_Db->set_charset ("utf8");
      $this->m_IsConnected = true;
   }

   /**
    * Gets the current database in use.
    * @return int The type of database to use, compare with DatabaseType class, -1 if not connected.
    */
   public function getDatabase ()
   {
      if ($this->m_IsConnected == false) return -1;
      return $this->m_DbType;
   }

   /**
    * Runs the given query against the database.
    * @param string $query Query statment to run.
    * @return mysqli_result Query results.
    */
   public function query ($query)
   {
      if ($this->m_IsConnected == false) $this->connect ();
      if ($this->m_IsConnected == false) return null;
      return $this->m_Db->query ($query);
   }
   
   /**
    * Begins a sql transaction.
    */
   public function beginTransaction ()
   {
      $this->m_Db->autocommit (FALSE);
   }
   
   /**
    * Commits a sql transaction.
    */
   public function endTransaction ()
   {
      $this->m_Db->commit ();
   }

   /**
    * Prepares the given statement.
    * @param string $statement Statement to prepare.
    * @return mysqli_stmt Prepaird statement.
    */
   public function prepare ($statement)
   {
      if ($this->m_IsConnected == false) $this->connect ();
      if ($this->m_IsConnected == false) return null;
      return $this->m_Db->prepare ($statement);
   }

   /**
    * Returns the last error message for the most recent MySQLi function call that can succeed or fail.
    * @return string A string that describes the error. An empty string if no error occurred.
    */
   public function lastError ()
   {
      return $this->m_Db->error;
   }

   /**
    * Creates an insert statement with the array data.
    * @param array $data Array of data, using the keys as the column names.
    * @param string $table the name of the table to insert data into.
    * @return string The generated INSERT statement.
    */
   public function createInsert (&$data, $table)
   {
      $cols = implode (',', array_keys ($data));
      foreach (array_values ($data) as $value) {
         isset ($vals) ? $vals .= ',' : $vals = '';
         $vals .= '\'' . $this->mysql->real_escape_string ($value) . '\'';
      }
      return 'INSERT INTO ' . $table . ' (' . $cols . ') VALUES (' . $vals . ')';
   }

   /**
    * Get the auto increment of the last insert.
    * @return int The auto increment id of the last insert, -1 if not connected.
    */
   public function getLastInsertId ()
   {
      if ($this->m_IsConnected == false) return -1;
      return $this->m_Db->insert_id;
   }

   /**
    * Verifies that the given user is known by the system.
    * @param string $email User's email.
    * @return boolean True if user is verified, otherwise false.
    */
   public function verifyUser ($email)
   {
      if (!$this->m_IsConnected) $this->connect (DatabaseType::User);
      if (!$this->m_IsConnected) return false;
      if (!isset ($_COOKIE ['sdwNonce'])) return false;
      $nonce = $_COOKIE ['sdwNonce'];
      $dbNonce = $this->getNonce ($email);

      if (isset ($dbNonce) && isset ($nonce)) {
         if ($nonce == $dbNonce) {
            $this->requestKey ($email);
            return true;
         }
      }
      return false;
   }

   /**
    * Gets the nonce stored in the database for the given user.
    * @param string $email The email of the user to get nonce for.
    * @return string Nonce storded in database.
    */
   private function getNonce ($email)
   {
      $dbNonce = "";
      $motto = getenv ('SEEKDEEPWITHIN_MOTTO');
      $query = "SELECT AES_DECRYPT(nonce, ?) FROM users WHERE email=?;";
      $statement = $this->prepare ($query);
      $statement->bind_param ("ss", $motto, $email);
      $statement->execute ();
      $statement->bind_result ($dbNonce);
      $statement->fetch ();
      $statement->free_result ();
      $statement->close ();
      return $dbNonce;
   }

   /**
    * Verifies the given user and requests an edit key.
    * If the user is verified, and able to edit, a key will be placed in a cookie.
    * @param string $email User's email.
    */
   private function requestKey ($email)
   {
      unset ($_SESSION ['sdwPubKey']);
      unset ($_SESSION ['sdwCanEdit']);
      $privKey = "";
      $res = openssl_pkey_new ();
      openssl_pkey_export ($res, $privKey);
      $keyDetails = openssl_pkey_get_details ($res);
      $pubKey = $keyDetails ["key"];
      $motto = getenv ('SEEKDEEPWITHIN_MOTTO');

      $query = "UPDATE editors SET privatekey=AES_ENCRYPT(?, ?) WHERE editor=?;";
      $statement = $this->prepare ($query);
      $statement->bind_param ("sss", $privKey, $motto, $email);
      $statement->execute ();
      if ($this->m_Db->affected_rows == 1) {
         $_SESSION ['sdwPubKey'] = $pubKey;
         $_SESSION ['sdwCanEdit'] = true;
      }
      $statement->close ();
   }

   /**
    * Login the given user in.
    * @param string $nonce The nonce of the user.
    * @param string $email The email of the user.
    * @param string $name The name of the user.
    * @param string $locale The locale of the user.
    */
   public function login ($nonce, $email, $name, $locale)
   {
      $motto = getenv ('SEEKDEEPWITHIN_MOTTO');
      $email = urldecode ($email);
      $_SESSION ['sdwUserEmail'] = $email;

      $expire = time () + 60 * 60 * 24 * 14; // two weeks...
      setcookie ("sdwUserEmail", $email, $expire, "/", ".seekdeepwithin.com");
      setcookie ("sdwNonce", $nonce, $expire, "/", ".seekdeepwithin.com");

      $statement = $this->prepare ("SELECT id, email, AES_DECRYPT(nonce, ?) FROM users WHERE email=?;");
      $statement->bind_param ("ss", $motto, $email);
      $statement->execute ();
      if ($statement->fetch ()) {
         $statement->free_result ();
         $statement->close ();
         // Update the user's nonce...
         $statement = $this->prepare ("UPDATE users SET nonce=AES_ENCRYPT(?, ?) WHERE email=?;");
         $statement->bind_param ("sss", $nonce, $motto, $email);
         $statement->execute ();
         $statement->close ();
      } else {
         $statement->free_result ();
         $statement->close ();
         // This is a new user...
         $statement = $this->prepare ("INSERT into users (email, nonce, name, locale) VALUES (?, AES_ENCRYPT(?, ?), ?, ?);");
         $statement->bind_param ("sssss", $email, $nonce, $motto, $name, $locale);
         $statement->execute ();
         $statement->close ();
      }
      $this->requestKey ($email);
   }

   /**
    * Verifies the given user's edit token.
    * @param string $email The email of the user to verify.
    * @param string $editToken The edit token to verify.
    * @return string The status of the verification.
    */
   public function verifyToken ($email, $editToken)
   {
      if (!$this->m_IsConnected) $this->connect (DatabaseType::User);
      if (!$this->m_IsConnected) return "Unable to connect to database.";

      $privKey = "";
      $motto = getenv ('SEEKDEEPWITHIN_MOTTO');
      $statement = $this->prepare ("SELECT AES_DECRYPT(privatekey, ?) FROM editors WHERE editor=?;");
      $statement->bind_param ("ss", $motto, $email);
      $statement->execute ();
      $statement->bind_result ($privKey);
      $statement->fetch ();
      $statement->free_result ();
      $statement->close ();

      if ($privKey == "") return "No private key found for user.";

      $nonce = "";
      $nonceStatement = $this->prepare ("SELECT AES_DECRYPT(nonce, ?) FROM users WHERE email=?;");
      $nonceStatement->bind_param ("ss", $motto, $email);
      $nonceStatement->execute ();
      $nonceStatement->bind_result ($nonce);
      $nonceStatement->fetch ();
      $nonceStatement->free_result ();
      $nonceStatement->close ();

      if ($nonce == "") return "Unable to attain user's identity.";

      $decrypted = "";
      // Decode public key...
      $encryptedData = base64_decode ($editToken);
      // Decrypt...
      openssl_private_decrypt ($encryptedData, $decrypted, $privKey);
      //Compare...
      if ($nonce == $decrypted) return "Verified";

      return "Unable to verify edit token.";
   }

   /**
    * Logs the given action to the log.
    * @param string $email The user that performed the action.
    * @param string $action The action that took place.
    * @return string The error message, or OK.
    */
   public function log ($email, $action)
   {
      if (!$this->m_IsConnected) $this->connect (DatabaseType::User);
      if (!$this->m_IsConnected) return "Unable to connect to database.";
      if ($this->m_DbType == DatabaseType::Main) return "Connected to the wrong database!";
      $statement = $this->prepare ("INSERT INTO log (user, action, date) VALUES (?, ?, NOW());");
      $statement->bind_param ("ss", $email, $action);
      $statement->execute ();
      $statement->close ();
      return "OK";
   }

   /**
    * Disconnects from the database.
    */
   public function disconnect ()
   {
      if ($this->m_IsConnected == true) $this->m_Db->close ();
   }

}

/**
 * The type of database to connect to.
 */
final class DatabaseType
{
   /**
    * Main database.
    */

   const Main = 0;

   /**
    * User database.
    */
   const User = 1;

}

?>