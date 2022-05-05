import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@mui/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Store from "../../../../../../shared/stores/Store";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import ClientForm from "../../components/ClientForm/ClientForm";

function ClientPage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <Header {...props.header}></Header>
        <MenuTiles {...props.menuTiles} />
        <ClientForm {...props.clientForm} />
        <Footer {...props.footer}></Footer>
      </Store>
    </ThemeProvider>
  );
}

export default ClientPage;
