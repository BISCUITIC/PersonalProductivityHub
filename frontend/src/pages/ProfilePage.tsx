
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import "../styles/ProfilePage.css";
import { getAllProjects, deleteProject, createProject, updateProject } from "../api/ProjectApi";
import type { CreateProjectRequest, UpdateProjectRequest } from "../types/Project";
import { logout } from "../api/AuthApi";

interface Project {
    id: string;
    name: string;
    description: string | null;
    createdAt: Date;
}

interface ProfilePageProps {
    setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function ProfilePage({ setIsAuthenticated }: ProfilePageProps) {
    const [projects, setProjects] = useState<Project[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isCreating, setIsCreating] = useState(false);
    const [editingProject, setEditingProject] = useState<Project | null>(null);

    const [showCreateModal, setShowCreateModal] = useState(false);
    const [newName, setNewName] = useState("");
    const [newDescription, setNewDescription] = useState("");

    const [showEditModal, setShowEditModal] = useState(false);
    const [editName, setEditName] = useState("");
    const [editDescription, setEditDescription] = useState("");

    const navigate = useNavigate();    

    useEffect(() => {
        const fetchProjects = async () => {
            try {
                const data = await getAllProjects();
                setProjects(
                    data.map((value) => ({
                        ...value,
                        createdAt: new Date(value.createdAt),
                    }))
                );
            } catch {
                setProjects([]);
            } finally {
                setIsLoading(false);
            }
        };

        fetchProjects();
    }, []);
   
    const handleDelete = async (id: string) => {
        if (!window.confirm("Are you sure you want to delete this project?")) return;
        try {
            await deleteProject(id);
            setProjects(projects.filter((p) => p.id !== id));

        } catch (error) {
            console.error("Failed to delete project", error);
        }
    };
    
    const handleCreate = async () => {
        if (!newName.trim() || isCreating) return;

        const request: CreateProjectRequest = {
            name: newName,
            description: newDescription || null,
        };

        setIsCreating(true);

        try {
            const created = await createProject(request);
            setProjects([
                ...projects,
                { ...created, createdAt: new Date(created.createdAt) },
            ]);

            setNewName("");
            setNewDescription("");
            setShowCreateModal(false);
        } catch (error) {
            console.error("Failed to create project", error);
        } finally {
            setIsCreating(false);
        }
    };

    const handleUpdate = async () => {
        if (!editingProject) return;

        const request: UpdateProjectRequest = {};

        if (editName !== editingProject.name) {
            request.name = editName;
        }

        if (editDescription !== (editingProject.description ?? "")) {
            request.description = editDescription;
        }

        try {
            const updated = await updateProject(editingProject.id, request);            
            setProjects(prev =>
                prev.map(p =>
                    p.id === editingProject.id ? { ...updated, createdAt: new Date(updated.createdAt) } : p
                )
            );

            setEditName("");
            setEditDescription("");
            setEditingProject(null);
            setShowEditModal(false);
        } catch (error) {
            console.error("Failed to update project", error);
        }
    };

    async function handleLogout () {       
        try {            
            await logout();

            setIsAuthenticated(false);
            navigate("/login");

        } catch (error) {
            console.error("Failed to delete project", error);
        }
    };

    if (isLoading) return <div className="isLoading">isLoading projects...</div>;

    return (
        <div className="profile-page">
            <header className="profile-header">
                <h2 className="profile-logo">My Profile</h2>
                <button className="logout-button" onClick={handleLogout}>Logout</button>
            </header>

            <div className="profile-container">
                <div className="projects-section">
                    <div className="projects-header">
                        <h3>My Projects</h3>
                        <button className="create-button" onClick={() => setShowCreateModal(true)} disabled={isCreating}>
                            {isCreating ? "Is creating..." : "Create project"}
                        </button>
                    </div>

                    {projects.length === 0 ? (
                        <p>No projects yet</p>
                    ) : (
                        <div className="projects">
                                <div className="projects-grid">
                                    {projects.map((project) => (
                                        <div key={project.id} className="project-card">

                                            <div className="project-info">
                                                <h4>{project.name}</h4>

                                                <p>{project.description ?? "No description"}</p>

                                                <small>{project.createdAt.toLocaleString()}</small>
                                            </div>

                                            <div className="project-actions">
                                                <button className="view-button"
                                                        onClick={()=>navigate(`/projects/${project.id}`) }
                                                >View
                                                </button>

                                                <button className="edit-button"
                                                    onClick={() => {
                                                        setEditingProject(project);
                                                        setEditName(project.name);
                                                        setEditDescription(project.description ?? "");
                                                        setShowEditModal(true);
                                                    }}>Edit
                                                </button>

                                                <button className="delete-button"
                                                        onClick={() => handleDelete(project.id)}
                                                >Delete
                                                </button>
                                            </div>
                                        </div>
                                    ))}
                             </div>
                        </div>
                    )}
                </div>
            </div>

            {showCreateModal && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3>Create Project</h3>
                        <input
                            type="text"
                            autoFocus
                            maxLength={128}
                            placeholder="Project Name"
                            value={newName}
                            onChange={(e) => setNewName(e.target.value)}
                        />
                        <textarea 
                            rows={10}
                            maxLength={1024}
                            style={ {resize : "none"} }
                            placeholder="Description (optional)"
                            value={newDescription}
                            onChange={(e) => setNewDescription(e.target.value)}
                        />
                        <div className="modal-actions">
                            <button onClick={handleCreate} className="accept-button">Create</button>
                            <button onClick={() => setShowCreateModal(false)} className="cancel-button">Cancel</button>
                        </div>
                    </div>
                </div>
            )}

            {showEditModal && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3>Edit Project</h3>
                        <input
                            type="text"
                            maxLength={128}
                            placeholder="Project Name"
                            value={editName}
                            onChange={(e) => setEditName(e.target.value)}
                        />
                        <textarea
                            rows={10}
                            maxLength={1024}
                            style={{ resize: "none" }}
                            placeholder="Description (optional)"
                            value={editDescription}
                            onChange={(e) => setEditDescription(e.target.value)}
                        />

                        <div className="modal-actions">
                            <button onClick={handleUpdate} className="accept-button">Save</button>
                            <button onClick={() => setShowEditModal(false)} className="cancel-button">Cancel</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}