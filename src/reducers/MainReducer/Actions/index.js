import { invoke } from '@tauri-apps/api';
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
  SET_LANGUAGE_SELECTOR,
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
} from './actionTypes';

export const setLanguageIndex = (index) => ({
  type: SET_LANGUAGE_INDEX,
  payload: index,
});

export const setThemeIndex = (index) => ({
  type: SET_THEME_INDEX,
  payload: index,
});

export const setThemeType = (type) => ({
  type: SET_THEME_TYPE,
  payload: type,
});

export const resetState = () => ({
  type: RESET_STATE,
});

export const setPageIndex = (index) => ({
  type: SET_PAGE_INDEX,
  payload: index,
});

export const setAutoUpdate = (value) => ({
  type: SET_AUTO_UPDATE,
  payload: value,
});

export const openWebSite = (website) => {
  // eslint-disable-next-line no-underscore-dangle
  if (window.__TAURI__) {
    invoke('open_website', { website })
      .catch((e) => {
        // eslint-disable-next-line no-console
        console.error(e);
      });
  } else {
    window.open(website, '_blank'); // We're running in a browser
  }
};

export const scanAddress = (address, startPort, endPort, timeout, threads, sort) => {
  const cmd = {
    address,
    startPort: parseInt(startPort, 10),
    endPort: parseInt(endPort, 10),
    timeout: parseFloat(timeout),
    threads: parseInt(threads, 10),
    sort,
  };

  return invoke('scan_port_range', cmd);
};

export const setUpdate = (update) => ({
  type: SET_UPDATE,
  payload: update,
});

export const setError = (error) => ({
  type: SET_ERROR,
  payload: error,
});

export const setLanguageSelector = (value) => ({
  type: SET_LANGUAGE_SELECTOR,
  payload: value,
});

export const setLoading = (value) => ({
  type: SET_LOADING,
  payload: value,
});

export const setCheckedForUpdates = (value) => ({
  type: SET_CHECKED_FOR_UPDATES,
  payload: value,
});

export const setColorOnDark = (value) => ({
  type: SET_COLOR_ON_DARK,
  payload: value,
});

export const setAddress = (address) => ({
  type: SET_ADDRESS,
  payload: address,
});

export const setStartPort = (port) => ({
  type: SET_START_PORT,
  payload: port,
});

export const setEndPort = (port) => ({
  type: SET_END_PORT,
  payload: port,
});

export const setIsScanning = (value) => ({
  type: SET_IS_SCANNING,
  payload: value,
});

export const setThreads = (value) => ({
  type: SET_THREADS,
  payload: value,
});

export const setTimeout = (value) => ({
  type: SET_TIMEOUT,
  payload: value,
});

export const setNoClosed = (value) => ({
  type: SET_NO_CLOSED,
  payload: value,
});

export const setSort = (value) => ({
  type: SET_SORT,
  payload: value,
});

export const setScanResults = (value) => ({
  type: SET_SCAN_RESULTS,
  payload: value,
});
