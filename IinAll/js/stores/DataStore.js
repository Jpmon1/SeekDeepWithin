import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';
import { EventEmitter } from 'events';

const CHANGE_EVENT = 'change';
let _dataStore = {
    list: [],
    isLoading: true
};

class DataStoreClass extends EventEmitter {
   addChangeListener (cb) {
      this.on (CHANGE_EVENT, cb);
   }
   removeChangeListener (cb) {
      this.removeListener (CHANGE_EVENT, cb);
   }
   getData () {
      return _dataStore;
   }
}

const DataStore = new DataStoreClass ();
AppDispatcher.register ((payload) => {
  const action = payload.action;
  switch (action.actionType) {

    case IinAllConstants.GET_RANDOM:
    case IinAllConstants.GET_SEARCH:
    case IinAllConstants.GET_TRUTH:
      _dataStore.isLoading = true;
      DataStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.GET_TRUTH_RESPONSE:
      const truths = action.response.truths;
      let newItems = [];
      for (const truth in truths){
        var itemT = truths[truth];
        newItems.push ({
          type:  "truth",
          id:    itemT.id,
          light: itemT.l,
          alias: itemT.a,
          body:  itemT.b,
          isSelected: false,
          key: _dataStore.list.length + newItems.length
        });
      }
      _dataStore.list[action.index].isSelected = true;
      var index = action.index + 1;
      var args = [index, 0].concat(newItems);
      Array.prototype.splice.apply(_dataStore.list, args);
      _dataStore.isLoading = false;
      DataStore.emit (CHANGE_EVENT);
      break;

    case IinAllConstants.GET_SEARCH_RESPONSE:
    case IinAllConstants.GET_RANDOM_RESPONSE:
      const response = action.response;
      const loves = response.love;
      for (const love in loves){
        var itemL = loves[love];
        _dataStore.list.push({
          type:  "love",
          id:    itemL.id,
          light: itemL.l,
          alias: itemL.a,
          body:  itemL.b,
          isSelected: false,
          key: _dataStore.list.length
        });
      }
      _dataStore.isLoading = false;
      DataStore.emit (CHANGE_EVENT);
      break;

    default:
      return true;
   }
});


export default DataStore;
