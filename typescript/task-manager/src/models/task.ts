export type Priority = 'low' | 'medium' | 'high' | 'critical';
export type Status = 'todo' | 'in_progress' | 'done' | 'cancelled';

export interface Task {
    id: string;
    title: string;
    description: string;
    priority: Priority;
    status: Status;
    tags: string[];
    createdAt: Date;
    updatedAt: Date;
    dueDate?: Date;
    assignee?: string;
}

export interface TaskFilter {
    status?: Status;
    priority?: Priority;
    tag?: string;
    assignee?: string;
    dueBefore?: Date;
}

export interface TaskStats {
    total: number;
    byStatus: Record<Status, number>;
    byPriority: Record<Priority, number>;
    overdue: number;
}
