import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import * as Counter from './Counter';
import * as WeatherForecasts from './WeatherForecasts';
import * as Login from './Login';
import { connectRouter, routerMiddleware  } from 'connected-react-router'

export default function configureStore(history, initialState) {
    const reducers = {
        counter: Counter.reducer,
        weatherForecasts: WeatherForecasts.reducer,
        login: Login.reducer
    };

    const middleware = [
        thunk,
        routerMiddleware(history)
    ];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
        enhancers.push(window.devToolsExtension());
    }

    //const rootReducer = combineReducers({
    //  ...reducers,
    //  routing: routerReducer
    //});

    const createRootReducer = history => combineReducers({
        router: connectRouter(history),
        ...reducers
    });

    return createStore(
        createRootReducer(history),
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}
