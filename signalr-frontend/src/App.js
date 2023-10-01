import React, {useState} from 'react';
import { UserContext } from "./context/UserContext";
import { BrowserRouter} from 'react-router-dom';
import AppRouter from './router/AppRouter';

function App() {

  const [user, setUser] = useState({role: undefined, email: undefined, jwt: undefined});

  return (
    <UserContext.Provider value={{user, setUser}}>
      <BrowserRouter>
        <AppRouter/>
      </BrowserRouter>
    </UserContext.Provider>
  );
}

export default App;
