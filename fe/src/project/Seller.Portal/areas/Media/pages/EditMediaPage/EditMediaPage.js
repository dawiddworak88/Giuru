import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Store from "../../../../../../shared/stores/Store";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import LocaleHelper from "../../../../../../shared/helpers/globals/LocaleHelper";
import EditForm from "../../components/EditForm/EditForm";

function UploadMediaPage(props) {

  toast.configure();
  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <EditForm {...props.editForm} />
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default UploadMediaPage;
