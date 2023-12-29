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
import { Link } from "react-router-dom";
import { client } from "../api";
import { useDispatch } from "react-redux";
import { loginUser } from "../redux/slices/userSlice";
import { resetPlayer } from "../redux/slices/playerSlice";

const RegisterPage = () => {
	const [error, setError] = useState(null);
	const [loading, setLoading] = useState(false);
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [showPassword, setShowPassword] = useState(false);
	const dispatch = useDispatch();
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

	const handleRegister = async () => {
		if (validateFields()) {
			setLoading(true);
			await client
				.post("/UserService/Auth/register", {
					email,
					password,
				})
				.then((res) => {
					dispatch(resetPlayer());
					dispatch(loginUser(res.data));
					setLoading(false);
					toast({
						description: "Register Success",
						status: "success",
					});
					
				})
				.catch((err) => {
					toast({
						description: "Register Failed!",
						status: "error",
					});
					setError(err?.response?.data?.message);
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
						Register
					</Heading>
					<Text fontSize="sm">To continue enjoying BeatBox</Text>
				</Box>
				<Flex direction="column" gap={4}>
					<FormControl>
						<FormLabel fontSize="xs" color="zinc.400">
							Username
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
							onClick={handleRegister}
							bg="accent.main"
							py={5}
							w="full"
							_hover={{ opacity: 0.8 }}>
							{loading ? <Spinner color="white" /> : "REGISTER"}
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
						Already have an account ?{" "}
						<Link to="/auth/login">
							{" "}
							<Text as="span" color="accent.main">
								Login
							</Text>
						</Link>
					</Text>
				</Flex>
			</Box>
		</Box>
	);
};

export default RegisterPage;
