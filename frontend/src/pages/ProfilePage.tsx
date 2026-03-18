import { useEffect, useState } from "react";
import { getAllProjects } from "../api/ProjectApi";
import type { Project } from "../models/Project";

interface ProjectProps {
    id: string;
    name: string;
    description?: string | null;
    createdAt: Date;
    onClick: (id: string) => void;
    onEdit: (id: string) => void;
    onDelete: (id: string) => void;
}

function ProjectCard({ id, name, description, createdAt, onClick, onEdit, onDelete }: ProjectProps) {
    return (
        <div
            onClick={() => onClick(id)}
            style={{
                backgroundColor: "#fff",
                borderRadius: "12px",
                boxShadow: "0 4px 10px rgba(0,0,0,0.1)",
                padding: "1rem",
                width: "220px",
                cursor: "pointer",
                display: "flex",
                flexDirection: "column",
                justifyContent: "space-between",
                transition: "transform 0.2s, box-shadow 0.2s",
                flex: "1 1 calc(33% - 1rem)"
            }}
        >
            <div>
                <h3 style={{ margin: "0 0 0.5rem 0" }}>{name}</h3>
                {description && <p style={{ fontSize: "0.9rem", color: "#555" }}>{description}</p>}
                <small style={{ color: "#888" }}>Created: {createdAt.toLocaleDateString()}</small>
            </div>

            <div style={{ display: "flex", gap: "0.5rem", marginTop: "1rem" }}>
                <button onClick={e => { e.stopPropagation(); onEdit(id); }}>Edit</button>
                <button onClick={e => { e.stopPropagation(); onDelete(id); }}>Delete</button>
            </div>
        </div>
    );
}

//interface UserProfile {
//    id: string;
//    userName: string;
//    email: string;
//    createdAt: Date;
//}

// Заглушка данных для шаблона
//const dummyUser: UserProfile = {
//    id: "1",
//    userName: "JohnDoe",
//    email: "johndoe@example.com",
//    createdAt: new Date("2023-01-01")
//};

export default function ProfilePage() {
    const [projects, setProjects] = useState<Project[] | null>(null);
    //const user = dummyUser;

    useEffect(() => {       
        getAllProjects().then(setProjects).catch(console.error);
    }, []);



    if (!projects) return <div>Loading...</div>;

    //const handleAddProject = () => {
    //    alert("Добавить проект (будет подключено к API)");
    //};

    const handleEditProject = (projectId: string) => {
        alert(`Редактировать проект ${projectId}`);
    };

    const handleDeleteProject = (projectId: string) => {
        alert(`Удалить проект ${projectId}`);
    };

    const handleProjectClick = (projectId: string) => {
        alert(`Перейти в проект ${projectId}`);
    };

    return (
        <div>
            <h1>User Profile</h1>
            <h2>My Projects</h2>
            <div style={{ display: "flex", flexWrap: "wrap", gap: "1rem" }}>
                {projects.map(p => (
                    <ProjectCard
                        id={p.id}
                        name={p.name}
                        description = { p.description }
                        createdAt = { p.createdAt }
                        onClick={handleProjectClick}
                        onEdit={handleEditProject}
                        onDelete={handleDeleteProject}
                    />
                ))}
            </div>
        </div>
    );
}