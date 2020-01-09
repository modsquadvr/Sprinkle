// intro.js

import React, { Component } from "react";
import "../../css/intro.css";
import Exposition from "./Exposition";
import Details from "./Details";
import Device from "./Device";
import Header from "../Header";

class Intro extends Component {
  render() {
    return (
      <div>
        <Header />
        <div className="intro" id="intro">
          <Exposition />
          <Details />
          <Device />
        </div>
      </div>
    );
  }
}

export default Intro;
