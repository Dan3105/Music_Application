import { createSlice } from "@reduxjs/toolkit";
const initialState = {
	favorites: null,
};

export const favoritesSlice = createSlice({
	name: "favorites",
	initialState,
	reducers: {
		clearFavorite: (state) => {
			state.favorites = null;
		},
		setFavorite: (state, action) => {
			state.favorites = action.payload;
		},
	},
});

export const { clearFavorite, setFavorite } = favoritesSlice.actions;

export default favoritesSlice.reducer;
