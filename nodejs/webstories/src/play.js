// play.js

import React, { Component } from 'react';
import './play.css';
import { connect } from "react-redux";
import Scene from "./Scene";
import Choices from "./Choices";
import Story from "./Story";
import { makeChoice } from "./state/actions";
import "./App.css";

const Play = props =>
  props.ending ? (
    <div className="ending">🎉🎉 YOU WIN! 🎉🎉</div>
  ) : (
    <div className="Play">
      <Scene tags={props.tags} />
      <Story sceneText={props.sceneText} />
      <Choices choices={props.currentChoices} makeChoice={props.makeChoice} />
    </div>
  );

const stateToProps = state => ({
  tags: state.tags,
  currentChoices: state.currentChoices,
  sceneText: state.sceneText,
  ending: state.ending
});
const dispatchToProps = dispatch => ({
  makeChoice: idx => dispatch(makeChoice(idx))
});

export default connect(stateToProps, dispatchToProps)(Play);
