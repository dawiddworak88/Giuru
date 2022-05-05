import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import NewsItemDetails from "../../components/NewsItemDetails/NewsItemDetails";
import Breadcrumbs from "../../../../shared/components/Breadcrumb/Breadcrumbs";

const NewsItemPage = (props) => {
    
    toast.configure();
    LocaleHelper.setMomentLocale(props.locale);

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Store>
                <Header {...props.header} />
                <MainNavigation {...props.mainNavigation} />
                <Breadcrumbs {...props.breadcrumbs} />
                <NewsItemDetails {...props.newsItemDetails} />
                <Footer {...props.footer} />
            </Store>
        </ThemeProvider>
    )
}

export default NewsItemPage;