import React from "react";
import ScrollAnimation from "react-animate-on-scroll";
import Typist from "react-typist";

const Exposition = () => {
  return (
    <div className="container exposition">
      <div className="intro-block">
        <Typist avgTypingDelay={30} cursor={{ show: false }}>
          <div className="intro-text">
            Do you remember the choose-your-own-adventure books you read as a
            kid?
          </div>
        </Typist>
        <ScrollAnimation animateIn="fadeIn" animateOnce={true} delay={100}>
          <div className="intro-text">
            They have grown into a genre of literature called Interactive
            Fiction.
          </div>
        </ScrollAnimation>
      </div>
    </div>
  );
};

export default Exposition;
