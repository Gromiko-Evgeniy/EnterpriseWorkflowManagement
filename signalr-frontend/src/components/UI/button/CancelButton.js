import React from 'react';
import ButtonWithImg from './ButtonWithImg';

const CancelButton = ({hidden, ...props}) => {
    return(
        <ButtonWithImg 
            hidden={hidden}
            {...props} 
            src={require('../../../images/cross.png')}
            style={{backgroundColor: '#ff9b96'}}
        />
    )
}

export default CancelButton
