import React, { useContext, useEffect } from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Container from '@mui/material/Container';
import Paper from '@mui/material/Paper';
import { DataGrid } from '@mui/x-data-grid';
import { Button } from '@mui/material';
import { MainContext } from '../../contexts/MainContextProvider';
import {
  cancelScan,
  scanAddress,
  setAddress,
  setEndPort,
  setError,
  setIsScanning,
  setPageIndex,
  setScanResults,
  setStartPort,
} from '../../reducers/MainReducer/Actions';
import LoadingBar from '../../components/LoadingBar';

const Home = () => {
  const [state, d1] = useContext(MainContext);

  const {
    languages, languageIndex, address, startPort, endPort,
    timeout, threads, noClosed, sort, isScanning, scanResults,
  } = state;
  const language = languages[languageIndex];

  /**
   * Change the (IP) address
   * @param e The event argument
   */
  const changeAddress = (e) => {
    d1(setAddress(e.target.value));
  };

  /**
   * Change the starting port
   * @param e The event argument
   */
  const changeStartPort = (e) => {
    if (parseInt(e.target.value, 10) < 0) return;
    if (parseInt(e.target.value, 10) > 65535) return;

    if (parseInt(e.target.value, 10) > endPort) {
      // eslint-disable-next-line no-use-before-define
      changeEndPort({ target: { value: parseInt(e.target.value, 10) } });
    }

    d1(setStartPort(parseInt(e.target.value, 10)));
  };

  /**
   * Change the ending port
   * @param e The event argument
   */
  const changeEndPort = (e) => {
    if (parseInt(e.target.value, 10) < 0) return;
    if (parseInt(e.target.value, 10) > 65535) return;

    if (parseInt(e.target.value, 10) < startPort) {
      changeStartPort({ target: { value: parseInt(e.target.value, 10) } });
    }

    d1(setEndPort(parseInt(e.target.value, 10)));
  };

  /**
   * Start (or cancel) a scan
   */
  const startStopScan = () => {
    if (isScanning) {
      cancelScan()
        .catch((err) => {
          d1(setError(err));
        });
    } else {
      d1(setIsScanning(true));
      scanAddress(address, startPort, endPort, timeout, threads, sort)
        .then((res) => {
          d1(setScanResults(res));
        })
        .catch((err) => {
          d1(setError(err));
        })
        .finally(() => {
          d1(setIsScanning(false));
        });
    }
  };

  /**
   * Create a data object
   * @param id The ID
   * @param addr The address
   * @param port The port
   * @param hostName The host name
   * @param portStatus The port status
   * @param scanDate The scan date
   * @returns {{hostName, portType, address, port, scanDate}}
   */
  const createData = (id, addr, port, hostName, portStatus, scanDate) => (
    {
      id, address: addr, port, hostName, portStatus, scanDate,
    }
  );

  const columns = [
    {
      field: 'address',
      headerName: language.address,
      editable: false,
      flex: 1,
    },
    {
      field: 'port',
      headerName: language.port,
      type: 'number',
      editable: false,
    },
    {
      field: 'hostName',
      headerName: language.hostName,
      editable: false,
      flex: 1,
    },
    {
      field: 'portStatus',
      headerName: language.portStatus,
      editable: false,
      flex: 1,
    },
    {
      field: 'scanDate',
      headerName: language.scanDate,
      editable: false,
      flex: 1,
    },
  ];

  const scanResultRows = [];
  if (scanResults && scanResults.length > 0) {
    // eslint-disable-next-line no-restricted-syntax
    for (const res of scanResults) {
      if (noClosed && res.portStatus === 'Closed') {
        // eslint-disable-next-line no-continue
        continue;
      }
      scanResultRows.push(
        createData(res.port, res.address, res.port, res.hostName, res.portStatus, res.scanDate),
      );
    }
  }

  useEffect(() => {
    d1(setPageIndex(0));
  }, []);

  return (
    <Container maxWidth="xxl" sx={{ flexGrow: 1 }}>
      <Card>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12} lg={12}>
              <TextField
                id="address-basic"
                label={language.address}
                variant="outlined"
                value={address}
                disabled={isScanning}
                fullWidth
                onChange={changeAddress}
              />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
              <TextField
                id="start-port-basic"
                label={language.startingPort}
                variant="outlined"
                value={startPort}
                disabled={isScanning}
                type="number"
                min={0}
                max={65535}
                pattern="[0-9]"
                fullWidth
                onChange={changeStartPort}
              />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
              <TextField
                id="end-port-basic"
                label={language.endingPort}
                variant="outlined"
                value={endPort}
                disabled={isScanning}
                type="number"
                pattern="[0-9]"
                min={0}
                max={65535}
                fullWidth
                onChange={changeEndPort}
              />
            </Grid>
          </Grid>
        </CardContent>
      </Card>
      {isScanning ? <LoadingBar marginTop={10} /> : (
        <Paper sx={{ height: '50vh', width: '100%', mt: 2 }}>
          <DataGrid
            rows={scanResultRows}
            columns={columns}
            pageSize={50}
            rowsPerPageOptions={[50]}
            disableSelectionOnClick
          />
        </Paper>
      )}
      <Button
        variant="contained"
        color="primary"
        sx={{ mt: 2, float: 'right' }}
        onClick={startStopScan}
      >
        {isScanning ? language.cancel : language.scan}
      </Button>
    </Container>
  );
};

export default Home;
