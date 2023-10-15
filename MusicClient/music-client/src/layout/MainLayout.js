import { Box } from "@mui/material"
import sizeConfigs from "../configs/sizeConfigs.tsx"
import TopBar from "../components/common/top-bar.js"
import SidebarMenu from "../components/common/side-bar.js"

const MainLayout = () => {
    return(
        <Box sx={{ display: "flex"}}>
            <Box component="nav"
                sx = {{
                    width: sizeConfigs.sizeBar.width
                }}
            >
                <SidebarMenu />
            </Box>
            <TopBar />
        </Box>
    )
}

export default MainLayout;