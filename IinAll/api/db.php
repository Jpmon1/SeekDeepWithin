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