import { apiClient } from "./ApiClient";
import { handleApiError } from "../api/HandleApiError"
import type { ProjectResponse, ProjectRequest } from "../types/Project";

export async function GetAll(): Promise<ProjectResponse[]> {

    try {
        const response = await apiClient.get<ProjectResponse[]>("/projects");

        return response.data;
    }
    catch (error: any) {
        handleApiError(error);
    }
}

export async function GetById(id: string): Promise<ProjectResponse> {
    try {
        const response = await apiClient.get<ProjectResponse>(`/projects/${id}`);
        return response.data;
    } catch (error: any) {
        handleApiError(error);        
    }
}

export async function Create(request: ProjectRequest): Promise<ProjectResponse> {
    try {
        const response = await apiClient.post<ProjectResponse>("/projects", request);
        return response.data;
    } catch (error: any) {
        handleApiError(error);        
    }
}

export async function Update(id: string, request: ProjectRequest): Promise<ProjectResponse> {
    try {
        const response = await apiClient.put<ProjectResponse>(`/projects/${id}`, request);
        return response.data;
    } catch (error: any) {
        handleApiError(error);        
    }
}

export async function Delete(id: string): Promise<void> {
    try {
        await apiClient.delete(`/projects/${id}`);
    } catch (error: any) {
        handleApiError(error);        
    }
}