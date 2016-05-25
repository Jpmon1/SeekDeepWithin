<?php
$dir = dirname (__FILE__) . '/';
require_once($dir . 'iinall_api.php');

if (!array_key_exists('HTTP_ORIGIN', $_SERVER)) {
    $_SERVER['HTTP_ORIGIN'] = $_SERVER['SERVER_NAME'];
}

try {
    $api = new IinAllApi($_REQUEST['request'], $_SERVER['HTTP_ORIGIN']);
    echo $api->process();
} catch (Exception $e) {
    echo Array('status' => 'fail', 'message' => $e->getMessage());
}
?>