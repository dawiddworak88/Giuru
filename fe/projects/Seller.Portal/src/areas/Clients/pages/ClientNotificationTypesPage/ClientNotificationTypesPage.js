import React from "react";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";
import { ThemeProvider } from "@emotion/react";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import { ToastContainer } from "react-toastify";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MenuTiles from "../../../../shared/components/MenuTiles/MenuTiles";
import Catalog from "../../../../shared/components/Catalog/Catalog";
import Footer from "../../../../shared/components/Footer/Footer";

const ClientNotificationTypesPage = (props) =>
{
    LocaleHelper.setMomentLocale(props.locale)

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Store>
                <Header {...props.header}></Header>
                <MenuTiles {...props.menuTiles} />
                <Catalog {...props.catalog} ></Catalog>
                <Footer {...props.footer} ></Footer>
            </Store>
        </ThemeProvider>
    );
}

export default ClientNotificationTypesPage;