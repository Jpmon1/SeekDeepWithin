<?php
session_start ();
ob_start ();
$dir = dirname (__FILE__) . '/';
?>
<!DOCTYPE html>
<!--[if IE 9]><html class="lt-ie10" lang="en" > <![endif]-->
<html class="no-js" lang="en">
   <?php
   include_once ($dir . "view_head.php");
   ?>
   <body>
      <?php
      include_once ($dir . "view_start.php");
      ?>
      <div class="main-content pTopBig">
         <div class="row">
            <div class="small-12 medium-4 medium-offset-4 large-4 large-offset-4 column text-center">
               <h4 style="font-weight: 300">I in All</h4>
               <hr />
            </div>
         </div>
         <div class="row column text-center seek">
            <h1>Seek, and ye shall find...</h1>
         </div>
         <div class="search-stuff">
            <div class="search-icon">
               <div class="inner-icon">
               <img src="images/icon_128.png">
               </div>
            </div>
            <div class="row column">
               <input type="search" placeholder="Search" id="txtSearch" />
            </div>
            <div class="row">
               <div class="small-6 small-offset-3 medium-4 medium-offset-4 column" id="divBtnSearch">
                  <a id="btnSearch" class="sdw-button alt expand small">Search</a>
               </div>
            </div>
            <div class="row column text-center mBottom">
               <em>Let your Heart be your guide</em>
            </div>
         </div>
         <div id="love"></div>
      </div>
      <div id="edit-stuff" class="pTopBig"></div>
      <div class="end-stuff pTop">
         <div class="row">
            <div class="large-6 columns">
               <p>&copy; <?php echo date("Y");?> - I in all</p>
            </div>
         </div>
      </div>
   </body>
   <?php
   include_once ($dir . "templates.php");
   ?>
</html>
<?php
ob_end_flush();
?>