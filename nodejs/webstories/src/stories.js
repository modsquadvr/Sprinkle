// stories.js

import React, { Component } from 'react';
import './stories.css';
var fs = require('fs');
var tmp = "foo";

class stories extends Component {


  render() {
   // fs.readFile('../compiledStories/intercept.ink.json', function (err, data) {
     //   if (err) throw err;

//console.log(data);
//});
   // let dirCont = fs.readdirSync( '../compiledStories' );
   // let files = dirCont.filter(dirCont => '*.json');

console.log(tmp);
   // console.log(files)
   //let tmp = fs.readdirSync('.');
    return (
        <div>
          <h2>stories</h2>
        </div>
    );
  }
}

export default stories;