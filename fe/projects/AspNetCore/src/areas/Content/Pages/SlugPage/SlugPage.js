import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../shared/components/Footer/Footer";

const SlugPage = (props) => {
    
    return (
        <ThemeProvider>
            <ToastContainer />
            <Store>
                <Header {...props.header}/>
                <MainNavigation {...props.mainNavigation} />
                <Footer {...props.footer}/>
            </Store>
        </ThemeProvider>
    )
}

export default SlugPage;