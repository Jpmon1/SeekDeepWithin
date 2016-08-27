import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';
import { login, logout, register } from '../api/AccountApi';

/**
 * Sends a login request to the server.
 */
export function loginRequest (email, hash)
{
   AppDispatcher.handleViewAction ({
      actionType: IinAllConstants.LOGIN
   });
   login (email, hash);
}

/**
 * Processes the login response from the server.
 */
export function loginResponse (response)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.LOGIN_RESPONSE,
      response: response,
   });
}

/**
 * Sends a logout request to the server.
 */
export function logoutRequest ()
{
   AppDispatcher.handleViewAction ({
      actionType: IinAllConstants.LOGOUT
   });
   logout ();
}

/**
 * Processes the logout response from the server.
 */
export function logoutResponse (response)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.LOGOUT_RESPONSE,
      response: response,
   });
}

/**
 * Sends a save user data request to the server.
 */
export function saveUserDataRequest ()
{
   AppDispatcher.handleViewAction ({
      actionType: IinAllConstants.SAVE_USER_DATA
   });
}

/**
 * Processes the save user data response from the server.
 */
export function saveUserDataResponse (response)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.SAVE_USER_DATA_RESPONSE,
      response: response,
   });
}

/**
 * Sends a register request to the server.
 */
export function registerRequest (name, email, hash)
{
   AppDispatcher.handleViewAction ({
      actionType: IinAllConstants.REGISTER
   });
   register (name, email, hash);
}

/**
 * Processes the register response from the server.
 */
export function registerResponse (response)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.REGISTER_RESPONSE,
      response: response,
   });
}

/**
 * Processes the user response.
 */
export function  userResponse (response) {
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.USER_RESPONSE,
      response: response,
   });
}

/**
 * Toggles the login screen.
 */
export function toggleLogin (show)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.TOGGLE_LOGIN,
      response: show
   });
}

/**
 * Toggles the register screen.
 */
export function toggleRegister (show)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.TOGGLE_REGISTER,
      response: show
   });
}

/**
 * Toggles the user edit screen.
 */
export function toggleUserEdit (show)
{
   AppDispatcher.handleServerAction({
      actionType: IinAllConstants.TOGGLE_USERDATA,
      response: show
   });
}
