import React from 'react';
import Header from './Header';
import LoveList from './LoveList';
import Footer from './Footer';
import Loader from './Loader';
import Search from './Search';
import { Throttle } from '../api/Utils';
import DataStore from '../stores/DataStore';
import { requestRandom } from '../actions/DataActions';
import { checkUser } from '../api/AccountApi';
import AccountStore from '../stores/AccountStore';

export default class IinAll extends React.Component {

   constructor(props) {
      super(props);
  
      checkUser ();
      requestRandom ();
      this._onChange = this._onChange.bind(this);
      this.state = { Data: DataStore.getData (), userData: AccountStore.getData () };
      window.onscroll = Throttle (()=>this.checkLoadMore(), 300)
   }

   checkLoadMore () {
      var windowHeight = window.innerHeight;
      var body = (document.body) ? document.body : document.documentElement;
      if (body.scrollTop >= body.offsetHeight - windowHeight) {
         requestRandom ();
      }
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
            <Search />
            <Header userData={this.state.userData} />
            <Loader isLoading={this.state.Data.isLoading} />
            <div className="main-content pTop pBottom">
               <LoveList children={this.state.Data.list}
                         userData={this.state.userData} />
            </div>
            <Footer />
         </div>
      );
   }
   
}
