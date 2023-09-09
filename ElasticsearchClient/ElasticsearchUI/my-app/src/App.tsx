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

    const handleKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter') {
            handleSearch();
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
                </div>
            </Container>
        </ThemeProvider>
    );
};

export default App;
