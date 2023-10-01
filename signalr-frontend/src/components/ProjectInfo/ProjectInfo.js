import React, {useState} from 'react';
import CancelButton from "../UI/button/CancelButton";
import EditButton from "../UI/button/EditButton";
import styles from "./ProjectInfo.module.css";
import StyledInput from '../UI/input/StyledInput';
import StyledTextArea from '../UI/textArea/StyledTextArea';
import CheckButton from '../UI/button/CheckButton';

const ProjectInfo = ({project, isCustomer=false, onEdit, onCancel}) => {

    const [projectUpdate, setProjectUpdate] = useState(project)
    const [editing, setEditing] = useState(false)

    return (

        !editing ?

        <div>
            <div className={styles.projectHeader}>
                <span>Project: "{project.objective}"</span>

                <div className={styles.centered}>
                    <span>Status:  {project.status}</span>
                    <EditButton
                        light={false} 
                        hidden={!isCustomer}
                        onClick={() => setEditing(true)}
                    />
                    <CancelButton 
                        hidden={!isCustomer} 
                        onClick={async () => await onCancel(project.id)}
                    />
                </div>
            </div>
            <p>
                <span>Description: </span>
                {project.description}
            </p>
        </div> 

        :

        <div>
            <div className={styles.projectHeader}>
                <div className={styles.centered}>
                    <label className={styles.marginRight10px} htmlFor='objective'>Project: </label>
                    <StyledInput 
                        id='objective'
                        onChange={e=>setProjectUpdate({...projectUpdate, objective: e.target.value })}
                        value={projectUpdate.objective}
                        placeholder="Objective"
                        type="text"
                    />
                </div>

                <div className={styles.centered}>
                    <label className={styles.marginRight10px} htmlFor='status'>Status: </label>
                    {/* <StyledInput 
                        id='status'
                        onChange={e=>setProjectUpdate({...projectUpdate, status: e.target.value })}
                        value={projectUpdate.status}
                        placeholder="Status"
                        type="text"
                    /> */}
                    <span>Status:  {project.status}</span>
                    <CheckButton
                        light={false} 
                        onClick={async () => {setEditing(false); await onEdit(projectUpdate)}}
                    />
                </div>
            </div>
            <div>
                <label className={styles.marginRight10px} htmlFor='description'>Description: </label>
                <StyledTextArea
                    id='description'
                    value={projectUpdate.description}
                    onChange={e=>setProjectUpdate({...projectUpdate, description: e.target.value })}
                    placeholder="Description"                
                />
            </div>
        </div>
    );
};

export default ProjectInfo;
