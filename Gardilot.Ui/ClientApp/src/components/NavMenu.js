import React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import './NavMenu.css';

export class NavMenu extends React.Component {
    constructor(props) {
        super(props);

        this.toggle = this.toggle.bind(this);
        this.state = {
            isOpen: false
        };
    }
    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
    render() {
        const { isAuthenticated, userName } = this.props

        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light >
                    <Container>
                        <NavbarBrand tag={Link} to="/">Gardilot.Ui</NavbarBrand>
                        <NavbarToggler onClick={this.toggle} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
                                </NavItem>
                                {!isAuthenticated &&
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                                    </NavItem>
                                }
                                {isAuthenticated &&
                                    <NavLink tag={Link} className="text-dark" to="/logout">Logout {userName}</NavLink>
                                }
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}

export default connect(
    state => state.login
)(NavMenu);
