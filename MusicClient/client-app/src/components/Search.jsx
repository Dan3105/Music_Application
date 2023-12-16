import { Box, Input, InputGroup, InputRightElement } from "@chakra-ui/react";
import { useState } from "react";
import { BsSearch } from "react-icons/bs";
import { useNavigate } from "react-router-dom";
const Search = () => {
	const [searchTerm, setSearchTerm] = useState();
	const nav= useNavigate();
	
	const handlerSearch = () => {
		nav(`/library/${searchTerm}`)
	};

	return (
		<Box mb={6} pb={4} borderBottom="1px" borderBottomColor="zinc.600">
			<InputGroup>
				<Input
					border="1px"
					borderColor="zinc.700"
					placeholder="Search..."
					w="full"
					outline={0}
					bg="transparent"
					p={2}
					onChange={(e) => setSearchTerm(e.target.value)}
					onKeyDown={(e) => {
						if(e.key === 'Enter'){
							handlerSearch();
						}
					}}
				/>
				<InputRightElement color="zinc.500">
					<BsSearch color="inherit" />
				</InputRightElement>
			</InputGroup>
		</Box>
	);
};

export default Search;
