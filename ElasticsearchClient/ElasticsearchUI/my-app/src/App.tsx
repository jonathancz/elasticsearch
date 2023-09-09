import React, { useState } from 'react';
import { Container, AppBar, Toolbar, Typography, TextField, IconButton, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import axios from 'axios';

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});

const App = () => {
    const [customerId, setCustomerId] = useState<string>('');
    const [subdivisions, setSubdivisions] = useState<Array<any>>([]);

    const [customerName, setCustomerName] = useState<string>('');
    const [customers, setCustomers] = useState<Array<any>>([]);

    const handleSearch = async () => {
        // Replace with your API endpoint
        const apiUrl = `https://localhost:7297/api/Search/searchSubdivisionByCustomerId?customerId=${customerId}`;

        try {
            const response = await axios.get(apiUrl);
            if (response.status === 200) {
                setSubdivisions(response.data);
            }
        } catch (error) {
            console.error('An error occurred while fetching data:', error);
        }
    };

    const handleCustomerSearch = async () => {
        let apiUrl = `https://localhost:7297/api/Search?entityType=customer`;

        if (customerName) {
            apiUrl += `&query=${encodeURIComponent(customerName)}`;
        }

        try {
            const response = await axios.get(apiUrl);
            if (response.status === 200) {
                setCustomers(response.data);
            }
        } catch (error) {
            console.error('An error occurred while fetching data:', error);
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter') {
            handleSearch();
        }
    };

    const handleCustomerKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter') {
            handleCustomerSearch();
        }
    };

    return (
        <ThemeProvider theme={darkTheme}>
            <Container component="main" maxWidth="md">
                <CssBaseline />
                <div style={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    height: '100vh'
                }}>
                    <AppBar position="relative">
                        <Toolbar>
                            <Typography variant="h6">
                                Elasticsearch Sample App
                            </Typography>
                        </Toolbar>
                    </AppBar>
                    <div style={{ marginTop: '2rem' }}>
                        <Typography component="h1" variant="h5">
                            Search Subdivisions
                        </Typography>
                        <div style={{ display: 'flex', marginTop: '1rem' }}>
                            <TextField
                                variant="outlined"
                                fullWidth
                                id="customerId"
                                label="Enter Customer ID"
                                value={customerId}
                                onChange={(e) => setCustomerId(e.target.value)}
                                onKeyDown={handleKeyDown}
                                autoFocus
                            />
                            <IconButton type="submit" color="primary" onClick={handleSearch}>
                                <SearchIcon />
                            </IconButton>
                        </div>
                        <TableContainer component={Paper} style={{ marginTop: '2rem' }}>
                            <Table aria-label="simple table">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>ID</TableCell>
                                        <TableCell align="right">Name</TableCell>
                                        <TableCell align="right">Customer ID</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {subdivisions.map((row, index) => (
                                        <TableRow key={index}>
                                            <TableCell component="th" scope="row">
                                                {row.id}
                                            </TableCell>
                                            <TableCell align="right">{row.name}</TableCell>
                                            <TableCell align="right">{row.customerId}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </div>


                    <div style={{ marginTop: '2rem' }}>
                        <Typography component="h1" variant="h5">
                            Search Customers
                        </Typography>
                        <div style={{ display: 'flex', marginTop: '1rem' }}>
                            <TextField
                                variant="outlined"
                                fullWidth
                                id="customerName"
                                label="Enter Customer Name"
                                value={customerName}
                                onChange={(e) => setCustomerName(e.target.value)}
                                onKeyDown={handleCustomerKeyDown}
                                autoFocus
                            />
                            <IconButton type="submit" color="primary" onClick={handleCustomerSearch}>
                                <SearchIcon />
                            </IconButton>
                        </div>
                        <TableContainer component={Paper} style={{ marginTop: '2rem' }}>
                            <div style={{ maxHeight: '80vh', overflow: 'auto' }}> {/* Add scroll functionality */}
                                <Table stickyHeader aria-label="simple table">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>ID</TableCell>
                                            <TableCell align="right">Customer ID</TableCell>
                                            <TableCell align="right">Customer Name</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {customers.slice(0, 500).map((row, index) => (  // Allow up to 500 results
                                            <TableRow key={index}>
                                                <TableCell component="th" scope="row">
                                                    {row.id}
                                                </TableCell>
                                                <TableCell align="right">{row.id}</TableCell>
                                                <TableCell align="right">{row.name}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </div>
                        </TableContainer>
                    </div>
                </div>
            </Container>
        </ThemeProvider>
    );
};

export default App;
