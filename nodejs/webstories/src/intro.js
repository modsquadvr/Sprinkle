// intro.js

import React, { Component } from 'react';
import './intro.css';

class intro extends Component {
  render() {
    return (
        <div className='intro'>
          <h2>intro</h2>
          <p>Do you remember the choose-your-own-adventure books you read as a kid? They have grown into a genre of literature called Interactive Fiction. Students at the University of Victoria have been working on interactive fiction projects about the films in this year's Victoria Film Festival. These stories can be experienced in your web browser, or even better on your cellphone. When on your phone they can use your location as an element in the stories, creating a simple form of Augmented Reality. You can even author your own stories.
            </p>
            <p>To get started, click on the Play Stories link above. When you have played a few try writing one of your own!</p>
        </div>
    );
  }
}

export default intro;