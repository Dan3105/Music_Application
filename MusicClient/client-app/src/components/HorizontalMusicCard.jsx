import {
	Box,
	Button,
	Flex,
	Heading,
	Hide,
	Image,
	Text,
} from "@chakra-ui/react";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import { motion } from "framer-motion";
import { fadeInUp } from "../theme/motionVariants";
import { useSelector } from "react-redux";
import "util";
import { convertToMins } from "../utils";
const HorizontalMusicCard = ({ song, onPlay }) => {
	const { currentTrack } = useSelector((state) => state.player);
	const { user } = useSelector((state) => state.user);

	return (
		<Box
			cursor="pointer"
			onClick={() => onPlay(song)}
			as={motion.div}
			initial="initial"
			animate="animate"
			variants={fadeInUp}
			bg="zinc.900"
			p={2}
			w="full"
			rounded="base">
			<Flex align="center" justify="space-between">
				<Flex align="center" gap={{ base: 2, md: 4 }}>
					<Image
						src={song?.coverImage || "https://wallpaperset.com/w/full/0/3/f/466996.jpg"}
						alt="album"
						objectFit="cover"
						w={{ base: 8, md: 10 }}
						h={{ base: 8, md: 10 }}
						rounded="base"
					/>
					<Box>
						<Heading
							as="h4"
							fontSize={{ base: "sm", md: "md" }}
							fontWeight={500}
							color={
								currentTrack?._id == song?._id ? "accent.light" : "zinc.200"
							}
							noOfLines={1}>
							{song?.title}
						</Heading>
						<Text fontSize="xs" noOfLines={1} color="zinc.400">
							{song?.artists?.map(artist => artist.name).join(", ")}
							{/* {console.log(song?.artists)} */}
						</Text>
					</Box>
				</Flex>
				<Flex align="center" gap={{ base: 1, md: 3 }}>
					<Hide below="xl">
						<Text fontSize="sm" color="zinc.400">
							{convertToMins(song?.duration)}
						</Text>
					</Hide>
					<Button variant="unstyled" minW={5} color="zinc.300">
						{user?.favorites?.includes(song._id) ? (
							<AiFillHeart color="inherit" />
						) : (	
							<AiOutlineHeart color="#ddd" />
						)}
					</Button>
				</Flex>
			</Flex>
		</Box>
	);
};

export default HorizontalMusicCard;
