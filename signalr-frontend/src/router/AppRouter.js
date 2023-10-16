import React, {useContext} from 'react';
import { Redirect, Route, Routes} from 'react-router-dom';
import { workerRoutes, customerRoutes, commonRoutes } from './Routes';
import { UserContext } from '../context/UserContext';
const AppRouter = () => {
    const {user, setUser} = useContext(UserContext);

    return(
        <Routes>
            {commonRoutes.map(route=> 
                <Route path={route.path} element = {route.element} key={route.path}/>)
            }
            {(user.role === 1 || user.role === 2) &&
            workerRoutes.map(route=> 
                <Route path={route.path} element = {route.element} key={route.path}/>)
            }
            {user.role === 0 &&
            customerRoutes.map(route=> 
                <Route path={route.path} element = {route.element} key={route.path}/>)
            }
        </Routes>
    )
}

export default AppRouter