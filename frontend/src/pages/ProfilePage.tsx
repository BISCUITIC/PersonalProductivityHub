import { useState } from "react";
import "../styles/ProfilePage.css";

interface Project {
    id: number;
    name: string;
    description: string;
}

export default function ProfilePage() {
    const [projects] = useState<Project[]>([
        { id: 1, name: "Project Alpha", description: "Description Alpha" },
        { id: 2, name: "Project Beta", description: "Description Beta" },
        { id: 3, name: "Project Gamma", description: "Description Gamma" },
    ]);

    return (
        <div className="profile-page">
            <header className="profile-header">
                <h2 className="profile-logo">My Profile</h2>
                <button className="logout-button">Logout</button>
            </header>

            <div className="profile-container">
                <div className="projects-section">
                    <h3>My Projects</h3>
                    {projects.length === 0 ? (
                        <p>No projects</p>
                    ) : (
                        <div className="projects-grid">
                            {projects.map((project) => (
                                <div key={project.id} className="project-card">
                                    <h4>{project.name}</h4>
                                    <p>{project.description}</p>
                                    <button className="view-button">View</button>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}