<?php

require_once (__DIR__."/BaseController.php");

class TruthController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Gets the truth for the given lights.
    * @param $light string A comma separated list of light ids.
    * @param $loveId int The love ID.
    * @return array The array of requested truth.
    */
   function Get ($light, $loveId, $format)
   {
      $truths = Array ();
      $lights = Array ();
      if ($loveId == null) {
         $loveId = $this->getLoveId ($light);
      }
      if ($loveId != -1) {
         $stmt = $this->prepare ("SELECT `peace`.`love_id`, `peace`.`light_id`, `peace`.`sequence` AS `peaceorder`, `light`.`text`, `truth`.`id`, `truth`.`sequence` AS `truthorder`
                                  FROM `peace` 
                                  INNER JOIN `light` ON `peace`.`light_id`=`light`.`id`
                                  INNER JOIN `truth` ON `peace`.`love_id`=`truth`.`truth_id`
                                  WHERE (`peace`.`love_id`, `truth`.`id`) IN
                                    (SELECT `truth`.`truth_id`, `truth`.`id`
                                     FROM `truth`
                                     WHERE `truth`.`love_id`=?
                                     ORDER BY `truth`.`sequence`)
                                  ORDER BY `truthorder` ASC, `peace`.`love_id` ASC, `peace`.`sequence` ASC;");
         $stmt->bind_param ("i", $loveId);
         $stmt->execute ();
         $stmt->bind_result ($love_Id, $lightId, $peaceOrder, $text, $truthId, $truthOrder);
         $loveIds = Array ();
         while ($stmt->fetch ()) {
            if (!array_key_exists ($love_Id, $truths)) {
               $loveIds[] = $love_Id;
               $truths[$love_Id] = Array("id" => $love_Id, "tid" => $truthId, "order" => $truthOrder);
            }
            $truths[$love_Id]["l"][] = Array ("id" => $lightId, "order" => $peaceOrder, "text" => $text);
            
         }
         $stmt->free_result ();
         $stmt->close ();

         $count = count ($loveIds);
         if ($count > 0) {
            $alias = $this->GetAlias ($loveIds);
            foreach ($alias as $key => $value) {
                $truths[$key]["a"] = $value;
            }
            $body = $this->GetBody ($loveIds);
            foreach ($body as $key => $value) {
                $truths[$key]["b"] = $value;
            }
            $style = $this->GetStyle ($loveIds);
            foreach ($style as $key => $value) {
                $truths[$key]["s"] = $value;
            }
            if ($format) {
               $truths = $this->format($truths);
            }
         } else {
            $this->addNoData ($loveId);
         }
      }
      return Array ("love" => $loveId, "truths" => $truths);
   }
   
   /**
    * Creates truths for the list of json data given.
    * @param string $jsonData The string representation of the data to create.
    * @return array An array with a log of what happened.
    */
   function Create ($jsonData)
   {
      $hasBody = false;
      $hasAlias = false;
      $data = json_decode($jsonData);
      $truthQuery = "INSERT INTO `iinallco_data`.`truth` (`love_id`, `truth_id`, `sequence`) VALUES ";
      $aliasQuery = "INSERT INTO `iinallco_data`.`alias` (`love_id`, `alias`) VALUES ";
      $bodyQuery = "INSERT INTO `iinallco_data`.`body` (`position`, `love_id`, `light_id`) VALUES ";
      $loveId = $this->createLove ($data->love);
      $this->removeNoData ($loveId);
      $itemCount = count ($data->truth);
      for ($j = 0; $j < $itemCount; $j++) {
         $item = $data->truth[$j];
         $love = $item->love;
         $order = $item->order;
         $truthId = $this->createLoveText ($love);
         $truthQuery .= "('".$loveId."','".$truthId."','".$order."'),";
         if ($item->body != null) {
            $hasBody = true;
            $bodyCount = count ($item->body);
            for ($i = 0; $i < $bodyCount; $i++) {
              $body = $item->body[$i];
              $pos = $body->pos;
              $lightId = $this->createLight ($body->text);
              $bodyQuery .= "('".$pos."','".$truthId."','".$lightId."'),";
            }
         }
         if ($item->alias != null) {
            $hasAlias = true;
            $aliasQuery .= "('".$truthId."','".$item->alias."'),";
         }
      }
      $this->query (rtrim ($truthQuery, ','));
      if ($hasBody){
         $this->query (rtrim ($bodyQuery, ','));
      }
      if ($hasAlias){
         $this->query (rtrim ($aliasQuery, ','));
      }
   }

   /**
    * Updates truths for the list of json data given.
    * @param string $jsonData The string representation of the data to update.
    * @return array An array with a log of what happened.
    */
   function Update ($jsonData)
   {
      $data = json_decode ($jsonData);
      $itemCount = count ($data->truth);
      $this->beginTransaction ();
      for ($j = 0; $j < $itemCount; $j++) {
         $item = $data->truth[$j];
         $id = $item->truthid;
         $order = $item->order;
         $this->query ("UPDATE `truth` SET `truth`.`sequence`=$order WHERE `truth`.`id`=$id;");
         $error = $this->lastError();
      }
      $this->endTransaction ();
   }

   /**
    * Deletes the given truth.
    * @param int $id The id of the truth to delete.
    */
   function Delete ($id)
   {
      $stmt = $this->prepare ("DELETE FROM `truth` WHERE `id`=?;");
      $stmt->bind_param ("i", $id);
      $stmt->execute ();
      $stmt->close ();
   }

   /**
    * Adds an item to the no data table.
    * @param int $loveId The id of the love with no data.
    */
   function addNoData ($loveId)
   {
      $stmt = $this->prepare ("REPLACE INTO `no_data` (`love_id`) VALUES (?);");
      $stmt->bind_param ("i", $loveId);
      $stmt->execute ();
      $stmt->close ();
   }

   /**
    * Removes an item from the no data table.
    * @param int $loveId The id of the love that had no data.
    */
   function removeNoData ($loveId)
   {
      $stmt = $this->prepare ("DELETE FROM `no_data` WHERE `love_id`=?;");
      $stmt->bind_param ("i", $loveId);
      $stmt->execute ();
      $stmt->close ();
   }
}
?>