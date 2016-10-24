<?php

require_once (__DIR__."/BaseController.php");

class BodyController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Adds the given body to the database.
    * @param int $position The position of the body to add.
    * @param string $text The text of the body to add.
    * @return int The ID of the added body.
    */
   function Create ($position, $text, $loveId)
   {
      $lightId = $this->createLight ($text);
      $stmt = $this->prepare ("INSERT INTO `body`(`position`, `love_id`, `light_id`) VALUES (?,?,?);");
      $stmt->bind_param ("iii", $position, $loveId, $lightId);
      $stmt->execute ();
      $id = $this->getLastInsertId ();
      $stmt->close ();
      return $id;
   }

   /**
    * Deletes the given body.
    */
   function Delete ($id)
   {
      $stmt = $this->prepare ("DELETE FROM `body` WHERE `id`=?;");
      $stmt->bind_param ("i", $id);
      $stmt->execute ();
      $stmt->close ();
   }
}
?>