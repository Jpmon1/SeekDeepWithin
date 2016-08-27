import React from 'react';

export default class Search extends React.Component {

   constructor(props) {
      super(props);
   }
   
   render () {
      return (
         <div>
            <div className="row">
               <div className="small-12 medium-4 medium-offset-4 large-4 large-offset-4 column text-center">
                  <h4 className="light-text">I in All</h4>
                  <hr />
               </div>
            </div>
            <div className="row column text-center seek">
               <h1>Seek, and ye shall find...</h1>
            </div>
         </div>
      );
   }
   
}
