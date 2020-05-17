/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "C:\\Projects\\Giuru\\be\\src\\Project\\Web\\Tenant.Portal\\wwwroot\\dist\\js";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 260);
/******/ })
/************************************************************************/
/******/ ({

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(53);
} else {}


/***/ }),

/***/ 1:
/***/ (function(module, exports, __webpack_require__) {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (false) { var throwOnDirectAccess, ReactIs; } else {
  // By explicitly using `prop-types` you are opting into new production behavior.
  // http://fb.me/prop-types-in-prod
  module.exports = __webpack_require__(57)();
}


/***/ }),

/***/ 12:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

// EXPORTS
__webpack_require__.d(__webpack_exports__, "a", function() { return /* binding */ _slicedToArray; });

// CONCATENATED MODULE: ./node_modules/babel-preset-react-app/node_modules/@babel/runtime/helpers/esm/arrayWithHoles.js
function _arrayWithHoles(arr) {
  if (Array.isArray(arr)) return arr;
}
// CONCATENATED MODULE: ./node_modules/babel-preset-react-app/node_modules/@babel/runtime/helpers/esm/iterableToArrayLimit.js
function _iterableToArrayLimit(arr, i) {
  if (typeof Symbol === "undefined" || !(Symbol.iterator in Object(arr))) return;
  var _arr = [];
  var _n = true;
  var _d = false;
  var _e = undefined;

  try {
    for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) {
      _arr.push(_s.value);

      if (i && _arr.length === i) break;
    }
  } catch (err) {
    _d = true;
    _e = err;
  } finally {
    try {
      if (!_n && _i["return"] != null) _i["return"]();
    } finally {
      if (_d) throw _e;
    }
  }

  return _arr;
}
// EXTERNAL MODULE: ./node_modules/babel-preset-react-app/node_modules/@babel/runtime/helpers/esm/unsupportedIterableToArray.js
var unsupportedIterableToArray = __webpack_require__(25);

// CONCATENATED MODULE: ./node_modules/babel-preset-react-app/node_modules/@babel/runtime/helpers/esm/nonIterableRest.js
function _nonIterableRest() {
  throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
}
// CONCATENATED MODULE: ./node_modules/babel-preset-react-app/node_modules/@babel/runtime/helpers/esm/slicedToArray.js




function _slicedToArray(arr, i) {
  return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || Object(unsupportedIterableToArray["a" /* default */])(arr, i) || _nonIterableRest();
}

/***/ }),

/***/ 21:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _arrayLikeToArray; });
function _arrayLikeToArray(arr, len) {
  if (len == null || len > arr.length) len = arr.length;

  for (var i = 0, arr2 = new Array(len); i < len; i++) {
    arr2[i] = arr[i];
  }

  return arr2;
}

/***/ }),

/***/ 25:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _unsupportedIterableToArray; });
/* harmony import */ var _arrayLikeToArray__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(21);

function _unsupportedIterableToArray(o, minLen) {
  if (!o) return;
  if (typeof o === "string") return Object(_arrayLikeToArray__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"])(o, minLen);
  var n = Object.prototype.toString.call(o).slice(8, -1);
  if (n === "Object" && o.constructor) n = o.constructor.name;
  if (n === "Map" || n === "Set") return Array.from(n);
  if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return Object(_arrayLikeToArray__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"])(o, minLen);
}

/***/ }),

/***/ 260:
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(424);
module.exports = __webpack_require__(261);


/***/ }),

/***/ 261:
/***/ (function(module, exports, __webpack_require__) {

// extracted by mini-css-extract-plugin

/***/ }),

/***/ 27:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(1);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var Plus = function Plus(props) {
  var color = props.color,
      size = props.size,
      otherProps = _objectWithoutProperties(props, ["color", "size"]);

  return react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("svg", _extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("line", {
    x1: "12",
    y1: "5",
    x2: "12",
    y2: "19"
  }), react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("line", {
    x1: "5",
    y1: "12",
    x2: "19",
    y2: "12"
  }));
};

Plus.propTypes = {
  color: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  size: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.oneOfType([prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string, prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number])
};
Plus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ __webpack_exports__["a"] = (Plus);

/***/ }),

/***/ 28:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
// ESM COMPAT FLAG
__webpack_require__.r(__webpack_exports__);

// EXPORTS
__webpack_require__.d(__webpack_exports__, "Activity", function() { return /* reexport */ activity; });
__webpack_require__.d(__webpack_exports__, "Airplay", function() { return /* reexport */ airplay; });
__webpack_require__.d(__webpack_exports__, "AlertCircle", function() { return /* reexport */ alert_circle; });
__webpack_require__.d(__webpack_exports__, "AlertOctagon", function() { return /* reexport */ alert_octagon; });
__webpack_require__.d(__webpack_exports__, "AlertTriangle", function() { return /* reexport */ alert_triangle; });
__webpack_require__.d(__webpack_exports__, "AlignCenter", function() { return /* reexport */ align_center; });
__webpack_require__.d(__webpack_exports__, "AlignJustify", function() { return /* reexport */ align_justify; });
__webpack_require__.d(__webpack_exports__, "AlignLeft", function() { return /* reexport */ align_left; });
__webpack_require__.d(__webpack_exports__, "AlignRight", function() { return /* reexport */ align_right; });
__webpack_require__.d(__webpack_exports__, "Anchor", function() { return /* reexport */ icons_anchor; });
__webpack_require__.d(__webpack_exports__, "Aperture", function() { return /* reexport */ aperture; });
__webpack_require__.d(__webpack_exports__, "Archive", function() { return /* reexport */ archive; });
__webpack_require__.d(__webpack_exports__, "ArrowDownCircle", function() { return /* reexport */ arrow_down_circle; });
__webpack_require__.d(__webpack_exports__, "ArrowDownLeft", function() { return /* reexport */ arrow_down_left; });
__webpack_require__.d(__webpack_exports__, "ArrowDownRight", function() { return /* reexport */ arrow_down_right; });
__webpack_require__.d(__webpack_exports__, "ArrowDown", function() { return /* reexport */ arrow_down; });
__webpack_require__.d(__webpack_exports__, "ArrowLeftCircle", function() { return /* reexport */ arrow_left_circle; });
__webpack_require__.d(__webpack_exports__, "ArrowLeft", function() { return /* reexport */ arrow_left; });
__webpack_require__.d(__webpack_exports__, "ArrowRightCircle", function() { return /* reexport */ arrow_right_circle; });
__webpack_require__.d(__webpack_exports__, "ArrowRight", function() { return /* reexport */ arrow_right; });
__webpack_require__.d(__webpack_exports__, "ArrowUpCircle", function() { return /* reexport */ arrow_up_circle; });
__webpack_require__.d(__webpack_exports__, "ArrowUpLeft", function() { return /* reexport */ arrow_up_left; });
__webpack_require__.d(__webpack_exports__, "ArrowUpRight", function() { return /* reexport */ arrow_up_right; });
__webpack_require__.d(__webpack_exports__, "ArrowUp", function() { return /* reexport */ arrow_up; });
__webpack_require__.d(__webpack_exports__, "AtSign", function() { return /* reexport */ at_sign; });
__webpack_require__.d(__webpack_exports__, "Award", function() { return /* reexport */ award; });
__webpack_require__.d(__webpack_exports__, "BarChart2", function() { return /* reexport */ bar_chart_2; });
__webpack_require__.d(__webpack_exports__, "BarChart", function() { return /* reexport */ bar_chart; });
__webpack_require__.d(__webpack_exports__, "BatteryCharging", function() { return /* reexport */ battery_charging; });
__webpack_require__.d(__webpack_exports__, "Battery", function() { return /* reexport */ battery; });
__webpack_require__.d(__webpack_exports__, "BellOff", function() { return /* reexport */ bell_off; });
__webpack_require__.d(__webpack_exports__, "Bell", function() { return /* reexport */ bell; });
__webpack_require__.d(__webpack_exports__, "Bluetooth", function() { return /* reexport */ bluetooth; });
__webpack_require__.d(__webpack_exports__, "Bold", function() { return /* reexport */ bold; });
__webpack_require__.d(__webpack_exports__, "BookOpen", function() { return /* reexport */ book_open; });
__webpack_require__.d(__webpack_exports__, "Book", function() { return /* reexport */ book; });
__webpack_require__.d(__webpack_exports__, "Bookmark", function() { return /* reexport */ bookmark; });
__webpack_require__.d(__webpack_exports__, "Box", function() { return /* reexport */ box; });
__webpack_require__.d(__webpack_exports__, "Briefcase", function() { return /* reexport */ briefcase; });
__webpack_require__.d(__webpack_exports__, "Calendar", function() { return /* reexport */ calendar; });
__webpack_require__.d(__webpack_exports__, "CameraOff", function() { return /* reexport */ camera_off; });
__webpack_require__.d(__webpack_exports__, "Camera", function() { return /* reexport */ camera; });
__webpack_require__.d(__webpack_exports__, "Cast", function() { return /* reexport */ cast; });
__webpack_require__.d(__webpack_exports__, "CheckCircle", function() { return /* reexport */ check_circle; });
__webpack_require__.d(__webpack_exports__, "CheckSquare", function() { return /* reexport */ check_square; });
__webpack_require__.d(__webpack_exports__, "Check", function() { return /* reexport */ check; });
__webpack_require__.d(__webpack_exports__, "ChevronDown", function() { return /* reexport */ chevron_down; });
__webpack_require__.d(__webpack_exports__, "ChevronLeft", function() { return /* reexport */ chevron_left; });
__webpack_require__.d(__webpack_exports__, "ChevronRight", function() { return /* reexport */ chevron_right; });
__webpack_require__.d(__webpack_exports__, "ChevronUp", function() { return /* reexport */ chevron_up; });
__webpack_require__.d(__webpack_exports__, "ChevronsDown", function() { return /* reexport */ chevrons_down; });
__webpack_require__.d(__webpack_exports__, "ChevronsLeft", function() { return /* reexport */ chevrons_left; });
__webpack_require__.d(__webpack_exports__, "ChevronsRight", function() { return /* reexport */ chevrons_right; });
__webpack_require__.d(__webpack_exports__, "ChevronsUp", function() { return /* reexport */ chevrons_up; });
__webpack_require__.d(__webpack_exports__, "Chrome", function() { return /* reexport */ chrome; });
__webpack_require__.d(__webpack_exports__, "Circle", function() { return /* reexport */ circle; });
__webpack_require__.d(__webpack_exports__, "Clipboard", function() { return /* reexport */ clipboard; });
__webpack_require__.d(__webpack_exports__, "Clock", function() { return /* reexport */ clock; });
__webpack_require__.d(__webpack_exports__, "CloudDrizzle", function() { return /* reexport */ cloud_drizzle; });
__webpack_require__.d(__webpack_exports__, "CloudLightning", function() { return /* reexport */ cloud_lightning; });
__webpack_require__.d(__webpack_exports__, "CloudOff", function() { return /* reexport */ cloud_off; });
__webpack_require__.d(__webpack_exports__, "CloudRain", function() { return /* reexport */ cloud_rain; });
__webpack_require__.d(__webpack_exports__, "CloudSnow", function() { return /* reexport */ cloud_snow; });
__webpack_require__.d(__webpack_exports__, "Cloud", function() { return /* reexport */ cloud; });
__webpack_require__.d(__webpack_exports__, "Code", function() { return /* reexport */ code; });
__webpack_require__.d(__webpack_exports__, "Codepen", function() { return /* reexport */ codepen; });
__webpack_require__.d(__webpack_exports__, "Codesandbox", function() { return /* reexport */ codesandbox; });
__webpack_require__.d(__webpack_exports__, "Coffee", function() { return /* reexport */ coffee; });
__webpack_require__.d(__webpack_exports__, "Columns", function() { return /* reexport */ columns; });
__webpack_require__.d(__webpack_exports__, "Command", function() { return /* reexport */ command; });
__webpack_require__.d(__webpack_exports__, "Compass", function() { return /* reexport */ compass; });
__webpack_require__.d(__webpack_exports__, "Copy", function() { return /* reexport */ copy; });
__webpack_require__.d(__webpack_exports__, "CornerDownLeft", function() { return /* reexport */ corner_down_left; });
__webpack_require__.d(__webpack_exports__, "CornerDownRight", function() { return /* reexport */ corner_down_right; });
__webpack_require__.d(__webpack_exports__, "CornerLeftDown", function() { return /* reexport */ corner_left_down; });
__webpack_require__.d(__webpack_exports__, "CornerLeftUp", function() { return /* reexport */ corner_left_up; });
__webpack_require__.d(__webpack_exports__, "CornerRightDown", function() { return /* reexport */ corner_right_down; });
__webpack_require__.d(__webpack_exports__, "CornerRightUp", function() { return /* reexport */ corner_right_up; });
__webpack_require__.d(__webpack_exports__, "CornerUpLeft", function() { return /* reexport */ corner_up_left; });
__webpack_require__.d(__webpack_exports__, "CornerUpRight", function() { return /* reexport */ corner_up_right; });
__webpack_require__.d(__webpack_exports__, "Cpu", function() { return /* reexport */ cpu; });
__webpack_require__.d(__webpack_exports__, "CreditCard", function() { return /* reexport */ credit_card; });
__webpack_require__.d(__webpack_exports__, "Crop", function() { return /* reexport */ crop; });
__webpack_require__.d(__webpack_exports__, "Crosshair", function() { return /* reexport */ crosshair; });
__webpack_require__.d(__webpack_exports__, "Database", function() { return /* reexport */ database; });
__webpack_require__.d(__webpack_exports__, "Delete", function() { return /* reexport */ icons_delete; });
__webpack_require__.d(__webpack_exports__, "Disc", function() { return /* reexport */ disc; });
__webpack_require__.d(__webpack_exports__, "DollarSign", function() { return /* reexport */ dollar_sign; });
__webpack_require__.d(__webpack_exports__, "DownloadCloud", function() { return /* reexport */ download_cloud; });
__webpack_require__.d(__webpack_exports__, "Download", function() { return /* reexport */ download; });
__webpack_require__.d(__webpack_exports__, "Droplet", function() { return /* reexport */ droplet; });
__webpack_require__.d(__webpack_exports__, "Edit2", function() { return /* reexport */ edit_2; });
__webpack_require__.d(__webpack_exports__, "Edit3", function() { return /* reexport */ edit_3; });
__webpack_require__.d(__webpack_exports__, "Edit", function() { return /* reexport */ edit; });
__webpack_require__.d(__webpack_exports__, "ExternalLink", function() { return /* reexport */ external_link; });
__webpack_require__.d(__webpack_exports__, "EyeOff", function() { return /* reexport */ eye_off; });
__webpack_require__.d(__webpack_exports__, "Eye", function() { return /* reexport */ eye; });
__webpack_require__.d(__webpack_exports__, "Facebook", function() { return /* reexport */ facebook; });
__webpack_require__.d(__webpack_exports__, "FastForward", function() { return /* reexport */ fast_forward; });
__webpack_require__.d(__webpack_exports__, "Feather", function() { return /* reexport */ feather; });
__webpack_require__.d(__webpack_exports__, "Figma", function() { return /* reexport */ figma; });
__webpack_require__.d(__webpack_exports__, "FileMinus", function() { return /* reexport */ file_minus; });
__webpack_require__.d(__webpack_exports__, "FilePlus", function() { return /* reexport */ file_plus; });
__webpack_require__.d(__webpack_exports__, "FileText", function() { return /* reexport */ file_text; });
__webpack_require__.d(__webpack_exports__, "File", function() { return /* reexport */ file; });
__webpack_require__.d(__webpack_exports__, "Film", function() { return /* reexport */ film; });
__webpack_require__.d(__webpack_exports__, "Filter", function() { return /* reexport */ filter; });
__webpack_require__.d(__webpack_exports__, "Flag", function() { return /* reexport */ flag; });
__webpack_require__.d(__webpack_exports__, "FolderMinus", function() { return /* reexport */ folder_minus; });
__webpack_require__.d(__webpack_exports__, "FolderPlus", function() { return /* reexport */ folder_plus; });
__webpack_require__.d(__webpack_exports__, "Folder", function() { return /* reexport */ folder; });
__webpack_require__.d(__webpack_exports__, "Framer", function() { return /* reexport */ framer; });
__webpack_require__.d(__webpack_exports__, "Frown", function() { return /* reexport */ frown; });
__webpack_require__.d(__webpack_exports__, "Gift", function() { return /* reexport */ gift; });
__webpack_require__.d(__webpack_exports__, "GitBranch", function() { return /* reexport */ git_branch; });
__webpack_require__.d(__webpack_exports__, "GitCommit", function() { return /* reexport */ git_commit; });
__webpack_require__.d(__webpack_exports__, "GitMerge", function() { return /* reexport */ git_merge; });
__webpack_require__.d(__webpack_exports__, "GitPullRequest", function() { return /* reexport */ git_pull_request; });
__webpack_require__.d(__webpack_exports__, "GitHub", function() { return /* reexport */ github; });
__webpack_require__.d(__webpack_exports__, "Gitlab", function() { return /* reexport */ gitlab; });
__webpack_require__.d(__webpack_exports__, "Globe", function() { return /* reexport */ globe; });
__webpack_require__.d(__webpack_exports__, "Grid", function() { return /* reexport */ grid; });
__webpack_require__.d(__webpack_exports__, "HardDrive", function() { return /* reexport */ hard_drive; });
__webpack_require__.d(__webpack_exports__, "Hash", function() { return /* reexport */ hash; });
__webpack_require__.d(__webpack_exports__, "Headphones", function() { return /* reexport */ headphones; });
__webpack_require__.d(__webpack_exports__, "Heart", function() { return /* reexport */ heart; });
__webpack_require__.d(__webpack_exports__, "HelpCircle", function() { return /* reexport */ help_circle; });
__webpack_require__.d(__webpack_exports__, "Hexagon", function() { return /* reexport */ hexagon; });
__webpack_require__.d(__webpack_exports__, "Home", function() { return /* reexport */ home; });
__webpack_require__.d(__webpack_exports__, "Image", function() { return /* reexport */ icons_image; });
__webpack_require__.d(__webpack_exports__, "Inbox", function() { return /* reexport */ inbox; });
__webpack_require__.d(__webpack_exports__, "Info", function() { return /* reexport */ info; });
__webpack_require__.d(__webpack_exports__, "Instagram", function() { return /* reexport */ instagram; });
__webpack_require__.d(__webpack_exports__, "Italic", function() { return /* reexport */ italic; });
__webpack_require__.d(__webpack_exports__, "Key", function() { return /* reexport */ key; });
__webpack_require__.d(__webpack_exports__, "Layers", function() { return /* reexport */ icons_layers; });
__webpack_require__.d(__webpack_exports__, "Layout", function() { return /* reexport */ layout; });
__webpack_require__.d(__webpack_exports__, "LifeBuoy", function() { return /* reexport */ life_buoy; });
__webpack_require__.d(__webpack_exports__, "Link2", function() { return /* reexport */ link_2; });
__webpack_require__.d(__webpack_exports__, "Link", function() { return /* reexport */ icons_link; });
__webpack_require__.d(__webpack_exports__, "Linkedin", function() { return /* reexport */ linkedin; });
__webpack_require__.d(__webpack_exports__, "List", function() { return /* reexport */ list; });
__webpack_require__.d(__webpack_exports__, "Loader", function() { return /* reexport */ loader; });
__webpack_require__.d(__webpack_exports__, "Lock", function() { return /* reexport */ lock; });
__webpack_require__.d(__webpack_exports__, "LogIn", function() { return /* reexport */ log_in; });
__webpack_require__.d(__webpack_exports__, "LogOut", function() { return /* reexport */ log_out; });
__webpack_require__.d(__webpack_exports__, "Mail", function() { return /* reexport */ mail; });
__webpack_require__.d(__webpack_exports__, "MapPin", function() { return /* reexport */ map_pin; });
__webpack_require__.d(__webpack_exports__, "Map", function() { return /* reexport */ map; });
__webpack_require__.d(__webpack_exports__, "Maximize2", function() { return /* reexport */ maximize_2; });
__webpack_require__.d(__webpack_exports__, "Maximize", function() { return /* reexport */ maximize; });
__webpack_require__.d(__webpack_exports__, "Meh", function() { return /* reexport */ meh; });
__webpack_require__.d(__webpack_exports__, "Menu", function() { return /* reexport */ menu; });
__webpack_require__.d(__webpack_exports__, "MessageCircle", function() { return /* reexport */ message_circle; });
__webpack_require__.d(__webpack_exports__, "MessageSquare", function() { return /* reexport */ message_square; });
__webpack_require__.d(__webpack_exports__, "MicOff", function() { return /* reexport */ mic_off; });
__webpack_require__.d(__webpack_exports__, "Mic", function() { return /* reexport */ mic; });
__webpack_require__.d(__webpack_exports__, "Minimize2", function() { return /* reexport */ minimize_2; });
__webpack_require__.d(__webpack_exports__, "Minimize", function() { return /* reexport */ minimize; });
__webpack_require__.d(__webpack_exports__, "MinusCircle", function() { return /* reexport */ minus_circle; });
__webpack_require__.d(__webpack_exports__, "MinusSquare", function() { return /* reexport */ minus_square; });
__webpack_require__.d(__webpack_exports__, "Minus", function() { return /* reexport */ minus; });
__webpack_require__.d(__webpack_exports__, "Monitor", function() { return /* reexport */ monitor; });
__webpack_require__.d(__webpack_exports__, "Moon", function() { return /* reexport */ moon; });
__webpack_require__.d(__webpack_exports__, "MoreHorizontal", function() { return /* reexport */ more_horizontal; });
__webpack_require__.d(__webpack_exports__, "MoreVertical", function() { return /* reexport */ more_vertical; });
__webpack_require__.d(__webpack_exports__, "MousePointer", function() { return /* reexport */ mouse_pointer; });
__webpack_require__.d(__webpack_exports__, "Move", function() { return /* reexport */ move; });
__webpack_require__.d(__webpack_exports__, "Music", function() { return /* reexport */ music; });
__webpack_require__.d(__webpack_exports__, "Navigation2", function() { return /* reexport */ navigation_2; });
__webpack_require__.d(__webpack_exports__, "Navigation", function() { return /* reexport */ navigation; });
__webpack_require__.d(__webpack_exports__, "Octagon", function() { return /* reexport */ octagon; });
__webpack_require__.d(__webpack_exports__, "Package", function() { return /* reexport */ icons_package; });
__webpack_require__.d(__webpack_exports__, "Paperclip", function() { return /* reexport */ paperclip; });
__webpack_require__.d(__webpack_exports__, "PauseCircle", function() { return /* reexport */ pause_circle; });
__webpack_require__.d(__webpack_exports__, "Pause", function() { return /* reexport */ pause; });
__webpack_require__.d(__webpack_exports__, "PenTool", function() { return /* reexport */ pen_tool; });
__webpack_require__.d(__webpack_exports__, "Percent", function() { return /* reexport */ percent; });
__webpack_require__.d(__webpack_exports__, "PhoneCall", function() { return /* reexport */ phone_call; });
__webpack_require__.d(__webpack_exports__, "PhoneForwarded", function() { return /* reexport */ phone_forwarded; });
__webpack_require__.d(__webpack_exports__, "PhoneIncoming", function() { return /* reexport */ phone_incoming; });
__webpack_require__.d(__webpack_exports__, "PhoneMissed", function() { return /* reexport */ phone_missed; });
__webpack_require__.d(__webpack_exports__, "PhoneOff", function() { return /* reexport */ phone_off; });
__webpack_require__.d(__webpack_exports__, "PhoneOutgoing", function() { return /* reexport */ phone_outgoing; });
__webpack_require__.d(__webpack_exports__, "Phone", function() { return /* reexport */ phone; });
__webpack_require__.d(__webpack_exports__, "PieChart", function() { return /* reexport */ pie_chart; });
__webpack_require__.d(__webpack_exports__, "PlayCircle", function() { return /* reexport */ play_circle; });
__webpack_require__.d(__webpack_exports__, "Play", function() { return /* reexport */ play; });
__webpack_require__.d(__webpack_exports__, "PlusCircle", function() { return /* reexport */ plus_circle; });
__webpack_require__.d(__webpack_exports__, "PlusSquare", function() { return /* reexport */ plus_square; });
__webpack_require__.d(__webpack_exports__, "Plus", function() { return /* reexport */ plus["a" /* default */]; });
__webpack_require__.d(__webpack_exports__, "Pocket", function() { return /* reexport */ pocket; });
__webpack_require__.d(__webpack_exports__, "Power", function() { return /* reexport */ power; });
__webpack_require__.d(__webpack_exports__, "Printer", function() { return /* reexport */ printer; });
__webpack_require__.d(__webpack_exports__, "Radio", function() { return /* reexport */ icons_radio; });
__webpack_require__.d(__webpack_exports__, "RefreshCcw", function() { return /* reexport */ refresh_ccw; });
__webpack_require__.d(__webpack_exports__, "RefreshCw", function() { return /* reexport */ refresh_cw; });
__webpack_require__.d(__webpack_exports__, "Repeat", function() { return /* reexport */ repeat; });
__webpack_require__.d(__webpack_exports__, "Rewind", function() { return /* reexport */ rewind; });
__webpack_require__.d(__webpack_exports__, "RotateCcw", function() { return /* reexport */ rotate_ccw; });
__webpack_require__.d(__webpack_exports__, "RotateCw", function() { return /* reexport */ rotate_cw; });
__webpack_require__.d(__webpack_exports__, "Rss", function() { return /* reexport */ rss; });
__webpack_require__.d(__webpack_exports__, "Save", function() { return /* reexport */ save; });
__webpack_require__.d(__webpack_exports__, "Scissors", function() { return /* reexport */ scissors; });
__webpack_require__.d(__webpack_exports__, "Search", function() { return /* reexport */ search; });
__webpack_require__.d(__webpack_exports__, "Send", function() { return /* reexport */ send; });
__webpack_require__.d(__webpack_exports__, "Server", function() { return /* reexport */ server; });
__webpack_require__.d(__webpack_exports__, "Settings", function() { return /* reexport */ settings; });
__webpack_require__.d(__webpack_exports__, "Share2", function() { return /* reexport */ share_2; });
__webpack_require__.d(__webpack_exports__, "Share", function() { return /* reexport */ share; });
__webpack_require__.d(__webpack_exports__, "ShieldOff", function() { return /* reexport */ shield_off; });
__webpack_require__.d(__webpack_exports__, "Shield", function() { return /* reexport */ shield; });
__webpack_require__.d(__webpack_exports__, "ShoppingBag", function() { return /* reexport */ shopping_bag; });
__webpack_require__.d(__webpack_exports__, "ShoppingCart", function() { return /* reexport */ shopping_cart; });
__webpack_require__.d(__webpack_exports__, "Shuffle", function() { return /* reexport */ shuffle; });
__webpack_require__.d(__webpack_exports__, "Sidebar", function() { return /* reexport */ sidebar; });
__webpack_require__.d(__webpack_exports__, "SkipBack", function() { return /* reexport */ skip_back; });
__webpack_require__.d(__webpack_exports__, "SkipForward", function() { return /* reexport */ skip_forward; });
__webpack_require__.d(__webpack_exports__, "Slack", function() { return /* reexport */ slack; });
__webpack_require__.d(__webpack_exports__, "Slash", function() { return /* reexport */ slash; });
__webpack_require__.d(__webpack_exports__, "Sliders", function() { return /* reexport */ sliders; });
__webpack_require__.d(__webpack_exports__, "Smartphone", function() { return /* reexport */ smartphone; });
__webpack_require__.d(__webpack_exports__, "Smile", function() { return /* reexport */ smile; });
__webpack_require__.d(__webpack_exports__, "Speaker", function() { return /* reexport */ speaker; });
__webpack_require__.d(__webpack_exports__, "Square", function() { return /* reexport */ square; });
__webpack_require__.d(__webpack_exports__, "Star", function() { return /* reexport */ star; });
__webpack_require__.d(__webpack_exports__, "StopCircle", function() { return /* reexport */ stop_circle; });
__webpack_require__.d(__webpack_exports__, "Sun", function() { return /* reexport */ sun; });
__webpack_require__.d(__webpack_exports__, "Sunrise", function() { return /* reexport */ sunrise; });
__webpack_require__.d(__webpack_exports__, "Sunset", function() { return /* reexport */ sunset; });
__webpack_require__.d(__webpack_exports__, "Tablet", function() { return /* reexport */ tablet; });
__webpack_require__.d(__webpack_exports__, "Tag", function() { return /* reexport */ tag; });
__webpack_require__.d(__webpack_exports__, "Target", function() { return /* reexport */ target; });
__webpack_require__.d(__webpack_exports__, "Terminal", function() { return /* reexport */ terminal; });
__webpack_require__.d(__webpack_exports__, "Thermometer", function() { return /* reexport */ thermometer; });
__webpack_require__.d(__webpack_exports__, "ThumbsDown", function() { return /* reexport */ thumbs_down; });
__webpack_require__.d(__webpack_exports__, "ThumbsUp", function() { return /* reexport */ thumbs_up; });
__webpack_require__.d(__webpack_exports__, "ToggleLeft", function() { return /* reexport */ toggle_left; });
__webpack_require__.d(__webpack_exports__, "ToggleRight", function() { return /* reexport */ toggle_right; });
__webpack_require__.d(__webpack_exports__, "Trash2", function() { return /* reexport */ trash_2; });
__webpack_require__.d(__webpack_exports__, "Trash", function() { return /* reexport */ trash; });
__webpack_require__.d(__webpack_exports__, "Trello", function() { return /* reexport */ trello; });
__webpack_require__.d(__webpack_exports__, "TrendingDown", function() { return /* reexport */ trending_down; });
__webpack_require__.d(__webpack_exports__, "TrendingUp", function() { return /* reexport */ trending_up; });
__webpack_require__.d(__webpack_exports__, "Triangle", function() { return /* reexport */ triangle; });
__webpack_require__.d(__webpack_exports__, "Truck", function() { return /* reexport */ truck; });
__webpack_require__.d(__webpack_exports__, "Tv", function() { return /* reexport */ tv; });
__webpack_require__.d(__webpack_exports__, "Twitter", function() { return /* reexport */ twitter; });
__webpack_require__.d(__webpack_exports__, "Type", function() { return /* reexport */ type; });
__webpack_require__.d(__webpack_exports__, "Umbrella", function() { return /* reexport */ umbrella; });
__webpack_require__.d(__webpack_exports__, "Underline", function() { return /* reexport */ underline; });
__webpack_require__.d(__webpack_exports__, "Unlock", function() { return /* reexport */ unlock; });
__webpack_require__.d(__webpack_exports__, "UploadCloud", function() { return /* reexport */ upload_cloud; });
__webpack_require__.d(__webpack_exports__, "Upload", function() { return /* reexport */ upload; });
__webpack_require__.d(__webpack_exports__, "UserCheck", function() { return /* reexport */ user_check; });
__webpack_require__.d(__webpack_exports__, "UserMinus", function() { return /* reexport */ user_minus; });
__webpack_require__.d(__webpack_exports__, "UserPlus", function() { return /* reexport */ user_plus; });
__webpack_require__.d(__webpack_exports__, "UserX", function() { return /* reexport */ user_x; });
__webpack_require__.d(__webpack_exports__, "User", function() { return /* reexport */ user; });
__webpack_require__.d(__webpack_exports__, "Users", function() { return /* reexport */ users; });
__webpack_require__.d(__webpack_exports__, "VideoOff", function() { return /* reexport */ video_off; });
__webpack_require__.d(__webpack_exports__, "Video", function() { return /* reexport */ video; });
__webpack_require__.d(__webpack_exports__, "Voicemail", function() { return /* reexport */ voicemail; });
__webpack_require__.d(__webpack_exports__, "Volume1", function() { return /* reexport */ volume_1; });
__webpack_require__.d(__webpack_exports__, "Volume2", function() { return /* reexport */ volume_2; });
__webpack_require__.d(__webpack_exports__, "VolumeX", function() { return /* reexport */ volume_x; });
__webpack_require__.d(__webpack_exports__, "Volume", function() { return /* reexport */ volume; });
__webpack_require__.d(__webpack_exports__, "Watch", function() { return /* reexport */ watch; });
__webpack_require__.d(__webpack_exports__, "WifiOff", function() { return /* reexport */ wifi_off; });
__webpack_require__.d(__webpack_exports__, "Wifi", function() { return /* reexport */ wifi; });
__webpack_require__.d(__webpack_exports__, "Wind", function() { return /* reexport */ wind; });
__webpack_require__.d(__webpack_exports__, "XCircle", function() { return /* reexport */ x_circle; });
__webpack_require__.d(__webpack_exports__, "XOctagon", function() { return /* reexport */ x_octagon; });
__webpack_require__.d(__webpack_exports__, "XSquare", function() { return /* reexport */ x_square; });
__webpack_require__.d(__webpack_exports__, "X", function() { return /* reexport */ x; });
__webpack_require__.d(__webpack_exports__, "Youtube", function() { return /* reexport */ youtube; });
__webpack_require__.d(__webpack_exports__, "ZapOff", function() { return /* reexport */ zap_off; });
__webpack_require__.d(__webpack_exports__, "Zap", function() { return /* reexport */ zap; });
__webpack_require__.d(__webpack_exports__, "ZoomIn", function() { return /* reexport */ zoom_in; });
__webpack_require__.d(__webpack_exports__, "ZoomOut", function() { return /* reexport */ zoom_out; });

// EXTERNAL MODULE: ./node_modules/react/index.js
var react = __webpack_require__(0);
var react_default = /*#__PURE__*/__webpack_require__.n(react);

// EXTERNAL MODULE: ./node_modules/prop-types/index.js
var prop_types = __webpack_require__(1);
var prop_types_default = /*#__PURE__*/__webpack_require__.n(prop_types);

// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/activity.js
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var activity_Activity = function Activity(props) {
  var color = props.color,
      size = props.size,
      otherProps = _objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", _extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "22 12 18 12 15 21 9 3 6 12 2 12"
  }));
};

activity_Activity.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
activity_Activity.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var activity = (activity_Activity);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/airplay.js
function airplay_extends() { airplay_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return airplay_extends.apply(this, arguments); }

function airplay_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = airplay_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function airplay_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var airplay_Airplay = function Airplay(props) {
  var color = props.color,
      size = props.size,
      otherProps = airplay_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", airplay_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M5 17H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2h-1"
  }), react_default.a.createElement("polygon", {
    points: "12 15 17 21 7 21 12 15"
  }));
};

airplay_Airplay.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
airplay_Airplay.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var airplay = (airplay_Airplay);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/alert-circle.js
function alert_circle_extends() { alert_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return alert_circle_extends.apply(this, arguments); }

function alert_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = alert_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function alert_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var alert_circle_AlertCircle = function AlertCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = alert_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", alert_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "16",
    x2: "12",
    y2: "16"
  }));
};

alert_circle_AlertCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
alert_circle_AlertCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var alert_circle = (alert_circle_AlertCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/alert-octagon.js
function alert_octagon_extends() { alert_octagon_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return alert_octagon_extends.apply(this, arguments); }

function alert_octagon_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = alert_octagon_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function alert_octagon_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var alert_octagon_AlertOctagon = function AlertOctagon(props) {
  var color = props.color,
      size = props.size,
      otherProps = alert_octagon_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", alert_octagon_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "16",
    x2: "12",
    y2: "16"
  }));
};

alert_octagon_AlertOctagon.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
alert_octagon_AlertOctagon.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var alert_octagon = (alert_octagon_AlertOctagon);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/alert-triangle.js
function alert_triangle_extends() { alert_triangle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return alert_triangle_extends.apply(this, arguments); }

function alert_triangle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = alert_triangle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function alert_triangle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var alert_triangle_AlertTriangle = function AlertTriangle(props) {
  var color = props.color,
      size = props.size,
      otherProps = alert_triangle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", alert_triangle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "9",
    x2: "12",
    y2: "13"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "17",
    x2: "12",
    y2: "17"
  }));
};

alert_triangle_AlertTriangle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
alert_triangle_AlertTriangle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var alert_triangle = (alert_triangle_AlertTriangle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/align-center.js
function align_center_extends() { align_center_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return align_center_extends.apply(this, arguments); }

function align_center_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = align_center_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function align_center_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var align_center_AlignCenter = function AlignCenter(props) {
  var color = props.color,
      size = props.size,
      otherProps = align_center_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", align_center_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "18",
    y1: "10",
    x2: "6",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "6",
    x2: "3",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "14",
    x2: "3",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "18",
    y1: "18",
    x2: "6",
    y2: "18"
  }));
};

align_center_AlignCenter.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
align_center_AlignCenter.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var align_center = (align_center_AlignCenter);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/align-justify.js
function align_justify_extends() { align_justify_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return align_justify_extends.apply(this, arguments); }

function align_justify_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = align_justify_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function align_justify_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var align_justify_AlignJustify = function AlignJustify(props) {
  var color = props.color,
      size = props.size,
      otherProps = align_justify_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", align_justify_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "21",
    y1: "10",
    x2: "3",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "6",
    x2: "3",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "14",
    x2: "3",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "18",
    x2: "3",
    y2: "18"
  }));
};

align_justify_AlignJustify.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
align_justify_AlignJustify.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var align_justify = (align_justify_AlignJustify);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/align-left.js
function align_left_extends() { align_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return align_left_extends.apply(this, arguments); }

function align_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = align_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function align_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var align_left_AlignLeft = function AlignLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = align_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", align_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "17",
    y1: "10",
    x2: "3",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "6",
    x2: "3",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "14",
    x2: "3",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "18",
    x2: "3",
    y2: "18"
  }));
};

align_left_AlignLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
align_left_AlignLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var align_left = (align_left_AlignLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/align-right.js
function align_right_extends() { align_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return align_right_extends.apply(this, arguments); }

function align_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = align_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function align_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var align_right_AlignRight = function AlignRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = align_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", align_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "21",
    y1: "10",
    x2: "7",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "6",
    x2: "3",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "14",
    x2: "3",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "18",
    x2: "7",
    y2: "18"
  }));
};

align_right_AlignRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
align_right_AlignRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var align_right = (align_right_AlignRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/anchor.js
function anchor_extends() { anchor_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return anchor_extends.apply(this, arguments); }

function anchor_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = anchor_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function anchor_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var anchor_Anchor = function Anchor(props) {
  var color = props.color,
      size = props.size,
      otherProps = anchor_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", anchor_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "5",
    r: "3"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22",
    x2: "12",
    y2: "8"
  }), react_default.a.createElement("path", {
    d: "M5 12H2a10 10 0 0 0 20 0h-3"
  }));
};

anchor_Anchor.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
anchor_Anchor.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_anchor = (anchor_Anchor);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/aperture.js
function aperture_extends() { aperture_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return aperture_extends.apply(this, arguments); }

function aperture_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = aperture_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function aperture_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var aperture_Aperture = function Aperture(props) {
  var color = props.color,
      size = props.size,
      otherProps = aperture_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", aperture_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "14.31",
    y1: "8",
    x2: "20.05",
    y2: "17.94"
  }), react_default.a.createElement("line", {
    x1: "9.69",
    y1: "8",
    x2: "21.17",
    y2: "8"
  }), react_default.a.createElement("line", {
    x1: "7.38",
    y1: "12",
    x2: "13.12",
    y2: "2.06"
  }), react_default.a.createElement("line", {
    x1: "9.69",
    y1: "16",
    x2: "3.95",
    y2: "6.06"
  }), react_default.a.createElement("line", {
    x1: "14.31",
    y1: "16",
    x2: "2.83",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "16.62",
    y1: "12",
    x2: "10.88",
    y2: "21.94"
  }));
};

aperture_Aperture.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
aperture_Aperture.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var aperture = (aperture_Aperture);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/archive.js
function archive_extends() { archive_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return archive_extends.apply(this, arguments); }

function archive_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = archive_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function archive_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var archive_Archive = function Archive(props) {
  var color = props.color,
      size = props.size,
      otherProps = archive_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", archive_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "21 8 21 21 3 21 3 8"
  }), react_default.a.createElement("rect", {
    x: "1",
    y: "3",
    width: "22",
    height: "5"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "12",
    x2: "14",
    y2: "12"
  }));
};

archive_Archive.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
archive_Archive.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var archive = (archive_Archive);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-down-circle.js
function arrow_down_circle_extends() { arrow_down_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_down_circle_extends.apply(this, arguments); }

function arrow_down_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_down_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_down_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_down_circle_ArrowDownCircle = function ArrowDownCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_down_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_down_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polyline", {
    points: "8 12 12 16 16 12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "16"
  }));
};

arrow_down_circle_ArrowDownCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_down_circle_ArrowDownCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_down_circle = (arrow_down_circle_ArrowDownCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-down-left.js
function arrow_down_left_extends() { arrow_down_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_down_left_extends.apply(this, arguments); }

function arrow_down_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_down_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_down_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_down_left_ArrowDownLeft = function ArrowDownLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_down_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_down_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "17",
    y1: "7",
    x2: "7",
    y2: "17"
  }), react_default.a.createElement("polyline", {
    points: "17 17 7 17 7 7"
  }));
};

arrow_down_left_ArrowDownLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_down_left_ArrowDownLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_down_left = (arrow_down_left_ArrowDownLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-down-right.js
function arrow_down_right_extends() { arrow_down_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_down_right_extends.apply(this, arguments); }

function arrow_down_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_down_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_down_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_down_right_ArrowDownRight = function ArrowDownRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_down_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_down_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "7",
    y1: "7",
    x2: "17",
    y2: "17"
  }), react_default.a.createElement("polyline", {
    points: "17 7 17 17 7 17"
  }));
};

arrow_down_right_ArrowDownRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_down_right_ArrowDownRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_down_right = (arrow_down_right_ArrowDownRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-down.js
function arrow_down_extends() { arrow_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_down_extends.apply(this, arguments); }

function arrow_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_down_ArrowDown = function ArrowDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "12",
    y1: "5",
    x2: "12",
    y2: "19"
  }), react_default.a.createElement("polyline", {
    points: "19 12 12 19 5 12"
  }));
};

arrow_down_ArrowDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_down_ArrowDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_down = (arrow_down_ArrowDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-left-circle.js
function arrow_left_circle_extends() { arrow_left_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_left_circle_extends.apply(this, arguments); }

function arrow_left_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_left_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_left_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_left_circle_ArrowLeftCircle = function ArrowLeftCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_left_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_left_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polyline", {
    points: "12 8 8 12 12 16"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "12",
    x2: "8",
    y2: "12"
  }));
};

arrow_left_circle_ArrowLeftCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_left_circle_ArrowLeftCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_left_circle = (arrow_left_circle_ArrowLeftCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-left.js
function arrow_left_extends() { arrow_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_left_extends.apply(this, arguments); }

function arrow_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_left_ArrowLeft = function ArrowLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "19",
    y1: "12",
    x2: "5",
    y2: "12"
  }), react_default.a.createElement("polyline", {
    points: "12 19 5 12 12 5"
  }));
};

arrow_left_ArrowLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_left_ArrowLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_left = (arrow_left_ArrowLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-right-circle.js
function arrow_right_circle_extends() { arrow_right_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_right_circle_extends.apply(this, arguments); }

function arrow_right_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_right_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_right_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_right_circle_ArrowRightCircle = function ArrowRightCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_right_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_right_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polyline", {
    points: "12 16 16 12 12 8"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "16",
    y2: "12"
  }));
};

arrow_right_circle_ArrowRightCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_right_circle_ArrowRightCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_right_circle = (arrow_right_circle_ArrowRightCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-right.js
function arrow_right_extends() { arrow_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_right_extends.apply(this, arguments); }

function arrow_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_right_ArrowRight = function ArrowRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "5",
    y1: "12",
    x2: "19",
    y2: "12"
  }), react_default.a.createElement("polyline", {
    points: "12 5 19 12 12 19"
  }));
};

arrow_right_ArrowRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_right_ArrowRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_right = (arrow_right_ArrowRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-up-circle.js
function arrow_up_circle_extends() { arrow_up_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_up_circle_extends.apply(this, arguments); }

function arrow_up_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_up_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_up_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_up_circle_ArrowUpCircle = function ArrowUpCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_up_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_up_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polyline", {
    points: "16 12 12 8 8 12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "16",
    x2: "12",
    y2: "8"
  }));
};

arrow_up_circle_ArrowUpCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_up_circle_ArrowUpCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_up_circle = (arrow_up_circle_ArrowUpCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-up-left.js
function arrow_up_left_extends() { arrow_up_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_up_left_extends.apply(this, arguments); }

function arrow_up_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_up_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_up_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_up_left_ArrowUpLeft = function ArrowUpLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_up_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_up_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "17",
    y1: "17",
    x2: "7",
    y2: "7"
  }), react_default.a.createElement("polyline", {
    points: "7 17 7 7 17 7"
  }));
};

arrow_up_left_ArrowUpLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_up_left_ArrowUpLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_up_left = (arrow_up_left_ArrowUpLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-up-right.js
function arrow_up_right_extends() { arrow_up_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_up_right_extends.apply(this, arguments); }

function arrow_up_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_up_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_up_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_up_right_ArrowUpRight = function ArrowUpRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_up_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_up_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "7",
    y1: "17",
    x2: "17",
    y2: "7"
  }), react_default.a.createElement("polyline", {
    points: "7 7 17 7 17 17"
  }));
};

arrow_up_right_ArrowUpRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_up_right_ArrowUpRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_up_right = (arrow_up_right_ArrowUpRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/arrow-up.js
function arrow_up_extends() { arrow_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return arrow_up_extends.apply(this, arguments); }

function arrow_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = arrow_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function arrow_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var arrow_up_ArrowUp = function ArrowUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = arrow_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", arrow_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "12",
    y1: "19",
    x2: "12",
    y2: "5"
  }), react_default.a.createElement("polyline", {
    points: "5 12 12 5 19 12"
  }));
};

arrow_up_ArrowUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
arrow_up_ArrowUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var arrow_up = (arrow_up_ArrowUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/at-sign.js
function at_sign_extends() { at_sign_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return at_sign_extends.apply(this, arguments); }

function at_sign_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = at_sign_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function at_sign_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var at_sign_AtSign = function AtSign(props) {
  var color = props.color,
      size = props.size,
      otherProps = at_sign_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", at_sign_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "4"
  }), react_default.a.createElement("path", {
    d: "M16 8v5a3 3 0 0 0 6 0v-1a10 10 0 1 0-3.92 7.94"
  }));
};

at_sign_AtSign.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
at_sign_AtSign.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var at_sign = (at_sign_AtSign);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/award.js
function award_extends() { award_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return award_extends.apply(this, arguments); }

function award_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = award_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function award_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var award_Award = function Award(props) {
  var color = props.color,
      size = props.size,
      otherProps = award_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", award_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "8",
    r: "7"
  }), react_default.a.createElement("polyline", {
    points: "8.21 13.89 7 23 12 20 17 23 15.79 13.88"
  }));
};

award_Award.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
award_Award.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var award = (award_Award);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bar-chart-2.js
function bar_chart_2_extends() { bar_chart_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bar_chart_2_extends.apply(this, arguments); }

function bar_chart_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bar_chart_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bar_chart_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bar_chart_2_BarChart2 = function BarChart2(props) {
  var color = props.color,
      size = props.size,
      otherProps = bar_chart_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bar_chart_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "18",
    y1: "20",
    x2: "18",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "20",
    x2: "12",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "20",
    x2: "6",
    y2: "14"
  }));
};

bar_chart_2_BarChart2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bar_chart_2_BarChart2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bar_chart_2 = (bar_chart_2_BarChart2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bar-chart.js
function bar_chart_extends() { bar_chart_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bar_chart_extends.apply(this, arguments); }

function bar_chart_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bar_chart_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bar_chart_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bar_chart_BarChart = function BarChart(props) {
  var color = props.color,
      size = props.size,
      otherProps = bar_chart_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bar_chart_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "12",
    y1: "20",
    x2: "12",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "18",
    y1: "20",
    x2: "18",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "20",
    x2: "6",
    y2: "16"
  }));
};

bar_chart_BarChart.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bar_chart_BarChart.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bar_chart = (bar_chart_BarChart);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/battery-charging.js
function battery_charging_extends() { battery_charging_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return battery_charging_extends.apply(this, arguments); }

function battery_charging_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = battery_charging_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function battery_charging_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var battery_charging_BatteryCharging = function BatteryCharging(props) {
  var color = props.color,
      size = props.size,
      otherProps = battery_charging_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", battery_charging_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M5 18H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h3.19M15 6h2a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2h-3.19"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "13",
    x2: "23",
    y2: "11"
  }), react_default.a.createElement("polyline", {
    points: "11 6 7 12 13 12 9 18"
  }));
};

battery_charging_BatteryCharging.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
battery_charging_BatteryCharging.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var battery_charging = (battery_charging_BatteryCharging);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/battery.js
function battery_extends() { battery_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return battery_extends.apply(this, arguments); }

function battery_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = battery_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function battery_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var battery_Battery = function Battery(props) {
  var color = props.color,
      size = props.size,
      otherProps = battery_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", battery_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "1",
    y: "6",
    width: "18",
    height: "12",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "13",
    x2: "23",
    y2: "11"
  }));
};

battery_Battery.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
battery_Battery.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var battery = (battery_Battery);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bell-off.js
function bell_off_extends() { bell_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bell_off_extends.apply(this, arguments); }

function bell_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bell_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bell_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bell_off_BellOff = function BellOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = bell_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bell_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M13.73 21a2 2 0 0 1-3.46 0"
  }), react_default.a.createElement("path", {
    d: "M18.63 13A17.89 17.89 0 0 1 18 8"
  }), react_default.a.createElement("path", {
    d: "M6.26 6.26A5.86 5.86 0 0 0 6 8c0 7-3 9-3 9h14"
  }), react_default.a.createElement("path", {
    d: "M18 8a6 6 0 0 0-9.33-5"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }));
};

bell_off_BellOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bell_off_BellOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bell_off = (bell_off_BellOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bell.js
function bell_extends() { bell_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bell_extends.apply(this, arguments); }

function bell_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bell_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bell_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bell_Bell = function Bell(props) {
  var color = props.color,
      size = props.size,
      otherProps = bell_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bell_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"
  }), react_default.a.createElement("path", {
    d: "M13.73 21a2 2 0 0 1-3.46 0"
  }));
};

bell_Bell.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bell_Bell.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bell = (bell_Bell);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bluetooth.js
function bluetooth_extends() { bluetooth_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bluetooth_extends.apply(this, arguments); }

function bluetooth_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bluetooth_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bluetooth_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bluetooth_Bluetooth = function Bluetooth(props) {
  var color = props.color,
      size = props.size,
      otherProps = bluetooth_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bluetooth_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "6.5 6.5 17.5 17.5 12 23 12 1 17.5 6.5 6.5 17.5"
  }));
};

bluetooth_Bluetooth.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bluetooth_Bluetooth.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bluetooth = (bluetooth_Bluetooth);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bold.js
function bold_extends() { bold_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bold_extends.apply(this, arguments); }

function bold_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bold_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bold_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bold_Bold = function Bold(props) {
  var color = props.color,
      size = props.size,
      otherProps = bold_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bold_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M6 4h8a4 4 0 0 1 4 4 4 4 0 0 1-4 4H6z"
  }), react_default.a.createElement("path", {
    d: "M6 12h9a4 4 0 0 1 4 4 4 4 0 0 1-4 4H6z"
  }));
};

bold_Bold.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bold_Bold.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bold = (bold_Bold);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/book-open.js
function book_open_extends() { book_open_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return book_open_extends.apply(this, arguments); }

function book_open_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = book_open_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function book_open_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var book_open_BookOpen = function BookOpen(props) {
  var color = props.color,
      size = props.size,
      otherProps = book_open_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", book_open_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"
  }), react_default.a.createElement("path", {
    d: "M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"
  }));
};

book_open_BookOpen.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
book_open_BookOpen.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var book_open = (book_open_BookOpen);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/book.js
function book_extends() { book_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return book_extends.apply(this, arguments); }

function book_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = book_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function book_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var book_Book = function Book(props) {
  var color = props.color,
      size = props.size,
      otherProps = book_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", book_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M4 19.5A2.5 2.5 0 0 1 6.5 17H20"
  }), react_default.a.createElement("path", {
    d: "M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"
  }));
};

book_Book.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
book_Book.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var book = (book_Book);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/bookmark.js
function bookmark_extends() { bookmark_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return bookmark_extends.apply(this, arguments); }

function bookmark_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = bookmark_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function bookmark_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var bookmark_Bookmark = function Bookmark(props) {
  var color = props.color,
      size = props.size,
      otherProps = bookmark_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", bookmark_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M19 21l-7-5-7 5V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2z"
  }));
};

bookmark_Bookmark.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
bookmark_Bookmark.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var bookmark = (bookmark_Bookmark);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/box.js
function box_extends() { box_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return box_extends.apply(this, arguments); }

function box_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = box_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function box_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var box_Box = function Box(props) {
  var color = props.color,
      size = props.size,
      otherProps = box_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", box_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"
  }), react_default.a.createElement("polyline", {
    points: "3.27 6.96 12 12.01 20.73 6.96"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22.08",
    x2: "12",
    y2: "12"
  }));
};

box_Box.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
box_Box.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var box = (box_Box);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/briefcase.js
function briefcase_extends() { briefcase_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return briefcase_extends.apply(this, arguments); }

function briefcase_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = briefcase_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function briefcase_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var briefcase_Briefcase = function Briefcase(props) {
  var color = props.color,
      size = props.size,
      otherProps = briefcase_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", briefcase_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "2",
    y: "7",
    width: "20",
    height: "14",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("path", {
    d: "M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16"
  }));
};

briefcase_Briefcase.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
briefcase_Briefcase.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var briefcase = (briefcase_Briefcase);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/calendar.js
function calendar_extends() { calendar_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return calendar_extends.apply(this, arguments); }

function calendar_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = calendar_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function calendar_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var calendar_Calendar = function Calendar(props) {
  var color = props.color,
      size = props.size,
      otherProps = calendar_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", calendar_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "4",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "2",
    x2: "16",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "2",
    x2: "8",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "10",
    x2: "21",
    y2: "10"
  }));
};

calendar_Calendar.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
calendar_Calendar.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var calendar = (calendar_Calendar);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/camera-off.js
function camera_off_extends() { camera_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return camera_off_extends.apply(this, arguments); }

function camera_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = camera_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function camera_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var camera_off_CameraOff = function CameraOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = camera_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", camera_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }), react_default.a.createElement("path", {
    d: "M21 21H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h3m3-3h6l2 3h4a2 2 0 0 1 2 2v9.34m-7.72-2.06a4 4 0 1 1-5.56-5.56"
  }));
};

camera_off_CameraOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
camera_off_CameraOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var camera_off = (camera_off_CameraOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/camera.js
function camera_extends() { camera_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return camera_extends.apply(this, arguments); }

function camera_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = camera_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function camera_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var camera_Camera = function Camera(props) {
  var color = props.color,
      size = props.size,
      otherProps = camera_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", camera_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "13",
    r: "4"
  }));
};

camera_Camera.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
camera_Camera.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var camera = (camera_Camera);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cast.js
function cast_extends() { cast_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cast_extends.apply(this, arguments); }

function cast_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cast_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cast_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cast_Cast = function Cast(props) {
  var color = props.color,
      size = props.size,
      otherProps = cast_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cast_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M2 16.1A5 5 0 0 1 5.9 20M2 12.05A9 9 0 0 1 9.95 20M2 8V6a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2h-6"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "20",
    x2: "2",
    y2: "20"
  }));
};

cast_Cast.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cast_Cast.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cast = (cast_Cast);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/check-circle.js
function check_circle_extends() { check_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return check_circle_extends.apply(this, arguments); }

function check_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = check_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function check_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var check_circle_CheckCircle = function CheckCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = check_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", check_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22 11.08V12a10 10 0 1 1-5.93-9.14"
  }), react_default.a.createElement("polyline", {
    points: "22 4 12 14.01 9 11.01"
  }));
};

check_circle_CheckCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
check_circle_CheckCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var check_circle = (check_circle_CheckCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/check-square.js
function check_square_extends() { check_square_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return check_square_extends.apply(this, arguments); }

function check_square_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = check_square_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function check_square_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var check_square_CheckSquare = function CheckSquare(props) {
  var color = props.color,
      size = props.size,
      otherProps = check_square_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", check_square_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "9 11 12 14 22 4"
  }), react_default.a.createElement("path", {
    d: "M21 12v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11"
  }));
};

check_square_CheckSquare.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
check_square_CheckSquare.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var check_square = (check_square_CheckSquare);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/check.js
function check_extends() { check_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return check_extends.apply(this, arguments); }

function check_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = check_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function check_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var check_Check = function Check(props) {
  var color = props.color,
      size = props.size,
      otherProps = check_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", check_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "20 6 9 17 4 12"
  }));
};

check_Check.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
check_Check.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var check = (check_Check);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevron-down.js
function chevron_down_extends() { chevron_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevron_down_extends.apply(this, arguments); }

function chevron_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevron_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevron_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevron_down_ChevronDown = function ChevronDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevron_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevron_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "6 9 12 15 18 9"
  }));
};

chevron_down_ChevronDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevron_down_ChevronDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevron_down = (chevron_down_ChevronDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevron-left.js
function chevron_left_extends() { chevron_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevron_left_extends.apply(this, arguments); }

function chevron_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevron_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevron_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevron_left_ChevronLeft = function ChevronLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevron_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevron_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "15 18 9 12 15 6"
  }));
};

chevron_left_ChevronLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevron_left_ChevronLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevron_left = (chevron_left_ChevronLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevron-right.js
function chevron_right_extends() { chevron_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevron_right_extends.apply(this, arguments); }

function chevron_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevron_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevron_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevron_right_ChevronRight = function ChevronRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevron_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevron_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "9 18 15 12 9 6"
  }));
};

chevron_right_ChevronRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevron_right_ChevronRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevron_right = (chevron_right_ChevronRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevron-up.js
function chevron_up_extends() { chevron_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevron_up_extends.apply(this, arguments); }

function chevron_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevron_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevron_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevron_up_ChevronUp = function ChevronUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevron_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevron_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "18 15 12 9 6 15"
  }));
};

chevron_up_ChevronUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevron_up_ChevronUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevron_up = (chevron_up_ChevronUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevrons-down.js
function chevrons_down_extends() { chevrons_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevrons_down_extends.apply(this, arguments); }

function chevrons_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevrons_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevrons_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevrons_down_ChevronsDown = function ChevronsDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevrons_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevrons_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "7 13 12 18 17 13"
  }), react_default.a.createElement("polyline", {
    points: "7 6 12 11 17 6"
  }));
};

chevrons_down_ChevronsDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevrons_down_ChevronsDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevrons_down = (chevrons_down_ChevronsDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevrons-left.js
function chevrons_left_extends() { chevrons_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevrons_left_extends.apply(this, arguments); }

function chevrons_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevrons_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevrons_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevrons_left_ChevronsLeft = function ChevronsLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevrons_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevrons_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "11 17 6 12 11 7"
  }), react_default.a.createElement("polyline", {
    points: "18 17 13 12 18 7"
  }));
};

chevrons_left_ChevronsLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevrons_left_ChevronsLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevrons_left = (chevrons_left_ChevronsLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevrons-right.js
function chevrons_right_extends() { chevrons_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevrons_right_extends.apply(this, arguments); }

function chevrons_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevrons_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevrons_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevrons_right_ChevronsRight = function ChevronsRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevrons_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevrons_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "13 17 18 12 13 7"
  }), react_default.a.createElement("polyline", {
    points: "6 17 11 12 6 7"
  }));
};

chevrons_right_ChevronsRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevrons_right_ChevronsRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevrons_right = (chevrons_right_ChevronsRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chevrons-up.js
function chevrons_up_extends() { chevrons_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chevrons_up_extends.apply(this, arguments); }

function chevrons_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chevrons_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chevrons_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chevrons_up_ChevronsUp = function ChevronsUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = chevrons_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chevrons_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "17 11 12 6 7 11"
  }), react_default.a.createElement("polyline", {
    points: "17 18 12 13 7 18"
  }));
};

chevrons_up_ChevronsUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chevrons_up_ChevronsUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chevrons_up = (chevrons_up_ChevronsUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/chrome.js
function chrome_extends() { chrome_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return chrome_extends.apply(this, arguments); }

function chrome_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = chrome_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function chrome_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var chrome_Chrome = function Chrome(props) {
  var color = props.color,
      size = props.size,
      otherProps = chrome_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", chrome_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "21.17",
    y1: "8",
    x2: "12",
    y2: "8"
  }), react_default.a.createElement("line", {
    x1: "3.95",
    y1: "6.06",
    x2: "8.54",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "10.88",
    y1: "21.94",
    x2: "15.46",
    y2: "14"
  }));
};

chrome_Chrome.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
chrome_Chrome.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var chrome = (chrome_Chrome);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/circle.js
function circle_extends() { circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return circle_extends.apply(this, arguments); }

function circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var circle_Circle = function Circle(props) {
  var color = props.color,
      size = props.size,
      otherProps = circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }));
};

circle_Circle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
circle_Circle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var circle = (circle_Circle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/clipboard.js
function clipboard_extends() { clipboard_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return clipboard_extends.apply(this, arguments); }

function clipboard_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = clipboard_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function clipboard_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var clipboard_Clipboard = function Clipboard(props) {
  var color = props.color,
      size = props.size,
      otherProps = clipboard_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", clipboard_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"
  }), react_default.a.createElement("rect", {
    x: "8",
    y: "2",
    width: "8",
    height: "4",
    rx: "1",
    ry: "1"
  }));
};

clipboard_Clipboard.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
clipboard_Clipboard.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var clipboard = (clipboard_Clipboard);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/clock.js
function clock_extends() { clock_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return clock_extends.apply(this, arguments); }

function clock_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = clock_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function clock_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var clock_Clock = function Clock(props) {
  var color = props.color,
      size = props.size,
      otherProps = clock_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", clock_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polyline", {
    points: "12 6 12 12 16 14"
  }));
};

clock_Clock.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
clock_Clock.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var clock = (clock_Clock);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cloud-drizzle.js
function cloud_drizzle_extends() { cloud_drizzle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cloud_drizzle_extends.apply(this, arguments); }

function cloud_drizzle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cloud_drizzle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cloud_drizzle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cloud_drizzle_CloudDrizzle = function CloudDrizzle(props) {
  var color = props.color,
      size = props.size,
      otherProps = cloud_drizzle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cloud_drizzle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "8",
    y1: "19",
    x2: "8",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "13",
    x2: "8",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "19",
    x2: "16",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "13",
    x2: "16",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "21",
    x2: "12",
    y2: "23"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "15",
    x2: "12",
    y2: "17"
  }), react_default.a.createElement("path", {
    d: "M20 16.58A5 5 0 0 0 18 7h-1.26A8 8 0 1 0 4 15.25"
  }));
};

cloud_drizzle_CloudDrizzle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cloud_drizzle_CloudDrizzle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cloud_drizzle = (cloud_drizzle_CloudDrizzle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cloud-lightning.js
function cloud_lightning_extends() { cloud_lightning_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cloud_lightning_extends.apply(this, arguments); }

function cloud_lightning_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cloud_lightning_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cloud_lightning_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cloud_lightning_CloudLightning = function CloudLightning(props) {
  var color = props.color,
      size = props.size,
      otherProps = cloud_lightning_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cloud_lightning_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M19 16.9A5 5 0 0 0 18 7h-1.26a8 8 0 1 0-11.62 9"
  }), react_default.a.createElement("polyline", {
    points: "13 11 9 17 15 17 11 23"
  }));
};

cloud_lightning_CloudLightning.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cloud_lightning_CloudLightning.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cloud_lightning = (cloud_lightning_CloudLightning);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cloud-off.js
function cloud_off_extends() { cloud_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cloud_off_extends.apply(this, arguments); }

function cloud_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cloud_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cloud_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cloud_off_CloudOff = function CloudOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = cloud_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cloud_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22.61 16.95A5 5 0 0 0 18 10h-1.26a8 8 0 0 0-7.05-6M5 5a8 8 0 0 0 4 15h9a5 5 0 0 0 1.7-.3"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }));
};

cloud_off_CloudOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cloud_off_CloudOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cloud_off = (cloud_off_CloudOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cloud-rain.js
function cloud_rain_extends() { cloud_rain_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cloud_rain_extends.apply(this, arguments); }

function cloud_rain_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cloud_rain_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cloud_rain_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cloud_rain_CloudRain = function CloudRain(props) {
  var color = props.color,
      size = props.size,
      otherProps = cloud_rain_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cloud_rain_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "16",
    y1: "13",
    x2: "16",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "13",
    x2: "8",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "15",
    x2: "12",
    y2: "23"
  }), react_default.a.createElement("path", {
    d: "M20 16.58A5 5 0 0 0 18 7h-1.26A8 8 0 1 0 4 15.25"
  }));
};

cloud_rain_CloudRain.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cloud_rain_CloudRain.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cloud_rain = (cloud_rain_CloudRain);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cloud-snow.js
function cloud_snow_extends() { cloud_snow_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cloud_snow_extends.apply(this, arguments); }

function cloud_snow_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cloud_snow_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cloud_snow_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cloud_snow_CloudSnow = function CloudSnow(props) {
  var color = props.color,
      size = props.size,
      otherProps = cloud_snow_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cloud_snow_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M20 17.58A5 5 0 0 0 18 8h-1.26A8 8 0 1 0 4 16.25"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "16",
    x2: "8",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "20",
    x2: "8",
    y2: "20"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "18",
    x2: "12",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22",
    x2: "12",
    y2: "22"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "16",
    x2: "16",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "20",
    x2: "16",
    y2: "20"
  }));
};

cloud_snow_CloudSnow.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cloud_snow_CloudSnow.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cloud_snow = (cloud_snow_CloudSnow);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cloud.js
function cloud_extends() { cloud_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cloud_extends.apply(this, arguments); }

function cloud_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cloud_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cloud_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cloud_Cloud = function Cloud(props) {
  var color = props.color,
      size = props.size,
      otherProps = cloud_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cloud_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18 10h-1.26A8 8 0 1 0 9 20h9a5 5 0 0 0 0-10z"
  }));
};

cloud_Cloud.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cloud_Cloud.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cloud = (cloud_Cloud);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/code.js
function code_extends() { code_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return code_extends.apply(this, arguments); }

function code_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = code_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function code_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var code_Code = function Code(props) {
  var color = props.color,
      size = props.size,
      otherProps = code_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", code_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "16 18 22 12 16 6"
  }), react_default.a.createElement("polyline", {
    points: "8 6 2 12 8 18"
  }));
};

code_Code.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
code_Code.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var code = (code_Code);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/codepen.js
function codepen_extends() { codepen_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return codepen_extends.apply(this, arguments); }

function codepen_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = codepen_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function codepen_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var codepen_Codepen = function Codepen(props) {
  var color = props.color,
      size = props.size,
      otherProps = codepen_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", codepen_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "12 2 22 8.5 22 15.5 12 22 2 15.5 2 8.5 12 2"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22",
    x2: "12",
    y2: "15.5"
  }), react_default.a.createElement("polyline", {
    points: "22 8.5 12 15.5 2 8.5"
  }), react_default.a.createElement("polyline", {
    points: "2 15.5 12 8.5 22 15.5"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "2",
    x2: "12",
    y2: "8.5"
  }));
};

codepen_Codepen.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
codepen_Codepen.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var codepen = (codepen_Codepen);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/codesandbox.js
function codesandbox_extends() { codesandbox_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return codesandbox_extends.apply(this, arguments); }

function codesandbox_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = codesandbox_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function codesandbox_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var codesandbox_Codesandbox = function Codesandbox(props) {
  var color = props.color,
      size = props.size,
      otherProps = codesandbox_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", codesandbox_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"
  }), react_default.a.createElement("polyline", {
    points: "7.5 4.21 12 6.81 16.5 4.21"
  }), react_default.a.createElement("polyline", {
    points: "7.5 19.79 7.5 14.6 3 12"
  }), react_default.a.createElement("polyline", {
    points: "21 12 16.5 14.6 16.5 19.79"
  }), react_default.a.createElement("polyline", {
    points: "3.27 6.96 12 12.01 20.73 6.96"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22.08",
    x2: "12",
    y2: "12"
  }));
};

codesandbox_Codesandbox.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
codesandbox_Codesandbox.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var codesandbox = (codesandbox_Codesandbox);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/coffee.js
function coffee_extends() { coffee_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return coffee_extends.apply(this, arguments); }

function coffee_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = coffee_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function coffee_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var coffee_Coffee = function Coffee(props) {
  var color = props.color,
      size = props.size,
      otherProps = coffee_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", coffee_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18 8h1a4 4 0 0 1 0 8h-1"
  }), react_default.a.createElement("path", {
    d: "M2 8h16v9a4 4 0 0 1-4 4H6a4 4 0 0 1-4-4V8z"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "1",
    x2: "6",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "1",
    x2: "10",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "14",
    y1: "1",
    x2: "14",
    y2: "4"
  }));
};

coffee_Coffee.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
coffee_Coffee.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var coffee = (coffee_Coffee);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/columns.js
function columns_extends() { columns_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return columns_extends.apply(this, arguments); }

function columns_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = columns_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function columns_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var columns_Columns = function Columns(props) {
  var color = props.color,
      size = props.size,
      otherProps = columns_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", columns_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M12 3h7a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-7m0-18H5a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h7m0-18v18"
  }));
};

columns_Columns.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
columns_Columns.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var columns = (columns_Columns);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/command.js
function command_extends() { command_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return command_extends.apply(this, arguments); }

function command_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = command_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function command_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var command_Command = function Command(props) {
  var color = props.color,
      size = props.size,
      otherProps = command_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", command_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18 3a3 3 0 0 0-3 3v12a3 3 0 0 0 3 3 3 3 0 0 0 3-3 3 3 0 0 0-3-3H6a3 3 0 0 0-3 3 3 3 0 0 0 3 3 3 3 0 0 0 3-3V6a3 3 0 0 0-3-3 3 3 0 0 0-3 3 3 3 0 0 0 3 3h12a3 3 0 0 0 3-3 3 3 0 0 0-3-3z"
  }));
};

command_Command.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
command_Command.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var command = (command_Command);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/compass.js
function compass_extends() { compass_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return compass_extends.apply(this, arguments); }

function compass_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = compass_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function compass_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var compass_Compass = function Compass(props) {
  var color = props.color,
      size = props.size,
      otherProps = compass_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", compass_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polygon", {
    points: "16.24 7.76 14.12 14.12 7.76 16.24 9.88 9.88 16.24 7.76"
  }));
};

compass_Compass.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
compass_Compass.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var compass = (compass_Compass);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/copy.js
function copy_extends() { copy_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return copy_extends.apply(this, arguments); }

function copy_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = copy_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function copy_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var copy_Copy = function Copy(props) {
  var color = props.color,
      size = props.size,
      otherProps = copy_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", copy_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "9",
    y: "9",
    width: "13",
    height: "13",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("path", {
    d: "M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"
  }));
};

copy_Copy.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
copy_Copy.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var copy = (copy_Copy);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-down-left.js
function corner_down_left_extends() { corner_down_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_down_left_extends.apply(this, arguments); }

function corner_down_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_down_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_down_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_down_left_CornerDownLeft = function CornerDownLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_down_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_down_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "9 10 4 15 9 20"
  }), react_default.a.createElement("path", {
    d: "M20 4v7a4 4 0 0 1-4 4H4"
  }));
};

corner_down_left_CornerDownLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_down_left_CornerDownLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_down_left = (corner_down_left_CornerDownLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-down-right.js
function corner_down_right_extends() { corner_down_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_down_right_extends.apply(this, arguments); }

function corner_down_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_down_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_down_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_down_right_CornerDownRight = function CornerDownRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_down_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_down_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "15 10 20 15 15 20"
  }), react_default.a.createElement("path", {
    d: "M4 4v7a4 4 0 0 0 4 4h12"
  }));
};

corner_down_right_CornerDownRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_down_right_CornerDownRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_down_right = (corner_down_right_CornerDownRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-left-down.js
function corner_left_down_extends() { corner_left_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_left_down_extends.apply(this, arguments); }

function corner_left_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_left_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_left_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_left_down_CornerLeftDown = function CornerLeftDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_left_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_left_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "14 15 9 20 4 15"
  }), react_default.a.createElement("path", {
    d: "M20 4h-7a4 4 0 0 0-4 4v12"
  }));
};

corner_left_down_CornerLeftDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_left_down_CornerLeftDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_left_down = (corner_left_down_CornerLeftDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-left-up.js
function corner_left_up_extends() { corner_left_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_left_up_extends.apply(this, arguments); }

function corner_left_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_left_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_left_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_left_up_CornerLeftUp = function CornerLeftUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_left_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_left_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "14 9 9 4 4 9"
  }), react_default.a.createElement("path", {
    d: "M20 20h-7a4 4 0 0 1-4-4V4"
  }));
};

corner_left_up_CornerLeftUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_left_up_CornerLeftUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_left_up = (corner_left_up_CornerLeftUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-right-down.js
function corner_right_down_extends() { corner_right_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_right_down_extends.apply(this, arguments); }

function corner_right_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_right_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_right_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_right_down_CornerRightDown = function CornerRightDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_right_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_right_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "10 15 15 20 20 15"
  }), react_default.a.createElement("path", {
    d: "M4 4h7a4 4 0 0 1 4 4v12"
  }));
};

corner_right_down_CornerRightDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_right_down_CornerRightDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_right_down = (corner_right_down_CornerRightDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-right-up.js
function corner_right_up_extends() { corner_right_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_right_up_extends.apply(this, arguments); }

function corner_right_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_right_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_right_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_right_up_CornerRightUp = function CornerRightUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_right_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_right_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "10 9 15 4 20 9"
  }), react_default.a.createElement("path", {
    d: "M4 20h7a4 4 0 0 0 4-4V4"
  }));
};

corner_right_up_CornerRightUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_right_up_CornerRightUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_right_up = (corner_right_up_CornerRightUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-up-left.js
function corner_up_left_extends() { corner_up_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_up_left_extends.apply(this, arguments); }

function corner_up_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_up_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_up_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_up_left_CornerUpLeft = function CornerUpLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_up_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_up_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "9 14 4 9 9 4"
  }), react_default.a.createElement("path", {
    d: "M20 20v-7a4 4 0 0 0-4-4H4"
  }));
};

corner_up_left_CornerUpLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_up_left_CornerUpLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_up_left = (corner_up_left_CornerUpLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/corner-up-right.js
function corner_up_right_extends() { corner_up_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return corner_up_right_extends.apply(this, arguments); }

function corner_up_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = corner_up_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function corner_up_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var corner_up_right_CornerUpRight = function CornerUpRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = corner_up_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", corner_up_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "15 14 20 9 15 4"
  }), react_default.a.createElement("path", {
    d: "M4 20v-7a4 4 0 0 1 4-4h12"
  }));
};

corner_up_right_CornerUpRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
corner_up_right_CornerUpRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var corner_up_right = (corner_up_right_CornerUpRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/cpu.js
function cpu_extends() { cpu_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return cpu_extends.apply(this, arguments); }

function cpu_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = cpu_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function cpu_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var cpu_Cpu = function Cpu(props) {
  var color = props.color,
      size = props.size,
      otherProps = cpu_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", cpu_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "4",
    y: "4",
    width: "16",
    height: "16",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("rect", {
    x: "9",
    y: "9",
    width: "6",
    height: "6"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "1",
    x2: "9",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "1",
    x2: "15",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "20",
    x2: "9",
    y2: "23"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "20",
    x2: "15",
    y2: "23"
  }), react_default.a.createElement("line", {
    x1: "20",
    y1: "9",
    x2: "23",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "20",
    y1: "14",
    x2: "23",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "9",
    x2: "4",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "14",
    x2: "4",
    y2: "14"
  }));
};

cpu_Cpu.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
cpu_Cpu.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var cpu = (cpu_Cpu);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/credit-card.js
function credit_card_extends() { credit_card_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return credit_card_extends.apply(this, arguments); }

function credit_card_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = credit_card_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function credit_card_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var credit_card_CreditCard = function CreditCard(props) {
  var color = props.color,
      size = props.size,
      otherProps = credit_card_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", credit_card_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "1",
    y: "4",
    width: "22",
    height: "16",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "10",
    x2: "23",
    y2: "10"
  }));
};

credit_card_CreditCard.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
credit_card_CreditCard.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var credit_card = (credit_card_CreditCard);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/crop.js
function crop_extends() { crop_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return crop_extends.apply(this, arguments); }

function crop_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = crop_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function crop_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var crop_Crop = function Crop(props) {
  var color = props.color,
      size = props.size,
      otherProps = crop_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", crop_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M6.13 1L6 16a2 2 0 0 0 2 2h15"
  }), react_default.a.createElement("path", {
    d: "M1 6.13L16 6a2 2 0 0 1 2 2v15"
  }));
};

crop_Crop.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
crop_Crop.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var crop = (crop_Crop);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/crosshair.js
function crosshair_extends() { crosshair_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return crosshair_extends.apply(this, arguments); }

function crosshair_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = crosshair_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function crosshair_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var crosshair_Crosshair = function Crosshair(props) {
  var color = props.color,
      size = props.size,
      otherProps = crosshair_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", crosshair_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "22",
    y1: "12",
    x2: "18",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "12",
    x2: "2",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "6",
    x2: "12",
    y2: "2"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22",
    x2: "12",
    y2: "18"
  }));
};

crosshair_Crosshair.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
crosshair_Crosshair.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var crosshair = (crosshair_Crosshair);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/database.js
function database_extends() { database_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return database_extends.apply(this, arguments); }

function database_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = database_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function database_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var database_Database = function Database(props) {
  var color = props.color,
      size = props.size,
      otherProps = database_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", database_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("ellipse", {
    cx: "12",
    cy: "5",
    rx: "9",
    ry: "3"
  }), react_default.a.createElement("path", {
    d: "M21 12c0 1.66-4 3-9 3s-9-1.34-9-3"
  }), react_default.a.createElement("path", {
    d: "M3 5v14c0 1.66 4 3 9 3s9-1.34 9-3V5"
  }));
};

database_Database.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
database_Database.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var database = (database_Database);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/delete.js
function delete_extends() { delete_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return delete_extends.apply(this, arguments); }

function delete_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = delete_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function delete_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var delete_Delete = function Delete(props) {
  var color = props.color,
      size = props.size,
      otherProps = delete_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", delete_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 4H8l-7 8 7 8h13a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2z"
  }), react_default.a.createElement("line", {
    x1: "18",
    y1: "9",
    x2: "12",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "9",
    x2: "18",
    y2: "15"
  }));
};

delete_Delete.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
delete_Delete.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_delete = (delete_Delete);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/disc.js
function disc_extends() { disc_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return disc_extends.apply(this, arguments); }

function disc_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = disc_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function disc_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var disc_Disc = function Disc(props) {
  var color = props.color,
      size = props.size,
      otherProps = disc_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", disc_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "3"
  }));
};

disc_Disc.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
disc_Disc.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var disc = (disc_Disc);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/dollar-sign.js
function dollar_sign_extends() { dollar_sign_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return dollar_sign_extends.apply(this, arguments); }

function dollar_sign_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = dollar_sign_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function dollar_sign_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var dollar_sign_DollarSign = function DollarSign(props) {
  var color = props.color,
      size = props.size,
      otherProps = dollar_sign_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", dollar_sign_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "12",
    y1: "1",
    x2: "12",
    y2: "23"
  }), react_default.a.createElement("path", {
    d: "M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"
  }));
};

dollar_sign_DollarSign.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
dollar_sign_DollarSign.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var dollar_sign = (dollar_sign_DollarSign);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/download-cloud.js
function download_cloud_extends() { download_cloud_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return download_cloud_extends.apply(this, arguments); }

function download_cloud_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = download_cloud_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function download_cloud_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var download_cloud_DownloadCloud = function DownloadCloud(props) {
  var color = props.color,
      size = props.size,
      otherProps = download_cloud_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", download_cloud_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "8 17 12 21 16 17"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "12",
    x2: "12",
    y2: "21"
  }), react_default.a.createElement("path", {
    d: "M20.88 18.09A5 5 0 0 0 18 9h-1.26A8 8 0 1 0 3 16.29"
  }));
};

download_cloud_DownloadCloud.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
download_cloud_DownloadCloud.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var download_cloud = (download_cloud_DownloadCloud);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/download.js
function download_extends() { download_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return download_extends.apply(this, arguments); }

function download_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = download_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function download_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var download_Download = function Download(props) {
  var color = props.color,
      size = props.size,
      otherProps = download_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", download_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"
  }), react_default.a.createElement("polyline", {
    points: "7 10 12 15 17 10"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "15",
    x2: "12",
    y2: "3"
  }));
};

download_Download.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
download_Download.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var download = (download_Download);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/droplet.js
function droplet_extends() { droplet_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return droplet_extends.apply(this, arguments); }

function droplet_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = droplet_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function droplet_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var droplet_Droplet = function Droplet(props) {
  var color = props.color,
      size = props.size,
      otherProps = droplet_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", droplet_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M12 2.69l5.66 5.66a8 8 0 1 1-11.31 0z"
  }));
};

droplet_Droplet.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
droplet_Droplet.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var droplet = (droplet_Droplet);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/edit-2.js
function edit_2_extends() { edit_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return edit_2_extends.apply(this, arguments); }

function edit_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = edit_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function edit_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var edit_2_Edit2 = function Edit2(props) {
  var color = props.color,
      size = props.size,
      otherProps = edit_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", edit_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z"
  }));
};

edit_2_Edit2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
edit_2_Edit2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var edit_2 = (edit_2_Edit2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/edit-3.js
function edit_3_extends() { edit_3_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return edit_3_extends.apply(this, arguments); }

function edit_3_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = edit_3_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function edit_3_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var edit_3_Edit3 = function Edit3(props) {
  var color = props.color,
      size = props.size,
      otherProps = edit_3_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", edit_3_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M12 20h9"
  }), react_default.a.createElement("path", {
    d: "M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"
  }));
};

edit_3_Edit3.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
edit_3_Edit3.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var edit_3 = (edit_3_Edit3);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/edit.js
function edit_extends() { edit_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return edit_extends.apply(this, arguments); }

function edit_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = edit_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function edit_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var edit_Edit = function Edit(props) {
  var color = props.color,
      size = props.size,
      otherProps = edit_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", edit_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"
  }), react_default.a.createElement("path", {
    d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"
  }));
};

edit_Edit.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
edit_Edit.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var edit = (edit_Edit);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/external-link.js
function external_link_extends() { external_link_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return external_link_extends.apply(this, arguments); }

function external_link_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = external_link_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function external_link_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var external_link_ExternalLink = function ExternalLink(props) {
  var color = props.color,
      size = props.size,
      otherProps = external_link_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", external_link_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18 13v6a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h6"
  }), react_default.a.createElement("polyline", {
    points: "15 3 21 3 21 9"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "14",
    x2: "21",
    y2: "3"
  }));
};

external_link_ExternalLink.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
external_link_ExternalLink.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var external_link = (external_link_ExternalLink);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/eye-off.js
function eye_off_extends() { eye_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return eye_off_extends.apply(this, arguments); }

function eye_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = eye_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function eye_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var eye_off_EyeOff = function EyeOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = eye_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", eye_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }));
};

eye_off_EyeOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
eye_off_EyeOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var eye_off = (eye_off_EyeOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/eye.js
function eye_extends() { eye_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return eye_extends.apply(this, arguments); }

function eye_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = eye_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function eye_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var eye_Eye = function Eye(props) {
  var color = props.color,
      size = props.size,
      otherProps = eye_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", eye_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "3"
  }));
};

eye_Eye.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
eye_Eye.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var eye = (eye_Eye);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/facebook.js
function facebook_extends() { facebook_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return facebook_extends.apply(this, arguments); }

function facebook_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = facebook_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function facebook_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var facebook_Facebook = function Facebook(props) {
  var color = props.color,
      size = props.size,
      otherProps = facebook_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", facebook_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18 2h-3a5 5 0 0 0-5 5v3H7v4h3v8h4v-8h3l1-4h-4V7a1 1 0 0 1 1-1h3z"
  }));
};

facebook_Facebook.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
facebook_Facebook.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var facebook = (facebook_Facebook);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/fast-forward.js
function fast_forward_extends() { fast_forward_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return fast_forward_extends.apply(this, arguments); }

function fast_forward_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = fast_forward_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function fast_forward_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var fast_forward_FastForward = function FastForward(props) {
  var color = props.color,
      size = props.size,
      otherProps = fast_forward_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", fast_forward_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "13 19 22 12 13 5 13 19"
  }), react_default.a.createElement("polygon", {
    points: "2 19 11 12 2 5 2 19"
  }));
};

fast_forward_FastForward.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
fast_forward_FastForward.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var fast_forward = (fast_forward_FastForward);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/feather.js
function feather_extends() { feather_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return feather_extends.apply(this, arguments); }

function feather_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = feather_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function feather_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var feather_Feather = function Feather(props) {
  var color = props.color,
      size = props.size,
      otherProps = feather_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", feather_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M20.24 12.24a6 6 0 0 0-8.49-8.49L5 10.5V19h8.5z"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "8",
    x2: "2",
    y2: "22"
  }), react_default.a.createElement("line", {
    x1: "17.5",
    y1: "15",
    x2: "9",
    y2: "15"
  }));
};

feather_Feather.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
feather_Feather.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var feather = (feather_Feather);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/figma.js
function figma_extends() { figma_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return figma_extends.apply(this, arguments); }

function figma_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = figma_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function figma_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var figma_Figma = function Figma(props) {
  var color = props.color,
      size = props.size,
      otherProps = figma_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", figma_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M5 5.5A3.5 3.5 0 0 1 8.5 2H12v7H8.5A3.5 3.5 0 0 1 5 5.5z"
  }), react_default.a.createElement("path", {
    d: "M12 2h3.5a3.5 3.5 0 1 1 0 7H12V2z"
  }), react_default.a.createElement("path", {
    d: "M12 12.5a3.5 3.5 0 1 1 7 0 3.5 3.5 0 1 1-7 0z"
  }), react_default.a.createElement("path", {
    d: "M5 19.5A3.5 3.5 0 0 1 8.5 16H12v3.5a3.5 3.5 0 1 1-7 0z"
  }), react_default.a.createElement("path", {
    d: "M5 12.5A3.5 3.5 0 0 1 8.5 9H12v7H8.5A3.5 3.5 0 0 1 5 12.5z"
  }));
};

figma_Figma.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
figma_Figma.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var figma = (figma_Figma);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/file-minus.js
function file_minus_extends() { file_minus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return file_minus_extends.apply(this, arguments); }

function file_minus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = file_minus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function file_minus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var file_minus_FileMinus = function FileMinus(props) {
  var color = props.color,
      size = props.size,
      otherProps = file_minus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", file_minus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"
  }), react_default.a.createElement("polyline", {
    points: "14 2 14 8 20 8"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "15",
    x2: "15",
    y2: "15"
  }));
};

file_minus_FileMinus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
file_minus_FileMinus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var file_minus = (file_minus_FileMinus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/file-plus.js
function file_plus_extends() { file_plus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return file_plus_extends.apply(this, arguments); }

function file_plus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = file_plus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function file_plus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var file_plus_FilePlus = function FilePlus(props) {
  var color = props.color,
      size = props.size,
      otherProps = file_plus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", file_plus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"
  }), react_default.a.createElement("polyline", {
    points: "14 2 14 8 20 8"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "18",
    x2: "12",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "15",
    x2: "15",
    y2: "15"
  }));
};

file_plus_FilePlus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
file_plus_FilePlus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var file_plus = (file_plus_FilePlus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/file-text.js
function file_text_extends() { file_text_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return file_text_extends.apply(this, arguments); }

function file_text_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = file_text_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function file_text_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var file_text_FileText = function FileText(props) {
  var color = props.color,
      size = props.size,
      otherProps = file_text_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", file_text_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"
  }), react_default.a.createElement("polyline", {
    points: "14 2 14 8 20 8"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "13",
    x2: "8",
    y2: "13"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "17",
    x2: "8",
    y2: "17"
  }), react_default.a.createElement("polyline", {
    points: "10 9 9 9 8 9"
  }));
};

file_text_FileText.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
file_text_FileText.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var file_text = (file_text_FileText);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/file.js
function file_extends() { file_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return file_extends.apply(this, arguments); }

function file_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = file_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function file_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var file_File = function File(props) {
  var color = props.color,
      size = props.size,
      otherProps = file_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", file_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M13 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V9z"
  }), react_default.a.createElement("polyline", {
    points: "13 2 13 9 20 9"
  }));
};

file_File.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
file_File.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var file = (file_File);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/film.js
function film_extends() { film_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return film_extends.apply(this, arguments); }

function film_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = film_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function film_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var film_Film = function Film(props) {
  var color = props.color,
      size = props.size,
      otherProps = film_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", film_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "2",
    y: "2",
    width: "20",
    height: "20",
    rx: "2.18",
    ry: "2.18"
  }), react_default.a.createElement("line", {
    x1: "7",
    y1: "2",
    x2: "7",
    y2: "22"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "2",
    x2: "17",
    y2: "22"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "12",
    x2: "22",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "7",
    x2: "7",
    y2: "7"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "17",
    x2: "7",
    y2: "17"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "17",
    x2: "22",
    y2: "17"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "7",
    x2: "22",
    y2: "7"
  }));
};

film_Film.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
film_Film.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var film = (film_Film);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/filter.js
function filter_extends() { filter_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return filter_extends.apply(this, arguments); }

function filter_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = filter_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function filter_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var filter_Filter = function Filter(props) {
  var color = props.color,
      size = props.size,
      otherProps = filter_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", filter_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "22 3 2 3 10 12.46 10 19 14 21 14 12.46 22 3"
  }));
};

filter_Filter.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
filter_Filter.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var filter = (filter_Filter);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/flag.js
function flag_extends() { flag_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return flag_extends.apply(this, arguments); }

function flag_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = flag_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function flag_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var flag_Flag = function Flag(props) {
  var color = props.color,
      size = props.size,
      otherProps = flag_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", flag_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1z"
  }), react_default.a.createElement("line", {
    x1: "4",
    y1: "22",
    x2: "4",
    y2: "15"
  }));
};

flag_Flag.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
flag_Flag.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var flag = (flag_Flag);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/folder-minus.js
function folder_minus_extends() { folder_minus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return folder_minus_extends.apply(this, arguments); }

function folder_minus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = folder_minus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function folder_minus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var folder_minus_FolderMinus = function FolderMinus(props) {
  var color = props.color,
      size = props.size,
      otherProps = folder_minus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", folder_minus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "14",
    x2: "15",
    y2: "14"
  }));
};

folder_minus_FolderMinus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
folder_minus_FolderMinus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var folder_minus = (folder_minus_FolderMinus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/folder-plus.js
function folder_plus_extends() { folder_plus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return folder_plus_extends.apply(this, arguments); }

function folder_plus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = folder_plus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function folder_plus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var folder_plus_FolderPlus = function FolderPlus(props) {
  var color = props.color,
      size = props.size,
      otherProps = folder_plus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", folder_plus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "11",
    x2: "12",
    y2: "17"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "14",
    x2: "15",
    y2: "14"
  }));
};

folder_plus_FolderPlus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
folder_plus_FolderPlus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var folder_plus = (folder_plus_FolderPlus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/folder.js
function folder_extends() { folder_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return folder_extends.apply(this, arguments); }

function folder_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = folder_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function folder_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var folder_Folder = function Folder(props) {
  var color = props.color,
      size = props.size,
      otherProps = folder_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", folder_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"
  }));
};

folder_Folder.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
folder_Folder.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var folder = (folder_Folder);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/framer.js
function framer_extends() { framer_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return framer_extends.apply(this, arguments); }

function framer_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = framer_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function framer_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var framer_Framer = function Framer(props) {
  var color = props.color,
      size = props.size,
      otherProps = framer_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", framer_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M5 16V9h14V2H5l14 14h-7m-7 0l7 7v-7m-7 0h7"
  }));
};

framer_Framer.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
framer_Framer.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var framer = (framer_Framer);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/frown.js
function frown_extends() { frown_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return frown_extends.apply(this, arguments); }

function frown_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = frown_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function frown_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var frown_Frown = function Frown(props) {
  var color = props.color,
      size = props.size,
      otherProps = frown_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", frown_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("path", {
    d: "M16 16s-1.5-2-4-2-4 2-4 2"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "9",
    x2: "9.01",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "9",
    x2: "15.01",
    y2: "9"
  }));
};

frown_Frown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
frown_Frown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var frown = (frown_Frown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/gift.js
function gift_extends() { gift_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return gift_extends.apply(this, arguments); }

function gift_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = gift_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function gift_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var gift_Gift = function Gift(props) {
  var color = props.color,
      size = props.size,
      otherProps = gift_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", gift_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "20 12 20 22 4 22 4 12"
  }), react_default.a.createElement("rect", {
    x: "2",
    y: "7",
    width: "20",
    height: "5"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22",
    x2: "12",
    y2: "7"
  }), react_default.a.createElement("path", {
    d: "M12 7H7.5a2.5 2.5 0 0 1 0-5C11 2 12 7 12 7z"
  }), react_default.a.createElement("path", {
    d: "M12 7h4.5a2.5 2.5 0 0 0 0-5C13 2 12 7 12 7z"
  }));
};

gift_Gift.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
gift_Gift.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var gift = (gift_Gift);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/git-branch.js
function git_branch_extends() { git_branch_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return git_branch_extends.apply(this, arguments); }

function git_branch_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = git_branch_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function git_branch_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var git_branch_GitBranch = function GitBranch(props) {
  var color = props.color,
      size = props.size,
      otherProps = git_branch_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", git_branch_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "6",
    y1: "3",
    x2: "6",
    y2: "15"
  }), react_default.a.createElement("circle", {
    cx: "18",
    cy: "6",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "6",
    cy: "18",
    r: "3"
  }), react_default.a.createElement("path", {
    d: "M18 9a9 9 0 0 1-9 9"
  }));
};

git_branch_GitBranch.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
git_branch_GitBranch.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var git_branch = (git_branch_GitBranch);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/git-commit.js
function git_commit_extends() { git_commit_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return git_commit_extends.apply(this, arguments); }

function git_commit_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = git_commit_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function git_commit_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var git_commit_GitCommit = function GitCommit(props) {
  var color = props.color,
      size = props.size,
      otherProps = git_commit_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", git_commit_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "1.05",
    y1: "12",
    x2: "7",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "17.01",
    y1: "12",
    x2: "22.96",
    y2: "12"
  }));
};

git_commit_GitCommit.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
git_commit_GitCommit.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var git_commit = (git_commit_GitCommit);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/git-merge.js
function git_merge_extends() { git_merge_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return git_merge_extends.apply(this, arguments); }

function git_merge_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = git_merge_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function git_merge_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var git_merge_GitMerge = function GitMerge(props) {
  var color = props.color,
      size = props.size,
      otherProps = git_merge_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", git_merge_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "18",
    cy: "18",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "6",
    cy: "6",
    r: "3"
  }), react_default.a.createElement("path", {
    d: "M6 21V9a9 9 0 0 0 9 9"
  }));
};

git_merge_GitMerge.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
git_merge_GitMerge.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var git_merge = (git_merge_GitMerge);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/git-pull-request.js
function git_pull_request_extends() { git_pull_request_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return git_pull_request_extends.apply(this, arguments); }

function git_pull_request_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = git_pull_request_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function git_pull_request_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var git_pull_request_GitPullRequest = function GitPullRequest(props) {
  var color = props.color,
      size = props.size,
      otherProps = git_pull_request_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", git_pull_request_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "18",
    cy: "18",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "6",
    cy: "6",
    r: "3"
  }), react_default.a.createElement("path", {
    d: "M13 6h3a2 2 0 0 1 2 2v7"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "9",
    x2: "6",
    y2: "21"
  }));
};

git_pull_request_GitPullRequest.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
git_pull_request_GitPullRequest.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var git_pull_request = (git_pull_request_GitPullRequest);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/github.js
function github_extends() { github_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return github_extends.apply(this, arguments); }

function github_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = github_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function github_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var github_GitHub = function GitHub(props) {
  var color = props.color,
      size = props.size,
      otherProps = github_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", github_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22"
  }));
};

github_GitHub.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
github_GitHub.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var github = (github_GitHub);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/gitlab.js
function gitlab_extends() { gitlab_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return gitlab_extends.apply(this, arguments); }

function gitlab_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = gitlab_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function gitlab_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var gitlab_Gitlab = function Gitlab(props) {
  var color = props.color,
      size = props.size,
      otherProps = gitlab_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", gitlab_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22.65 14.39L12 22.13 1.35 14.39a.84.84 0 0 1-.3-.94l1.22-3.78 2.44-7.51A.42.42 0 0 1 4.82 2a.43.43 0 0 1 .58 0 .42.42 0 0 1 .11.18l2.44 7.49h8.1l2.44-7.51A.42.42 0 0 1 18.6 2a.43.43 0 0 1 .58 0 .42.42 0 0 1 .11.18l2.44 7.51L23 13.45a.84.84 0 0 1-.35.94z"
  }));
};

gitlab_Gitlab.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
gitlab_Gitlab.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var gitlab = (gitlab_Gitlab);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/globe.js
function globe_extends() { globe_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return globe_extends.apply(this, arguments); }

function globe_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = globe_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function globe_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var globe_Globe = function Globe(props) {
  var color = props.color,
      size = props.size,
      otherProps = globe_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", globe_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "12",
    x2: "22",
    y2: "12"
  }), react_default.a.createElement("path", {
    d: "M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"
  }));
};

globe_Globe.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
globe_Globe.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var globe = (globe_Globe);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/grid.js
function grid_extends() { grid_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return grid_extends.apply(this, arguments); }

function grid_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = grid_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function grid_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var grid_Grid = function Grid(props) {
  var color = props.color,
      size = props.size,
      otherProps = grid_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", grid_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "7",
    height: "7"
  }), react_default.a.createElement("rect", {
    x: "14",
    y: "3",
    width: "7",
    height: "7"
  }), react_default.a.createElement("rect", {
    x: "14",
    y: "14",
    width: "7",
    height: "7"
  }), react_default.a.createElement("rect", {
    x: "3",
    y: "14",
    width: "7",
    height: "7"
  }));
};

grid_Grid.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
grid_Grid.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var grid = (grid_Grid);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/hard-drive.js
function hard_drive_extends() { hard_drive_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return hard_drive_extends.apply(this, arguments); }

function hard_drive_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = hard_drive_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function hard_drive_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var hard_drive_HardDrive = function HardDrive(props) {
  var color = props.color,
      size = props.size,
      otherProps = hard_drive_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", hard_drive_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "22",
    y1: "12",
    x2: "2",
    y2: "12"
  }), react_default.a.createElement("path", {
    d: "M5.45 5.11L2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "16",
    x2: "6",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "16",
    x2: "10",
    y2: "16"
  }));
};

hard_drive_HardDrive.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
hard_drive_HardDrive.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var hard_drive = (hard_drive_HardDrive);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/hash.js
function hash_extends() { hash_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return hash_extends.apply(this, arguments); }

function hash_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = hash_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function hash_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var hash_Hash = function Hash(props) {
  var color = props.color,
      size = props.size,
      otherProps = hash_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", hash_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "4",
    y1: "9",
    x2: "20",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "4",
    y1: "15",
    x2: "20",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "3",
    x2: "8",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "3",
    x2: "14",
    y2: "21"
  }));
};

hash_Hash.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
hash_Hash.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var hash = (hash_Hash);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/headphones.js
function headphones_extends() { headphones_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return headphones_extends.apply(this, arguments); }

function headphones_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = headphones_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function headphones_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var headphones_Headphones = function Headphones(props) {
  var color = props.color,
      size = props.size,
      otherProps = headphones_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", headphones_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M3 18v-6a9 9 0 0 1 18 0v6"
  }), react_default.a.createElement("path", {
    d: "M21 19a2 2 0 0 1-2 2h-1a2 2 0 0 1-2-2v-3a2 2 0 0 1 2-2h3zM3 19a2 2 0 0 0 2 2h1a2 2 0 0 0 2-2v-3a2 2 0 0 0-2-2H3z"
  }));
};

headphones_Headphones.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
headphones_Headphones.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var headphones = (headphones_Headphones);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/heart.js
function heart_extends() { heart_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return heart_extends.apply(this, arguments); }

function heart_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = heart_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function heart_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var heart_Heart = function Heart(props) {
  var color = props.color,
      size = props.size,
      otherProps = heart_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", heart_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"
  }));
};

heart_Heart.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
heart_Heart.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var heart = (heart_Heart);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/help-circle.js
function help_circle_extends() { help_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return help_circle_extends.apply(this, arguments); }

function help_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = help_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function help_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var help_circle_HelpCircle = function HelpCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = help_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", help_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("path", {
    d: "M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "17",
    x2: "12",
    y2: "17"
  }));
};

help_circle_HelpCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
help_circle_HelpCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var help_circle = (help_circle_HelpCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/hexagon.js
function hexagon_extends() { hexagon_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return hexagon_extends.apply(this, arguments); }

function hexagon_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = hexagon_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function hexagon_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var hexagon_Hexagon = function Hexagon(props) {
  var color = props.color,
      size = props.size,
      otherProps = hexagon_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", hexagon_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"
  }));
};

hexagon_Hexagon.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
hexagon_Hexagon.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var hexagon = (hexagon_Hexagon);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/home.js
function home_extends() { home_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return home_extends.apply(this, arguments); }

function home_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = home_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function home_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var home_Home = function Home(props) {
  var color = props.color,
      size = props.size,
      otherProps = home_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", home_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"
  }), react_default.a.createElement("polyline", {
    points: "9 22 9 12 15 12 15 22"
  }));
};

home_Home.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
home_Home.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var home = (home_Home);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/image.js
function image_extends() { image_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return image_extends.apply(this, arguments); }

function image_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = image_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function image_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var image_Image = function Image(props) {
  var color = props.color,
      size = props.size,
      otherProps = image_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", image_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("circle", {
    cx: "8.5",
    cy: "8.5",
    r: "1.5"
  }), react_default.a.createElement("polyline", {
    points: "21 15 16 10 5 21"
  }));
};

image_Image.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
image_Image.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_image = (image_Image);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/inbox.js
function inbox_extends() { inbox_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return inbox_extends.apply(this, arguments); }

function inbox_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = inbox_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function inbox_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var inbox_Inbox = function Inbox(props) {
  var color = props.color,
      size = props.size,
      otherProps = inbox_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", inbox_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "22 12 16 12 14 15 10 15 8 12 2 12"
  }), react_default.a.createElement("path", {
    d: "M5.45 5.11L2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"
  }));
};

inbox_Inbox.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
inbox_Inbox.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var inbox = (inbox_Inbox);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/info.js
function info_extends() { info_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return info_extends.apply(this, arguments); }

function info_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = info_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function info_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var info_Info = function Info(props) {
  var color = props.color,
      size = props.size,
      otherProps = info_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", info_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "16",
    x2: "12",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "8"
  }));
};

info_Info.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
info_Info.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var info = (info_Info);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/instagram.js
function instagram_extends() { instagram_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return instagram_extends.apply(this, arguments); }

function instagram_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = instagram_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function instagram_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var instagram_Instagram = function Instagram(props) {
  var color = props.color,
      size = props.size,
      otherProps = instagram_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", instagram_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "2",
    y: "2",
    width: "20",
    height: "20",
    rx: "5",
    ry: "5"
  }), react_default.a.createElement("path", {
    d: "M16 11.37A4 4 0 1 1 12.63 8 4 4 0 0 1 16 11.37z"
  }), react_default.a.createElement("line", {
    x1: "17.5",
    y1: "6.5",
    x2: "17.5",
    y2: "6.5"
  }));
};

instagram_Instagram.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
instagram_Instagram.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var instagram = (instagram_Instagram);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/italic.js
function italic_extends() { italic_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return italic_extends.apply(this, arguments); }

function italic_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = italic_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function italic_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var italic_Italic = function Italic(props) {
  var color = props.color,
      size = props.size,
      otherProps = italic_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", italic_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "19",
    y1: "4",
    x2: "10",
    y2: "4"
  }), react_default.a.createElement("line", {
    x1: "14",
    y1: "20",
    x2: "5",
    y2: "20"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "4",
    x2: "9",
    y2: "20"
  }));
};

italic_Italic.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
italic_Italic.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var italic = (italic_Italic);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/key.js
function key_extends() { key_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return key_extends.apply(this, arguments); }

function key_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = key_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function key_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var key_Key = function Key(props) {
  var color = props.color,
      size = props.size,
      otherProps = key_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", key_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 2l-2 2m-7.61 7.61a5.5 5.5 0 1 1-7.778 7.778 5.5 5.5 0 0 1 7.777-7.777zm0 0L15.5 7.5m0 0l3 3L22 7l-3-3m-3.5 3.5L19 4"
  }));
};

key_Key.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
key_Key.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var key = (key_Key);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/layers.js
function layers_extends() { layers_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return layers_extends.apply(this, arguments); }

function layers_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = layers_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function layers_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var layers_Layers = function Layers(props) {
  var color = props.color,
      size = props.size,
      otherProps = layers_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", layers_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "12 2 2 7 12 12 22 7 12 2"
  }), react_default.a.createElement("polyline", {
    points: "2 17 12 22 22 17"
  }), react_default.a.createElement("polyline", {
    points: "2 12 12 17 22 12"
  }));
};

layers_Layers.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
layers_Layers.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_layers = (layers_Layers);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/layout.js
function layout_extends() { layout_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return layout_extends.apply(this, arguments); }

function layout_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = layout_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function layout_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var layout_Layout = function Layout(props) {
  var color = props.color,
      size = props.size,
      otherProps = layout_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", layout_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "9",
    x2: "21",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "21",
    x2: "9",
    y2: "9"
  }));
};

layout_Layout.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
layout_Layout.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var layout = (layout_Layout);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/life-buoy.js
function life_buoy_extends() { life_buoy_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return life_buoy_extends.apply(this, arguments); }

function life_buoy_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = life_buoy_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function life_buoy_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var life_buoy_LifeBuoy = function LifeBuoy(props) {
  var color = props.color,
      size = props.size,
      otherProps = life_buoy_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", life_buoy_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "4.93",
    y1: "4.93",
    x2: "9.17",
    y2: "9.17"
  }), react_default.a.createElement("line", {
    x1: "14.83",
    y1: "14.83",
    x2: "19.07",
    y2: "19.07"
  }), react_default.a.createElement("line", {
    x1: "14.83",
    y1: "9.17",
    x2: "19.07",
    y2: "4.93"
  }), react_default.a.createElement("line", {
    x1: "14.83",
    y1: "9.17",
    x2: "18.36",
    y2: "5.64"
  }), react_default.a.createElement("line", {
    x1: "4.93",
    y1: "19.07",
    x2: "9.17",
    y2: "14.83"
  }));
};

life_buoy_LifeBuoy.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
life_buoy_LifeBuoy.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var life_buoy = (life_buoy_LifeBuoy);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/link-2.js
function link_2_extends() { link_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return link_2_extends.apply(this, arguments); }

function link_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = link_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function link_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var link_2_Link2 = function Link2(props) {
  var color = props.color,
      size = props.size,
      otherProps = link_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", link_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M15 7h3a5 5 0 0 1 5 5 5 5 0 0 1-5 5h-3m-6 0H6a5 5 0 0 1-5-5 5 5 0 0 1 5-5h3"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "16",
    y2: "12"
  }));
};

link_2_Link2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
link_2_Link2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var link_2 = (link_2_Link2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/link.js
function link_extends() { link_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return link_extends.apply(this, arguments); }

function link_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = link_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function link_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var link_Link = function Link(props) {
  var color = props.color,
      size = props.size,
      otherProps = link_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", link_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M10 13a5 5 0 0 0 7.54.54l3-3a5 5 0 0 0-7.07-7.07l-1.72 1.71"
  }), react_default.a.createElement("path", {
    d: "M14 11a5 5 0 0 0-7.54-.54l-3 3a5 5 0 0 0 7.07 7.07l1.71-1.71"
  }));
};

link_Link.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
link_Link.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_link = (link_Link);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/linkedin.js
function linkedin_extends() { linkedin_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return linkedin_extends.apply(this, arguments); }

function linkedin_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = linkedin_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function linkedin_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var linkedin_Linkedin = function Linkedin(props) {
  var color = props.color,
      size = props.size,
      otherProps = linkedin_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", linkedin_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 8a6 6 0 0 1 6 6v7h-4v-7a2 2 0 0 0-2-2 2 2 0 0 0-2 2v7h-4v-7a6 6 0 0 1 6-6z"
  }), react_default.a.createElement("rect", {
    x: "2",
    y: "9",
    width: "4",
    height: "12"
  }), react_default.a.createElement("circle", {
    cx: "4",
    cy: "4",
    r: "2"
  }));
};

linkedin_Linkedin.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
linkedin_Linkedin.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var linkedin = (linkedin_Linkedin);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/list.js
function list_extends() { list_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return list_extends.apply(this, arguments); }

function list_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = list_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function list_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var list_List = function List(props) {
  var color = props.color,
      size = props.size,
      otherProps = list_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", list_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "8",
    y1: "6",
    x2: "21",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "21",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "18",
    x2: "21",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "6",
    x2: "3",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "12",
    x2: "3",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "18",
    x2: "3",
    y2: "18"
  }));
};

list_List.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
list_List.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var list = (list_List);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/loader.js
function loader_extends() { loader_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return loader_extends.apply(this, arguments); }

function loader_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = loader_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function loader_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var loader_Loader = function Loader(props) {
  var color = props.color,
      size = props.size,
      otherProps = loader_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", loader_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "12",
    y1: "2",
    x2: "12",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "18",
    x2: "12",
    y2: "22"
  }), react_default.a.createElement("line", {
    x1: "4.93",
    y1: "4.93",
    x2: "7.76",
    y2: "7.76"
  }), react_default.a.createElement("line", {
    x1: "16.24",
    y1: "16.24",
    x2: "19.07",
    y2: "19.07"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "12",
    x2: "6",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "18",
    y1: "12",
    x2: "22",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "4.93",
    y1: "19.07",
    x2: "7.76",
    y2: "16.24"
  }), react_default.a.createElement("line", {
    x1: "16.24",
    y1: "7.76",
    x2: "19.07",
    y2: "4.93"
  }));
};

loader_Loader.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
loader_Loader.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var loader = (loader_Loader);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/lock.js
function lock_extends() { lock_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return lock_extends.apply(this, arguments); }

function lock_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = lock_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function lock_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var lock_Lock = function Lock(props) {
  var color = props.color,
      size = props.size,
      otherProps = lock_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", lock_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "11",
    width: "18",
    height: "11",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("path", {
    d: "M7 11V7a5 5 0 0 1 10 0v4"
  }));
};

lock_Lock.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
lock_Lock.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var lock = (lock_Lock);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/log-in.js
function log_in_extends() { log_in_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return log_in_extends.apply(this, arguments); }

function log_in_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = log_in_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function log_in_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var log_in_LogIn = function LogIn(props) {
  var color = props.color,
      size = props.size,
      otherProps = log_in_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", log_in_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"
  }), react_default.a.createElement("polyline", {
    points: "10 17 15 12 10 7"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "12",
    x2: "3",
    y2: "12"
  }));
};

log_in_LogIn.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
log_in_LogIn.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var log_in = (log_in_LogIn);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/log-out.js
function log_out_extends() { log_out_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return log_out_extends.apply(this, arguments); }

function log_out_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = log_out_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function log_out_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var log_out_LogOut = function LogOut(props) {
  var color = props.color,
      size = props.size,
      otherProps = log_out_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", log_out_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"
  }), react_default.a.createElement("polyline", {
    points: "16 17 21 12 16 7"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "12",
    x2: "9",
    y2: "12"
  }));
};

log_out_LogOut.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
log_out_LogOut.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var log_out = (log_out_LogOut);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/mail.js
function mail_extends() { mail_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return mail_extends.apply(this, arguments); }

function mail_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = mail_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function mail_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var mail_Mail = function Mail(props) {
  var color = props.color,
      size = props.size,
      otherProps = mail_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", mail_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"
  }), react_default.a.createElement("polyline", {
    points: "22,6 12,13 2,6"
  }));
};

mail_Mail.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
mail_Mail.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var mail = (mail_Mail);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/map-pin.js
function map_pin_extends() { map_pin_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return map_pin_extends.apply(this, arguments); }

function map_pin_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = map_pin_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function map_pin_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var map_pin_MapPin = function MapPin(props) {
  var color = props.color,
      size = props.size,
      otherProps = map_pin_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", map_pin_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "10",
    r: "3"
  }));
};

map_pin_MapPin.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
map_pin_MapPin.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var map_pin = (map_pin_MapPin);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/map.js
function map_extends() { map_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return map_extends.apply(this, arguments); }

function map_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = map_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function map_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var map_Map = function Map(props) {
  var color = props.color,
      size = props.size,
      otherProps = map_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", map_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "1 6 1 22 8 18 16 22 23 18 23 2 16 6 8 2 1 6"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "2",
    x2: "8",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "6",
    x2: "16",
    y2: "22"
  }));
};

map_Map.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
map_Map.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var map = (map_Map);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/maximize-2.js
function maximize_2_extends() { maximize_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return maximize_2_extends.apply(this, arguments); }

function maximize_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = maximize_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function maximize_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var maximize_2_Maximize2 = function Maximize2(props) {
  var color = props.color,
      size = props.size,
      otherProps = maximize_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", maximize_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "15 3 21 3 21 9"
  }), react_default.a.createElement("polyline", {
    points: "9 21 3 21 3 15"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "3",
    x2: "14",
    y2: "10"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "21",
    x2: "10",
    y2: "14"
  }));
};

maximize_2_Maximize2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
maximize_2_Maximize2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var maximize_2 = (maximize_2_Maximize2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/maximize.js
function maximize_extends() { maximize_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return maximize_extends.apply(this, arguments); }

function maximize_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = maximize_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function maximize_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var maximize_Maximize = function Maximize(props) {
  var color = props.color,
      size = props.size,
      otherProps = maximize_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", maximize_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M8 3H5a2 2 0 0 0-2 2v3m18 0V5a2 2 0 0 0-2-2h-3m0 18h3a2 2 0 0 0 2-2v-3M3 16v3a2 2 0 0 0 2 2h3"
  }));
};

maximize_Maximize.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
maximize_Maximize.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var maximize = (maximize_Maximize);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/meh.js
function meh_extends() { meh_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return meh_extends.apply(this, arguments); }

function meh_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = meh_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function meh_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var meh_Meh = function Meh(props) {
  var color = props.color,
      size = props.size,
      otherProps = meh_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", meh_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "15",
    x2: "16",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "9",
    x2: "9.01",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "9",
    x2: "15.01",
    y2: "9"
  }));
};

meh_Meh.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
meh_Meh.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var meh = (meh_Meh);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/menu.js
function menu_extends() { menu_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return menu_extends.apply(this, arguments); }

function menu_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = menu_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function menu_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var menu_Menu = function Menu(props) {
  var color = props.color,
      size = props.size,
      otherProps = menu_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", menu_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "3",
    y1: "12",
    x2: "21",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "6",
    x2: "21",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "18",
    x2: "21",
    y2: "18"
  }));
};

menu_Menu.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
menu_Menu.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var menu = (menu_Menu);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/message-circle.js
function message_circle_extends() { message_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return message_circle_extends.apply(this, arguments); }

function message_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = message_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function message_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var message_circle_MessageCircle = function MessageCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = message_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", message_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"
  }));
};

message_circle_MessageCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
message_circle_MessageCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var message_circle = (message_circle_MessageCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/message-square.js
function message_square_extends() { message_square_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return message_square_extends.apply(this, arguments); }

function message_square_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = message_square_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function message_square_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var message_square_MessageSquare = function MessageSquare(props) {
  var color = props.color,
      size = props.size,
      otherProps = message_square_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", message_square_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"
  }));
};

message_square_MessageSquare.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
message_square_MessageSquare.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var message_square = (message_square_MessageSquare);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/mic-off.js
function mic_off_extends() { mic_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return mic_off_extends.apply(this, arguments); }

function mic_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = mic_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function mic_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var mic_off_MicOff = function MicOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = mic_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", mic_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }), react_default.a.createElement("path", {
    d: "M9 9v3a3 3 0 0 0 5.12 2.12M15 9.34V4a3 3 0 0 0-5.94-.6"
  }), react_default.a.createElement("path", {
    d: "M17 16.95A7 7 0 0 1 5 12v-2m14 0v2a7 7 0 0 1-.11 1.23"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "19",
    x2: "12",
    y2: "23"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "23",
    x2: "16",
    y2: "23"
  }));
};

mic_off_MicOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
mic_off_MicOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var mic_off = (mic_off_MicOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/mic.js
function mic_extends() { mic_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return mic_extends.apply(this, arguments); }

function mic_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = mic_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function mic_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var mic_Mic = function Mic(props) {
  var color = props.color,
      size = props.size,
      otherProps = mic_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", mic_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M12 1a3 3 0 0 0-3 3v8a3 3 0 0 0 6 0V4a3 3 0 0 0-3-3z"
  }), react_default.a.createElement("path", {
    d: "M19 10v2a7 7 0 0 1-14 0v-2"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "19",
    x2: "12",
    y2: "23"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "23",
    x2: "16",
    y2: "23"
  }));
};

mic_Mic.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
mic_Mic.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var mic = (mic_Mic);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/minimize-2.js
function minimize_2_extends() { minimize_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return minimize_2_extends.apply(this, arguments); }

function minimize_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = minimize_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function minimize_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var minimize_2_Minimize2 = function Minimize2(props) {
  var color = props.color,
      size = props.size,
      otherProps = minimize_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", minimize_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "4 14 10 14 10 20"
  }), react_default.a.createElement("polyline", {
    points: "20 10 14 10 14 4"
  }), react_default.a.createElement("line", {
    x1: "14",
    y1: "10",
    x2: "21",
    y2: "3"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "21",
    x2: "10",
    y2: "14"
  }));
};

minimize_2_Minimize2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
minimize_2_Minimize2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var minimize_2 = (minimize_2_Minimize2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/minimize.js
function minimize_extends() { minimize_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return minimize_extends.apply(this, arguments); }

function minimize_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = minimize_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function minimize_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var minimize_Minimize = function Minimize(props) {
  var color = props.color,
      size = props.size,
      otherProps = minimize_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", minimize_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M8 3v3a2 2 0 0 1-2 2H3m18 0h-3a2 2 0 0 1-2-2V3m0 18v-3a2 2 0 0 1 2-2h3M3 16h3a2 2 0 0 1 2 2v3"
  }));
};

minimize_Minimize.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
minimize_Minimize.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var minimize = (minimize_Minimize);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/minus-circle.js
function minus_circle_extends() { minus_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return minus_circle_extends.apply(this, arguments); }

function minus_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = minus_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function minus_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var minus_circle_MinusCircle = function MinusCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = minus_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", minus_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "16",
    y2: "12"
  }));
};

minus_circle_MinusCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
minus_circle_MinusCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var minus_circle = (minus_circle_MinusCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/minus-square.js
function minus_square_extends() { minus_square_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return minus_square_extends.apply(this, arguments); }

function minus_square_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = minus_square_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function minus_square_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var minus_square_MinusSquare = function MinusSquare(props) {
  var color = props.color,
      size = props.size,
      otherProps = minus_square_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", minus_square_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "16",
    y2: "12"
  }));
};

minus_square_MinusSquare.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
minus_square_MinusSquare.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var minus_square = (minus_square_MinusSquare);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/minus.js
function minus_extends() { minus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return minus_extends.apply(this, arguments); }

function minus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = minus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function minus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var minus_Minus = function Minus(props) {
  var color = props.color,
      size = props.size,
      otherProps = minus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", minus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "5",
    y1: "12",
    x2: "19",
    y2: "12"
  }));
};

minus_Minus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
minus_Minus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var minus = (minus_Minus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/monitor.js
function monitor_extends() { monitor_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return monitor_extends.apply(this, arguments); }

function monitor_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = monitor_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function monitor_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var monitor_Monitor = function Monitor(props) {
  var color = props.color,
      size = props.size,
      otherProps = monitor_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", monitor_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "2",
    y: "3",
    width: "20",
    height: "14",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "21",
    x2: "16",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "17",
    x2: "12",
    y2: "21"
  }));
};

monitor_Monitor.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
monitor_Monitor.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var monitor = (monitor_Monitor);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/moon.js
function moon_extends() { moon_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return moon_extends.apply(this, arguments); }

function moon_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = moon_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function moon_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var moon_Moon = function Moon(props) {
  var color = props.color,
      size = props.size,
      otherProps = moon_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", moon_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"
  }));
};

moon_Moon.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
moon_Moon.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var moon = (moon_Moon);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/more-horizontal.js
function more_horizontal_extends() { more_horizontal_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return more_horizontal_extends.apply(this, arguments); }

function more_horizontal_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = more_horizontal_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function more_horizontal_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var more_horizontal_MoreHorizontal = function MoreHorizontal(props) {
  var color = props.color,
      size = props.size,
      otherProps = more_horizontal_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", more_horizontal_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "1"
  }), react_default.a.createElement("circle", {
    cx: "19",
    cy: "12",
    r: "1"
  }), react_default.a.createElement("circle", {
    cx: "5",
    cy: "12",
    r: "1"
  }));
};

more_horizontal_MoreHorizontal.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
more_horizontal_MoreHorizontal.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var more_horizontal = (more_horizontal_MoreHorizontal);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/more-vertical.js
function more_vertical_extends() { more_vertical_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return more_vertical_extends.apply(this, arguments); }

function more_vertical_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = more_vertical_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function more_vertical_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var more_vertical_MoreVertical = function MoreVertical(props) {
  var color = props.color,
      size = props.size,
      otherProps = more_vertical_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", more_vertical_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "1"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "5",
    r: "1"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "19",
    r: "1"
  }));
};

more_vertical_MoreVertical.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
more_vertical_MoreVertical.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var more_vertical = (more_vertical_MoreVertical);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/mouse-pointer.js
function mouse_pointer_extends() { mouse_pointer_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return mouse_pointer_extends.apply(this, arguments); }

function mouse_pointer_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = mouse_pointer_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function mouse_pointer_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var mouse_pointer_MousePointer = function MousePointer(props) {
  var color = props.color,
      size = props.size,
      otherProps = mouse_pointer_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", mouse_pointer_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M3 3l7.07 16.97 2.51-7.39 7.39-2.51L3 3z"
  }), react_default.a.createElement("path", {
    d: "M13 13l6 6"
  }));
};

mouse_pointer_MousePointer.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
mouse_pointer_MousePointer.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var mouse_pointer = (mouse_pointer_MousePointer);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/move.js
function move_extends() { move_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return move_extends.apply(this, arguments); }

function move_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = move_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function move_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var move_Move = function Move(props) {
  var color = props.color,
      size = props.size,
      otherProps = move_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", move_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "5 9 2 12 5 15"
  }), react_default.a.createElement("polyline", {
    points: "9 5 12 2 15 5"
  }), react_default.a.createElement("polyline", {
    points: "15 19 12 22 9 19"
  }), react_default.a.createElement("polyline", {
    points: "19 9 22 12 19 15"
  }), react_default.a.createElement("line", {
    x1: "2",
    y1: "12",
    x2: "22",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "2",
    x2: "12",
    y2: "22"
  }));
};

move_Move.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
move_Move.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var move = (move_Move);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/music.js
function music_extends() { music_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return music_extends.apply(this, arguments); }

function music_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = music_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function music_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var music_Music = function Music(props) {
  var color = props.color,
      size = props.size,
      otherProps = music_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", music_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M9 18V5l12-2v13"
  }), react_default.a.createElement("circle", {
    cx: "6",
    cy: "18",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "18",
    cy: "16",
    r: "3"
  }));
};

music_Music.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
music_Music.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var music = (music_Music);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/navigation-2.js
function navigation_2_extends() { navigation_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return navigation_2_extends.apply(this, arguments); }

function navigation_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = navigation_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function navigation_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var navigation_2_Navigation2 = function Navigation2(props) {
  var color = props.color,
      size = props.size,
      otherProps = navigation_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", navigation_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "12 2 19 21 12 17 5 21 12 2"
  }));
};

navigation_2_Navigation2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
navigation_2_Navigation2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var navigation_2 = (navigation_2_Navigation2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/navigation.js
function navigation_extends() { navigation_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return navigation_extends.apply(this, arguments); }

function navigation_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = navigation_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function navigation_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var navigation_Navigation = function Navigation(props) {
  var color = props.color,
      size = props.size,
      otherProps = navigation_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", navigation_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "3 11 22 2 13 21 11 13 3 11"
  }));
};

navigation_Navigation.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
navigation_Navigation.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var navigation = (navigation_Navigation);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/octagon.js
function octagon_extends() { octagon_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return octagon_extends.apply(this, arguments); }

function octagon_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = octagon_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function octagon_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var octagon_Octagon = function Octagon(props) {
  var color = props.color,
      size = props.size,
      otherProps = octagon_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", octagon_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"
  }));
};

octagon_Octagon.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
octagon_Octagon.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var octagon = (octagon_Octagon);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/package.js
function package_extends() { package_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return package_extends.apply(this, arguments); }

function package_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = package_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function package_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var package_Package = function Package(props) {
  var color = props.color,
      size = props.size,
      otherProps = package_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", package_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "16.5",
    y1: "9.4",
    x2: "7.5",
    y2: "4.21"
  }), react_default.a.createElement("path", {
    d: "M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"
  }), react_default.a.createElement("polyline", {
    points: "3.27 6.96 12 12.01 20.73 6.96"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "22.08",
    x2: "12",
    y2: "12"
  }));
};

package_Package.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
package_Package.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_package = (package_Package);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/paperclip.js
function paperclip_extends() { paperclip_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return paperclip_extends.apply(this, arguments); }

function paperclip_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = paperclip_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function paperclip_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var paperclip_Paperclip = function Paperclip(props) {
  var color = props.color,
      size = props.size,
      otherProps = paperclip_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", paperclip_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21.44 11.05l-9.19 9.19a6 6 0 0 1-8.49-8.49l9.19-9.19a4 4 0 0 1 5.66 5.66l-9.2 9.19a2 2 0 0 1-2.83-2.83l8.49-8.48"
  }));
};

paperclip_Paperclip.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
paperclip_Paperclip.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var paperclip = (paperclip_Paperclip);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/pause-circle.js
function pause_circle_extends() { pause_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return pause_circle_extends.apply(this, arguments); }

function pause_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = pause_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function pause_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var pause_circle_PauseCircle = function PauseCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = pause_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", pause_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "15",
    x2: "10",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "14",
    y1: "15",
    x2: "14",
    y2: "9"
  }));
};

pause_circle_PauseCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
pause_circle_PauseCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var pause_circle = (pause_circle_PauseCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/pause.js
function pause_extends() { pause_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return pause_extends.apply(this, arguments); }

function pause_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = pause_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function pause_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var pause_Pause = function Pause(props) {
  var color = props.color,
      size = props.size,
      otherProps = pause_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", pause_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "6",
    y: "4",
    width: "4",
    height: "16"
  }), react_default.a.createElement("rect", {
    x: "14",
    y: "4",
    width: "4",
    height: "16"
  }));
};

pause_Pause.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
pause_Pause.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var pause = (pause_Pause);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/pen-tool.js
function pen_tool_extends() { pen_tool_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return pen_tool_extends.apply(this, arguments); }

function pen_tool_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = pen_tool_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function pen_tool_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var pen_tool_PenTool = function PenTool(props) {
  var color = props.color,
      size = props.size,
      otherProps = pen_tool_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", pen_tool_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M12 19l7-7 3 3-7 7-3-3z"
  }), react_default.a.createElement("path", {
    d: "M18 13l-1.5-7.5L2 2l3.5 14.5L13 18l5-5z"
  }), react_default.a.createElement("path", {
    d: "M2 2l7.586 7.586"
  }), react_default.a.createElement("circle", {
    cx: "11",
    cy: "11",
    r: "2"
  }));
};

pen_tool_PenTool.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
pen_tool_PenTool.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var pen_tool = (pen_tool_PenTool);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/percent.js
function percent_extends() { percent_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return percent_extends.apply(this, arguments); }

function percent_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = percent_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function percent_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var percent_Percent = function Percent(props) {
  var color = props.color,
      size = props.size,
      otherProps = percent_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", percent_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "19",
    y1: "5",
    x2: "5",
    y2: "19"
  }), react_default.a.createElement("circle", {
    cx: "6.5",
    cy: "6.5",
    r: "2.5"
  }), react_default.a.createElement("circle", {
    cx: "17.5",
    cy: "17.5",
    r: "2.5"
  }));
};

percent_Percent.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
percent_Percent.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var percent = (percent_Percent);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone-call.js
function phone_call_extends() { phone_call_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_call_extends.apply(this, arguments); }

function phone_call_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_call_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_call_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_call_PhoneCall = function PhoneCall(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_call_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_call_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M15.05 5A5 5 0 0 1 19 8.95M15.05 1A9 9 0 0 1 23 8.94m-1 7.98v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"
  }));
};

phone_call_PhoneCall.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_call_PhoneCall.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone_call = (phone_call_PhoneCall);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone-forwarded.js
function phone_forwarded_extends() { phone_forwarded_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_forwarded_extends.apply(this, arguments); }

function phone_forwarded_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_forwarded_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_forwarded_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_forwarded_PhoneForwarded = function PhoneForwarded(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_forwarded_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_forwarded_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "19 1 23 5 19 9"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "5",
    x2: "23",
    y2: "5"
  }), react_default.a.createElement("path", {
    d: "M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"
  }));
};

phone_forwarded_PhoneForwarded.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_forwarded_PhoneForwarded.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone_forwarded = (phone_forwarded_PhoneForwarded);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone-incoming.js
function phone_incoming_extends() { phone_incoming_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_incoming_extends.apply(this, arguments); }

function phone_incoming_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_incoming_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_incoming_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_incoming_PhoneIncoming = function PhoneIncoming(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_incoming_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_incoming_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "16 2 16 8 22 8"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "1",
    x2: "16",
    y2: "8"
  }), react_default.a.createElement("path", {
    d: "M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"
  }));
};

phone_incoming_PhoneIncoming.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_incoming_PhoneIncoming.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone_incoming = (phone_incoming_PhoneIncoming);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone-missed.js
function phone_missed_extends() { phone_missed_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_missed_extends.apply(this, arguments); }

function phone_missed_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_missed_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_missed_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_missed_PhoneMissed = function PhoneMissed(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_missed_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_missed_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "23",
    y1: "1",
    x2: "17",
    y2: "7"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "1",
    x2: "23",
    y2: "7"
  }), react_default.a.createElement("path", {
    d: "M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"
  }));
};

phone_missed_PhoneMissed.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_missed_PhoneMissed.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone_missed = (phone_missed_PhoneMissed);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone-off.js
function phone_off_extends() { phone_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_off_extends.apply(this, arguments); }

function phone_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_off_PhoneOff = function PhoneOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M10.68 13.31a16 16 0 0 0 3.41 2.6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7 2 2 0 0 1 1.72 2v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.42 19.42 0 0 1-3.33-2.67m-2.67-3.34a19.79 19.79 0 0 1-3.07-8.63A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "1",
    x2: "1",
    y2: "23"
  }));
};

phone_off_PhoneOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_off_PhoneOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone_off = (phone_off_PhoneOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone-outgoing.js
function phone_outgoing_extends() { phone_outgoing_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_outgoing_extends.apply(this, arguments); }

function phone_outgoing_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_outgoing_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_outgoing_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_outgoing_PhoneOutgoing = function PhoneOutgoing(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_outgoing_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_outgoing_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "23 7 23 1 17 1"
  }), react_default.a.createElement("line", {
    x1: "16",
    y1: "8",
    x2: "23",
    y2: "1"
  }), react_default.a.createElement("path", {
    d: "M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"
  }));
};

phone_outgoing_PhoneOutgoing.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_outgoing_PhoneOutgoing.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone_outgoing = (phone_outgoing_PhoneOutgoing);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/phone.js
function phone_extends() { phone_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return phone_extends.apply(this, arguments); }

function phone_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = phone_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function phone_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var phone_Phone = function Phone(props) {
  var color = props.color,
      size = props.size,
      otherProps = phone_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", phone_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"
  }));
};

phone_Phone.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
phone_Phone.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var phone = (phone_Phone);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/pie-chart.js
function pie_chart_extends() { pie_chart_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return pie_chart_extends.apply(this, arguments); }

function pie_chart_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = pie_chart_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function pie_chart_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var pie_chart_PieChart = function PieChart(props) {
  var color = props.color,
      size = props.size,
      otherProps = pie_chart_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", pie_chart_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21.21 15.89A10 10 0 1 1 8 2.83"
  }), react_default.a.createElement("path", {
    d: "M22 12A10 10 0 0 0 12 2v10z"
  }));
};

pie_chart_PieChart.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
pie_chart_PieChart.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var pie_chart = (pie_chart_PieChart);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/play-circle.js
function play_circle_extends() { play_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return play_circle_extends.apply(this, arguments); }

function play_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = play_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function play_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var play_circle_PlayCircle = function PlayCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = play_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", play_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("polygon", {
    points: "10 8 16 12 10 16 10 8"
  }));
};

play_circle_PlayCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
play_circle_PlayCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var play_circle = (play_circle_PlayCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/play.js
function play_extends() { play_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return play_extends.apply(this, arguments); }

function play_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = play_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function play_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var play_Play = function Play(props) {
  var color = props.color,
      size = props.size,
      otherProps = play_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", play_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "5 3 19 12 5 21 5 3"
  }));
};

play_Play.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
play_Play.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var play = (play_Play);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/plus-circle.js
function plus_circle_extends() { plus_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return plus_circle_extends.apply(this, arguments); }

function plus_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = plus_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function plus_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var plus_circle_PlusCircle = function PlusCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = plus_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", plus_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "16",
    y2: "12"
  }));
};

plus_circle_PlusCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
plus_circle_PlusCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var plus_circle = (plus_circle_PlusCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/plus-square.js
function plus_square_extends() { plus_square_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return plus_square_extends.apply(this, arguments); }

function plus_square_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = plus_square_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function plus_square_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var plus_square_PlusSquare = function PlusSquare(props) {
  var color = props.color,
      size = props.size,
      otherProps = plus_square_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", plus_square_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "12",
    x2: "16",
    y2: "12"
  }));
};

plus_square_PlusSquare.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
plus_square_PlusSquare.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var plus_square = (plus_square_PlusSquare);
// EXTERNAL MODULE: ./node_modules/react-feather/dist/icons/plus.js
var plus = __webpack_require__(27);

// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/pocket.js
function pocket_extends() { pocket_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return pocket_extends.apply(this, arguments); }

function pocket_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = pocket_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function pocket_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var pocket_Pocket = function Pocket(props) {
  var color = props.color,
      size = props.size,
      otherProps = pocket_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", pocket_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M4 3h16a2 2 0 0 1 2 2v6a10 10 0 0 1-10 10A10 10 0 0 1 2 11V5a2 2 0 0 1 2-2z"
  }), react_default.a.createElement("polyline", {
    points: "8 10 12 14 16 10"
  }));
};

pocket_Pocket.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
pocket_Pocket.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var pocket = (pocket_Pocket);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/power.js
function power_extends() { power_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return power_extends.apply(this, arguments); }

function power_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = power_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function power_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var power_Power = function Power(props) {
  var color = props.color,
      size = props.size,
      otherProps = power_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", power_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M18.36 6.64a9 9 0 1 1-12.73 0"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "2",
    x2: "12",
    y2: "12"
  }));
};

power_Power.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
power_Power.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var power = (power_Power);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/printer.js
function printer_extends() { printer_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return printer_extends.apply(this, arguments); }

function printer_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = printer_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function printer_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var printer_Printer = function Printer(props) {
  var color = props.color,
      size = props.size,
      otherProps = printer_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", printer_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "6 9 6 2 18 2 18 9"
  }), react_default.a.createElement("path", {
    d: "M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"
  }), react_default.a.createElement("rect", {
    x: "6",
    y: "14",
    width: "12",
    height: "8"
  }));
};

printer_Printer.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
printer_Printer.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var printer = (printer_Printer);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/radio.js
function radio_extends() { radio_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return radio_extends.apply(this, arguments); }

function radio_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = radio_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function radio_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var radio_Radio = function Radio(props) {
  var color = props.color,
      size = props.size,
      otherProps = radio_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", radio_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "2"
  }), react_default.a.createElement("path", {
    d: "M16.24 7.76a6 6 0 0 1 0 8.49m-8.48-.01a6 6 0 0 1 0-8.49m11.31-2.82a10 10 0 0 1 0 14.14m-14.14 0a10 10 0 0 1 0-14.14"
  }));
};

radio_Radio.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
radio_Radio.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var icons_radio = (radio_Radio);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/refresh-ccw.js
function refresh_ccw_extends() { refresh_ccw_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return refresh_ccw_extends.apply(this, arguments); }

function refresh_ccw_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = refresh_ccw_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function refresh_ccw_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var refresh_ccw_RefreshCcw = function RefreshCcw(props) {
  var color = props.color,
      size = props.size,
      otherProps = refresh_ccw_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", refresh_ccw_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "1 4 1 10 7 10"
  }), react_default.a.createElement("polyline", {
    points: "23 20 23 14 17 14"
  }), react_default.a.createElement("path", {
    d: "M20.49 9A9 9 0 0 0 5.64 5.64L1 10m22 4l-4.64 4.36A9 9 0 0 1 3.51 15"
  }));
};

refresh_ccw_RefreshCcw.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
refresh_ccw_RefreshCcw.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var refresh_ccw = (refresh_ccw_RefreshCcw);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/refresh-cw.js
function refresh_cw_extends() { refresh_cw_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return refresh_cw_extends.apply(this, arguments); }

function refresh_cw_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = refresh_cw_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function refresh_cw_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var refresh_cw_RefreshCw = function RefreshCw(props) {
  var color = props.color,
      size = props.size,
      otherProps = refresh_cw_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", refresh_cw_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "23 4 23 10 17 10"
  }), react_default.a.createElement("polyline", {
    points: "1 20 1 14 7 14"
  }), react_default.a.createElement("path", {
    d: "M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"
  }));
};

refresh_cw_RefreshCw.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
refresh_cw_RefreshCw.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var refresh_cw = (refresh_cw_RefreshCw);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/repeat.js
function repeat_extends() { repeat_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return repeat_extends.apply(this, arguments); }

function repeat_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = repeat_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function repeat_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var repeat_Repeat = function Repeat(props) {
  var color = props.color,
      size = props.size,
      otherProps = repeat_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", repeat_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "17 1 21 5 17 9"
  }), react_default.a.createElement("path", {
    d: "M3 11V9a4 4 0 0 1 4-4h14"
  }), react_default.a.createElement("polyline", {
    points: "7 23 3 19 7 15"
  }), react_default.a.createElement("path", {
    d: "M21 13v2a4 4 0 0 1-4 4H3"
  }));
};

repeat_Repeat.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
repeat_Repeat.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var repeat = (repeat_Repeat);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/rewind.js
function rewind_extends() { rewind_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return rewind_extends.apply(this, arguments); }

function rewind_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = rewind_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function rewind_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var rewind_Rewind = function Rewind(props) {
  var color = props.color,
      size = props.size,
      otherProps = rewind_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", rewind_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "11 19 2 12 11 5 11 19"
  }), react_default.a.createElement("polygon", {
    points: "22 19 13 12 22 5 22 19"
  }));
};

rewind_Rewind.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
rewind_Rewind.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var rewind = (rewind_Rewind);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/rotate-ccw.js
function rotate_ccw_extends() { rotate_ccw_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return rotate_ccw_extends.apply(this, arguments); }

function rotate_ccw_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = rotate_ccw_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function rotate_ccw_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var rotate_ccw_RotateCcw = function RotateCcw(props) {
  var color = props.color,
      size = props.size,
      otherProps = rotate_ccw_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", rotate_ccw_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "1 4 1 10 7 10"
  }), react_default.a.createElement("path", {
    d: "M3.51 15a9 9 0 1 0 2.13-9.36L1 10"
  }));
};

rotate_ccw_RotateCcw.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
rotate_ccw_RotateCcw.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var rotate_ccw = (rotate_ccw_RotateCcw);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/rotate-cw.js
function rotate_cw_extends() { rotate_cw_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return rotate_cw_extends.apply(this, arguments); }

function rotate_cw_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = rotate_cw_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function rotate_cw_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var rotate_cw_RotateCw = function RotateCw(props) {
  var color = props.color,
      size = props.size,
      otherProps = rotate_cw_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", rotate_cw_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "23 4 23 10 17 10"
  }), react_default.a.createElement("path", {
    d: "M20.49 15a9 9 0 1 1-2.12-9.36L23 10"
  }));
};

rotate_cw_RotateCw.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
rotate_cw_RotateCw.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var rotate_cw = (rotate_cw_RotateCw);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/rss.js
function rss_extends() { rss_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return rss_extends.apply(this, arguments); }

function rss_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = rss_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function rss_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var rss_Rss = function Rss(props) {
  var color = props.color,
      size = props.size,
      otherProps = rss_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", rss_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M4 11a9 9 0 0 1 9 9"
  }), react_default.a.createElement("path", {
    d: "M4 4a16 16 0 0 1 16 16"
  }), react_default.a.createElement("circle", {
    cx: "5",
    cy: "19",
    r: "1"
  }));
};

rss_Rss.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
rss_Rss.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var rss = (rss_Rss);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/save.js
function save_extends() { save_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return save_extends.apply(this, arguments); }

function save_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = save_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function save_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var save_Save = function Save(props) {
  var color = props.color,
      size = props.size,
      otherProps = save_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", save_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z"
  }), react_default.a.createElement("polyline", {
    points: "17 21 17 13 7 13 7 21"
  }), react_default.a.createElement("polyline", {
    points: "7 3 7 8 15 8"
  }));
};

save_Save.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
save_Save.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var save = (save_Save);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/scissors.js
function scissors_extends() { scissors_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return scissors_extends.apply(this, arguments); }

function scissors_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = scissors_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function scissors_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var scissors_Scissors = function Scissors(props) {
  var color = props.color,
      size = props.size,
      otherProps = scissors_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", scissors_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "6",
    cy: "6",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "6",
    cy: "18",
    r: "3"
  }), react_default.a.createElement("line", {
    x1: "20",
    y1: "4",
    x2: "8.12",
    y2: "15.88"
  }), react_default.a.createElement("line", {
    x1: "14.47",
    y1: "14.48",
    x2: "20",
    y2: "20"
  }), react_default.a.createElement("line", {
    x1: "8.12",
    y1: "8.12",
    x2: "12",
    y2: "12"
  }));
};

scissors_Scissors.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
scissors_Scissors.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var scissors = (scissors_Scissors);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/search.js
function search_extends() { search_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return search_extends.apply(this, arguments); }

function search_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = search_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function search_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var search_Search = function Search(props) {
  var color = props.color,
      size = props.size,
      otherProps = search_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", search_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "11",
    cy: "11",
    r: "8"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "21",
    x2: "16.65",
    y2: "16.65"
  }));
};

search_Search.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
search_Search.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var search = (search_Search);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/send.js
function send_extends() { send_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return send_extends.apply(this, arguments); }

function send_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = send_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function send_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var send_Send = function Send(props) {
  var color = props.color,
      size = props.size,
      otherProps = send_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", send_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "22",
    y1: "2",
    x2: "11",
    y2: "13"
  }), react_default.a.createElement("polygon", {
    points: "22 2 15 22 11 13 2 9 22 2"
  }));
};

send_Send.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
send_Send.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var send = (send_Send);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/server.js
function server_extends() { server_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return server_extends.apply(this, arguments); }

function server_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = server_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function server_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var server_Server = function Server(props) {
  var color = props.color,
      size = props.size,
      otherProps = server_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", server_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "2",
    y: "2",
    width: "20",
    height: "8",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("rect", {
    x: "2",
    y: "14",
    width: "20",
    height: "8",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "6",
    x2: "6",
    y2: "6"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "18",
    x2: "6",
    y2: "18"
  }));
};

server_Server.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
server_Server.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var server = (server_Server);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/settings.js
function settings_extends() { settings_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return settings_extends.apply(this, arguments); }

function settings_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = settings_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function settings_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var settings_Settings = function Settings(props) {
  var color = props.color,
      size = props.size,
      otherProps = settings_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", settings_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "3"
  }), react_default.a.createElement("path", {
    d: "M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"
  }));
};

settings_Settings.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
settings_Settings.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var settings = (settings_Settings);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/share-2.js
function share_2_extends() { share_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return share_2_extends.apply(this, arguments); }

function share_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = share_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function share_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var share_2_Share2 = function Share2(props) {
  var color = props.color,
      size = props.size,
      otherProps = share_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", share_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "18",
    cy: "5",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "6",
    cy: "12",
    r: "3"
  }), react_default.a.createElement("circle", {
    cx: "18",
    cy: "19",
    r: "3"
  }), react_default.a.createElement("line", {
    x1: "8.59",
    y1: "13.51",
    x2: "15.42",
    y2: "17.49"
  }), react_default.a.createElement("line", {
    x1: "15.41",
    y1: "6.51",
    x2: "8.59",
    y2: "10.49"
  }));
};

share_2_Share2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
share_2_Share2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var share_2 = (share_2_Share2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/share.js
function share_extends() { share_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return share_extends.apply(this, arguments); }

function share_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = share_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function share_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var share_Share = function Share(props) {
  var color = props.color,
      size = props.size,
      otherProps = share_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", share_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8"
  }), react_default.a.createElement("polyline", {
    points: "16 6 12 2 8 6"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "2",
    x2: "12",
    y2: "15"
  }));
};

share_Share.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
share_Share.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var share = (share_Share);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/shield-off.js
function shield_off_extends() { shield_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return shield_off_extends.apply(this, arguments); }

function shield_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = shield_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function shield_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var shield_off_ShieldOff = function ShieldOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = shield_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", shield_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M19.69 14a6.9 6.9 0 0 0 .31-2V5l-8-3-3.16 1.18"
  }), react_default.a.createElement("path", {
    d: "M4.73 4.73L4 5v7c0 6 8 10 8 10a20.29 20.29 0 0 0 5.62-4.38"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }));
};

shield_off_ShieldOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
shield_off_ShieldOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var shield_off = (shield_off_ShieldOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/shield.js
function shield_extends() { shield_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return shield_extends.apply(this, arguments); }

function shield_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = shield_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function shield_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var shield_Shield = function Shield(props) {
  var color = props.color,
      size = props.size,
      otherProps = shield_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", shield_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"
  }));
};

shield_Shield.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
shield_Shield.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var shield = (shield_Shield);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/shopping-bag.js
function shopping_bag_extends() { shopping_bag_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return shopping_bag_extends.apply(this, arguments); }

function shopping_bag_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = shopping_bag_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function shopping_bag_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var shopping_bag_ShoppingBag = function ShoppingBag(props) {
  var color = props.color,
      size = props.size,
      otherProps = shopping_bag_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", shopping_bag_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"
  }), react_default.a.createElement("line", {
    x1: "3",
    y1: "6",
    x2: "21",
    y2: "6"
  }), react_default.a.createElement("path", {
    d: "M16 10a4 4 0 0 1-8 0"
  }));
};

shopping_bag_ShoppingBag.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
shopping_bag_ShoppingBag.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var shopping_bag = (shopping_bag_ShoppingBag);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/shopping-cart.js
function shopping_cart_extends() { shopping_cart_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return shopping_cart_extends.apply(this, arguments); }

function shopping_cart_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = shopping_cart_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function shopping_cart_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var shopping_cart_ShoppingCart = function ShoppingCart(props) {
  var color = props.color,
      size = props.size,
      otherProps = shopping_cart_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", shopping_cart_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "9",
    cy: "21",
    r: "1"
  }), react_default.a.createElement("circle", {
    cx: "20",
    cy: "21",
    r: "1"
  }), react_default.a.createElement("path", {
    d: "M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"
  }));
};

shopping_cart_ShoppingCart.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
shopping_cart_ShoppingCart.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var shopping_cart = (shopping_cart_ShoppingCart);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/shuffle.js
function shuffle_extends() { shuffle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return shuffle_extends.apply(this, arguments); }

function shuffle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = shuffle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function shuffle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var shuffle_Shuffle = function Shuffle(props) {
  var color = props.color,
      size = props.size,
      otherProps = shuffle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", shuffle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "16 3 21 3 21 8"
  }), react_default.a.createElement("line", {
    x1: "4",
    y1: "20",
    x2: "21",
    y2: "3"
  }), react_default.a.createElement("polyline", {
    points: "21 16 21 21 16 21"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "15",
    x2: "21",
    y2: "21"
  }), react_default.a.createElement("line", {
    x1: "4",
    y1: "4",
    x2: "9",
    y2: "9"
  }));
};

shuffle_Shuffle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
shuffle_Shuffle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var shuffle = (shuffle_Shuffle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/sidebar.js
function sidebar_extends() { sidebar_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return sidebar_extends.apply(this, arguments); }

function sidebar_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = sidebar_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function sidebar_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var sidebar_Sidebar = function Sidebar(props) {
  var color = props.color,
      size = props.size,
      otherProps = sidebar_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", sidebar_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "3",
    x2: "9",
    y2: "21"
  }));
};

sidebar_Sidebar.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
sidebar_Sidebar.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var sidebar = (sidebar_Sidebar);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/skip-back.js
function skip_back_extends() { skip_back_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return skip_back_extends.apply(this, arguments); }

function skip_back_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = skip_back_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function skip_back_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var skip_back_SkipBack = function SkipBack(props) {
  var color = props.color,
      size = props.size,
      otherProps = skip_back_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", skip_back_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "19 20 9 12 19 4 19 20"
  }), react_default.a.createElement("line", {
    x1: "5",
    y1: "19",
    x2: "5",
    y2: "5"
  }));
};

skip_back_SkipBack.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
skip_back_SkipBack.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var skip_back = (skip_back_SkipBack);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/skip-forward.js
function skip_forward_extends() { skip_forward_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return skip_forward_extends.apply(this, arguments); }

function skip_forward_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = skip_forward_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function skip_forward_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var skip_forward_SkipForward = function SkipForward(props) {
  var color = props.color,
      size = props.size,
      otherProps = skip_forward_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", skip_forward_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "5 4 15 12 5 20 5 4"
  }), react_default.a.createElement("line", {
    x1: "19",
    y1: "5",
    x2: "19",
    y2: "19"
  }));
};

skip_forward_SkipForward.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
skip_forward_SkipForward.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var skip_forward = (skip_forward_SkipForward);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/slack.js
function slack_extends() { slack_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return slack_extends.apply(this, arguments); }

function slack_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = slack_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function slack_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var slack_Slack = function Slack(props) {
  var color = props.color,
      size = props.size,
      otherProps = slack_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", slack_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M14.5 10c-.83 0-1.5-.67-1.5-1.5v-5c0-.83.67-1.5 1.5-1.5s1.5.67 1.5 1.5v5c0 .83-.67 1.5-1.5 1.5z"
  }), react_default.a.createElement("path", {
    d: "M20.5 10H19V8.5c0-.83.67-1.5 1.5-1.5s1.5.67 1.5 1.5-.67 1.5-1.5 1.5z"
  }), react_default.a.createElement("path", {
    d: "M9.5 14c.83 0 1.5.67 1.5 1.5v5c0 .83-.67 1.5-1.5 1.5S8 21.33 8 20.5v-5c0-.83.67-1.5 1.5-1.5z"
  }), react_default.a.createElement("path", {
    d: "M3.5 14H5v1.5c0 .83-.67 1.5-1.5 1.5S2 16.33 2 15.5 2.67 14 3.5 14z"
  }), react_default.a.createElement("path", {
    d: "M14 14.5c0-.83.67-1.5 1.5-1.5h5c.83 0 1.5.67 1.5 1.5s-.67 1.5-1.5 1.5h-5c-.83 0-1.5-.67-1.5-1.5z"
  }), react_default.a.createElement("path", {
    d: "M15.5 19H14v1.5c0 .83.67 1.5 1.5 1.5s1.5-.67 1.5-1.5-.67-1.5-1.5-1.5z"
  }), react_default.a.createElement("path", {
    d: "M10 9.5C10 8.67 9.33 8 8.5 8h-5C2.67 8 2 8.67 2 9.5S2.67 11 3.5 11h5c.83 0 1.5-.67 1.5-1.5z"
  }), react_default.a.createElement("path", {
    d: "M8.5 5H10V3.5C10 2.67 9.33 2 8.5 2S7 2.67 7 3.5 7.67 5 8.5 5z"
  }));
};

slack_Slack.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
slack_Slack.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var slack = (slack_Slack);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/slash.js
function slash_extends() { slash_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return slash_extends.apply(this, arguments); }

function slash_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = slash_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function slash_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var slash_Slash = function Slash(props) {
  var color = props.color,
      size = props.size,
      otherProps = slash_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", slash_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "4.93",
    y1: "4.93",
    x2: "19.07",
    y2: "19.07"
  }));
};

slash_Slash.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
slash_Slash.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var slash = (slash_Slash);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/sliders.js
function sliders_extends() { sliders_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return sliders_extends.apply(this, arguments); }

function sliders_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = sliders_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function sliders_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var sliders_Sliders = function Sliders(props) {
  var color = props.color,
      size = props.size,
      otherProps = sliders_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", sliders_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "4",
    y1: "21",
    x2: "4",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "4",
    y1: "10",
    x2: "4",
    y2: "3"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "21",
    x2: "12",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "8",
    x2: "12",
    y2: "3"
  }), react_default.a.createElement("line", {
    x1: "20",
    y1: "21",
    x2: "20",
    y2: "16"
  }), react_default.a.createElement("line", {
    x1: "20",
    y1: "12",
    x2: "20",
    y2: "3"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "14",
    x2: "7",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "8",
    x2: "15",
    y2: "8"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "16",
    x2: "23",
    y2: "16"
  }));
};

sliders_Sliders.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
sliders_Sliders.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var sliders = (sliders_Sliders);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/smartphone.js
function smartphone_extends() { smartphone_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return smartphone_extends.apply(this, arguments); }

function smartphone_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = smartphone_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function smartphone_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var smartphone_Smartphone = function Smartphone(props) {
  var color = props.color,
      size = props.size,
      otherProps = smartphone_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", smartphone_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "5",
    y: "2",
    width: "14",
    height: "20",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "18",
    x2: "12",
    y2: "18"
  }));
};

smartphone_Smartphone.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
smartphone_Smartphone.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var smartphone = (smartphone_Smartphone);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/smile.js
function smile_extends() { smile_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return smile_extends.apply(this, arguments); }

function smile_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = smile_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function smile_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var smile_Smile = function Smile(props) {
  var color = props.color,
      size = props.size,
      otherProps = smile_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", smile_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("path", {
    d: "M8 14s1.5 2 4 2 4-2 4-2"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "9",
    x2: "9.01",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "9",
    x2: "15.01",
    y2: "9"
  }));
};

smile_Smile.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
smile_Smile.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var smile = (smile_Smile);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/speaker.js
function speaker_extends() { speaker_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return speaker_extends.apply(this, arguments); }

function speaker_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = speaker_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function speaker_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var speaker_Speaker = function Speaker(props) {
  var color = props.color,
      size = props.size,
      otherProps = speaker_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", speaker_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "4",
    y: "2",
    width: "16",
    height: "20",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "14",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "6",
    x2: "12",
    y2: "6"
  }));
};

speaker_Speaker.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
speaker_Speaker.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var speaker = (speaker_Speaker);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/square.js
function square_extends() { square_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return square_extends.apply(this, arguments); }

function square_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = square_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function square_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var square_Square = function Square(props) {
  var color = props.color,
      size = props.size,
      otherProps = square_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", square_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }));
};

square_Square.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
square_Square.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var square = (square_Square);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/star.js
function star_extends() { star_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return star_extends.apply(this, arguments); }

function star_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = star_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function star_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var star_Star = function Star(props) {
  var color = props.color,
      size = props.size,
      otherProps = star_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", star_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"
  }));
};

star_Star.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
star_Star.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var star = (star_Star);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/stop-circle.js
function stop_circle_extends() { stop_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return stop_circle_extends.apply(this, arguments); }

function stop_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = stop_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function stop_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var stop_circle_StopCircle = function StopCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = stop_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", stop_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("rect", {
    x: "9",
    y: "9",
    width: "6",
    height: "6"
  }));
};

stop_circle_StopCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
stop_circle_StopCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var stop_circle = (stop_circle_StopCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/sun.js
function sun_extends() { sun_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return sun_extends.apply(this, arguments); }

function sun_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = sun_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function sun_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var sun_Sun = function Sun(props) {
  var color = props.color,
      size = props.size,
      otherProps = sun_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", sun_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "5"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "1",
    x2: "12",
    y2: "3"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "21",
    x2: "12",
    y2: "23"
  }), react_default.a.createElement("line", {
    x1: "4.22",
    y1: "4.22",
    x2: "5.64",
    y2: "5.64"
  }), react_default.a.createElement("line", {
    x1: "18.36",
    y1: "18.36",
    x2: "19.78",
    y2: "19.78"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "12",
    x2: "3",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "12",
    x2: "23",
    y2: "12"
  }), react_default.a.createElement("line", {
    x1: "4.22",
    y1: "19.78",
    x2: "5.64",
    y2: "18.36"
  }), react_default.a.createElement("line", {
    x1: "18.36",
    y1: "5.64",
    x2: "19.78",
    y2: "4.22"
  }));
};

sun_Sun.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
sun_Sun.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var sun = (sun_Sun);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/sunrise.js
function sunrise_extends() { sunrise_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return sunrise_extends.apply(this, arguments); }

function sunrise_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = sunrise_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function sunrise_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var sunrise_Sunrise = function Sunrise(props) {
  var color = props.color,
      size = props.size,
      otherProps = sunrise_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", sunrise_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M17 18a5 5 0 0 0-10 0"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "2",
    x2: "12",
    y2: "9"
  }), react_default.a.createElement("line", {
    x1: "4.22",
    y1: "10.22",
    x2: "5.64",
    y2: "11.64"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "18",
    x2: "3",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "18",
    x2: "23",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "18.36",
    y1: "11.64",
    x2: "19.78",
    y2: "10.22"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "22",
    x2: "1",
    y2: "22"
  }), react_default.a.createElement("polyline", {
    points: "8 6 12 2 16 6"
  }));
};

sunrise_Sunrise.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
sunrise_Sunrise.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var sunrise = (sunrise_Sunrise);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/sunset.js
function sunset_extends() { sunset_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return sunset_extends.apply(this, arguments); }

function sunset_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = sunset_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function sunset_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var sunset_Sunset = function Sunset(props) {
  var color = props.color,
      size = props.size,
      otherProps = sunset_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", sunset_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M17 18a5 5 0 0 0-10 0"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "9",
    x2: "12",
    y2: "2"
  }), react_default.a.createElement("line", {
    x1: "4.22",
    y1: "10.22",
    x2: "5.64",
    y2: "11.64"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "18",
    x2: "3",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "18",
    x2: "23",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "18.36",
    y1: "11.64",
    x2: "19.78",
    y2: "10.22"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "22",
    x2: "1",
    y2: "22"
  }), react_default.a.createElement("polyline", {
    points: "16 5 12 9 8 5"
  }));
};

sunset_Sunset.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
sunset_Sunset.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var sunset = (sunset_Sunset);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/tablet.js
function tablet_extends() { tablet_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return tablet_extends.apply(this, arguments); }

function tablet_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = tablet_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function tablet_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var tablet_Tablet = function Tablet(props) {
  var color = props.color,
      size = props.size,
      otherProps = tablet_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", tablet_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "4",
    y: "2",
    width: "16",
    height: "20",
    rx: "2",
    ry: "2",
    transform: "rotate(180 12 12)"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "18",
    x2: "12",
    y2: "18"
  }));
};

tablet_Tablet.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
tablet_Tablet.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var tablet = (tablet_Tablet);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/tag.js
function tag_extends() { tag_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return tag_extends.apply(this, arguments); }

function tag_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = tag_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function tag_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var tag_Tag = function Tag(props) {
  var color = props.color,
      size = props.size,
      otherProps = tag_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", tag_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z"
  }), react_default.a.createElement("line", {
    x1: "7",
    y1: "7",
    x2: "7",
    y2: "7"
  }));
};

tag_Tag.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
tag_Tag.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var tag = (tag_Tag);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/target.js
function target_extends() { target_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return target_extends.apply(this, arguments); }

function target_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = target_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function target_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var target_Target = function Target(props) {
  var color = props.color,
      size = props.size,
      otherProps = target_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", target_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "6"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "2"
  }));
};

target_Target.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
target_Target.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var target = (target_Target);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/terminal.js
function terminal_extends() { terminal_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return terminal_extends.apply(this, arguments); }

function terminal_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = terminal_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function terminal_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var terminal_Terminal = function Terminal(props) {
  var color = props.color,
      size = props.size,
      otherProps = terminal_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", terminal_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "4 17 10 11 4 5"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "19",
    x2: "20",
    y2: "19"
  }));
};

terminal_Terminal.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
terminal_Terminal.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var terminal = (terminal_Terminal);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/thermometer.js
function thermometer_extends() { thermometer_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return thermometer_extends.apply(this, arguments); }

function thermometer_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = thermometer_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function thermometer_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var thermometer_Thermometer = function Thermometer(props) {
  var color = props.color,
      size = props.size,
      otherProps = thermometer_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", thermometer_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M14 14.76V3.5a2.5 2.5 0 0 0-5 0v11.26a4.5 4.5 0 1 0 5 0z"
  }));
};

thermometer_Thermometer.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
thermometer_Thermometer.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var thermometer = (thermometer_Thermometer);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/thumbs-down.js
function thumbs_down_extends() { thumbs_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return thumbs_down_extends.apply(this, arguments); }

function thumbs_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = thumbs_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function thumbs_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var thumbs_down_ThumbsDown = function ThumbsDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = thumbs_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", thumbs_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M10 15v4a3 3 0 0 0 3 3l4-9V2H5.72a2 2 0 0 0-2 1.7l-1.38 9a2 2 0 0 0 2 2.3zm7-13h2.67A2.31 2.31 0 0 1 22 4v7a2.31 2.31 0 0 1-2.33 2H17"
  }));
};

thumbs_down_ThumbsDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
thumbs_down_ThumbsDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var thumbs_down = (thumbs_down_ThumbsDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/thumbs-up.js
function thumbs_up_extends() { thumbs_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return thumbs_up_extends.apply(this, arguments); }

function thumbs_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = thumbs_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function thumbs_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var thumbs_up_ThumbsUp = function ThumbsUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = thumbs_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", thumbs_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M14 9V5a3 3 0 0 0-3-3l-4 9v11h11.28a2 2 0 0 0 2-1.7l1.38-9a2 2 0 0 0-2-2.3zM7 22H4a2 2 0 0 1-2-2v-7a2 2 0 0 1 2-2h3"
  }));
};

thumbs_up_ThumbsUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
thumbs_up_ThumbsUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var thumbs_up = (thumbs_up_ThumbsUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/toggle-left.js
function toggle_left_extends() { toggle_left_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return toggle_left_extends.apply(this, arguments); }

function toggle_left_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = toggle_left_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function toggle_left_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var toggle_left_ToggleLeft = function ToggleLeft(props) {
  var color = props.color,
      size = props.size,
      otherProps = toggle_left_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", toggle_left_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "1",
    y: "5",
    width: "22",
    height: "14",
    rx: "7",
    ry: "7"
  }), react_default.a.createElement("circle", {
    cx: "8",
    cy: "12",
    r: "3"
  }));
};

toggle_left_ToggleLeft.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
toggle_left_ToggleLeft.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var toggle_left = (toggle_left_ToggleLeft);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/toggle-right.js
function toggle_right_extends() { toggle_right_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return toggle_right_extends.apply(this, arguments); }

function toggle_right_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = toggle_right_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function toggle_right_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var toggle_right_ToggleRight = function ToggleRight(props) {
  var color = props.color,
      size = props.size,
      otherProps = toggle_right_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", toggle_right_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "1",
    y: "5",
    width: "22",
    height: "14",
    rx: "7",
    ry: "7"
  }), react_default.a.createElement("circle", {
    cx: "16",
    cy: "12",
    r: "3"
  }));
};

toggle_right_ToggleRight.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
toggle_right_ToggleRight.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var toggle_right = (toggle_right_ToggleRight);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/trash-2.js
function trash_2_extends() { trash_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return trash_2_extends.apply(this, arguments); }

function trash_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = trash_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function trash_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var trash_2_Trash2 = function Trash2(props) {
  var color = props.color,
      size = props.size,
      otherProps = trash_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", trash_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "3 6 5 6 21 6"
  }), react_default.a.createElement("path", {
    d: "M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"
  }), react_default.a.createElement("line", {
    x1: "10",
    y1: "11",
    x2: "10",
    y2: "17"
  }), react_default.a.createElement("line", {
    x1: "14",
    y1: "11",
    x2: "14",
    y2: "17"
  }));
};

trash_2_Trash2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
trash_2_Trash2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var trash_2 = (trash_2_Trash2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/trash.js
function trash_extends() { trash_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return trash_extends.apply(this, arguments); }

function trash_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = trash_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function trash_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var trash_Trash = function Trash(props) {
  var color = props.color,
      size = props.size,
      otherProps = trash_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", trash_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "3 6 5 6 21 6"
  }), react_default.a.createElement("path", {
    d: "M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"
  }));
};

trash_Trash.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
trash_Trash.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var trash = (trash_Trash);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/trello.js
function trello_extends() { trello_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return trello_extends.apply(this, arguments); }

function trello_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = trello_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function trello_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var trello_Trello = function Trello(props) {
  var color = props.color,
      size = props.size,
      otherProps = trello_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", trello_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("rect", {
    x: "7",
    y: "7",
    width: "3",
    height: "9"
  }), react_default.a.createElement("rect", {
    x: "14",
    y: "7",
    width: "3",
    height: "5"
  }));
};

trello_Trello.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
trello_Trello.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var trello = (trello_Trello);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/trending-down.js
function trending_down_extends() { trending_down_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return trending_down_extends.apply(this, arguments); }

function trending_down_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = trending_down_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function trending_down_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var trending_down_TrendingDown = function TrendingDown(props) {
  var color = props.color,
      size = props.size,
      otherProps = trending_down_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", trending_down_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "23 18 13.5 8.5 8.5 13.5 1 6"
  }), react_default.a.createElement("polyline", {
    points: "17 18 23 18 23 12"
  }));
};

trending_down_TrendingDown.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
trending_down_TrendingDown.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var trending_down = (trending_down_TrendingDown);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/trending-up.js
function trending_up_extends() { trending_up_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return trending_up_extends.apply(this, arguments); }

function trending_up_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = trending_up_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function trending_up_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var trending_up_TrendingUp = function TrendingUp(props) {
  var color = props.color,
      size = props.size,
      otherProps = trending_up_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", trending_up_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "23 6 13.5 15.5 8.5 10.5 1 18"
  }), react_default.a.createElement("polyline", {
    points: "17 6 23 6 23 12"
  }));
};

trending_up_TrendingUp.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
trending_up_TrendingUp.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var trending_up = (trending_up_TrendingUp);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/triangle.js
function triangle_extends() { triangle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return triangle_extends.apply(this, arguments); }

function triangle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = triangle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function triangle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var triangle_Triangle = function Triangle(props) {
  var color = props.color,
      size = props.size,
      otherProps = triangle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", triangle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"
  }));
};

triangle_Triangle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
triangle_Triangle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var triangle = (triangle_Triangle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/truck.js
function truck_extends() { truck_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return truck_extends.apply(this, arguments); }

function truck_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = truck_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function truck_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var truck_Truck = function Truck(props) {
  var color = props.color,
      size = props.size,
      otherProps = truck_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", truck_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "1",
    y: "3",
    width: "15",
    height: "13"
  }), react_default.a.createElement("polygon", {
    points: "16 8 20 8 23 11 23 16 16 16 16 8"
  }), react_default.a.createElement("circle", {
    cx: "5.5",
    cy: "18.5",
    r: "2.5"
  }), react_default.a.createElement("circle", {
    cx: "18.5",
    cy: "18.5",
    r: "2.5"
  }));
};

truck_Truck.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
truck_Truck.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var truck = (truck_Truck);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/tv.js
function tv_extends() { tv_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return tv_extends.apply(this, arguments); }

function tv_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = tv_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function tv_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var tv_Tv = function Tv(props) {
  var color = props.color,
      size = props.size,
      otherProps = tv_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", tv_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "2",
    y: "7",
    width: "20",
    height: "15",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("polyline", {
    points: "17 2 12 7 7 2"
  }));
};

tv_Tv.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
tv_Tv.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var tv = (tv_Tv);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/twitter.js
function twitter_extends() { twitter_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return twitter_extends.apply(this, arguments); }

function twitter_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = twitter_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function twitter_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var twitter_Twitter = function Twitter(props) {
  var color = props.color,
      size = props.size,
      otherProps = twitter_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", twitter_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M23 3a10.9 10.9 0 0 1-3.14 1.53 4.48 4.48 0 0 0-7.86 3v1A10.66 10.66 0 0 1 3 4s-4 9 5 13a11.64 11.64 0 0 1-7 2c9 5 20 0 20-11.5a4.5 4.5 0 0 0-.08-.83A7.72 7.72 0 0 0 23 3z"
  }));
};

twitter_Twitter.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
twitter_Twitter.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var twitter = (twitter_Twitter);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/type.js
function type_extends() { type_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return type_extends.apply(this, arguments); }

function type_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = type_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function type_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var type_Type = function Type(props) {
  var color = props.color,
      size = props.size,
      otherProps = type_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", type_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "4 7 4 4 20 4 20 7"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "20",
    x2: "15",
    y2: "20"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "4",
    x2: "12",
    y2: "20"
  }));
};

type_Type.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
type_Type.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var type = (type_Type);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/umbrella.js
function umbrella_extends() { umbrella_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return umbrella_extends.apply(this, arguments); }

function umbrella_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = umbrella_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function umbrella_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var umbrella_Umbrella = function Umbrella(props) {
  var color = props.color,
      size = props.size,
      otherProps = umbrella_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", umbrella_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M23 12a11.05 11.05 0 0 0-22 0zm-5 7a3 3 0 0 1-6 0v-7"
  }));
};

umbrella_Umbrella.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
umbrella_Umbrella.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var umbrella = (umbrella_Umbrella);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/underline.js
function underline_extends() { underline_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return underline_extends.apply(this, arguments); }

function underline_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = underline_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function underline_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var underline_Underline = function Underline(props) {
  var color = props.color,
      size = props.size,
      otherProps = underline_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", underline_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M6 3v7a6 6 0 0 0 6 6 6 6 0 0 0 6-6V3"
  }), react_default.a.createElement("line", {
    x1: "4",
    y1: "21",
    x2: "20",
    y2: "21"
  }));
};

underline_Underline.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
underline_Underline.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var underline = (underline_Underline);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/unlock.js
function unlock_extends() { unlock_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return unlock_extends.apply(this, arguments); }

function unlock_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = unlock_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function unlock_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var unlock_Unlock = function Unlock(props) {
  var color = props.color,
      size = props.size,
      otherProps = unlock_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", unlock_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "11",
    width: "18",
    height: "11",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("path", {
    d: "M7 11V7a5 5 0 0 1 9.9-1"
  }));
};

unlock_Unlock.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
unlock_Unlock.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var unlock = (unlock_Unlock);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/upload-cloud.js
function upload_cloud_extends() { upload_cloud_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return upload_cloud_extends.apply(this, arguments); }

function upload_cloud_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = upload_cloud_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function upload_cloud_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var upload_cloud_UploadCloud = function UploadCloud(props) {
  var color = props.color,
      size = props.size,
      otherProps = upload_cloud_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", upload_cloud_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "16 16 12 12 8 16"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "12",
    x2: "12",
    y2: "21"
  }), react_default.a.createElement("path", {
    d: "M20.39 18.39A5 5 0 0 0 18 9h-1.26A8 8 0 1 0 3 16.3"
  }), react_default.a.createElement("polyline", {
    points: "16 16 12 12 8 16"
  }));
};

upload_cloud_UploadCloud.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
upload_cloud_UploadCloud.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var upload_cloud = (upload_cloud_UploadCloud);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/upload.js
function upload_extends() { upload_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return upload_extends.apply(this, arguments); }

function upload_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = upload_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function upload_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var upload_Upload = function Upload(props) {
  var color = props.color,
      size = props.size,
      otherProps = upload_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", upload_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"
  }), react_default.a.createElement("polyline", {
    points: "17 8 12 3 7 8"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "3",
    x2: "12",
    y2: "15"
  }));
};

upload_Upload.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
upload_Upload.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var upload = (upload_Upload);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/user-check.js
function user_check_extends() { user_check_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return user_check_extends.apply(this, arguments); }

function user_check_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = user_check_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function user_check_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var user_check_UserCheck = function UserCheck(props) {
  var color = props.color,
      size = props.size,
      otherProps = user_check_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", user_check_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"
  }), react_default.a.createElement("circle", {
    cx: "8.5",
    cy: "7",
    r: "4"
  }), react_default.a.createElement("polyline", {
    points: "17 11 19 13 23 9"
  }));
};

user_check_UserCheck.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
user_check_UserCheck.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var user_check = (user_check_UserCheck);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/user-minus.js
function user_minus_extends() { user_minus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return user_minus_extends.apply(this, arguments); }

function user_minus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = user_minus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function user_minus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var user_minus_UserMinus = function UserMinus(props) {
  var color = props.color,
      size = props.size,
      otherProps = user_minus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", user_minus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"
  }), react_default.a.createElement("circle", {
    cx: "8.5",
    cy: "7",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "11",
    x2: "17",
    y2: "11"
  }));
};

user_minus_UserMinus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
user_minus_UserMinus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var user_minus = (user_minus_UserMinus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/user-plus.js
function user_plus_extends() { user_plus_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return user_plus_extends.apply(this, arguments); }

function user_plus_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = user_plus_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function user_plus_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var user_plus_UserPlus = function UserPlus(props) {
  var color = props.color,
      size = props.size,
      otherProps = user_plus_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", user_plus_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"
  }), react_default.a.createElement("circle", {
    cx: "8.5",
    cy: "7",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "20",
    y1: "8",
    x2: "20",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "11",
    x2: "17",
    y2: "11"
  }));
};

user_plus_UserPlus.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
user_plus_UserPlus.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var user_plus = (user_plus_UserPlus);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/user-x.js
function user_x_extends() { user_x_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return user_x_extends.apply(this, arguments); }

function user_x_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = user_x_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function user_x_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var user_x_UserX = function UserX(props) {
  var color = props.color,
      size = props.size,
      otherProps = user_x_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", user_x_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"
  }), react_default.a.createElement("circle", {
    cx: "8.5",
    cy: "7",
    r: "4"
  }), react_default.a.createElement("line", {
    x1: "18",
    y1: "8",
    x2: "23",
    y2: "13"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "8",
    x2: "18",
    y2: "13"
  }));
};

user_x_UserX.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
user_x_UserX.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var user_x = (user_x_UserX);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/user.js
function user_extends() { user_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return user_extends.apply(this, arguments); }

function user_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = user_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function user_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var user_User = function User(props) {
  var color = props.color,
      size = props.size,
      otherProps = user_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", user_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"
  }), react_default.a.createElement("circle", {
    cx: "12",
    cy: "7",
    r: "4"
  }));
};

user_User.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
user_User.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var user = (user_User);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/users.js
function users_extends() { users_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return users_extends.apply(this, arguments); }

function users_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = users_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function users_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var users_Users = function Users(props) {
  var color = props.color,
      size = props.size,
      otherProps = users_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", users_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"
  }), react_default.a.createElement("circle", {
    cx: "9",
    cy: "7",
    r: "4"
  }), react_default.a.createElement("path", {
    d: "M23 21v-2a4 4 0 0 0-3-3.87"
  }), react_default.a.createElement("path", {
    d: "M16 3.13a4 4 0 0 1 0 7.75"
  }));
};

users_Users.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
users_Users.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var users = (users_Users);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/video-off.js
function video_off_extends() { video_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return video_off_extends.apply(this, arguments); }

function video_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = video_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function video_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var video_off_VideoOff = function VideoOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = video_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", video_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M16 16v1a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V7a2 2 0 0 1 2-2h2m5.66 0H14a2 2 0 0 1 2 2v3.34l1 1L23 7v10"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }));
};

video_off_VideoOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
video_off_VideoOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var video_off = (video_off_VideoOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/video.js
function video_extends() { video_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return video_extends.apply(this, arguments); }

function video_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = video_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function video_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var video_Video = function Video(props) {
  var color = props.color,
      size = props.size,
      otherProps = video_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", video_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "23 7 16 12 23 17 23 7"
  }), react_default.a.createElement("rect", {
    x: "1",
    y: "5",
    width: "15",
    height: "14",
    rx: "2",
    ry: "2"
  }));
};

video_Video.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
video_Video.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var video = (video_Video);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/voicemail.js
function voicemail_extends() { voicemail_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return voicemail_extends.apply(this, arguments); }

function voicemail_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = voicemail_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function voicemail_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var voicemail_Voicemail = function Voicemail(props) {
  var color = props.color,
      size = props.size,
      otherProps = voicemail_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", voicemail_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "5.5",
    cy: "11.5",
    r: "4.5"
  }), react_default.a.createElement("circle", {
    cx: "18.5",
    cy: "11.5",
    r: "4.5"
  }), react_default.a.createElement("line", {
    x1: "5.5",
    y1: "16",
    x2: "18.5",
    y2: "16"
  }));
};

voicemail_Voicemail.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
voicemail_Voicemail.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var voicemail = (voicemail_Voicemail);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/volume-1.js
function volume_1_extends() { volume_1_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return volume_1_extends.apply(this, arguments); }

function volume_1_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = volume_1_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function volume_1_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var volume_1_Volume1 = function Volume1(props) {
  var color = props.color,
      size = props.size,
      otherProps = volume_1_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", volume_1_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "11 5 6 9 2 9 2 15 6 15 11 19 11 5"
  }), react_default.a.createElement("path", {
    d: "M15.54 8.46a5 5 0 0 1 0 7.07"
  }));
};

volume_1_Volume1.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
volume_1_Volume1.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var volume_1 = (volume_1_Volume1);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/volume-2.js
function volume_2_extends() { volume_2_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return volume_2_extends.apply(this, arguments); }

function volume_2_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = volume_2_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function volume_2_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var volume_2_Volume2 = function Volume2(props) {
  var color = props.color,
      size = props.size,
      otherProps = volume_2_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", volume_2_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "11 5 6 9 2 9 2 15 6 15 11 19 11 5"
  }), react_default.a.createElement("path", {
    d: "M19.07 4.93a10 10 0 0 1 0 14.14M15.54 8.46a5 5 0 0 1 0 7.07"
  }));
};

volume_2_Volume2.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
volume_2_Volume2.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var volume_2 = (volume_2_Volume2);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/volume-x.js
function volume_x_extends() { volume_x_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return volume_x_extends.apply(this, arguments); }

function volume_x_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = volume_x_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function volume_x_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var volume_x_VolumeX = function VolumeX(props) {
  var color = props.color,
      size = props.size,
      otherProps = volume_x_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", volume_x_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "11 5 6 9 2 9 2 15 6 15 11 19 11 5"
  }), react_default.a.createElement("line", {
    x1: "23",
    y1: "9",
    x2: "17",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "17",
    y1: "9",
    x2: "23",
    y2: "15"
  }));
};

volume_x_VolumeX.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
volume_x_VolumeX.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var volume_x = (volume_x_VolumeX);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/volume.js
function volume_extends() { volume_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return volume_extends.apply(this, arguments); }

function volume_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = volume_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function volume_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var volume_Volume = function Volume(props) {
  var color = props.color,
      size = props.size,
      otherProps = volume_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", volume_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "11 5 6 9 2 9 2 15 6 15 11 19 11 5"
  }));
};

volume_Volume.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
volume_Volume.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var volume = (volume_Volume);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/watch.js
function watch_extends() { watch_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return watch_extends.apply(this, arguments); }

function watch_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = watch_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function watch_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var watch_Watch = function Watch(props) {
  var color = props.color,
      size = props.size,
      otherProps = watch_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", watch_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "7"
  }), react_default.a.createElement("polyline", {
    points: "12 9 12 12 13.5 13.5"
  }), react_default.a.createElement("path", {
    d: "M16.51 17.35l-.35 3.83a2 2 0 0 1-2 1.82H9.83a2 2 0 0 1-2-1.82l-.35-3.83m.01-10.7l.35-3.83A2 2 0 0 1 9.83 1h4.35a2 2 0 0 1 2 1.82l.35 3.83"
  }));
};

watch_Watch.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
watch_Watch.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var watch = (watch_Watch);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/wifi-off.js
function wifi_off_extends() { wifi_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return wifi_off_extends.apply(this, arguments); }

function wifi_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = wifi_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function wifi_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var wifi_off_WifiOff = function WifiOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = wifi_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", wifi_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }), react_default.a.createElement("path", {
    d: "M16.72 11.06A10.94 10.94 0 0 1 19 12.55"
  }), react_default.a.createElement("path", {
    d: "M5 12.55a10.94 10.94 0 0 1 5.17-2.39"
  }), react_default.a.createElement("path", {
    d: "M10.71 5.05A16 16 0 0 1 22.58 9"
  }), react_default.a.createElement("path", {
    d: "M1.42 9a15.91 15.91 0 0 1 4.7-2.88"
  }), react_default.a.createElement("path", {
    d: "M8.53 16.11a6 6 0 0 1 6.95 0"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "20",
    x2: "12",
    y2: "20"
  }));
};

wifi_off_WifiOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
wifi_off_WifiOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var wifi_off = (wifi_off_WifiOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/wifi.js
function wifi_extends() { wifi_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return wifi_extends.apply(this, arguments); }

function wifi_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = wifi_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function wifi_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var wifi_Wifi = function Wifi(props) {
  var color = props.color,
      size = props.size,
      otherProps = wifi_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", wifi_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M5 12.55a11 11 0 0 1 14.08 0"
  }), react_default.a.createElement("path", {
    d: "M1.42 9a16 16 0 0 1 21.16 0"
  }), react_default.a.createElement("path", {
    d: "M8.53 16.11a6 6 0 0 1 6.95 0"
  }), react_default.a.createElement("line", {
    x1: "12",
    y1: "20",
    x2: "12",
    y2: "20"
  }));
};

wifi_Wifi.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
wifi_Wifi.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var wifi = (wifi_Wifi);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/wind.js
function wind_extends() { wind_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return wind_extends.apply(this, arguments); }

function wind_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = wind_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function wind_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var wind_Wind = function Wind(props) {
  var color = props.color,
      size = props.size,
      otherProps = wind_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", wind_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M9.59 4.59A2 2 0 1 1 11 8H2m10.59 11.41A2 2 0 1 0 14 16H2m15.73-8.27A2.5 2.5 0 1 1 19.5 12H2"
  }));
};

wind_Wind.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
wind_Wind.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var wind = (wind_Wind);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/x-circle.js
function x_circle_extends() { x_circle_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return x_circle_extends.apply(this, arguments); }

function x_circle_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = x_circle_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function x_circle_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var x_circle_XCircle = function XCircle(props) {
  var color = props.color,
      size = props.size,
      otherProps = x_circle_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", x_circle_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "12",
    cy: "12",
    r: "10"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "9",
    x2: "9",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "9",
    x2: "15",
    y2: "15"
  }));
};

x_circle_XCircle.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
x_circle_XCircle.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var x_circle = (x_circle_XCircle);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/x-octagon.js
function x_octagon_extends() { x_octagon_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return x_octagon_extends.apply(this, arguments); }

function x_octagon_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = x_octagon_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function x_octagon_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var x_octagon_XOctagon = function XOctagon(props) {
  var color = props.color,
      size = props.size,
      otherProps = x_octagon_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", x_octagon_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "7.86 2 16.14 2 22 7.86 22 16.14 16.14 22 7.86 22 2 16.14 2 7.86 7.86 2"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "9",
    x2: "9",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "9",
    x2: "15",
    y2: "15"
  }));
};

x_octagon_XOctagon.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
x_octagon_XOctagon.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var x_octagon = (x_octagon_XOctagon);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/x-square.js
function x_square_extends() { x_square_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return x_square_extends.apply(this, arguments); }

function x_square_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = x_square_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function x_square_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var x_square_XSquare = function XSquare(props) {
  var color = props.color,
      size = props.size,
      otherProps = x_square_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", x_square_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("rect", {
    x: "3",
    y: "3",
    width: "18",
    height: "18",
    rx: "2",
    ry: "2"
  }), react_default.a.createElement("line", {
    x1: "9",
    y1: "9",
    x2: "15",
    y2: "15"
  }), react_default.a.createElement("line", {
    x1: "15",
    y1: "9",
    x2: "9",
    y2: "15"
  }));
};

x_square_XSquare.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
x_square_XSquare.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var x_square = (x_square_XSquare);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/x.js
function x_extends() { x_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return x_extends.apply(this, arguments); }

function x_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = x_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function x_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var x_X = function X(props) {
  var color = props.color,
      size = props.size,
      otherProps = x_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", x_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("line", {
    x1: "18",
    y1: "6",
    x2: "6",
    y2: "18"
  }), react_default.a.createElement("line", {
    x1: "6",
    y1: "6",
    x2: "18",
    y2: "18"
  }));
};

x_X.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
x_X.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var x = (x_X);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/youtube.js
function youtube_extends() { youtube_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return youtube_extends.apply(this, arguments); }

function youtube_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = youtube_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function youtube_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var youtube_Youtube = function Youtube(props) {
  var color = props.color,
      size = props.size,
      otherProps = youtube_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", youtube_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("path", {
    d: "M22.54 6.42a2.78 2.78 0 0 0-1.94-2C18.88 4 12 4 12 4s-6.88 0-8.6.46a2.78 2.78 0 0 0-1.94 2A29 29 0 0 0 1 11.75a29 29 0 0 0 .46 5.33A2.78 2.78 0 0 0 3.4 19c1.72.46 8.6.46 8.6.46s6.88 0 8.6-.46a2.78 2.78 0 0 0 1.94-2 29 29 0 0 0 .46-5.25 29 29 0 0 0-.46-5.33z"
  }), react_default.a.createElement("polygon", {
    points: "9.75 15.02 15.5 11.75 9.75 8.48 9.75 15.02"
  }));
};

youtube_Youtube.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
youtube_Youtube.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var youtube = (youtube_Youtube);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/zap-off.js
function zap_off_extends() { zap_off_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return zap_off_extends.apply(this, arguments); }

function zap_off_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = zap_off_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function zap_off_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var zap_off_ZapOff = function ZapOff(props) {
  var color = props.color,
      size = props.size,
      otherProps = zap_off_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", zap_off_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polyline", {
    points: "12.41 6.75 13 2 10.57 4.92"
  }), react_default.a.createElement("polyline", {
    points: "18.57 12.91 21 10 15.66 10"
  }), react_default.a.createElement("polyline", {
    points: "8 8 3 14 12 14 11 22 16 16"
  }), react_default.a.createElement("line", {
    x1: "1",
    y1: "1",
    x2: "23",
    y2: "23"
  }));
};

zap_off_ZapOff.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
zap_off_ZapOff.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var zap_off = (zap_off_ZapOff);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/zap.js
function zap_extends() { zap_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return zap_extends.apply(this, arguments); }

function zap_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = zap_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function zap_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var zap_Zap = function Zap(props) {
  var color = props.color,
      size = props.size,
      otherProps = zap_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", zap_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("polygon", {
    points: "13 2 3 14 12 14 11 22 21 10 12 10 13 2"
  }));
};

zap_Zap.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
zap_Zap.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var zap = (zap_Zap);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/zoom-in.js
function zoom_in_extends() { zoom_in_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return zoom_in_extends.apply(this, arguments); }

function zoom_in_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = zoom_in_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function zoom_in_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var zoom_in_ZoomIn = function ZoomIn(props) {
  var color = props.color,
      size = props.size,
      otherProps = zoom_in_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", zoom_in_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "11",
    cy: "11",
    r: "8"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "21",
    x2: "16.65",
    y2: "16.65"
  }), react_default.a.createElement("line", {
    x1: "11",
    y1: "8",
    x2: "11",
    y2: "14"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "11",
    x2: "14",
    y2: "11"
  }));
};

zoom_in_ZoomIn.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
zoom_in_ZoomIn.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var zoom_in = (zoom_in_ZoomIn);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/icons/zoom-out.js
function zoom_out_extends() { zoom_out_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return zoom_out_extends.apply(this, arguments); }

function zoom_out_objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = zoom_out_objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function zoom_out_objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




var zoom_out_ZoomOut = function ZoomOut(props) {
  var color = props.color,
      size = props.size,
      otherProps = zoom_out_objectWithoutProperties(props, ["color", "size"]);

  return react_default.a.createElement("svg", zoom_out_extends({
    xmlns: "http://www.w3.org/2000/svg",
    width: size,
    height: size,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: color,
    strokeWidth: "2",
    strokeLinecap: "round",
    strokeLinejoin: "round"
  }, otherProps), react_default.a.createElement("circle", {
    cx: "11",
    cy: "11",
    r: "8"
  }), react_default.a.createElement("line", {
    x1: "21",
    y1: "21",
    x2: "16.65",
    y2: "16.65"
  }), react_default.a.createElement("line", {
    x1: "8",
    y1: "11",
    x2: "14",
    y2: "11"
  }));
};

zoom_out_ZoomOut.propTypes = {
  color: prop_types_default.a.string,
  size: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};
zoom_out_ZoomOut.defaultProps = {
  color: 'currentColor',
  size: '24'
};
/* harmony default export */ var zoom_out = (zoom_out_ZoomOut);
// CONCATENATED MODULE: ./node_modules/react-feather/dist/index.js

























































































































































































































































































/***/ }),

/***/ 35:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/*
object-assign
(c) Sindre Sorhus
@license MIT
*/


/* eslint-disable no-unused-vars */
var getOwnPropertySymbols = Object.getOwnPropertySymbols;
var hasOwnProperty = Object.prototype.hasOwnProperty;
var propIsEnumerable = Object.prototype.propertyIsEnumerable;

function toObject(val) {
	if (val === null || val === undefined) {
		throw new TypeError('Object.assign cannot be called with null or undefined');
	}

	return Object(val);
}

function shouldUseNative() {
	try {
		if (!Object.assign) {
			return false;
		}

		// Detect buggy property enumeration order in older V8 versions.

		// https://bugs.chromium.org/p/v8/issues/detail?id=4118
		var test1 = new String('abc');  // eslint-disable-line no-new-wrappers
		test1[5] = 'de';
		if (Object.getOwnPropertyNames(test1)[0] === '5') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test2 = {};
		for (var i = 0; i < 10; i++) {
			test2['_' + String.fromCharCode(i)] = i;
		}
		var order2 = Object.getOwnPropertyNames(test2).map(function (n) {
			return test2[n];
		});
		if (order2.join('') !== '0123456789') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test3 = {};
		'abcdefghijklmnopqrst'.split('').forEach(function (letter) {
			test3[letter] = letter;
		});
		if (Object.keys(Object.assign({}, test3)).join('') !==
				'abcdefghijklmnopqrst') {
			return false;
		}

		return true;
	} catch (err) {
		// We don't expect any of the above to throw, but better to be safe.
		return false;
	}
}

module.exports = shouldUseNative() ? Object.assign : function (target, source) {
	var from;
	var to = toObject(target);
	var symbols;

	for (var s = 1; s < arguments.length; s++) {
		from = Object(arguments[s]);

		for (var key in from) {
			if (hasOwnProperty.call(from, key)) {
				to[key] = from[key];
			}
		}

		if (getOwnPropertySymbols) {
			symbols = getOwnPropertySymbols(from);
			for (var i = 0; i < symbols.length; i++) {
				if (propIsEnumerable.call(from, symbols[i])) {
					to[symbols[i]] = from[symbols[i]];
				}
			}
		}
	}

	return to;
};


/***/ }),

/***/ 36:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);


function Footer(props) {
  var links = props.links.map(function (link, index) {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", {
      key: index
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("a", {
      href: link.url
    }, link.text));
  });
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("footer", {
    className: "footer"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "content"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, links)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "content has-text-centered has-text-white"
  }, props.copyright));
}

/* harmony default export */ __webpack_exports__["a"] = (Footer);

/***/ }),

/***/ 37:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

// EXTERNAL MODULE: ./node_modules/babel-preset-react-app/node_modules/@babel/runtime/helpers/esm/slicedToArray.js + 3 modules
var slicedToArray = __webpack_require__(12);

// EXTERNAL MODULE: ./node_modules/react/index.js
var react = __webpack_require__(0);
var react_default = /*#__PURE__*/__webpack_require__.n(react);

// CONCATENATED MODULE: ./src/shared/components/LanguageSwitcher/LanguageSwitcher.js


function LanguageSwitcher(props) {
  function handleLanguageChange(e) {
    if (typeof window !== 'undefined' && e && e.target) {
      window.location.href = e.target.value;
    }
  }

  var languages = props.availableLanguages.map(function (language, index) {
    return /*#__PURE__*/react_default.a.createElement("option", {
      key: index,
      value: language.url
    }, language.text);
  });
  return /*#__PURE__*/react_default.a.createElement("div", {
    className: "select"
  }, /*#__PURE__*/react_default.a.createElement("select", {
    value: props.selectedLanguageUrl,
    onChange: function onChange(e) {
      return handleLanguageChange(e);
    }
  }, languages));
}

/* harmony default export */ var LanguageSwitcher_LanguageSwitcher = (LanguageSwitcher);
// CONCATENATED MODULE: ./src/shared/layouts/images/logo.png
/* harmony default export */ var logo = ("/dist/images/logo.png");
// CONCATENATED MODULE: ./src/shared/components/Header/Header.js





function Header(props) {
  var _useState = Object(react["useState"])(false),
      _useState2 = Object(slicedToArray["a" /* default */])(_useState, 2),
      isActive = _useState2[0],
      setIsActive = _useState2[1];

  var links = props.links.map(function (link, index) {
    return /*#__PURE__*/react_default.a.createElement("a", {
      key: index,
      className: "navbar-item",
      href: link.url
    }, link.text);
  });
  return /*#__PURE__*/react_default.a.createElement("header", null, /*#__PURE__*/react_default.a.createElement("nav", {
    className: "navbar is-spaced"
  }, /*#__PURE__*/react_default.a.createElement("div", {
    className: "navbar-brand"
  }, /*#__PURE__*/react_default.a.createElement("a", {
    href: props.logo.targetUrl
  }, /*#__PURE__*/react_default.a.createElement("img", {
    src: logo,
    alt: props.logo.logoAltLabel
  })), /*#__PURE__*/react_default.a.createElement("div", {
    role: "button",
    onClick: function onClick() {
      return setIsActive(!isActive);
    },
    className: isActive ? 'navbar-burger is-active' : 'navbar-burger',
    "aria-label": "menu",
    "aria-expanded": "false"
  }, /*#__PURE__*/react_default.a.createElement("span", {
    "aria-hidden": "true"
  }), /*#__PURE__*/react_default.a.createElement("span", {
    "aria-hidden": "true"
  }), /*#__PURE__*/react_default.a.createElement("span", {
    "aria-hidden": "true"
  }))), /*#__PURE__*/react_default.a.createElement("div", {
    className: isActive ? 'navbar-menu is-active' : 'navbar-menu'
  }, /*#__PURE__*/react_default.a.createElement("div", {
    className: "navbar-start"
  }, links), /*#__PURE__*/react_default.a.createElement("div", {
    className: "navbar-end"
  }, /*#__PURE__*/react_default.a.createElement("div", {
    className: "navbar-item"
  }, /*#__PURE__*/react_default.a.createElement(LanguageSwitcher_LanguageSwitcher, props.languageSwitcher))))));
}

/* harmony default export */ var Header_Header = __webpack_exports__["a"] = (Header);

/***/ }),

/***/ 38:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

// EXTERNAL MODULE: ./node_modules/react/index.js
var react = __webpack_require__(0);
var react_default = /*#__PURE__*/__webpack_require__.n(react);

// EXTERNAL MODULE: ./node_modules/react-feather/dist/index.js + 279 modules
var dist = __webpack_require__(28);

// CONCATENATED MODULE: ./src/shared/components/MenuTiles/Tile.js



function Tile(props) {
  var IconTag = dist[props.icon];
  return /*#__PURE__*/react_default.a.createElement("a", {
    href: props.url,
    className: "tile"
  }, /*#__PURE__*/react_default.a.createElement("div", {
    className: "tile__icon"
  }, /*#__PURE__*/react_default.a.createElement(IconTag, {
    size: 32
  })), /*#__PURE__*/react_default.a.createElement("div", {
    className: "tile__title"
  }, props.title));
}

/* harmony default export */ var MenuTiles_Tile = (Tile);
// CONCATENATED MODULE: ./src/shared/components/MenuTiles/MenuTiles.js



function MenuTiles(props) {
  return /*#__PURE__*/react_default.a.createElement("nav", {
    className: "section menu-tiles"
  }, props.tiles.map(function (tile, index) {
    return /*#__PURE__*/react_default.a.createElement(MenuTiles_Tile, {
      key: index,
      icon: tile.icon,
      title: tile.title,
      url: tile.url
    });
  }));
}

/* harmony default export */ var MenuTiles_MenuTiles = __webpack_exports__["a"] = (MenuTiles);

/***/ }),

/***/ 424:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
// ESM COMPAT FLAG
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: ./node_modules/react/index.js
var react = __webpack_require__(0);
var react_default = /*#__PURE__*/__webpack_require__.n(react);

// EXTERNAL MODULE: ./node_modules/react-dom/index.js
var react_dom = __webpack_require__(7);
var react_dom_default = /*#__PURE__*/__webpack_require__.n(react_dom);

// EXTERNAL MODULE: ./src/shared/components/Header/Header.js + 2 modules
var Header = __webpack_require__(37);

// EXTERNAL MODULE: ./src/shared/components/Footer/Footer.js
var Footer = __webpack_require__(36);

// EXTERNAL MODULE: ./src/shared/components/MenuTiles/MenuTiles.js + 1 modules
var MenuTiles = __webpack_require__(38);

// EXTERNAL MODULE: ./src/shared/components/Catalog/Catalog.js
var Catalog = __webpack_require__(73);

// EXTERNAL MODULE: ./src/shared/layouts/images/favicon.png
var favicon = __webpack_require__(59);

// CONCATENATED MODULE: ./src/project/Tenant.Portal/areas/Products/pages/ProductPage/ProductPage.js





/* eslint-disable no-unused-vars */


/* eslint-enable no-unused-vars */

function ProductPage(props) {
  return /*#__PURE__*/react_default.a.createElement("div", null, /*#__PURE__*/react_default.a.createElement(Header["a" /* default */], props.header), /*#__PURE__*/react_default.a.createElement(MenuTiles["a" /* default */], props.menuTiles), /*#__PURE__*/react_default.a.createElement(Catalog["a" /* default */], props.catalog), /*#__PURE__*/react_default.a.createElement(Footer["a" /* default */], props.footer));
}

/* harmony default export */ var ProductPage_ProductPage = (ProductPage);
// CONCATENATED MODULE: ./src/project/Tenant.Portal/areas/Products/pages/ProductPage/index.js



react_dom_default.a.hydrate( /*#__PURE__*/react_default.a.createElement(ProductPage_ProductPage, window.data), document.getElementById('root'));

/***/ }),

/***/ 53:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/** @license React v16.13.1
 * react.production.min.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

var l=__webpack_require__(35),n="function"===typeof Symbol&&Symbol.for,p=n?Symbol.for("react.element"):60103,q=n?Symbol.for("react.portal"):60106,r=n?Symbol.for("react.fragment"):60107,t=n?Symbol.for("react.strict_mode"):60108,u=n?Symbol.for("react.profiler"):60114,v=n?Symbol.for("react.provider"):60109,w=n?Symbol.for("react.context"):60110,x=n?Symbol.for("react.forward_ref"):60112,y=n?Symbol.for("react.suspense"):60113,z=n?Symbol.for("react.memo"):60115,A=n?Symbol.for("react.lazy"):
60116,B="function"===typeof Symbol&&Symbol.iterator;function C(a){for(var b="https://reactjs.org/docs/error-decoder.html?invariant="+a,c=1;c<arguments.length;c++)b+="&args[]="+encodeURIComponent(arguments[c]);return"Minified React error #"+a+"; visit "+b+" for the full message or use the non-minified dev environment for full errors and additional helpful warnings."}
var D={isMounted:function(){return!1},enqueueForceUpdate:function(){},enqueueReplaceState:function(){},enqueueSetState:function(){}},E={};function F(a,b,c){this.props=a;this.context=b;this.refs=E;this.updater=c||D}F.prototype.isReactComponent={};F.prototype.setState=function(a,b){if("object"!==typeof a&&"function"!==typeof a&&null!=a)throw Error(C(85));this.updater.enqueueSetState(this,a,b,"setState")};F.prototype.forceUpdate=function(a){this.updater.enqueueForceUpdate(this,a,"forceUpdate")};
function G(){}G.prototype=F.prototype;function H(a,b,c){this.props=a;this.context=b;this.refs=E;this.updater=c||D}var I=H.prototype=new G;I.constructor=H;l(I,F.prototype);I.isPureReactComponent=!0;var J={current:null},K=Object.prototype.hasOwnProperty,L={key:!0,ref:!0,__self:!0,__source:!0};
function M(a,b,c){var e,d={},g=null,k=null;if(null!=b)for(e in void 0!==b.ref&&(k=b.ref),void 0!==b.key&&(g=""+b.key),b)K.call(b,e)&&!L.hasOwnProperty(e)&&(d[e]=b[e]);var f=arguments.length-2;if(1===f)d.children=c;else if(1<f){for(var h=Array(f),m=0;m<f;m++)h[m]=arguments[m+2];d.children=h}if(a&&a.defaultProps)for(e in f=a.defaultProps,f)void 0===d[e]&&(d[e]=f[e]);return{$$typeof:p,type:a,key:g,ref:k,props:d,_owner:J.current}}
function N(a,b){return{$$typeof:p,type:a.type,key:b,ref:a.ref,props:a.props,_owner:a._owner}}function O(a){return"object"===typeof a&&null!==a&&a.$$typeof===p}function escape(a){var b={"=":"=0",":":"=2"};return"$"+(""+a).replace(/[=:]/g,function(a){return b[a]})}var P=/\/+/g,Q=[];function R(a,b,c,e){if(Q.length){var d=Q.pop();d.result=a;d.keyPrefix=b;d.func=c;d.context=e;d.count=0;return d}return{result:a,keyPrefix:b,func:c,context:e,count:0}}
function S(a){a.result=null;a.keyPrefix=null;a.func=null;a.context=null;a.count=0;10>Q.length&&Q.push(a)}
function T(a,b,c,e){var d=typeof a;if("undefined"===d||"boolean"===d)a=null;var g=!1;if(null===a)g=!0;else switch(d){case "string":case "number":g=!0;break;case "object":switch(a.$$typeof){case p:case q:g=!0}}if(g)return c(e,a,""===b?"."+U(a,0):b),1;g=0;b=""===b?".":b+":";if(Array.isArray(a))for(var k=0;k<a.length;k++){d=a[k];var f=b+U(d,k);g+=T(d,f,c,e)}else if(null===a||"object"!==typeof a?f=null:(f=B&&a[B]||a["@@iterator"],f="function"===typeof f?f:null),"function"===typeof f)for(a=f.call(a),k=
0;!(d=a.next()).done;)d=d.value,f=b+U(d,k++),g+=T(d,f,c,e);else if("object"===d)throw c=""+a,Error(C(31,"[object Object]"===c?"object with keys {"+Object.keys(a).join(", ")+"}":c,""));return g}function V(a,b,c){return null==a?0:T(a,"",b,c)}function U(a,b){return"object"===typeof a&&null!==a&&null!=a.key?escape(a.key):b.toString(36)}function W(a,b){a.func.call(a.context,b,a.count++)}
function aa(a,b,c){var e=a.result,d=a.keyPrefix;a=a.func.call(a.context,b,a.count++);Array.isArray(a)?X(a,e,c,function(a){return a}):null!=a&&(O(a)&&(a=N(a,d+(!a.key||b&&b.key===a.key?"":(""+a.key).replace(P,"$&/")+"/")+c)),e.push(a))}function X(a,b,c,e,d){var g="";null!=c&&(g=(""+c).replace(P,"$&/")+"/");b=R(b,g,e,d);V(a,aa,b);S(b)}var Y={current:null};function Z(){var a=Y.current;if(null===a)throw Error(C(321));return a}
var ba={ReactCurrentDispatcher:Y,ReactCurrentBatchConfig:{suspense:null},ReactCurrentOwner:J,IsSomeRendererActing:{current:!1},assign:l};exports.Children={map:function(a,b,c){if(null==a)return a;var e=[];X(a,e,null,b,c);return e},forEach:function(a,b,c){if(null==a)return a;b=R(null,null,b,c);V(a,W,b);S(b)},count:function(a){return V(a,function(){return null},null)},toArray:function(a){var b=[];X(a,b,null,function(a){return a});return b},only:function(a){if(!O(a))throw Error(C(143));return a}};
exports.Component=F;exports.Fragment=r;exports.Profiler=u;exports.PureComponent=H;exports.StrictMode=t;exports.Suspense=y;exports.__SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED=ba;
exports.cloneElement=function(a,b,c){if(null===a||void 0===a)throw Error(C(267,a));var e=l({},a.props),d=a.key,g=a.ref,k=a._owner;if(null!=b){void 0!==b.ref&&(g=b.ref,k=J.current);void 0!==b.key&&(d=""+b.key);if(a.type&&a.type.defaultProps)var f=a.type.defaultProps;for(h in b)K.call(b,h)&&!L.hasOwnProperty(h)&&(e[h]=void 0===b[h]&&void 0!==f?f[h]:b[h])}var h=arguments.length-2;if(1===h)e.children=c;else if(1<h){f=Array(h);for(var m=0;m<h;m++)f[m]=arguments[m+2];e.children=f}return{$$typeof:p,type:a.type,
key:d,ref:g,props:e,_owner:k}};exports.createContext=function(a,b){void 0===b&&(b=null);a={$$typeof:w,_calculateChangedBits:b,_currentValue:a,_currentValue2:a,_threadCount:0,Provider:null,Consumer:null};a.Provider={$$typeof:v,_context:a};return a.Consumer=a};exports.createElement=M;exports.createFactory=function(a){var b=M.bind(null,a);b.type=a;return b};exports.createRef=function(){return{current:null}};exports.forwardRef=function(a){return{$$typeof:x,render:a}};exports.isValidElement=O;
exports.lazy=function(a){return{$$typeof:A,_ctor:a,_status:-1,_result:null}};exports.memo=function(a,b){return{$$typeof:z,type:a,compare:void 0===b?null:b}};exports.useCallback=function(a,b){return Z().useCallback(a,b)};exports.useContext=function(a,b){return Z().useContext(a,b)};exports.useDebugValue=function(){};exports.useEffect=function(a,b){return Z().useEffect(a,b)};exports.useImperativeHandle=function(a,b,c){return Z().useImperativeHandle(a,b,c)};
exports.useLayoutEffect=function(a,b){return Z().useLayoutEffect(a,b)};exports.useMemo=function(a,b){return Z().useMemo(a,b)};exports.useReducer=function(a,b,c){return Z().useReducer(a,b,c)};exports.useRef=function(a){return Z().useRef(a)};exports.useState=function(a){return Z().useState(a)};exports.version="16.13.1";


/***/ }),

/***/ 54:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/** @license React v16.13.1
 * react-dom.production.min.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

/*
 Modernizr 3.0.0pre (Custom Build) | MIT
*/
var aa=__webpack_require__(0),n=__webpack_require__(35),r=__webpack_require__(55);function u(a){for(var b="https://reactjs.org/docs/error-decoder.html?invariant="+a,c=1;c<arguments.length;c++)b+="&args[]="+encodeURIComponent(arguments[c]);return"Minified React error #"+a+"; visit "+b+" for the full message or use the non-minified dev environment for full errors and additional helpful warnings."}if(!aa)throw Error(u(227));
function ba(a,b,c,d,e,f,g,h,k){var l=Array.prototype.slice.call(arguments,3);try{b.apply(c,l)}catch(m){this.onError(m)}}var da=!1,ea=null,fa=!1,ha=null,ia={onError:function(a){da=!0;ea=a}};function ja(a,b,c,d,e,f,g,h,k){da=!1;ea=null;ba.apply(ia,arguments)}function ka(a,b,c,d,e,f,g,h,k){ja.apply(this,arguments);if(da){if(da){var l=ea;da=!1;ea=null}else throw Error(u(198));fa||(fa=!0,ha=l)}}var la=null,ma=null,na=null;
function oa(a,b,c){var d=a.type||"unknown-event";a.currentTarget=na(c);ka(d,b,void 0,a);a.currentTarget=null}var pa=null,qa={};
function ra(){if(pa)for(var a in qa){var b=qa[a],c=pa.indexOf(a);if(!(-1<c))throw Error(u(96,a));if(!sa[c]){if(!b.extractEvents)throw Error(u(97,a));sa[c]=b;c=b.eventTypes;for(var d in c){var e=void 0;var f=c[d],g=b,h=d;if(ta.hasOwnProperty(h))throw Error(u(99,h));ta[h]=f;var k=f.phasedRegistrationNames;if(k){for(e in k)k.hasOwnProperty(e)&&ua(k[e],g,h);e=!0}else f.registrationName?(ua(f.registrationName,g,h),e=!0):e=!1;if(!e)throw Error(u(98,d,a));}}}}
function ua(a,b,c){if(va[a])throw Error(u(100,a));va[a]=b;wa[a]=b.eventTypes[c].dependencies}var sa=[],ta={},va={},wa={};function xa(a){var b=!1,c;for(c in a)if(a.hasOwnProperty(c)){var d=a[c];if(!qa.hasOwnProperty(c)||qa[c]!==d){if(qa[c])throw Error(u(102,c));qa[c]=d;b=!0}}b&&ra()}var ya=!("undefined"===typeof window||"undefined"===typeof window.document||"undefined"===typeof window.document.createElement),za=null,Aa=null,Ba=null;
function Ca(a){if(a=ma(a)){if("function"!==typeof za)throw Error(u(280));var b=a.stateNode;b&&(b=la(b),za(a.stateNode,a.type,b))}}function Da(a){Aa?Ba?Ba.push(a):Ba=[a]:Aa=a}function Ea(){if(Aa){var a=Aa,b=Ba;Ba=Aa=null;Ca(a);if(b)for(a=0;a<b.length;a++)Ca(b[a])}}function Fa(a,b){return a(b)}function Ga(a,b,c,d,e){return a(b,c,d,e)}function Ha(){}var Ia=Fa,Ja=!1,Ka=!1;function La(){if(null!==Aa||null!==Ba)Ha(),Ea()}
function Ma(a,b,c){if(Ka)return a(b,c);Ka=!0;try{return Ia(a,b,c)}finally{Ka=!1,La()}}var Na=/^[:A-Z_a-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD][:A-Z_a-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD\-.0-9\u00B7\u0300-\u036F\u203F-\u2040]*$/,Oa=Object.prototype.hasOwnProperty,Pa={},Qa={};
function Ra(a){if(Oa.call(Qa,a))return!0;if(Oa.call(Pa,a))return!1;if(Na.test(a))return Qa[a]=!0;Pa[a]=!0;return!1}function Sa(a,b,c,d){if(null!==c&&0===c.type)return!1;switch(typeof b){case "function":case "symbol":return!0;case "boolean":if(d)return!1;if(null!==c)return!c.acceptsBooleans;a=a.toLowerCase().slice(0,5);return"data-"!==a&&"aria-"!==a;default:return!1}}
function Ta(a,b,c,d){if(null===b||"undefined"===typeof b||Sa(a,b,c,d))return!0;if(d)return!1;if(null!==c)switch(c.type){case 3:return!b;case 4:return!1===b;case 5:return isNaN(b);case 6:return isNaN(b)||1>b}return!1}function v(a,b,c,d,e,f){this.acceptsBooleans=2===b||3===b||4===b;this.attributeName=d;this.attributeNamespace=e;this.mustUseProperty=c;this.propertyName=a;this.type=b;this.sanitizeURL=f}var C={};
"children dangerouslySetInnerHTML defaultValue defaultChecked innerHTML suppressContentEditableWarning suppressHydrationWarning style".split(" ").forEach(function(a){C[a]=new v(a,0,!1,a,null,!1)});[["acceptCharset","accept-charset"],["className","class"],["htmlFor","for"],["httpEquiv","http-equiv"]].forEach(function(a){var b=a[0];C[b]=new v(b,1,!1,a[1],null,!1)});["contentEditable","draggable","spellCheck","value"].forEach(function(a){C[a]=new v(a,2,!1,a.toLowerCase(),null,!1)});
["autoReverse","externalResourcesRequired","focusable","preserveAlpha"].forEach(function(a){C[a]=new v(a,2,!1,a,null,!1)});"allowFullScreen async autoFocus autoPlay controls default defer disabled disablePictureInPicture formNoValidate hidden loop noModule noValidate open playsInline readOnly required reversed scoped seamless itemScope".split(" ").forEach(function(a){C[a]=new v(a,3,!1,a.toLowerCase(),null,!1)});
["checked","multiple","muted","selected"].forEach(function(a){C[a]=new v(a,3,!0,a,null,!1)});["capture","download"].forEach(function(a){C[a]=new v(a,4,!1,a,null,!1)});["cols","rows","size","span"].forEach(function(a){C[a]=new v(a,6,!1,a,null,!1)});["rowSpan","start"].forEach(function(a){C[a]=new v(a,5,!1,a.toLowerCase(),null,!1)});var Ua=/[\-:]([a-z])/g;function Va(a){return a[1].toUpperCase()}
"accent-height alignment-baseline arabic-form baseline-shift cap-height clip-path clip-rule color-interpolation color-interpolation-filters color-profile color-rendering dominant-baseline enable-background fill-opacity fill-rule flood-color flood-opacity font-family font-size font-size-adjust font-stretch font-style font-variant font-weight glyph-name glyph-orientation-horizontal glyph-orientation-vertical horiz-adv-x horiz-origin-x image-rendering letter-spacing lighting-color marker-end marker-mid marker-start overline-position overline-thickness paint-order panose-1 pointer-events rendering-intent shape-rendering stop-color stop-opacity strikethrough-position strikethrough-thickness stroke-dasharray stroke-dashoffset stroke-linecap stroke-linejoin stroke-miterlimit stroke-opacity stroke-width text-anchor text-decoration text-rendering underline-position underline-thickness unicode-bidi unicode-range units-per-em v-alphabetic v-hanging v-ideographic v-mathematical vector-effect vert-adv-y vert-origin-x vert-origin-y word-spacing writing-mode xmlns:xlink x-height".split(" ").forEach(function(a){var b=a.replace(Ua,
Va);C[b]=new v(b,1,!1,a,null,!1)});"xlink:actuate xlink:arcrole xlink:role xlink:show xlink:title xlink:type".split(" ").forEach(function(a){var b=a.replace(Ua,Va);C[b]=new v(b,1,!1,a,"http://www.w3.org/1999/xlink",!1)});["xml:base","xml:lang","xml:space"].forEach(function(a){var b=a.replace(Ua,Va);C[b]=new v(b,1,!1,a,"http://www.w3.org/XML/1998/namespace",!1)});["tabIndex","crossOrigin"].forEach(function(a){C[a]=new v(a,1,!1,a.toLowerCase(),null,!1)});
C.xlinkHref=new v("xlinkHref",1,!1,"xlink:href","http://www.w3.org/1999/xlink",!0);["src","href","action","formAction"].forEach(function(a){C[a]=new v(a,1,!1,a.toLowerCase(),null,!0)});var Wa=aa.__SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED;Wa.hasOwnProperty("ReactCurrentDispatcher")||(Wa.ReactCurrentDispatcher={current:null});Wa.hasOwnProperty("ReactCurrentBatchConfig")||(Wa.ReactCurrentBatchConfig={suspense:null});
function Xa(a,b,c,d){var e=C.hasOwnProperty(b)?C[b]:null;var f=null!==e?0===e.type:d?!1:!(2<b.length)||"o"!==b[0]&&"O"!==b[0]||"n"!==b[1]&&"N"!==b[1]?!1:!0;f||(Ta(b,c,e,d)&&(c=null),d||null===e?Ra(b)&&(null===c?a.removeAttribute(b):a.setAttribute(b,""+c)):e.mustUseProperty?a[e.propertyName]=null===c?3===e.type?!1:"":c:(b=e.attributeName,d=e.attributeNamespace,null===c?a.removeAttribute(b):(e=e.type,c=3===e||4===e&&!0===c?"":""+c,d?a.setAttributeNS(d,b,c):a.setAttribute(b,c))))}
var Ya=/^(.*)[\\\/]/,E="function"===typeof Symbol&&Symbol.for,Za=E?Symbol.for("react.element"):60103,$a=E?Symbol.for("react.portal"):60106,ab=E?Symbol.for("react.fragment"):60107,bb=E?Symbol.for("react.strict_mode"):60108,cb=E?Symbol.for("react.profiler"):60114,db=E?Symbol.for("react.provider"):60109,eb=E?Symbol.for("react.context"):60110,fb=E?Symbol.for("react.concurrent_mode"):60111,gb=E?Symbol.for("react.forward_ref"):60112,hb=E?Symbol.for("react.suspense"):60113,ib=E?Symbol.for("react.suspense_list"):
60120,jb=E?Symbol.for("react.memo"):60115,kb=E?Symbol.for("react.lazy"):60116,lb=E?Symbol.for("react.block"):60121,mb="function"===typeof Symbol&&Symbol.iterator;function nb(a){if(null===a||"object"!==typeof a)return null;a=mb&&a[mb]||a["@@iterator"];return"function"===typeof a?a:null}function ob(a){if(-1===a._status){a._status=0;var b=a._ctor;b=b();a._result=b;b.then(function(b){0===a._status&&(b=b.default,a._status=1,a._result=b)},function(b){0===a._status&&(a._status=2,a._result=b)})}}
function pb(a){if(null==a)return null;if("function"===typeof a)return a.displayName||a.name||null;if("string"===typeof a)return a;switch(a){case ab:return"Fragment";case $a:return"Portal";case cb:return"Profiler";case bb:return"StrictMode";case hb:return"Suspense";case ib:return"SuspenseList"}if("object"===typeof a)switch(a.$$typeof){case eb:return"Context.Consumer";case db:return"Context.Provider";case gb:var b=a.render;b=b.displayName||b.name||"";return a.displayName||(""!==b?"ForwardRef("+b+")":
"ForwardRef");case jb:return pb(a.type);case lb:return pb(a.render);case kb:if(a=1===a._status?a._result:null)return pb(a)}return null}function qb(a){var b="";do{a:switch(a.tag){case 3:case 4:case 6:case 7:case 10:case 9:var c="";break a;default:var d=a._debugOwner,e=a._debugSource,f=pb(a.type);c=null;d&&(c=pb(d.type));d=f;f="";e?f=" (at "+e.fileName.replace(Ya,"")+":"+e.lineNumber+")":c&&(f=" (created by "+c+")");c="\n    in "+(d||"Unknown")+f}b+=c;a=a.return}while(a);return b}
function rb(a){switch(typeof a){case "boolean":case "number":case "object":case "string":case "undefined":return a;default:return""}}function sb(a){var b=a.type;return(a=a.nodeName)&&"input"===a.toLowerCase()&&("checkbox"===b||"radio"===b)}
function tb(a){var b=sb(a)?"checked":"value",c=Object.getOwnPropertyDescriptor(a.constructor.prototype,b),d=""+a[b];if(!a.hasOwnProperty(b)&&"undefined"!==typeof c&&"function"===typeof c.get&&"function"===typeof c.set){var e=c.get,f=c.set;Object.defineProperty(a,b,{configurable:!0,get:function(){return e.call(this)},set:function(a){d=""+a;f.call(this,a)}});Object.defineProperty(a,b,{enumerable:c.enumerable});return{getValue:function(){return d},setValue:function(a){d=""+a},stopTracking:function(){a._valueTracker=
null;delete a[b]}}}}function xb(a){a._valueTracker||(a._valueTracker=tb(a))}function yb(a){if(!a)return!1;var b=a._valueTracker;if(!b)return!0;var c=b.getValue();var d="";a&&(d=sb(a)?a.checked?"true":"false":a.value);a=d;return a!==c?(b.setValue(a),!0):!1}function zb(a,b){var c=b.checked;return n({},b,{defaultChecked:void 0,defaultValue:void 0,value:void 0,checked:null!=c?c:a._wrapperState.initialChecked})}
function Ab(a,b){var c=null==b.defaultValue?"":b.defaultValue,d=null!=b.checked?b.checked:b.defaultChecked;c=rb(null!=b.value?b.value:c);a._wrapperState={initialChecked:d,initialValue:c,controlled:"checkbox"===b.type||"radio"===b.type?null!=b.checked:null!=b.value}}function Bb(a,b){b=b.checked;null!=b&&Xa(a,"checked",b,!1)}
function Cb(a,b){Bb(a,b);var c=rb(b.value),d=b.type;if(null!=c)if("number"===d){if(0===c&&""===a.value||a.value!=c)a.value=""+c}else a.value!==""+c&&(a.value=""+c);else if("submit"===d||"reset"===d){a.removeAttribute("value");return}b.hasOwnProperty("value")?Db(a,b.type,c):b.hasOwnProperty("defaultValue")&&Db(a,b.type,rb(b.defaultValue));null==b.checked&&null!=b.defaultChecked&&(a.defaultChecked=!!b.defaultChecked)}
function Eb(a,b,c){if(b.hasOwnProperty("value")||b.hasOwnProperty("defaultValue")){var d=b.type;if(!("submit"!==d&&"reset"!==d||void 0!==b.value&&null!==b.value))return;b=""+a._wrapperState.initialValue;c||b===a.value||(a.value=b);a.defaultValue=b}c=a.name;""!==c&&(a.name="");a.defaultChecked=!!a._wrapperState.initialChecked;""!==c&&(a.name=c)}
function Db(a,b,c){if("number"!==b||a.ownerDocument.activeElement!==a)null==c?a.defaultValue=""+a._wrapperState.initialValue:a.defaultValue!==""+c&&(a.defaultValue=""+c)}function Fb(a){var b="";aa.Children.forEach(a,function(a){null!=a&&(b+=a)});return b}function Gb(a,b){a=n({children:void 0},b);if(b=Fb(b.children))a.children=b;return a}
function Hb(a,b,c,d){a=a.options;if(b){b={};for(var e=0;e<c.length;e++)b["$"+c[e]]=!0;for(c=0;c<a.length;c++)e=b.hasOwnProperty("$"+a[c].value),a[c].selected!==e&&(a[c].selected=e),e&&d&&(a[c].defaultSelected=!0)}else{c=""+rb(c);b=null;for(e=0;e<a.length;e++){if(a[e].value===c){a[e].selected=!0;d&&(a[e].defaultSelected=!0);return}null!==b||a[e].disabled||(b=a[e])}null!==b&&(b.selected=!0)}}
function Ib(a,b){if(null!=b.dangerouslySetInnerHTML)throw Error(u(91));return n({},b,{value:void 0,defaultValue:void 0,children:""+a._wrapperState.initialValue})}function Jb(a,b){var c=b.value;if(null==c){c=b.children;b=b.defaultValue;if(null!=c){if(null!=b)throw Error(u(92));if(Array.isArray(c)){if(!(1>=c.length))throw Error(u(93));c=c[0]}b=c}null==b&&(b="");c=b}a._wrapperState={initialValue:rb(c)}}
function Kb(a,b){var c=rb(b.value),d=rb(b.defaultValue);null!=c&&(c=""+c,c!==a.value&&(a.value=c),null==b.defaultValue&&a.defaultValue!==c&&(a.defaultValue=c));null!=d&&(a.defaultValue=""+d)}function Lb(a){var b=a.textContent;b===a._wrapperState.initialValue&&""!==b&&null!==b&&(a.value=b)}var Mb={html:"http://www.w3.org/1999/xhtml",mathml:"http://www.w3.org/1998/Math/MathML",svg:"http://www.w3.org/2000/svg"};
function Nb(a){switch(a){case "svg":return"http://www.w3.org/2000/svg";case "math":return"http://www.w3.org/1998/Math/MathML";default:return"http://www.w3.org/1999/xhtml"}}function Ob(a,b){return null==a||"http://www.w3.org/1999/xhtml"===a?Nb(b):"http://www.w3.org/2000/svg"===a&&"foreignObject"===b?"http://www.w3.org/1999/xhtml":a}
var Pb,Qb=function(a){return"undefined"!==typeof MSApp&&MSApp.execUnsafeLocalFunction?function(b,c,d,e){MSApp.execUnsafeLocalFunction(function(){return a(b,c,d,e)})}:a}(function(a,b){if(a.namespaceURI!==Mb.svg||"innerHTML"in a)a.innerHTML=b;else{Pb=Pb||document.createElement("div");Pb.innerHTML="<svg>"+b.valueOf().toString()+"</svg>";for(b=Pb.firstChild;a.firstChild;)a.removeChild(a.firstChild);for(;b.firstChild;)a.appendChild(b.firstChild)}});
function Rb(a,b){if(b){var c=a.firstChild;if(c&&c===a.lastChild&&3===c.nodeType){c.nodeValue=b;return}}a.textContent=b}function Sb(a,b){var c={};c[a.toLowerCase()]=b.toLowerCase();c["Webkit"+a]="webkit"+b;c["Moz"+a]="moz"+b;return c}var Tb={animationend:Sb("Animation","AnimationEnd"),animationiteration:Sb("Animation","AnimationIteration"),animationstart:Sb("Animation","AnimationStart"),transitionend:Sb("Transition","TransitionEnd")},Ub={},Vb={};
ya&&(Vb=document.createElement("div").style,"AnimationEvent"in window||(delete Tb.animationend.animation,delete Tb.animationiteration.animation,delete Tb.animationstart.animation),"TransitionEvent"in window||delete Tb.transitionend.transition);function Wb(a){if(Ub[a])return Ub[a];if(!Tb[a])return a;var b=Tb[a],c;for(c in b)if(b.hasOwnProperty(c)&&c in Vb)return Ub[a]=b[c];return a}
var Xb=Wb("animationend"),Yb=Wb("animationiteration"),Zb=Wb("animationstart"),$b=Wb("transitionend"),ac="abort canplay canplaythrough durationchange emptied encrypted ended error loadeddata loadedmetadata loadstart pause play playing progress ratechange seeked seeking stalled suspend timeupdate volumechange waiting".split(" "),bc=new ("function"===typeof WeakMap?WeakMap:Map);function cc(a){var b=bc.get(a);void 0===b&&(b=new Map,bc.set(a,b));return b}
function dc(a){var b=a,c=a;if(a.alternate)for(;b.return;)b=b.return;else{a=b;do b=a,0!==(b.effectTag&1026)&&(c=b.return),a=b.return;while(a)}return 3===b.tag?c:null}function ec(a){if(13===a.tag){var b=a.memoizedState;null===b&&(a=a.alternate,null!==a&&(b=a.memoizedState));if(null!==b)return b.dehydrated}return null}function fc(a){if(dc(a)!==a)throw Error(u(188));}
function gc(a){var b=a.alternate;if(!b){b=dc(a);if(null===b)throw Error(u(188));return b!==a?null:a}for(var c=a,d=b;;){var e=c.return;if(null===e)break;var f=e.alternate;if(null===f){d=e.return;if(null!==d){c=d;continue}break}if(e.child===f.child){for(f=e.child;f;){if(f===c)return fc(e),a;if(f===d)return fc(e),b;f=f.sibling}throw Error(u(188));}if(c.return!==d.return)c=e,d=f;else{for(var g=!1,h=e.child;h;){if(h===c){g=!0;c=e;d=f;break}if(h===d){g=!0;d=e;c=f;break}h=h.sibling}if(!g){for(h=f.child;h;){if(h===
c){g=!0;c=f;d=e;break}if(h===d){g=!0;d=f;c=e;break}h=h.sibling}if(!g)throw Error(u(189));}}if(c.alternate!==d)throw Error(u(190));}if(3!==c.tag)throw Error(u(188));return c.stateNode.current===c?a:b}function hc(a){a=gc(a);if(!a)return null;for(var b=a;;){if(5===b.tag||6===b.tag)return b;if(b.child)b.child.return=b,b=b.child;else{if(b===a)break;for(;!b.sibling;){if(!b.return||b.return===a)return null;b=b.return}b.sibling.return=b.return;b=b.sibling}}return null}
function ic(a,b){if(null==b)throw Error(u(30));if(null==a)return b;if(Array.isArray(a)){if(Array.isArray(b))return a.push.apply(a,b),a;a.push(b);return a}return Array.isArray(b)?[a].concat(b):[a,b]}function jc(a,b,c){Array.isArray(a)?a.forEach(b,c):a&&b.call(c,a)}var kc=null;
function lc(a){if(a){var b=a._dispatchListeners,c=a._dispatchInstances;if(Array.isArray(b))for(var d=0;d<b.length&&!a.isPropagationStopped();d++)oa(a,b[d],c[d]);else b&&oa(a,b,c);a._dispatchListeners=null;a._dispatchInstances=null;a.isPersistent()||a.constructor.release(a)}}function mc(a){null!==a&&(kc=ic(kc,a));a=kc;kc=null;if(a){jc(a,lc);if(kc)throw Error(u(95));if(fa)throw a=ha,fa=!1,ha=null,a;}}
function nc(a){a=a.target||a.srcElement||window;a.correspondingUseElement&&(a=a.correspondingUseElement);return 3===a.nodeType?a.parentNode:a}function oc(a){if(!ya)return!1;a="on"+a;var b=a in document;b||(b=document.createElement("div"),b.setAttribute(a,"return;"),b="function"===typeof b[a]);return b}var pc=[];function qc(a){a.topLevelType=null;a.nativeEvent=null;a.targetInst=null;a.ancestors.length=0;10>pc.length&&pc.push(a)}
function rc(a,b,c,d){if(pc.length){var e=pc.pop();e.topLevelType=a;e.eventSystemFlags=d;e.nativeEvent=b;e.targetInst=c;return e}return{topLevelType:a,eventSystemFlags:d,nativeEvent:b,targetInst:c,ancestors:[]}}
function sc(a){var b=a.targetInst,c=b;do{if(!c){a.ancestors.push(c);break}var d=c;if(3===d.tag)d=d.stateNode.containerInfo;else{for(;d.return;)d=d.return;d=3!==d.tag?null:d.stateNode.containerInfo}if(!d)break;b=c.tag;5!==b&&6!==b||a.ancestors.push(c);c=tc(d)}while(c);for(c=0;c<a.ancestors.length;c++){b=a.ancestors[c];var e=nc(a.nativeEvent);d=a.topLevelType;var f=a.nativeEvent,g=a.eventSystemFlags;0===c&&(g|=64);for(var h=null,k=0;k<sa.length;k++){var l=sa[k];l&&(l=l.extractEvents(d,b,f,e,g))&&(h=
ic(h,l))}mc(h)}}function uc(a,b,c){if(!c.has(a)){switch(a){case "scroll":vc(b,"scroll",!0);break;case "focus":case "blur":vc(b,"focus",!0);vc(b,"blur",!0);c.set("blur",null);c.set("focus",null);break;case "cancel":case "close":oc(a)&&vc(b,a,!0);break;case "invalid":case "submit":case "reset":break;default:-1===ac.indexOf(a)&&F(a,b)}c.set(a,null)}}
var wc,xc,yc,zc=!1,Ac=[],Bc=null,Cc=null,Dc=null,Ec=new Map,Fc=new Map,Gc=[],Hc="mousedown mouseup touchcancel touchend touchstart auxclick dblclick pointercancel pointerdown pointerup dragend dragstart drop compositionend compositionstart keydown keypress keyup input textInput close cancel copy cut paste click change contextmenu reset submit".split(" "),Ic="focus blur dragenter dragleave mouseover mouseout pointerover pointerout gotpointercapture lostpointercapture".split(" ");
function Jc(a,b){var c=cc(b);Hc.forEach(function(a){uc(a,b,c)});Ic.forEach(function(a){uc(a,b,c)})}function Kc(a,b,c,d,e){return{blockedOn:a,topLevelType:b,eventSystemFlags:c|32,nativeEvent:e,container:d}}
function Lc(a,b){switch(a){case "focus":case "blur":Bc=null;break;case "dragenter":case "dragleave":Cc=null;break;case "mouseover":case "mouseout":Dc=null;break;case "pointerover":case "pointerout":Ec.delete(b.pointerId);break;case "gotpointercapture":case "lostpointercapture":Fc.delete(b.pointerId)}}function Mc(a,b,c,d,e,f){if(null===a||a.nativeEvent!==f)return a=Kc(b,c,d,e,f),null!==b&&(b=Nc(b),null!==b&&xc(b)),a;a.eventSystemFlags|=d;return a}
function Oc(a,b,c,d,e){switch(b){case "focus":return Bc=Mc(Bc,a,b,c,d,e),!0;case "dragenter":return Cc=Mc(Cc,a,b,c,d,e),!0;case "mouseover":return Dc=Mc(Dc,a,b,c,d,e),!0;case "pointerover":var f=e.pointerId;Ec.set(f,Mc(Ec.get(f)||null,a,b,c,d,e));return!0;case "gotpointercapture":return f=e.pointerId,Fc.set(f,Mc(Fc.get(f)||null,a,b,c,d,e)),!0}return!1}
function Pc(a){var b=tc(a.target);if(null!==b){var c=dc(b);if(null!==c)if(b=c.tag,13===b){if(b=ec(c),null!==b){a.blockedOn=b;r.unstable_runWithPriority(a.priority,function(){yc(c)});return}}else if(3===b&&c.stateNode.hydrate){a.blockedOn=3===c.tag?c.stateNode.containerInfo:null;return}}a.blockedOn=null}function Qc(a){if(null!==a.blockedOn)return!1;var b=Rc(a.topLevelType,a.eventSystemFlags,a.container,a.nativeEvent);if(null!==b){var c=Nc(b);null!==c&&xc(c);a.blockedOn=b;return!1}return!0}
function Sc(a,b,c){Qc(a)&&c.delete(b)}function Tc(){for(zc=!1;0<Ac.length;){var a=Ac[0];if(null!==a.blockedOn){a=Nc(a.blockedOn);null!==a&&wc(a);break}var b=Rc(a.topLevelType,a.eventSystemFlags,a.container,a.nativeEvent);null!==b?a.blockedOn=b:Ac.shift()}null!==Bc&&Qc(Bc)&&(Bc=null);null!==Cc&&Qc(Cc)&&(Cc=null);null!==Dc&&Qc(Dc)&&(Dc=null);Ec.forEach(Sc);Fc.forEach(Sc)}function Uc(a,b){a.blockedOn===b&&(a.blockedOn=null,zc||(zc=!0,r.unstable_scheduleCallback(r.unstable_NormalPriority,Tc)))}
function Vc(a){function b(b){return Uc(b,a)}if(0<Ac.length){Uc(Ac[0],a);for(var c=1;c<Ac.length;c++){var d=Ac[c];d.blockedOn===a&&(d.blockedOn=null)}}null!==Bc&&Uc(Bc,a);null!==Cc&&Uc(Cc,a);null!==Dc&&Uc(Dc,a);Ec.forEach(b);Fc.forEach(b);for(c=0;c<Gc.length;c++)d=Gc[c],d.blockedOn===a&&(d.blockedOn=null);for(;0<Gc.length&&(c=Gc[0],null===c.blockedOn);)Pc(c),null===c.blockedOn&&Gc.shift()}
var Wc={},Yc=new Map,Zc=new Map,$c=["abort","abort",Xb,"animationEnd",Yb,"animationIteration",Zb,"animationStart","canplay","canPlay","canplaythrough","canPlayThrough","durationchange","durationChange","emptied","emptied","encrypted","encrypted","ended","ended","error","error","gotpointercapture","gotPointerCapture","load","load","loadeddata","loadedData","loadedmetadata","loadedMetadata","loadstart","loadStart","lostpointercapture","lostPointerCapture","playing","playing","progress","progress","seeking",
"seeking","stalled","stalled","suspend","suspend","timeupdate","timeUpdate",$b,"transitionEnd","waiting","waiting"];function ad(a,b){for(var c=0;c<a.length;c+=2){var d=a[c],e=a[c+1],f="on"+(e[0].toUpperCase()+e.slice(1));f={phasedRegistrationNames:{bubbled:f,captured:f+"Capture"},dependencies:[d],eventPriority:b};Zc.set(d,b);Yc.set(d,f);Wc[e]=f}}
ad("blur blur cancel cancel click click close close contextmenu contextMenu copy copy cut cut auxclick auxClick dblclick doubleClick dragend dragEnd dragstart dragStart drop drop focus focus input input invalid invalid keydown keyDown keypress keyPress keyup keyUp mousedown mouseDown mouseup mouseUp paste paste pause pause play play pointercancel pointerCancel pointerdown pointerDown pointerup pointerUp ratechange rateChange reset reset seeked seeked submit submit touchcancel touchCancel touchend touchEnd touchstart touchStart volumechange volumeChange".split(" "),0);
ad("drag drag dragenter dragEnter dragexit dragExit dragleave dragLeave dragover dragOver mousemove mouseMove mouseout mouseOut mouseover mouseOver pointermove pointerMove pointerout pointerOut pointerover pointerOver scroll scroll toggle toggle touchmove touchMove wheel wheel".split(" "),1);ad($c,2);for(var bd="change selectionchange textInput compositionstart compositionend compositionupdate".split(" "),cd=0;cd<bd.length;cd++)Zc.set(bd[cd],0);
var dd=r.unstable_UserBlockingPriority,ed=r.unstable_runWithPriority,fd=!0;function F(a,b){vc(b,a,!1)}function vc(a,b,c){var d=Zc.get(b);switch(void 0===d?2:d){case 0:d=gd.bind(null,b,1,a);break;case 1:d=hd.bind(null,b,1,a);break;default:d=id.bind(null,b,1,a)}c?a.addEventListener(b,d,!0):a.addEventListener(b,d,!1)}function gd(a,b,c,d){Ja||Ha();var e=id,f=Ja;Ja=!0;try{Ga(e,a,b,c,d)}finally{(Ja=f)||La()}}function hd(a,b,c,d){ed(dd,id.bind(null,a,b,c,d))}
function id(a,b,c,d){if(fd)if(0<Ac.length&&-1<Hc.indexOf(a))a=Kc(null,a,b,c,d),Ac.push(a);else{var e=Rc(a,b,c,d);if(null===e)Lc(a,d);else if(-1<Hc.indexOf(a))a=Kc(e,a,b,c,d),Ac.push(a);else if(!Oc(e,a,b,c,d)){Lc(a,d);a=rc(a,d,null,b);try{Ma(sc,a)}finally{qc(a)}}}}
function Rc(a,b,c,d){c=nc(d);c=tc(c);if(null!==c){var e=dc(c);if(null===e)c=null;else{var f=e.tag;if(13===f){c=ec(e);if(null!==c)return c;c=null}else if(3===f){if(e.stateNode.hydrate)return 3===e.tag?e.stateNode.containerInfo:null;c=null}else e!==c&&(c=null)}}a=rc(a,d,c,b);try{Ma(sc,a)}finally{qc(a)}return null}
var jd={animationIterationCount:!0,borderImageOutset:!0,borderImageSlice:!0,borderImageWidth:!0,boxFlex:!0,boxFlexGroup:!0,boxOrdinalGroup:!0,columnCount:!0,columns:!0,flex:!0,flexGrow:!0,flexPositive:!0,flexShrink:!0,flexNegative:!0,flexOrder:!0,gridArea:!0,gridRow:!0,gridRowEnd:!0,gridRowSpan:!0,gridRowStart:!0,gridColumn:!0,gridColumnEnd:!0,gridColumnSpan:!0,gridColumnStart:!0,fontWeight:!0,lineClamp:!0,lineHeight:!0,opacity:!0,order:!0,orphans:!0,tabSize:!0,widows:!0,zIndex:!0,zoom:!0,fillOpacity:!0,
floodOpacity:!0,stopOpacity:!0,strokeDasharray:!0,strokeDashoffset:!0,strokeMiterlimit:!0,strokeOpacity:!0,strokeWidth:!0},kd=["Webkit","ms","Moz","O"];Object.keys(jd).forEach(function(a){kd.forEach(function(b){b=b+a.charAt(0).toUpperCase()+a.substring(1);jd[b]=jd[a]})});function ld(a,b,c){return null==b||"boolean"===typeof b||""===b?"":c||"number"!==typeof b||0===b||jd.hasOwnProperty(a)&&jd[a]?(""+b).trim():b+"px"}
function md(a,b){a=a.style;for(var c in b)if(b.hasOwnProperty(c)){var d=0===c.indexOf("--"),e=ld(c,b[c],d);"float"===c&&(c="cssFloat");d?a.setProperty(c,e):a[c]=e}}var nd=n({menuitem:!0},{area:!0,base:!0,br:!0,col:!0,embed:!0,hr:!0,img:!0,input:!0,keygen:!0,link:!0,meta:!0,param:!0,source:!0,track:!0,wbr:!0});
function od(a,b){if(b){if(nd[a]&&(null!=b.children||null!=b.dangerouslySetInnerHTML))throw Error(u(137,a,""));if(null!=b.dangerouslySetInnerHTML){if(null!=b.children)throw Error(u(60));if(!("object"===typeof b.dangerouslySetInnerHTML&&"__html"in b.dangerouslySetInnerHTML))throw Error(u(61));}if(null!=b.style&&"object"!==typeof b.style)throw Error(u(62,""));}}
function pd(a,b){if(-1===a.indexOf("-"))return"string"===typeof b.is;switch(a){case "annotation-xml":case "color-profile":case "font-face":case "font-face-src":case "font-face-uri":case "font-face-format":case "font-face-name":case "missing-glyph":return!1;default:return!0}}var qd=Mb.html;function rd(a,b){a=9===a.nodeType||11===a.nodeType?a:a.ownerDocument;var c=cc(a);b=wa[b];for(var d=0;d<b.length;d++)uc(b[d],a,c)}function sd(){}
function td(a){a=a||("undefined"!==typeof document?document:void 0);if("undefined"===typeof a)return null;try{return a.activeElement||a.body}catch(b){return a.body}}function ud(a){for(;a&&a.firstChild;)a=a.firstChild;return a}function vd(a,b){var c=ud(a);a=0;for(var d;c;){if(3===c.nodeType){d=a+c.textContent.length;if(a<=b&&d>=b)return{node:c,offset:b-a};a=d}a:{for(;c;){if(c.nextSibling){c=c.nextSibling;break a}c=c.parentNode}c=void 0}c=ud(c)}}
function wd(a,b){return a&&b?a===b?!0:a&&3===a.nodeType?!1:b&&3===b.nodeType?wd(a,b.parentNode):"contains"in a?a.contains(b):a.compareDocumentPosition?!!(a.compareDocumentPosition(b)&16):!1:!1}function xd(){for(var a=window,b=td();b instanceof a.HTMLIFrameElement;){try{var c="string"===typeof b.contentWindow.location.href}catch(d){c=!1}if(c)a=b.contentWindow;else break;b=td(a.document)}return b}
function yd(a){var b=a&&a.nodeName&&a.nodeName.toLowerCase();return b&&("input"===b&&("text"===a.type||"search"===a.type||"tel"===a.type||"url"===a.type||"password"===a.type)||"textarea"===b||"true"===a.contentEditable)}var zd="$",Ad="/$",Bd="$?",Cd="$!",Dd=null,Ed=null;function Fd(a,b){switch(a){case "button":case "input":case "select":case "textarea":return!!b.autoFocus}return!1}
function Gd(a,b){return"textarea"===a||"option"===a||"noscript"===a||"string"===typeof b.children||"number"===typeof b.children||"object"===typeof b.dangerouslySetInnerHTML&&null!==b.dangerouslySetInnerHTML&&null!=b.dangerouslySetInnerHTML.__html}var Hd="function"===typeof setTimeout?setTimeout:void 0,Id="function"===typeof clearTimeout?clearTimeout:void 0;function Jd(a){for(;null!=a;a=a.nextSibling){var b=a.nodeType;if(1===b||3===b)break}return a}
function Kd(a){a=a.previousSibling;for(var b=0;a;){if(8===a.nodeType){var c=a.data;if(c===zd||c===Cd||c===Bd){if(0===b)return a;b--}else c===Ad&&b++}a=a.previousSibling}return null}var Ld=Math.random().toString(36).slice(2),Md="__reactInternalInstance$"+Ld,Nd="__reactEventHandlers$"+Ld,Od="__reactContainere$"+Ld;
function tc(a){var b=a[Md];if(b)return b;for(var c=a.parentNode;c;){if(b=c[Od]||c[Md]){c=b.alternate;if(null!==b.child||null!==c&&null!==c.child)for(a=Kd(a);null!==a;){if(c=a[Md])return c;a=Kd(a)}return b}a=c;c=a.parentNode}return null}function Nc(a){a=a[Md]||a[Od];return!a||5!==a.tag&&6!==a.tag&&13!==a.tag&&3!==a.tag?null:a}function Pd(a){if(5===a.tag||6===a.tag)return a.stateNode;throw Error(u(33));}function Qd(a){return a[Nd]||null}
function Rd(a){do a=a.return;while(a&&5!==a.tag);return a?a:null}
function Sd(a,b){var c=a.stateNode;if(!c)return null;var d=la(c);if(!d)return null;c=d[b];a:switch(b){case "onClick":case "onClickCapture":case "onDoubleClick":case "onDoubleClickCapture":case "onMouseDown":case "onMouseDownCapture":case "onMouseMove":case "onMouseMoveCapture":case "onMouseUp":case "onMouseUpCapture":case "onMouseEnter":(d=!d.disabled)||(a=a.type,d=!("button"===a||"input"===a||"select"===a||"textarea"===a));a=!d;break a;default:a=!1}if(a)return null;if(c&&"function"!==typeof c)throw Error(u(231,
b,typeof c));return c}function Td(a,b,c){if(b=Sd(a,c.dispatchConfig.phasedRegistrationNames[b]))c._dispatchListeners=ic(c._dispatchListeners,b),c._dispatchInstances=ic(c._dispatchInstances,a)}function Ud(a){if(a&&a.dispatchConfig.phasedRegistrationNames){for(var b=a._targetInst,c=[];b;)c.push(b),b=Rd(b);for(b=c.length;0<b--;)Td(c[b],"captured",a);for(b=0;b<c.length;b++)Td(c[b],"bubbled",a)}}
function Vd(a,b,c){a&&c&&c.dispatchConfig.registrationName&&(b=Sd(a,c.dispatchConfig.registrationName))&&(c._dispatchListeners=ic(c._dispatchListeners,b),c._dispatchInstances=ic(c._dispatchInstances,a))}function Wd(a){a&&a.dispatchConfig.registrationName&&Vd(a._targetInst,null,a)}function Xd(a){jc(a,Ud)}var Yd=null,Zd=null,$d=null;
function ae(){if($d)return $d;var a,b=Zd,c=b.length,d,e="value"in Yd?Yd.value:Yd.textContent,f=e.length;for(a=0;a<c&&b[a]===e[a];a++);var g=c-a;for(d=1;d<=g&&b[c-d]===e[f-d];d++);return $d=e.slice(a,1<d?1-d:void 0)}function be(){return!0}function ce(){return!1}
function G(a,b,c,d){this.dispatchConfig=a;this._targetInst=b;this.nativeEvent=c;a=this.constructor.Interface;for(var e in a)a.hasOwnProperty(e)&&((b=a[e])?this[e]=b(c):"target"===e?this.target=d:this[e]=c[e]);this.isDefaultPrevented=(null!=c.defaultPrevented?c.defaultPrevented:!1===c.returnValue)?be:ce;this.isPropagationStopped=ce;return this}
n(G.prototype,{preventDefault:function(){this.defaultPrevented=!0;var a=this.nativeEvent;a&&(a.preventDefault?a.preventDefault():"unknown"!==typeof a.returnValue&&(a.returnValue=!1),this.isDefaultPrevented=be)},stopPropagation:function(){var a=this.nativeEvent;a&&(a.stopPropagation?a.stopPropagation():"unknown"!==typeof a.cancelBubble&&(a.cancelBubble=!0),this.isPropagationStopped=be)},persist:function(){this.isPersistent=be},isPersistent:ce,destructor:function(){var a=this.constructor.Interface,
b;for(b in a)this[b]=null;this.nativeEvent=this._targetInst=this.dispatchConfig=null;this.isPropagationStopped=this.isDefaultPrevented=ce;this._dispatchInstances=this._dispatchListeners=null}});G.Interface={type:null,target:null,currentTarget:function(){return null},eventPhase:null,bubbles:null,cancelable:null,timeStamp:function(a){return a.timeStamp||Date.now()},defaultPrevented:null,isTrusted:null};
G.extend=function(a){function b(){}function c(){return d.apply(this,arguments)}var d=this;b.prototype=d.prototype;var e=new b;n(e,c.prototype);c.prototype=e;c.prototype.constructor=c;c.Interface=n({},d.Interface,a);c.extend=d.extend;de(c);return c};de(G);function ee(a,b,c,d){if(this.eventPool.length){var e=this.eventPool.pop();this.call(e,a,b,c,d);return e}return new this(a,b,c,d)}
function fe(a){if(!(a instanceof this))throw Error(u(279));a.destructor();10>this.eventPool.length&&this.eventPool.push(a)}function de(a){a.eventPool=[];a.getPooled=ee;a.release=fe}var ge=G.extend({data:null}),he=G.extend({data:null}),ie=[9,13,27,32],je=ya&&"CompositionEvent"in window,ke=null;ya&&"documentMode"in document&&(ke=document.documentMode);
var le=ya&&"TextEvent"in window&&!ke,me=ya&&(!je||ke&&8<ke&&11>=ke),ne=String.fromCharCode(32),oe={beforeInput:{phasedRegistrationNames:{bubbled:"onBeforeInput",captured:"onBeforeInputCapture"},dependencies:["compositionend","keypress","textInput","paste"]},compositionEnd:{phasedRegistrationNames:{bubbled:"onCompositionEnd",captured:"onCompositionEndCapture"},dependencies:"blur compositionend keydown keypress keyup mousedown".split(" ")},compositionStart:{phasedRegistrationNames:{bubbled:"onCompositionStart",
captured:"onCompositionStartCapture"},dependencies:"blur compositionstart keydown keypress keyup mousedown".split(" ")},compositionUpdate:{phasedRegistrationNames:{bubbled:"onCompositionUpdate",captured:"onCompositionUpdateCapture"},dependencies:"blur compositionupdate keydown keypress keyup mousedown".split(" ")}},pe=!1;
function qe(a,b){switch(a){case "keyup":return-1!==ie.indexOf(b.keyCode);case "keydown":return 229!==b.keyCode;case "keypress":case "mousedown":case "blur":return!0;default:return!1}}function re(a){a=a.detail;return"object"===typeof a&&"data"in a?a.data:null}var se=!1;function te(a,b){switch(a){case "compositionend":return re(b);case "keypress":if(32!==b.which)return null;pe=!0;return ne;case "textInput":return a=b.data,a===ne&&pe?null:a;default:return null}}
function ue(a,b){if(se)return"compositionend"===a||!je&&qe(a,b)?(a=ae(),$d=Zd=Yd=null,se=!1,a):null;switch(a){case "paste":return null;case "keypress":if(!(b.ctrlKey||b.altKey||b.metaKey)||b.ctrlKey&&b.altKey){if(b.char&&1<b.char.length)return b.char;if(b.which)return String.fromCharCode(b.which)}return null;case "compositionend":return me&&"ko"!==b.locale?null:b.data;default:return null}}
var ve={eventTypes:oe,extractEvents:function(a,b,c,d){var e;if(je)b:{switch(a){case "compositionstart":var f=oe.compositionStart;break b;case "compositionend":f=oe.compositionEnd;break b;case "compositionupdate":f=oe.compositionUpdate;break b}f=void 0}else se?qe(a,c)&&(f=oe.compositionEnd):"keydown"===a&&229===c.keyCode&&(f=oe.compositionStart);f?(me&&"ko"!==c.locale&&(se||f!==oe.compositionStart?f===oe.compositionEnd&&se&&(e=ae()):(Yd=d,Zd="value"in Yd?Yd.value:Yd.textContent,se=!0)),f=ge.getPooled(f,
b,c,d),e?f.data=e:(e=re(c),null!==e&&(f.data=e)),Xd(f),e=f):e=null;(a=le?te(a,c):ue(a,c))?(b=he.getPooled(oe.beforeInput,b,c,d),b.data=a,Xd(b)):b=null;return null===e?b:null===b?e:[e,b]}},we={color:!0,date:!0,datetime:!0,"datetime-local":!0,email:!0,month:!0,number:!0,password:!0,range:!0,search:!0,tel:!0,text:!0,time:!0,url:!0,week:!0};function xe(a){var b=a&&a.nodeName&&a.nodeName.toLowerCase();return"input"===b?!!we[a.type]:"textarea"===b?!0:!1}
var ye={change:{phasedRegistrationNames:{bubbled:"onChange",captured:"onChangeCapture"},dependencies:"blur change click focus input keydown keyup selectionchange".split(" ")}};function ze(a,b,c){a=G.getPooled(ye.change,a,b,c);a.type="change";Da(c);Xd(a);return a}var Ae=null,Be=null;function Ce(a){mc(a)}function De(a){var b=Pd(a);if(yb(b))return a}function Ee(a,b){if("change"===a)return b}var Fe=!1;ya&&(Fe=oc("input")&&(!document.documentMode||9<document.documentMode));
function Ge(){Ae&&(Ae.detachEvent("onpropertychange",He),Be=Ae=null)}function He(a){if("value"===a.propertyName&&De(Be))if(a=ze(Be,a,nc(a)),Ja)mc(a);else{Ja=!0;try{Fa(Ce,a)}finally{Ja=!1,La()}}}function Ie(a,b,c){"focus"===a?(Ge(),Ae=b,Be=c,Ae.attachEvent("onpropertychange",He)):"blur"===a&&Ge()}function Je(a){if("selectionchange"===a||"keyup"===a||"keydown"===a)return De(Be)}function Ke(a,b){if("click"===a)return De(b)}function Le(a,b){if("input"===a||"change"===a)return De(b)}
var Me={eventTypes:ye,_isInputEventSupported:Fe,extractEvents:function(a,b,c,d){var e=b?Pd(b):window,f=e.nodeName&&e.nodeName.toLowerCase();if("select"===f||"input"===f&&"file"===e.type)var g=Ee;else if(xe(e))if(Fe)g=Le;else{g=Je;var h=Ie}else(f=e.nodeName)&&"input"===f.toLowerCase()&&("checkbox"===e.type||"radio"===e.type)&&(g=Ke);if(g&&(g=g(a,b)))return ze(g,c,d);h&&h(a,e,b);"blur"===a&&(a=e._wrapperState)&&a.controlled&&"number"===e.type&&Db(e,"number",e.value)}},Ne=G.extend({view:null,detail:null}),
Oe={Alt:"altKey",Control:"ctrlKey",Meta:"metaKey",Shift:"shiftKey"};function Pe(a){var b=this.nativeEvent;return b.getModifierState?b.getModifierState(a):(a=Oe[a])?!!b[a]:!1}function Qe(){return Pe}
var Re=0,Se=0,Te=!1,Ue=!1,Ve=Ne.extend({screenX:null,screenY:null,clientX:null,clientY:null,pageX:null,pageY:null,ctrlKey:null,shiftKey:null,altKey:null,metaKey:null,getModifierState:Qe,button:null,buttons:null,relatedTarget:function(a){return a.relatedTarget||(a.fromElement===a.srcElement?a.toElement:a.fromElement)},movementX:function(a){if("movementX"in a)return a.movementX;var b=Re;Re=a.screenX;return Te?"mousemove"===a.type?a.screenX-b:0:(Te=!0,0)},movementY:function(a){if("movementY"in a)return a.movementY;
var b=Se;Se=a.screenY;return Ue?"mousemove"===a.type?a.screenY-b:0:(Ue=!0,0)}}),We=Ve.extend({pointerId:null,width:null,height:null,pressure:null,tangentialPressure:null,tiltX:null,tiltY:null,twist:null,pointerType:null,isPrimary:null}),Xe={mouseEnter:{registrationName:"onMouseEnter",dependencies:["mouseout","mouseover"]},mouseLeave:{registrationName:"onMouseLeave",dependencies:["mouseout","mouseover"]},pointerEnter:{registrationName:"onPointerEnter",dependencies:["pointerout","pointerover"]},pointerLeave:{registrationName:"onPointerLeave",
dependencies:["pointerout","pointerover"]}},Ye={eventTypes:Xe,extractEvents:function(a,b,c,d,e){var f="mouseover"===a||"pointerover"===a,g="mouseout"===a||"pointerout"===a;if(f&&0===(e&32)&&(c.relatedTarget||c.fromElement)||!g&&!f)return null;f=d.window===d?d:(f=d.ownerDocument)?f.defaultView||f.parentWindow:window;if(g){if(g=b,b=(b=c.relatedTarget||c.toElement)?tc(b):null,null!==b){var h=dc(b);if(b!==h||5!==b.tag&&6!==b.tag)b=null}}else g=null;if(g===b)return null;if("mouseout"===a||"mouseover"===
a){var k=Ve;var l=Xe.mouseLeave;var m=Xe.mouseEnter;var p="mouse"}else if("pointerout"===a||"pointerover"===a)k=We,l=Xe.pointerLeave,m=Xe.pointerEnter,p="pointer";a=null==g?f:Pd(g);f=null==b?f:Pd(b);l=k.getPooled(l,g,c,d);l.type=p+"leave";l.target=a;l.relatedTarget=f;c=k.getPooled(m,b,c,d);c.type=p+"enter";c.target=f;c.relatedTarget=a;d=g;p=b;if(d&&p)a:{k=d;m=p;g=0;for(a=k;a;a=Rd(a))g++;a=0;for(b=m;b;b=Rd(b))a++;for(;0<g-a;)k=Rd(k),g--;for(;0<a-g;)m=Rd(m),a--;for(;g--;){if(k===m||k===m.alternate)break a;
k=Rd(k);m=Rd(m)}k=null}else k=null;m=k;for(k=[];d&&d!==m;){g=d.alternate;if(null!==g&&g===m)break;k.push(d);d=Rd(d)}for(d=[];p&&p!==m;){g=p.alternate;if(null!==g&&g===m)break;d.push(p);p=Rd(p)}for(p=0;p<k.length;p++)Vd(k[p],"bubbled",l);for(p=d.length;0<p--;)Vd(d[p],"captured",c);return 0===(e&64)?[l]:[l,c]}};function Ze(a,b){return a===b&&(0!==a||1/a===1/b)||a!==a&&b!==b}var $e="function"===typeof Object.is?Object.is:Ze,af=Object.prototype.hasOwnProperty;
function bf(a,b){if($e(a,b))return!0;if("object"!==typeof a||null===a||"object"!==typeof b||null===b)return!1;var c=Object.keys(a),d=Object.keys(b);if(c.length!==d.length)return!1;for(d=0;d<c.length;d++)if(!af.call(b,c[d])||!$e(a[c[d]],b[c[d]]))return!1;return!0}
var cf=ya&&"documentMode"in document&&11>=document.documentMode,df={select:{phasedRegistrationNames:{bubbled:"onSelect",captured:"onSelectCapture"},dependencies:"blur contextmenu dragend focus keydown keyup mousedown mouseup selectionchange".split(" ")}},ef=null,ff=null,gf=null,hf=!1;
function jf(a,b){var c=b.window===b?b.document:9===b.nodeType?b:b.ownerDocument;if(hf||null==ef||ef!==td(c))return null;c=ef;"selectionStart"in c&&yd(c)?c={start:c.selectionStart,end:c.selectionEnd}:(c=(c.ownerDocument&&c.ownerDocument.defaultView||window).getSelection(),c={anchorNode:c.anchorNode,anchorOffset:c.anchorOffset,focusNode:c.focusNode,focusOffset:c.focusOffset});return gf&&bf(gf,c)?null:(gf=c,a=G.getPooled(df.select,ff,a,b),a.type="select",a.target=ef,Xd(a),a)}
var kf={eventTypes:df,extractEvents:function(a,b,c,d,e,f){e=f||(d.window===d?d.document:9===d.nodeType?d:d.ownerDocument);if(!(f=!e)){a:{e=cc(e);f=wa.onSelect;for(var g=0;g<f.length;g++)if(!e.has(f[g])){e=!1;break a}e=!0}f=!e}if(f)return null;e=b?Pd(b):window;switch(a){case "focus":if(xe(e)||"true"===e.contentEditable)ef=e,ff=b,gf=null;break;case "blur":gf=ff=ef=null;break;case "mousedown":hf=!0;break;case "contextmenu":case "mouseup":case "dragend":return hf=!1,jf(c,d);case "selectionchange":if(cf)break;
case "keydown":case "keyup":return jf(c,d)}return null}},lf=G.extend({animationName:null,elapsedTime:null,pseudoElement:null}),mf=G.extend({clipboardData:function(a){return"clipboardData"in a?a.clipboardData:window.clipboardData}}),nf=Ne.extend({relatedTarget:null});function of(a){var b=a.keyCode;"charCode"in a?(a=a.charCode,0===a&&13===b&&(a=13)):a=b;10===a&&(a=13);return 32<=a||13===a?a:0}
var pf={Esc:"Escape",Spacebar:" ",Left:"ArrowLeft",Up:"ArrowUp",Right:"ArrowRight",Down:"ArrowDown",Del:"Delete",Win:"OS",Menu:"ContextMenu",Apps:"ContextMenu",Scroll:"ScrollLock",MozPrintableKey:"Unidentified"},qf={8:"Backspace",9:"Tab",12:"Clear",13:"Enter",16:"Shift",17:"Control",18:"Alt",19:"Pause",20:"CapsLock",27:"Escape",32:" ",33:"PageUp",34:"PageDown",35:"End",36:"Home",37:"ArrowLeft",38:"ArrowUp",39:"ArrowRight",40:"ArrowDown",45:"Insert",46:"Delete",112:"F1",113:"F2",114:"F3",115:"F4",
116:"F5",117:"F6",118:"F7",119:"F8",120:"F9",121:"F10",122:"F11",123:"F12",144:"NumLock",145:"ScrollLock",224:"Meta"},rf=Ne.extend({key:function(a){if(a.key){var b=pf[a.key]||a.key;if("Unidentified"!==b)return b}return"keypress"===a.type?(a=of(a),13===a?"Enter":String.fromCharCode(a)):"keydown"===a.type||"keyup"===a.type?qf[a.keyCode]||"Unidentified":""},location:null,ctrlKey:null,shiftKey:null,altKey:null,metaKey:null,repeat:null,locale:null,getModifierState:Qe,charCode:function(a){return"keypress"===
a.type?of(a):0},keyCode:function(a){return"keydown"===a.type||"keyup"===a.type?a.keyCode:0},which:function(a){return"keypress"===a.type?of(a):"keydown"===a.type||"keyup"===a.type?a.keyCode:0}}),sf=Ve.extend({dataTransfer:null}),tf=Ne.extend({touches:null,targetTouches:null,changedTouches:null,altKey:null,metaKey:null,ctrlKey:null,shiftKey:null,getModifierState:Qe}),uf=G.extend({propertyName:null,elapsedTime:null,pseudoElement:null}),vf=Ve.extend({deltaX:function(a){return"deltaX"in a?a.deltaX:"wheelDeltaX"in
a?-a.wheelDeltaX:0},deltaY:function(a){return"deltaY"in a?a.deltaY:"wheelDeltaY"in a?-a.wheelDeltaY:"wheelDelta"in a?-a.wheelDelta:0},deltaZ:null,deltaMode:null}),wf={eventTypes:Wc,extractEvents:function(a,b,c,d){var e=Yc.get(a);if(!e)return null;switch(a){case "keypress":if(0===of(c))return null;case "keydown":case "keyup":a=rf;break;case "blur":case "focus":a=nf;break;case "click":if(2===c.button)return null;case "auxclick":case "dblclick":case "mousedown":case "mousemove":case "mouseup":case "mouseout":case "mouseover":case "contextmenu":a=
Ve;break;case "drag":case "dragend":case "dragenter":case "dragexit":case "dragleave":case "dragover":case "dragstart":case "drop":a=sf;break;case "touchcancel":case "touchend":case "touchmove":case "touchstart":a=tf;break;case Xb:case Yb:case Zb:a=lf;break;case $b:a=uf;break;case "scroll":a=Ne;break;case "wheel":a=vf;break;case "copy":case "cut":case "paste":a=mf;break;case "gotpointercapture":case "lostpointercapture":case "pointercancel":case "pointerdown":case "pointermove":case "pointerout":case "pointerover":case "pointerup":a=
We;break;default:a=G}b=a.getPooled(e,b,c,d);Xd(b);return b}};if(pa)throw Error(u(101));pa=Array.prototype.slice.call("ResponderEventPlugin SimpleEventPlugin EnterLeaveEventPlugin ChangeEventPlugin SelectEventPlugin BeforeInputEventPlugin".split(" "));ra();var xf=Nc;la=Qd;ma=xf;na=Pd;xa({SimpleEventPlugin:wf,EnterLeaveEventPlugin:Ye,ChangeEventPlugin:Me,SelectEventPlugin:kf,BeforeInputEventPlugin:ve});var yf=[],zf=-1;function H(a){0>zf||(a.current=yf[zf],yf[zf]=null,zf--)}
function I(a,b){zf++;yf[zf]=a.current;a.current=b}var Af={},J={current:Af},K={current:!1},Bf=Af;function Cf(a,b){var c=a.type.contextTypes;if(!c)return Af;var d=a.stateNode;if(d&&d.__reactInternalMemoizedUnmaskedChildContext===b)return d.__reactInternalMemoizedMaskedChildContext;var e={},f;for(f in c)e[f]=b[f];d&&(a=a.stateNode,a.__reactInternalMemoizedUnmaskedChildContext=b,a.__reactInternalMemoizedMaskedChildContext=e);return e}function L(a){a=a.childContextTypes;return null!==a&&void 0!==a}
function Df(){H(K);H(J)}function Ef(a,b,c){if(J.current!==Af)throw Error(u(168));I(J,b);I(K,c)}function Ff(a,b,c){var d=a.stateNode;a=b.childContextTypes;if("function"!==typeof d.getChildContext)return c;d=d.getChildContext();for(var e in d)if(!(e in a))throw Error(u(108,pb(b)||"Unknown",e));return n({},c,{},d)}function Gf(a){a=(a=a.stateNode)&&a.__reactInternalMemoizedMergedChildContext||Af;Bf=J.current;I(J,a);I(K,K.current);return!0}
function Hf(a,b,c){var d=a.stateNode;if(!d)throw Error(u(169));c?(a=Ff(a,b,Bf),d.__reactInternalMemoizedMergedChildContext=a,H(K),H(J),I(J,a)):H(K);I(K,c)}
var If=r.unstable_runWithPriority,Jf=r.unstable_scheduleCallback,Kf=r.unstable_cancelCallback,Lf=r.unstable_requestPaint,Mf=r.unstable_now,Nf=r.unstable_getCurrentPriorityLevel,Of=r.unstable_ImmediatePriority,Pf=r.unstable_UserBlockingPriority,Qf=r.unstable_NormalPriority,Rf=r.unstable_LowPriority,Sf=r.unstable_IdlePriority,Tf={},Uf=r.unstable_shouldYield,Vf=void 0!==Lf?Lf:function(){},Wf=null,Xf=null,Yf=!1,Zf=Mf(),$f=1E4>Zf?Mf:function(){return Mf()-Zf};
function ag(){switch(Nf()){case Of:return 99;case Pf:return 98;case Qf:return 97;case Rf:return 96;case Sf:return 95;default:throw Error(u(332));}}function bg(a){switch(a){case 99:return Of;case 98:return Pf;case 97:return Qf;case 96:return Rf;case 95:return Sf;default:throw Error(u(332));}}function cg(a,b){a=bg(a);return If(a,b)}function dg(a,b,c){a=bg(a);return Jf(a,b,c)}function eg(a){null===Wf?(Wf=[a],Xf=Jf(Of,fg)):Wf.push(a);return Tf}function gg(){if(null!==Xf){var a=Xf;Xf=null;Kf(a)}fg()}
function fg(){if(!Yf&&null!==Wf){Yf=!0;var a=0;try{var b=Wf;cg(99,function(){for(;a<b.length;a++){var c=b[a];do c=c(!0);while(null!==c)}});Wf=null}catch(c){throw null!==Wf&&(Wf=Wf.slice(a+1)),Jf(Of,gg),c;}finally{Yf=!1}}}function hg(a,b,c){c/=10;return 1073741821-(((1073741821-a+b/10)/c|0)+1)*c}function ig(a,b){if(a&&a.defaultProps){b=n({},b);a=a.defaultProps;for(var c in a)void 0===b[c]&&(b[c]=a[c])}return b}var jg={current:null},kg=null,lg=null,mg=null;function ng(){mg=lg=kg=null}
function og(a){var b=jg.current;H(jg);a.type._context._currentValue=b}function pg(a,b){for(;null!==a;){var c=a.alternate;if(a.childExpirationTime<b)a.childExpirationTime=b,null!==c&&c.childExpirationTime<b&&(c.childExpirationTime=b);else if(null!==c&&c.childExpirationTime<b)c.childExpirationTime=b;else break;a=a.return}}function qg(a,b){kg=a;mg=lg=null;a=a.dependencies;null!==a&&null!==a.firstContext&&(a.expirationTime>=b&&(rg=!0),a.firstContext=null)}
function sg(a,b){if(mg!==a&&!1!==b&&0!==b){if("number"!==typeof b||1073741823===b)mg=a,b=1073741823;b={context:a,observedBits:b,next:null};if(null===lg){if(null===kg)throw Error(u(308));lg=b;kg.dependencies={expirationTime:0,firstContext:b,responders:null}}else lg=lg.next=b}return a._currentValue}var tg=!1;function ug(a){a.updateQueue={baseState:a.memoizedState,baseQueue:null,shared:{pending:null},effects:null}}
function vg(a,b){a=a.updateQueue;b.updateQueue===a&&(b.updateQueue={baseState:a.baseState,baseQueue:a.baseQueue,shared:a.shared,effects:a.effects})}function wg(a,b){a={expirationTime:a,suspenseConfig:b,tag:0,payload:null,callback:null,next:null};return a.next=a}function xg(a,b){a=a.updateQueue;if(null!==a){a=a.shared;var c=a.pending;null===c?b.next=b:(b.next=c.next,c.next=b);a.pending=b}}
function yg(a,b){var c=a.alternate;null!==c&&vg(c,a);a=a.updateQueue;c=a.baseQueue;null===c?(a.baseQueue=b.next=b,b.next=b):(b.next=c.next,c.next=b)}
function zg(a,b,c,d){var e=a.updateQueue;tg=!1;var f=e.baseQueue,g=e.shared.pending;if(null!==g){if(null!==f){var h=f.next;f.next=g.next;g.next=h}f=g;e.shared.pending=null;h=a.alternate;null!==h&&(h=h.updateQueue,null!==h&&(h.baseQueue=g))}if(null!==f){h=f.next;var k=e.baseState,l=0,m=null,p=null,x=null;if(null!==h){var z=h;do{g=z.expirationTime;if(g<d){var ca={expirationTime:z.expirationTime,suspenseConfig:z.suspenseConfig,tag:z.tag,payload:z.payload,callback:z.callback,next:null};null===x?(p=x=
ca,m=k):x=x.next=ca;g>l&&(l=g)}else{null!==x&&(x=x.next={expirationTime:1073741823,suspenseConfig:z.suspenseConfig,tag:z.tag,payload:z.payload,callback:z.callback,next:null});Ag(g,z.suspenseConfig);a:{var D=a,t=z;g=b;ca=c;switch(t.tag){case 1:D=t.payload;if("function"===typeof D){k=D.call(ca,k,g);break a}k=D;break a;case 3:D.effectTag=D.effectTag&-4097|64;case 0:D=t.payload;g="function"===typeof D?D.call(ca,k,g):D;if(null===g||void 0===g)break a;k=n({},k,g);break a;case 2:tg=!0}}null!==z.callback&&
(a.effectTag|=32,g=e.effects,null===g?e.effects=[z]:g.push(z))}z=z.next;if(null===z||z===h)if(g=e.shared.pending,null===g)break;else z=f.next=g.next,g.next=h,e.baseQueue=f=g,e.shared.pending=null}while(1)}null===x?m=k:x.next=p;e.baseState=m;e.baseQueue=x;Bg(l);a.expirationTime=l;a.memoizedState=k}}
function Cg(a,b,c){a=b.effects;b.effects=null;if(null!==a)for(b=0;b<a.length;b++){var d=a[b],e=d.callback;if(null!==e){d.callback=null;d=e;e=c;if("function"!==typeof d)throw Error(u(191,d));d.call(e)}}}var Dg=Wa.ReactCurrentBatchConfig,Eg=(new aa.Component).refs;function Fg(a,b,c,d){b=a.memoizedState;c=c(d,b);c=null===c||void 0===c?b:n({},b,c);a.memoizedState=c;0===a.expirationTime&&(a.updateQueue.baseState=c)}
var Jg={isMounted:function(a){return(a=a._reactInternalFiber)?dc(a)===a:!1},enqueueSetState:function(a,b,c){a=a._reactInternalFiber;var d=Gg(),e=Dg.suspense;d=Hg(d,a,e);e=wg(d,e);e.payload=b;void 0!==c&&null!==c&&(e.callback=c);xg(a,e);Ig(a,d)},enqueueReplaceState:function(a,b,c){a=a._reactInternalFiber;var d=Gg(),e=Dg.suspense;d=Hg(d,a,e);e=wg(d,e);e.tag=1;e.payload=b;void 0!==c&&null!==c&&(e.callback=c);xg(a,e);Ig(a,d)},enqueueForceUpdate:function(a,b){a=a._reactInternalFiber;var c=Gg(),d=Dg.suspense;
c=Hg(c,a,d);d=wg(c,d);d.tag=2;void 0!==b&&null!==b&&(d.callback=b);xg(a,d);Ig(a,c)}};function Kg(a,b,c,d,e,f,g){a=a.stateNode;return"function"===typeof a.shouldComponentUpdate?a.shouldComponentUpdate(d,f,g):b.prototype&&b.prototype.isPureReactComponent?!bf(c,d)||!bf(e,f):!0}
function Lg(a,b,c){var d=!1,e=Af;var f=b.contextType;"object"===typeof f&&null!==f?f=sg(f):(e=L(b)?Bf:J.current,d=b.contextTypes,f=(d=null!==d&&void 0!==d)?Cf(a,e):Af);b=new b(c,f);a.memoizedState=null!==b.state&&void 0!==b.state?b.state:null;b.updater=Jg;a.stateNode=b;b._reactInternalFiber=a;d&&(a=a.stateNode,a.__reactInternalMemoizedUnmaskedChildContext=e,a.__reactInternalMemoizedMaskedChildContext=f);return b}
function Mg(a,b,c,d){a=b.state;"function"===typeof b.componentWillReceiveProps&&b.componentWillReceiveProps(c,d);"function"===typeof b.UNSAFE_componentWillReceiveProps&&b.UNSAFE_componentWillReceiveProps(c,d);b.state!==a&&Jg.enqueueReplaceState(b,b.state,null)}
function Ng(a,b,c,d){var e=a.stateNode;e.props=c;e.state=a.memoizedState;e.refs=Eg;ug(a);var f=b.contextType;"object"===typeof f&&null!==f?e.context=sg(f):(f=L(b)?Bf:J.current,e.context=Cf(a,f));zg(a,c,e,d);e.state=a.memoizedState;f=b.getDerivedStateFromProps;"function"===typeof f&&(Fg(a,b,f,c),e.state=a.memoizedState);"function"===typeof b.getDerivedStateFromProps||"function"===typeof e.getSnapshotBeforeUpdate||"function"!==typeof e.UNSAFE_componentWillMount&&"function"!==typeof e.componentWillMount||
(b=e.state,"function"===typeof e.componentWillMount&&e.componentWillMount(),"function"===typeof e.UNSAFE_componentWillMount&&e.UNSAFE_componentWillMount(),b!==e.state&&Jg.enqueueReplaceState(e,e.state,null),zg(a,c,e,d),e.state=a.memoizedState);"function"===typeof e.componentDidMount&&(a.effectTag|=4)}var Og=Array.isArray;
function Pg(a,b,c){a=c.ref;if(null!==a&&"function"!==typeof a&&"object"!==typeof a){if(c._owner){c=c._owner;if(c){if(1!==c.tag)throw Error(u(309));var d=c.stateNode}if(!d)throw Error(u(147,a));var e=""+a;if(null!==b&&null!==b.ref&&"function"===typeof b.ref&&b.ref._stringRef===e)return b.ref;b=function(a){var b=d.refs;b===Eg&&(b=d.refs={});null===a?delete b[e]:b[e]=a};b._stringRef=e;return b}if("string"!==typeof a)throw Error(u(284));if(!c._owner)throw Error(u(290,a));}return a}
function Qg(a,b){if("textarea"!==a.type)throw Error(u(31,"[object Object]"===Object.prototype.toString.call(b)?"object with keys {"+Object.keys(b).join(", ")+"}":b,""));}
function Rg(a){function b(b,c){if(a){var d=b.lastEffect;null!==d?(d.nextEffect=c,b.lastEffect=c):b.firstEffect=b.lastEffect=c;c.nextEffect=null;c.effectTag=8}}function c(c,d){if(!a)return null;for(;null!==d;)b(c,d),d=d.sibling;return null}function d(a,b){for(a=new Map;null!==b;)null!==b.key?a.set(b.key,b):a.set(b.index,b),b=b.sibling;return a}function e(a,b){a=Sg(a,b);a.index=0;a.sibling=null;return a}function f(b,c,d){b.index=d;if(!a)return c;d=b.alternate;if(null!==d)return d=d.index,d<c?(b.effectTag=
2,c):d;b.effectTag=2;return c}function g(b){a&&null===b.alternate&&(b.effectTag=2);return b}function h(a,b,c,d){if(null===b||6!==b.tag)return b=Tg(c,a.mode,d),b.return=a,b;b=e(b,c);b.return=a;return b}function k(a,b,c,d){if(null!==b&&b.elementType===c.type)return d=e(b,c.props),d.ref=Pg(a,b,c),d.return=a,d;d=Ug(c.type,c.key,c.props,null,a.mode,d);d.ref=Pg(a,b,c);d.return=a;return d}function l(a,b,c,d){if(null===b||4!==b.tag||b.stateNode.containerInfo!==c.containerInfo||b.stateNode.implementation!==
c.implementation)return b=Vg(c,a.mode,d),b.return=a,b;b=e(b,c.children||[]);b.return=a;return b}function m(a,b,c,d,f){if(null===b||7!==b.tag)return b=Wg(c,a.mode,d,f),b.return=a,b;b=e(b,c);b.return=a;return b}function p(a,b,c){if("string"===typeof b||"number"===typeof b)return b=Tg(""+b,a.mode,c),b.return=a,b;if("object"===typeof b&&null!==b){switch(b.$$typeof){case Za:return c=Ug(b.type,b.key,b.props,null,a.mode,c),c.ref=Pg(a,null,b),c.return=a,c;case $a:return b=Vg(b,a.mode,c),b.return=a,b}if(Og(b)||
nb(b))return b=Wg(b,a.mode,c,null),b.return=a,b;Qg(a,b)}return null}function x(a,b,c,d){var e=null!==b?b.key:null;if("string"===typeof c||"number"===typeof c)return null!==e?null:h(a,b,""+c,d);if("object"===typeof c&&null!==c){switch(c.$$typeof){case Za:return c.key===e?c.type===ab?m(a,b,c.props.children,d,e):k(a,b,c,d):null;case $a:return c.key===e?l(a,b,c,d):null}if(Og(c)||nb(c))return null!==e?null:m(a,b,c,d,null);Qg(a,c)}return null}function z(a,b,c,d,e){if("string"===typeof d||"number"===typeof d)return a=
a.get(c)||null,h(b,a,""+d,e);if("object"===typeof d&&null!==d){switch(d.$$typeof){case Za:return a=a.get(null===d.key?c:d.key)||null,d.type===ab?m(b,a,d.props.children,e,d.key):k(b,a,d,e);case $a:return a=a.get(null===d.key?c:d.key)||null,l(b,a,d,e)}if(Og(d)||nb(d))return a=a.get(c)||null,m(b,a,d,e,null);Qg(b,d)}return null}function ca(e,g,h,k){for(var l=null,t=null,m=g,y=g=0,A=null;null!==m&&y<h.length;y++){m.index>y?(A=m,m=null):A=m.sibling;var q=x(e,m,h[y],k);if(null===q){null===m&&(m=A);break}a&&
m&&null===q.alternate&&b(e,m);g=f(q,g,y);null===t?l=q:t.sibling=q;t=q;m=A}if(y===h.length)return c(e,m),l;if(null===m){for(;y<h.length;y++)m=p(e,h[y],k),null!==m&&(g=f(m,g,y),null===t?l=m:t.sibling=m,t=m);return l}for(m=d(e,m);y<h.length;y++)A=z(m,e,y,h[y],k),null!==A&&(a&&null!==A.alternate&&m.delete(null===A.key?y:A.key),g=f(A,g,y),null===t?l=A:t.sibling=A,t=A);a&&m.forEach(function(a){return b(e,a)});return l}function D(e,g,h,l){var k=nb(h);if("function"!==typeof k)throw Error(u(150));h=k.call(h);
if(null==h)throw Error(u(151));for(var m=k=null,t=g,y=g=0,A=null,q=h.next();null!==t&&!q.done;y++,q=h.next()){t.index>y?(A=t,t=null):A=t.sibling;var D=x(e,t,q.value,l);if(null===D){null===t&&(t=A);break}a&&t&&null===D.alternate&&b(e,t);g=f(D,g,y);null===m?k=D:m.sibling=D;m=D;t=A}if(q.done)return c(e,t),k;if(null===t){for(;!q.done;y++,q=h.next())q=p(e,q.value,l),null!==q&&(g=f(q,g,y),null===m?k=q:m.sibling=q,m=q);return k}for(t=d(e,t);!q.done;y++,q=h.next())q=z(t,e,y,q.value,l),null!==q&&(a&&null!==
q.alternate&&t.delete(null===q.key?y:q.key),g=f(q,g,y),null===m?k=q:m.sibling=q,m=q);a&&t.forEach(function(a){return b(e,a)});return k}return function(a,d,f,h){var k="object"===typeof f&&null!==f&&f.type===ab&&null===f.key;k&&(f=f.props.children);var l="object"===typeof f&&null!==f;if(l)switch(f.$$typeof){case Za:a:{l=f.key;for(k=d;null!==k;){if(k.key===l){switch(k.tag){case 7:if(f.type===ab){c(a,k.sibling);d=e(k,f.props.children);d.return=a;a=d;break a}break;default:if(k.elementType===f.type){c(a,
k.sibling);d=e(k,f.props);d.ref=Pg(a,k,f);d.return=a;a=d;break a}}c(a,k);break}else b(a,k);k=k.sibling}f.type===ab?(d=Wg(f.props.children,a.mode,h,f.key),d.return=a,a=d):(h=Ug(f.type,f.key,f.props,null,a.mode,h),h.ref=Pg(a,d,f),h.return=a,a=h)}return g(a);case $a:a:{for(k=f.key;null!==d;){if(d.key===k)if(4===d.tag&&d.stateNode.containerInfo===f.containerInfo&&d.stateNode.implementation===f.implementation){c(a,d.sibling);d=e(d,f.children||[]);d.return=a;a=d;break a}else{c(a,d);break}else b(a,d);d=
d.sibling}d=Vg(f,a.mode,h);d.return=a;a=d}return g(a)}if("string"===typeof f||"number"===typeof f)return f=""+f,null!==d&&6===d.tag?(c(a,d.sibling),d=e(d,f),d.return=a,a=d):(c(a,d),d=Tg(f,a.mode,h),d.return=a,a=d),g(a);if(Og(f))return ca(a,d,f,h);if(nb(f))return D(a,d,f,h);l&&Qg(a,f);if("undefined"===typeof f&&!k)switch(a.tag){case 1:case 0:throw a=a.type,Error(u(152,a.displayName||a.name||"Component"));}return c(a,d)}}var Xg=Rg(!0),Yg=Rg(!1),Zg={},$g={current:Zg},ah={current:Zg},bh={current:Zg};
function ch(a){if(a===Zg)throw Error(u(174));return a}function dh(a,b){I(bh,b);I(ah,a);I($g,Zg);a=b.nodeType;switch(a){case 9:case 11:b=(b=b.documentElement)?b.namespaceURI:Ob(null,"");break;default:a=8===a?b.parentNode:b,b=a.namespaceURI||null,a=a.tagName,b=Ob(b,a)}H($g);I($g,b)}function eh(){H($g);H(ah);H(bh)}function fh(a){ch(bh.current);var b=ch($g.current);var c=Ob(b,a.type);b!==c&&(I(ah,a),I($g,c))}function gh(a){ah.current===a&&(H($g),H(ah))}var M={current:0};
function hh(a){for(var b=a;null!==b;){if(13===b.tag){var c=b.memoizedState;if(null!==c&&(c=c.dehydrated,null===c||c.data===Bd||c.data===Cd))return b}else if(19===b.tag&&void 0!==b.memoizedProps.revealOrder){if(0!==(b.effectTag&64))return b}else if(null!==b.child){b.child.return=b;b=b.child;continue}if(b===a)break;for(;null===b.sibling;){if(null===b.return||b.return===a)return null;b=b.return}b.sibling.return=b.return;b=b.sibling}return null}function ih(a,b){return{responder:a,props:b}}
var jh=Wa.ReactCurrentDispatcher,kh=Wa.ReactCurrentBatchConfig,lh=0,N=null,O=null,P=null,mh=!1;function Q(){throw Error(u(321));}function nh(a,b){if(null===b)return!1;for(var c=0;c<b.length&&c<a.length;c++)if(!$e(a[c],b[c]))return!1;return!0}
function oh(a,b,c,d,e,f){lh=f;N=b;b.memoizedState=null;b.updateQueue=null;b.expirationTime=0;jh.current=null===a||null===a.memoizedState?ph:qh;a=c(d,e);if(b.expirationTime===lh){f=0;do{b.expirationTime=0;if(!(25>f))throw Error(u(301));f+=1;P=O=null;b.updateQueue=null;jh.current=rh;a=c(d,e)}while(b.expirationTime===lh)}jh.current=sh;b=null!==O&&null!==O.next;lh=0;P=O=N=null;mh=!1;if(b)throw Error(u(300));return a}
function th(){var a={memoizedState:null,baseState:null,baseQueue:null,queue:null,next:null};null===P?N.memoizedState=P=a:P=P.next=a;return P}function uh(){if(null===O){var a=N.alternate;a=null!==a?a.memoizedState:null}else a=O.next;var b=null===P?N.memoizedState:P.next;if(null!==b)P=b,O=a;else{if(null===a)throw Error(u(310));O=a;a={memoizedState:O.memoizedState,baseState:O.baseState,baseQueue:O.baseQueue,queue:O.queue,next:null};null===P?N.memoizedState=P=a:P=P.next=a}return P}
function vh(a,b){return"function"===typeof b?b(a):b}
function wh(a){var b=uh(),c=b.queue;if(null===c)throw Error(u(311));c.lastRenderedReducer=a;var d=O,e=d.baseQueue,f=c.pending;if(null!==f){if(null!==e){var g=e.next;e.next=f.next;f.next=g}d.baseQueue=e=f;c.pending=null}if(null!==e){e=e.next;d=d.baseState;var h=g=f=null,k=e;do{var l=k.expirationTime;if(l<lh){var m={expirationTime:k.expirationTime,suspenseConfig:k.suspenseConfig,action:k.action,eagerReducer:k.eagerReducer,eagerState:k.eagerState,next:null};null===h?(g=h=m,f=d):h=h.next=m;l>N.expirationTime&&
(N.expirationTime=l,Bg(l))}else null!==h&&(h=h.next={expirationTime:1073741823,suspenseConfig:k.suspenseConfig,action:k.action,eagerReducer:k.eagerReducer,eagerState:k.eagerState,next:null}),Ag(l,k.suspenseConfig),d=k.eagerReducer===a?k.eagerState:a(d,k.action);k=k.next}while(null!==k&&k!==e);null===h?f=d:h.next=g;$e(d,b.memoizedState)||(rg=!0);b.memoizedState=d;b.baseState=f;b.baseQueue=h;c.lastRenderedState=d}return[b.memoizedState,c.dispatch]}
function xh(a){var b=uh(),c=b.queue;if(null===c)throw Error(u(311));c.lastRenderedReducer=a;var d=c.dispatch,e=c.pending,f=b.memoizedState;if(null!==e){c.pending=null;var g=e=e.next;do f=a(f,g.action),g=g.next;while(g!==e);$e(f,b.memoizedState)||(rg=!0);b.memoizedState=f;null===b.baseQueue&&(b.baseState=f);c.lastRenderedState=f}return[f,d]}
function yh(a){var b=th();"function"===typeof a&&(a=a());b.memoizedState=b.baseState=a;a=b.queue={pending:null,dispatch:null,lastRenderedReducer:vh,lastRenderedState:a};a=a.dispatch=zh.bind(null,N,a);return[b.memoizedState,a]}function Ah(a,b,c,d){a={tag:a,create:b,destroy:c,deps:d,next:null};b=N.updateQueue;null===b?(b={lastEffect:null},N.updateQueue=b,b.lastEffect=a.next=a):(c=b.lastEffect,null===c?b.lastEffect=a.next=a:(d=c.next,c.next=a,a.next=d,b.lastEffect=a));return a}
function Bh(){return uh().memoizedState}function Ch(a,b,c,d){var e=th();N.effectTag|=a;e.memoizedState=Ah(1|b,c,void 0,void 0===d?null:d)}function Dh(a,b,c,d){var e=uh();d=void 0===d?null:d;var f=void 0;if(null!==O){var g=O.memoizedState;f=g.destroy;if(null!==d&&nh(d,g.deps)){Ah(b,c,f,d);return}}N.effectTag|=a;e.memoizedState=Ah(1|b,c,f,d)}function Eh(a,b){return Ch(516,4,a,b)}function Fh(a,b){return Dh(516,4,a,b)}function Gh(a,b){return Dh(4,2,a,b)}
function Hh(a,b){if("function"===typeof b)return a=a(),b(a),function(){b(null)};if(null!==b&&void 0!==b)return a=a(),b.current=a,function(){b.current=null}}function Ih(a,b,c){c=null!==c&&void 0!==c?c.concat([a]):null;return Dh(4,2,Hh.bind(null,b,a),c)}function Jh(){}function Kh(a,b){th().memoizedState=[a,void 0===b?null:b];return a}function Lh(a,b){var c=uh();b=void 0===b?null:b;var d=c.memoizedState;if(null!==d&&null!==b&&nh(b,d[1]))return d[0];c.memoizedState=[a,b];return a}
function Mh(a,b){var c=uh();b=void 0===b?null:b;var d=c.memoizedState;if(null!==d&&null!==b&&nh(b,d[1]))return d[0];a=a();c.memoizedState=[a,b];return a}function Nh(a,b,c){var d=ag();cg(98>d?98:d,function(){a(!0)});cg(97<d?97:d,function(){var d=kh.suspense;kh.suspense=void 0===b?null:b;try{a(!1),c()}finally{kh.suspense=d}})}
function zh(a,b,c){var d=Gg(),e=Dg.suspense;d=Hg(d,a,e);e={expirationTime:d,suspenseConfig:e,action:c,eagerReducer:null,eagerState:null,next:null};var f=b.pending;null===f?e.next=e:(e.next=f.next,f.next=e);b.pending=e;f=a.alternate;if(a===N||null!==f&&f===N)mh=!0,e.expirationTime=lh,N.expirationTime=lh;else{if(0===a.expirationTime&&(null===f||0===f.expirationTime)&&(f=b.lastRenderedReducer,null!==f))try{var g=b.lastRenderedState,h=f(g,c);e.eagerReducer=f;e.eagerState=h;if($e(h,g))return}catch(k){}finally{}Ig(a,
d)}}
var sh={readContext:sg,useCallback:Q,useContext:Q,useEffect:Q,useImperativeHandle:Q,useLayoutEffect:Q,useMemo:Q,useReducer:Q,useRef:Q,useState:Q,useDebugValue:Q,useResponder:Q,useDeferredValue:Q,useTransition:Q},ph={readContext:sg,useCallback:Kh,useContext:sg,useEffect:Eh,useImperativeHandle:function(a,b,c){c=null!==c&&void 0!==c?c.concat([a]):null;return Ch(4,2,Hh.bind(null,b,a),c)},useLayoutEffect:function(a,b){return Ch(4,2,a,b)},useMemo:function(a,b){var c=th();b=void 0===b?null:b;a=a();c.memoizedState=[a,
b];return a},useReducer:function(a,b,c){var d=th();b=void 0!==c?c(b):b;d.memoizedState=d.baseState=b;a=d.queue={pending:null,dispatch:null,lastRenderedReducer:a,lastRenderedState:b};a=a.dispatch=zh.bind(null,N,a);return[d.memoizedState,a]},useRef:function(a){var b=th();a={current:a};return b.memoizedState=a},useState:yh,useDebugValue:Jh,useResponder:ih,useDeferredValue:function(a,b){var c=yh(a),d=c[0],e=c[1];Eh(function(){var c=kh.suspense;kh.suspense=void 0===b?null:b;try{e(a)}finally{kh.suspense=
c}},[a,b]);return d},useTransition:function(a){var b=yh(!1),c=b[0];b=b[1];return[Kh(Nh.bind(null,b,a),[b,a]),c]}},qh={readContext:sg,useCallback:Lh,useContext:sg,useEffect:Fh,useImperativeHandle:Ih,useLayoutEffect:Gh,useMemo:Mh,useReducer:wh,useRef:Bh,useState:function(){return wh(vh)},useDebugValue:Jh,useResponder:ih,useDeferredValue:function(a,b){var c=wh(vh),d=c[0],e=c[1];Fh(function(){var c=kh.suspense;kh.suspense=void 0===b?null:b;try{e(a)}finally{kh.suspense=c}},[a,b]);return d},useTransition:function(a){var b=
wh(vh),c=b[0];b=b[1];return[Lh(Nh.bind(null,b,a),[b,a]),c]}},rh={readContext:sg,useCallback:Lh,useContext:sg,useEffect:Fh,useImperativeHandle:Ih,useLayoutEffect:Gh,useMemo:Mh,useReducer:xh,useRef:Bh,useState:function(){return xh(vh)},useDebugValue:Jh,useResponder:ih,useDeferredValue:function(a,b){var c=xh(vh),d=c[0],e=c[1];Fh(function(){var c=kh.suspense;kh.suspense=void 0===b?null:b;try{e(a)}finally{kh.suspense=c}},[a,b]);return d},useTransition:function(a){var b=xh(vh),c=b[0];b=b[1];return[Lh(Nh.bind(null,
b,a),[b,a]),c]}},Oh=null,Ph=null,Qh=!1;function Rh(a,b){var c=Sh(5,null,null,0);c.elementType="DELETED";c.type="DELETED";c.stateNode=b;c.return=a;c.effectTag=8;null!==a.lastEffect?(a.lastEffect.nextEffect=c,a.lastEffect=c):a.firstEffect=a.lastEffect=c}
function Th(a,b){switch(a.tag){case 5:var c=a.type;b=1!==b.nodeType||c.toLowerCase()!==b.nodeName.toLowerCase()?null:b;return null!==b?(a.stateNode=b,!0):!1;case 6:return b=""===a.pendingProps||3!==b.nodeType?null:b,null!==b?(a.stateNode=b,!0):!1;case 13:return!1;default:return!1}}
function Uh(a){if(Qh){var b=Ph;if(b){var c=b;if(!Th(a,b)){b=Jd(c.nextSibling);if(!b||!Th(a,b)){a.effectTag=a.effectTag&-1025|2;Qh=!1;Oh=a;return}Rh(Oh,c)}Oh=a;Ph=Jd(b.firstChild)}else a.effectTag=a.effectTag&-1025|2,Qh=!1,Oh=a}}function Vh(a){for(a=a.return;null!==a&&5!==a.tag&&3!==a.tag&&13!==a.tag;)a=a.return;Oh=a}
function Wh(a){if(a!==Oh)return!1;if(!Qh)return Vh(a),Qh=!0,!1;var b=a.type;if(5!==a.tag||"head"!==b&&"body"!==b&&!Gd(b,a.memoizedProps))for(b=Ph;b;)Rh(a,b),b=Jd(b.nextSibling);Vh(a);if(13===a.tag){a=a.memoizedState;a=null!==a?a.dehydrated:null;if(!a)throw Error(u(317));a:{a=a.nextSibling;for(b=0;a;){if(8===a.nodeType){var c=a.data;if(c===Ad){if(0===b){Ph=Jd(a.nextSibling);break a}b--}else c!==zd&&c!==Cd&&c!==Bd||b++}a=a.nextSibling}Ph=null}}else Ph=Oh?Jd(a.stateNode.nextSibling):null;return!0}
function Xh(){Ph=Oh=null;Qh=!1}var Yh=Wa.ReactCurrentOwner,rg=!1;function R(a,b,c,d){b.child=null===a?Yg(b,null,c,d):Xg(b,a.child,c,d)}function Zh(a,b,c,d,e){c=c.render;var f=b.ref;qg(b,e);d=oh(a,b,c,d,f,e);if(null!==a&&!rg)return b.updateQueue=a.updateQueue,b.effectTag&=-517,a.expirationTime<=e&&(a.expirationTime=0),$h(a,b,e);b.effectTag|=1;R(a,b,d,e);return b.child}
function ai(a,b,c,d,e,f){if(null===a){var g=c.type;if("function"===typeof g&&!bi(g)&&void 0===g.defaultProps&&null===c.compare&&void 0===c.defaultProps)return b.tag=15,b.type=g,ci(a,b,g,d,e,f);a=Ug(c.type,null,d,null,b.mode,f);a.ref=b.ref;a.return=b;return b.child=a}g=a.child;if(e<f&&(e=g.memoizedProps,c=c.compare,c=null!==c?c:bf,c(e,d)&&a.ref===b.ref))return $h(a,b,f);b.effectTag|=1;a=Sg(g,d);a.ref=b.ref;a.return=b;return b.child=a}
function ci(a,b,c,d,e,f){return null!==a&&bf(a.memoizedProps,d)&&a.ref===b.ref&&(rg=!1,e<f)?(b.expirationTime=a.expirationTime,$h(a,b,f)):di(a,b,c,d,f)}function ei(a,b){var c=b.ref;if(null===a&&null!==c||null!==a&&a.ref!==c)b.effectTag|=128}function di(a,b,c,d,e){var f=L(c)?Bf:J.current;f=Cf(b,f);qg(b,e);c=oh(a,b,c,d,f,e);if(null!==a&&!rg)return b.updateQueue=a.updateQueue,b.effectTag&=-517,a.expirationTime<=e&&(a.expirationTime=0),$h(a,b,e);b.effectTag|=1;R(a,b,c,e);return b.child}
function fi(a,b,c,d,e){if(L(c)){var f=!0;Gf(b)}else f=!1;qg(b,e);if(null===b.stateNode)null!==a&&(a.alternate=null,b.alternate=null,b.effectTag|=2),Lg(b,c,d),Ng(b,c,d,e),d=!0;else if(null===a){var g=b.stateNode,h=b.memoizedProps;g.props=h;var k=g.context,l=c.contextType;"object"===typeof l&&null!==l?l=sg(l):(l=L(c)?Bf:J.current,l=Cf(b,l));var m=c.getDerivedStateFromProps,p="function"===typeof m||"function"===typeof g.getSnapshotBeforeUpdate;p||"function"!==typeof g.UNSAFE_componentWillReceiveProps&&
"function"!==typeof g.componentWillReceiveProps||(h!==d||k!==l)&&Mg(b,g,d,l);tg=!1;var x=b.memoizedState;g.state=x;zg(b,d,g,e);k=b.memoizedState;h!==d||x!==k||K.current||tg?("function"===typeof m&&(Fg(b,c,m,d),k=b.memoizedState),(h=tg||Kg(b,c,h,d,x,k,l))?(p||"function"!==typeof g.UNSAFE_componentWillMount&&"function"!==typeof g.componentWillMount||("function"===typeof g.componentWillMount&&g.componentWillMount(),"function"===typeof g.UNSAFE_componentWillMount&&g.UNSAFE_componentWillMount()),"function"===
typeof g.componentDidMount&&(b.effectTag|=4)):("function"===typeof g.componentDidMount&&(b.effectTag|=4),b.memoizedProps=d,b.memoizedState=k),g.props=d,g.state=k,g.context=l,d=h):("function"===typeof g.componentDidMount&&(b.effectTag|=4),d=!1)}else g=b.stateNode,vg(a,b),h=b.memoizedProps,g.props=b.type===b.elementType?h:ig(b.type,h),k=g.context,l=c.contextType,"object"===typeof l&&null!==l?l=sg(l):(l=L(c)?Bf:J.current,l=Cf(b,l)),m=c.getDerivedStateFromProps,(p="function"===typeof m||"function"===
typeof g.getSnapshotBeforeUpdate)||"function"!==typeof g.UNSAFE_componentWillReceiveProps&&"function"!==typeof g.componentWillReceiveProps||(h!==d||k!==l)&&Mg(b,g,d,l),tg=!1,k=b.memoizedState,g.state=k,zg(b,d,g,e),x=b.memoizedState,h!==d||k!==x||K.current||tg?("function"===typeof m&&(Fg(b,c,m,d),x=b.memoizedState),(m=tg||Kg(b,c,h,d,k,x,l))?(p||"function"!==typeof g.UNSAFE_componentWillUpdate&&"function"!==typeof g.componentWillUpdate||("function"===typeof g.componentWillUpdate&&g.componentWillUpdate(d,
x,l),"function"===typeof g.UNSAFE_componentWillUpdate&&g.UNSAFE_componentWillUpdate(d,x,l)),"function"===typeof g.componentDidUpdate&&(b.effectTag|=4),"function"===typeof g.getSnapshotBeforeUpdate&&(b.effectTag|=256)):("function"!==typeof g.componentDidUpdate||h===a.memoizedProps&&k===a.memoizedState||(b.effectTag|=4),"function"!==typeof g.getSnapshotBeforeUpdate||h===a.memoizedProps&&k===a.memoizedState||(b.effectTag|=256),b.memoizedProps=d,b.memoizedState=x),g.props=d,g.state=x,g.context=l,d=m):
("function"!==typeof g.componentDidUpdate||h===a.memoizedProps&&k===a.memoizedState||(b.effectTag|=4),"function"!==typeof g.getSnapshotBeforeUpdate||h===a.memoizedProps&&k===a.memoizedState||(b.effectTag|=256),d=!1);return gi(a,b,c,d,f,e)}
function gi(a,b,c,d,e,f){ei(a,b);var g=0!==(b.effectTag&64);if(!d&&!g)return e&&Hf(b,c,!1),$h(a,b,f);d=b.stateNode;Yh.current=b;var h=g&&"function"!==typeof c.getDerivedStateFromError?null:d.render();b.effectTag|=1;null!==a&&g?(b.child=Xg(b,a.child,null,f),b.child=Xg(b,null,h,f)):R(a,b,h,f);b.memoizedState=d.state;e&&Hf(b,c,!0);return b.child}function hi(a){var b=a.stateNode;b.pendingContext?Ef(a,b.pendingContext,b.pendingContext!==b.context):b.context&&Ef(a,b.context,!1);dh(a,b.containerInfo)}
var ii={dehydrated:null,retryTime:0};
function ji(a,b,c){var d=b.mode,e=b.pendingProps,f=M.current,g=!1,h;(h=0!==(b.effectTag&64))||(h=0!==(f&2)&&(null===a||null!==a.memoizedState));h?(g=!0,b.effectTag&=-65):null!==a&&null===a.memoizedState||void 0===e.fallback||!0===e.unstable_avoidThisFallback||(f|=1);I(M,f&1);if(null===a){void 0!==e.fallback&&Uh(b);if(g){g=e.fallback;e=Wg(null,d,0,null);e.return=b;if(0===(b.mode&2))for(a=null!==b.memoizedState?b.child.child:b.child,e.child=a;null!==a;)a.return=e,a=a.sibling;c=Wg(g,d,c,null);c.return=
b;e.sibling=c;b.memoizedState=ii;b.child=e;return c}d=e.children;b.memoizedState=null;return b.child=Yg(b,null,d,c)}if(null!==a.memoizedState){a=a.child;d=a.sibling;if(g){e=e.fallback;c=Sg(a,a.pendingProps);c.return=b;if(0===(b.mode&2)&&(g=null!==b.memoizedState?b.child.child:b.child,g!==a.child))for(c.child=g;null!==g;)g.return=c,g=g.sibling;d=Sg(d,e);d.return=b;c.sibling=d;c.childExpirationTime=0;b.memoizedState=ii;b.child=c;return d}c=Xg(b,a.child,e.children,c);b.memoizedState=null;return b.child=
c}a=a.child;if(g){g=e.fallback;e=Wg(null,d,0,null);e.return=b;e.child=a;null!==a&&(a.return=e);if(0===(b.mode&2))for(a=null!==b.memoizedState?b.child.child:b.child,e.child=a;null!==a;)a.return=e,a=a.sibling;c=Wg(g,d,c,null);c.return=b;e.sibling=c;c.effectTag|=2;e.childExpirationTime=0;b.memoizedState=ii;b.child=e;return c}b.memoizedState=null;return b.child=Xg(b,a,e.children,c)}
function ki(a,b){a.expirationTime<b&&(a.expirationTime=b);var c=a.alternate;null!==c&&c.expirationTime<b&&(c.expirationTime=b);pg(a.return,b)}function li(a,b,c,d,e,f){var g=a.memoizedState;null===g?a.memoizedState={isBackwards:b,rendering:null,renderingStartTime:0,last:d,tail:c,tailExpiration:0,tailMode:e,lastEffect:f}:(g.isBackwards=b,g.rendering=null,g.renderingStartTime=0,g.last=d,g.tail=c,g.tailExpiration=0,g.tailMode=e,g.lastEffect=f)}
function mi(a,b,c){var d=b.pendingProps,e=d.revealOrder,f=d.tail;R(a,b,d.children,c);d=M.current;if(0!==(d&2))d=d&1|2,b.effectTag|=64;else{if(null!==a&&0!==(a.effectTag&64))a:for(a=b.child;null!==a;){if(13===a.tag)null!==a.memoizedState&&ki(a,c);else if(19===a.tag)ki(a,c);else if(null!==a.child){a.child.return=a;a=a.child;continue}if(a===b)break a;for(;null===a.sibling;){if(null===a.return||a.return===b)break a;a=a.return}a.sibling.return=a.return;a=a.sibling}d&=1}I(M,d);if(0===(b.mode&2))b.memoizedState=
null;else switch(e){case "forwards":c=b.child;for(e=null;null!==c;)a=c.alternate,null!==a&&null===hh(a)&&(e=c),c=c.sibling;c=e;null===c?(e=b.child,b.child=null):(e=c.sibling,c.sibling=null);li(b,!1,e,c,f,b.lastEffect);break;case "backwards":c=null;e=b.child;for(b.child=null;null!==e;){a=e.alternate;if(null!==a&&null===hh(a)){b.child=e;break}a=e.sibling;e.sibling=c;c=e;e=a}li(b,!0,c,null,f,b.lastEffect);break;case "together":li(b,!1,null,null,void 0,b.lastEffect);break;default:b.memoizedState=null}return b.child}
function $h(a,b,c){null!==a&&(b.dependencies=a.dependencies);var d=b.expirationTime;0!==d&&Bg(d);if(b.childExpirationTime<c)return null;if(null!==a&&b.child!==a.child)throw Error(u(153));if(null!==b.child){a=b.child;c=Sg(a,a.pendingProps);b.child=c;for(c.return=b;null!==a.sibling;)a=a.sibling,c=c.sibling=Sg(a,a.pendingProps),c.return=b;c.sibling=null}return b.child}var ni,oi,pi,qi;
ni=function(a,b){for(var c=b.child;null!==c;){if(5===c.tag||6===c.tag)a.appendChild(c.stateNode);else if(4!==c.tag&&null!==c.child){c.child.return=c;c=c.child;continue}if(c===b)break;for(;null===c.sibling;){if(null===c.return||c.return===b)return;c=c.return}c.sibling.return=c.return;c=c.sibling}};oi=function(){};
pi=function(a,b,c,d,e){var f=a.memoizedProps;if(f!==d){var g=b.stateNode;ch($g.current);a=null;switch(c){case "input":f=zb(g,f);d=zb(g,d);a=[];break;case "option":f=Gb(g,f);d=Gb(g,d);a=[];break;case "select":f=n({},f,{value:void 0});d=n({},d,{value:void 0});a=[];break;case "textarea":f=Ib(g,f);d=Ib(g,d);a=[];break;default:"function"!==typeof f.onClick&&"function"===typeof d.onClick&&(g.onclick=sd)}od(c,d);var h,k;c=null;for(h in f)if(!d.hasOwnProperty(h)&&f.hasOwnProperty(h)&&null!=f[h])if("style"===
h)for(k in g=f[h],g)g.hasOwnProperty(k)&&(c||(c={}),c[k]="");else"dangerouslySetInnerHTML"!==h&&"children"!==h&&"suppressContentEditableWarning"!==h&&"suppressHydrationWarning"!==h&&"autoFocus"!==h&&(va.hasOwnProperty(h)?a||(a=[]):(a=a||[]).push(h,null));for(h in d){var l=d[h];g=null!=f?f[h]:void 0;if(d.hasOwnProperty(h)&&l!==g&&(null!=l||null!=g))if("style"===h)if(g){for(k in g)!g.hasOwnProperty(k)||l&&l.hasOwnProperty(k)||(c||(c={}),c[k]="");for(k in l)l.hasOwnProperty(k)&&g[k]!==l[k]&&(c||(c={}),
c[k]=l[k])}else c||(a||(a=[]),a.push(h,c)),c=l;else"dangerouslySetInnerHTML"===h?(l=l?l.__html:void 0,g=g?g.__html:void 0,null!=l&&g!==l&&(a=a||[]).push(h,l)):"children"===h?g===l||"string"!==typeof l&&"number"!==typeof l||(a=a||[]).push(h,""+l):"suppressContentEditableWarning"!==h&&"suppressHydrationWarning"!==h&&(va.hasOwnProperty(h)?(null!=l&&rd(e,h),a||g===l||(a=[])):(a=a||[]).push(h,l))}c&&(a=a||[]).push("style",c);e=a;if(b.updateQueue=e)b.effectTag|=4}};
qi=function(a,b,c,d){c!==d&&(b.effectTag|=4)};function ri(a,b){switch(a.tailMode){case "hidden":b=a.tail;for(var c=null;null!==b;)null!==b.alternate&&(c=b),b=b.sibling;null===c?a.tail=null:c.sibling=null;break;case "collapsed":c=a.tail;for(var d=null;null!==c;)null!==c.alternate&&(d=c),c=c.sibling;null===d?b||null===a.tail?a.tail=null:a.tail.sibling=null:d.sibling=null}}
function si(a,b,c){var d=b.pendingProps;switch(b.tag){case 2:case 16:case 15:case 0:case 11:case 7:case 8:case 12:case 9:case 14:return null;case 1:return L(b.type)&&Df(),null;case 3:return eh(),H(K),H(J),c=b.stateNode,c.pendingContext&&(c.context=c.pendingContext,c.pendingContext=null),null!==a&&null!==a.child||!Wh(b)||(b.effectTag|=4),oi(b),null;case 5:gh(b);c=ch(bh.current);var e=b.type;if(null!==a&&null!=b.stateNode)pi(a,b,e,d,c),a.ref!==b.ref&&(b.effectTag|=128);else{if(!d){if(null===b.stateNode)throw Error(u(166));
return null}a=ch($g.current);if(Wh(b)){d=b.stateNode;e=b.type;var f=b.memoizedProps;d[Md]=b;d[Nd]=f;switch(e){case "iframe":case "object":case "embed":F("load",d);break;case "video":case "audio":for(a=0;a<ac.length;a++)F(ac[a],d);break;case "source":F("error",d);break;case "img":case "image":case "link":F("error",d);F("load",d);break;case "form":F("reset",d);F("submit",d);break;case "details":F("toggle",d);break;case "input":Ab(d,f);F("invalid",d);rd(c,"onChange");break;case "select":d._wrapperState=
{wasMultiple:!!f.multiple};F("invalid",d);rd(c,"onChange");break;case "textarea":Jb(d,f),F("invalid",d),rd(c,"onChange")}od(e,f);a=null;for(var g in f)if(f.hasOwnProperty(g)){var h=f[g];"children"===g?"string"===typeof h?d.textContent!==h&&(a=["children",h]):"number"===typeof h&&d.textContent!==""+h&&(a=["children",""+h]):va.hasOwnProperty(g)&&null!=h&&rd(c,g)}switch(e){case "input":xb(d);Eb(d,f,!0);break;case "textarea":xb(d);Lb(d);break;case "select":case "option":break;default:"function"===typeof f.onClick&&
(d.onclick=sd)}c=a;b.updateQueue=c;null!==c&&(b.effectTag|=4)}else{g=9===c.nodeType?c:c.ownerDocument;a===qd&&(a=Nb(e));a===qd?"script"===e?(a=g.createElement("div"),a.innerHTML="<script>\x3c/script>",a=a.removeChild(a.firstChild)):"string"===typeof d.is?a=g.createElement(e,{is:d.is}):(a=g.createElement(e),"select"===e&&(g=a,d.multiple?g.multiple=!0:d.size&&(g.size=d.size))):a=g.createElementNS(a,e);a[Md]=b;a[Nd]=d;ni(a,b,!1,!1);b.stateNode=a;g=pd(e,d);switch(e){case "iframe":case "object":case "embed":F("load",
a);h=d;break;case "video":case "audio":for(h=0;h<ac.length;h++)F(ac[h],a);h=d;break;case "source":F("error",a);h=d;break;case "img":case "image":case "link":F("error",a);F("load",a);h=d;break;case "form":F("reset",a);F("submit",a);h=d;break;case "details":F("toggle",a);h=d;break;case "input":Ab(a,d);h=zb(a,d);F("invalid",a);rd(c,"onChange");break;case "option":h=Gb(a,d);break;case "select":a._wrapperState={wasMultiple:!!d.multiple};h=n({},d,{value:void 0});F("invalid",a);rd(c,"onChange");break;case "textarea":Jb(a,
d);h=Ib(a,d);F("invalid",a);rd(c,"onChange");break;default:h=d}od(e,h);var k=h;for(f in k)if(k.hasOwnProperty(f)){var l=k[f];"style"===f?md(a,l):"dangerouslySetInnerHTML"===f?(l=l?l.__html:void 0,null!=l&&Qb(a,l)):"children"===f?"string"===typeof l?("textarea"!==e||""!==l)&&Rb(a,l):"number"===typeof l&&Rb(a,""+l):"suppressContentEditableWarning"!==f&&"suppressHydrationWarning"!==f&&"autoFocus"!==f&&(va.hasOwnProperty(f)?null!=l&&rd(c,f):null!=l&&Xa(a,f,l,g))}switch(e){case "input":xb(a);Eb(a,d,!1);
break;case "textarea":xb(a);Lb(a);break;case "option":null!=d.value&&a.setAttribute("value",""+rb(d.value));break;case "select":a.multiple=!!d.multiple;c=d.value;null!=c?Hb(a,!!d.multiple,c,!1):null!=d.defaultValue&&Hb(a,!!d.multiple,d.defaultValue,!0);break;default:"function"===typeof h.onClick&&(a.onclick=sd)}Fd(e,d)&&(b.effectTag|=4)}null!==b.ref&&(b.effectTag|=128)}return null;case 6:if(a&&null!=b.stateNode)qi(a,b,a.memoizedProps,d);else{if("string"!==typeof d&&null===b.stateNode)throw Error(u(166));
c=ch(bh.current);ch($g.current);Wh(b)?(c=b.stateNode,d=b.memoizedProps,c[Md]=b,c.nodeValue!==d&&(b.effectTag|=4)):(c=(9===c.nodeType?c:c.ownerDocument).createTextNode(d),c[Md]=b,b.stateNode=c)}return null;case 13:H(M);d=b.memoizedState;if(0!==(b.effectTag&64))return b.expirationTime=c,b;c=null!==d;d=!1;null===a?void 0!==b.memoizedProps.fallback&&Wh(b):(e=a.memoizedState,d=null!==e,c||null===e||(e=a.child.sibling,null!==e&&(f=b.firstEffect,null!==f?(b.firstEffect=e,e.nextEffect=f):(b.firstEffect=b.lastEffect=
e,e.nextEffect=null),e.effectTag=8)));if(c&&!d&&0!==(b.mode&2))if(null===a&&!0!==b.memoizedProps.unstable_avoidThisFallback||0!==(M.current&1))S===ti&&(S=ui);else{if(S===ti||S===ui)S=vi;0!==wi&&null!==T&&(xi(T,U),yi(T,wi))}if(c||d)b.effectTag|=4;return null;case 4:return eh(),oi(b),null;case 10:return og(b),null;case 17:return L(b.type)&&Df(),null;case 19:H(M);d=b.memoizedState;if(null===d)return null;e=0!==(b.effectTag&64);f=d.rendering;if(null===f)if(e)ri(d,!1);else{if(S!==ti||null!==a&&0!==(a.effectTag&
64))for(f=b.child;null!==f;){a=hh(f);if(null!==a){b.effectTag|=64;ri(d,!1);e=a.updateQueue;null!==e&&(b.updateQueue=e,b.effectTag|=4);null===d.lastEffect&&(b.firstEffect=null);b.lastEffect=d.lastEffect;for(d=b.child;null!==d;)e=d,f=c,e.effectTag&=2,e.nextEffect=null,e.firstEffect=null,e.lastEffect=null,a=e.alternate,null===a?(e.childExpirationTime=0,e.expirationTime=f,e.child=null,e.memoizedProps=null,e.memoizedState=null,e.updateQueue=null,e.dependencies=null):(e.childExpirationTime=a.childExpirationTime,
e.expirationTime=a.expirationTime,e.child=a.child,e.memoizedProps=a.memoizedProps,e.memoizedState=a.memoizedState,e.updateQueue=a.updateQueue,f=a.dependencies,e.dependencies=null===f?null:{expirationTime:f.expirationTime,firstContext:f.firstContext,responders:f.responders}),d=d.sibling;I(M,M.current&1|2);return b.child}f=f.sibling}}else{if(!e)if(a=hh(f),null!==a){if(b.effectTag|=64,e=!0,c=a.updateQueue,null!==c&&(b.updateQueue=c,b.effectTag|=4),ri(d,!0),null===d.tail&&"hidden"===d.tailMode&&!f.alternate)return b=
b.lastEffect=d.lastEffect,null!==b&&(b.nextEffect=null),null}else 2*$f()-d.renderingStartTime>d.tailExpiration&&1<c&&(b.effectTag|=64,e=!0,ri(d,!1),b.expirationTime=b.childExpirationTime=c-1);d.isBackwards?(f.sibling=b.child,b.child=f):(c=d.last,null!==c?c.sibling=f:b.child=f,d.last=f)}return null!==d.tail?(0===d.tailExpiration&&(d.tailExpiration=$f()+500),c=d.tail,d.rendering=c,d.tail=c.sibling,d.lastEffect=b.lastEffect,d.renderingStartTime=$f(),c.sibling=null,b=M.current,I(M,e?b&1|2:b&1),c):null}throw Error(u(156,
b.tag));}function zi(a){switch(a.tag){case 1:L(a.type)&&Df();var b=a.effectTag;return b&4096?(a.effectTag=b&-4097|64,a):null;case 3:eh();H(K);H(J);b=a.effectTag;if(0!==(b&64))throw Error(u(285));a.effectTag=b&-4097|64;return a;case 5:return gh(a),null;case 13:return H(M),b=a.effectTag,b&4096?(a.effectTag=b&-4097|64,a):null;case 19:return H(M),null;case 4:return eh(),null;case 10:return og(a),null;default:return null}}function Ai(a,b){return{value:a,source:b,stack:qb(b)}}
var Bi="function"===typeof WeakSet?WeakSet:Set;function Ci(a,b){var c=b.source,d=b.stack;null===d&&null!==c&&(d=qb(c));null!==c&&pb(c.type);b=b.value;null!==a&&1===a.tag&&pb(a.type);try{console.error(b)}catch(e){setTimeout(function(){throw e;})}}function Di(a,b){try{b.props=a.memoizedProps,b.state=a.memoizedState,b.componentWillUnmount()}catch(c){Ei(a,c)}}function Fi(a){var b=a.ref;if(null!==b)if("function"===typeof b)try{b(null)}catch(c){Ei(a,c)}else b.current=null}
function Gi(a,b){switch(b.tag){case 0:case 11:case 15:case 22:return;case 1:if(b.effectTag&256&&null!==a){var c=a.memoizedProps,d=a.memoizedState;a=b.stateNode;b=a.getSnapshotBeforeUpdate(b.elementType===b.type?c:ig(b.type,c),d);a.__reactInternalSnapshotBeforeUpdate=b}return;case 3:case 5:case 6:case 4:case 17:return}throw Error(u(163));}
function Hi(a,b){b=b.updateQueue;b=null!==b?b.lastEffect:null;if(null!==b){var c=b=b.next;do{if((c.tag&a)===a){var d=c.destroy;c.destroy=void 0;void 0!==d&&d()}c=c.next}while(c!==b)}}function Ii(a,b){b=b.updateQueue;b=null!==b?b.lastEffect:null;if(null!==b){var c=b=b.next;do{if((c.tag&a)===a){var d=c.create;c.destroy=d()}c=c.next}while(c!==b)}}
function Ji(a,b,c){switch(c.tag){case 0:case 11:case 15:case 22:Ii(3,c);return;case 1:a=c.stateNode;if(c.effectTag&4)if(null===b)a.componentDidMount();else{var d=c.elementType===c.type?b.memoizedProps:ig(c.type,b.memoizedProps);a.componentDidUpdate(d,b.memoizedState,a.__reactInternalSnapshotBeforeUpdate)}b=c.updateQueue;null!==b&&Cg(c,b,a);return;case 3:b=c.updateQueue;if(null!==b){a=null;if(null!==c.child)switch(c.child.tag){case 5:a=c.child.stateNode;break;case 1:a=c.child.stateNode}Cg(c,b,a)}return;
case 5:a=c.stateNode;null===b&&c.effectTag&4&&Fd(c.type,c.memoizedProps)&&a.focus();return;case 6:return;case 4:return;case 12:return;case 13:null===c.memoizedState&&(c=c.alternate,null!==c&&(c=c.memoizedState,null!==c&&(c=c.dehydrated,null!==c&&Vc(c))));return;case 19:case 17:case 20:case 21:return}throw Error(u(163));}
function Ki(a,b,c){"function"===typeof Li&&Li(b);switch(b.tag){case 0:case 11:case 14:case 15:case 22:a=b.updateQueue;if(null!==a&&(a=a.lastEffect,null!==a)){var d=a.next;cg(97<c?97:c,function(){var a=d;do{var c=a.destroy;if(void 0!==c){var g=b;try{c()}catch(h){Ei(g,h)}}a=a.next}while(a!==d)})}break;case 1:Fi(b);c=b.stateNode;"function"===typeof c.componentWillUnmount&&Di(b,c);break;case 5:Fi(b);break;case 4:Mi(a,b,c)}}
function Ni(a){var b=a.alternate;a.return=null;a.child=null;a.memoizedState=null;a.updateQueue=null;a.dependencies=null;a.alternate=null;a.firstEffect=null;a.lastEffect=null;a.pendingProps=null;a.memoizedProps=null;a.stateNode=null;null!==b&&Ni(b)}function Oi(a){return 5===a.tag||3===a.tag||4===a.tag}
function Pi(a){a:{for(var b=a.return;null!==b;){if(Oi(b)){var c=b;break a}b=b.return}throw Error(u(160));}b=c.stateNode;switch(c.tag){case 5:var d=!1;break;case 3:b=b.containerInfo;d=!0;break;case 4:b=b.containerInfo;d=!0;break;default:throw Error(u(161));}c.effectTag&16&&(Rb(b,""),c.effectTag&=-17);a:b:for(c=a;;){for(;null===c.sibling;){if(null===c.return||Oi(c.return)){c=null;break a}c=c.return}c.sibling.return=c.return;for(c=c.sibling;5!==c.tag&&6!==c.tag&&18!==c.tag;){if(c.effectTag&2)continue b;
if(null===c.child||4===c.tag)continue b;else c.child.return=c,c=c.child}if(!(c.effectTag&2)){c=c.stateNode;break a}}d?Qi(a,c,b):Ri(a,c,b)}
function Qi(a,b,c){var d=a.tag,e=5===d||6===d;if(e)a=e?a.stateNode:a.stateNode.instance,b?8===c.nodeType?c.parentNode.insertBefore(a,b):c.insertBefore(a,b):(8===c.nodeType?(b=c.parentNode,b.insertBefore(a,c)):(b=c,b.appendChild(a)),c=c._reactRootContainer,null!==c&&void 0!==c||null!==b.onclick||(b.onclick=sd));else if(4!==d&&(a=a.child,null!==a))for(Qi(a,b,c),a=a.sibling;null!==a;)Qi(a,b,c),a=a.sibling}
function Ri(a,b,c){var d=a.tag,e=5===d||6===d;if(e)a=e?a.stateNode:a.stateNode.instance,b?c.insertBefore(a,b):c.appendChild(a);else if(4!==d&&(a=a.child,null!==a))for(Ri(a,b,c),a=a.sibling;null!==a;)Ri(a,b,c),a=a.sibling}
function Mi(a,b,c){for(var d=b,e=!1,f,g;;){if(!e){e=d.return;a:for(;;){if(null===e)throw Error(u(160));f=e.stateNode;switch(e.tag){case 5:g=!1;break a;case 3:f=f.containerInfo;g=!0;break a;case 4:f=f.containerInfo;g=!0;break a}e=e.return}e=!0}if(5===d.tag||6===d.tag){a:for(var h=a,k=d,l=c,m=k;;)if(Ki(h,m,l),null!==m.child&&4!==m.tag)m.child.return=m,m=m.child;else{if(m===k)break a;for(;null===m.sibling;){if(null===m.return||m.return===k)break a;m=m.return}m.sibling.return=m.return;m=m.sibling}g?(h=
f,k=d.stateNode,8===h.nodeType?h.parentNode.removeChild(k):h.removeChild(k)):f.removeChild(d.stateNode)}else if(4===d.tag){if(null!==d.child){f=d.stateNode.containerInfo;g=!0;d.child.return=d;d=d.child;continue}}else if(Ki(a,d,c),null!==d.child){d.child.return=d;d=d.child;continue}if(d===b)break;for(;null===d.sibling;){if(null===d.return||d.return===b)return;d=d.return;4===d.tag&&(e=!1)}d.sibling.return=d.return;d=d.sibling}}
function Si(a,b){switch(b.tag){case 0:case 11:case 14:case 15:case 22:Hi(3,b);return;case 1:return;case 5:var c=b.stateNode;if(null!=c){var d=b.memoizedProps,e=null!==a?a.memoizedProps:d;a=b.type;var f=b.updateQueue;b.updateQueue=null;if(null!==f){c[Nd]=d;"input"===a&&"radio"===d.type&&null!=d.name&&Bb(c,d);pd(a,e);b=pd(a,d);for(e=0;e<f.length;e+=2){var g=f[e],h=f[e+1];"style"===g?md(c,h):"dangerouslySetInnerHTML"===g?Qb(c,h):"children"===g?Rb(c,h):Xa(c,g,h,b)}switch(a){case "input":Cb(c,d);break;
case "textarea":Kb(c,d);break;case "select":b=c._wrapperState.wasMultiple,c._wrapperState.wasMultiple=!!d.multiple,a=d.value,null!=a?Hb(c,!!d.multiple,a,!1):b!==!!d.multiple&&(null!=d.defaultValue?Hb(c,!!d.multiple,d.defaultValue,!0):Hb(c,!!d.multiple,d.multiple?[]:"",!1))}}}return;case 6:if(null===b.stateNode)throw Error(u(162));b.stateNode.nodeValue=b.memoizedProps;return;case 3:b=b.stateNode;b.hydrate&&(b.hydrate=!1,Vc(b.containerInfo));return;case 12:return;case 13:c=b;null===b.memoizedState?
d=!1:(d=!0,c=b.child,Ti=$f());if(null!==c)a:for(a=c;;){if(5===a.tag)f=a.stateNode,d?(f=f.style,"function"===typeof f.setProperty?f.setProperty("display","none","important"):f.display="none"):(f=a.stateNode,e=a.memoizedProps.style,e=void 0!==e&&null!==e&&e.hasOwnProperty("display")?e.display:null,f.style.display=ld("display",e));else if(6===a.tag)a.stateNode.nodeValue=d?"":a.memoizedProps;else if(13===a.tag&&null!==a.memoizedState&&null===a.memoizedState.dehydrated){f=a.child.sibling;f.return=a;a=
f;continue}else if(null!==a.child){a.child.return=a;a=a.child;continue}if(a===c)break;for(;null===a.sibling;){if(null===a.return||a.return===c)break a;a=a.return}a.sibling.return=a.return;a=a.sibling}Ui(b);return;case 19:Ui(b);return;case 17:return}throw Error(u(163));}function Ui(a){var b=a.updateQueue;if(null!==b){a.updateQueue=null;var c=a.stateNode;null===c&&(c=a.stateNode=new Bi);b.forEach(function(b){var d=Vi.bind(null,a,b);c.has(b)||(c.add(b),b.then(d,d))})}}
var Wi="function"===typeof WeakMap?WeakMap:Map;function Xi(a,b,c){c=wg(c,null);c.tag=3;c.payload={element:null};var d=b.value;c.callback=function(){Yi||(Yi=!0,Zi=d);Ci(a,b)};return c}
function $i(a,b,c){c=wg(c,null);c.tag=3;var d=a.type.getDerivedStateFromError;if("function"===typeof d){var e=b.value;c.payload=function(){Ci(a,b);return d(e)}}var f=a.stateNode;null!==f&&"function"===typeof f.componentDidCatch&&(c.callback=function(){"function"!==typeof d&&(null===aj?aj=new Set([this]):aj.add(this),Ci(a,b));var c=b.stack;this.componentDidCatch(b.value,{componentStack:null!==c?c:""})});return c}
var bj=Math.ceil,cj=Wa.ReactCurrentDispatcher,dj=Wa.ReactCurrentOwner,V=0,ej=8,fj=16,gj=32,ti=0,hj=1,ij=2,ui=3,vi=4,jj=5,W=V,T=null,X=null,U=0,S=ti,kj=null,lj=1073741823,mj=1073741823,nj=null,wi=0,oj=!1,Ti=0,pj=500,Y=null,Yi=!1,Zi=null,aj=null,qj=!1,rj=null,sj=90,tj=null,uj=0,vj=null,wj=0;function Gg(){return(W&(fj|gj))!==V?1073741821-($f()/10|0):0!==wj?wj:wj=1073741821-($f()/10|0)}
function Hg(a,b,c){b=b.mode;if(0===(b&2))return 1073741823;var d=ag();if(0===(b&4))return 99===d?1073741823:1073741822;if((W&fj)!==V)return U;if(null!==c)a=hg(a,c.timeoutMs|0||5E3,250);else switch(d){case 99:a=1073741823;break;case 98:a=hg(a,150,100);break;case 97:case 96:a=hg(a,5E3,250);break;case 95:a=2;break;default:throw Error(u(326));}null!==T&&a===U&&--a;return a}
function Ig(a,b){if(50<uj)throw uj=0,vj=null,Error(u(185));a=xj(a,b);if(null!==a){var c=ag();1073741823===b?(W&ej)!==V&&(W&(fj|gj))===V?yj(a):(Z(a),W===V&&gg()):Z(a);(W&4)===V||98!==c&&99!==c||(null===tj?tj=new Map([[a,b]]):(c=tj.get(a),(void 0===c||c>b)&&tj.set(a,b)))}}
function xj(a,b){a.expirationTime<b&&(a.expirationTime=b);var c=a.alternate;null!==c&&c.expirationTime<b&&(c.expirationTime=b);var d=a.return,e=null;if(null===d&&3===a.tag)e=a.stateNode;else for(;null!==d;){c=d.alternate;d.childExpirationTime<b&&(d.childExpirationTime=b);null!==c&&c.childExpirationTime<b&&(c.childExpirationTime=b);if(null===d.return&&3===d.tag){e=d.stateNode;break}d=d.return}null!==e&&(T===e&&(Bg(b),S===vi&&xi(e,U)),yi(e,b));return e}
function zj(a){var b=a.lastExpiredTime;if(0!==b)return b;b=a.firstPendingTime;if(!Aj(a,b))return b;var c=a.lastPingedTime;a=a.nextKnownPendingLevel;a=c>a?c:a;return 2>=a&&b!==a?0:a}
function Z(a){if(0!==a.lastExpiredTime)a.callbackExpirationTime=1073741823,a.callbackPriority=99,a.callbackNode=eg(yj.bind(null,a));else{var b=zj(a),c=a.callbackNode;if(0===b)null!==c&&(a.callbackNode=null,a.callbackExpirationTime=0,a.callbackPriority=90);else{var d=Gg();1073741823===b?d=99:1===b||2===b?d=95:(d=10*(1073741821-b)-10*(1073741821-d),d=0>=d?99:250>=d?98:5250>=d?97:95);if(null!==c){var e=a.callbackPriority;if(a.callbackExpirationTime===b&&e>=d)return;c!==Tf&&Kf(c)}a.callbackExpirationTime=
b;a.callbackPriority=d;b=1073741823===b?eg(yj.bind(null,a)):dg(d,Bj.bind(null,a),{timeout:10*(1073741821-b)-$f()});a.callbackNode=b}}}
function Bj(a,b){wj=0;if(b)return b=Gg(),Cj(a,b),Z(a),null;var c=zj(a);if(0!==c){b=a.callbackNode;if((W&(fj|gj))!==V)throw Error(u(327));Dj();a===T&&c===U||Ej(a,c);if(null!==X){var d=W;W|=fj;var e=Fj();do try{Gj();break}catch(h){Hj(a,h)}while(1);ng();W=d;cj.current=e;if(S===hj)throw b=kj,Ej(a,c),xi(a,c),Z(a),b;if(null===X)switch(e=a.finishedWork=a.current.alternate,a.finishedExpirationTime=c,d=S,T=null,d){case ti:case hj:throw Error(u(345));case ij:Cj(a,2<c?2:c);break;case ui:xi(a,c);d=a.lastSuspendedTime;
c===d&&(a.nextKnownPendingLevel=Ij(e));if(1073741823===lj&&(e=Ti+pj-$f(),10<e)){if(oj){var f=a.lastPingedTime;if(0===f||f>=c){a.lastPingedTime=c;Ej(a,c);break}}f=zj(a);if(0!==f&&f!==c)break;if(0!==d&&d!==c){a.lastPingedTime=d;break}a.timeoutHandle=Hd(Jj.bind(null,a),e);break}Jj(a);break;case vi:xi(a,c);d=a.lastSuspendedTime;c===d&&(a.nextKnownPendingLevel=Ij(e));if(oj&&(e=a.lastPingedTime,0===e||e>=c)){a.lastPingedTime=c;Ej(a,c);break}e=zj(a);if(0!==e&&e!==c)break;if(0!==d&&d!==c){a.lastPingedTime=
d;break}1073741823!==mj?d=10*(1073741821-mj)-$f():1073741823===lj?d=0:(d=10*(1073741821-lj)-5E3,e=$f(),c=10*(1073741821-c)-e,d=e-d,0>d&&(d=0),d=(120>d?120:480>d?480:1080>d?1080:1920>d?1920:3E3>d?3E3:4320>d?4320:1960*bj(d/1960))-d,c<d&&(d=c));if(10<d){a.timeoutHandle=Hd(Jj.bind(null,a),d);break}Jj(a);break;case jj:if(1073741823!==lj&&null!==nj){f=lj;var g=nj;d=g.busyMinDurationMs|0;0>=d?d=0:(e=g.busyDelayMs|0,f=$f()-(10*(1073741821-f)-(g.timeoutMs|0||5E3)),d=f<=e?0:e+d-f);if(10<d){xi(a,c);a.timeoutHandle=
Hd(Jj.bind(null,a),d);break}}Jj(a);break;default:throw Error(u(329));}Z(a);if(a.callbackNode===b)return Bj.bind(null,a)}}return null}
function yj(a){var b=a.lastExpiredTime;b=0!==b?b:1073741823;if((W&(fj|gj))!==V)throw Error(u(327));Dj();a===T&&b===U||Ej(a,b);if(null!==X){var c=W;W|=fj;var d=Fj();do try{Kj();break}catch(e){Hj(a,e)}while(1);ng();W=c;cj.current=d;if(S===hj)throw c=kj,Ej(a,b),xi(a,b),Z(a),c;if(null!==X)throw Error(u(261));a.finishedWork=a.current.alternate;a.finishedExpirationTime=b;T=null;Jj(a);Z(a)}return null}function Lj(){if(null!==tj){var a=tj;tj=null;a.forEach(function(a,c){Cj(c,a);Z(c)});gg()}}
function Mj(a,b){var c=W;W|=1;try{return a(b)}finally{W=c,W===V&&gg()}}function Nj(a,b){var c=W;W&=-2;W|=ej;try{return a(b)}finally{W=c,W===V&&gg()}}
function Ej(a,b){a.finishedWork=null;a.finishedExpirationTime=0;var c=a.timeoutHandle;-1!==c&&(a.timeoutHandle=-1,Id(c));if(null!==X)for(c=X.return;null!==c;){var d=c;switch(d.tag){case 1:d=d.type.childContextTypes;null!==d&&void 0!==d&&Df();break;case 3:eh();H(K);H(J);break;case 5:gh(d);break;case 4:eh();break;case 13:H(M);break;case 19:H(M);break;case 10:og(d)}c=c.return}T=a;X=Sg(a.current,null);U=b;S=ti;kj=null;mj=lj=1073741823;nj=null;wi=0;oj=!1}
function Hj(a,b){do{try{ng();jh.current=sh;if(mh)for(var c=N.memoizedState;null!==c;){var d=c.queue;null!==d&&(d.pending=null);c=c.next}lh=0;P=O=N=null;mh=!1;if(null===X||null===X.return)return S=hj,kj=b,X=null;a:{var e=a,f=X.return,g=X,h=b;b=U;g.effectTag|=2048;g.firstEffect=g.lastEffect=null;if(null!==h&&"object"===typeof h&&"function"===typeof h.then){var k=h;if(0===(g.mode&2)){var l=g.alternate;l?(g.updateQueue=l.updateQueue,g.memoizedState=l.memoizedState,g.expirationTime=l.expirationTime):(g.updateQueue=
null,g.memoizedState=null)}var m=0!==(M.current&1),p=f;do{var x;if(x=13===p.tag){var z=p.memoizedState;if(null!==z)x=null!==z.dehydrated?!0:!1;else{var ca=p.memoizedProps;x=void 0===ca.fallback?!1:!0!==ca.unstable_avoidThisFallback?!0:m?!1:!0}}if(x){var D=p.updateQueue;if(null===D){var t=new Set;t.add(k);p.updateQueue=t}else D.add(k);if(0===(p.mode&2)){p.effectTag|=64;g.effectTag&=-2981;if(1===g.tag)if(null===g.alternate)g.tag=17;else{var y=wg(1073741823,null);y.tag=2;xg(g,y)}g.expirationTime=1073741823;
break a}h=void 0;g=b;var A=e.pingCache;null===A?(A=e.pingCache=new Wi,h=new Set,A.set(k,h)):(h=A.get(k),void 0===h&&(h=new Set,A.set(k,h)));if(!h.has(g)){h.add(g);var q=Oj.bind(null,e,k,g);k.then(q,q)}p.effectTag|=4096;p.expirationTime=b;break a}p=p.return}while(null!==p);h=Error((pb(g.type)||"A React component")+" suspended while rendering, but no fallback UI was specified.\n\nAdd a <Suspense fallback=...> component higher in the tree to provide a loading indicator or placeholder to display."+qb(g))}S!==
jj&&(S=ij);h=Ai(h,g);p=f;do{switch(p.tag){case 3:k=h;p.effectTag|=4096;p.expirationTime=b;var B=Xi(p,k,b);yg(p,B);break a;case 1:k=h;var w=p.type,ub=p.stateNode;if(0===(p.effectTag&64)&&("function"===typeof w.getDerivedStateFromError||null!==ub&&"function"===typeof ub.componentDidCatch&&(null===aj||!aj.has(ub)))){p.effectTag|=4096;p.expirationTime=b;var vb=$i(p,k,b);yg(p,vb);break a}}p=p.return}while(null!==p)}X=Pj(X)}catch(Xc){b=Xc;continue}break}while(1)}
function Fj(){var a=cj.current;cj.current=sh;return null===a?sh:a}function Ag(a,b){a<lj&&2<a&&(lj=a);null!==b&&a<mj&&2<a&&(mj=a,nj=b)}function Bg(a){a>wi&&(wi=a)}function Kj(){for(;null!==X;)X=Qj(X)}function Gj(){for(;null!==X&&!Uf();)X=Qj(X)}function Qj(a){var b=Rj(a.alternate,a,U);a.memoizedProps=a.pendingProps;null===b&&(b=Pj(a));dj.current=null;return b}
function Pj(a){X=a;do{var b=X.alternate;a=X.return;if(0===(X.effectTag&2048)){b=si(b,X,U);if(1===U||1!==X.childExpirationTime){for(var c=0,d=X.child;null!==d;){var e=d.expirationTime,f=d.childExpirationTime;e>c&&(c=e);f>c&&(c=f);d=d.sibling}X.childExpirationTime=c}if(null!==b)return b;null!==a&&0===(a.effectTag&2048)&&(null===a.firstEffect&&(a.firstEffect=X.firstEffect),null!==X.lastEffect&&(null!==a.lastEffect&&(a.lastEffect.nextEffect=X.firstEffect),a.lastEffect=X.lastEffect),1<X.effectTag&&(null!==
a.lastEffect?a.lastEffect.nextEffect=X:a.firstEffect=X,a.lastEffect=X))}else{b=zi(X);if(null!==b)return b.effectTag&=2047,b;null!==a&&(a.firstEffect=a.lastEffect=null,a.effectTag|=2048)}b=X.sibling;if(null!==b)return b;X=a}while(null!==X);S===ti&&(S=jj);return null}function Ij(a){var b=a.expirationTime;a=a.childExpirationTime;return b>a?b:a}function Jj(a){var b=ag();cg(99,Sj.bind(null,a,b));return null}
function Sj(a,b){do Dj();while(null!==rj);if((W&(fj|gj))!==V)throw Error(u(327));var c=a.finishedWork,d=a.finishedExpirationTime;if(null===c)return null;a.finishedWork=null;a.finishedExpirationTime=0;if(c===a.current)throw Error(u(177));a.callbackNode=null;a.callbackExpirationTime=0;a.callbackPriority=90;a.nextKnownPendingLevel=0;var e=Ij(c);a.firstPendingTime=e;d<=a.lastSuspendedTime?a.firstSuspendedTime=a.lastSuspendedTime=a.nextKnownPendingLevel=0:d<=a.firstSuspendedTime&&(a.firstSuspendedTime=
d-1);d<=a.lastPingedTime&&(a.lastPingedTime=0);d<=a.lastExpiredTime&&(a.lastExpiredTime=0);a===T&&(X=T=null,U=0);1<c.effectTag?null!==c.lastEffect?(c.lastEffect.nextEffect=c,e=c.firstEffect):e=c:e=c.firstEffect;if(null!==e){var f=W;W|=gj;dj.current=null;Dd=fd;var g=xd();if(yd(g)){if("selectionStart"in g)var h={start:g.selectionStart,end:g.selectionEnd};else a:{h=(h=g.ownerDocument)&&h.defaultView||window;var k=h.getSelection&&h.getSelection();if(k&&0!==k.rangeCount){h=k.anchorNode;var l=k.anchorOffset,
m=k.focusNode;k=k.focusOffset;try{h.nodeType,m.nodeType}catch(wb){h=null;break a}var p=0,x=-1,z=-1,ca=0,D=0,t=g,y=null;b:for(;;){for(var A;;){t!==h||0!==l&&3!==t.nodeType||(x=p+l);t!==m||0!==k&&3!==t.nodeType||(z=p+k);3===t.nodeType&&(p+=t.nodeValue.length);if(null===(A=t.firstChild))break;y=t;t=A}for(;;){if(t===g)break b;y===h&&++ca===l&&(x=p);y===m&&++D===k&&(z=p);if(null!==(A=t.nextSibling))break;t=y;y=t.parentNode}t=A}h=-1===x||-1===z?null:{start:x,end:z}}else h=null}h=h||{start:0,end:0}}else h=
null;Ed={activeElementDetached:null,focusedElem:g,selectionRange:h};fd=!1;Y=e;do try{Tj()}catch(wb){if(null===Y)throw Error(u(330));Ei(Y,wb);Y=Y.nextEffect}while(null!==Y);Y=e;do try{for(g=a,h=b;null!==Y;){var q=Y.effectTag;q&16&&Rb(Y.stateNode,"");if(q&128){var B=Y.alternate;if(null!==B){var w=B.ref;null!==w&&("function"===typeof w?w(null):w.current=null)}}switch(q&1038){case 2:Pi(Y);Y.effectTag&=-3;break;case 6:Pi(Y);Y.effectTag&=-3;Si(Y.alternate,Y);break;case 1024:Y.effectTag&=-1025;break;case 1028:Y.effectTag&=
-1025;Si(Y.alternate,Y);break;case 4:Si(Y.alternate,Y);break;case 8:l=Y,Mi(g,l,h),Ni(l)}Y=Y.nextEffect}}catch(wb){if(null===Y)throw Error(u(330));Ei(Y,wb);Y=Y.nextEffect}while(null!==Y);w=Ed;B=xd();q=w.focusedElem;h=w.selectionRange;if(B!==q&&q&&q.ownerDocument&&wd(q.ownerDocument.documentElement,q)){null!==h&&yd(q)&&(B=h.start,w=h.end,void 0===w&&(w=B),"selectionStart"in q?(q.selectionStart=B,q.selectionEnd=Math.min(w,q.value.length)):(w=(B=q.ownerDocument||document)&&B.defaultView||window,w.getSelection&&
(w=w.getSelection(),l=q.textContent.length,g=Math.min(h.start,l),h=void 0===h.end?g:Math.min(h.end,l),!w.extend&&g>h&&(l=h,h=g,g=l),l=vd(q,g),m=vd(q,h),l&&m&&(1!==w.rangeCount||w.anchorNode!==l.node||w.anchorOffset!==l.offset||w.focusNode!==m.node||w.focusOffset!==m.offset)&&(B=B.createRange(),B.setStart(l.node,l.offset),w.removeAllRanges(),g>h?(w.addRange(B),w.extend(m.node,m.offset)):(B.setEnd(m.node,m.offset),w.addRange(B))))));B=[];for(w=q;w=w.parentNode;)1===w.nodeType&&B.push({element:w,left:w.scrollLeft,
top:w.scrollTop});"function"===typeof q.focus&&q.focus();for(q=0;q<B.length;q++)w=B[q],w.element.scrollLeft=w.left,w.element.scrollTop=w.top}fd=!!Dd;Ed=Dd=null;a.current=c;Y=e;do try{for(q=a;null!==Y;){var ub=Y.effectTag;ub&36&&Ji(q,Y.alternate,Y);if(ub&128){B=void 0;var vb=Y.ref;if(null!==vb){var Xc=Y.stateNode;switch(Y.tag){case 5:B=Xc;break;default:B=Xc}"function"===typeof vb?vb(B):vb.current=B}}Y=Y.nextEffect}}catch(wb){if(null===Y)throw Error(u(330));Ei(Y,wb);Y=Y.nextEffect}while(null!==Y);Y=
null;Vf();W=f}else a.current=c;if(qj)qj=!1,rj=a,sj=b;else for(Y=e;null!==Y;)b=Y.nextEffect,Y.nextEffect=null,Y=b;b=a.firstPendingTime;0===b&&(aj=null);1073741823===b?a===vj?uj++:(uj=0,vj=a):uj=0;"function"===typeof Uj&&Uj(c.stateNode,d);Z(a);if(Yi)throw Yi=!1,a=Zi,Zi=null,a;if((W&ej)!==V)return null;gg();return null}function Tj(){for(;null!==Y;){var a=Y.effectTag;0!==(a&256)&&Gi(Y.alternate,Y);0===(a&512)||qj||(qj=!0,dg(97,function(){Dj();return null}));Y=Y.nextEffect}}
function Dj(){if(90!==sj){var a=97<sj?97:sj;sj=90;return cg(a,Vj)}}function Vj(){if(null===rj)return!1;var a=rj;rj=null;if((W&(fj|gj))!==V)throw Error(u(331));var b=W;W|=gj;for(a=a.current.firstEffect;null!==a;){try{var c=a;if(0!==(c.effectTag&512))switch(c.tag){case 0:case 11:case 15:case 22:Hi(5,c),Ii(5,c)}}catch(d){if(null===a)throw Error(u(330));Ei(a,d)}c=a.nextEffect;a.nextEffect=null;a=c}W=b;gg();return!0}
function Wj(a,b,c){b=Ai(c,b);b=Xi(a,b,1073741823);xg(a,b);a=xj(a,1073741823);null!==a&&Z(a)}function Ei(a,b){if(3===a.tag)Wj(a,a,b);else for(var c=a.return;null!==c;){if(3===c.tag){Wj(c,a,b);break}else if(1===c.tag){var d=c.stateNode;if("function"===typeof c.type.getDerivedStateFromError||"function"===typeof d.componentDidCatch&&(null===aj||!aj.has(d))){a=Ai(b,a);a=$i(c,a,1073741823);xg(c,a);c=xj(c,1073741823);null!==c&&Z(c);break}}c=c.return}}
function Oj(a,b,c){var d=a.pingCache;null!==d&&d.delete(b);T===a&&U===c?S===vi||S===ui&&1073741823===lj&&$f()-Ti<pj?Ej(a,U):oj=!0:Aj(a,c)&&(b=a.lastPingedTime,0!==b&&b<c||(a.lastPingedTime=c,Z(a)))}function Vi(a,b){var c=a.stateNode;null!==c&&c.delete(b);b=0;0===b&&(b=Gg(),b=Hg(b,a,null));a=xj(a,b);null!==a&&Z(a)}var Rj;
Rj=function(a,b,c){var d=b.expirationTime;if(null!==a){var e=b.pendingProps;if(a.memoizedProps!==e||K.current)rg=!0;else{if(d<c){rg=!1;switch(b.tag){case 3:hi(b);Xh();break;case 5:fh(b);if(b.mode&4&&1!==c&&e.hidden)return b.expirationTime=b.childExpirationTime=1,null;break;case 1:L(b.type)&&Gf(b);break;case 4:dh(b,b.stateNode.containerInfo);break;case 10:d=b.memoizedProps.value;e=b.type._context;I(jg,e._currentValue);e._currentValue=d;break;case 13:if(null!==b.memoizedState){d=b.child.childExpirationTime;
if(0!==d&&d>=c)return ji(a,b,c);I(M,M.current&1);b=$h(a,b,c);return null!==b?b.sibling:null}I(M,M.current&1);break;case 19:d=b.childExpirationTime>=c;if(0!==(a.effectTag&64)){if(d)return mi(a,b,c);b.effectTag|=64}e=b.memoizedState;null!==e&&(e.rendering=null,e.tail=null);I(M,M.current);if(!d)return null}return $h(a,b,c)}rg=!1}}else rg=!1;b.expirationTime=0;switch(b.tag){case 2:d=b.type;null!==a&&(a.alternate=null,b.alternate=null,b.effectTag|=2);a=b.pendingProps;e=Cf(b,J.current);qg(b,c);e=oh(null,
b,d,a,e,c);b.effectTag|=1;if("object"===typeof e&&null!==e&&"function"===typeof e.render&&void 0===e.$$typeof){b.tag=1;b.memoizedState=null;b.updateQueue=null;if(L(d)){var f=!0;Gf(b)}else f=!1;b.memoizedState=null!==e.state&&void 0!==e.state?e.state:null;ug(b);var g=d.getDerivedStateFromProps;"function"===typeof g&&Fg(b,d,g,a);e.updater=Jg;b.stateNode=e;e._reactInternalFiber=b;Ng(b,d,a,c);b=gi(null,b,d,!0,f,c)}else b.tag=0,R(null,b,e,c),b=b.child;return b;case 16:a:{e=b.elementType;null!==a&&(a.alternate=
null,b.alternate=null,b.effectTag|=2);a=b.pendingProps;ob(e);if(1!==e._status)throw e._result;e=e._result;b.type=e;f=b.tag=Xj(e);a=ig(e,a);switch(f){case 0:b=di(null,b,e,a,c);break a;case 1:b=fi(null,b,e,a,c);break a;case 11:b=Zh(null,b,e,a,c);break a;case 14:b=ai(null,b,e,ig(e.type,a),d,c);break a}throw Error(u(306,e,""));}return b;case 0:return d=b.type,e=b.pendingProps,e=b.elementType===d?e:ig(d,e),di(a,b,d,e,c);case 1:return d=b.type,e=b.pendingProps,e=b.elementType===d?e:ig(d,e),fi(a,b,d,e,c);
case 3:hi(b);d=b.updateQueue;if(null===a||null===d)throw Error(u(282));d=b.pendingProps;e=b.memoizedState;e=null!==e?e.element:null;vg(a,b);zg(b,d,null,c);d=b.memoizedState.element;if(d===e)Xh(),b=$h(a,b,c);else{if(e=b.stateNode.hydrate)Ph=Jd(b.stateNode.containerInfo.firstChild),Oh=b,e=Qh=!0;if(e)for(c=Yg(b,null,d,c),b.child=c;c;)c.effectTag=c.effectTag&-3|1024,c=c.sibling;else R(a,b,d,c),Xh();b=b.child}return b;case 5:return fh(b),null===a&&Uh(b),d=b.type,e=b.pendingProps,f=null!==a?a.memoizedProps:
null,g=e.children,Gd(d,e)?g=null:null!==f&&Gd(d,f)&&(b.effectTag|=16),ei(a,b),b.mode&4&&1!==c&&e.hidden?(b.expirationTime=b.childExpirationTime=1,b=null):(R(a,b,g,c),b=b.child),b;case 6:return null===a&&Uh(b),null;case 13:return ji(a,b,c);case 4:return dh(b,b.stateNode.containerInfo),d=b.pendingProps,null===a?b.child=Xg(b,null,d,c):R(a,b,d,c),b.child;case 11:return d=b.type,e=b.pendingProps,e=b.elementType===d?e:ig(d,e),Zh(a,b,d,e,c);case 7:return R(a,b,b.pendingProps,c),b.child;case 8:return R(a,
b,b.pendingProps.children,c),b.child;case 12:return R(a,b,b.pendingProps.children,c),b.child;case 10:a:{d=b.type._context;e=b.pendingProps;g=b.memoizedProps;f=e.value;var h=b.type._context;I(jg,h._currentValue);h._currentValue=f;if(null!==g)if(h=g.value,f=$e(h,f)?0:("function"===typeof d._calculateChangedBits?d._calculateChangedBits(h,f):1073741823)|0,0===f){if(g.children===e.children&&!K.current){b=$h(a,b,c);break a}}else for(h=b.child,null!==h&&(h.return=b);null!==h;){var k=h.dependencies;if(null!==
k){g=h.child;for(var l=k.firstContext;null!==l;){if(l.context===d&&0!==(l.observedBits&f)){1===h.tag&&(l=wg(c,null),l.tag=2,xg(h,l));h.expirationTime<c&&(h.expirationTime=c);l=h.alternate;null!==l&&l.expirationTime<c&&(l.expirationTime=c);pg(h.return,c);k.expirationTime<c&&(k.expirationTime=c);break}l=l.next}}else g=10===h.tag?h.type===b.type?null:h.child:h.child;if(null!==g)g.return=h;else for(g=h;null!==g;){if(g===b){g=null;break}h=g.sibling;if(null!==h){h.return=g.return;g=h;break}g=g.return}h=
g}R(a,b,e.children,c);b=b.child}return b;case 9:return e=b.type,f=b.pendingProps,d=f.children,qg(b,c),e=sg(e,f.unstable_observedBits),d=d(e),b.effectTag|=1,R(a,b,d,c),b.child;case 14:return e=b.type,f=ig(e,b.pendingProps),f=ig(e.type,f),ai(a,b,e,f,d,c);case 15:return ci(a,b,b.type,b.pendingProps,d,c);case 17:return d=b.type,e=b.pendingProps,e=b.elementType===d?e:ig(d,e),null!==a&&(a.alternate=null,b.alternate=null,b.effectTag|=2),b.tag=1,L(d)?(a=!0,Gf(b)):a=!1,qg(b,c),Lg(b,d,e),Ng(b,d,e,c),gi(null,
b,d,!0,a,c);case 19:return mi(a,b,c)}throw Error(u(156,b.tag));};var Uj=null,Li=null;function Yj(a){if("undefined"===typeof __REACT_DEVTOOLS_GLOBAL_HOOK__)return!1;var b=__REACT_DEVTOOLS_GLOBAL_HOOK__;if(b.isDisabled||!b.supportsFiber)return!0;try{var c=b.inject(a);Uj=function(a){try{b.onCommitFiberRoot(c,a,void 0,64===(a.current.effectTag&64))}catch(e){}};Li=function(a){try{b.onCommitFiberUnmount(c,a)}catch(e){}}}catch(d){}return!0}
function Zj(a,b,c,d){this.tag=a;this.key=c;this.sibling=this.child=this.return=this.stateNode=this.type=this.elementType=null;this.index=0;this.ref=null;this.pendingProps=b;this.dependencies=this.memoizedState=this.updateQueue=this.memoizedProps=null;this.mode=d;this.effectTag=0;this.lastEffect=this.firstEffect=this.nextEffect=null;this.childExpirationTime=this.expirationTime=0;this.alternate=null}function Sh(a,b,c,d){return new Zj(a,b,c,d)}
function bi(a){a=a.prototype;return!(!a||!a.isReactComponent)}function Xj(a){if("function"===typeof a)return bi(a)?1:0;if(void 0!==a&&null!==a){a=a.$$typeof;if(a===gb)return 11;if(a===jb)return 14}return 2}
function Sg(a,b){var c=a.alternate;null===c?(c=Sh(a.tag,b,a.key,a.mode),c.elementType=a.elementType,c.type=a.type,c.stateNode=a.stateNode,c.alternate=a,a.alternate=c):(c.pendingProps=b,c.effectTag=0,c.nextEffect=null,c.firstEffect=null,c.lastEffect=null);c.childExpirationTime=a.childExpirationTime;c.expirationTime=a.expirationTime;c.child=a.child;c.memoizedProps=a.memoizedProps;c.memoizedState=a.memoizedState;c.updateQueue=a.updateQueue;b=a.dependencies;c.dependencies=null===b?null:{expirationTime:b.expirationTime,
firstContext:b.firstContext,responders:b.responders};c.sibling=a.sibling;c.index=a.index;c.ref=a.ref;return c}
function Ug(a,b,c,d,e,f){var g=2;d=a;if("function"===typeof a)bi(a)&&(g=1);else if("string"===typeof a)g=5;else a:switch(a){case ab:return Wg(c.children,e,f,b);case fb:g=8;e|=7;break;case bb:g=8;e|=1;break;case cb:return a=Sh(12,c,b,e|8),a.elementType=cb,a.type=cb,a.expirationTime=f,a;case hb:return a=Sh(13,c,b,e),a.type=hb,a.elementType=hb,a.expirationTime=f,a;case ib:return a=Sh(19,c,b,e),a.elementType=ib,a.expirationTime=f,a;default:if("object"===typeof a&&null!==a)switch(a.$$typeof){case db:g=
10;break a;case eb:g=9;break a;case gb:g=11;break a;case jb:g=14;break a;case kb:g=16;d=null;break a;case lb:g=22;break a}throw Error(u(130,null==a?a:typeof a,""));}b=Sh(g,c,b,e);b.elementType=a;b.type=d;b.expirationTime=f;return b}function Wg(a,b,c,d){a=Sh(7,a,d,b);a.expirationTime=c;return a}function Tg(a,b,c){a=Sh(6,a,null,b);a.expirationTime=c;return a}
function Vg(a,b,c){b=Sh(4,null!==a.children?a.children:[],a.key,b);b.expirationTime=c;b.stateNode={containerInfo:a.containerInfo,pendingChildren:null,implementation:a.implementation};return b}
function ak(a,b,c){this.tag=b;this.current=null;this.containerInfo=a;this.pingCache=this.pendingChildren=null;this.finishedExpirationTime=0;this.finishedWork=null;this.timeoutHandle=-1;this.pendingContext=this.context=null;this.hydrate=c;this.callbackNode=null;this.callbackPriority=90;this.lastExpiredTime=this.lastPingedTime=this.nextKnownPendingLevel=this.lastSuspendedTime=this.firstSuspendedTime=this.firstPendingTime=0}
function Aj(a,b){var c=a.firstSuspendedTime;a=a.lastSuspendedTime;return 0!==c&&c>=b&&a<=b}function xi(a,b){var c=a.firstSuspendedTime,d=a.lastSuspendedTime;c<b&&(a.firstSuspendedTime=b);if(d>b||0===c)a.lastSuspendedTime=b;b<=a.lastPingedTime&&(a.lastPingedTime=0);b<=a.lastExpiredTime&&(a.lastExpiredTime=0)}
function yi(a,b){b>a.firstPendingTime&&(a.firstPendingTime=b);var c=a.firstSuspendedTime;0!==c&&(b>=c?a.firstSuspendedTime=a.lastSuspendedTime=a.nextKnownPendingLevel=0:b>=a.lastSuspendedTime&&(a.lastSuspendedTime=b+1),b>a.nextKnownPendingLevel&&(a.nextKnownPendingLevel=b))}function Cj(a,b){var c=a.lastExpiredTime;if(0===c||c>b)a.lastExpiredTime=b}
function bk(a,b,c,d){var e=b.current,f=Gg(),g=Dg.suspense;f=Hg(f,e,g);a:if(c){c=c._reactInternalFiber;b:{if(dc(c)!==c||1!==c.tag)throw Error(u(170));var h=c;do{switch(h.tag){case 3:h=h.stateNode.context;break b;case 1:if(L(h.type)){h=h.stateNode.__reactInternalMemoizedMergedChildContext;break b}}h=h.return}while(null!==h);throw Error(u(171));}if(1===c.tag){var k=c.type;if(L(k)){c=Ff(c,k,h);break a}}c=h}else c=Af;null===b.context?b.context=c:b.pendingContext=c;b=wg(f,g);b.payload={element:a};d=void 0===
d?null:d;null!==d&&(b.callback=d);xg(e,b);Ig(e,f);return f}function ck(a){a=a.current;if(!a.child)return null;switch(a.child.tag){case 5:return a.child.stateNode;default:return a.child.stateNode}}function dk(a,b){a=a.memoizedState;null!==a&&null!==a.dehydrated&&a.retryTime<b&&(a.retryTime=b)}function ek(a,b){dk(a,b);(a=a.alternate)&&dk(a,b)}
function fk(a,b,c){c=null!=c&&!0===c.hydrate;var d=new ak(a,b,c),e=Sh(3,null,null,2===b?7:1===b?3:0);d.current=e;e.stateNode=d;ug(e);a[Od]=d.current;c&&0!==b&&Jc(a,9===a.nodeType?a:a.ownerDocument);this._internalRoot=d}fk.prototype.render=function(a){bk(a,this._internalRoot,null,null)};fk.prototype.unmount=function(){var a=this._internalRoot,b=a.containerInfo;bk(null,a,null,function(){b[Od]=null})};
function gk(a){return!(!a||1!==a.nodeType&&9!==a.nodeType&&11!==a.nodeType&&(8!==a.nodeType||" react-mount-point-unstable "!==a.nodeValue))}function hk(a,b){b||(b=a?9===a.nodeType?a.documentElement:a.firstChild:null,b=!(!b||1!==b.nodeType||!b.hasAttribute("data-reactroot")));if(!b)for(var c;c=a.lastChild;)a.removeChild(c);return new fk(a,0,b?{hydrate:!0}:void 0)}
function ik(a,b,c,d,e){var f=c._reactRootContainer;if(f){var g=f._internalRoot;if("function"===typeof e){var h=e;e=function(){var a=ck(g);h.call(a)}}bk(b,g,a,e)}else{f=c._reactRootContainer=hk(c,d);g=f._internalRoot;if("function"===typeof e){var k=e;e=function(){var a=ck(g);k.call(a)}}Nj(function(){bk(b,g,a,e)})}return ck(g)}function jk(a,b,c){var d=3<arguments.length&&void 0!==arguments[3]?arguments[3]:null;return{$$typeof:$a,key:null==d?null:""+d,children:a,containerInfo:b,implementation:c}}
wc=function(a){if(13===a.tag){var b=hg(Gg(),150,100);Ig(a,b);ek(a,b)}};xc=function(a){13===a.tag&&(Ig(a,3),ek(a,3))};yc=function(a){if(13===a.tag){var b=Gg();b=Hg(b,a,null);Ig(a,b);ek(a,b)}};
za=function(a,b,c){switch(b){case "input":Cb(a,c);b=c.name;if("radio"===c.type&&null!=b){for(c=a;c.parentNode;)c=c.parentNode;c=c.querySelectorAll("input[name="+JSON.stringify(""+b)+'][type="radio"]');for(b=0;b<c.length;b++){var d=c[b];if(d!==a&&d.form===a.form){var e=Qd(d);if(!e)throw Error(u(90));yb(d);Cb(d,e)}}}break;case "textarea":Kb(a,c);break;case "select":b=c.value,null!=b&&Hb(a,!!c.multiple,b,!1)}};Fa=Mj;
Ga=function(a,b,c,d,e){var f=W;W|=4;try{return cg(98,a.bind(null,b,c,d,e))}finally{W=f,W===V&&gg()}};Ha=function(){(W&(1|fj|gj))===V&&(Lj(),Dj())};Ia=function(a,b){var c=W;W|=2;try{return a(b)}finally{W=c,W===V&&gg()}};function kk(a,b){var c=2<arguments.length&&void 0!==arguments[2]?arguments[2]:null;if(!gk(b))throw Error(u(200));return jk(a,b,null,c)}var lk={Events:[Nc,Pd,Qd,xa,ta,Xd,function(a){jc(a,Wd)},Da,Ea,id,mc,Dj,{current:!1}]};
(function(a){var b=a.findFiberByHostInstance;return Yj(n({},a,{overrideHookState:null,overrideProps:null,setSuspenseHandler:null,scheduleUpdate:null,currentDispatcherRef:Wa.ReactCurrentDispatcher,findHostInstanceByFiber:function(a){a=hc(a);return null===a?null:a.stateNode},findFiberByHostInstance:function(a){return b?b(a):null},findHostInstancesForRefresh:null,scheduleRefresh:null,scheduleRoot:null,setRefreshHandler:null,getCurrentFiber:null}))})({findFiberByHostInstance:tc,bundleType:0,version:"16.13.1",
rendererPackageName:"react-dom"});exports.__SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED=lk;exports.createPortal=kk;exports.findDOMNode=function(a){if(null==a)return null;if(1===a.nodeType)return a;var b=a._reactInternalFiber;if(void 0===b){if("function"===typeof a.render)throw Error(u(188));throw Error(u(268,Object.keys(a)));}a=hc(b);a=null===a?null:a.stateNode;return a};
exports.flushSync=function(a,b){if((W&(fj|gj))!==V)throw Error(u(187));var c=W;W|=1;try{return cg(99,a.bind(null,b))}finally{W=c,gg()}};exports.hydrate=function(a,b,c){if(!gk(b))throw Error(u(200));return ik(null,a,b,!0,c)};exports.render=function(a,b,c){if(!gk(b))throw Error(u(200));return ik(null,a,b,!1,c)};
exports.unmountComponentAtNode=function(a){if(!gk(a))throw Error(u(40));return a._reactRootContainer?(Nj(function(){ik(null,null,a,!1,function(){a._reactRootContainer=null;a[Od]=null})}),!0):!1};exports.unstable_batchedUpdates=Mj;exports.unstable_createPortal=function(a,b){return kk(a,b,2<arguments.length&&void 0!==arguments[2]?arguments[2]:null)};
exports.unstable_renderSubtreeIntoContainer=function(a,b,c,d){if(!gk(c))throw Error(u(200));if(null==a||void 0===a._reactInternalFiber)throw Error(u(38));return ik(a,b,c,!1,d)};exports.version="16.13.1";


/***/ }),

/***/ 55:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(56);
} else {}


/***/ }),

/***/ 56:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/** @license React v0.19.1
 * scheduler.production.min.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

var f,g,h,k,l;
if("undefined"===typeof window||"function"!==typeof MessageChannel){var p=null,q=null,t=function(){if(null!==p)try{var a=exports.unstable_now();p(!0,a);p=null}catch(b){throw setTimeout(t,0),b;}},u=Date.now();exports.unstable_now=function(){return Date.now()-u};f=function(a){null!==p?setTimeout(f,0,a):(p=a,setTimeout(t,0))};g=function(a,b){q=setTimeout(a,b)};h=function(){clearTimeout(q)};k=function(){return!1};l=exports.unstable_forceFrameRate=function(){}}else{var w=window.performance,x=window.Date,
y=window.setTimeout,z=window.clearTimeout;if("undefined"!==typeof console){var A=window.cancelAnimationFrame;"function"!==typeof window.requestAnimationFrame&&console.error("This browser doesn't support requestAnimationFrame. Make sure that you load a polyfill in older browsers. https://fb.me/react-polyfills");"function"!==typeof A&&console.error("This browser doesn't support cancelAnimationFrame. Make sure that you load a polyfill in older browsers. https://fb.me/react-polyfills")}if("object"===
typeof w&&"function"===typeof w.now)exports.unstable_now=function(){return w.now()};else{var B=x.now();exports.unstable_now=function(){return x.now()-B}}var C=!1,D=null,E=-1,F=5,G=0;k=function(){return exports.unstable_now()>=G};l=function(){};exports.unstable_forceFrameRate=function(a){0>a||125<a?console.error("forceFrameRate takes a positive int between 0 and 125, forcing framerates higher than 125 fps is not unsupported"):F=0<a?Math.floor(1E3/a):5};var H=new MessageChannel,I=H.port2;H.port1.onmessage=
function(){if(null!==D){var a=exports.unstable_now();G=a+F;try{D(!0,a)?I.postMessage(null):(C=!1,D=null)}catch(b){throw I.postMessage(null),b;}}else C=!1};f=function(a){D=a;C||(C=!0,I.postMessage(null))};g=function(a,b){E=y(function(){a(exports.unstable_now())},b)};h=function(){z(E);E=-1}}function J(a,b){var c=a.length;a.push(b);a:for(;;){var d=c-1>>>1,e=a[d];if(void 0!==e&&0<K(e,b))a[d]=b,a[c]=e,c=d;else break a}}function L(a){a=a[0];return void 0===a?null:a}
function M(a){var b=a[0];if(void 0!==b){var c=a.pop();if(c!==b){a[0]=c;a:for(var d=0,e=a.length;d<e;){var m=2*(d+1)-1,n=a[m],v=m+1,r=a[v];if(void 0!==n&&0>K(n,c))void 0!==r&&0>K(r,n)?(a[d]=r,a[v]=c,d=v):(a[d]=n,a[m]=c,d=m);else if(void 0!==r&&0>K(r,c))a[d]=r,a[v]=c,d=v;else break a}}return b}return null}function K(a,b){var c=a.sortIndex-b.sortIndex;return 0!==c?c:a.id-b.id}var N=[],O=[],P=1,Q=null,R=3,S=!1,T=!1,U=!1;
function V(a){for(var b=L(O);null!==b;){if(null===b.callback)M(O);else if(b.startTime<=a)M(O),b.sortIndex=b.expirationTime,J(N,b);else break;b=L(O)}}function W(a){U=!1;V(a);if(!T)if(null!==L(N))T=!0,f(X);else{var b=L(O);null!==b&&g(W,b.startTime-a)}}
function X(a,b){T=!1;U&&(U=!1,h());S=!0;var c=R;try{V(b);for(Q=L(N);null!==Q&&(!(Q.expirationTime>b)||a&&!k());){var d=Q.callback;if(null!==d){Q.callback=null;R=Q.priorityLevel;var e=d(Q.expirationTime<=b);b=exports.unstable_now();"function"===typeof e?Q.callback=e:Q===L(N)&&M(N);V(b)}else M(N);Q=L(N)}if(null!==Q)var m=!0;else{var n=L(O);null!==n&&g(W,n.startTime-b);m=!1}return m}finally{Q=null,R=c,S=!1}}
function Y(a){switch(a){case 1:return-1;case 2:return 250;case 5:return 1073741823;case 4:return 1E4;default:return 5E3}}var Z=l;exports.unstable_IdlePriority=5;exports.unstable_ImmediatePriority=1;exports.unstable_LowPriority=4;exports.unstable_NormalPriority=3;exports.unstable_Profiling=null;exports.unstable_UserBlockingPriority=2;exports.unstable_cancelCallback=function(a){a.callback=null};exports.unstable_continueExecution=function(){T||S||(T=!0,f(X))};
exports.unstable_getCurrentPriorityLevel=function(){return R};exports.unstable_getFirstCallbackNode=function(){return L(N)};exports.unstable_next=function(a){switch(R){case 1:case 2:case 3:var b=3;break;default:b=R}var c=R;R=b;try{return a()}finally{R=c}};exports.unstable_pauseExecution=function(){};exports.unstable_requestPaint=Z;exports.unstable_runWithPriority=function(a,b){switch(a){case 1:case 2:case 3:case 4:case 5:break;default:a=3}var c=R;R=a;try{return b()}finally{R=c}};
exports.unstable_scheduleCallback=function(a,b,c){var d=exports.unstable_now();if("object"===typeof c&&null!==c){var e=c.delay;e="number"===typeof e&&0<e?d+e:d;c="number"===typeof c.timeout?c.timeout:Y(a)}else c=Y(a),e=d;c=e+c;a={id:P++,callback:b,priorityLevel:a,startTime:e,expirationTime:c,sortIndex:-1};e>d?(a.sortIndex=e,J(O,a),null===L(N)&&a===L(O)&&(U?h():U=!0,g(W,e-d))):(a.sortIndex=c,J(N,a),T||S||(T=!0,f(X)));return a};
exports.unstable_shouldYield=function(){var a=exports.unstable_now();V(a);var b=L(N);return b!==Q&&null!==Q&&null!==b&&null!==b.callback&&b.startTime<=a&&b.expirationTime<Q.expirationTime||k()};exports.unstable_wrapCallback=function(a){var b=R;return function(){var c=R;R=b;try{return a.apply(this,arguments)}finally{R=c}}};


/***/ }),

/***/ 57:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(58);

function emptyFunction() {}
function emptyFunctionWithReset() {}
emptyFunctionWithReset.resetWarningCache = emptyFunction;

module.exports = function() {
  function shim(props, propName, componentName, location, propFullName, secret) {
    if (secret === ReactPropTypesSecret) {
      // It is still safe when called from React.
      return;
    }
    var err = new Error(
      'Calling PropTypes validators directly is not supported by the `prop-types` package. ' +
      'Use PropTypes.checkPropTypes() to call them. ' +
      'Read more at http://fb.me/use-check-prop-types'
    );
    err.name = 'Invariant Violation';
    throw err;
  };
  shim.isRequired = shim;
  function getShim() {
    return shim;
  };
  // Important!
  // Keep this list in sync with production version in `./factoryWithTypeCheckers.js`.
  var ReactPropTypes = {
    array: shim,
    bool: shim,
    func: shim,
    number: shim,
    object: shim,
    string: shim,
    symbol: shim,

    any: shim,
    arrayOf: getShim,
    element: shim,
    elementType: shim,
    instanceOf: getShim,
    node: shim,
    objectOf: getShim,
    oneOf: getShim,
    oneOfType: getShim,
    shape: getShim,
    exact: getShim,

    checkPropTypes: emptyFunctionWithReset,
    resetWarningCache: emptyFunction
  };

  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),

/***/ 58:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),

/***/ 59:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony default export */ var _unused_webpack_default_export = ("/dist/images/favicon.png");

/***/ }),

/***/ 7:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


function checkDCE() {
  /* global __REACT_DEVTOOLS_GLOBAL_HOOK__ */
  if (
    typeof __REACT_DEVTOOLS_GLOBAL_HOOK__ === 'undefined' ||
    typeof __REACT_DEVTOOLS_GLOBAL_HOOK__.checkDCE !== 'function'
  ) {
    return;
  }
  if (false) {}
  try {
    // Verify that the code above has been dead code eliminated (DCE'd).
    __REACT_DEVTOOLS_GLOBAL_HOOK__.checkDCE(checkDCE);
  } catch (err) {
    // DevTools shouldn't crash React, no matter what.
    // We should still report in case we break this code.
    console.error(err);
  }
}

if (true) {
  // DCE check should happen before ReactDOM bundle executes so that
  // DevTools can report bad minification during injection.
  checkDCE();
  module.exports = __webpack_require__(54);
} else {}


/***/ }),

/***/ 73:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_feather__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(27);



function Catalog(props) {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("section", {
    className: "section section-small-padding catalog"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h1", {
    className: "subtitle is-4"
  }, props.title), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, props.showNew && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("a", {
    href: props.newUrl,
    className: "button is-primary"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
    className: "icon"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_feather__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"], null)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, props.newText))));
}

/* harmony default export */ __webpack_exports__["a"] = (Catalog);

/***/ })

/******/ });