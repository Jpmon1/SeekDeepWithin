import React from 'react';

export default class Footer extends React.Component {

   constructor(props) {
      super(props);
   }

   render () {
      var year = new Date().getFullYear();
      const end = {margin:'0 1rem'};
      return (<div style={end}><p>&copy; { year } - I in all</p></div>);
   }
   
}
