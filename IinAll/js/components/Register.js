import React from 'react';
import SHA512 from 'sha512-es';
import { registerRequest, toggleRegister } from '../actions/AccountActions';

export default class Register extends React.Component {
  /**
   * Initializes the register dialog class.
   */
  constructor(props) {
    super(props);
    this.state = { localError: '' };
    this.onRegister = this.onRegister.bind (this);
  }

  /**
   * Occurs when the register button is pressed.
   */
  onRegister () {
    var name = this.refs.txtName.value;
    var email = this.refs.txtEmail.value;
    var pass = this.refs.txtPassword.value;
    var pass2 = this.refs.txtPassword2.value;
    if (pass != '' && pass === pass2) {
      this.setState ({localError: ''});
      var hash = SHA512.hash (pass);
      registerRequest (name, email, hash);
    } else {
      this.setState ({localError: 'The passwords entered do not match.'});
    }
  }
   
  /**
   * Renders the Component.
   */
  render () {
    var classnames = "ui ";
    classnames += (this.props.userData.isRegisterOpen) ? "active " : "disabled "
    classnames += "page dimmer";
    return (
      <div className={classnames}>
        <div className="content">
          <div className="center">
            <div className="ui very padded text container segment">
              <h2 className="ui header">Register</h2>

              <div className="ui form textLeft">
                <div className="field">
                  <label htmlFor="txtEmail">Email</label>
                  <input id="txtEmail" ref="txtEmail" type="text" placeholder="Email" />
                </div>
                <div className="field">
                  <label htmlFor="txtName">Name</label>
                  <input id="txtName" ref="txtName" type="text" placeholder="Name" />
                </div>
                <div className="field">
                  <label htmlFor="txtPassword" className="left">Password</label>
                  <input id="txtPassword" type="Password" ref="txtPassword" placeholder="Password" />
                </div>
                <div className="field">
                  <label htmlFor="txtPassword2">Repeat Password</label>
                  <input id="txtPassword2" type="Password" ref="txtPassword2" placeholder="Repeat Password" />
                </div>
              </div>
                  <span className="error">{this.state.localError}</span>
                  <span className="error">{this.props.userData.registerError}</span>
              <div className="mTop">
                <div className="ui black deny button" onClick={() => toggleRegister(false)}>Cancel</div>
                <div className="ui positive button" onClick={() => this.onRegister()}>Submit</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }   
}
