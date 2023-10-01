import React from 'react';
import ButtonWithImg from './ButtonWithImg';

const EditButton = ({hidden, light = false, ...props}) => {
    return(
        <ButtonWithImg
            {...props}
            hidden = {hidden} 
            src={require('../../../images/edit.png')}
            style={{backgroundColor: light ? 'beige' : '#82d26e'}}
        />
    )
}

export default EditButton
