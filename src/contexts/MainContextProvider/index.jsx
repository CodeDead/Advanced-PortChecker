import React, { createContext, useReducer } from 'react';
import enUs from '../../languages/en_us.json';
import esEs from '../../languages/es_es.json';
import frFr from '../../languages/fr_fr.json';
import itIt from '../../languages/it_it.json';
import jaJp from '../../languages/ja_jp.json';
import nlNl from '../../languages/nl_nl.json';
import zhCn from '../../languages/zh_cn.json';
import MainReducer from '../../reducers/MainReducer';

const languageIndex = localStorage.languageIndex
  ? parseFloat(localStorage.languageIndex)
  : 0;
const themeIndex = localStorage.themeIndex
  ? parseFloat(localStorage.themeIndex)
  : 0;
const themeType = localStorage.themeType ? localStorage.themeType : 'light';
const autoUpdate = localStorage.autoUpdate
  ? localStorage.autoUpdate === 'true'
  : true;
const colorOnDark = localStorage.colorOnDark
  ? localStorage.colorOnDark === 'true'
  : false;

const threads = localStorage.threads ? parseFloat(localStorage.threads) : 1;
const timeout = localStorage.timeout ? parseFloat(localStorage.timeout) : 250;
const noClosed = localStorage.noClosed
  ? localStorage.noClosed === 'true'
  : false;
const noUnknown = localStorage.noUnknown
  ? localStorage.noUnknown === 'true'
  : false;
const exportNoClosed = localStorage.exportNoClosed
  ? localStorage.exportNoClosed === 'true'
  : true;
const exportNoUnknown = localStorage.exportNoUnknown
  ? localStorage.exportNoUnknown === 'true'
  : true;
const sort = localStorage.sort ? localStorage.sort === 'true' : true;
const themeToggle = localStorage.themeToggle
  ? localStorage.themeToggle === 'true'
  : true;

const initState = {
  autoUpdate,
  languageIndex,
  languages: [enUs, esEs, frFr, itIt, jaJp, nlNl, zhCn],
  themeIndex,
  themeType,
  pageIndex: 0,
  update: null,
  checkedForUpdates: false,
  loading: false,
  colorOnDark,
  error: null,
  addresses: [''],
  startPort: 0,
  endPort: 65535,
  isScanning: false,
  isCancelling: false,
  threads,
  timeout,
  noClosed,
  noUnknown,
  exportNoClosed,
  exportNoUnknown,
  sort,
  scanResults: null,
  themeToggle,
};

// eslint-disable-next-line react-refresh/only-export-components
export const MainContext = createContext(initState);

const MainContextProvider = ({ children }) => {
  const [state, dispatch] = useReducer(MainReducer, initState);

  return (
    <MainContext.Provider value={[state, dispatch]}>
      {children}
    </MainContext.Provider>
  );
};

export default MainContextProvider;
