import React, { Component } from "react";
import logo from "../images/vicstories.svg";
import { Link } from "react-router-dom";
import "../css/mainnav.css";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";

class MainNav extends Component {
  constructor(props) {
    super(props);
    this.state = {
      shrink: false
    };
  }
  componentDidMount() {
    window.addEventListener("scroll", this.handleScroll);
  }

  handleScroll = event => {
    this.setState({ shrink: window.scrollY > 200 });
  };

  render() {
    return (
      <Navbar
        expand="lg"
        id="mainNav"
        variant="dark"
        fixed="top"
        className={this.state.shrink ? "navbar-shrink" : ""}
      >
        <div className="container">
          <Link to={"/"} className="navbar-brand">
            <img src={logo} className="App-logo" alt="logo" />
          </Link>
          <Navbar.Toggle aria-controls="navbarResponsive" />
          <Navbar.Collapse id="navbarResponsive">
            <Nav className="ml-auto text-uppercase">
              <li className="nav-item">
                <Link to={"/stories"} className="nav-link">
                  View Stories
                </Link>
              </li>
              <li className="nav-item">
                <Link to={"/author"} className="nav-link">
                  Author your own
                </Link>
              </li>
            </Nav>
          </Navbar.Collapse>
        </div>
      </Navbar>
    );
  }
}

export default MainNav;
