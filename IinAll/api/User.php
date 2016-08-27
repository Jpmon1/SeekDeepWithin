<?php
$incDir = dirname(__FILE__) . '/';
require_once ($incDir."UserDb.php");

/**
 * User operations for iinall.com.
 */
class User
{

    /**
     * The time the page loads.
     * @var int
     */
    private $m_Time;
    private $m_IsUserLoggedIn = false;

    /**
     * Class constructor.
     */
    function __construct ()
    {
        if (session_status() == PHP_SESSION_NONE) {
           session_start();
        }
        session_regenerate_id ();
        $this->m_Time = time ();
        $this->checkForUser ();
    }

    /**
     * Gets if a user is logged in or not.
     * @return boolean True is a user is logged in.
     */
    public function isUserLoggedIn () { return $this->m_IsUserLoggedIn; }

    /**
     * Gets the user's name.
     * @return string The user's name.
     */
    public function getUserName ()
    {
        if (isset ($_SESSION ['iinAlluser'] ['name'])) {
            return $_SESSION ['iinAlluser'] ['name'];
        }
        return "";
    }

    /**
     * Gets the user's email address.
     * @return string The user's email address.
     */
    public function getUserEmail ()
    {
        if (isset ($_SESSION ['iinAlluser'] ['email'])) {
            return $_SESSION ['iinAlluser'] ['email'];
        }
        return "";
    }

    /**
     * Gets the user's access level.
     * @return int The user's access level.
     */
    public function getLevel ()
    {
        if (isset ($_SESSION ['iinAlluser'] ['level'])) {
            return $_SESSION ['iinAlluser'] ['level'];
        }
        return 0;
    }

    /**
     * Gets the user's edit token.
     * @return int The user's edit token.
     */
    public function getEditToken ()
    {
        if (isset ($_SESSION ['iinAlluser'] ['token'])) {
            return $_SESSION ['iinAlluser'] ['token'];
        }
        return NULL;
    }

    /**
     * Gets the user's id.
     * @return int The user's id.
     */
    public function getUserId ()
    {
        if (isset ($_SESSION ['iinAlluser'] ['id'])) {
            return $_SESSION ['iinAlluser'] ['id'];
        }
        return -1;
    }

    /**
     * Gets if the current user is able to edit data.
     * @param int $level The requested edit level.
     * @return boolean True if the current user can edit, otherwise false.
     */
    public function canEdit ($level = 0)
    {
        if (isset ($_SESSION ['iinAlluser'] ['level']) && isset ($_SESSION ['iinAlluser'] ['token']) && isset ($_SESSION ['iinAlluser'] ['id'])) {
            $userAccess = $this->getLevel ();
            if ($userAccess & $level) {
                return true;
            }
        }
        return false;
    }

    /**
     * Performs a login action.
     * @param string $email The user's email.
     * @param string $password The user's hashed password.
     */
    public function logIn ($email, $password)
    {
        $db = new UserDb ();
        $ok = $db->login ($email, $password);
        if ($ok === true) {
            $this->m_IsUserLoggedIn = true;
        }
        $db->disconnect ();
        return $ok;
    }

    /**
     * Performs a login action from an open id account.
     * @param string $nonce The nonce from the open id login.
     * @param string $email The user's email.
     * @param string $name The name of the user.
     * @param string $locale The language setting of the user.
     */
    public function loginOpenId ($nonce, $email, $name, $locale)
    {
        $db = new UserDb ();
        $ok = $db->loginOpenId ($nonce, $email, $name, $locale);
        if ($ok === true) {
            $this->m_IsUserLoggedIn = true;
        }
        $db->disconnect ();
        return $ok;
    }

    /**
     * Regiserts a user with the given informaiton.
     * @param string $name The user's name.
     * @param string $email The user's email.
     * @param string $password The user's password.
     * @return string The outcome of the register process.
     */
    public function register ($name, $email, $password)
    {
        // This should never occur, but just in-case...
        if ($this->m_IsUserLoggedIn) {
            $this->logOut ();
        }
        // Add user...
        $db = new UserDb ();
        $status = $db->register ($name, $email, $password);
        $db->disconnect ();
        return $status;
    }

    /**
     * Verifies a user by cookie data.
     */
    private function verifyUser ()
    {
        $nonce = $_COOKIE ['iinAllNonce'];
        $userEmail = $_COOKIE ['iinAllEmail'];
        $db = new UserDb ();
        if ($db->verifyUser ($userEmail, $nonce) !== false) {
            $this->m_IsUserLoggedIn = true;
        } else {
            $this->logOut ();
        }
        $db->disconnect ();
    }

    /**
     * Logs the current user out.
     */
    public function logout ()
    {
        unset ($_SESSION ['iinAlluser']);
        session_unset ();
        session_destroy ();
        setcookie ("iinAllNonce", "", time () - 3600, "/", ".iinall.com");
        setcookie ("iinAllEmail", "", time () - 3600, "/", ".iinall.com");
    }

    /**
     * Checks to see if a user is logged in, gathering all the information we have for the user.
     */
    private function checkForUser ()
    {
        $db = new UserDb ();
        if  (isset ($_SESSION ['iinAlluser'] ['name']) && isset ($_SESSION ['iinAlluser'] ['email'])) {
            $this->m_IsUserLoggedIn = true;
        } elseif (isset ($_COOKIE ['iinAllNonce']) && isset ($_COOKIE ['iinAllEmail'])) {
            $this->verifyUser ();
        }
        if ($this->m_IsUserLoggedIn) {
            $db->addActiveUser ($this->getUserEmail (), $this->m_Time);
        } else {
            $db->addActiveGuest ($_SERVER['REMOTE_ADDR'], $this->m_Time);
        }
        $db->removeInactive ();
        $db->disconnect ();
    }

}

?>
