import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Login';


class Login extends Component {
    state = {
        userName: "",
        password: ""
    }

    handleChange = event => {
        this.setState({
            [event.target.name]: event.target.value
        });
    }

    handleSubmit = event => {
        event.preventDefault()
        this.props.requestLogin(this.state)
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <h1>Login</h1>

                <label>Username</label>
                <input
                    name='userName'
                    placeholder='Username'
                    value={this.state.userName}
                    onChange={this.handleChange}
                /><br />

                <label>Password</label>
                <input
                    type='password'
                    name='password'
                    placeholder='Password'
                    value={this.state.password}
                    onChange={this.handleChange}
                /><br />

                <input type='submit' />
            </form>
        )
    }
}

export default connect(
    null,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Login);
