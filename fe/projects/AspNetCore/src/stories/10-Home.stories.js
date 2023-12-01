import React from "react";
import "../areas/Home/pages/HomePage/HomePage.scss";
import HomePage from "../areas/Home/pages/HomePage/HomePage";
import { header, mainNavigation, footer } from "./shared/Props";

var notificationBar = {
  items: [
    {
      icon: "LocalShipping",
      link: {
        url: "#",
        text: "Up to 30 days to return",
      }
    },
    {
      icon: "LocalPostOffice",
      link: {
        url: "#",
        text: "Sign up for the newsletter and get a PLN 50 discount",
      }
    }
  ]
}

var heroSlider = {
  items: [
    {
      image: {
        imageSrc: "https://eltap.pl/upload/gallery/55/marinosavana05soft11okajpg6870.jpg",
        imageAlt: "Best sectionals",
        imageTitle: "Best sectionals",
      },
      teaserTitle: "Shop sectionals",
      teaserText: "Best sectionals in the industry",
      ctaUrl: "#",
      ctaText: "Shop now!" 
    },
    {
      image: {
        imageSrc: "https://eltap.pl/upload/gallery/83/sofa-neva01197rgbjpg8615.jpg",
        imageAlt: "Best sectionals",
        imageTitle: "Best sectionals",
      },
      teaserTitle: "Shop sectionals",
      teaserText: "The Arcadova Sofa is the perfect complement to any style, providing an ideal place for relaxation",
      ctaUrl: "#",
      ctaText: "See colors!" 
    }
  ]
};

var contentGrid = {
  items: [
    {
      id: 1,
      title: "Living Room",
      carouselItems: [
        {
          id: 1000,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1001,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1002,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1003,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1004,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1005,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1006,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1007,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1008,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        }
      ]
    },
    {
      id: 2,
      title: "Bedroom",
      carouselItems: [
        {
          id: 1000,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1001,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1002,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1003,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1004,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1005,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1006,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1007,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1008,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        }
      ]
    },
    {
      id: 3,
      title: "Bathroom",
      carouselItems: [
        {
          id: 1000,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1001,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1002,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1003,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1004,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1005,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1006,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1007,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1008,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        }
      ]
    },
    {
      id: 4,
      title: "Kitchen",
      carouselItems: [
        {
          id: 1000,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1001,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1002,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1003,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1004,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1005,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1006,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1007,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        },
        {
          id: 1008,
          url: "#",
          imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
          imageAlt: "Sofas",
          title: "Sofas"
        }
      ]
    }
  ]
};

export const HomePageStory = () => <HomePage notificationBar={notificationBar} header={header} mainNavigation={mainNavigation} heroSlider={heroSlider} contentGrid={contentGrid} footer={footer} />

HomePageStory.story = {
  name: "Home Page",
};

const HomeStories = {
  title: "Pages",
  component: HomePageStory,
};

export default HomeStories;