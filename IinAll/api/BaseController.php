<?php

require_once (__DIR__."/db.php");

class BaseController extends Database
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      $this->connect ();
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
    * Gets the max ID for the love table.
    * @return the max available ID in the love table.
    */
   function getMaxLoveId ()
   {
      $stmt = $this->prepare ("SELECT MAX(id) FROM love;");
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
   function createLoveText ($light)
   {
      $light = trim ($light);
      $lights = explode ("|", $light);
      $lightCount = count ($lights);
      $lightIds = Array();
      for ($i = 0; $i < $lightCount; $i++){
         $lightIds[] = $this->createLight ($lights[$i]);
      }
      $light = implode (",", $lightIds);
      $id = $this->getLoveId ($light);
      if ($id === -1) {
         $stmt = $this->prepare ("INSERT INTO love (light) VALUES (?);");
         $stmt->bind_param ("s", $light);
         $stmt->execute ();
         $id = $this->getLastInsertId ();
         $stmt->close ();
         $this->createPeace ($id, $lightIds);
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
         $lightIds = explode (",", $light);
         $this->createPeace ($id, $lightIds);
      }
      return $id;
   }

   /**
    * Creates a new peace based on the given love and light.
    * @param $loveId int The ID of the love.
    * @param $lightIds array The list of light IDs for the love.
    */
   function createPeace ($loveId, $lightIds)
   {
      $query = "INSERT INTO `peace` (`love_id`, `light_id`, `sequence`) VALUES ";
      $count = count ($lightIds);
      for ($i = 0; $i < $count; $i++) {
         if ($i > 0) {
            $query .= ",";
         }
         $value = $lightIds[$i];
         $query .= "('$loveId', '$value', '$i')";
      }
      $this->query ($query);
   }

   /**
    * Gets the list of lights from the given list of light ids.
    * @param $lightIds array The list of light IDs for the love.
    * @return array The array of light.
    */
   function getLights ($lightIds)
   {
      $lights = Array ();
      $data = implode(",", $lightIds);
      $stmtL = $this->prepare ("SELECT `light`.`id`, `light`.`text` 
                                FROM `light`
                                WHERE `light`.`id` IN ($data);");
      $stmtL->execute ();
      $stmtL->bind_result ($id, $text);
      while ($stmtL->fetch ()) {
         $lights[$id] = $text;
      }
      $stmtL->free_result ();
      $stmtL->close ();
      return $lights;
   }

   function arrayCopy ($array) {
      $result = array();
      foreach( $array as $key => $val ) {
         if( is_array( $val ) ) {
            $result[$key] = arrayCopy( $val );
         } elseif ( is_object( $val ) ) {
            $result[$key] = clone $val;
         } else {
            $result[$key] = $val;
         }
      }
      return $result;
   }

   /**
    * Gets the alias for the given love.
    * @param $loveIds string A list of love ids.
    * @return array The array of requested aliases.
    */
   function GetAlias ($loveIds)
   {
      $alias = Array ();
      if (!empty ($loveIds)) {
         $idString = implode(",", $loveIds);
         $stmt = $this->prepare ("SELECT `alias`.`alias`, `alias`.`love_id`
                                  FROM `alias`
                                  WHERE `alias`.`love_id` IN ($idString);");
         $stmt->execute ();
         $stmt->bind_result ($aliasId, $loveId);
         while ($stmt->fetch ()) {
            $alias[$loveId] = $aliasId;
         } 
         $stmt->free_result ();
         $stmt->close ();
      }
      return $alias;
   }

   /**
    * Gets the body for the given love.
    * @param $loveIds string A list of love ids.
    * @return array The array of requested body.
    */
   function GetBody ($loveIds)
   {
      $body = Array ();
      if (!empty ($loveIds)) {
         $idString = implode(",", $loveIds);
         $stmt = $this->prepare ("SELECT `body`.`id`, `body`.`position`, `body`.`love_id`, `body`.`light_id`, `light`.`text`
                                  FROM `body`
                                  INNER JOIN `light` ON `body`.`light_id`=`light`.`id`
                                  WHERE `body`.`love_id` IN ($idString);");
         $stmt->execute ();
         $stmt->bind_result ($id, $position, $loveId, $lightId, $text);
         while ($stmt->fetch ()) {
            if (!array_key_exists ($loveId, $body)) {
                  $body[$loveId] = Array();
            }
            $body[$loveId][] = Array ("id" => $id, "position" => $position, "lid" => $lightId, "text" => $text);
         } 
         $stmt->free_result ();
         $stmt->close ();
      }
      return $body;
   }

   function addNoData ($loveId)
   {
      $stmt = $this->prepare ("REPLACE INTO `no_data` (`love_id`) VALUES (?);");
      $stmt->bind_param ("i", $loveId);
      $stmt->execute ();
      $stmt->close ();
   }

   function removeNoData ($loveId)
   {
      $stmt = $this->prepare ("DELETE FROM `no_data` WHERE `love_id`=?;");
      $stmt->bind_param ("i", $loveId);
      $stmt->execute ();
      $stmt->close ();
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