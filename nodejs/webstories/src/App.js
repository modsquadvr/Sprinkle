import React, {component} from 'react';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';
import Play from './play.js'
import Stories from './stories.js'
import Author from './author.js'
import Intro from './intro.js'
import logo from './images/vicstories.svg';
import './App.css';

function App() {


  return (
    <Router>
    <div className="App">
      <header className="App-header">
        <Link to={'/'} className="nav-link">
          <img src={logo} className="App-logo" alt="logo" />
        </Link>
        <h2>  Welcome to Victoria Stories</h2>
        <nav className="navbar navbar-light bg-light">
          <ul className="navbar-nav mr-auto App-link">
            <li><Link to={'/stories'} className="nav-link">Play Stories</Link></li>
            <li><Link to={'/author'} className="nav-link">Author your own</Link></li>
          </ul>
        </nav>
      </header>
      <Switch>
        <Route exact path='/' component={Intro} />
        <Route exact path='/play' component={Play} />
        <Route path='/stories' component={Stories} />
        <Route path='/author' component={Author} />
      </Switch>
    </div>

    </Router>
  );
}

export default App;
