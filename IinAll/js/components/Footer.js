import React from 'react';

export default class Footer extends React.Component {

   constructor(props) {
      super(props);
   }

   render () {
      var year = new Date().getFullYear();
      return (
         <div className="end-stuff">
            <p>&copy; { year } - I in all</p>
         </div>
      );
   }
   
}
