// stories.js

import React, { Component, useState } from 'react';
import './stories.css';
const fetch = require('node-fetch');


const Stories = () => {
    const [story, setStory] = useState('stories');


        fetch('/api/stories/')
            .then(response => {console.log(response);
            return response.json();})
            .then((values) => {
                console.log(values);
                setStory(values.toString());
            });


    return (
        <div>
          <h2>stories: {story}</h2>
        </div>
    );

};

export default Stories;