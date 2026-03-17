import { apiClient } from "./ApiClient";
import type { Project } from "../models/Project";

export interface ProjectResponse {
    id: string;
    name: string;
    description: string | null;
    createdAt: string;
}

export async function getAllProjects(): Promise<Project[]> {

    const response = await apiClient.get<ProjectResponse[]>("/projects");

    const projects = response.data.map(project => ({ ...project, createdAt: new Date(project.createdAt)}));

    return projects;
}