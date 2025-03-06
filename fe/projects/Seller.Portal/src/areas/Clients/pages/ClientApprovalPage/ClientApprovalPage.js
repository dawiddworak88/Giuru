import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../shared/components/Header/Header";
import Store from "../../../../shared/stores/Store";
import Footer from "../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../shared/components/MenuTiles/MenuTiles";
import ClientApprovalForm from "../../components/ClientApprovalForm/ClientApprovalForm";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";

const ClientApprovalPage = (props) => {

    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <ToastContainer />
            <Store>
                <Header {...props.header}></Header>
                <MenuTiles {...props.menuTiles} />
                <ClientApprovalForm {...props.clientApprovalForm} locale={props.locale} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    )
}

export default ClientApprovalPage;