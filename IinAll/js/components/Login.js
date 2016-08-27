import React from 'react';
import SHA512 from 'sha512-es';
import Register from './Register';
import { loginRequest, toggleLogin, toggleRegister } from '../actions/AccountActions';

export default class Login extends React.Component {
  /**
   * Initializes the login dialog class.
   */
  constructor(props) {
    super(props);
    this.onLogin = this.onLogin.bind (this);
  }

  /**
   * Occurs when the login button is pressed.
   */
  onLogin ()
  {
    var hash = SHA512.hash(this.refs.txtPassword.value);
    loginRequest (this.refs.txtEmail.value, hash);
  }
   
   /**
    * Renders the Component.
    */
   render () {
     var dimmerClasses = "ui ";
     dimmerClasses += (this.props.userData.isLoginOpen) ? "active " : "disabled "
     dimmerClasses += "page dimmer";
     var formClasses = "ui ";
     formClasses += (this.props.userData.isLoading) ? "loading " : ""
     formClasses += "form textLeft";
     return (
      <div className={dimmerClasses}>
        <div className="content">
          <div className="center">
            <div className="ui very padded text container segment">
              <h2 className="ui header">Login</h2>

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
              <span className="error">{this.props.userData.loginError}</span>
              <div className="mTop">
                <div className="ui button">
                  <a onClick={()=> toggleRegister(true)}>Register</a>
                  <Register userData={this.props.userData} />
                </div>
                <div className="ui black deny button" onClick={() => toggleLogin(false)}>Cancel</div>
                <div className="ui positive button" onClick={() => this.onLogin()}>Submit</div>
              </div>
            </div>
          </div>
        </div>
      </div>
     );
   }   
}
