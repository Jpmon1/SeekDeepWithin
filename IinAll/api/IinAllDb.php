<?php

require_once (__DIR__."/db.php");

class IinAllDb extends Database
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      $this->connect ();
   }
   
   /**
    * Get suggestions for the given token.
    * @param string $token The token to query for.
    * @return array An array of matching lights.
    */
   function suggest($token)
   {
      $light = array();
      $stmt = $this->prepare ("SELECT id, text FROM light WHERE text LIKE ? LIMIT 25;");
      $search = "%" . $token . "%";
      $stmt->bind_param ("s", $search);
      $stmt->execute ();
      $stmt->bind_result ($id, $text);
      while ($stmt->fetch ()) {
         $light[] = array('data' => $id, 'value' => $text);
      }
      $stmt->free_result ();
      $stmt->close ();
      return $light;
   }
   
   /**
    * Get a random number of lights.
    * @param int $count The number of items to get
    * @return array An array of random lights.
    */
   function getRandomLight($count)
   {
      $max = $this->getMaxLightId ();
      $randIds = array();
      for ($i = 0; $i <= $count; $i++) {
         $randId = mt_rand(1, $max);
         if (!in_array($randId, $randIds)) {
            $randIds[] = $randId;
         }
      }
      $light = array();
      $idString = implode(",", $randIds);
      $stmt = $this->prepare ("SELECT id, text FROM light WHERE id IN ($idString);");
      $stmt->execute ();
      $stmt->bind_result ($id, $text);
      while ($stmt->fetch ()) {
         $light[] = array('id' => $id, 'text' => $text);
      }
      $stmt->free_result ();
      $stmt->close ();
      return $light;
   }
   
   /**
    * Gets the truth for the given lights.
    * @param $light string A comma separated list of light ids.
    * @return array The array of requested truth.
    */
   function getTruth($light)
   {
      $loveId = $this->getLoveId ($light);
      if ($loveId === -1) {
         $truths = array();
         $stmt = $this->prepare ("SELECT `truth`.`id`, `truth`.`order`, `truth`.`number`, `truth`.`light_id`, `light`.`text` 
                                  FROM `truth` 
                                  INNER JOIN `light` ON `truth`.`light_id`=`light`.`id` 
                                  WHERE `love_id`=?;");
         $stmt->bind_param ("i", $loveId);
         $stmt->execute ();
         $stmt->bind_result ($id, $order, $number, $lightId, $text);
         while ($stmt->fetch ()) {
            $truths[$id] = array ("o" => $order, "n" => $number, "t" => $text, "lid" => $lightId);
         }
         $stmt->free_result ();
         $stmt->close ();
         return $truths;
      }
      return Array ();
   }
   
   /**
    * Adds the given light to the database.
    * @param string $light The light to add.
    * @return int The ID of the added light.
    */
   function createLight ($light)
   {
      $light = trim ($light);
      $id = $this->getLightId ($light);
      if ($id === -1) {
         $stmt = $this->prepare ("INSERT INTO light (text) VALUES (?);");
         $stmt->bind_param ("s", $light);
         $stmt->execute ();
         $id = $this->getLastInsertId ();
         $stmt->close ();
      }
      return $id;
   }

   /**
    * Creates a new love based on the given light.
    * @param $light string The list of light for the love.
    * @return int The ID of the love.
    */
   function createLove ($light)
   {
      $light = trim ($light);
      $id = $this->getLoveId ($light);
      if ($id === -1) {
         $stmt = $this->prepare ("INSERT INTO love (light) VALUES (?);");
         $stmt->bind_param ("s", $light);
         $stmt->execute ();
         $id = $this->getLastInsertId ();
         $stmt->close ();
         
         $lightIds = explode(",", $light);
         $idCount = count ($lightIds);
         for($i = 0; $i < $idCount; $i++){
            $lightId = $lightIds[$i];
            $stmtP = $this->prepare ("INSERT INTO `iinallco_data`.`peace` (`love_id`, `light_id`, `order`) VALUES (?, ?, ?)");
            $stmtP->bind_param ("iii", $id, $lightId, $i);
            $stmtP->execute ();
            $stmtP->close ();
         }
      }
      return $id;
   }
   
   /**
    * Creates truths for the list of json data given.
    * @param string $jsonData The string representation of the data to create.
    * @return array An array with a log of what happened.
    */
   function createTruth ($jsonData)
   {
      $log = Array();
      $decodedData = json_decode($jsonData);
      $count = count ($decodedData);
      $log[] = array("Found $count loves");
      $query = "INSERT INTO `iinallco_data`.`truth` (`love_id`, `light_id`, `order`, `number`) VALUES ";
      for ($i = 0; $i < $count; $i++) {
         $data = $decodedData [$i];
         $loveId = $this->createLove ($data->love);
         $log[] = array("Got love $loveId");
         $itemCount = count ($data->items);
         for ($j = 0; $j < $itemCount; $j++) {
            $item = $data->items[$j];
            $text = $item->t;
            $order = $item->o;
            $log[] = array("Adding ($order) $text");
            $lightId = $this->createLight ($text);
            if ($j > 0) {
               $query .= ",";
            }
            if (array_key_exists ("n", $item)) {
               $number = $item->n;
               $query .= "('".$loveId."','" .$lightId."','" .$order."','" .$number."')";
            } else {
               $query .= "('".$loveId."','" .$lightId."','" .$order."',NULL)";
            }
         }
      }
      $log[] = array("Query: $query");
      $result = $this->query ($query);
      $log[] = array("Query Result: $result");
      $error = $this->lastError ();
      $log[] = array("Last Error: $error");
      return $log;
   }
   
   /**
    * Formats the given data into I in All truth.
    * @param string $love The love of the data to format.
    * @param string $data The data to format.
    * @param string $regex The regular express to use to do the format.
    * @param int $startOrder The starting order, null if starting from 1.
    * @param int $startNumber The starting number, null if numbers are not used.
    * @return string The formatted data.
    */
   function format ($love, $data, $regex, $startOrder, $startNumber)
   {
      $items = array();
      $this->addRegex ($regex);
      $order = $startOrder == null ? 1 : intval ($startOrder);
      $number = $startNumber == null ? null : intval ($startNumber );
      $count = preg_match_all ($regex, $data, $matches, PREG_OFFSET_CAPTURE);
      for ($i = 0; $i < $count; $i++) {
         $o = $order;
         $n = $number;
         if (array_key_exists("o", $matches)){
            $o = intval (trim($matches["o"][$i][0]));
            $order = $o;
         }
         if (array_key_exists("n", $matches)){
            $n = intval (trim($matches["n"][$i][0]));
            $number = $n;
         }
         $t = trim($matches["t"][$i][0]);
         $item = array ("t" => $t, "o" => $o);
         if ($n != null){
            $item["n"] = $n;
         }
         $items[] = $item;
         $order++;
         if ($number != null) { $number++; }
      }
      return Array ("love" => $love, "items" => $items);
   }
   
   /**
    * Gets the max ID for the light table.
    * @return the max available ID in the light table.
    */
   function getMaxLightId ()
   {
      $stmt = $this->prepare ("SELECT MAX(id) FROM light;");
      $stmt->execute ();
      $stmt->bind_result ($maxId);
      if ($stmt->fetch ()) {
         $max = $maxId;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $max;
   }
   
   /**
    * Gets the ID of the given light.
    * @param string $light The light to get the ID for.
    * @return int The ID of the requested light, -1 if not found.
    */
   function getLightId ($light)
   {
      $id = -1;
      $stmt = $this->prepare ("SELECT id FROM light WHERE text=?;");
      $stmt->bind_param ("s", $light);
      $stmt->execute ();
      $stmt->bind_result ($lightId);
      if ($stmt->fetch ()) {
         $id = $lightId;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $id;
   }
   
   /**
    * Gets the ID of the given love.
    * @param string $light The light IDs to get the ID for.
    * @return int The ID of the requested love, -1 if not found.
    */
   function getLoveId ($light)
   {
      $id = -1;
      $stmt = $this->prepare ("SELECT id FROM love WHERE light=?;");
      $stmt->bind_param ("s", $light);
      $stmt->execute ();
      $stmt->bind_result ($loveId);
      if ($stmt->fetch ()) {
         $id = $loveId;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $id;
   }
   
   /**
    * Adds the given regular expression to the database if not found.
    * @param string $regex The regular expression to add to the database.
    */
   function addRegex ($regex)
   {
      $stmt = $this->prepare ("SELECT id FROM edit_regex WHERE regex=?;");
      $stmt->bind_param ("s", $regex);
      $stmt->execute ();
      $stmt->bind_result ($regexId);
      if (!$stmt->fetch ()) {
         $stmtR = $this->prepare ("INSERT INTO edit_regex (regex) VALUES (?);");
         $stmtR->bind_param ("s", $regex);
         $stmtR->execute ();
         $stmtR->close ();
      }
      $stmt->free_result ();
      $stmt->close ();
   }
   
   /**
    * Gets a list of all the regular expressions in the database.
    * @return array The list of regular expressions in the database.
    */
   function getAllRegex ()
   {
      $regexes= array();
      $stmt = $this->prepare ("SELECT id, regex FROM edit_regex;");
      $stmt->execute ();
      $stmt->bind_result ($id, $regex);
      while ($stmt->fetch ()) {
         $regexes[$id] = $regex;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $regexes;
   }

   /**
    * Gets the motto.
    * @return string The motto.
    */
   function getMotto ()
   {
      $stmt = $this->prepare ("SELECT `data`.`value` FROM `data` WHERE `key`='motto';");
      $stmt->execute ();
      $stmt->bind_result ($motto);
      $stmt->fetch ();
      $stmt->free_result ();
      $stmt->close ();
      return $motto;
   }
   
   /**
    * Closes the underlying connection.
    */
   function close ()
   {
      $this->disconnect ();
   }
}
?>