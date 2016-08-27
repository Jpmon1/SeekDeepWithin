import React from 'react';
import Header from './Header';
import LightList from './LightList';
import Footer from './Footer';
import Loader from './Loader';
import LightStore from '../stores/LightStore';
import { getRandomApi } from '../api/LightApi';
import { checkUser } from '../api/AccountApi';
import AccountStore from '../stores/AccountStore';

export default class IinAll extends React.Component {

   constructor(props) {
      super(props);
  
      checkUser ();
      getRandomApi ();
      this._onChange = this._onChange.bind(this);
      this.state = { lightData: LightStore.getData (), userData: AccountStore.getData () };
   }

   componentDidMount() {
      LightStore.addChangeListener(this._onChange);
      AccountStore.addChangeListener(this._onChange);
   }

   componentWillUnmount() {
      LightStore.removeChangeListener(this._onChange);
      AccountStore.removeChangeListener(this._onChange);
   }

   _onChange() {
      this.setState({ lightData: LightStore.getData(), userData: AccountStore.getData () });
   }

   render () {
      var edit = <span />;
      return (
         <div>
            <Header userData={this.state.userData} />
            <Loader isLoading={this.state.lightData.isLoading} />
            <div className="main-content pTop pBottomBig">
               <LightList children={this.state.lightData.list}
                          userData={this.state.userData} />
            </div>
            <Footer />
         </div>
      );
   }
   
}
