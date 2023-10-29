import React from 'react'
import dataCategories from '../config/categories_data'
import Playlists from './Playlists'
const Categories = () => {
    return (
        <div>
            {dataCategories.map((category) => (
                <div className="cardsWrap" key={category.id}>
                    <h2>{category.name}</h2>
                    <p className="subText">{category.tagline}</p>
                    <Playlists category_id={category.id}/>
                </div>
            ))}
        </div>
    )
}

export default Categories