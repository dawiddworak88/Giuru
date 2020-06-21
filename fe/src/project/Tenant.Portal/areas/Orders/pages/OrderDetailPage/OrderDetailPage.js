import React from 'react';
import { toast } from 'react-toastify';
import { ThemeProvider } from '@material-ui/core/styles';
import GlobalHelper from '../../../../../../shared/helpers/globals/GlobalHelper';
import Header from '../../../../../../shared/components/Header/Header';
import Store from '../../../../../../shared/stores/Store';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function OrderDetailPage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div>
          <Header {...props.header}></Header>
          <MenuTiles {...props.menuTiles} />
          <section className="section section-small-padding catalog">
            <h1 className="subtitle is-4">{props.title}</h1>
          </section>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default OrderDetailPage;