import React from 'react'
import { ReactComponent as Playicon } from '../svgs/play.svg'
import dataPlaylists from '../config/playlist_data'
import { Link } from 'react-router-dom'

const Playlists = (props) => {

    const matchPlaylist = dataPlaylists.filter(playlist => playlist.category_id === props.category_id)
    return (
        <div>
            <div className="cardsWrapInner">
                {matchPlaylist.map((playist, id) => (
                    <Link to={`/playlist/${playist.id}`} >

                    <div className="card">
                        <div className="cardImage" key={id}>
                            <img src={playist.img} alt="Pic"></img>
                        </div>
                        <div className="cardContent">
                            <h3>{playist.name}</h3>
                            <span>Music to help you concentrate</span>
                        </div>
                        <span className="playIcon">
                            <Playicon />
                        </span>
                    </div></Link>
                    ))}
        </div>
        </div >
    )
}

export default Playlists