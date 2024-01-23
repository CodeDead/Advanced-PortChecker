import React, {
  lazy, Suspense, useContext, useEffect,
} from 'react';
import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { type } from '@tauri-apps/api/os';
import DrawerHeader from '../DrawerHeader';
import TopBar from '../TopBar';
import Updater from '../../utils/Updater';
import packageJson from '../../../package.json';
import {
  getNumberOfThreads,
  openWebSite,
  setCheckedForUpdates,
  setError,
  setLoading,
  setThreads,
  setUpdate,
} from '../../reducers/MainReducer/Actions';
import { MainContext } from '../../contexts/MainContextProvider';
import ThemeSelector from '../../utils/ThemeSelector';
import UpdateDialog from '../UpdateDialog';
import AlertDialog from '../AlertDialog';
import LoadingBar from '../LoadingBar';

const Home = lazy(() => import('../../routes/Home'));
const Settings = lazy(() => import('../../routes/Settings'));
const About = lazy(() => import('../../routes/About'));

const App = () => {
  const [state, d1] = useContext(MainContext);
  const {
    themeIndex, themeType, update, languageIndex, autoUpdate, error, loading,
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
  const checkForUpdates = () => {
    if (loading) {
      return;
    }

    d1(setUpdate(null));
    d1(setError(null));

    type()
      .then((res) => {
        Updater(res.toLowerCase(), packageJson.version)
          .then((up) => {
            d1(setUpdate(up));
          })
          .catch((err) => {
            d1(setError(err));
          });
      })
      .catch((e) => {
        d1(setError(e));
      })
      .finally(() => {
        d1(setLoading(false));
      });
  };

  /**
   * Update the number of threads
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
    // eslint-disable-next-line no-underscore-dangle
    if (window.__TAURI__ && autoUpdate) {
      updateThreads();
      checkForUpdates();
    }
  }, []);

  return (
    <ThemeProvider theme={theme}>
      <BrowserRouter>
        <Box sx={{ display: 'flex' }}>
          <CssBaseline />
          <TopBar />
          <Box component="main" sx={{ flexGrow: 1, p: 3 }} fullWidth>
            <DrawerHeader />
            <Suspense fallback={<LoadingBar />}>
              <Routes>
                <Route exact path="/" element={<Home />} />
                <Route exact path="/settings" element={<Settings />} />
                <Route exact path="/about" element={<About />} />
              </Routes>
            </Suspense>
          </Box>
        </Box>
      </BrowserRouter>
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
      {/* eslint-disable-next-line no-nested-ternary */}
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

export default App;
