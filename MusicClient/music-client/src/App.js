import React from 'react';
import './App.css';
import Nav from './components/Nav.js'
import Main from './components/Main'
function App() {
  return (
    <div className="outerWrap">
      <div className="App">
       <Nav />
       <Main />
      </div>
      <div className="musicControls">
        
      </div>
    </div>
  );
}

export default App;
