import './App.css';
import { BrowserRouter, Routes, Route} from 'react-router-dom';
import MainLayout from './layout/MainLayout.js'
import LoginSignupForm from './components/pages/LoginSignupForm.js'
function App() {
  return (
      <Routes>
        <Route path="/" element={<MainLayout />}>
          {/* <Route index element={}></Route> */}
          
        </Route>
        <Route path="login" element={<LoginSignupForm/>}></Route>
      </Routes>
    
  );
}

export default App;
