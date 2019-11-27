import React, {component} from 'react';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';
import play from './play.js'
import stories from './stories.js'
import author from './author.js'
import intro from './intro.js'
import logo from './vicstories.svg';
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
        <Route exact path='/' component={intro} />
        <Route exact path='/play' component={play} />
        <Route path='/stories' component={stories} />
        <Route path='/author' component={author} />
      </Switch>
    </div>

    </Router>
  );
}

export default App;
