import React from 'react';
import ReactDOM from 'react-dom';
import IinAll from './components/IinAll';
import injectTapEventPlugin from 'react-tap-event-plugin';
 
// Needed for onTouchTap 
// http://stackoverflow.com/a/34015469/988941 
injectTapEventPlugin();
ReactDOM.render(<IinAll />, document.getElementById('appContent'));
