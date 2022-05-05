import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Store from "../../../../../../shared/stores/Store";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import OutletForm from "../../components/OutletForm/OutletForm";

const OutletPage = (props) => {

  toast.configure();
  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <Header {...props.header}></Header>
        <MenuTiles {...props.menuTiles} />
        <OutletForm {...props.outletForm} />
        <Footer {...props.footer}></Footer>
      </Store>
    </ThemeProvider>
  );
}

export default OutletPage;
