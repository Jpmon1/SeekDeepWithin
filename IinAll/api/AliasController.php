<?php

require_once (__DIR__."/BaseController.php");

class AliasController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Gets the body for the given love.
    * @param int $loveId The id of the love that has the body.
    * @return array The current body.
    */
   function Get ($loveId)
   {
      $alias = $this->GetAlias (array($loveId));
      return empty($alias) ? -1 : $alias [$loveId];
   }
   
   /**
    * Adds the given alias to the database.
    * @param int $loveId The id of the love that has an alias.
    * @param int $aliasId The id of the alias.
    */
   function Create ($loveId, $aliasId)
   {
      $stmt = $this->prepare ("REPLACE INTO `alias` (`love_id`, `alias`) VALUES (?, ?);");
      $stmt->bind_param ("ii", $loveId, $aliasId);
      $stmt->execute ();
      $stmt->close ();
   }

   /**
    * Deletes the given alias.
    * @param int $loveId The id of the love that has an alias.
    */
   function Delete ($loveId)
   {
      $stmt = $this->prepare ("DELETE FROM `alias` WHERE `love_id`=?;");
      $stmt->bind_param ("i", $loveId);
      $stmt->execute ();
      $stmt->close ();
   }
}
?>