// stories.js

import React, { Component, useState } from 'react';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';
import './stories.css';
const fetch = require('node-fetch');


const Stories = () => {
    const [story, setStory] = useState('stories');


        fetch('https://vicstories.ca/api/stories/')
            .then(response => {console.log(response);
            return response.json();})
            .then((values) => {
                console.log(values);
                setStory(values.toString());
            });


    return (
        <div>
          <h2>stories: {story}</h2>

          <li><Link to={'/play'} className="nav-link">Play Story</Link></li>
        </div>
    );

};

export default Stories;