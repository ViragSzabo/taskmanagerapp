   M S B A M L   `   `   `  ����  TaskManagerApp    TaskManagerApp.TaskView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Task Details��5   	   $	��522��6   $	��514��6+   ���#F9F9F9.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10q�6.   .+�5      �� $��Task Details��6   $��24(�6,   $
��Bold"�6:   $��0,0,10,0q�6L   �� 5      -   5        $��Close��5      $��80��6?   ���#007ACC
�������$��0,0,10,0q�6r   5      �� 5   
   -   5         ��Name$ TaskListBox��5      $	��150��6'   $��10q�5      ���#FFFFFF���#D3D3D3$��1q�5   (   ��5      �� *Tasks+��5      �� 5      .R�5      �� $
��Bold"�66   $��16(�6H   ��6    �� *Name+6H   5      5      �� 5   
   $��1��6   $��10q�6"   .+�5       �� $��24(�6;   $
��Bold"�6I   $��0,0,0,10q�6[   ��6   �� *SelectedTask.Name+6[   �� 5!      $��Description:��6   $
��Bold"�6,   �� 5"      $��0,0,0,10q�6@   $��80��6R    �� TextWrapping$
 Wrap=�6^   ���#F0F0F0���#D3D3D3$��1q�5#   A    �� 
IsReadOnly$
 True��6U   ��5"      �� *SelectedTask.Description+5#   U   �� 5$      $��	Due Date:��6   $
��Bold"�6)   �� 5%      $��0,0,0,10q�6B   ��6   �� *SelectedTask.DueDateTime+6B   �� 5&      $��	Priority:��6   $
��Bold"�6)   �� 5'      $��0,0,0,10q�6?   ��6   �� *SelectedTask.Priority+6?   �� 5(      $��Status:��6   $
��Bold"�6'   �� 5)      $��0,0,0,10q�6=   ��6   �� *SelectedTask.Status+6=   �� 5,      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5-      �� -   5        $
��Edit��5-      $��0,0,10,0q�6E   $��80��6W   ���#FFA500
�������6w   �� 5.      -   5        $��Mark as Complete��5.      $��0,0,10,0q�6W   $	��120��6i   ���#28A745
�������6�   5/      50      51      52      