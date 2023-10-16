import ProjectInfo from "../ProjectInfo/ProjectInfo";
import TaskList from "../TaskList/TaskList";
import styles from "./ProjectAndTasksInfo.module.css";
import React from 'react';

const ProjectAndTasksInfo = ({project, tasks, isCustomer, onProjectEdit, onProjectCancel, onTaskEdit, onTaskCancel}) => {
    return (
        <div className={styles.main}>
            <ProjectInfo 
                project={project}
                isCustomer={isCustomer}
                onEdit={onProjectEdit}
                onCancel={onProjectCancel}
            />
            <TaskList
                tasks={tasks}
                isCustomer={isCustomer}
                onEdit={onTaskEdit}
                onCancel={onTaskCancel}
            />
        </div>   
    );
};

export default ProjectAndTasksInfo;
