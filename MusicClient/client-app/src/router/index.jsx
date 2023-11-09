import { Navigate, createBrowserRouter } from "react-router-dom";
import HomePage from "../pages/HomePage";
import LibraryPage from "../pages/LibraryPage";
import ErrorPage from "../pages/ErrorPage";
import HomeLayout from "../layouts/HomeLayout";
import PlaylistsPage from "../pages/PlaylistsPage";
import PlaylistPage from "../pages/PlaylistPage";
import EditPlaylistPage from "../pages/EditPlaylistPage";
import CreatePlaylistPage from "../pages/CreatePlaylistPage";
import ArtistePage from "../pages/ArtistePage";
import ArtistesPage from "../pages/ArtistesPage";
import FavoritesPage from "../pages/FavoritesPage";

import AuthLayout from "../layouts/AuthLayout";
import LoginPage from "../pages/LoginPage";
import RegisterPage from "../pages/RegisterPage";


export const router = createBrowserRouter([
	{
		path: "/",
		element: <HomeLayout />,
		errorElement: <ErrorPage />,
		children: [
			{ index: true, element: <Navigate to="/home" replace /> },
			{
				path: "/home",
				element: <HomePage />,
			},
			{
				path: "library",
				element: <LibraryPage />,
			},
			{
				path: "playlists",
				element: <PlaylistsPage />,
			},
			{
				path: "playlists/:id",
				element: <PlaylistPage />,
			},
			{
				path: "playlists/create",
				element: <CreatePlaylistPage />,
			},
			{
				path: "playlists/edit/:id",
				element: <EditPlaylistPage />,
			},
			{
				path: "artistes",
				element: <ArtistesPage />,
			},
			{
				path: "artiste/:id",
				element: <ArtistePage />,
			},
			{
				path: "favorites",
				element: <FavoritesPage />,
			},
		],
	},
	{
		path: "/auth",
		element: <AuthLayout />,
		errorElement: <ErrorPage />,
		children: [
			{
				path: "login",
				element: <LoginPage />,
			},
			{
				path: "register",
				element: <RegisterPage />,
			},
		],
	},
]);
