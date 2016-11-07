import React from 'react';
import { requestSearch } from '../actions/DataActions';

export default class Search extends React.Component {
  /**
   * Initializes the Component.
   */
  constructor(props) {
    super(props);
    this.searchKeyPress = this.searchKeyPress.bind (this);
  }

  searchKeyPress (e) {
    if (e.key === 'Enter') {
       requestSearch (this.refs["searchBox"].value);
    }
  }

   /**
    * Renders the Component.
    */
  render () {
    const styles = {
       bar: {top:0,left:0,width:'100%',zIndex: 3000,padding: '0.5rem 0.5rem',backgroundColor: 'white',position: 'fixed',
               msBoxShadow: '2px 2px 2px #aaa', OBoxShadow: '2px 2px 2px #aaa',WebkitBoxShadow: '2px 2px 2px #aaa',
               MozBoxShadow: '2px 2px 2px #aaa',BoxShadow: '2px 2px 2px #aaa'},
       about: {marginLeft: '1rem'}
    };
    //<a style={styles.about}>About</a>
    return (<div style={styles.bar} className="expanded row">
               <div className="small-6 medium-3 columns" style={styles.column}>
                  <a href="http://www.iinall.com">I in All</a>
               </div>
               <div className="small-6 medium-9 columns">
                  <input type="text" ref="searchBox" placeholder="Search..." onKeyPress={this.searchKeyPress} />
               </div>
            </div>);
  }   
}
