import request from 'superagent';
import { loginResponse, registerResponse, userResponse, logoutResponse } from '../actions/AccountActions';

/**
 * Sends a login request to the server.
 */
export function login (email, hash) {
   request.post ('/IinAllDev/api/api.php?request=Login')
      .send ({email: email, hash: hash})
      .set ('Accept', 'application/json')
      .end ((err, response) => {
         if (err) return console.error (err);
            loginResponse (response.body);
      });
}

/**
 * Sends a logout request to the server.
 */
export function logout () {
   request.post ('/IinAllDev/api/api.php?request=Logout')
      .set ('Accept', 'application/json')
      .end ((err, response) => {
         if (err) return console.error (err);
            logoutResponse (response.body);
      });
}
/**
 * Sends a register request to the server.
 */
export function register (name, email, hash) {
   request.post ('/IinAllDev/api/api.php?request=Register')
      .send ({name: name, email: email, hash: hash})
      .set ('Accept', 'application/json')
      .end ((err, response) => {
         if (err) return console.error (err);
            registerResponse (response.body);
      });
}

/**
 * Checks to see if a user is logged in or not.
 */
export function checkUser ()
{
   request.get ('/IinAllDev/api/api.php?request=CheckUser')
      .set ('Accept', 'application/json')
      .end ((err, response) => {
         if (err) return console.error (err);
            userResponse (response.body);
      });
}