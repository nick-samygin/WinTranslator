   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullT6clr-namespace:PasswordBoss.UserControls;assembly=themePasswordBoss.UserControls  5       PasswordVault0   *PasswordBoss.Views.PasswordVaultMenuButton   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ctelerik1http://schemas.telerik.com/2008/xaml/presentation    5      c	 _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity 	 5      Econtrols6clr-namespace:PasswordBoss.UserControls;assembly=theme 	 5      :d2http://schemas.microsoft.com/expression/blend/2008 	 5        � Cursor$
  Hand��6K    � 	Resources 5
         BoolToVisibility  FolderTemplate � ColumnDefinitions �� Orientation  HierarchicalDataTemplate �� ItemsSource��%�  &	        &	      &	     7  �� 5   
   6'   �� 5   
   .R�5      �  5      �� 5      $
��auto �6'   �� 5      $
��auto �6'   �� 5      $
��auto �6'   .+�5      �� $ 
Horizontal=�6   .+�5      �� $��0q�6#   ��6.   �� *
FolderName+5      5      5      �� 5   
    6#   �� *	ChildList+.R�5      �� 8��  ��6O   �� *.+5      5       .��5!      � -   5         �Name$ border��5!       5"   
   �� 5#      $��* �6#   $��40��6-   �� 5$      $
��auto �6#    � RowDefinitions 5&   
   �� 5'      $
��auto �6   �� 5(      $��* �6   ��5*   
   �� 5+       �� 
TargetType" �6    �� Triggers 5,      P� 5-      	 P� Property	 �@J�IsMouseOver
 P� Value
 . . �5.      ��  �� Property � �� �� Value �#2D4A5B67   5/      �� 50       �� Binding 6#   ��  �� ElementName$ btnPasswordVault�� �� Path$ 	IsChecked� �� Value True6d   .a�51      ��  � ��  AlertButtonBackgroundcolor# �  68   52      54      .+�56   
   1   +PasswordBoss.UserControls.MenuButtonControl  -   5          Name$ btnPasswordVault��56   J     	Passwords   Header# �  6%     lockGrey   ImageSource# �  6{     	lockGreen   SelectedImageSource# �  6�   5�      5�      