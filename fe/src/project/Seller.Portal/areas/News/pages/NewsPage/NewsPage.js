import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import Store from "../../../../../../shared/stores/Store";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import Catalog from "../../../../../../shared/components/Catalog/Catalog";

const NewsPage = (props) => {

    toast.configure();
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Store>
                <Header {...props.header}></Header>
                <MenuTiles {...props.menuTiles} />
                <Catalog {...props.catalog}></Catalog>
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    );
}

export default NewsPage;
