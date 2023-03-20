import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../.../.../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import DownloadCenterCatalog from "../../components/DownloadCenterCatalog/DownloadCenterCatalog";

const DownloadCenterPage = (props) => {
    
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Store>
                <Header {...props.header}></Header>
                <MainNavigation {...props.mainNavigation} />
                <DownloadCenterCatalog {...props.catalog} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    )
}

export default DownloadCenterPage;