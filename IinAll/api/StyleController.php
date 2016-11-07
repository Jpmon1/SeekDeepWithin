<?php

require_once (__DIR__."/BaseController.php");

class StyleController extends BaseController
{
   /**
    * Database constructor.
    */
   function __construct ()
   {
      parent::__construct ();
   }
   
   /**
    * Gets the style for the given love.
    * @param int $loveId The id of the love that has the style.
    * @return array The current style.
    */
   function Get ($loveId)
   {
      $style = $this->GetStyle (array($loveId));
      return empty($style) ? $style : $style [$loveId];
   }
   
   /**
    * Adds the given style to the database.
    * @param string $tag The tag of the style to add.
    * @param int $startIndex The starting position of the style to add.
    * @param int $endIndex The ending position of the style to add.
    * @param string $css The css of the style to add.
    * @return int The ID of the added style.
    */
   function Create ($loveId, $tag, $startIndex, $endIndex, $css)
   {
      $styleId = empty ($css) ? null : $this->CreateStyle ($css);
      $styleTagId = $this->CreateStyleTag ($tag);
      $stmt = $this->prepare ("INSERT INTO `love_style`(`love_id`, `start_index`, `end_index`, `style_tag_id`, `style_id`) VALUES (?,?,?,?,?);");
      $stmt->bind_param ("iiiii", $loveId, $startIndex, $endIndex, $styleTagId, $styleId);
      $stmt->execute ();
      $id = $this->getLastInsertId ();
      $stmt->close ();
      return $id;
   }

   /**
    * Gets or Adds the given style tag to the database.
    * @param string $tag The tag of the style to add.
    * @return int The ID of the added style.
    */
   private function CreateStyleTag ($tag)
   {
      $id = $this->GetStyleTagId ($tag);
      if ($id == -1) {
         $stmt = $this->prepare ("INSERT INTO `style_tag` (`tag`) VALUES (?);");
         $stmt->bind_param ("s", $tag);
         $stmt->execute ();
         $id = $this->getLastInsertId ();
         $stmt->close ();
      }
      return $id;
   }

   /**
    * Gets or Adds the given style to the database.
    * @param string $css The css of the style to add.
    * @return int The ID of the added style.
    */
   private function CreateStyle ($css)
   {
      $id = $this->GetStyleId ($css);
      if ($id == -1) {
         $stmt = $this->prepare ("INSERT INTO `style` (`css`) VALUES (?);");
         $stmt->bind_param ("s", $css);
         $stmt->execute ();
         $id = $this->getLastInsertId ();
         $stmt->close ();
      }
      return $id;
   }

   /**
    * Gets the ID of the given style tag from the database.
    * @param string $tag The tag of the style to add.
    * @return int The ID of the added style, -1 if not in the database.
    */
   private function GetStyleTagId ($tag)
   {
      $id = -1;
      $stmt = $this->prepare ("SELECT `id` FROM `style_tag` WHERE `tag`=? ;");
      $stmt->bind_param ("s", $tag);
      $stmt->execute ();
      $stmt->bind_result ($styleId);
      if ($stmt->fetch ()) {
         $id = $styleId;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $id;
   }

   /**
    * Gets the ID of the given style from the database.
    * @param string $css The css of the style to add.
    * @return int The ID of the added style, -1 if not in the database.
    */
   private function GetStyleId ($css)
   {
      $id = -1;
      $stmt = $this->prepare ("SELECT `id` FROM `style` WHERE `css`=?");
      $stmt->bind_param ("s", $css);
      $stmt->execute ();
      $stmt->bind_result ($styleId);
      if ($stmt->fetch ()) {
         $id = $styleId;
      }
      $stmt->free_result ();
      $stmt->close ();
      return $id;
   }

   /**
    * Deletes the given body.
    */
   function Delete ($id)
   {
      $stmt = $this->prepare ("DELETE FROM `love_style` WHERE `id`=?;");
      $stmt->bind_param ("i", $id);
      $stmt->execute ();
      $stmt->close ();
   }
}
?>