// intro.js

import React, { Component } from 'react';
import './intro.css';
import device from './images/device.jpg';

class Intro extends Component {
  render() {
    return (
        <div className='intro'>
          <h2>intro</h2>
          <p>Do you remember the choose-your-own-adventure books you read as a kid? They have grown into a genre of literature called Interactive Fiction. Students at the University of Victoria have been working on interactive fiction projects about the films in this year's Victoria Film Festival. These stories can be experienced in your web browser, or even better on your cellphone. When on your phone they can use your location as an element in the stories, creating a simple form of Augmented Reality. You can even author your own stories.
            </p>
            <p>To get started, click on the Play Stories link above. When you have played a few try writing one of your own!</p>
            <p>
              When playing a story on the web, there will be a checkbox to allow you tell the story where you are. When playing on the phone, though, the small devices you see below will allow your phone to get a signal telling it you are in a special location and story information specific to that place can be shown! These devices can run for a month or two on the rechargeable battery and are small enough to be stuck pretty much anywhere.
            </p>


            <img src={device} className="device_image" alt="device" />
        </div>
    );
  }
}

export default Intro;