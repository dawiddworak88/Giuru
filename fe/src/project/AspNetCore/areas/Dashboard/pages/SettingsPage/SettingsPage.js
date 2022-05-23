import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import Footer from "../../../../../../shared/components/Footer/Footer";
import SettingsForm from "../../components/SettingsForm/SettingsForm";
import DashboardNavigation from "../../../../shared/components/DashboardNavigation/DashboardNavigation";

const SettingsPage = (props) => {

  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <ToastContainer />
      <Store>
        <Header {...props.header}></Header>
        <MainNavigation {...props.mainNavigation}></MainNavigation>
        <div className="container pt-6 pb-6">
            <div className="columns p-2">
                <div className="column">
                    <DashboardNavigation {...props.dashboardNavigation} />
                </div>
                <div className="column p-2 is-three-quarters">
                    <SettingsForm {...props.settingsForm} />
                </div>
            </div>
        </div>
        <Footer {...props.footer}></Footer>
      </Store>
    </ThemeProvider>
  );
}

export default SettingsPage;
