import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import NewsCatalog from "../../components/NewsCatalog/NewsCatalog";

const NewsPage = (props) => {
    
    toast.configure();
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Store>
                <Header {...props.header}></Header>
                <MainNavigation {...props.mainNavigation} />
                <NewsCatalog {...props.newsCatalog} />
                <Footer {...props.footer}></Footer>
            </Store>
        </ThemeProvider>
    )
}

export default NewsPage;