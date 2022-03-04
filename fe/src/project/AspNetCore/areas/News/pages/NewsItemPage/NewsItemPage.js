import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import NewsItemDetails from "../../components/NewsItemDetails/NewsItemDetails";

const NewsItemPage = (props) => {
    
    toast.configure();

    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Store>
                <Header {...props.header} />
                <MainNavigation {...props.mainNavigation} />
                <NewsItemDetails {...props.newsItemDetails} />
                <Footer {...props.footer} />
            </Store>
        </ThemeProvider>
    )
}

export default NewsItemPage;