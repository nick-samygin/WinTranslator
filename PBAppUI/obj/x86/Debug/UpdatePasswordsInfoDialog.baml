   M S B A M L   `   `   `  ����  PBAppUI?    9PasswordBoss.Views.UserControls.UpdatePasswordsInfoDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          �Name$%  UpdatePasswordsInfoDialogWindow��5       � 
ResizeMode$ NoResize=�67    � Title$ Password Boss��5	      
������  � AllowsTransparency .  � WindowStyle$
 None=�5
   (    � ShowInTaskbar .   � UseLayoutRounding .  � 	Resources 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      � $	��600��6   $	��888��6   ��5   
   �� 5       �� Color$
 Gray��6   	 �� Opacity$		 0.4Q�6+   .+�5   
   �� -   5        $��AddPassword��5      $��0q�6%   $	��525��6:   
�������$��Center=�5      $��Center=�6(   ._�5      �  . 
 � RowDefinitions
 5      �� 5      $
��auto �6$   �� 5      $
��Auto �6$   �� 5      $
��auto �6$   �� 5      $
��Auto �6$   .+�5      �� -   5        $��Center=�5      $��Right=�64   $��15��5       $��15��6)     btnStyleNoHover#���  64   $��9q�5!      ��5#      �� 5$        imgWizardCloseLogo �� ImageSource# �  6%   5&      �� 5(      $��Update Passwords��6   $��31(�65   $��	41 11 0 7q�6C     PasswordBossBlueHeaderColor#���  5)   %     ProximaSemiBold#���  6`   $
��Left=�5*       �� 5+      $��Stretch=�6   $��1��6:   $��1��6E   ���#D7D7D76R   �� 5,      $��41 27 50 27q�6   $��2��62   .+�5-      �� $
��Wrap=�6!    $  PasswordBossBlackBackgroundColor#���  65     ProximaRegularFamily#���  5.   !   $��14(�6U     SecurityScoreInfo_Body#���  6c   �� 50      $��Follow this steps��6    #���  69   $��	0 29 0 27q�6h   $��15(�51   $   $��SemiBold"�62   #���  6H   $
��Wrap=�6�   �� 54        ProximaRegular#���  6%   $��9,0,0,19q�6S   $
��Wrap=�6e   $��14(�55   $   $��Normal"�62    #	 SecureScoreUpdateLightGrayColor#��� 	 6F   $	��Top=�6�   �� 56      $�1.��6"    �� 57       !
 SecurityScoreInfo_FirstStep_I#�� 
 6"    �� 58       " SecurityScoreInfo_FirstStep_II#��  6"    �� 59       # SecurityScoreInfo_FirstStep_III#��  6"   5:      �� 5<      #���  6%   $��9,0,0,19q�6S   $
��Wrap=�6e   $��14(�5=   (   $��Normal"�66   #��� 	 6J   $	��Top=�6�   �� 5>   "   $�2.��6&    �� 5?   "      SecurityScoreInfo_SecondStep#��  6&   5@      �� 5B      #���  6!   $��9,0,0,19q�6O   $
��Wrap=�6a   $��14(�5C   (   $��Normal"�66   #��� 	 6J   $	��Top=�6�   �� 5D   "   $�3.��6&    �� 5E   "     SecurityScoreInfo_ThirdStep#��  6&   5F      �� 5H      #���  6!   $��9,0,0,0q�6O   $
��Wrap=�6`   $��14(�5I   (   $��Normal"�66   #��� 	 6J   $	��Top=�6�   �� 5J   "   $�4.��6&    �� 5K   "    " SecurityScoreInfo_FourthStep_I#��  6&    �� 5L   "    # SecurityScoreInfo_FourthStep_II#��  6&    �� 5M   "    $  SecurityScoreInfo_FourthStep_III#��  6&   5N      5P      �� 5Q      $��3��6    ! MainWindowGrayBackgroundColor#���  6&    �� Padding$ 41 21 40 21q�6c   ._�5R      �  � ColumnDefinitions 5S      �� 5T      $
��auto �6/   �� 5U      $��* �6/   �� 5V      $
��auto �6/   .+�5X      �� -   5         ��Name$ NeverShowCheckBox��5X   #   $��-2,0,0,0q�6>   $
��Left=�6P   $��Center=�6k    ) %AddPasswordAutomaticallyComboboxStyle#���  5Y       $
��Auto��6`   $	��250��6m    d� 	IsChecked 6|   �� *NeverShowChecked+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�.��5Z      ��  �� TextWrapping$
 Wrap=�6)     NeverShowAgain#���  6=    �� 
Foreground# � 	 6e   5[      �� 5\      -   5          WhiteSettingsButton#���  5\   "   $��1��6Q   $��Right=�6a    
 Cancel#���  5]   %   #���  6H   $	��117��6�   $��27��6�   #���  5^   %   �� 5_      -   5         ��Name$ btnOk��5_   b     BlueSettingsButton#���  6"   $��13 0 0 0q�6P   $��2��6q   $	��117��6�   $��27��6�     Continue#���  5`   $   #���  5a   %   ��5`   I   �� *UpdatePasswordContinueCommand+5a   T   5b      5c      5�      5�      5�      5�      