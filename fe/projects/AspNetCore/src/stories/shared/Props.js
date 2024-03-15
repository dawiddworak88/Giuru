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
    selectedLanguageUrl: "/en",
    selectedLanguageText: "EN"
  },
  userPopup: {
    signOutLink: {
      url: "#",
      text: "Logout"
    },
    signInLink: {
      url: "#",
      text: "Sign In",
    },
    actions: [
      {
        url: "#",
        text: "My orders"
      },
      {
        url: "#",
        text: "Place order"
      }
    ],
    welcomeText: "Hi",
    name: "PIGU",
    isLoggedIn: true,
  },
  searchTerm: "",
  searchUrl: "#",
  searchLabel: "Search",
  searchPlaceholderLabel: "Search"
};

export var breadcrumbs = {
  items: [
    {
      url: "#",
      name: "Home",
      isActive: false
    },
    {
      url: "#",
      name: "Corners",
      isActive: false
    },
    {
      url: "#",
      name: "Mounting Dream Tilt TV Wall",
      isActive: true
    }
  ]
};

export var mainNavigation = {
  links: [
    { url: "#", text: "Categories" },
    { url: "#", text: "Sell" }
  ]
};

export var files = {
  files: [
    { filename: "pricelist_EUR.xlsx", name: "Price List 2020", description: "Price List for year 2020 in Excel format", isProtected: true, size: ".2 MB" }
  ],
  downloadLabel: "Download",
  filenameLabel: "Filename",
  nameLabel: "Name",
  descriptionLabel: "Description:",
  sizeLabel: "Size",
  lastModifiedDateLabel: "Last Modified Date",
  createdDateLabel: "Created Date"
};

export var footer = {
  copyright: "© 2023 | Giuru. All rigths reserved.",
  links: [
    { text: "Privacy Policy", url: "#privacy-policy" },
    { text: "Terms & Conditions", url: "#terms-conditions" }
  ]
};