import React from "react";
import "../css/header.css";
import Typist from "react-typist";

const Header = () => {
  return (
    <header className="masthead">
      <div className="container">
        <div className="intro-text">
          <div className="intro-lead-in">
            In collaboration with VFF and UVic
          </div>
          <div className="intro-heading text-uppercase">VicStories</div>
          <Typist cursor={{ show: false }}>
            <div className="intro-subheading">
              An Interactive Fiction Project
            </div>
          </Typist>
          {/* <a className="btn btn-primary btn-xl text-uppercase" href="#intro">
            Tell Me More
          </a> */}
        </div>
      </div>
    </header>
  );
};

export default Header;
