import React from 'react'
import { useParams } from 'react-router-dom'

const PlaylistPage = () => {
    let { id } = useParams();
    return (
        <div className='playlistPage'>
            <div className='playlistPageInfo'>
                <div className='playlistPageImage'>
                    <img src="https://images.unsplash.com/photo-1587169544748-d21bd810f57e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=934&q=80" alt="pic"/>
                </div>
                <h1>Title</h1>
                <span>Spotify</span>
                <button>Play</button>
                <div className='icon'>
                    <div className='iconHeart'></div>
                    <dic className='iconDots'></dic>
                </div>
                <span>Lorem</span>
            </div>
            <div className='playlistPageSongs'>
                <ul>
                    <li>Song one</li>
                    <li>Song one</li>
                    <li>Song one</li>
                    <li>Song one</li>
                    <li>Song one</li>
                    <li>Song one</li>
                    <li>Song one</li>
                    <li>Song one</li>
                </ul>
            </div>
        </div>
    )
}

export default PlaylistPage