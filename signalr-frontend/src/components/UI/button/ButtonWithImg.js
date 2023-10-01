import React from 'react';
import styles from './Buttons.module.css'

const ButtonWithImg = ({onClick, src, hidden = false, ...props}) => {
    return(
        !hidden &&
        <button {...props} className={styles.roundButton}>
            <img 
                src={src}
                className={styles.smallImage}
                alt='cancel'
                onClick={()=> setTimeout(onClick, 200)}
            />
        </button>
    )
}

export default ButtonWithImg
