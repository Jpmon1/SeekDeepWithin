import React from 'react';
import LightItem from './LightItem';

export default class LightList extends React.Component {
  /**
   * Initializes the light list.
   */
  constructor(props) {
    super(props);
  }

  /**
   * Renders the light list.
   */
  render() {
    let items = [];
    if (this.props.children) {
      this.props.children.map((item, index) => {
        items.push(<LightItem item={item} index={index} key={item.id} canEdit={this.props.userData.canUserEdit} />);
      });
    }

    return (
      <div className="pLeft pRight">
        <div className="ui four doubling cards">
          {items}
        </div>
      </div>
    );
  }
}

LightList.propTypes = {
  children: React.PropTypes.array,
  userData: React.PropTypes.object
};