import { TaskService } from './services/task-service';

function main(): void {
    const service = new TaskService();

    console.log('=== Task Manager Demo ===\n');

    // Create tasks
    const task1 = service.create('Set up CI/CD pipeline', 'Configure GitHub Actions for build and deploy', 'high');
    const task2 = service.create('Write unit tests', 'Add tests for TaskService class', 'medium');
    const task3 = service.create('Update README', 'Add installation and usage instructions', 'low');
    const task4 = service.create('Fix authentication bug', 'Users are logged out after 5 minutes', 'critical');

    // Add tags
    service.addTag(task1.id, 'devops');
    service.addTag(task2.id, 'testing');
    service.addTag(task3.id, 'docs');
    service.addTag(task4.id, 'bug');
    service.addTag(task4.id, 'security');

    // Assign
    service.assign(task1.id, 'alice');
    service.assign(task4.id, 'bob');

    // Set due dates
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    service.setDueDate(task4.id, tomorrow);

    const yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    service.setDueDate(task1.id, yesterday);

    // Update statuses
    service.updateStatus(task1.id, 'in_progress');
    service.updateStatus(task4.id, 'in_progress');
    service.updateStatus(task3.id, 'done');

    // Show all tasks sorted by priority
    console.log('All tasks (by priority):');
    for (const task of service.sortByPriority()) {
        console.log(`  [${task.priority.toUpperCase()}] ${task.title} - ${task.status} ${task.assignee ? `(${task.assignee})` : ''}`);
    }

    // Filter
    console.log('\nIn-progress tasks:');
    for (const task of service.filter({ status: 'in_progress' })) {
        console.log(`  - ${task.title}`);
    }

    // Stats
    const stats = service.getStats();
    console.log('\nTask Statistics:');
    console.log(`  Total: ${stats.total}`);
    console.log(`  Todo: ${stats.byStatus.todo}, In Progress: ${stats.byStatus.in_progress}, Done: ${stats.byStatus.done}`);
    console.log(`  Overdue: ${stats.overdue}`);
}

main();
