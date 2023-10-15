import React from "react";
import '../../assets/css/login.css';
import { Formik } from "formik";
import * as Yup from "yup";
import { loginUser } from "../../services/auth-api";

const schema = Yup.object().shape({
    email: Yup.string()
        .required("Email is a required field")
        .email("Invalid email format"),
    password: Yup.string()
        .required("Password is a required field")
        .min(5, "Password must be at least 8 characters"),
});

const LoginForm = () => {
    return (
        <>
            <Formik
                validationSchema={schema}
                initialValues={{ email: "", password: "" }}
                onSubmit={(values) => {
                    const response = loginUser(values.email, values.password);
                    console.log(response);
                    //console.log('Call API here');
                }}
            >
                {({
                    values,
                    errors,
                    touched,
                    handleChange,
                    handleBlur,
                    handleSubmit,
                }) => (
                    <div className="login">
                        <div className="form">
                            <form noValidate onSubmit={handleSubmit}>
                                <span>Login</span>

                                <input
                                    type="email"
                                    name="email"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    value={values.email}
                                    placeholder="Enter email id / username"
                                    className="form-control inp_text"
                                    id="email"
                                />
                                {/* If validation is not passed show errors */}
                                <p className="error">
                                    {errors.email && touched.email && errors.email}
                                </p>

                                <input
                                    type="password"
                                    name="password"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    value={values.password}
                                    placeholder="Enter password"
                                    className="form-control"
                                />
                                {/* If validation is not passed show errors */}
                                <p className="error">
                                    {errors.password && touched.password && errors.password}
                                </p>

                                <button type="submit">Login</button>
                            </form>
                        </div>
                    </div>
                )}

            </Formik>
        </>)
}

export default LoginForm;