import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import "../styles/ProjectDetailsPage.css";

import { getProjectById } from "../api/ProjectApi";
import {
    getAllProjectTasks,
    createProjectTask,
    updateProjectTask,
    deleteProjectTask
} from "../api/ProjectTaskApi";

import {
    type StatusTask,    
    StatusTaskLabel,
    type UpdateProjectTaskRequest
} from "../types/ProjectTask";

interface Project {
    id: string;
    name: string;
    description: string | null;
    createdAt: Date;
}
interface Task {
    id: string;
    name: string;
    description: string | null;
    status: StatusTask;
    deadline: string | null;
    createdAt: Date;
}

export default function ProjectDetailsPage() {
    const { projectId } = useParams();
    const navigate = useNavigate();

    const [project, setProject] = useState<Project | null>(null);
    const [tasks, setTasks] = useState<Task[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [editingTask, setEditingTask] = useState<Task | null>(null);

    const [showCreateModal, setShowCreateModal] = useState(false);
    const [newName, setNewName] = useState("");
    const [newDescription, setNewDescription] = useState("");
    const [newDeadline, setNewDeadline] = useState("");

    const [showEditModal, setShowEditModal] = useState(false);
    const [editName, setEditName] = useState("");
    const [editDescription, setEditDescription] = useState("");
    const [editStatus, setEditStatus] = useState<StatusTask>(0);
    const [editDeadline, setEditDeadline] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            try {
                if (!projectId) return;

                const projectData = await getProjectById(projectId);
                const tasksData = await getAllProjectTasks(projectId);

                setProject({
                    ...projectData,
                    createdAt: new Date(projectData.createdAt)
                });

                setTasks(tasksData.map((value) => 
                ({
                    ...value,
                    createdAt: new Date(value.createdAt),
                })));

            } catch (e) {
                console.error(e);
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, [projectId]);

    const getNextStatus = (status: StatusTask): StatusTask => {        
        return status;
    };

    const handleChangeStatus = async (task: Task) => {
        if (!projectId) return;

        const newStatus = getNextStatus(task.status);

        try {

            console.log(typeof(newStatus))
            const updated = await updateProjectTask(projectId, task.id, {
                status: newStatus
            });

            setTasks(prev =>
                prev.map(t => t.id === task.id ? { ...updated, createdAt: new Date(updated.createdAt) } : t)
            );

        } catch (e) {
            console.error(e);
        }
    };

    const handleCreateTask = async () => {
        if (!newName.trim() || !projectId) return;

        try {
            const created = await createProjectTask(projectId, {
                name: newName,
                description: newDescription || null,
                deadline: newDeadline || null
            });

            setTasks([...tasks, { ...created, createdAt: new Date(created.createdAt) }]);

            setNewName("");
            setNewDescription("");
            setNewDeadline("");
            setShowCreateModal(false);

        } catch (e) {
            console.error(e);
        }
    };

    const handleUpdateTask = async () => {
        if (!editingTask || !projectId) return;

        const request: UpdateProjectTaskRequest = {};

        if (editName !== editingTask.name) {
            request.name = editName;
        }

        if (editDescription !== (editingTask.description ?? "")) {
            request.description = editDescription;
        }

        if (editStatus !== editingTask.status) {
            request.status = editStatus;
        }

        if (editDeadline !== (editingTask.deadline ?? "")) {
            request.deadline = editDeadline;
        } 

        try {
            const updated = await updateProjectTask(projectId, editingTask.id, request);
            setTasks(prev =>
                prev.map(p =>
                    p.id === editingTask.id ? { ...updated, createdAt: new Date(updated.createdAt) } : p
                )
            );

            setEditName("");
            setEditDescription("");            
            setEditingTask(null);
            setEditStatus(0);
            setEditDeadline("");
            setShowEditModal(false);
        } catch (error) {
            console.error("Failed to update project", error);
        }
    };

    const handleDeleteTask = async (taskId: string) => {
        if (!projectId) return;
        if (!window.confirm("Delete task?")) return;

        try {
            await deleteProjectTask(projectId, taskId);
            setTasks(prev => prev.filter(t => t.id !== taskId));
        } catch (e) {
            console.error(e);
        }
    };


    if (isLoading) return <div>Loading...</div>;
    if (!project) return <div>Project not found</div>;

    return (
        <div className="project-details-page">

            <header className="project-header">
                <button onClick={() => navigate("/profile")}>Back</button>
                <h2>{project.name}</h2>
            </header>

            <div className="project-details-container">

                <div className="project-info-block">
                    <p>{project.description ?? "No description"}</p>
                    <small>{project.createdAt.toLocaleString()}</small>
                </div>

                <div className="tasks-header">
                    <h3>Tasks</h3>
                    <button onClick={() => setShowCreateModal(true)}>
                        Create task
                    </button>
                </div>

                {tasks.length === 0 ? (
                    <p>No tasks yet</p>
                ) : (
                    <div className="tasks-grid">
                        {tasks.map(task => (
                            <div key={task.id} className="task-card">

                                <div className="task-info">
                                    <h4>{task.name}</h4>
                                    <p>{task.description ?? "No description"}</p>
                                    <small>{StatusTaskLabel[task.status]}</small>

                                    <div className = "task-date">
                                        {task.deadline && (                                            
                                            <small> {new Date(task.deadline).toLocaleDateString()} </small>                                                                                       
                                        )}                                
                                        <small> {new Date(task.createdAt).toLocaleDateString()} </small>                             
                                    </div>
                                </div>

                                <div className="task-actions">
                                    <button className="edit-button" onClick={() => handleChangeStatus(task)}>
                                        Change status
                                    </button>

                                    <button className="edit-button"
                                        onClick={() => {
                                            setEditingTask(task);
                                            setEditName(task.name);
                                            setEditDescription(task.description ?? "");
                                            setEditStatus(task.status)
                                            setEditDeadline(task.deadline ?? "")
                                            setShowEditModal(true);
                                        }}>Edit
                                    </button>

                                    <button
                                        className="delete-button"
                                        onClick={() => handleDeleteTask(task.id)}
                                    >
                                        Delete
                                    </button>
                                </div>

                            </div>                           
                        ))}
                    </div>
                )}
            </div>

            {showCreateModal && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3>Create Task</h3>

                        <input
                            value={newName}
                            onChange={e => setNewName(e.target.value)}
                            placeholder="Task name"
                        />

                        <textarea
                            value={newDescription}
                            onChange={e => setNewDescription(e.target.value)}
                            placeholder="Description(optional)"
                        />

                        <input
                            type="date"
                            value={newDeadline}
                            onChange={e => setNewDeadline(e.target.value)}
                        />

                        <div className="modal-actions">
                            <button onClick={handleCreateTask} className="accept-button">
                                Create
                            </button>
                            <button
                                onClick={() => setShowCreateModal(false)}
                                className="cancel-button"
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {showEditModal && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3>Edit Task</h3>

                        <input
                            value={editName}
                            onChange={e => setEditName(e.target.value)}
                            placeholder="Task name"
                        />

                        <textarea
                            value={editDescription}
                            onChange={e => setEditDescription(e.target.value)}
                            placeholder="Description(optional)"
                        />

                        <input
                            type="date"
                            value={editDeadline}
                            onChange={e => setEditDeadline(e.target.value)}
                        />

                        <div className="modal-actions">
                            <button onClick={handleUpdateTask} className="c">Save</button>
                            <button onClick={() => setShowEditModal(false)}className="cancel-button">Cancel</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}