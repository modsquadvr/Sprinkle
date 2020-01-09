// stories.js

import React, { Component, useState } from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";
import "../css/stories.css";
const fetch = require("node-fetch");

const Stories = () => {
  const [story, setStory] = useState("stories");

  fetch("https://vicstories.ca/api/stories/")
    .then(response => {
      console.log(response);
      return response.json();
    })
    .then(values => {
      console.log(values);
      setStory(values.toString());
    });

  return (
    <div id="stories">
      <div className="container stories">
        <Link to={"/play"} className="nav-link btn btn-secondary btn-lg">
          Play Story
        </Link>
      </div>
    </div>
  );
};

export default Stories;
