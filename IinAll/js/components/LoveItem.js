import React, { PropTypes } from 'react';
import sizeMe from 'react-sizeme';
import { requestTruth } from '../actions/DataActions';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';

class LoveItem extends React.Component {
  constructor(props) {
    super(props);
    this.getChildrenTruth = this.getChildrenTruth.bind (this);
  }

  /**
   * Gets the truth associated with this item.
   */
  getChildrenTruth () {
    var loveId = this.props.item.id;
    if (this.props.item.alias != null){
      loveId = this.props.item.alias;
    }
    requestTruth (loveId, this.props.index);
  }

   /**
    * Renders the Component.
    */
  render() {
    var subText = "";
    var mainText = "";
    var lights = this.props.item.light;
    for (var a = 0; a < lights.length; a++) {
      if (a == lights.length - 1) {
        mainText = lights[a].text;
      } else {
        if (subText.length > 0) {
          subText +=" | ";
        }
        subText += lights[a].text;
      }
    }
    var subDisplay = null;
    if (subText.length > 0) {
      subDisplay = <div className="extra content textCenter">{subText}</div>;
    }
    let footers = [];
    var header = null;
    var columns = "small-12 medium-4 large-3";
    var body = this.props.item.body;
    if (body != null) {
      for (var b = 0; b < body.length; b++) {
        if (body[b].position == -2) {
          header = <div className="header">{body[b].text}</div>;
        } else if (body[b].position == -1) {
          if (this.props.item.type == "truth") {
            columns = "small-12"
          }
          mainText = <div className="description">
            <div className="row">
              <div className="small-1 columns">{body[b].text}</div>
              <div className="small-11 columns">{mainText}</div>
            </div>
          </div>;
        } else if (body[b].position == -3) {

        } else {
          footers.push(<div className="extra content textCenter">{body[b].text}</div>);
        }
      }
    }
    var click = null;
    if (!this.props.item.isSelected){
      click = ()=>this.getChildrenTruth();
    }
    return (
        <div ref="col" className={columns + " columns"} id={'c' + this.props.item.id} onClick={click}>
          <div className="love">
            <div className="content">
              {header}
              {mainText}
            </div>
            {footers}
            {subDisplay}
          </div>
        </div>
    );
        /*<div className="extra content">
          <span className="like">
            <i className="like icon"></i> Like
          </span>
          <span className="right floated share">
            <i className="share alternate icon"></i> Share
          </span>
        </div>*/
  }
}

LoveItem.propTypes = {
   children: React.PropTypes.array,
   index: React.PropTypes.number,
   item: React.PropTypes.object,
   size: PropTypes.shape({
    width: PropTypes.number.isRequired,
    height: PropTypes.number.isRequired,
  })
};

export default sizeMe()(LoveItem);

/*const LoveItemComponent = sizeMe()(LoveItem);

// We create this wrapper component so that our size aware rendering
// will have a handle on the 'className'.
function LoveItemWrapper(props) {
  return (
    <LoveItemComponent className={cssStyles.foo} {...props} />
  );
}

export default LoveItemWrapper;*/