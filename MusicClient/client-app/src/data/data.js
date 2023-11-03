let song_db = [
  {
    "_id": 1,
    "title": "Sample Song 1",
    "duration": "3:45",
    "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
    "artistes": ["Artist A", "Artist B"],
    "songUrl": "https://example.com/song1.mp3"
  },
  {
    "_id": 2,
    "title": "Sample Song 2",
    "duration": "4:10",
    "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
    "artistes": ["Artist C"],
    "songUrl": "https://example.com/song2.mp3"
  },
  {
    "_id": 3,
    "title": "Sample Song 3",
    "duration": "2:55",
    "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
    "artistes": ["Artist D", "Artist E"],
    "songUrl": "https://example.com/song3.mp3"
  },
  {
    "_id": 4,
    "title": "Sample Song 4",
    "duration": "3:30",
    "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
    "artistes": ["Artist F"],
    "songUrl": "https://example.com/song4.mp3"
  },
  {
    "_id": 5,
    "title": "Sample Song 5",
    "duration": "5:15",
    "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
    "artistes": ["Artist G"],
    "songUrl": "https://example.com/song5.mp3"
  }
]

let artiste_db = [
  {
    "_id": 1,
    "name": "Artist A",
    "image": "https://www.forbes.com/advisor/wp-content/uploads/2023/09/getty_creative.jpeg.jpg",
    "type": "Artiste",
    "bio": "Artist A's bio..."
  },
  {
    "_id": 2,
    "name": "Artist B",
    "image": "https://www.forbes.com/advisor/wp-content/uploads/2023/09/getty_creative.jpeg.jpg",
    "type": "Artiste",
    "bio": "Artist B's bio..."
  },
  {
    "_id": 3,
    "name": "Artist C",
    "image": "https://www.forbes.com/advisor/wp-content/uploads/2023/09/getty_creative.jpeg.jpg",
    "type": "Artiste",
    "bio": "Artist C's bio..."
  },
  {
    "_id": 4,
    "name": "Artist D",
    "image": "https://www.forbes.com/advisor/wp-content/uploads/2023/09/getty_creative.jpeg.jpg",
    "type": "Artiste",
    "bio": "Artist D's bio..."
  },
  {
    "_id": 5,
    "name": "Artist E",
    "image": "https://www.forbes.com/advisor/wp-content/uploads/2023/09/getty_creative.jpeg.jpg",
    "type": "Artiste",
    "bio": "Artist E's bio..."
  }
]

let playlist_db = [
  {
    "_id": 1,
    "title": "Playlist 1",
    "description": "My favorite songs",
    "userId": "user1",
    "userName": "User One",
    "songs": [

    ],
    "isPrivate": false
  },
  {
    "_id": 2,
    "title": "Playlist 2",
    "description": "Cool music",
    "userId": "user2",
    "userName": "User Two",
    "songs": [

    ],
    "isPrivate": true
  },
  {
    "_id": 3,
    "title": "Playlist 3",
    "description": "Chill vibes",
    "userId": "user3",
    "userName": "User Three",
    "songs": [
      {
        "_id": 1,
        "title": "Sample Song 1",
        "duration": "3:45",
        "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
        "artistes": ["Artist A", "Artist B"],
        "songUrl": "https://example.com/song1.mp3"
      },
      {
        "_id": 2,
        "title": "Sample Song 2",
        "duration": "4:10",
        "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
        "artistes": ["Artist C"],
        "songUrl": "https://example.com/song2.mp3"
      },
      {
        "_id": 3,
        "title": "Sample Song 3",
        "duration": "2:55",
        "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
        "artistes": ["Artist D", "Artist E"],
        "songUrl": "https://example.com/song3.mp3"
      },
      {
        "_id": 4,
        "title": "Sample Song 4",
        "duration": "3:30",
        "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
        "artistes": ["Artist F"],
        "songUrl": "https://example.com/song4.mp3"
      }
    ],
    "isPrivate": false
  },
  {
    "_id": 4,
    "title": "Playlist 4",
    "description": "Relaxing tunes",
    "userId": "user4",
    "userName": "User Four",
    "songs": [

    ],
    "isPrivate": true
  },
  {
    "_id": 5,
    "title": "Playlist 5",
    "description": "Party favorites",
    "userId": "user5",
    "userName": "User Five",
    "songs": [

    ],
    "isPrivate": false
  }
]

const favorites_db = [{
  "_id": 1,
  "title": "Sample Song 1",
  "duration": "3:45",
  "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
  "artistes": ["Artist A", "Artist B"],
  "songUrl": "https://example.com/song1.mp3"
},
{
  "_id": 2,
  "title": "Sample Song 2",
  "duration": "4:10",
  "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
  "artistes": ["Artist C"],
  "songUrl": "https://example.com/song2.mp3"
},
{
  "_id": 3,
  "title": "Sample Song 3",
  "duration": "2:55",
  "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
  "artistes": ["Artist D", "Artist E"],
  "songUrl": "https://example.com/song3.mp3"
},
{
  "_id": 4,
  "title": "Sample Song 4",
  "duration": "3:30",
  "coverImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/RedCat_8727.jpg/1200px-RedCat_8727.jpg",
  "artistes": ["Artist F"],
  "songUrl": "https://example.com/song4.mp3"
}]

export { song_db, artiste_db, playlist_db, favorites_db };