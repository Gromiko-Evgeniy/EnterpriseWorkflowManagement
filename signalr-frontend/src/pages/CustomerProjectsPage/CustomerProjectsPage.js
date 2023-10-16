import React, {useContext, useState, useEffect} from 'react';
import { UserContext } from '../../context/UserContext';
import ProjectAndTasksInfo from '../../components/ProjectWithTaskList/ProjectAndTasksInfo';
import { fetchProjectWithTasks, updateProject, cancelProject, updateTask, cancelTask } from '../../fetching/SignalRInteraction';
import styles from './CustomerProjectsPage.module.css' 
import Navbar from '../../components/Navbar/Navbar';

const MultipleProjectsPage = () => {

    const {user, setUser} = useContext(UserContext);
    const [projects, setProjects] = useState([])
    const [error, setError] = useState(null)
    const [connection, setConnection] = useState(null)

    useEffect(()=>{
        fetchProjectWithTasks(processProjectWithTasks, setConnection, user.jwt)
        console.log(projects);
    }, [])

    const processProjectWithTasks = (newProject) => {
        if (typeof newProject === 'string') {
            setError(newProject)
            setProjects([])
        }
        else {
            setError(null)
            setProjects(projects => {
                const updatedProjects = [...projects];
                const index = updatedProjects.findIndex(p => p.id === newProject.id);

                if (index !== -1) {
                    updatedProjects[index] = newProject;
                }
                else {
                    updatedProjects.push(newProject)
                }

                return updatedProjects;
            })
            console.log(newProject);
        }
    }

    const onProjectEdit = async (project) => {
        await updateProject(connection, project, user.jwt)
    }

    const onProjectCancel = async (id) => {
        await cancelProject(connection, id, user.jwt)
    }

    const onTaskEdit = async (task) => {
        await updateTask(connection, task, user.jwt)
    }

    const onTaskCancel = async (id) => {
        await cancelTask(connection, id, user.jwt)
    }

    return (
        <div className={styles.page}>
            <Navbar/>

            {!error ? 
                <div className={styles.projectContent}> 
                    {projects.map(project => 
                        <ProjectAndTasksInfo
                            isCustomer={true}
                            key={project.id}
                            project={project}
                            tasks={project.tasks}
                            onProjectEdit={onProjectEdit}
                            onProjectCancel={onProjectCancel}
                            onTaskEdit={onTaskEdit}
                            onTaskCancel={onTaskCancel}
                        />
                    )}
                </div>
                : 
                <div>{error}</div> 
            }
        </div>
    );
};

export default MultipleProjectsPage;
