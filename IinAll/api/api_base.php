<?php
abstract class Api
{
    /**
     * Property: method
     * The HTTP method this request was made in, either GET, POST, PUT or DELETE
     */
    protected $method = '';
    /**
     * Property: endpoint
     * The Model requested in the URI. eg: /files
     */
    protected $endpoint = '';
    /**
     * Property: verb
     * An optional additional descriptor about the endpoint, used for things that can
     * not be handled by the basic methods. eg: /files/process
     */
    protected $verb = '';
    /**
     * Property: args
     * Any additional URI components after the endpoint and verb have been removed, in our
     * case, an integer ID for the resource. eg: /<endpoint>/<verb>/<arg0>/<arg1>
     * or /<endpoint>/<arg0>
     */
    protected $args = Array();
    protected $params = Array();
    /**
     * Property: file
     * Stores the input of the PUT request
     */
    protected $file = Null;

    /**
     * Constructor: __construct
     * Allow for CORS, assemble and pre-process the data
     */
    public function __construct($request) {
        header("Access-Control-Allow-Orgin: *");
        header("Access-Control-Allow-Methods: *");
        header("Content-Type: application/json");

        $this->args = explode('/', rtrim($request, '/'));
        $this->endpoint = array_shift($this->args);
        if (array_key_exists(0, $this->args) && !is_numeric($this->args[0])) {
            $this->verb = array_shift($this->args);
        }

        $this->method = $_SERVER['REQUEST_METHOD'];
        if ($this->method == 'POST' && array_key_exists('HTTP_X_HTTP_METHOD', $_SERVER)) {
            if ($_SERVER['HTTP_X_HTTP_METHOD'] == 'DELETE') {
                $this->method = 'DELETE';
            } else if ($_SERVER['HTTP_X_HTTP_METHOD'] == 'PUT') {
                $this->method = 'PUT';
            } else {
                throw new Exception("Unexpected Header");
            }
        }

        switch($this->method) {
        case 'DELETE':
        case 'POST':
            $this->params = $this->retrievePostData ();
            break;
        case 'GET':
            $this->params = $this->_cleanInputs($_GET);
            break;
        case 'PUT':
            //$this->params = $this->_cleanInputs($_GET);
            $this->file = file_get_contents("php://input");
            //$this->file = urldecode ($this->file);
            $this->params = json_decode($this->file, TRUE);
            if ($this->params == null) {
                parse_str($this->file, $put_variables);
                if (count($put_variables)) {
                    $this->params = $put_variables;
                }
            }
            break;
        default:
            $this->_response('Invalid Method', 405);
            break;
        }
    }

    function retrievePostData() {
        // First check to see if $_POST is not empty.
        if (count($_POST)) {
            return $_POST;
        }
        // Else try to retrieve data from php://input for raw POST data.
        elseif ($this->file = file_get_contents('php://input')) {
            // Check to see if post_data is a json string.
            if ($post_json = json_decode($this->file, TRUE)) {
                return $post_json;
            }
            // Else attempt to extract key value pairs from data, using parse_str().
            else {
                parse_str($this->file, $post_variables);
                // Successful at retrieving post variables?
                if (count($post_variables)) {
                    return $post_variables;
                }
            }
        }
        // Not able to retrieve POST data, return FALSE.
        return FALSE;
    }
    
    public function process() {
        if (method_exists($this, $this->endpoint)) {
            return $this->_response($this->{$this->endpoint}($this->params));
        }
        return $this->_response("No Endpoint: $this->endpoint", 404);
    }

    private function _response($data, $status = 200) {
        header("HTTP/1.1 " . $status . " " . $this->_requestStatus($status));
        return json_encode($data);
    }

    private function _cleanInputs($data) {
        $clean_input = Array();
        if (is_array($data)) {
            foreach ($data as $k => $v) {
                $clean_input[$k] = $this->_cleanInputs($v);
            }
        } else {
            $clean_input = trim(strip_tags($data));
        }
        return $clean_input;
    }

    private function _requestStatus($code) {
        $status = array(  
            200 => 'OK',
            404 => 'Not Found',   
            405 => 'Method Not Allowed',
            500 => 'Internal Server Error',
        ); 
        return ($status[$code])?$status[$code]:$status[500]; 
    }
}
?>