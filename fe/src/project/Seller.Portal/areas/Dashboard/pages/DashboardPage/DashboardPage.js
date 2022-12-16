import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import Store from "../../../../../../shared/stores/Store";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Header from "../../../../../../shared/components/Header/Header";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import Footer from "../../../../../../shared/components/Footer/Footer";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import DashboardDetail from "../../components/DashboardDetail/DashboardDetail";

const DashboardPage = (props) => {
    
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
            <ToastContainer />
            <Store>
                <Header {...props.header}/>
                <MenuTiles {...props.menuTiles} />
                <DashboardDetail {...props.dashboardDetail} />
                <Footer {...props.footer}/>
            </Store>
        </ThemeProvider>
    )
}

export default DashboardPage;