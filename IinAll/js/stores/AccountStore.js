import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';
import { EventEmitter } from 'events';

const CHANGE_EVENT = 'change';
let _accountStore = {
  isLoading: false,
  isLoggedIn: false,
  isLoginOpen: false,
  canUserEdit: false,
  isRegisterOpen: false,
  isUserDataOpen: false,
  name: '',
  email: '',
  userId: '',
  token: '',
  level: 0,
  loginError: '',
  registerError: ''
};

class AccountStoreClass extends EventEmitter {
  addChangeListener (cb) {
    this.on (CHANGE_EVENT, cb);
  }
  removeChangeListener (cb) {
    this.removeListener (CHANGE_EVENT, cb);
  }
  getData () {
    return _accountStore;
  }
}

/**
 * Sets the user data retrieved from the server.
 */
function _setUserData (response)
{
    _accountStore.isLoggedIn = true;
    _accountStore.userId = response.id;
    _accountStore.name = response.name;
    _accountStore.email = response.email;
    _accountStore.token = response.token;
    _accountStore.level = response.level;
    if (response.level > 0) {
        _accountStore.canUserEdit = true;
    }
}

const AccountStore = new AccountStoreClass ();

AppDispatcher.register ((payload) => {
  const action = payload.action;
  switch (action.actionType) {

    case IinAllConstants.TOGGLE_LOGIN:
      _accountStore.isLoginOpen = action.response;
      AccountStore.emit (CHANGE_EVENT);
      break;   

    case IinAllConstants.TOGGLE_REGISTER:
      _accountStore.isRegisterOpen = action.response;
      AccountStore.emit (CHANGE_EVENT);
      break;    

    case IinAllConstants.TOGGLE_USERDATA:
      _accountStore.isUserDataOpen = action.response;
      AccountStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.LOGIN:
      _accountStore.isLoading = true;
      AccountStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.REGISTER:
      _accountStore.isLoading = true;
      AccountStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.LOGOUT:
      _accountStore.isLoading = true;
      AccountStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.USER_RESPONSE:
      const userResponse = action.response;
      _accountStore.isLoading = false;
      if (userResponse.status == 'success') {
        _setUserData (userResponse); 
        AccountStore.emit (CHANGE_EVENT);
      }
      break;

    case IinAllConstants.LOGIN_RESPONSE:
	    const loginResponse = action.response;
      if (loginResponse.status == 'success'){
        _setUserData (loginResponse);
        _accountStore.isLoginOpen = false;
      } else {
        _accountStore.loginError = response.message;
      }
      _accountStore.isLoading = false;
      AccountStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.LOGOUT_RESPONSE:
	  const logoutResponse = action.response;
      _accountStore.isLoading = false;
      _accountStore.isLoggedIn = false;
      _accountStore.isUserDataOpen = false;
      _accountStore.userId = 0;
      _accountStore.name = '';
      _accountStore.email = '';
      _accountStore.token = '';
      _accountStore.level = 0;
      AccountStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.REGISTER_RESPONSE:
	  const regResponse = action.response;
      _accountStore.isLoading = false;
      if (regResponse.status == 'success'){
        _setUserData (regResponse);
        _accountStore.isLoginOpen = false;
        _accountStore.isRegisterOpen = false;
      } else {
        _accountStore.loginError = regResponse.message;
      }
      _accountStore.isLoading = false;
      AccountStore.emit (CHANGE_EVENT);
      break;

    default:
      return true;
  }
});

export default AccountStore;
