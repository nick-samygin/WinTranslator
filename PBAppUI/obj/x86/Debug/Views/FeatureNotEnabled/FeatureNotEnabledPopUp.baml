   M S B A M L   `   `   `  ����  PBAppUIA    ;PasswordBoss.Views.FeatureNotEnabled.FeatureNotEnabledPopUp   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          �Name$  ProductTourControl��5       � 
ResizeMode$ NoResize=�6*    � Title$ Password Boss��5	      
������  � AllowsTransparency .  � WindowStyle$
 None=�5
   (    � WindowStartupLocation$ CenterOwner=�5       � ShowInTaskbar .  ��5      ��  �� RelativeSource �� *Self+ �� Path$ Owner.Width���5      ��  �� *Self+$ Owner.Height�	 � 	Resources	 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      � ��5   
   �� 5      
 �� Color$

 Gray��6    �� Opacity$	 0.4Q�6+   .+�5   
   � -   5         �Name$ mainGrid��5      $	��309��6!   $	��640��6.     
WhiteColor#���  6:    � ColumnDefinitions 5      �� 5      $��12 �6#   �� 5      $��44 �6#   �� 5      $��* �6#   �� 5      $��50 �6#   �� 5      $��20 �6#    � RowDefinitions 5      �� 5      $��59 �6    �� 5       $��24 �6    �� 5!      $��24 �6    �� 5"      $��23 �6    �� 5#      $��15 �6    �� 5$      $��20 �6    �� 5%      $��* �6    �� 5&      $��36 �6    �� 5'      $��18 �6    .+�5)      �� $��0��6   $��0��6"   $��5��62   ���#1F4663._�5*      �  5+      �� 5,      $��12 �6+   �� 5-      $��44 �6+   �� 5.      $��* �6+   �� 5/      $��50 �6+   �� 50      $��20 �6+   .+�52      �� $��1��6   $��2��6,   $��42��6@   $��Center=�6L   $
��Left=�6g     imgPasswordBossWhiteLogo#���  6�   �� 53      -   5        $��btnCloseWindow��53   o     ProximaRegular#���  6   $��3��6K   $��2��6[     NoHoverEffectButtonStyle#���  6�   $��0,-2,2,0q�6�   $��0q�6�   $	��Top=�6�   $��Right=�6�   ���#9D9C9B
������ $��17(�6@  $��20��6N  $��25��6Y  .��6~  x55      56      57      �� 58      ���#302F34$��2��6-   $��2��6=   $��23(�6J   $��SemiBold"�6X   $��Center=�6n   #���  6�    	 Hello#���  6�   �� 59      ���#4D494F$��2��6-   $��4��6=   $��17(�6J   $��Center=�6X   #���  6s     FeatureNotEnabledText1#���  6�   �� 5:       �� LineStackingStrategy$ BlockLineHeight=�6    �� 
LineHeight$ 18��6?   $
��Wrap=�6O   $��2��6c   $��6��6s   $��17(�6�   $	��Top=�6�   #���  6�   �� 5;      ���#4D494F  FeatureNotEnabledText2#��  6+   6]    �� 5<      ���#302F34$��SemiBold"�6+     FeatureNotEnabledText3#��  6A   6s   5=      �� 5>      -   5        $��	btnCancel��5>      $��7��6&   $��2��63    $	  FeatureNotEnabledLinkButtonStyle#��� 	 6C    
 
BlackColor#��� 
 6~   $��Center=�6�   $
��Left=�6�     NoThanks#���  6�   6  �� 5?      -   5        $��
btnUpgrade��5?      $��7��6'   $��1��64   $��2��6D   $��15(�6X   $��SemiBold"�6f   $	��183��6|   $��36��6�   $��Right=�6�   $��Bottom=�6�   #���  6�     UpgradeUppercase#���  6�   #���  6&    SignInButtonStyleRed#���  6P  6�  5@      5A      5B      