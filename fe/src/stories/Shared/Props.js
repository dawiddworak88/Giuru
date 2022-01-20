export var footer = {
    copyright: "Copyright © 2021 Giuru",
    links: [
    ]
  };

export var header = {
  logo: {
    targetUrl: "/",
    logoAltLabel: "Logo",
    logoUrl: "https://media.eltap.com/api/v1/files/85b14b09-856d-4fd0-8af4-7c077953b214?o=true"
  },
  languageSwitcher: {
    availableLanguages: [
      {
        url: "/en",
        text: "EN"
      },
      {
        url: "/de",
        text: "DE"
      },
      {
        url: "/pl",
        text: "PL"
      }
    ],
    selectedLanguageUrl: "/en"
  },
  drawerMenuCategories: [
    {
      items: [
        {
          title: "Orders",
          icon: "ShoppingCart",
          isActive: true
        }
      ]
    },
    {
      items: [
        {
          title: "Products",
          icon: "Package",
          url: "https://google.com"
        },
        {
          title: "Categories",
          icon: "Grid"
        }
      ]
    },
    {
      items: [
        {
          title: "Clients",
          icon: "Users"
        }
      ]
    },
    {
      items: [
        {
          title: "Settings",
          icon: "Settings"
        }
      ]
    }
  ],
  links: [
  ],
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

export var menuTiles = {
  tiles: [
    { icon: "ShoppingCart", title: "Orders", url: "#" },
    { icon: "Package", title: "Products", url: "#" },
    { icon: "Server", title: "Warehouses", url: "#" },
    { icon: "Box", title: "Inventory", url: "#" },
    { icon: "Users", title: "Clients", url: "#" },
    { icon: "Image", title: "Media", url: "#" },
    { icon: "Settings", title: "Settings", url: "#" }
  ],
};