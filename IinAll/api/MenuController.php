<?php

require_once (__DIR__."/BaseController.php");

class MenuController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Gets the menu for the given love.
    * @param int $loveId The id of the love to get a menu for.
    * @return array The menu items.
    */
   function Get ($loveId)
   {
   }
}
?>