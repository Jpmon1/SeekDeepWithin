import React from 'react';

export default class LightItem extends React.Component {
  constructor(props) {
    super(props);
  }

   /**
    * Renders the Component.
    */
  render() {
    var edit = <span />;
    return (
      <div className="ui raised card" id="c{this.props.item.id}">
        <div className="content">
          <div className="description">
            {this.props.item.text}
          </div>
        </div>
        <div className="extra content">
          <span className="like">
            <i className="like icon"></i> Like
          </span>
          <span className="right floated share">
            <i className="share alternate icon"></i> Share
          </span>
        </div>
      </div>
    );
  }
}

LightItem.propTypes = {
   children: React.PropTypes.array,
   index: React.PropTypes.number,
   item: React.PropTypes.object,
   canEdit: React.PropTypes.bool
};