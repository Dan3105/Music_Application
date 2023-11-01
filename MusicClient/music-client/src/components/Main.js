import React from "react";
import Categories from "./pages/CategoriesPage";
import PlaylistPage from "./pages/PlaylistPage";
import { Routes, Route} from 'react-router-dom';
import Header from "./Header";
import UserProfilePage from "./pages/UserProfilePage";
const Main = () => {
  return (
    <div className="main">
      <div className="upperNav"><Header/></div>
      <div className="mainContent">
        <Routes>
          <Route path="/" exact Component={Categories }/>
          <Route path="/search">Search</Route>
          <Route path="/your-library">Library</Route>
          <Route path="/playlist/:id" Component={PlaylistPage} />
          <Route path="/profile" Component={UserProfilePage } />
        </Routes>
        
      </div>
    </div>
  )
}

export default Main;