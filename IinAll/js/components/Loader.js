import React from 'react';

export default class Loader extends React.Component {
  /**
   * Initializes the loader.
   */
  constructor(props) {
    super(props);
  }

   /**
    * Renders the Component.
    */
  render () {
    var loader = null;
    if (this.props.isLoading){
      loader = <div className="loader">Loading...</div>;
    }
    return (<div>{loader}</div>);
  }   
}

Loader.propTypes = {
   isLoading: React.PropTypes.bool
};
