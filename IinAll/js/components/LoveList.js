import React from 'react';
import LoveItem from './LoveItem';
import { Throttle } from '../api/Utils';

class LoveList extends React.Component {
  /**
   * Initializes the love list.
   */
  constructor(props) {
    super(props);
    this.items = [];
    this.columns = [];
    this.transform = this.getSupportedTransform ();
    window.onresize = () => Throttle (this.componentDidUpdate(), 300);
  }

  componentDidMount () {
    this.onNextFrame (() => this.layoutItems());
  }

  componentDidUpdate () {
    this.onNextFrame (() => this.layoutItems());
  }

  onNextFrame(callback) {
    setTimeout(function () { window.requestAnimationFrame(callback) }, 0);
  }

  getSupportedTransform() {
    var prefixes = 'transform WebkitTransform MozTransform OTransform msTransform'.split(' ');
    var div = document.createElement('div');
    for(var i = 0; i < prefixes.length; i++) {
      if(div && div.style[prefixes[i]] !== undefined) {
        return prefixes[i];
      }
    }
    return false;
  }

  layoutItems (){
    var i = 0;
    var totalHeight = 0;
    var container = this.refs["container"];
    var totalWidth = container.offsetWidth + 4;
    this.columns = [{top: 0, left: 0, right: totalWidth, bottom: 0, width: totalWidth, index: 0}];
    for (const item in this.items) {
      var ref = "item" + this.items[item].props.item.key;
      var domEl = this.refs[ref].domEl;
      var width = domEl.offsetWidth;
      var height = domEl.offsetHeight;
      var postion = this.getPosition (width, height, totalWidth);
      if (width <=0 || height <= 0)
        console.log("width or height is zero");
      domEl.style.position = 'absolute';
      if (this.transform === false) {
        domEl.style.top = postion.top + 'px';
        domEl.style.left = postion.left + 'px';
      } else {
        if (domEl.className.indexOf("loveContainer") == -1) {
          domEl.className += " loveContainer";
        }
        domEl.style.transform = "translate3d(" + postion.left +"px, " + postion.top + "px, 0)";
      }
      totalHeight = postion.top + height;
      i++;
    }
    if (i > 0){
      container.style.position = 'relative';
      container.style.height = totalHeight + 'px';
    }
  }

  getPosition (width, height, totalWidth) {
    var orderedColumns = this.columns.slice(0);
    orderedColumns.sort (function (a, b) {
      if (a.bottom < b.bottom) { return -1; }
      if (a.bottom > b.bottom) { return 1; }
      return 0;
    });
    var result, columnIndex = -1;
    var columnCount = orderedColumns.length;
    for (var i = 0; i < columnCount; i++) {
      var column = orderedColumns[i];
      columnIndex = column.index;
      result = this.sideSearch (1, width, column, columnCount);
      if (!result.fits) { result = this.sideSearch (-1, width, column, columnCount); }
      if (result.fits) { break; }
      columnIndex = -1;
    }
    if (columnIndex == -1) {
      var lowestColumn = orderedColumns[columnCount - 1];
      columnIndex = 0;
      columnCount = 1;
      this.columns = [{top: lowestColumn.bottom, left: 0, right: totalWidth, bottom: lowestColumn.bottom, width: totalWidth, index: 0}];
    }
    var column = this.columns[columnIndex];
    if (result.span > 1 && result.fits) {
      var removed = result.span - 1;
      if (result.dir == -1) {
        columnIndex -= removed;
        this.columns[columnIndex].bottom = column.bottom;
        column = this.columns[columnIndex];
      }
      if (column.left + width < this.columns[columnIndex + removed].right) {
        this.columns[columnIndex + removed].left = column.left + width;
        removed--;
      }
      this.columns.splice (columnIndex + 1, removed);
      for (var a = columnIndex + 1; a < columnCount - removed; a++) {
        this.columns[a].index -= removed;
      }
    } else if (width < column.width) {
      for (var a = columnIndex + 1; a < columnCount; a++) {
        this.columns[a].index++;
      }
      var newColumn = {
        top: column.top,
        left: column.left + width,
        right: column.right,
        bottom: column.bottom,
        index: columnIndex + 1,
        width: column.width - width
      };
      column.width = width;
      column.right = column.left + width;
      this.columns.splice (columnIndex + 1, 0, newColumn);
    }
    this.columns[columnIndex] = {
      top: column.bottom,
      left: column.left,
      right: column.left + width,
      bottom: column.bottom + height,
      index: columnIndex,
      width: width
    };
    column = this.columns[columnIndex];
    return {top: column.top, left: column.left};
  }

  checkColumn (width, height, totalWidth) {
    var orderedColumns = this.columns.slice(0);
    orderedColumns.sort (function (a, b) {
      if (a.bottom < b.bottom) { return -1; }
      if (a.bottom > b.bottom) { return 1; }
      return 0;
    });
    var result, columnIndex = -1;
    var columnCount = orderedColumns.length;
    for (var i = 0; i < columnCount; i++) {
      var column = orderedColumns[i];
      columnIndex = column.index;
      result = this.sideSearch (1, width, column, columnCount);
      if (!result.fits) { result = this.sideSearch (-1, width, column, columnCount); }
      if (result.fits) { break; }
      columnIndex = -1;
    }
    if (columnIndex == -1) {
      var lowestColumn = orderedColumns[columnCount - 1];
      columnIndex = 0;
      columnCount = 1;
      this.columns = [{top: lowestColumn.bottom, left: 0, right: totalWidth, bottom: lowestColumn.bottom, width: totalWidth, index: 0}];
    }
    var column = this.columns[columnIndex];
    if (result.span > 1 && result.fits) {
      var removed = result.span - 1;
      if (result.dir == -1) {
        columnIndex -= removed;
        this.columns[columnIndex].bottom = column.bottom;
        column = this.columns[columnIndex];
      }
      if (column.left + width < this.columns[columnIndex + removed].right) {
        this.columns[columnIndex + removed].left = column.left + width;
        removed--;
      }
      this.columns.splice (columnIndex + 1, removed);
      for (var a = columnIndex + 1; a < columnCount - removed; a++) {
        this.columns[a].index -= removed;
      }
    } else if (width < column.width) {
      for (var a = columnIndex + 1; a < columnCount; a++) {
        this.columns[a].index++;
      }
      var newColumn = {
        top: column.top,
        left: column.left + width,
        right: column.right,
        bottom: column.bottom,
        index: columnIndex + 1,
        width: column.width - width
      };
      column.width = width;
      column.right = column.left + width;
      this.columns.splice (columnIndex + 1, 0, newColumn);
    }
    this.columns[columnIndex] = {
      top: column.bottom,
      left: column.left,
      right: column.left + width,
      bottom: column.bottom + height,
      index: columnIndex,
      width: width
    };
    return columnIndex;
  }

  sideSearch (dir, width, column, columnCount) {
    var span = 1;
    var fits = true;
    var countIndex = column.index;
    var columnWidth = column.right - column.left;
    while (columnWidth + 4 < width) {
      var nextIndex = countIndex + dir;
      if (nextIndex < 0 || nextIndex >= columnCount){
        fits = false;
        break;
      }
      var nextWidth = this.columns [nextIndex].right - this.columns [nextIndex].left;
      var nextBottom = this.columns [nextIndex].bottom;
      if (column.bottom < nextBottom) {
        fits = false;
        break;
      }
      span++;
      countIndex += dir;
      columnWidth += nextWidth;
    }
    return {fits: fits, span: span, dir: dir};
  }

  /**
   * Renders the love list.
   */
  render() {
    if (this.items.length != this.props.children.length) {
      this.items = [];
      if (this.props.children) {
        var index = 0;
        this.props.children.map((item, index) => {
          this.items.push(<LoveItem item={item} index={index} key={item.key} size={{width:0, height:0}} ref={'item' + item.key} />);
          index++;
        });
      }
    }

    return (
      <div className="pLeft pRight">
        <div className="expanded row" ref="container">
          {this.items}
        </div>
      </div>
    );
  }
}

LoveList.propTypes = {
  children: React.PropTypes.array,
  userData: React.PropTypes.object
};

export default LoveList;