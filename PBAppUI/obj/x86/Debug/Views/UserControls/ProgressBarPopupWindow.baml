   M S B A M L   `   `   `  ����  PBAppUI<    6PasswordBoss.Views.UserControls.ProgressBarPopupWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          �Name$  ProductTourControl��5       � 
ResizeMode$ NoResize=�6*    � Title$ Password Boss��5	      
������  � AllowsTransparency .  � WindowStyle$
 None=�5
   (    � WindowStartupLocation$ CenterOwner=�5       � ShowInTaskbar .   � 	Resources 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      � ��5   
   �� 5       �� Color$ #000000��6   	 �� Opacity$		 0.5Q�6.   .+�5   
   � -   5        
 �Name$
 ProgressBarGrid��5      $��Center=�6(   $��Center=�6E     
WhiteColor#���  5       K� ZIndex$ 1��69   y�5      �� *ProgressBarVisibility+ �� 	Converter# [   � ColumnDefinitions 5      �� 5      $	��200��6#   $��* �62    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      � $��1��6   $��0��6    $��2��60   $��Stretch=�6D   .+�5      � -   5        $��pbStatus��5       � Minimum$ 0Q�6.   $��3��5    A   $��2��6N   $��Bottom=�6^   $��26��5!      $��Center=�6*    � Maximum 5   :   �� *ProgressBarMaxValue+ � Value 5       �� *ProgressBarValue+5!   *   �� 5"      $��Center=�6Y   $��Center=�5#      ��5"      �� *ProgresBarPercentage+ �� StringFormat$ {0:0}%��5#      5$      5%      5&      5'      