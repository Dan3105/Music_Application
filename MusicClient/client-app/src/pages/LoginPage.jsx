import {
	Box,
	Button,
	Flex,
	FormControl,
	FormLabel,
	Heading,
	Input,
	InputGroup,
	InputRightElement,
	Spinner,
	Text,
	useToast,
} from "@chakra-ui/react";
import { useState } from "react";
import { AiOutlineEye, AiOutlineEyeInvisible } from "react-icons/ai";
import { MdError } from "react-icons/md";
import { useDispatch } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { client } from "../api";
import { loginUser, setUser } from "../redux/slices/userSlice";
import { resetPlayer } from "../redux/slices/playerSlice";
import { setFavorite } from "../redux/slices/favoriteSlice";
import { useSelector } from "react-redux";
const LoginPage = () => {
	const [error, setError] = useState(null);
	const [loading, setLoading] = useState(false);
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [showPassword, setShowPassword] = useState(false);
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const toast = useToast();
	const validateFields = () => {
		if (email == "" || password == "") {
			setError("All fields are required!");
			return false;
		} else {
			setError(null);
			return true;
		}
	};

	
	
	const fetchFavorites = async (userId) => {
		if (userId !== null)
		{
			await client
				.get(`/MusicService/FavoriteSongs/user/${userId}`, {withCredentials: true})
				.then((res) => {
					setLoading(false);
					dispatch(setFavorite(res.data));
				})
				.catch(() => {
					setLoading(false);
					setError(true);
				});
		}
	};

	const handleLogin = async () => {
		if (validateFields()) {
			setLoading(true);
			await client
				.post("/UserService/Auth/login", {
					email,
					password,
				}, {withCredentials: true})
				.then((res) => {
					dispatch(resetPlayer());
					dispatch(loginUser(res.data));
					dispatch(setUser(res.data.userRequest))
					setLoading(false);
					navigate('/home');
					fetchFavorites(res.data.userRequest.id);
				})
				.catch((err) => {
					console.log(err);
					setError(err?.response?.data?.message);
					toast({
						description: "Login Failed",
						status: "error",
					});
					setLoading(false);
				});
		}
	};


	return (
		<Box minH="calc(100vh - 5rem)" maxW="2xl" mx="auto" p={6}>
			<Box
				bg={{ base: "zinc.950", md: "zinc.900" }}
				rounded="base"
				p={{ base: 2, md: 10 }}>
				<Box mb={8}>
					<Heading fontSize="2xl" color="zinc.200">
						Login
					</Heading>
					<Text fontSize="sm">To continue enjoying BeatBox</Text>
				</Box>
				<Flex direction="column" gap={4}>
					<FormControl>
						<FormLabel fontSize="xs" color="zinc.400">
							Email
						</FormLabel>
						<Input
							border="1px"
							borderColor="zinc.600"
							rounded="base"
							outline={0}
							type="text"
							color="zinc.300"
							fontSize="sm"
							value={email}
							onChange={(e) => setEmail(e.target.value)}
						/>
					</FormControl>
					<FormControl>
						<FormLabel fontSize="xs" color="zinc.400">
							Password
						</FormLabel>
						<InputGroup border="1px" borderColor="zinc.600" rounded="base">
							<Input
								border="none"
								_focus={{ outline: "none" }}
								type={showPassword ? "text" : "password"}
								color="zinc.300"
								fontSize="sm"
								value={password}
								onChange={(e) => setPassword(e.target.value)}
							/>
							<InputRightElement>
								<Button
									p={1}
									color="zinc.300"
									_hover={{ opacity: 0.8 }}
									variant="ghost"
									onClick={() => setShowPassword(!showPassword)}>
									{showPassword ? <AiOutlineEye /> : <AiOutlineEyeInvisible />}
								</Button>
							</InputRightElement>
						</InputGroup>
					</FormControl>
					{error && (
						<Flex align="center" color="red.500" gap={4}>
							<MdError color="inherit" />
							<Text color="inherit" fontSize="xs">
								{error}
							</Text>
						</Flex>
					)}
					<Box mt={6}>
						<Button
							onClick={handleLogin}
							bg="accent.main"
							py={5}
							w="full"
							_hover={{ opacity: 0.8 }}>
							{loading ? <Spinner color="white" /> : "LOGIN"}
						</Button>
						<Text my={2} fontSize="sm" textAlign="center">
							OR
						</Text>
						<Link to="/home">
							<Text color="zinc.400" fontSize="sm" textAlign="center">
								Continue without logging in
							</Text>
						</Link>
					</Box>
					<Text fontSize="sm" color="zinc.400">
						{"Don't have an account yet?"}{" "}
						<Link to="/auth/register">
							{" "}
							<Text as="span" color="accent.main">
								Register
							</Text>
						</Link>
					</Text>
				</Flex>
			</Box>
		</Box>
	);
};

export default LoginPage;
