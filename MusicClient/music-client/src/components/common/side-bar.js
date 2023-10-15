import { Sidebar, Menu, MenuItem } from "react-pro-sidebar";
import { Home, Search } from '@mui/icons-material'
import { Link } from 'react-router-dom'
import "../../assets/css/sub-menu.css"
import sizeConfigs from "../../configs/sizeConfigs.tsx";
const customMenuItemStyles = {
    button: {
      [`&:hover`]: {
        backgroundColor: '#121212',
        color: '#f2f2f2',
        borderRadius: '8px',
      },
    },
  };

const SidebarMenu = () => {
    return <div className="sidemenu-background" style={({height: "100vh", display: "flex", paddingRight: "30px"})}>
        <Sidebar style={{ 
            width: sizeConfigs.sizeBar.width,
            height: sizeConfigs.sizeBar.height, 
            backgroundColor:"transparent",
            }}>
            <Menu
             menuItemStyles= {customMenuItemStyles}
             className="sidemenu-block" 
             iconShape="square" 
             >
                <MenuItem className="sidemenu-btn" component={<Link to="#" />} icon={<Home />}>Home</MenuItem>
                <MenuItem className="sidemenu-btn" component={<Link to="#" />} icon={<Search />}>Search</MenuItem>
            </Menu>

            <Menu 
            menuItemStyles= {customMenuItemStyles}
            className="sidemenu-block" 
            iconShape="square" 
            style={({ display:"flex", flex:"1", width:"100%", marginTop:"5vh" })}
            >
                <MenuItem icon={<Home />}>Dashboard</MenuItem>
                <MenuItem >Component 1</MenuItem>
                <MenuItem >Component 2</MenuItem>
            </Menu>
        </Sidebar>
    </div>
}

export default SidebarMenu;