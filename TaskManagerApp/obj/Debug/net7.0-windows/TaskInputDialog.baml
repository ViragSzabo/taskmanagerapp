   M S B A M L   `   `   `  ����U  QSystem.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a7,clr-namespace:System;assembly=System.RuntimeSystem  5   	    TaskManagerApp$   TaskManagerApp.TaskInputDialog   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   <sys,clr-namespace:System;assembly=System.Runtime     5   	     � Title$  Add New Task��5   	   $	��400��6   $	��400��6+    � WindowStartupLocation$ CenterOwner=�5   	    � WindowStyle$ 
ToolWindow=�5   	   .��5      � ���#F0F0F0.+�5	   
   �� $��20q�6   .+�5      �� $��Add New Task��6   $��Arial)�6,   $��20(�6?   $
��Bold"�6M   $��0,0,0,20q�6_   $��Center=�6q   �� 5      $��
Task Name:��6   $��16(�6*   $��0,0,0,5q�68   �� 5      -   5         ��Name$ TaskNameTextBox��5      $��0,0,0,10q�5      $��30��5      $��5q�5      $��14(�5      $��1q�5      ���#D3D3D3
�������$
|�True��5      �� 5      $��Description:��6   $��16(�6,   $��0,0,0,5q�6:   �� 5      -   5        $ TaskDescriptionTextBox��5      $��0,0,0,10q�5      $��60��5      $��5q�5      $��14(�5      $��1q�5       ���#D3D3D3
������� �� AcceptsReturn$
 True��5#      �� 5'      $��	Due Date:��6   $��16(�6)   $��0,0,0,5q�67   (  "System.Windows.Controls.DatePicker  5(      -   5          Name$ DueDatePicker��5(       �� Today   SelectedDate# Z 5)      $��0,0,0,10q�5*      �� 5-      -   5        $��Add Task��5-      $��0,10,0,0q�5/      $��35��50      $��16(�51      ���#007ACC
�������
������  � Cursor$
 Hand��55      ��56      �� 57      	 �� 
TargetType"	 ��6   .�58      �� 
 �� Property
 � �� �� Value �#007ACC67   �� 59      
 � ��
 �����67   �� 5:      
 �@�Cursor$
 Hand��63   �� 5;      
 � ��
 ���� 68   �� 5<      
 � ��$ 0q�6<   �� 5=      
 � ��$
 10,5q�64   �� 5>      
 � ��$ 0,10,0,0q�63   �� 5?      
 � ��$ 16(�65   �� 5@      
 � ��$ 35��63    �� Triggers 5A      P� 5B       P� Property �@J�IsMouseOver P� Value$
 True��6=   . �5C   "   �� 
 � �� �#005A996?   5D      P� 5E       � ��$
 True��6;   . �5F   "   �� 
 � �� �#003D666?   5G      5I      5K      5L      5M      5N      