import React, {useContext, useState} from 'react';
import { UserContext } from '../../context/UserContext';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../../components/LoginForm/LoginForm';
import authorize from '../../fetching/HTTP'; 
import styles from './LoginPage.module.css'

const LoginPage = () => {
    const {user, setUser} = useContext(UserContext);
    const navigate = useNavigate();

    const login = async (userInfo) => {
        var jwt = await authorize(userInfo.email, userInfo.password, userInfo.role)
        console.log(jwt)        
        setUser({...user, role: userInfo.role, email: userInfo.email,  jwt: jwt})
        navigate('/projects');
    }

    var roles = [
        {name: 'Customer', value: 0},
        {name: 'Worker', value: 1},
        {name: 'Project manager', value: 2},
    ]

    return (
        // authorizingLoading ? <h1> Loading ...</h1> :
        <div className={styles.loginPage}>
            <LoginForm 
                roles={roles} 
                defaultRole={roles[0]}
                loginHangler={login}
            />
        </div>    
    );
};

export default LoginPage;
