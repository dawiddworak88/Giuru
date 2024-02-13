import React from "react"
import { ThemeProvider } from "@mui/material/styles"
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper"
import Header from "../../../../shared/components/Header/Header"
import Footer from "../../../../shared/components/Footer/Footer"
import PrivacyPolicyDetail from "../../components/PrivacyPolicy/PrivacyPolicyDetail"

const PrivacyPolicyPage = (props) => {
    return (
        <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
            <Header {...props.header}></Header>
            <PrivacyPolicyDetail {...props.privacyPolicy}></PrivacyPolicyDetail>
            <Footer {...props.footer}></Footer>
        </ThemeProvider>
    )
}

export default PrivacyPolicyPage;