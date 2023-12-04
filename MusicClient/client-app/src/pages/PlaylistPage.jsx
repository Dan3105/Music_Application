import { useEffect, useState, useRef } from "react";
import { client } from "../api";
import { useParams, Link } from "react-router-dom";
import { MdErrorOutline } from "react-icons/md";
import {
	Box,
	Button,
	Divider,
	Flex,
	Heading,
	Image,
	Text,
} from "@chakra-ui/react";
import ArtisteSong from "../components/ArtisteSong";
import LoadingSkeleton from "../components/LoadingSkeleton";
import { useDispatch, useSelector } from "react-redux";
import { playTrack, setTrackList } from "../redux/slices/playerSlice";
import { BsFillPlayFill } from "react-icons/bs";
import { AiFillEdit } from "react-icons/ai";
import { useToast, useDisclosure } from "@chakra-ui/react";
import AlertForm from "../components/AlertForm";
import { useNavigate } from "react-router-dom";
const PlaylistPage = () => {
	const [data, setData] = useState(null);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState(false);

	const { id } = useParams();

	const dispatch = useDispatch();
	const navigate = useNavigate();
	const { user } = useSelector((state) => state.user);
	let isUserPlaylist = user?.id === data?.userId;;
	const fetchPlaylist = async () => {
		setLoading(true);
		setError(false);
		await client
			.get(`/Playlist/${id}`, { withCredentials: true })
			.then((res) => {
				setData(res.data);
				setLoading(false);

			})
			.catch(() => {
				setError(true);
				setLoading(false);
			});
	};

	useEffect(() => {
		fetchPlaylist();
	}, []);
	const toast = useToast();
	const handlePlay = () => {
		dispatch(setTrackList({ list: data?.songs }));
		dispatch(playTrack(data?.songs[0]));
	};

	const { isOpen, onOpen, onClose } = useDisclosure()
	const cancelRef = useRef()
	const onSongPlay = (song) => {
		const index = data?.songs.findIndex((s) => s.id == song.id);
		dispatch(setTrackList({ list: data?.songs, index }));
		dispatch(playTrack(song));
	};

	const onDeletePlaylist = async () => {
		await client.delete(`/Playlist/${id}`, { withCredentials: true })
				.then((res) => {
				if (res.status === 200) {
					toast({
						description: "Delete Playlist Successfully",
						status: "success",
					});
					navigate('/home');
				}
			})
			.catch((err) => {
				toast({
					description: `${err.message ? err.message : "Failed in Request"}`,
					status: "Error",
				});
			});
	}

	if (loading) {
		return <LoadingSkeleton />;
	}

	if (error) {
		return (
			<Flex align="center" justify="center" minH="100vh">
				<Flex direction="column" align="center" color="accent.light">
					<MdErrorOutline color="inherit" size={32} />
					<Text color="zinc.400" textAlign="center">
						An error occured
					</Text>
				</Flex>
			</Flex>
		);
	}

	return (
		<Box
			minH="100vh"
			p={{ base: 2, md: 4 }}
			pl={{ base: 4, md: 14, xl: 0 }}
			pb={{ base: 32, md: 32 }}
			pt={{ base: 12, md: 4 }}>
			<Box pt={6}>
				<Flex
					direction={{ base: "column", md: "row" }}
					align="flex-start"
					justify="flex-start"
					gap={5}>
					<Box minWidth={{ base: "10rem", md: "14rem" }} h="14rem">
						<Image
							src="https://firebasestorage.googleapis.com/v0/b/socialstream-ba300.appspot.com/o/music_app_files%2Fplaylist_cover.jpg?alt=media&token=546adcad-e9c3-402f-8a57-b7ba252100ec"
							alt={data?.title}
							w="full"
							h="full"
							objectFit="cover"
							rounded="lg"
						/>
					</Box>
					<Box>
						<Heading
							as="h5"
							fontSize="xs"
							color="accent.light"
							mb={2}
							fontWeight={400}>
							Playlist
						</Heading>
						<Heading
							as="h1"
							fontSize={{ base: "xl", md: "3xl" }}
							color="zinc.100"
							mb={{ base: 1, md: 4 }}
							fontWeight={600}>
							{data?.title}
						</Heading>
						<Text fontSize="sm" color="zinc.400">
							{data?.description}
						</Text>
						{isUserPlaylist && (
							<>
								<Link to={`/playlists/edit/${id}`}>
									<Button
										variant="outline"
										leftIcon={<AiFillEdit />}
										size="sm"
										mt={2}
										color="whiteAlpha.400"
										_hover={{
											color: "white",
											borderColor: "white"
										}}>
										Edit
									</Button>
								</Link>

								<Button
									marginLeft={2}
									variant="outline"
									leftIcon={<AiFillEdit />}
									size="sm"
									color="whiteAlpha.400"
									mt={2}
									_hover={{
										color: "white",
										borderColor: "white"
									}}
									onClick={onOpen}
								>
									<AlertForm
										isOpen={isOpen}
										cancelRef={cancelRef}
										onClose={onClose}
										title="Delete Playlist"
										desc="Do you want delete this Playlist"
										callbackAgree={
											onDeletePlaylist
										}
									/>
									Delete
								</Button>

							</>)}
					</Box>
				</Flex>
				<Box mt={12}>
					<Flex align="center" gap={6} mb={4}>
						<Heading
							as="h3"
							fontSize={{ base: "lg", md: "xl" }}
							fontWeight={600}>
							{data?.songs?.length} Songs
						</Heading>
						<Button
							onClick={handlePlay}
							display="inline-flex"
							alignItems="center"
							variant="unstyled"
							bg="accent.light"
							color="white"
							rounded="2rem"
							fontSize={{ base: "sm", md: "md" }}
							py={1}
							px={4}
							leftIcon={<BsFillPlayFill size={20} />}>
							Play All
						</Button>
					</Flex>
					<Divider w="full" h="1px" border="0" bg="zinc.600" mb={3} />

					<Flex direction="column" gap={4}>
						{data?.songs?.map((song) => (
							<ArtisteSong
								key={song?.id}
								song={song}
								handlePlay={onSongPlay}
							/>
							// <div>{song.title}</div>
						))}
					</Flex>
				</Box>
			</Box>
		</Box>
	);
};

export default PlaylistPage;
