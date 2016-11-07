<?php

class LoveRenderer
{
   private $m_Insertions;
   private $m_SearchText;

   /**
    * Database constructor.
    */
   function __construct ($searchText)
   {
      $this->m_SearchText = $searchText;
   }
   
   /**
    * Renders the given data.
    * @param string $text The original text.
    * @param array $body Any body for the data.
    * @param array $style Any style for the data.
    * @param string $searchText The text that the user searched for.
    * @return string The text with HTML markup.
    */
   function Render ($text, $body = null, $style = null)
   {
      $html = $text;
      $footerIndex = 1;
      unset ($this->m_Insertions);
      $this->m_Insertions = array();
      
      if ($style != null) {
         foreach ($style as $key => $value) {
            $startTag = "<".$value["tag"];
            if (!empty ($style["css"])) {
               $startTag .= " style=\"".$style["css"]."\"";
            }
            $startTag .= ">";
            $html = $this->Insert ($html, $startTag, $value["start"], "</".$value["tag"].">", $value["end"]);
         }
      }

      // TODO: Highlight search text...
      if (!empty ($this->m_SearchText)) {
         
      }

      if ($body != null) {
         foreach ($body as $key => $value) {
            if ($value["position"] >= 0) {
               $html = $this->Insert ($html, "<sup>($footerIndex)</sup>", $value["position"]);
               $footerIndex++;
            }
         }
      }

      return $html;
   }

   /**
    * Inserts the given tag information at the given indexes.
    * @param string $start Beginning tag information.
    * @param int $startIndex Start insertion index.
    * @param string $end Ending tag information.
    * @param int $endIndex End insertion index.
    */
   private function Insert ($html, $start, $startIndex, $end = "", $endIndex = -1)
   {
      $index = $this->GetIndex ($startIndex);
      $length = strlen($html);
      if ($index > $length) { $index = $length; }
      $head = mb_substr ($html, 0, $index);
      $tail = mb_substr ($html, $index);
      $html =$head.$start.$tail;
      $this->AddLength ($startIndex, strlen($start));

      if (!empty($end) && $endIndex > 0) {
         $index = $this->GetIndex ($endIndex, true);
         $length = strlen($html);
         if ($index > $length) { $index = $length; }
         $head = mb_substr ($html, 0, $index);
         $tail = mb_substr ($html, $index);
         $html =$head.$end.$tail;
         $this->AddLength ($endIndex, strlen($end));
      }
      return $html;
   }

   /**
    * Adds length the the insertion dictionary.
    * @param int $index Index to add length for.
    * @param int $length The amount of length to add.
    */
   private function AddLength ($index, $length)
   {
      if (array_key_exists($index, $this->m_Insertions)){
         $this->m_Insertions[$index] = $this->m_Insertions[$index] + length;
      } else {
         $this->m_Insertions[$index] = $length;
      }
   }

   /**
    * Gets the insertion index for the given index based on previous insertions.
    * @param int $index The index to start.
    * @param bool $before Index returned will come before a conflicting index.
    * @return int The corrected start index.
    */
   private function GetIndex ($index, $before = false)
   {
      $addIndex = $index;
      foreach ($this->m_Insertions as $key => $value) {
         if ($before) {
            if ($key < $index) {
               $addIndex += $value;
            }
         } else {
            if ($key <= $index) {
               $addIndex += $value;
            }
         }
      }
      return $addIndex;
   }
}
?>