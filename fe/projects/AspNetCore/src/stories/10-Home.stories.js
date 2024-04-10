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
      imageUrl: "https://media.eltap.com/api/v1/files/0535ad85-4bba-4f22-1a82-08dc52e27976",
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
      imageUrl: "https://media.eltap.com/api/v1/files/bd75afbf-c548-47ec-1a83-08dc52e27976",
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
      imageUrl: "https://media.eltap.com/api/v1/files/259ff133-5cb7-45c9-1a89-08dc52e27976",
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
      imageUrl: "https://media.eltap.com/api/v1/files/fa7af3fd-ba6a-4778-f841-08dc10ea6350",
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

var newsCarouselGrid = {
  items: [
    {
      id: 1,
      carouselItems: [
        {
          id: 1,
          url: "#",
          title: "Spring promo",
          categoryName: "Information",
          subtitle: "Sell",
          createdDate: "2024-03-15",
        },
        {
          id: 2,
          url: "#",
          title: "Sell",
          categoryName: "Information",
          subtitle: "Sell",
          createdDate: "2024-03-15",
        }
      ]
    }
  ]
}

export const HomePageStory = () => <HomePage notificationBar={notificationBar} header={header} mainNavigation={mainNavigation} heroSlider={heroSlider} contentGrid={contentGrid} newsCarouselGrid={newsCarouselGrid} footer={footer} />

HomePageStory.story = {
  name: "Home Page",
};

const HomeStories = {
  title: "Pages",
  component: HomePageStory,
};

export default HomeStories;