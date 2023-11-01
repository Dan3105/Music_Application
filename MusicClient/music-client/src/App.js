import React from 'react';
import './App.scss';
import Nav from './components/Nav.js'
import Main from './components/Main'
import Footer from './components/Footer';
import { Route, Routes } from 'react-router-dom';
function App() {
  return (
    <div>
      <Routes>
        <Route index path="/">
          <div className="outerWrap">
            <div className="App">
              <Nav />
              <Main />
            </div>
            <div className="musicControls">
              <Footer />
            </div>
          </div>
        </Route>
      </Routes>
    </div>

  );
}

export default App;
