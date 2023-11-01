import React from 'react'
import Nav from '../Nav.js'
import { Route, Routes } from 'react-router-dom';
import Categories from "./CategoriesPage";
import PlaylistPage from "./PlaylistPage";
import Header from "../Header";
import UserProfilePage from "./UserProfilePage";
import Footer from '../Footer';
const UserPage = () => {
    return (
        <div className="outerWrap">
            <div className="App">
                <Nav />
                <div className="main">
                    <div className="upperNav"><Header /></div>
                    <div className="mainContent">
                        <Routes>
                            <Route path="/" exact Component={Categories} />
                            <Route path="/search">Search</Route>
                            <Route path="/your-library">Library</Route>
                            <Route path="/playlist/:id" Component={PlaylistPage} />
                            <Route path="/profile" Component={UserProfilePage} />
                        </Routes>

                    </div>
                </div>
            </div>
            <div className="musicControls">
                <Footer />
            </div>
        </div>
    )
}

export default UserPage