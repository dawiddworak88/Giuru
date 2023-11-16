import React from "react";
import "../areas/Home/pages/HomePage/HomePage.scss";
import HomePage from "../areas/Home/pages/HomePage/HomePage";
import { header, mainNavigation, footer } from "./shared/Props";

var notificationBar = {
  items: [
    {
      icon: "AccessTime",
      link: {
        url: "test",
        text: "Umów spotkanie z opiekunem Twojej firmy",
        target: "blank"
      }
    },
    {
      icon: "LocalShipping",
      link: {
        url: "test",
        text: "Do 30 dni na zwrot",
      }
    },
    {
      icon: "LocalPostOffice",
      link: {
        url: "test",
        text: "Zapisz się do newslettera i zyskaj 50 zł rabatu",
      }
    }
  ]
}

var heroSlider = {
  items: [
    { imageSrc: "https://eltap.pl/upload/gallery/55/marinosavana05soft11okajpg6870.jpg", imageAlt: "Best sectionals", imageTitle: "Best sectionals", teaserTitle: "Shop sectionals", teaserText: "Best sectionals in the industry", ctaUrl: "#", ctaText: "Shop now!" },
    { imageSrc: "https://eltap.pl/upload/gallery/83/sofa-neva01197rgbjpg8615.jpg", imageAlt: "Best sectionals", imageTitle: "Best sectionals" }
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