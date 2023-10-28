import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';

const baseURL = "http://localhost:5268/";
const apiLogin = '/api/Auth/login'
const apiRegister = '/api/Auth/register'

//Call back after success
const LoginUser = async (e, callbackAfterSuccess = null) => {
    const _email = e.email;
    const _password = e.password;

    try {
        const instance = axios.create({
            baseURL: baseURL
        })

        const response = await instance.post(apiLogin, {
            email: _email,
            password: _password,
        }, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.status === 200) {
            // Handle the response here
            toast.success('Login Success!', {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                theme: "light",
            });

            localStorage.setItem("user", JSON.stringify({
                userId: response.data.userId,
                roles: response.data.roles,
                ipAddress: response.data.ipAddress,
                userEmail: response.data.userEmail
            }));
            
            //navigate to Menu please
            if (callbackAfterSuccess !== null) {
                callbackAfterSuccess();
            }
        }
        else {

            toast.error(response.data, {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                theme: "light",
            });
        }
    }
    catch (error) {
        console.log(error)
        toast.error('error.response', {
            position: "top-right",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: "light",
        });
    }

}


const RegisterUser = async (e, callbackAfterSuccess = null) => {
    const _email = e.email;
    const _password = e.password;

    try {
        const instance = axios.create({
            baseURL: baseURL
        });

        // Send data as an object, no need for JSON.stringify
        const response = await instance.post(apiRegister, {
            email: _email,
            password: _password
        }, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.status === 200) {
            // Handle the response here
            toast.success('Register Success!', {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                theme: "light",
            });

            if (callbackAfterSuccess !== null) {
                callbackAfterSuccess();
            }

        }
        else {

            toast.error(response.data, {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                theme: "light",
            });
        }


    } catch (error) {
        // Handle any errors here
        toast.error(error.response.data.message, {
            position: "top-right",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: "light",
        });
    }
}

export { LoginUser, RegisterUser };