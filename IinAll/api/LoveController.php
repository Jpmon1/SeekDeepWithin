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
    * Gets random love.
    * @param int $count The amount of love to get.
    * @return array The array of love.
    */
   function Get ($light)
   {
      if (empty($light)) {
         $count = 25;
         $max = $this->getMaxLoveId ();
         $randomIds = Array();
         for ($i = 0; $i <= $count; $i++) {
            $randId = mt_rand(1, $max);
            if (!in_array($randId, $randomIds)) {
               $randomIds[] = $randId;
            }
         }
         $loves = Array ();
         $idString = implode(",", $randomIds);
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
         
         $alias = $this->GetAlias ($randomIds);
         foreach ($alias as $key => $value) {
            $loves[$key]["a"] = $value;
         }
         $body = $this->GetBody ($randomIds);
         foreach ($body as $key => $value) {
            $loves[$key]["b"] = $value;
         }
         usort($loves, function($a, $b) use ($randomIds) {
            $aIndex = array_search ($a["id"], $randomIds);
            $bIndex = array_search ($b["id"], $randomIds);
            if ($aIndex == $bIndex) {
               return 0;
            }
            return $aIndex < $bIndex ? -1 : 1;
         });
         return $loves;
      } else {
         return $this->getLoveId ($light);
      }
   }
   
   /**
    * Creates love for the list of json data given.
    * @param string $jsonData The string representation of the data to create.
    * @return array An array with a log of what happened.
    */
   function Create ($jsonData)
   {
   }
}
?>