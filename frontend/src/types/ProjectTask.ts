export type StatusTask = 0 | 1 | 2;

export const StatusTaskLabel: Record<StatusTask, string> = {
    0: "New",
    1: "In Progress",
    2: "Done"
};

export interface ProjectTaskResponse {
    id: string;
    name: string;
    description: string | null;
    status: StatusTask;
    deadline: string | null;
    createdAt: string;
}

export interface CreateProjectTaskRequest {
    name: string;
    description: string | null;     
    deadline: string | null;
}

export interface UpdateProjectTaskRequest {
    name?: string;
    description?: string;
    status?: StatusTask,
    deadline?: string
}