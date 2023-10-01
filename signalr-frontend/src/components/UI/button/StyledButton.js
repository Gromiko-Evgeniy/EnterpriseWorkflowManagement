import React from 'react';
import styles from './Buttons.module.css'

const StyledButton = ({children, ...props}) => {
    return(
        <button {...props} className={styles.styledButton}>
            {children}
        </button>
    )
}

export default StyledButton