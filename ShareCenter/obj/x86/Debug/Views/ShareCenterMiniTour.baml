   M S B A M L   `   `   `  ����F  BShareCenter, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5      ,    &PasswordBoss.Views.ShareCenterMiniTour   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6helpers"clr-namespace:PasswordBoss.Helpers     5      -   5          �Name$  ShareCenterTour��5	       � 
ResizeMode$ NoResize=�6'    � Title$ Password Boss��5
      
������  � AllowsTransparency .  � WindowStyle$
 None=�5   (    � WindowStartupLocation$ CenterOwner=�5       � ShowInTaskbar .  $
��Auto��5      $
��Auto��6    � 	Resources 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      � $	��600��6   $	��888��6   .+�5   
   � -   5         �Name$ ShareTabsGrid��5      
������ y�6&   �� *ShareTabsVisibility+	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� Mode$
 TwoWay=� �� 	Converter# [   � ColumnDefinitions 5      �� 5      $	��183 �6#   �� 5      $��* �6#   �� 5      $��35 �6#    � RowDefinitions 5      �� 5      $	��138 �6    �� 5      $	��145 �6    �� 5      $��* �6    .+�5!      � $��0��6   $��0��6    $��3��60     	GrayColor#���  6A    J� Opacity$	 0.4Q�6j   � 5#      $��1��6   $��1��6    $��2��60   #���  6A   $	 0.4Q�6j   � 5%      $��0��6   $��2��6    $��3��60   #���  6A   $	 0.4Q�6j   � 5'      $��0��6   $��1��6    $	��105��60   $	��Top=�6=   #���  6U   $	 0.4Q�6~   � 5)      $��1��6   $��Bottom=�6#   $��33��6>    5*      �� 5+      $	��360 �6'   �� 5,      .+�5/      � $��0��6   $��Bottom=�6'   
������ 6B   � 51      $��1��6   #���  6'   $	 0.4Q�6P   53      �� 55      -   5        $��FavoriteSitesPopup��55   +    K� ZIndex$ 2��6   $��1��6E   $��1��6R   $	��Top=�6c   $
��Left=�56      $��0,0,0,0q�68   .+�57      � $
��Auto��6   $
��Auto��6%   .+�58      �  59      �� 5:      $��15 �6,   �� 5;       5=      �� 5>      $��* �6/   .+�5@      F� $��0��6   $��10��6,   $��15��67    �� Stretch$
 Fill=�6E   
�������
�������+���� G j%р�y?р��^� G j%$��Bottom=�5A      $��Center=�6:   $��10,0,0,0q�6W   z�5B      �� 5C   "    �� Angle$ 90Q�62   5E      � 5F      $��1��6     
WhiteColor#���  6,   $��0,-6,0,0q�6V   $
��Left=�6i   $	��125��6�    5G      �� 5H   "   $��10 �60   �� 5I   "   $
��Auto �60   �� 5J   "   $��10 �60   �� 5K   "   $
��Auto �60   �� 5L   "   $��10 �60   �� 5M   "   $��25 �60   �� 5N   "   $��10 �60    5P      �� 5Q   "   $��30 �63   �� 5R   "   $	��140 �63   �� 5S   "   $��30 �63   .+�5U      �� $��1��6)   $��3��69    �� Orientation$ Vertical=�6F   .+�5V   "   �� $)��#See a list of all your shared items��6,   $��14(�6W      LightGrayTextForegroundColor#���  6e   $��Normal"�6�     ProximaRegular#���  5W   ,    �� 
LineHeight$ 24��6Z   $
��Wrap=�6j   5X      � 5Y      $��5��6#   $��1��60    5Z   "   �� 5[   &   $��40 �67   �� 5\   &   .+�5_   "   �� $	��1/3��6,   $��16(�67     
BlackColor#���  6E   $��SemiBold"�6o   #���  6�   �� 5`   "   -   5        $��btnFavoriteSitesNext��5`   )   $��1��6E   $��90��6V     Next#���  6a   $��22��6�   $��11(�6�   #���  6�   $��SemiBold"�6�     PasswordBossGreenColor#���  6�   #���  5a   )   $��Right=�6z     MiniTourNextButtonStyle#���  5b   )   $��Center=�6[   $��Center=�6~   5c      5d      5e      5f      5g      5h      5k      5l      