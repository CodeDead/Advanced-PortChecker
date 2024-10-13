import React, { useContext, useEffect, useState } from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Grid from '@mui/material/Grid2';
import TextField from '@mui/material/TextField';
import Container from '@mui/material/Container';
import Paper from '@mui/material/Paper';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import { save } from '@tauri-apps/plugin-dialog';
import { invoke } from '@tauri-apps/api/core';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import Alert from '@mui/material/Alert';
import Snackbar from '@mui/material/Snackbar';
import LoadingBar from '../../components/LoadingBar';
import {
  cancelScan,
  scanAddress,
  setAddress,
  setEndPort,
  setError,
  setIsCancelling,
  setIsScanning,
  setPageIndex,
  setScanResults,
  setStartPort,
} from '../../reducers/MainReducer/Actions';
import { MainContext } from '../../contexts/MainContextProvider';
import PortInput from '../../components/PortInput';

const Home = () => {
  const [state, d1] = useContext(MainContext);

  const {
    languages, languageIndex, address, startPort, endPort, timeout,
    threads, noClosed, sort, isScanning, scanResults, exportNoClosed,
    isCancelling, noUnknown, exportNoUnknown,
  } = state;
  const language = languages[languageIndex];

  const [exportType, setExportType] = useState('application/json');
  const [snackOpen, setSnackOpen] = useState(false);

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
      d1(setIsCancelling(true));
      cancelScan()
        .catch((err) => {
          d1(setError(err));
        })
        .finally(() => {
          d1(setIsCancelling(false));
        });
    } else {
      if (address === '' || startPort < 0
        || startPort > 65535
        || endPort < 0
        || endPort > 65535
        || startPort > endPort) return;

      d1(setIsScanning(true));
      d1(setScanResults(null));

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
   * Clear the scan results
   */
  const clearScanResults = () => {
    d1(setScanResults(null));
  };

  /**
   * Handle key down
   * @param event The event argument
   */
  const handleKeyDown = (event) => {
    if (event.key === 'Enter') {
      startStopScan();
    }
  };

  /**
   * Set the export type
   * @param e The change event
   */
  const handleExportTypeChange = (e) => {
    setExportType(e.target.value);
  };

  /**
   * Close the snackbar
   */
  const closeSnack = () => {
    setSnackOpen(false);
  };

  /**
   * Get the export data
   * @param res The array of ScanResult objects
   * @param type The type of export
   * @returns {string} The export data
   */
  const getExportData = (res, type) => {
    let toExport = '';

    if (type === 'text/plain') {
      res.forEach((e) => {
        if ((!exportNoClosed && e.portStatus === 'Closed') || (!exportNoUnknown && e.portStatus === 'Unknown')) return;
        toExport += `${e.address} ${e.port} ${e.hostName} ${e.portStatus} ${e.scanDate}\n`;
      });
    } else if (type === 'application/json') {
      let exportJson = JSON.parse(JSON.stringify(res));
      if (!exportNoClosed) {
        exportJson = exportJson.filter((e) => e.portStatus !== 'Closed');
      }
      if (!exportNoUnknown) {
        exportJson = exportJson.filter((e) => e.portStatus !== 'Unknown');
      }
      toExport = JSON.stringify(exportJson, null, 2);
    } else if (type === 'text/csv') {
      res.forEach((e) => {
        if ((!exportNoClosed && e.portStatus === 'Closed') || (!exportNoUnknown && e.portStatus === 'Unknown')) return;
        toExport += `"${e.address.replaceAll('"', '""')}","${e.port}","${e.hostName.replaceAll('"', '""')}","${e.portStatus.replaceAll('"', '""')}","${e.scanDate.replaceAll('"', '""')}",\n`;
      });
    } else if (type === 'text/html') {
      toExport = '<!DOCTYPE html><html lang="en"><head><title>Advanced PortChecker</title><style>table, th, td {border: 1px solid black;}</style></head><body><table><thead><tr><th>Address</th><th>Port</th><th>Host Name</th><th>Port Status</th><th>Scan Date</th></tr></thead><tbody>';
      res.forEach((e) => {
        if ((!exportNoClosed && e.portStatus === 'Closed') || (!exportNoUnknown && e.portStatus === 'Unknown')) return;
        toExport += `<tr><td>${e.address}</td><td>${e.port}</td><td>${e.hostName}</td><td>${e.portStatus}</td><td>${e.scanDate}</td></tr>`;
      });
      toExport += '</tbody></table></body></html>';
    }

    return toExport;
  };

  /**
   * Export the data
   */
  const onExport = () => {
    let ext = '';
    switch (exportType) {
      case 'text/plain':
        ext = 'txt';
        break;
      case 'application/json':
        ext = 'json';
        break;
      case 'text/html':
        ext = 'html';
        break;
      default:
        ext = 'csv';
        break;
    }
    save({
      multiple: false,
      filters: [{
        name: exportType,
        extensions: [ext],
      }],
    })
      .then((res) => {
        if (res && res.length > 0) {
          // eslint-disable-next-line no-bitwise
          const resExt = res.slice((res.lastIndexOf('.') - 1 >>> 0) + 2);
          const path = resExt && resExt.length > 0 ? res : `${res}.${ext}`;
          invoke('save_string_to_disk', { content: getExportData(scanResults, exportType), path })
            .then(() => {
              setSnackOpen(true);
            })
            .catch((e) => {
              d1(setError(e));
            });
        }
      })
      .catch((e) => {
        d1(setError(e));
      });
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
      if ((noClosed && res.portStatus === 'Closed') || (noUnknown && res.portStatus === 'Unknown')) {
        // eslint-disable-next-line no-continue
        continue;
      }

      let portStatus = language.closed;
      if (res.portStatus === 'Open') {
        portStatus = language.open;
      } else if (res.portStatus === 'Unknown') {
        portStatus = language.unknown;
      }

      scanResultRows.push(
        createData(
          res.address + res.port,
          res.address,
          res.port,
          res.hostName,
          portStatus,
          res.scanDate,
        ),
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
            <Grid size={12}>
              <TextField
                id="address-basic"
                label={language.address}
                variant="outlined"
                placeholder="127.0.0.1/24"
                value={address}
                disabled={isScanning}
                fullWidth
                onChange={changeAddress}
                onKeyDown={handleKeyDown}
              />
            </Grid>
            <Grid size={{ xs: 12, md: 6, lg: 6 }}>
              <PortInput
                label={language.startingPort}
                port={startPort}
                disabled={isScanning}
                onKeyDown={handleKeyDown}
                onChange={changeStartPort}
              />
            </Grid>
            <Grid size={{ xs: 12, md: 6, lg: 6 }}>
              <PortInput
                label={language.endingPort}
                port={endPort}
                disabled={isScanning}
                onKeyDown={handleKeyDown}
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
            pageSizeOptions={[5, 10, 25, 50, 100]}
            disableSelectionOnClick
          />
        </Paper>
      )}
      <Button
        variant="contained"
        color="primary"
        disabled={!scanResults || scanResults.length === 0 || isScanning}
        sx={{ mt: 2, float: 'left' }}
        onClick={clearScanResults}
      >
        {language.clear}
      </Button>
      <Button
        variant="contained"
        color="primary"
        sx={{ mt: 2, float: 'right' }}
        disabled={address === '' || isCancelling}
        onClick={startStopScan}
      >
        {isScanning ? language.cancel : language.scan}
      </Button>
      <Button
        variant="contained"
        color="primary"
        onClick={onExport}
        sx={{ mt: 2, ml: 2, mr: 2 }}
        disabled={!scanResults || scanResults.length === 0 || exportType === null || isScanning}
        style={{ float: 'right' }}
      >
        {language.export}
      </Button>
      <FormControl
        sx={{ mt: 2, minWidth: 150, float: 'right' }}
        size="small"
      >
        <InputLabel id="export-type-label">{language.exportType}</InputLabel>
        <Select
          labelId="export-type-label"
          id="export-type-select"
          value={exportType}
          label={language.exportType}
          autoWidth
          onChange={handleExportTypeChange}
          variant="outlined"
        >
          <MenuItem value="application/json">JSON</MenuItem>
          <MenuItem value="text/csv">CSV</MenuItem>
          <MenuItem value="text/plain">TXT</MenuItem>
          <MenuItem value="text/html">HTML</MenuItem>
        </Select>
      </FormControl>
      <Snackbar open={snackOpen} autoHideDuration={3000} onClose={closeSnack}>
        <Alert onClose={closeSnack} severity="success" sx={{ width: '100%' }}>
          {language.exportSuccessful}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default Home;
