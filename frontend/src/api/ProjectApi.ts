import { apiClient } from "./ApiClient";
import { handleApiError } from "../api/HandleApiError"
import type { ProjectResponse, UpdateProjectRequest, CreateProjectRequest } from "../types/Project";

export async function getAllProjects(): Promise<ProjectResponse[]> {

    try {
        const response = await apiClient.get<ProjectResponse[]>("/projects");

        return response.data;
    }
    catch (error: any) {
        handleApiError(error);
    }
}

export async function getProjectById(id: string): Promise<ProjectResponse> {
    try {
        const response = await apiClient.get<ProjectResponse>(`/projects/${id}`);
        return response.data;
    } catch (error: any) {
        handleApiError(error);        
    }
}

export async function createProject(request: CreateProjectRequest): Promise<ProjectResponse> {
    try {
        const response = await apiClient.post<ProjectResponse>("/projects", request);
        return response.data;
    } catch (error: any) {
        handleApiError(error);        
    }
}

export async function updateProject(id: string, request: UpdateProjectRequest): Promise<ProjectResponse> {
    try {
        const response = await apiClient.patch<ProjectResponse>(`/projects/${id}`, request);
        return response.data;
    } catch (error: any) {
        handleApiError(error);        
    }
}

export async function deleteProject(id: string): Promise<void> {
    try {
        await apiClient.delete(`/projects/${id}`);
    } catch (error: any) {
        handleApiError(error);        
    }
}