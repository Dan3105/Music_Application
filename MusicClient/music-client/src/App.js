import './App.css';
import { BrowserRouter, Routes, Route} from 'react-router-dom';
import MainLayout from './layout/MainLayout.js'
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          <Route index element={}
        </Route>
      </Routes>
    </BrowserRouter>
    
  );
}

export default App;
