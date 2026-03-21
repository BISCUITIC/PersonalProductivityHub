
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import "../styles/ProfilePage.css";
import { getAllProjects, deleteProject, createProject } from "../api/ProjectApi";
import type { ProjectRequest } from "../types/Project";
import { logout } from "../api/AuthApi";

interface Project {
    id: string;
    name: string;
    description: string | null;
    createdAt: Date;
}

interface LoginPageProps {
    setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function ProfilePage({ setIsAuthenticated }: LoginPageProps) {
    const [projects, setProjects] = useState<Project[]>([]);
    const [loading, setLoading] = useState(true);
    
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [newProjectName, setNewProjectName] = useState("");
    const [newProjectDescription, setNewProjectDescription] = useState("");

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
                setLoading(false);
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
        if (!newProjectName.trim()) return;

        const request: ProjectRequest = {
            name: newProjectName,
            description: newProjectDescription || null,
        };

        try {
            const created = await createProject(request);
            setProjects([
                ...projects,
                { ...created, createdAt: new Date(created.createdAt) },
            ]);

            setNewProjectName("");
            setNewProjectDescription("");
            setShowCreateModal(false);
        } catch (error) {
            console.error("Failed to create project", error);
        }
    };

    async function handleLogout () {       
        try {
            console.log("ASDDD");
            await logout();

            setIsAuthenticated(false);
            navigate("/login");

        } catch (error) {
            console.error("Failed to delete project", error);
        }
    };

    if (loading) return <div className="loading">Loading projects...</div>;

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
                        <button className="create-button" onClick={() => setShowCreateModal(true)}>Create Project</button>
                    </div>

                    {projects.length === 0 ? (
                        <p>No projects yet</p>
                    ) : (
                        <div className="projects-grid">
                            {projects.map((project) => (
                                <div key={project.id} className="project-card">
                                    <h4>{project.name}</h4>
                                    <p>{project.description ?? "No description"}</p>
                                    <small>{project.createdAt.toLocaleString()}</small>
                                    <div className="project-actions">
                                        <button className="view-button">View</button>
                                        <button className="delete-button" onClick={() => handleDelete(project.id)}>Delete</button>
                                    </div>
                                </div>
                            ))}
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
                            value={newProjectName}
                            onChange={(e) => setNewProjectName(e.target.value)}
                        />
                        <textarea 
                            rows={10}
                            maxLength={1024}
                            style={ {resize : "none"} }
                            placeholder="Description (optional)"
                            value={newProjectDescription}
                            onChange={(e) => setNewProjectDescription(e.target.value)}
                        />
                        <div className="modal-actions">
                            <button onClick={handleCreate} className="create-button">Create</button>
                            <button onClick={() => setShowCreateModal(false)} className="cancel-button">Cancel</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}