import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';
import { EventEmitter } from 'events';

const CHANGE_EVENT = 'change';
let _lightStore = {
    list: [],
    isLoading: true
};

class LightStoreClass extends EventEmitter {
   addChangeListener (cb) {
      this.on (CHANGE_EVENT, cb);
   }
   removeChangeListener (cb) {
      this.removeListener (CHANGE_EVENT, cb);
   }
   getData () {
      return _lightStore;
   }
}

const LightStore = new LightStoreClass ();
AppDispatcher.register ((payload) => {
  const action = payload.action;
  switch (action.actionType) {

    case IinAllConstants.GET_RANDOM_RESPONSE:
	  const lights = action.response;
      const len = lights.length;
      for (var i = 0; i < len; i++){
        const newLight = lights[i];
        _lightStore.list.push(newLight);
      }
      _lightStore.isLoading = false;
      LightStore.emit (CHANGE_EVENT);
      break;

    default:
      return true;
   }
});


export default LightStore;
