<?php

/**
 * Gets server Parameters.
 *
 * @author Jonathan Montiverdi
 */
class Params
{

   private $m_Get = null;
   private $m_Post = null;
   private $m_Server = null;
   private $m_Cookie = null;

   /**
    * Gets the given parameter from the URL get parameters.
    * @param string $param Name of parameter.
    * @param string $default The default value to return.
    * @return mixed The value of the requested parameter.
    */
   public function get ($param, $default = "")
   {
      if ($this->m_Get == null) {
         $this->m_Get = filter_input_array (INPUT_GET);
      }
      return isset ($this->m_Get[$param]) ? urldecode ($this->m_Get[$param]) : $default;
   }

   /**
    * Gets the given parameter from the post parameters.
    * @param string $param Name of parameter.
    * @param string $default The default value to return.
    * @return mixed The value of the requested parameter.
    */
   public function post ($param, $default = "")
   {
      if ($this->m_Post == null) {
         $this->m_Post = filter_input_array (INPUT_POST);
      }
      return isset ($this->m_Post[$param]) ? urldecode ($this->m_Post[$param]) : $default;
   }

   /**
    * Gets the given parameter from the server parameters.
    * @param string $param Name of parameter.
    * @param string $default The default value to return.
    * @return mixed The value of the requested parameter.
    */
   public function server ($param, $default = "")
   {
      if ($this->m_Server == null) {
         $this->m_Server = filter_input_array (INPUT_SERVER);
      }
      return isset ($this->m_Server[$param]) ? urldecode ($this->m_Server[$param]) : $default;
   }

   /**
    * Gets the given parameter from the cookies.
    * @param string $param Name of parameter.
    * @param string $default The default value to return.
    * @return mixed The value of the requested parameter.
    */
   public function cookie ($param, $default = "")
   {
      if ($this->m_Cookie == null) {
         $this->m_Cookie = filter_input_array (INPUT_COOKIE);
      }
      return isset ($this->m_Cookie[$param]) ? urldecode ($this->m_Cookie[$param]) : $default;
   }

}
