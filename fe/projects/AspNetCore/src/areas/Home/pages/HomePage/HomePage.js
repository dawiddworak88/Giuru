import React from "react";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material/styles";
import GlobalHelper from "../../../../shared/helpers/globals/GlobalHelper";
import LocaleHelper from "../../../../shared/helpers/globals/LocaleHelper";
import Store from "../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import HeroSlider from "../../components/HeroSlider/HeroSlider";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import ContentGrid from "../../../../shared/components/ContentGrid/ContentGrid";
import Footer from "../../../../shared/components/Footer/Footer";
import OrdersAnalyticsDetail from "../../../../shared/components/OrdersAnalytics/OrdersAnalyticsDetail";
import NotificationBar from "../../../../shared/components/NotificationBar/NotificationBar";

function HomePage(props) {

  LocaleHelper.setMomentLocale(props.locale);

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <ToastContainer />
      <Store>
        {props.notificationBar && props.notificationBar.items &&
          <NotificationBar {...props.notificationBar}></NotificationBar>
        }
        <Header {...props.header}></Header>
        <MainNavigation {...props.mainNavigation}></MainNavigation>
        <HeroSlider {...props.heroSlider}></HeroSlider>
        {props.carouselGrid && 
          <CarouselGrid {...props.carouselGrid}></CarouselGrid>
        }
        {props.newsCarouselGrid &&
          <CarouselGrid {...props.newsCarouselGrid} />
        }
        {props.ordersAnalytics && props.ordersAnalytics.salesAnalytics &&
          <OrdersAnalyticsDetail {...props.ordersAnalytics } />
        }
        {props.contentGrid &&
          <ContentGrid {...props.contentGrid}></ContentGrid>
        }
        <Footer {...props.footer}></Footer>
      </Store>
    </ThemeProvider>
  );
}

export default HomePage;
