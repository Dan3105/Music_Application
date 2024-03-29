import { useEffect, useRef, useState } from "react";
import {
	Button,
	Flex,
	Hide,
	SimpleGrid,
	useDisclosure,
	useToast,
} from "@chakra-ui/react";
import { useDispatch, useSelector } from "react-redux";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import {
	nextTrack,
	prevTrack,
	setPlaying,
} from "../../redux/slices/playerSlice";
import { client } from "../../api";
import { setUser } from "../../redux/slices/userSlice";
import VolumeControl from "./VolumeControl";
import TrackDetails from "./TrackDetails";
import PlayControls from "./PlayControls";
import LoginModal from "../LoginModal";
import PlayingBar from "./PlayingBar";
import { setModalMessage } from "../../redux/slices/modalSlice";
import { setFavorite } from "../../redux/slices/favoriteSlice";
const MusicPlayer = () => {
	const { isOpen, onOpen, onClose } = useDisclosure();
	const modalRef = useRef();
	const toast = useToast();
	const dispatch = useDispatch();
	const { currentTrack, repeatStatus, currentIndex, trackList, isPlaying } =
		useSelector((state) => state.player);
	const { user, token } = useSelector((state) => state.user);
	const audioRef = useRef();
	const { favorites } = useSelector((state) => state.favorites)
	const isEndOfTracklist = currentIndex === trackList.length - 1;

	const [songDetails, setSongDetails] = useState(null);
	const [audioPlaying, setAudioPlaying] = useState(
		audioRef.current && audioRef.current.playing
	);

	useEffect(() => {
		dispatch(setPlaying(audioPlaying));
	}, [audioPlaying]);

	useEffect(() => {
		if (isPlaying) {
			var promise = audioRef.current.play();
			if (promise) {
				promise.catch(function (error) { console.error(error); });
			}
		}
	}, [isPlaying]);

	useEffect(() => {
		setSongDetails((prev) => {
			return { ...prev, time: 0 };
		});
		audioRef.current.currentTime = 0;
		var promise = audioRef.current.play();
		if (promise) {
			promise.catch(function (error) { console.error(error); });
		}
	}, [currentTrack?.id]);

	useEffect(() => {
		setSongDetails({
			volume: 1,
			time: audioRef?.current
				? Math.round(
					(audioRef?.current.currentTime / audioRef.current.duration) * 100
				) // eslint-disable-line no-mixed-spaces-and-tabs
				: 0,
			shuffle: false,
			repeat: false,
		});
	}, [audioRef.current]);

	const seekPoint = (e) => {
		audioRef.current.currentTime = (e / 100) * audioRef.current.duration;

		setSongDetails((prev) => ({
			...prev,
			time: Math.round(
				(audioRef.current.currentTime / audioRef.current.duration) * 100
			),
		}));
	};

	const changeVolume = (e) => {
		setSongDetails((prevValues) => {
			return { ...prevValues, volume: e / 100 };
		});
		audioRef.current.volume = e / 100;
	};

	const handlePlayPause = () => {
		console.log(audioRef)
		if (audioRef.current.readyState >= 2) { // Check if audio is loaded
			if (isPlaying) {
				audioRef.current.pause();
				setAudioPlaying(false);
			} else {
				const playPromise = audioRef.current.play();
				if (playPromise !== undefined) {
					playPromise
						.then(() => {
							setAudioPlaying(true);
						})
						.catch((error) => {
							console.error("Play promise error:", error);
							setAudioPlaying(false);
						});
				}
			}
		}
	};

	const volumeToggle = () => {
		if (songDetails?.volume > 0) {
			setSongDetails((prev) => {
				return { ...prev, volume: 0 };
			});
			audioRef.current.volume = 0;
		} else {
			setSongDetails((prev) => {
				return { ...prev, volume: 1 };
			});
			audioRef.current.volume = 1;
		}
	};

	useEffect(() => {
		audioRef.current.currentTime = 0;
		audioRef?.current.play();
		dispatch(setPlaying(true));
	}, [currentTrack.src]);

	const handleNextSong = () => {
		if (trackList.length == 1) {
			restartSong();
		} else {
			dispatch(nextTrack());
		}
	};

	const handlePreviousSong = () => {
		if (trackList.length == 1) {
			restartSong();
		} else {
			dispatch(prevTrack());
		}
	};

	const restartSong = () => {
		setSongDetails((prev) => {
			return { ...prev, time: 0 };
		});
		audioRef.current.currentTime = 0;
		audioRef.current.play();
	};

	const handleEnded = () => {
		switch (repeatStatus) {
			case "OFF":
				if (!isEndOfTracklist) {
					handleNextSong();
				}
				break;
			case "TRACKLIST":
				handleNextSong();
				break;
			case "SINGLE":
				audioRef.current.play();
				break;

			default:
				break;
		}
	};

	const update = async () => {
		const ress = await client.get(`/MusicService/FavoriteSongs/user/${user.id}`, {withCredentials:true})
		dispatch(setFavorite(ress.data));
	  }

	const likeSong = async () => {
		await client
			.patch(`/MusicService/Song/like/${currentTrack?.id}`, null, { withCredentials: true })
			.then((res) => {
				//dispatch(setUser(res.data));
				update();
				toast({
					description: "Your favorites have been updated",
					status: "success",
				});

				updateSong();
			})
			.catch(() => {
				toast({
					description: "An error occured",
					status: "error",
				});
			});

	};

	const updateSong = async () => {
		await client.get(`/MusicService/FavoriteSongs/user/${user.id}`, {withCredentials: true})
		.then((ress) => {
			dispatch(setFavorite(ress.data));
		})
		.catch((ex) => { console.log(ex) });
	}

	const handleLike = () => {
		if (!user) {
			dispatch(
				setModalMessage("Please login to save songs to your favorites.")
			);
			onOpen();
		} else {
			likeSong();
		}
	};
	return (
		<>
			<LoginModal ref={modalRef} onClose={onClose} isOpen={isOpen} />
			<SimpleGrid
				templateColumns="repeat(3, 1fr)"
				align="center"
				justify="space-between"
				position="fixed"
				bottom="0"
				left="0"
				zIndex={100}
				width="full"
				p={4}
				border="1px"
				borderColor="zinc.600"
				roundedTop="lg"
				bgColor="blackAlpha.700"
				backdropFilter="blur(15px)">
				<TrackDetails track={currentTrack} />
				<Flex direction="column" gap={2}>
					<PlayControls
						isPlaying={isPlaying}
						onNext={handleNextSong}
						onPlay={handlePlayPause}
						onPrevious={handlePreviousSong}
						repeatStatus={repeatStatus}
					/>
					<Hide below="md">
						<PlayingBar
							onSeek={seekPoint}
							time={songDetails?.time}
							track={currentTrack}
							trackRef={audioRef.current}
						/>
					</Hide>
				</Flex>
				<Flex align="center" justify="flex-end" gap={{ base: 0, md: 4 }}>
					<Button
						variant="unstyled"
						fontSize={{ base: 18, md: 24 }}
						p={0}
						h={{ base: 8, md: 12 }}
						minW={6}
						display="inline-flex"
						alignItems="center"
						justifyContent="center"
						color="accent.main"
						onClick={handleLike}>
						{favorites?.map(obj => obj.id)?.includes(currentTrack.id) ? (
							<AiFillHeart color="inherit" />
						) : (
							<AiOutlineHeart color="#ddd" />
						)}
					</Button>
					<Flex justifyContent="space-between" gap={0}>
						<Hide below="md">
							<VolumeControl
								onChange={changeVolume}
								onToggle={volumeToggle}
								volume={songDetails ? songDetails?.volume : 0}
							/>
						</Hide>
						<audio
							ref={audioRef}
							src={currentTrack?.songURL}
							onPause={() => setAudioPlaying(false)}
							onPlay={() => setAudioPlaying(true)}
							onEnded={handleEnded}
							onTimeUpdate={() => {
								setSongDetails((prev) => ({
									...prev,
									time: Math.round(
										(audioRef.current.currentTime / audioRef.current.duration) *
										100
									),
								}));
							}}
						/>
					</Flex>
				</Flex>
			</SimpleGrid>
		</>
	);
};

export { MusicPlayer };
