   M S B A M L   `   `   `  ����  TaskManagerApp    TaskManagerApp.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Task Manager��5   	   $	��450��6   $	��800��6+   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10q�6.   .+�5      �� $��24(�6   $
��Bold"�6&   $��Task Manager��68   $��Center=�6L   �� 5      -   5         ��Name$ 	addButton��5      $��Add Task��6(   $
��10,0q�6Q   $	��100��6_   �� 5      -   5        $ removeButton��5      $��Remove Task��6+   $	��0,0q�6Z   $	��100��6g   �� 5      -   5        $ clearFilterButton��5      $��Clear Filter��60   $	��0,0q�6a   $	��100��6n   5      �� 5   
   $��1��6   $��10q�6"   .+�5      �� $��18(�6   $
��Bold"�6&   $"��Current High Priority Tasks:��68   $
��0,10q�6\   �� 5      -   5         ��Name$ highPriorityTasksListBox��5      $��0,0,0,10q�68   $	��150��6J   ��5      �� *HighPriorityTasks+��5      �� 5      .R�5      �� $
��Bold"�6:   
���  ����6$   �� *Name+6L   5      5      �� 5      $��18(�6   $
��Bold"�6&   $��
All Tasks:��68   $
��0,10q�6J   �� 5       -   5        $ taskListBox��5       $	��200��6+   ��5!      �� *Tasks+��5"      �� 5#      .R�5$      �� ��6$   �� *Name+5%      5'      5(      5)      5*      