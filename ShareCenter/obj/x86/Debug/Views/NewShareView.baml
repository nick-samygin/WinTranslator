   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullT6clr-namespace:PasswordBoss.UserControls;assembly=themePasswordBoss.UserControls  5      F BShareCenter, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:PasswordBoss.ConvertersPasswordBoss.Converters 5      %   PasswordBoss.Views.NewShareView   5   !   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      OuserControls6clr-namespace:PasswordBoss.UserControls;assembly=theme     5      <
converters%clr-namespace:PasswordBoss.Converters     5      $	��400��5	      1   +PasswordBoss.UserControls.CustomChildWindow    OkButtonCommand  5      ��  �� Path$ ShareCommand�   IsWindowMayClose 5      �� $ MayClose�   CancelCommand 5      �� $ BackCommand� � 	Resources 5
      6  0PasswordBoss.Converters.BoolToVisibilityInverter   BoolToVisibilityInverter��%   &	          5   
   6.   5         TitelContent 5      �� 5   
   .+�5      �� $��	0 23 0 15q�6     OBProximaRegular#���  6,     ShareNewSharedItemHeadline#���  6\   $
��Wrap=�6�     SettingsTextLableForeground#���  6�   $��30(�6�   5      .��5      � $	��530��6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ Vertical=�6"   $��0,25,0,0q�69   .+�5      �� #���  6   $��SemiBold"�6H   $��16(�6^     NameForTheNewShare#���  5      /   )PasswordBoss.UserControls.RequiredTextBox  5      $	��300��6+   $��30��6g     TextboxBorderbrush#���  6s      Text 6:   �� $ 	ShareName�	 �� Mode$	 TwoWay=�
   IsValid
 6�   �� $ IsShareNameValid�$	 TwoWay=��� 5       #���  6      LightGrayTextForegroundColor#���  6H   $��12(�6�   $��0,3,0,0q�6�     AddShareHint#���  5!      5"      �� 5$   
   $��1��6   $ Vertical=�6"   $��0,25,0,0q�69   .+�5%      �� #���  6   $��SemiBold"�6H   $��16(�6^     RecipientEmailAddresses#���  5&        5(      $	��300��6+   $��30��6c   #���  6o    6:   �� $ Email�$	 TwoWay=�
 6�   �� $ IsEmailValid�$	 TwoWay=��� 5*      #���  6   
���  ��$��12(�6Y   $��0,3,0,0q�6g    	 ValidEmailRequest#��� 	 5+      y�5,      �� $ IsEmailValid� �� 	Converter# [  5-      9� 5/   
   $��2��6    
 ShareExpanderStyle#��� 
 6    $��	0,25,0,25q�6M   ��50      �� 51        AdvancedSettings#���  6   $��5,0,0,0q�6F   #���  6W   #���  6�   $��12(�6�   $��SemiBold"�6�   ��53      � 54      $��0,5,0,0q�6    55      �� 56      $
��Auto �6(   �� 57      $
��Auto �6(   �� 58      $
��Auto �6(   .+�5;      �� $��0��6!   $ Vertical=�6.   $��0,7,0,0q�6E   .+�5<      �� #���  6$   $��SemiBold"�6T   $��16(�6j     
BlackColor#���  5=   $     MessageToRecipient#���  6N   �� 5?      $	��300��6"   #���  6O   $	��110��6�   $��0,5,0,0q�6�   ��61   �� $ Message�6�   5@      �� 5B      $��1��6!   $ Vertical=�6.   $��0,15,0,0q�6E   .+�5C      �� #���  6$   $��SemiBold"�6T   $��16(�6j   #���  5D   $     HowLong#���  5E   $   �� 5F      $
��Left=�6#     SettingsComboboxStyle#���  6>   $	��200��6n   $��32��6z   $��0,5,0,0q�6�   5G      �� 5I      $��2��6   $��0,15,0,0q�6,   ��5J      �� 5K      #���  6(   $��16(�6X   #���  6f    ' #PasswordVisibleToRecipientStatement#���  6�   5M      5N      5P      5Q      5R      