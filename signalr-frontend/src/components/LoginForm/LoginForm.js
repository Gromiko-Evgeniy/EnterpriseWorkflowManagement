import styles from "./LoginForm.module.css";
import React, {useState} from 'react';
import StyledButton from '../../components/UI/button/StyledButton';
import StyledInput from '../../components/UI/input/StyledInput';
import SelectWithLablel from '../UI/select/SelectWithLablel';


const LoginForm = ({roles, loginHangler}) => {
    const [loginData, setLoginData] = useState({email: '', password: '', role: roles[0].value})

    const login = async (event) => {
        event.preventDefault();
        loginHangler(loginData)
    }

    return (
        <div className={styles.loginForm} >
            <h1 className={styles.text}>Log in</h1>

            <StyledInput onChange={e=>setLoginData({...loginData, email: e.target.value })}
                value={loginData.email}  type="text" placeholder="Email"
            />
            <StyledInput onChange={e=>setLoginData({...loginData, password: e.target.value })}
                value={loginData.password} type="password" placeholder="Password"
            />

            <SelectWithLablel 
                label={'Role: '}
                options={roles} 
                value={loginData.role  }
                onChange={role=>setLoginData({...loginData, role: role })}
            />

            <StyledButton onClick={login} >Submit</StyledButton>
        </div>    
    );
};

export default LoginForm;
