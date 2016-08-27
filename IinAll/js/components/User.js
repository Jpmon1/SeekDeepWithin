import React from 'react';
import SHA512 from 'sha512-es';
import Register from './Register';
import { logoutRequest, saveUserDataRequest, toggleUserEdit } from '../actions/AccountActions';

export default class Login extends React.Component {
  /**
   * Initializes the login dialog class.
   */
  constructor(props) {
    super(props);
    this.onSave = this.onSave.bind (this);
  }

  /**
   * Occurs when the login button is pressed.
   */
  onSave ()
  {
    var hash = SHA512.hash(this.refs.txtPassword.value);
    //loginRequest (this.refs.txtEmail.value, hash);
  }
   
  /**
   * Renders the Component.
   */
  render () {
    var dimmerNames = "ui ";
    dimmerNames += (this.props.userData.isUserDataOpen) ? "active " : "disabled "
    dimmerNames += "page dimmer";
    var formClasses = "ui ";
    formClasses += (this.props.userData.isLoading) ? "loading " : ""
    formClasses += "form textLeft";
    return (
      <div className={dimmerNames}>
        <div className="content">
          <div className="center">
            <div className="ui very padded text container segment">
              <h2 className="ui header">Edit Your Information</h2>

              <div className={formClasses}>
                <div className="field">
                  <label htmlFor="txtEmail">Email</label>
                  <input id="txtEmail" ref="txtEmail" type="text" placeholder="Email" />
                </div>
                <div className="field">
                  <label htmlFor="txtPassword" className="left">Password</label>
                  <input id="txtPassword" type="Password" ref="txtPassword" placeholder="Password" />
                </div>
              </div>
              <div className="mTop">
               <div className="ui button" onClick={() => logoutRequest()}>Logout</div>
               <div className="ui black deny button" onClick={() => toggleUserEdit(false)}>Cancel</div>
               <div className="ui positive button">Submit</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }   
}
