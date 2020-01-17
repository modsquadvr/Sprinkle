import React from "react";
import ScrollAnimation from "react-animate-on-scroll";
import device from "../../images/device.jpg";

const Device = () => {
  return (
    <div className="container device">
      <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
        <div className="intro-block">
          When playing a story on the web, there will be a checkbox to allow you
          tell the story where you are. When playing on the phone, though, the
          small devices you see below will allow your phone to get a signal
          telling it you are in a special location and story information
          specific to that place can be shown!
        </div>
      </ScrollAnimation>
      <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
        <div className="intro-block">
          These devices can run for a month or two on the rechargeable battery
          and are small enough to be stuck pretty much anywhere.
        </div>
      </ScrollAnimation>
      <ScrollAnimation animateIn="fadeIn" animateOnce={true}>
        <img src={device} className="device_image" alt="device" />
      </ScrollAnimation>
    </div>
  );
};

export default Device;
