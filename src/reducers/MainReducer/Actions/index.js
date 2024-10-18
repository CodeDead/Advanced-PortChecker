import { invoke } from '@tauri-apps/api/core';
import {
  RESET_STATE,
  SET_ADDRESSES,
  SET_AUTO_UPDATE,
  SET_CHECKED_FOR_UPDATES,
  SET_COLOR_ON_DARK,
  SET_END_PORT,
  SET_ERROR,
  SET_EXPORT_NO_CLOSED,
  SET_EXPORT_NO_UNKNOWN,
  SET_IS_CANCELLING,
  SET_IS_SCANNING,
  SET_LANGUAGE_INDEX,
  SET_LOADING,
  SET_NO_CLOSED,
  SET_NO_UNKNOWN,
  SET_PAGE_INDEX,
  SET_SCAN_RESULTS,
  SET_SORT,
  SET_START_PORT,
  SET_THEME_INDEX,
  SET_THEME_TOGGLE,
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
  invoke('open_website', { website })
    .catch((e) => {
      // eslint-disable-next-line no-console
      console.error(e);
    });
};

export const scanAddresses = (addresses, startPort, endPort, timeout, threads, sort) => {
  const cmd = {
    addresses,
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

export const setAddresses = (addresses) => ({
  type: SET_ADDRESSES,
  payload: addresses,
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

export const cancelScan = () => invoke('cancel_scan');

export const getNumberOfThreads = () => invoke('get_number_of_threads');

export const setThemeToggle = (value) => ({
  type: SET_THEME_TOGGLE,
  payload: value,
});

export const setExportNoClosed = (value) => ({
  type: SET_EXPORT_NO_CLOSED,
  payload: value,
});

export const setIsCancelling = (value) => ({
  type: SET_IS_CANCELLING,
  payload: value,
});

export const setNoUnknown = (value) => ({
  type: SET_NO_UNKNOWN,
  payload: value,
});

export const setExportNoUnknown = (value) => ({
  type: SET_EXPORT_NO_UNKNOWN,
  payload: value,
});
