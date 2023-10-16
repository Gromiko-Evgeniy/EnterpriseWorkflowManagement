import React from 'react';
import TaskInfo from '../TaskInfo/TaskInfo';

const TaskList = ({tasks, onEdit, onCancel, isCustomer}) => {
    return (
        <div>
            {tasks.map(task =>
                    <TaskInfo 
                        key={task.name}
                        task={task}
                        onEdit={onEdit}
                        onCancel={onCancel}
                        isCustomer={isCustomer}
                    />
                )}
        </div>
    );
};

export default TaskList;
