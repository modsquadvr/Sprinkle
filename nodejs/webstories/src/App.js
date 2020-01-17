import React, { component } from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";
import Play from "./components/Play.js";
import Stories from "./components/Stories.js";
import Author from "./components/Author.js";
import Intro from "./components/Intro/index.js";
import MainNav from "./components/MainNav.js";
import Header from "./components/Header.js";
import "./css/agency.min.css";
import "./css/App.css";

function App() {
  return (
    <Router>
      <div className="App">
        <MainNav />
        <Switch>
          <Route exact path="/" component={Intro} />
          <Route exact path="/play" component={Play} />
          <Route path="/stories" component={Stories} />
          <Route path="/author" component={Author} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
