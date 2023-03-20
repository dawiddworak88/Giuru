import React from "react";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../shared/components/Header/Header";
import Footer from "../../../../shared/components/Footer/Footer";
import ContentDetail from "../../components/Content/ContentDetail";

function ContentPage(props) {
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Header {...props.header}></Header>
            <ContentDetail {...props.content}></ContentDetail>
            <Footer {...props.footer}></Footer>
        </ThemeProvider>
    );
}

export default ContentPage;
