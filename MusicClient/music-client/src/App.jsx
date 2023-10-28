import Sidebar from "./components/common/SideBar.jsx"
import { useSelector } from "react-redux";
import { Route, Routes } from "react-router-dom";
function App() {
  return (
    <div className="relative flex">
      <Sidebar />
    </div>
    
    
  );
}

export default App;
