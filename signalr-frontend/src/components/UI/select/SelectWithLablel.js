import React from 'react';
import styles from './SelectWithLablel.module.css'

const SelectWithLablel = ({options, value, onChange, label}) => {
    
    return (
        <div className={styles.main}>
            <label htmlFor='select' className={styles.text}>{label}</label>
            <select
                className={styles.select}
                id="select"
                value={value}
                onChange={(e) => onChange(parseInt(e.target.value, 10))}
            >
                {options.map(option =>
                    <option key={option.value} value={option.value}>
                        {option.name}
                    </option>
                )}
            </select>
        </div>
    );
};

export default SelectWithLablel;
