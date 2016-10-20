<?php

require_once (__DIR__."/BaseController.php");

class LightController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Get a random number of lights.
    * @param int $count The number of items to get
    * @return array An array of random lights.
    */
   function Get ($count)
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
    * Adds the given light to the database.
    * @param string $light The light to add.
    * @return int The ID of the added light.
    */
   function Create ($light)
   {
      return $this->createLight ($light);
   }
   
   /**
    * Edits the given light in the database.
    * @param integer $id The id of the light to edit.
    * @param string $light The light's new text.
    */
   function Update ($id, $light)
   {
      $light = trim ($light);
      $stmt = $this->prepare ("UPDATE light SET text=? WHERE id=?;");
      $stmt->bind_param ("si", $light, $id);
      $stmt->execute ();
      $stmt->close ();
   }
}
?>