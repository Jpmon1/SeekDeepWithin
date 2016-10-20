import React from 'react';
import Header from './Header';
import LoveList from './LoveList';
import Footer from './Footer';
import Loader from './Loader';
import DataStore from '../stores/DataStore';
import { getRandom } from '../api/DataApi';
import { checkUser } from '../api/AccountApi';
import AccountStore from '../stores/AccountStore';

export default class IinAll extends React.Component {

   constructor(props) {
      super(props);
  
      checkUser ();
      getRandom ();
      this._onChange = this._onChange.bind(this);
      this.state = { Data: DataStore.getData (), userData: AccountStore.getData () };
   }

   componentDidMount() {
      DataStore.addChangeListener(this._onChange);
      AccountStore.addChangeListener(this._onChange);
   }

   componentWillUnmount() {
      DataStore.removeChangeListener(this._onChange);
      AccountStore.removeChangeListener(this._onChange);
   }

   _onChange() {
      this.setState({ Data: DataStore.getData(), userData: AccountStore.getData () });
   }

   render () {
      var edit = <span />;
      return (
         <div>
            <Header userData={this.state.userData} />
            <Loader isLoading={this.state.Data.isLoading} />
            <div className="main-content pTop pBottomBig">
               <LoveList children={this.state.Data.list}
                         userData={this.state.userData} />
            </div>
            <Footer />
         </div>
      );
   }
   
}
