import React, { Suspense, useContext, useEffect } from 'react';
import Box from '@mui/material/Box';
import CssBaseline from '@mui/material/CssBaseline';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { getVersion } from '@tauri-apps/api/app';
import { platform, arch } from '@tauri-apps/plugin-os';
import { Outlet } from 'react-router-dom';
import { MainContext } from '../../contexts/MainContextProvider';
import {
  getNumberOfThreads,
  openWebSite,
  setCheckedForUpdates,
  setError,
  setLoading,
  setThreads,
  setUpdate,
} from '../../reducers/MainReducer/Actions';
import ThemeSelector from '../../utils/ThemeSelector';
import Updater from '../../utils/Updater';
import AlertDialog from '../AlertDialog';
import DrawerHeader from '../DrawerHeader';
import LoadingBar from '../LoadingBar';
import TopBar from '../TopBar';
import UpdateDialog from '../UpdateDialog';

const Layout = () => {
  const [state, d1] = useContext(MainContext);
  const {
    themeIndex,
    themeType,
    update,
    languageIndex,
    autoUpdate,
    error,
    loading,
    checkedForUpdates,
  } = state;

  const language = state.languages[languageIndex];
  const color = ThemeSelector(themeIndex);

  const theme = createTheme({
    palette: {
      primary: color,
      mode: themeType,
    },
  });

  /**
   * Check for updates
   */
  const checkForUpdates = async () => {
    if (loading) {
      return;
    }

    d1(setUpdate(null));
    d1(setError(null));

    try {
      const res = platform();
      const archRes = arch();
      const ver = 'v' + (await getVersion());

      Updater(res.toLowerCase(), archRes.toLowerCase(), ver)
        .then((up) => {
          d1(setUpdate(up));
        })
        .catch((err) => {
          d1(setError(err));
        });
    } catch (e) {
      d1(setError(e));
    }
    d1(setLoading(false));
  };

  /**
   * Update the number of threads
   * @returns {Promise<void>}
   */
  const updateThreads = async () => {
    if (!localStorage.threads) {
      getNumberOfThreads()
        .then((res) => {
          d1(setThreads(res));
        })
        .catch(() => {
          d1(setThreads(1));
        });
    }
  };

  /**
   * Close the dialog that displays a message that no updates are available
   */
  const closeNoUpdate = () => {
    d1(setCheckedForUpdates(false));
  };

  /**
   * Close the alert dialog
   */
  const closeAlertDialog = () => {
    d1(setError(null));
  };

  useEffect(() => {
    if (window.__TAURI__ && autoUpdate) {
      updateThreads();
      checkForUpdates();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <ThemeProvider theme={theme}>
      <Box sx={{ display: 'flex' }}>
        <CssBaseline />
        <TopBar />
        <Box
          component="main"
          sx={{
            flexGrow: 1,
            p: 3,
            minHeight: 0,
            minWidth: 0,
            overflow: 'auto',
          }}
        >
          <DrawerHeader />
          <Suspense fallback={<LoadingBar />}>
            <Outlet />
          </Suspense>
        </Box>
      </Box>
      {error && error.length > 0 ? (
        <AlertDialog
          open
          title={language.error}
          content={error}
          agreeLabel={language.ok}
          onOk={closeAlertDialog}
          onClose={closeAlertDialog}
        />
      ) : null}
      {}
      {update && update.updateAvailable ? (
        <UpdateDialog
          downloadUrl={update.updateUrl}
          infoUrl={update.infoUrl}
          openWebsite={openWebSite}
          newVersion={update.version}
          onClose={() => d1(setUpdate(null))}
          updateAvailable={language.updateAvailable}
          newVersionText={language.newVersion}
          information={language.information}
          download={language.download}
          cancel={language.cancel}
        />
      ) : update && !update.updateAvailable && checkedForUpdates ? (
        <AlertDialog
          open
          title={language.checkForUpdates}
          content={language.runningLatestVersion}
          onOk={closeNoUpdate}
          onClose={closeNoUpdate}
          agreeLabel={language.ok}
        />
      ) : null}
    </ThemeProvider>
  );
};

export default Layout;
