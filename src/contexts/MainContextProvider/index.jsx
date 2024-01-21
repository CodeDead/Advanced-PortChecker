import React, { createContext, useReducer } from 'react';
import MainReducer from '../../reducers/MainReducer';
import enUs from '../../languages/en_us.json';

const languageIndex = localStorage.languageIndex ? parseFloat(localStorage.languageIndex) : 0;
const themeIndex = localStorage.themeIndex ? parseFloat(localStorage.themeIndex) : 0;
const themeType = localStorage.themeType ? localStorage.themeType : 'light';
const autoUpdate = localStorage.autoUpdate ? (localStorage.autoUpdate === 'true') : true;
const colorOnDark = localStorage.colorOnDark ? (localStorage.colorOnDark === 'true') : false;
const languageSelector = localStorage.languageSelector
  ? (localStorage.languageSelector === 'true')
  : false;

const threads = localStorage.threads ? parseFloat(localStorage.threads) : 24;
const timeout = localStorage.timeout ? parseFloat(localStorage.timeout) : 250;
const noClosed = localStorage.noClosed ? (localStorage.noClosed === 'true') : false;
const sort = localStorage.sort ? (localStorage.sort === 'true') : true;

const initState = {
  autoUpdate,
  languageIndex,
  languages: [
    enUs,
  ],
  themeIndex,
  themeType,
  pageIndex: 0,
  update: null,
  checkedForUpdates: false,
  languageSelector,
  loading: false,
  colorOnDark,
  error: null,
  address: '',
  startPort: 1,
  endPort: 65535,
  isScanning: false,
  threads,
  timeout,
  noClosed,
  sort,
  scanResults: null,
  scanType: 'tcp',
};

export const MainContext = createContext(initState);

const MainContextProvider = ({ children }) => {
  const [state, dispatch] = useReducer(MainReducer, initState);

  return (
    // eslint-disable-next-line react/jsx-no-constructed-context-values
    <MainContext.Provider value={[state, dispatch]}>
      {children}
    </MainContext.Provider>
  );
};

export default MainContextProvider;