import React, {useState, useContext} from 'react';
import styles from './TaskInfo.module.css'
import CancelButton from '../UI/button/CancelButton';
import EditButton from '../UI/button/EditButton';
import CheckButton from '../UI/button/CheckButton';
import StyledInput from '../UI/input/StyledInput';
import StyledTextArea from '../UI/textArea/StyledTextArea';
import { UserContext } from '../../context/UserContext';

const TaskInfo = ({task, isCustomer=false, onEdit, onCancel}) => {

    const {user, setUser} = useContext(UserContext);
    const [taskUpdate, setTaskUpdate] = useState(task)
    const [editing, setEditing] = useState(false)

    return (
        !editing ?

        <div className={styles.task}>
            <div className={styles.spaceBetween}>
                <span>Name: {task.name}</span>
                <span className={styles.beige} hidden={isCustomer || user.email !== task.workerEmail}>Yours!</span>
                <div className={styles.centered}>
                    <div>
                        <p className={styles.noMargin}>Status:  {task.status}</p>
                        <p className={styles.noMargin}>Priority:  {task.priority}</p>
                    </div>
                    <EditButton
                        light={true} 
                        hidden={!isCustomer}
                        onClick={() => setEditing(true)}
                    />
                    <CancelButton
                        hidden={!isCustomer} 
                        onClick={async () => await onCancel(task.id)}
                    />
                </div>
            </div>
            <p>
                <span>Description: </span>
                {task.description}
            </p>
        </div>

        :

        <div className={styles.task}>
            <div className={styles.spaceBetween}>
                <div className={styles.centered}>
                    <label className={styles.marginRight10px} htmlFor='name'>Name: </label>
                    <StyledInput 
                        id='name'
                        onChange={e=>setTaskUpdate({...taskUpdate, name: e.target.value })}
                        value={taskUpdate.name}
                        placeholder="Name"
                        type="text"
                    />
                </div>
                
                <div className={styles.centered}>
                    <label className={styles.marginRight10px} htmlFor='priority'>Priority: </label>
                    <StyledInput
                        id='priority'
                        onChange={e=>setTaskUpdate({...taskUpdate, priority: e.target.value })}
                        value={taskUpdate.priority}
                        placeholder="Priority"
                        type="text"
                    />

                    <CheckButton
                        light={true} 
                        onClick={async () => {setEditing(false); await onEdit(taskUpdate)}}
                    />
                </div>
            </div>
            <div>
                <label className={styles.marginRight10px} htmlFor='description'>Description: </label>
                <StyledTextArea
                    id='description'
                    value={taskUpdate.description}
                    onChange={e=>setTaskUpdate({...taskUpdate, description: e.target.value })}
                    placeholder="Description"                
                />
            </div>
        </div>
    );
};

export default TaskInfo;
