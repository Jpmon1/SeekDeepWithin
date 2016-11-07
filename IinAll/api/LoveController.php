<?php

require_once (__DIR__."/BaseController.php");

class LoveController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Gets a love id.
    * @return int The id of the love.
    */
   function Get ($light)
   {
      return $this->getLoveId ($light);
   }
   
   /**
    * Gets random love.
    * @param int $count The amount of love to get.
    * @return array The array of love.
    */
   function Random ($count, $format = false)
   {
      $count = 25;
      $max = $this->getMaxLoveId ();
      $randomIds = Array();
      for ($i = 0; $i <= $count; $i++) {
         $randId = mt_rand(1, $max);
         if (!in_array($randId, $randomIds)) {
            $randomIds[] = $randId;
         }
      }
      return $this->GetLove ($randomIds, $format);
   }

   /**
    * Searches for the given token.
    * @param string $token The token to search for.
    * @param bool $format True if the result should be formatted.
    * @return array The results.
    */
   function Search ($token, $format)
   {
      $lightIds = $this->SearchLight ($token);
      $loveIds = $this->SearchLove ($lightIds);
      return $this->GetLove ($loveIds, $format, $token);
   }

   /**
    * Gets the love of the given love ids..
    * @param array $loveIds The love ids to get.
    * @param bool $format True if the result should be formatted.
    * @return array The results.
    */
   private function GetLove ($loveIds, $format, $searchText = "")
   {
      $loves = Array ();
      $idString = implode(",", $loveIds);
      $stmt = $this->prepare ("SELECT `peace`.`love_id`, `peace`.`light_id`, `peace`.`sequence`, `light`.`text` 
                              FROM `peace` 
                              INNER JOIN `light` ON `peace`.`light_id`=`light`.`id` 
                              WHERE `peace`.`love_id` IN ($idString) 
                              ORDER BY `peace`.`sequence` ASC;");/*`peace`.`love_id` ASC, */
      $stmt->execute ();
      $stmt->bind_result ($love_id, $light_id, $order, $text);
      while ($stmt->fetch ()) {
         if (!array_key_exists ($love_id, $loves)) {
            $loves[$love_id] = Array("id" => $love_id);
         }
         $loves[$love_id]["l"][] = Array ("id" => $light_id, "order" => $order, "text" => $text);
      } 
      $stmt->free_result ();
      $stmt->close ();
      
      $alias = $this->GetAlias ($loveIds);
      foreach ($alias as $key => $value) {
         $loves[$key]["a"] = $value;
      }
      $body = $this->GetBody ($loveIds);
      foreach ($body as $key => $value) {
         $loves[$key]["b"] = $value;
      }
      $style = $this->GetStyle ($loveIds);
      foreach ($style as $key => $value) {
         $loves[$key]["s"] = $value;
      }
      if ($format) {
         $loves = $this->format($loves, $searchText);
      }
      usort($loves, function($a, $b) use ($loveIds) {
         $aIndex = array_search ($a["id"], $loveIds);
         $bIndex = array_search ($b["id"], $loveIds);
         if ($aIndex == $bIndex) {
            return 0;
         }
         return $aIndex < $bIndex ? -1 : 1;
      });
      return $loves;
   }

   /**
    * Searches love for the given light ids.
    * @param array $lightIds The light ids to search for.
    * @return array The list of love ids found.
    */
   private function SearchLove ($lightIds)
   {
      $love = array ();
      $idString = implode(",", $lightIds);
      $stmt = $this->prepare ("SELECT `love_id`
                               FROM `peace`
                               WHERE (`love_id`, `sequence`) IN
                                 (SELECT `love_id`, MAX(`sequence`)
                                 FROM `peace`
                                 GROUP BY `love_id`)
                               AND `light_id` IN ($idString) LIMIT 25;");
      $stmt->execute ();
      $stmt->bind_result ($love_id);
      while ($stmt->fetch ()) {
         $love[] = $love_id;
      } 
      $stmt->free_result ();
      $stmt->close ();
      return $love;
   }

   /**
    * Searches light for the given token.
    * @param string $token The token to search for.
    * @return array The list of light ids found.
    */
   private function SearchLight ($token)
   {
      $light = array();
      $stmt = $this->prepare ("SELECT id FROM light WHERE text LIKE ?;");
      $search = "%" . $token . "%";
      $stmt->bind_param ("s", $search);
      $stmt->execute ();
      $stmt->bind_result ($id);
      while ($stmt->fetch ()) {
         $light[] = $id;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $light;
   }
}
?>