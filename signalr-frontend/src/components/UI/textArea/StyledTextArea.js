import React from 'react';
import styleClasses from './StyledTextArea.module.css'


const StyledTextArea = (props) => {
    return (
        <textarea className={styleClasses.textArea} {...props}/>
    );
};

export default StyledTextArea;