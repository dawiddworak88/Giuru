import React from "react";
import PropTypes from "prop-types";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";
import Store from "../../../../../../shared/stores/Store";
import ClientDetailForm from "../../components/ClientDetail/ClientDetailForm";

function ClientDetailPage(props) {

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <section className="section section-small-padding client-detail">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <ClientDetailForm {...props.clientDetailForm} />
                </div>
            </div>
          </section>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

ClientDetailPage.propTypes = {
    title: PropTypes.string.isRequired
};

export default ClientDetailPage;