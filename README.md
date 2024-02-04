# Task Manager Application
## NHL Stenden | Final Assignment | C#2

### 1. Objective
Develop a task manager application using C# that falls under the category of applications with a standard Windows GUI. The application should allow users to manage their tasks efficiently.

### 2. Diagrams
#### 2.1. Unified Modeling Language (UML) 
| Class | Description |
| --- | --- |
| TaskManager | Manages the overall application. Contains a list of the task lists and the currently selected task. Provides methods to create and remove lists. Includes methods for sorting tasks. | 
| TaskList | Represents a task list screen. Holds a list of tasks. Provides methods to add, edit, and remove tasks. Implements sorting and filtering functionality. |
| Task | It represents a task with attributes like name, due date, priority, and status. |
| Priority | This enum represents the priority of a task (High, Medium, Low). |
| Status | This enum represents the status of a task (Completed, In Progress, Not Started). |

[image]

#### 2.2. Use Case
| Class | Description |
| --- | --- |

[image]

#### 2.3. Component
| Class | Description |
| --- | --- |

[image]

### 3. Features
| Name | Version | Date | Note |
| --- | --- |
| Visual Studio 2022 | 17.8 | January 22, 2024 | The official source of the project. |
| WPF | 4.5 | February 6, 2023 | Giving a visual presentation of the project. | 
| .NET Community Toolkit | 8.2 | April 27, 2023 | Including libraries like Microsoft MVVM Toolkit, Common, Diagnostics, HighPerformance |
| GitHub | 3.11.4 | January 30, 2024 | Version Control for the project. |

### 4. Test Plan
Ensure the Task Manager application functions as intended, providing users with efficient task management capabilities.

#### 4.1 Task Creation
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.1.1 | Start | Open the application. |
| 4.1.2 | New Task | Create a new task with various attributes (name, due date, priority, status). | The application successfully creates a new task. |
| 4.1.3 | Save | Save the task. | The application stops running. |

#### 4.2 Task Editing
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.2.1 | Start | Open the application. | The application starts running. |
| 4.2.2 | Edit Task | Edit an existing task, modifying its attributes. | The application successfully edits the task. |
| 4.2.3 | Save | Save the task. | The application stops running. |

#### 4.3 Task Deletion
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.3.1 | Start | Open the application. | The application starts running. |
| 4.3.2 | Remove Task | Delete an existing task. | The application successfully removes the selected task. |
| 4.3.3 | Save | Save the task. | The application stops running. |

#### 4.4 Task List Sorting
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.4.1 | Start | Open the application. | The application starts running. |
| 4.4.2 | Edit Task | Sort tasks by due date. | The tasks are going to be sorted by the selected due date. |
| 4.4.3 | Edit Task | Sort tasks by priority. | The tasks are going to be sorted by the selected priority |
| 4.4.4 | Edit Task | Sort tasks by status. | The tasks are going to be sorted by the selected status. |
| 4.4.5 | Save | Save the task. | The application stops running. |

#### 4.5 Task Filtering
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.5.1 | Start | Open the application. | The application starts running. |
| 4.5.2 | Remove Task | Apply filters to display tasks based on due date. | The tasks are going to be filtered by the selected due date. |
| 4.5.3 | Remove Task | Apply filters to display tasks based on priority. | The tasks are going to be filtered by the selected priority. |
| 4.5.4 | Remove Task | Apply filters to display tasks based on status. | The tasks are going to be filtered by the selected status. |
| 4.5.5 | Save | Save the task. | The application stops running. |

#### 4.6 Task List Interaction
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.6.1 | Start | Open the application. | The application starts running. |
| 4.6.2 | Edit Task | Select a task from the list. | Task details are displayed, and options (edit, delete) are available. |
| 4.6.3 | Save | Save the task. | The application stops running. |

#### 4.7 User Interface Responsiveness
| Step | Title | Description | Expected Result |
| --- | --- | --- | --- |
| 4.7.1 | Start | Open the application. | The application starts running. |
| 4.7.2 | Window | Resize the application window. | UI elements adapt to different window sizes without loss of functionality. |
| 4.7.3 | Close | Close the application. | The application stops running. |

### 5. MoSCoW
#### 5.1 Must-Have
| Title | Description |
| --- | --- |
| --- | As a user... |

#### 5.2 Should-Have
| Title | Description |
| --- | --- |
| --- | As a user... |

#### 5.3 Could-Have
| Title | Description |
| --- | --- |
| --- | As a user... |

#### 5.4 Won't-Have
| Title | Description |
| --- | --- |
| --- | As a user... |

### 6. Timetable
| Date | Title | Description | 
| --- | --- | --- |
| 2024-02-04 | Documentation | Create the UML, a use case, and a component diagram. Write the start document. Plan out the project for the next days and weeks. Write user stories (MoSCoW). |
| 2024-02-05 | Development | Building up the structure of the project within Visual Studio. |
| 2024-02-06 | Development | Building up the necessary methods of the project. |
| 2024-02-07 | Development | Building up the remaining methods of the project. |
| 2024-02-08 | Development | Building up the user interface of the Windows application. |
| 2024-02-09 | Development | Building up the user interface of the Windows application. |
| 2024-02-10 | Testing | Testing out the different actions of the Windows application. |
| 2024-02-11 | Documentation | Write a User Manual for the Windows application.  |
*The dates might be changed during the process of the project.*

### 7. User Manual
#### 7.1 Overview
This application is used to increase productivity and it was a great way to practice the C# programming language for something useful.

#### 7.2 Getting Started
...

#### 7.3 User Interface
| Pages | Description |
| --- | --- |
| Home | --- |
| Task List | --- |

#### 7.4 Task Management
| Action | Description |
| --- | --- |
| Create | --- |
| Edit | --- |
| Delete | --- |
| Sort | --- |
| Filter | --- |

### 8. Additional Information
Virag Szabo | BS | Information Technology | February 2024
