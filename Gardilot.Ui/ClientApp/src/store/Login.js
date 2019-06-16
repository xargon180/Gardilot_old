import { push } from 'react-router-redux'

const loginRequestType = 'LOGIN_REQUEST'
const loginSuccessType = 'LOGIN_SUCCESS'
const loginFailureType = 'LOGIN_FAILURE'

const logoutRequestType = 'LOGOUT_REQUEST'
const logoutSuccessType = 'LOGOUT_SUCCESS'
const logoutFailureType = 'LOGOUT_FAILURE'

// TODO: Check if token in store is expired
const initialState = { isFetching: false, isAuthenticated: localStorage.getItem('token') ? true : false, token: null, userName: null };

export const actionCreators = {
    requestLogin: creds => async (dispatch, getState) => {
        dispatch({ type: loginRequestType, creds });

        const url = 'api/Auth/Login';
        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                Accept: 'application/json',
            },
            body: JSON.stringify(creds)
        });
        const responseData = await response.json();

        if (responseData.token) {
            localStorage.setItem("token", responseData.token)
            dispatch({ type: loginSuccessType, creds, token: responseData.token });
            dispatch(push('/'))
        }
        else {
            dispatch({ type: loginFailureType, message: 'Login failed' });
        }
    },
    logoutUser: () => (dispatch, getState) => {
        dispatch({ type: logoutRequestType });

        localStorage.removeItem('token')

        dispatch({ type: logoutSuccessType })
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === loginRequestType) {
        return {
            ...state,
            isFetching: true,
            isAuthenticated: false,
            userName: action.creds.userName
        };
    }

    if (action.type === loginSuccessType) {
        return {
            ...state,
            isFetching: false,
            isAuthenticated: true,
            errorMessage: '',
            userName: action.creds.userName,
            token: action.token
        };
    }

    if (action.type === loginFailureType) {
        return {
            ...state,
            isFetching: false,
            isAuthenticated: false,
            errorMessage: action.message
        };
    }




    if (action.type === logoutRequestType) {
        return {
            ...state,
            isFetching: true,
            isAuthenticated: true
        };
    }

    if (action.type === logoutSuccessType) {
        return {
            ...state,
            isFetching: false,
            isAuthenticated: false,
        };
    }

    if (action.type === logoutFailureType) {
        return {
            ...state,
            isFetching: false,
            isAuthenticated: false,
            errorMessage: action.message
        };
    }

    return state;
};
