import React from 'react'
import '../css/header.css'
import SearchIcon from '@material-ui/icons/Search';
import { Avatar } from "@material-ui/core";
import { Link } from 'react-router-dom'
import Login from './Login'
function Header() {
  return (
    <div className="header">
      <div className="header__left">
        <SearchIcon />
        <input
          placeholder='Search for Artists, Songs or Podcasts'
          type="text" />
      </div>
      <div className="header__right">
        <h4>user.display_name</h4>
        <Link to="profile"><Avatar src="https://ih1.redbubble.net/image.4978239506.3232/bg,f8f8f8-flat,750x,075,f-pad,750x1000,f8f8f8.jpg" alt="" /></Link>
        
        <Login />
      </div>
    </div>
  )
}

export default Header