   M S B A M L   `   `   `  ����  PBAppUIG    APasswordBoss.Views.UserControls.CustomInformationMessageBoxWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � WindowStyle$
  None=�5      $��0q�62   
������  � ShowInTaskbar .   � AllowsTransparency . 
������  � 	Resources 5	         BoolToVisibility��%   &	        �� 5
   
   6'   5      .��5      � $��Stretch=�6   $��Stretch=�6)   ��5   
   �� 5       �� Color$
 Gray��6    �� Opacity$	 0.4Q�6+   .+�5   
   � -   5         �Name$ MessageBoxGrid��5      $��Center=�6'   $��Center=�6D   $	��300��6_     
WhiteColor#���  5       K� ZIndex$ 1��69    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   	 � RowDefinitions	 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      � $��0��6   $��0��6    $��2��60   $��Stretch=�6D   	 5      �� 5      $��30 �6$   �� 5      �� 5       $��10 �6$   �� 5!      �� 5"      $��2 �6$   �� 5#      �� 5$      $��15 �6$   .+�5&      �� $��1��6   $��0��6)   $��3��69   $��18(�6M     PasswordBoss#���  5'        TextForegroundcolor#���  5(      $��Center=�5)        ProximaRegular#���  69   $��SemiBold"�5*      �� 5+      -   5        
 ��Name$
 DialogMessageTextBlock��5+      $��3��6<   $��1��6I   $��	20,0,20,0q�6Y   $��14(�6l   $��Center=�5,         LightGrayTextForegroundColor#���  69    �� TextAlignment$ Center=�6u   $
��Wrap=�6�     ProximaLight#���  5-      5.      �� 50      -   5        $��1��50      $��0��6"   $��2��62   $��Center=�6F     PasswordBossGreenColor#���  6c   $��55��51      $	��300��6!   #���  6-   $��SemiBold"�52      $��16(�6+   $��0q�69     btnStyleNoHover#���  53       �� 	IsDefault .  	 ProximaSemiBold#��� 	 54       
 OK#��� 
 55      64   57      58      59      