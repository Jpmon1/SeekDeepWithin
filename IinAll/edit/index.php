<?php
session_start ();
ob_start ();
$dir = dirname (__FILE__) . '/';
require_once ($dir . '../api/include/IinAllDb.php');
$db = new IinAllDb ();
?>

<div class="row column">
   <input type="text" id="txtMotto" />
</div>
<hr />
<div class="row">
   <div class="small-12 medium-10 large-11 column">
         <label for="txtCreateLight">New Light</label>
      <input type="text" id="txtCreateLight" />
   </div>
   <div class="small-12 medium-2 large-1 column">
      <a id="btnCreateLight" class="sdw-button add tiny expand">Create</a>
   </div>
</div>
<hr />
<div class="row">
   <div class="small-12 medium-10 large-11 column">
         <label for="txtSearch">Build Love</label>
      <input type="text" id="txtSearch" />
   </div>
   <div class="small-12 medium-2 large-1 column">
      <a id="btnAddLightToLove" class="sdw-button add tiny expand">Add</a>
   </div>
</div>
<div class="row column" id="divLoveLight"></div>
<hr />
<div class="row column">Current Truth:</div>
<div class="row column" id="divLoveTruth"></div>
<hr />
<div class="row column">Add Truth:</div>
<div class="row">
   <div class="small-12 large-6 column">
      <div class="row column">
         <label for="txtToFormat">Text to Format:</label>
         <textarea rows="10" id="txtToFormat"></textarea>
      </div>
      <div class="row column">
         <label for="txtRegex">Formatting Regex:</label>
         <input type="text" id="txtRegex" />
      </div>
      <div class="row column">
         <?php 
         $regexes = $db->getAllRegex();
         $count = count ($regexes);
         ?>
         <select id="cmbFormatRegex">
         <?php foreach ($regexes as $key => $value) { ?>
            <option value="<?php echo $key; ?>"><?php echo $value; ?></option>
         <?php } ?>
         </select>
      </div>
      <div class="row">
         <div class="small-6 column">
            <label for="txtStartOrder">Starting Order:</label>
            <input type="number" id="txtStartOrder" />
         </div>
         <div class="small-6 column">
            <label for="txtStartNumber">Starting Number:</label>
            <input type="number" id="txtStartNumber" />
         </div>
      </div>
      <div class="row column">
         <a id="btnFormat" class="sdw-button alt small expand">Format</a>
      </div>
   </div>
   <div class="small-12 large-6 column">
      <div class="row column">
         <label for="txtFormatted">Formatted Text:</label>
         <textarea rows="10" id="txtFormatted"></textarea>
      </div>
      <div class="row column">
         <a id="btnCreateTruth" class="sdw-button add small expand">Add</a>
      </div>
   </div>
</div>
