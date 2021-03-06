import React from "react";
import "../project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss";
import HomePage from "../project/AspNetCore/areas/Home/pages/HomePage/HomePage";
import { header, mainNavigation, footer } from "./Shared/AspNetCoreProps";

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

export const HomePageStory = () => <HomePage header={header} mainNavigation={mainNavigation} heroSlider={heroSlider} contentGrid={contentGrid} footer={footer} />

HomePageStory.story = {
  name: "Home Page",
};

export default {
  title: "AspNetCore.Pages",
  component: HomePageStory,
};