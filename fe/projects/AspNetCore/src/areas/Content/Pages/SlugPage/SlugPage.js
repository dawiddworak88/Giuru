import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../shared/components/Footer/Footer";
import NotificationBar from "../../../../shared/components/NotificationBar/NotificationBar";
import { StrapiContent } from "../../../../shared/components/StrapiContent/StrapiContent";

const SlugPage = (props) => {
    
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme(props.locale)}>
            <ToastContainer />
            <Store>
                {props.notificationBar && props.notificationBar.items &&
                    <NotificationBar {...props.notificationBar}></NotificationBar>
                }
                <Header {...props.header}/>
                <MainNavigation {...props.mainNavigation} />
                <StrapiContent {...props.strapiContentWidgets} />
                <Footer {...props.footer}/>
            </Store>
        </ThemeProvider>
    )
}

export default SlugPage;