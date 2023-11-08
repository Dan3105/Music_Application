import axios from "axios";
import { useNavigate } from "react-router-dom";
export const client = axios.create({
	baseURL: "http://localhost:5070/api/",
	// https://localhost:7263/api/
	// http://localhost:5070/api/
});
