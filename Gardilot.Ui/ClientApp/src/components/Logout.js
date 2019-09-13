import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Login';

class Logout extends Component {
    handleonLogoutClick = event => {
        event.preventDefault()
        this.props.logoutUser()
    }

    render() {
        return (
            <button onClick={this.handleonLogoutClick} className="btn btn-primary">
                Logout
      </button>
        )
    }
}

export default connect(
    null,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Logout);
