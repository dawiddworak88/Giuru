import React from "react";
import { toast } from "react-toastify";
import { ThemeProvider } from "@material-ui/core/styles";
import GlobalHelper from "../../../../../../shared/helpers/globals/GlobalHelper";
import Store from "../../../../../../shared/stores/Store";
import Header from "../../../../shared/components/Header/Header";
import MainNavigation from "../../../../shared/components/MainNavigation/MainNavigation";
import HeroSlider from "../../components/HeroSlider/HeroSlider";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import ContentGrid from "../../../../shared/components/ContentGrid/ContentGrid";
import Footer from "../../../../../../shared/components/Footer/Footer";

function HomePage(props) {

  toast.configure();

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <Store>
        <Header {...props.header}></Header>
        <MainNavigation {...props.mainNavigation}></MainNavigation>
        <HeroSlider {...props.heroSlider}></HeroSlider>
        {props.carouselGrid && 
          <CarouselGrid {...props.carouselGrid}></CarouselGrid>
        }
        {props.newsCarousel &&
          <CarouselGrid {...props.newsCarousel} />
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
