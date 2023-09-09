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
                                autoFocus
                            />
                            <IconButton type="submit" color="primary" onClick={handleSearch}>
                                <SearchIcon />
                            </IconButton>
                        </div>
                        <TableContainer component={Paper} style={{marginTop: '1rem'}}>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>ID</TableCell>
                                        <TableCell>Name</TableCell>
                                        <TableCell>Customer ID</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {subdivisions.map((subdivision, index) => (
                                        <TableRow key={index}>
                                            <TableCell>{subdivision.id}</TableCell>
                                            <TableCell>{subdivision.name}</TableCell>
                                            <TableCell>{subdivision.customerId}</TableCell>
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
