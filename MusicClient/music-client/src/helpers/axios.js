import axios from "axios";
import createAuthRefreshInterceptor from "axios-auth-refresh";

const axiosService = axios.create({
    baseURL: "http://localhost:5268/",
    headers: {
        "Content-Type": "application/json",
    },
})

/** 
 * neu file json co dang
 * {
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJuYmYiOjE2OTczMzI4NDgsImV4cCI6MTY5NzMzNDY0OCwiaWF0IjoxNjk3MzMyODQ4fQ.ZWAM9dxRhReV0RXApP6X-9jF5NJXw3flWbV8Zqvln68",
  "refreshToken": "7NcsXXp8GKvuXyS1mXzRV859/tYE3Z8Sa4NEkZGNIPt9i2QazEHv71XE1a37hlh67MXsDP34GKj7TE4kn9xjVg==",
  "isSuccess": true,
  "reason": null
}

thi de lay gia tri token viet nhu ben duoi
*/
axiosService.interceptors.request.use(async (config) => {
    const {token} = JSON.parse(localStorage.getItem("auth"));
    config.headers.Authorization = `Bearer ${token}`;

    return config;
})


axiosService.interceptors.response.use(
    (res) => Promise.resolve(res),
    (err) => Promise.reject(err),
);

const refreshAuthLogic = async (faliedRequest) => {
    //const { refreshToken } = JSON.parse(localStorage.getItem("auth"));
    const { refreshToken } = Request.coo
    return axios.post("/api/Auth/RefreshToken/", null, {
        baseURL: "http://localhost:5268/",
        headers: {
            Authorization: `Bearer ${refreshToken}`,
        },
    })
    .then((resp) => {
        const { token, refreshToken} = resp.data;
        faliedRequest.response.config.headers[
            "Authorization"] = "Bearer " + token;
        localStorage.setItem("auth", JSON.stringify({token, refreshToken}))
    }).catch(() => {
        localStorage.removeItem("auth")
    });
}

createAuthRefreshInterceptor(axiosService, refreshAuthLogic);

export function fetcher(url) {
    return axiosService.get(url).then((res)=>res.data);
}

export default axiosService;