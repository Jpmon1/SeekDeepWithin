<?php
session_start ();
ob_start ();
$dir = dirname (__FILE__) . '/';
require_once ($dir . 'api/IinAllDb.php');
require_once ($dir . "api/User.php");
$user = new User ();
?>
<!DOCTYPE html>
<!--[if IE 9]><html class="lt-ie10" lang="en" > <![endif]-->
<html class="no-js" lang="en">
   <?php
   include_once ($dir . "php/view_head.php");
   ?>
   <body>
      <?php
      if ($user->isUserLoggedIn ()){
         echo "<div id=\"user\"></div>";
      }
      if ($user->canEdit (1)) {
         echo "<div id=\"editToken\" style=\"display:none;\" data-token=\"" . $user->getEditToken () . "\" data-user=\"" . $user->getUserId () . "\"></div>\n";
      }
      ?>
      <div id="appContent">
      </div>
      <script src="dist/bundle.js"></script>
   </body>
</html>
<?php
ob_end_flush();
