import { useEffect, useState } from "react";
import PlaylistCard from "../components/PlaylistCard";
import { Box, Flex, Grid, Heading, Text } from "@chakra-ui/react";
import CreatePlaylistCard from "../components/CreatePlaylistCard";
import { client } from "../api";
import { AiOutlineLoading } from "react-icons/ai";
import { MdErrorOutline } from "react-icons/md";
import { useSelector } from "react-redux";
import AlbumCard from "../components/AlbumCard";

const AlbumsPage = () => {
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState(false);
	const [albums, setAlbums] = useState([]);
	const { user } = useSelector((state) => state.user);

	const fetchAlbums = async () => {
		setLoading(true);
		setError(false);
		await client
			.get(`/Album`)
			.then((res) => {
				setLoading(false);
				setAlbums(res.data);

			})
			.catch((e) => {
				console.log(e);
				setLoading(false);
				setError(true);
			});
	};

	useEffect(() => {
		fetchAlbums();

	}, []);

	if (error) {
		return (
			<Flex align="center" justify="center" minH="100vh">
				<Flex direction="column" align="center" color="accent.light">
					<MdErrorOutline color="inherit" size={32} />
					<Text color="zinc.400" textAlign="center" mt={2}>
						An error occured while fetching Album.
					</Text>
				</Flex>
			</Flex>
		);
	}

	return (
		<Box p={6} pb={32} pt={{ base: 20, md: 6 }} pl={{ base: 4, md: 14, xl: 10 }}>
			<Box>
				<Heading
					as="h2"
					fontSize={{ base: "lg", md: "2xl" }}
					fontWeight="semibold"
					mb={{ base: 1, md: 3 }}>
					Albums
				</Heading>
				<Text color="zinc.400" fontSize="sm">
					Here are some new Albums.
				</Text>
			</Box>
			{loading && albums.length < 1 && (
				<Flex align="center" justify="center" color="accent.main" minH="20rem">
					<AiOutlineLoading className="spin" size={36} />
				</Flex>
			)}
			{!loading && !error && (
				<Grid
					templateColumns={{
						base: "repeat(2, 1fr)",
						md: "repeat(3, 1fr)",
						lg: "repeat(4, 1fr)",
						xl: "repeat(5, 1fr)",
					}}
					gap={5}
					mt={10}>
					{albums.map((album) => (
						<AlbumCard key={album?.id} album={album} />
					))}
				</Grid>
			)}
		</Box>
	);
};

export default AlbumsPage;
