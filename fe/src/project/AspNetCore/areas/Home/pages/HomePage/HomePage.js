import React from 'react';
import { toast } from 'react-toastify';
import { ThemeProvider } from '@material-ui/core/styles';
import GlobalHelper from '../../../../../../shared/helpers/globals/GlobalHelper';
import Store from '../../../../../../shared/stores/Store';
import Header from '../../components/Header/Header';
import MainNavigation from '../../components/MainNavigation/MainNavigation';
import HeroSlider from '../../components/HeroSlider/HeroSlider';
import Footer from '../../../../../../shared/components/Footer/Footer';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function HomePage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <div className="home-page">
          <Header {...props.header}></Header>
          <MainNavigation {...props.mainNavigation}></MainNavigation>
          <HeroSlider {...props.heroSlider}></HeroSlider>
          <Footer {...props.footer}></Footer>
        </div>
      </Store>
    </ThemeProvider>
  );
}

export default HomePage;
