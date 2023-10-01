import React, {useContext} from 'react';
import styles from './Navbar.module.css'
import { UserContext } from '../../context/UserContext';
import StyledButton from '../UI/button/StyledButton';

const Navbar = () => {

    const {user, setUser} = useContext(UserContext);

    const logOutHandler = ()=>{
        setUser({role: undefined, email: ''})
    }

    return (
        <div className={styles.navbar}>

            <p className={styles.userInfo}>
                <span>Role: {user.role}</span> 
                <span>Email: {user.email}</span>
            </p>

            <StyledButton onClick={logOutHandler}> Log out </StyledButton>
    </div>
    );
};

export default Navbar;