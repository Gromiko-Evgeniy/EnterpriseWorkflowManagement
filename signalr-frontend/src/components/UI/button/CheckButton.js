import React from 'react';
import ButtonWithImg from './ButtonWithImg';

const CheckButton = ({light = false, ...props}) => {
    return(
        <ButtonWithImg
            {...props} 
            src={require('../../../images/check.png')}
            style={{backgroundColor: light ? 'beige' : '#82d26e'}}
        />
    )
}

export default CheckButton
