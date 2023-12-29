import axios from "axios";
import { useNavigate } from "react-router-dom";
export const client = axios.create({
	baseURL: "http://localhost:5067/api/",
	// https://localhost:7263/api/
	// http://localhost:5070/api/
});

client.interceptors.response.use(
	(response) => {return response;},
	(error) => {
		if(error.response && error.response.status === 401)
		{
			return client.post("/UserService/Auth/RefreshToken", []	, {
				withCredentials: true,
			  })
			  .then(() => {
				return client(error.config);
			  })
				.catch((errorRefresh) => {
					return Promise.reject(errorRefresh)
				})
		}
		return Promise.reject(error);
	}
	);