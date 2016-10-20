import React from 'react';
import User from './User';
import Login from './Login';
import { toggleLogin, toggleUserEdit } from '../actions/AccountActions';

export default class Header extends React.Component {
   /**
    * Initializes the header.
    */
   constructor(props) {
      super(props);
   }
   
   /**
    * Renders the Component.
    */
   render () {
      // var action;
      // if (this.props.userData.isLoggedIn) {
      //   var message = "Welcome, " + this.props.userData.name;
      //   action = <div>
      //     <a className="item" onClick={()=>toggleUserEdit(true)}>{message}</a>
      //     <User userData={this.props.userData} />
      //   </div>;
      // } else {
      //   action = <div>
      //     <a className="item" onClick={()=>toggleLogin(true)}><i className="sign in icon"></i> Login</a>
      //     <Login userData={this.props.userData} />
      //   </div>;
      // }
      return (<h2 className="textCenter">I in All</h2>);
      /*<div className="ui stackable menu">
            <div className="header item">I in All</div>
            <div className="item">
              <div className="ui transparent icon input">
                  <input className="prompt" type="text" placeholder="Search..." />
                  <i className="search link icon"></i>
              </div>
              <div className="results"></div>
            </div>
            <div className="right menu">  
              <a className="item"><i className="help circle outline icon"></i>About</a>
            </div>
          </div>*/
   }
   
}
