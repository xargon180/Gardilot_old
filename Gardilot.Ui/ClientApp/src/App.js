import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import Login from './components/Login';
import FetchData from './components/FetchData';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/counter' component={Counter} />
    <Route path='/login' component={Login} />
    <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
  </Layout>
);
