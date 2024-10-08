import React, { createContext, useReducer } from 'react';
import MainReducer from '../../reducers/MainReducer';
import enUs from '../../languages/en_us.json';
import esEs from '../../languages/es_es.json';
import nlNl from '../../languages/nl_nl.json';
import frFr from '../../languages/fr_fr.json';
import zhCn from '../../languages/zh_cn.json';
import itIt from '../../languages/it_it.json';

const languageIndex = localStorage.languageIndex ? parseFloat(localStorage.languageIndex) : 0;
const themeIndex = localStorage.themeIndex ? parseFloat(localStorage.themeIndex) : 0;
const themeType = localStorage.themeType ? localStorage.themeType : 'light';
const autoUpdate = localStorage.autoUpdate ? (localStorage.autoUpdate === 'true') : true;
const colorOnDark = localStorage.colorOnDark ? (localStorage.colorOnDark === 'true') : false;

const threads = localStorage.threads ? parseFloat(localStorage.threads) : 1;
const timeout = localStorage.timeout ? parseFloat(localStorage.timeout) : 250;
const noClosed = localStorage.noClosed ? (localStorage.noClosed === 'true') : false;
const exportNoClosed = localStorage.exportNoClosed ? (localStorage.exportNoClosed === 'true') : true;
const sort = localStorage.sort ? (localStorage.sort === 'true') : true;
const themeToggle = localStorage.themeToggle ? (localStorage.themeToggle === 'true') : true;

const initState = {
  autoUpdate,
  languageIndex,
  languages: [
    enUs,
    esEs,
    frFr,
    itIt,
    nlNl,
    zhCn,
  ],
  themeIndex,
  themeType,
  pageIndex: 0,
  update: null,
  checkedForUpdates: false,
  loading: false,
  colorOnDark,
  error: null,
  address: '',
  startPort: 0,
  endPort: 65535,
  isScanning: false,
  threads,
  timeout,
  noClosed,
  exportNoClosed,
  sort,
  scanResults: null,
  themeToggle,
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
