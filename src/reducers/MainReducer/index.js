import {
  RESET_STATE,
  SET_ADDRESS,
  SET_AUTO_UPDATE,
  SET_CHECKED_FOR_UPDATES,
  SET_COLOR_ON_DARK,
  SET_END_PORT,
  SET_ERROR,
  SET_IS_SCANNING,
  SET_LANGUAGE_INDEX,
  SET_LOADING,
  SET_NO_CLOSED,
  SET_PAGE_INDEX,
  SET_SCAN_RESULTS,
  SET_SORT,
  SET_START_PORT,
  SET_THEME_INDEX,
  SET_THEME_TYPE,
  SET_THREADS,
  SET_TIMEOUT,
  SET_UPDATE,
  SET_THEME_TOGGLE,
  SET_EXPORT_NO_CLOSED,
  SET_IS_CANCELLING,
  SET_NO_UNKNOWN,
  SET_EXPORT_NO_UNKNOWN,
} from './Actions/actionTypes';

const MainReducer = (state, action) => {
  if (!action || !action.type) {
    return state;
  }
  switch (action.type) {
    case SET_LANGUAGE_INDEX:
      localStorage.languageIndex = action.payload;
      return {
        ...state,
        languageIndex: action.payload,
      };
    case SET_THEME_INDEX:
      localStorage.themeIndex = action.payload;
      return {
        ...state,
        themeIndex: action.payload,
      };
    case SET_THEME_TYPE:
      localStorage.themeType = action.payload;
      return {
        ...state,
        themeType: action.payload,
      };
    case SET_PAGE_INDEX:
      return {
        ...state,
        pageIndex: action.payload,
      };
    case RESET_STATE:
      localStorage.clear();
      return {
        ...state,
        languageIndex: 0,
        themeIndex: 0,
        themeType: 'light',
        autoUpdate: true,
        colorOnDark: false,
        threads: 1,
        timeout: 300,
        noClosed: false,
        noHidden: false,
        sort: true,
        themeToggle: true,
        exportNoClosed: true,
        exportNoHidden: true,
      };
    case SET_AUTO_UPDATE:
      localStorage.autoUpdate = action.payload;
      return {
        ...state,
        autoUpdate: action.payload,
      };
    case SET_UPDATE:
      return {
        ...state,
        update: action.payload,
      };
    case SET_LOADING:
      return {
        ...state,
        loading: action.payload,
      };
    case SET_ERROR:
      return {
        ...state,
        error: action.payload,
      };
    case SET_CHECKED_FOR_UPDATES:
      return {
        ...state,
        checkedForUpdates: action.payload,
      };
    case SET_COLOR_ON_DARK:
      localStorage.colorOnDark = action.payload;
      return {
        ...state,
        colorOnDark: action.payload,
      };
    case SET_ADDRESS:
      // eslint-disable-next-line no-case-declarations
      let address = action.payload;
      if (address.startsWith('http://')) {
        address = address.replace('http://', '');
      }
      if (address.startsWith('https://')) {
        address = address.replace('https://', '');
      }
      return {
        ...state,
        address,
      };
    case SET_START_PORT:
      return {
        ...state,
        startPort: action.payload,
      };
    case SET_END_PORT:
      return {
        ...state,
        endPort: action.payload,
      };
    case SET_IS_SCANNING:
      return {
        ...state,
        isScanning: action.payload,
      };
    case SET_THREADS:
      localStorage.threads = action.payload;
      return {
        ...state,
        threads: action.payload,
      };
    case SET_TIMEOUT:
      localStorage.timeout = action.payload;
      return {
        ...state,
        timeout: action.payload,
      };
    case SET_NO_CLOSED:
      localStorage.noClosed = action.payload;
      return {
        ...state,
        noClosed: action.payload,
      };
    case SET_SORT:
      localStorage.sort = action.payload;
      return {
        ...state,
        sort: action.payload,
      };
    case SET_SCAN_RESULTS:
      return {
        ...state,
        scanResults: action.payload,
      };
    case SET_THEME_TOGGLE:
      localStorage.themeToggle = action.payload;
      return {
        ...state,
        themeToggle: action.payload,
      };
    case SET_EXPORT_NO_CLOSED:
      localStorage.exportNoClosed = action.payload;
      return {
        ...state,
        exportNoClosed: action.payload,
      };
    case SET_IS_CANCELLING:
      return {
        ...state,
        isCancelling: action.payload,
      };
    case SET_NO_UNKNOWN:
      localStorage.noUnknown = action.payload;
      return {
        ...state,
        noUnknown: action.payload,
      };
    case SET_EXPORT_NO_UNKNOWN:
      localStorage.exportNoUnknown = action.payload;
      return {
        ...state,
        exportNoUnknown: action.payload,
      };
    default:
      return state;
  }
};

export default MainReducer;
