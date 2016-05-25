<?php

class TestApi
{
   public function __construct()
   {
      header("Access-Control-Allow-Orgin: *");
      header("Access-Control-Allow-Methods: *");
      header("Content-Type: application/json");
   }
   
   public function format()
   {
      $data = $_POST['d'];
      $regex = $_POST["r"];
      //return Array ('data' => $_REQUEST['d']);
      if ($data === "") throw new Exception('No data given');
      if ($regex === "") throw new Exception('Cannot parse data without instructions');
      //$db = new IinAllDb ();
      //$formattedData = $db->formatTruth ($data, $regex);
      //$db->close ();
      return Array('status' => 'success', 'data' => $formattedData, 'd' => $data , 'r' => $regex);
   }
}

try {
   $api = new TestApi ();
   echo json_encode($api->format());
} catch (Exception $e) {
   echo json_encode(Array('status' => 'fail', 'message' => $e->getMessage()));
}
?>