import React from "react";
import Categories from "./Categories";
import PlaylistPage from "./pages/PlaylistPage";
import { Routes, Route} from 'react-router-dom';

const Main = () => {
  return (
    <div className="main">
      <div className="upperNav">Heheheh</div>
      <div className="mainContent">
        <Routes>
          <Route path="/" exact Component={Categories }></Route>
          <Route path="/search">Search</Route>
          <Route path="/your-library">Library</Route>
          <Route path="/playlist/:id" Component={PlaylistPage}>Playlist</Route>
        </Routes>
        
      </div>
    </div>
  )
}

export default Main;