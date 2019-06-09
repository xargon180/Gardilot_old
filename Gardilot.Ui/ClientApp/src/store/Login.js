const loginUserType = 'LOGIN_USER';
const logoutUserType = 'LOGOUT_USER';
const initialState = { token: null, currentUser: {} };

export const actionCreators = {
    requestLogin: user => async (dispatch, getState) => {
        //dispatch({ type: loginUserType, user: user });

        const url = 'api/Auth/Login';
        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                Accept: 'application/json',
            },
            body: JSON.stringify(user)
        });
        const responseData = await response.json();

        if (responseData.token) {
            localStorage.setItem("token", responseData.token)
            dispatch({ type: loginUserType, user: user, token: responseData.token });
        }
        else {
            dispatch({ type: logoutUserType });
        }
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === loginUserType) {
        return {
            ...state,
            currentUser: action.user,
            token: action.token
        };
    }

    if (action.type === logoutUserType) {
        return {
            ...initialState
        };
    }

    return state;
};
