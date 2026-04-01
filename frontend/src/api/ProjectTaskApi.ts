import { apiClient } from "./ApiClient";
import { handleApiError } from "./HandleApiError";

import type {
    ProjectTaskResponse,
    CreateProjectTaskRequest,
    UpdateProjectTaskRequest
} from "../types/ProjectTask";

const base = (projectId: string) => `/projects/${projectId}/tasks`;

export async function getAllProjectTasks(projectId: string): Promise<ProjectTaskResponse[]> {
    try {
        const response = await apiClient.get<ProjectTaskResponse[]>(`${base(projectId)}`);

        return response.data;
    } catch (error: any) {
        handleApiError(error);
    }
}

export async function getProjectTaskById(projectId: string, taskId: string): Promise<ProjectTaskResponse> {
    try {
        const response = await apiClient.get<ProjectTaskResponse>(`${base(projectId)}/${taskId}`);

        return response.data;
    } catch (error: any) {
        handleApiError(error);
    }
}

export async function createProjectTask(projectId: string, request: CreateProjectTaskRequest): Promise<ProjectTaskResponse> {
    try {
        const response = await apiClient.post<ProjectTaskResponse>(`${base(projectId)}`,request);

        return response.data;
    } catch (error: any) {
        handleApiError(error);
    }
}

export async function updateProjectTask(projectId: string, taskId: string, request: UpdateProjectTaskRequest): Promise<ProjectTaskResponse> {
    try {
        const response = await apiClient.patch<ProjectTaskResponse>(`${base(projectId)}/${taskId}`,request);

        return response.data;
    } catch (error: any) {
        handleApiError(error);
    }
}

export async function deleteProjectTask(projectId: string, taskId: string): Promise<void> {
    try {
        await apiClient.delete(`${base(projectId)}/${taskId}`);
    } catch (error: any) {
        handleApiError(error);
    }
}