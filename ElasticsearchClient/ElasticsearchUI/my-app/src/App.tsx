import React from 'react';
import {Input, NextUIProvider} from '@nextui-org/react';
import './App.css';

function App() {
  return (
      <NextUIProvider>
      <div className="App">
        <div className="App-content">
          <Input placeholder="Search..." />
        </div>
      </div>
      </NextUIProvider>
  );
}

export default App;