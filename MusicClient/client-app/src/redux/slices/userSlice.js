import { createSlice } from "@reduxjs/toolkit";

const initialState = {
	user: null,
	// token: null,
};

export const userSlice = createSlice({
	name: "user",
	initialState,
	reducers: {
		loginUser: (state, action) => {
			//state.id = action.payload.id;
			//state.token = action.payload.token;
			// console.log(action);
			state.id = action.payload.id;
			state.email = action.payload.email;
			state.favorites = action.payload.favorites;
		},

		logoutUser: (state) => {
			state.user = null;
			// state.token = null;
		},
		setUser: (state, action) => {
			console.log(action.payload);
			state.user = action.payload;
		},
	},
});

export const { loginUser, logoutUser, setUser } = userSlice.actions;

export default userSlice.reducer;
