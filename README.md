# Task Manager Application
## NHL Stenden | Final Assignment | C#2

### Table of Content
- [1. Objective](#1-objective)
- [2. Diagrams](#2-diagrams)
  - [2.1. Unified Modeling Language (UML)](#21-unified-modeling-language-uml)
  - [2.2. Use Case](#22-use-case)
- [3. Features](#3-features)
- [4. Test Plan](#4-test-plan)
  - [4.1 Task Creation](#41-task-creation)
  - [4.2 Task Editing](#42-task-editing)
  - [4.3 Task Deletion](#43-task-deletion)
  - [4.4 Task List Sorting](#44-task-list-sorting)
  - [4.5 Task Filtering](#45-task-filtering)
  - [4.6 Task List Interaction](#46-task-list-interaction)
  - [4.7 User Interface Responsiveness](#47-user-interface-responsiveness)
- [5. MoSCoW](#5-moscow)
  - [5.1 Must-Have](#51-must-have)
  - [5.2 Should-Have](#52-should-have)
  - [5.3 Could-Have](#53-could-have)
  - [5.4 Won't-Have](#54-wont-have)
- [6. Timetable](#6-timetable)
- [7. User Manual](#7-user-manual)
  - [7.1 Overview](#71-overview)
  - [7.2 Getting Started](#72-getting-started)
  - [7.3 User Interface](#73-user-interface)
  - [7.4 Task Management](#74-task-management)
- [8. Additional Information](#8-additional-information)

### 1. Objective
The primary objective of this project is to develop a task management application utilizing C#. This application aims to provide users with a straightforward yet effective interface for task administration. With a standard Windows graphical user interface, the application will facilitate the creation, viewing, updating, and deletion of tasks, thereby enhancing user productivity and organization.

#### 1.1 Key Features
- **Home Page:** This page functions as the central hub, providing an overview of task categories or recent tasks, as well as facilitating quick access to other views.
- **List View:** This feature offers users a categorized overview of all their tasks, thereby enhancing the ability to track multiple task lists (e.g., Work, Personal, etc.). Users can sort, filter, and search for tasks within each category.
**Task View:** This component presents detailed information regarding each task. Users may add new tasks, modify existing ones, or designate tasks as completed. The view accommodates task priorities, due dates, and status tracking.

### 2. Diagrams
#### 2.1. Unified Modeling Language (UML) 
| Class | Description | Methods |
| --- | --- | --- |
| TaskManager | It contains a list of task lists (TaskList objects) and tracks the currently selected task. | It provides methods for creating, removing, and updating task lists, as well as methods for sorting and filtering tasks at the application level. Acts as the "Controller" in an MVC architecture. | 
| TaskList | It represents a list of tasks, containing a collection of Task objects. | It provides methods for adding, editing, removing tasks, and performing list-level operations like sorting, filtering, or searching tasks. |
| Task | It represents an individual task. Attributes include task name, description, due date, priority, and status. It may also include methods to update its attributes. | It implements INotifyPropertyChanged to notify the UI when properties (such as status) are updated. |
| Priority | This is an Enum task representing task priority (High, Medium, Low). | It is used to prioritize tasks for better management and display purposes. |
| Status | This is an enum representing task status (Completed, In Progress, Not Started). | It provides the foundation for task tracking and filtering. |
| TaskView | A user interface component that displays details for a selected task. | It provides buttons and forms to edit the task, mark it as completed, or delete it. Communicates changes to TaskList. |
| ListView | The view that displays all tasks in a selected TaskList. | Users can sort, filter, or search for tasks from this view, and it updates based on the TaskList data source. |

#### 2.2 Class Relationships and Design Patterns
| Name | Description |
| --- | --- |
| **TaskManager** | It follows the Singleton pattern to ensure a single point of control for task management across the application. |
| **TaskList** | Each TaskList contains multiple Task objects, forming a one-to-many relationship. | 
| **Task** | It could implement the Observer pattern using events (e.g., INotifyPropertyChanged) to notify the user interface when task properties change. This ensures that updates in task details are reflected across all relevant views in real-time. | 
| **TaskView** and **ListView** | The Task class is bound to the user interface using data-binding principles. Whenever task attributes such as name, due date, or status are updated, the corresponding UI views are automatically updated to reflect these changes. | 
| **Status** and **Priority** | Both Priority and Status enums are used throughout the application for filtering, sorting, and intuitively displaying tasks. They also play a role in defining the business logic (e.g., only Progress tasks are displayed by default on the home screen). | 

![The UML Diagram](Diagrams/uml.png)

#### 2.3 Sequence Diagrams
It captures the interaction between different components or objects within the system over time.
##### 2.3.1 Task Creation
The steps to create a task by the user with appropriate methods involving list and UI updates.

![The UML Diagram](Diagrams/sequence.png)

#### 2.4 Activity Diagram
It shows the workflow of tasks and operations in the system.
##### 2.4.1 Task Completion
The workflow how a completion of a task looks like within the app.

![The UML Diagram](Diagrams/activity.png)

#### 2.5 Use Case
It helps represent the different actions that users can perform within the application.

![The Use Case Diagram](Diagrams/usecase-v2.png)

#### 2.6 Component Diagram
It focuses on the high-level architecture of the application, showing the different components that make up the system.
- UI Components: TaskView, ListView, MainWindow
- Business Logic Components: TaskManager, TaskList, Task
- Data Components: Storage or Data Handling logic

![The Use Case Diagram](Diagrams/component.png)

#### 2.7 State Diagram
Show the process of a task and how its state can change

![The Use Case Diagram](Diagrams/state.png)

### 3. Features
| Name | Version | Date | Note |
| --- | --- | --- | --- |
| Visual Studio 2022 | 17.8 | January 22, 2024 | The official source of the project. |
| WPF | 4.5 | February 6, 2023 | Windows Presentation Foundation (WPF) for creating the UI. | 
| .NET Framework | 7.0 | November 2022| The ASP.NET Core Runtime enables you to run existing web/server applications. |
| GitHub | 3.11.4 | January 30, 2024 | Version Control for the project. |
| Astah UML | 8.4 | February 10, 2024 | Used for creating class diagrams and other UML designs. |

### 4. Test Plan
Ensure the Task Manager application functions as intended, providing users with efficient task management capabilities.

#### 4.1 Create a list
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.1.1 | Start | Open the application. | The application starts running. |
| 4.1.2 | Create a new list | Create a new list with a name.	 | The home page is updated with the new list. |

#### 4.2 Create a task
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.1.1 | Start | Open the application. | The application starts running. |
| 4.1.2 | View a list | Select a task list.	 | The page of the selected list opens. |
| 4.1.3 | Create a new task | Open an input page of the task. | The task is successfully created. |
| 4.1.4 | Save | Save the attributes of the task. | The list page is updated with the new task. |

#### 4.3 Edit a task in a list
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.3.1 | Start | Open the application. | The application starts running. |
| 4.3.2 | View a list | Select a task list.	 | The page of the selected list opens. |
| 4.3.3 | View a task | Select a task.	| The page of the selected task opens. |
| 4.3.4 | Edit Task | Modify the task’s attributes (name, due date, priority, etc.).	| Task attributes are successfully updated. |
| 4.3.5 | Save | Save the edited task. | The list page is updated with the edited task. |

#### 4.4 Remove a task from a list
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.4.1 | Start | Open the application. | The application starts running. |
| 4.4.2 | View a list | Select a task list.	 | The page of the selected list opens. |
| 4.4.3 | View a task | Select a task to be removed.	| The page of the selected task opens. |
| 4.4.4 | Remove a task | Remove the task from the list.	 | The task is successfully deleted and removed from the list. |

#### 4.5 Sort a list of tasks (ASC)
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.5.1 | Start | Open the application. | The application starts running. |
| 4.5.2 | View a list | Select a task list.	 | The page of the selected list opens. |
| 4.5.3 | Sort by ASC order | Sort the tasks based on due date. | Tasks are correctly sorted by the names. |

#### 4.6 Sort DESC a list of tasks (DESC)
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.6.1 | Start | Open the application. | The application starts running. |
| 4.6.2 | View a list | Select a task list.	 | The page of the selected list opens. |
| 4.6.3 | Sort by DESC order | Sort the tasks based on priority. | Tasks are correctly sorted by the names. |

#### 4.7 Filter a list of tasks by date
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.7.1 | Start | Open the application. | The application starts running. |
| 4.7.2 | Filter by date | Filter the tasks based on the due date. | Tasks are correctly filtered by their due dates. |

#### 4.8 Filter a list of tasks by priority
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.8.1 | Start | Open the application. | The application starts running. |
| 4.8.2 | Filter by priority | Filter the tasks based on priority. | Tasks are correctly filtered by their priority. |

#### 4.9 Filter a list of tasks by status
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.9.1 | Start | Open the application. | The application starts running. |
| 4.9.2 | Filter by status | Filter the tasks based on status (e.g., In Progress, Completed). | Tasks are correctly filtered by their status. |

### 5. MoSCoW
#### 5.1 Must-Have
| Title | Description |
| --- | --- |
| Add a list | As a user, I want to organize my tasks. |
| Edit a list | As a user, I want to change the name of the list. |
| Remove a list | As a user, I want to remove a list. |
| Add a task | As a user, I want to add tasks to a list. |
| Edit a task | As a user, I want to update my tasks. |
| Remove a task | As a user, I want to remove old tasks from the list. |

#### 5.2 Should-Have
| Title | Description |
| --- | --- |
| View list | As a user, I want to view the tasks of a list. |
| View task | As a user, I want to see the details of a specific task. |
| Task Notifications | As a user, I want to receive notifications for upcoming, overdue tasks. |

#### 5.3 Could-Have
| Title | Description |
| --- | --- |
| Filter the tasks | As a user, I want to filter tasks based on attributes like due date, priority, or status within the list. |
| Sort the tasks | As a user, I want to sort tasks based on their names (ASC, DESC). |
| Sort the lists | As a user, I want to sort lists based on their names (ASC, DESC). |

#### 5.4 Won't-Have
| Title | Description |
| --- | --- |
| Attachments | Users won’t be able to attach images, documents, or other media to tasks. |
| Comments | Users won’t have the ability to add comments or notes within tasks. |
| Shares | There will be no built-in functionality for sharing tasks with other users. |

### 6. Timetable
*These dates might be changed during the process of the project.*

| Date | Title | Description | 
| --- | --- | --- |
| October 22 | Finalize the design, layout and tests. Comment and review. Create a new presentation video. |
| October 21 | Update the code. Fix up the diagrams, documentation and implementation according to the feedback. |
| May 1 - June 2 | End | Write a User Manual for the Windows application.  |
| February 13 | Test | Debug and make sure the application runs. |
| February 12 | Dev | Make a fancy UI. Add comments and review the code. |
| February 11 | Dev | Fixing UI and methods. Remain tasks: sort, filter, edit date, priority, status. |
| February 6 | Dev | Building up the necessary methods of the project. |
| February 5 | Dev | Building up the structure of the project within Visual Studio. |
| February 4 | Start | Create the UML and a use case diagram. Write the start document. Plan out the project for the next days and weeks. Write user stories (MoSCoW). |

### 7. User Manual
#### 7.1 Overview
This application significantly boosts productivity by providing a well-organized and systematic approach to task management. It allows users to easily prioritize, track, and manage their tasks in a streamlined manner. Additionally, this project serves as an excellent opportunity for honing C# programming skills, as it requires the implementation of various programming concepts and techniques. Ultimately, it delivers a practical and functional tool that helps individuals and teams efficiently organize their work and stay focused on their goals.

#### 7.2 Getting Started
| Step | Title | Description |
| --- | --- | --- |
| 1 | Download | Clone the repository from the provided GitHub link. |
| 2 | Open | Open the project in Visual Studio 2022. |
| 3 | Start | Build the solution and run the application. |
| 4 | Use | Begin using the application by creating and managing task lists and tasks. |

#### 7.3 User Interface
| Pages | Description |
| --- | --- |
| Home | 	Displays the main interface of the task manager application with the prioritized tasks and all lists. |
| Task List | Displays tasks within the selected list, including options to add, edit, or remove tasks. |
| Task View | Shows detailed information about a selected task, allowing users to modify task attributes. |

#### 7.4 Task Management
| Action | Description |
| --- | --- |
| Create List | Organize tasks by creating a new task list. |
| Edit List | Change the name of an existing task list. |
| Remove List | Remove a task list that is no longer needed. |
| Create Task | Add a new task to the selected list with attributes like name, due date, and priority. |
| Edit Task | Update attributes of an existing task, such as its name, due date, priority, or status. |
| Remove Task | Remove a task that is no longer relevant from the list. |
| View Task | See the details of a specific task, including its attributes. |
| Sort Tasks | Organize tasks based on attributes like name (ascending or descending). |
| Filter Tasks | Apply filters to display tasks based on attributes like due date, priority, or status. |

### 8. Additional Information
Virag Szabo | BS | Information Technology | February - October 2024
