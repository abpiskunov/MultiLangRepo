import { v4 as uuidv4 } from 'uuid';
import { Task, TaskFilter, TaskStats, Priority, Status } from '../models/task';

export class TaskService {
    private tasks: Map<string, Task> = new Map();

    create(title: string, description: string, priority: Priority = 'medium'): Task {
        const task: Task = {
            id: uuidv4(),
            title,
            description,
            priority,
            status: 'todo',
            tags: [],
            createdAt: new Date(),
            updatedAt: new Date(),
        };
        this.tasks.set(task.id, task);
        return task;
    }

    getById(id: string): Task | undefined {
        return this.tasks.get(id);
    }

    getAll(): Task[] {
        return Array.from(this.tasks.values());
    }

    filter(criteria: TaskFilter): Task[] {
        let results = this.getAll();

        if (criteria.status) {
            results = results.filter(t => t.status === criteria.status);
        }
        if (criteria.priority) {
            results = results.filter(t => t.priority === criteria.priority);
        }
        if (criteria.tag) {
            results = results.filter(t => t.tags.includes(criteria.tag!));
        }
        if (criteria.assignee) {
            results = results.filter(t => t.assignee === criteria.assignee);
        }
        if (criteria.dueBefore) {
            results = results.filter(t => t.dueDate && t.dueDate <= criteria.dueBefore!);
        }

        return results;
    }

    updateStatus(id: string, status: Status): Task | undefined {
        const task = this.tasks.get(id);
        if (task) {
            task.status = status;
            task.updatedAt = new Date();
        }
        return task;
    }

    addTag(id: string, tag: string): Task | undefined {
        const task = this.tasks.get(id);
        if (task && !task.tags.includes(tag)) {
            task.tags.push(tag);
            task.updatedAt = new Date();
        }
        return task;
    }

    assign(id: string, assignee: string): Task | undefined {
        const task = this.tasks.get(id);
        if (task) {
            task.assignee = assignee;
            task.updatedAt = new Date();
        }
        return task;
    }

    setDueDate(id: string, dueDate: Date): Task | undefined {
        const task = this.tasks.get(id);
        if (task) {
            task.dueDate = dueDate;
            task.updatedAt = new Date();
        }
        return task;
    }

    delete(id: string): boolean {
        return this.tasks.delete(id);
    }

    getStats(): TaskStats {
        const all = this.getAll();
        const now = new Date();

        const stats: TaskStats = {
            total: all.length,
            byStatus: { todo: 0, in_progress: 0, done: 0, cancelled: 0 },
            byPriority: { low: 0, medium: 0, high: 0, critical: 0 },
            overdue: 0,
        };

        for (const task of all) {
            stats.byStatus[task.status]++;
            stats.byPriority[task.priority]++;
            if (task.dueDate && task.dueDate < now && task.status !== 'done' && task.status !== 'cancelled') {
                stats.overdue++;
            }
        }

        return stats;
    }

    sortByPriority(tasks?: Task[]): Task[] {
        const priorityOrder: Record<Priority, number> = {
            critical: 0,
            high: 1,
            medium: 2,
            low: 3,
        };
        const list = tasks ?? this.getAll();
        return [...list].sort((a, b) => priorityOrder[a.priority] - priorityOrder[b.priority]);
    }
}
