import '../../assets/css/login.css'
import React, { useState } from 'react';
import { Formik, Form, Field } from 'formik';
import * as Yup from 'yup';
import { FormHelperText } from '@mui/material';
import axios from 'axios'


const LoginSignupForm = () => {
  const [activeForm, setActiveForm] = useState('login');

  const SignupSchema = Yup.object().shape({
    password: Yup.string()
      .min(8, 'Password must be at least 8 characters long.')
      .required('Password is required'),
    confirmPassword: Yup.string().oneOf([Yup.ref('password'), null], 'Passwords must match'),
    email: Yup.string()
      .email('Email must be a valid email address.')
      .required('Email is required'),
  })

  const handleSwitch = (formType) => {
    setActiveForm(formType);
  };

  const LoginUser = (e) => {
    e.preventDefault();
    console.log('Hahaha')
  }

  const RegisterUser = async (e) => {
      const _email = e.email;
      const _password = e.password;
      
      try {
        const instance = axios.create({
          baseURL: "http://localhost:5268/"
        });
    
        // Send data as an object, no need for JSON.stringify
        const response = await instance.post('/api/Auth/register', {
          email: _email,
          password: _password
        }, {
          headers: {
            'Content-Type': 'application/json'
          }
        });
    
        // Handle the response here
        console.log('Registration response:', response.data);
      } catch (error) {
        // Handle any errors here
        console.error('Registration failed', error);
      }
    }

  return (
    <section className="forms-section">
      <h1 className="section-title">Animated Forms</h1>
      <div className="forms">
        <div className={`form-wrapper ${activeForm === 'login' ? 'is-active' : ''}`}>
          <button type="button" className="switcher switcher-login" onClick={() => handleSwitch('login')}>
            Login
            <span className="underline" />
          </button>
          <Formik

          >
            <Form className="form form-login">
              {/* Login form content */}
              <fieldset>
                <legend>Please, enter your email and password for login.</legend>
                <div className="input-block">
                  <label htmlFor="login-email">E-mail</label>
                  <input id="login-email" type="email" required />
                </div>
                <div className="input-block">
                  <label htmlFor="login-password">Password</label>
                  <input id="login-password" type="password" required />
                </div>
              </fieldset>
              <button type="submit" className="btn-login">Login</button>

            </Form>
          </Formik>
        </div>

        <div className={`form-wrapper ${activeForm === 'signup' ? 'is-active' : ''}`}>
          <button type="button" className="switcher switcher-signup" onClick={() => handleSwitch('signup')}>
            Sign Up
            <span className="underline" />
          </button>
          <Formik
          initialValues={{
            password:'',
            confirmPassword: '',
            email: '',
          }}
          validationSchema={SignupSchema}
          onSubmit={RegisterUser}
          >{({ errors, touched}) => (
            <Form className="form form-signup" >
              <fieldset>
                <legend>Please, enter your email, password and password confirmation for sign up.</legend>
                <div className="input-block">
                  <label htmlFor="signup-email">E-mail</label>
                  <Field name="email" id="signup-email" type="email" required />
                  {errors.email && touched.email && <FormHelperText className="MuiFormHelperText-error">{errors.email}</FormHelperText>}
                </div>
                <div className="input-block">
                  <label htmlFor="signup-password">Password</label>
                  <Field name="password" id="signup-password" type="password" required />
                  {errors.password && touched.password && <FormHelperText className="MuiFormHelperText-error">{errors.password}</FormHelperText>}
                </div>
                <div className="input-block">
                  <label htmlFor="signup-password-confirm">Confirm password</label>
                  <Field name="confirmPassword" id="signup-password-confirm" type="password" required />
                  {errors.confirmPassword && touched.confirmPassword && <FormHelperText className="MuiFormHelperText-error">{errors.confirmPassword}</FormHelperText>}
                </div>
              </fieldset>
              <button type="submit" className="btn-signup">Continue</button>

            </Form>
            )}
          </Formik>
        </div>
      </div>
    </section>
  );
};

export default LoginSignupForm;
