import React from 'react';
import styleClasses from './StyledInput.module.css'


const StyledInput = (props) => {
    return (
        <input className={styleClasses.input} {...props}/>
    );
};

export default StyledInput;
