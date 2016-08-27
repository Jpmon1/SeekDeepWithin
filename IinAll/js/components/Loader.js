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
    var classnames = "ui ";
    classnames += (this.props.isLoading) ? "active " : "disabled "
    classnames += "page dimmer";
    return (
      <div className={classnames}>
        <div className="content">
          <div className="center">
            <h2 className="ui inverted icon header">
              <i className="heart icon"></i>
              Loading...
            </h2>
          </div>
        </div>
      </div>
    );
  }   
}

Loader.propTypes = {
   isLoading: React.PropTypes.bool
};
