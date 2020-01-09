import React from "react";
import ScrollAnimation from "react-animate-on-scroll";
import device from "../../images/device.jpg";
import { Link } from "react-router-dom";

const Details = () => {
  return (
    <div className="details">
      <div className="container">
        <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
          <div className="intro-block">
            Students at the University of Victoria have been working on
            interactive fiction projects about the films in this year's Victoria
            Film Festival.
          </div>
        </ScrollAnimation>
        <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
          <div className="intro-block">
            These stories can be experienced in your web browser, or even better
            on your cellphone. When on your phone they can use your location as
            an element in the stories, creating a simple form of Augmented
            Reality.
          </div>
        </ScrollAnimation>
        <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
          <div className="intro-block">
            You can even author your own stories.
          </div>
        </ScrollAnimation>
        <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
          <div className="intro-block">
            <div>
              <Link to={"/stories"} className="btn btn-secondary">
                View Stories
              </Link>
            </div>
            <div>
              <Link to={"/author"} className="btn btn-secondary">
                Author your own
              </Link>
            </div>
          </div>
        </ScrollAnimation>
      </div>
    </div>
  );
};

export default Details;
