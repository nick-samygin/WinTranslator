   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:PasswordBoss.UserControlsPasswordBoss.UserControls  5      ;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5      .    (PasswordBoss.UserControls.PasswordTextUC   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:PasswordBoss.UserControls     5      6helpers"clr-namespace:PasswordBoss.Helpers     5      -   5          >�Name$  passwordTextUC��5       � 	Resources 5	         PasswordBoxTemplate �� 
TargetType �� Property  
WhiteColor �� Value  DialogGrayColor �� 
TargetType J� SnapsToDevicePixels �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName��%�  &	        �� 5
   
   " G�6,   .�5      ��  � ��# �  6+   �� 5       � ��# �  6,   �� 5       � �� 5      �� 5      " G�6&   .R�5      �� $��Bd��6!   $��2q�6+   #��z��5   .   #��z��5   .    . $ 2��5   I   ._�5      �� $��PART_ContentHost��6+   #��z��6C   # z 5   +   5       5      P� 5      	 � {�
 .  . �5   "   �� $ PART_ContentHost��6)   # �  6]    � ��5      5      5      5      5       .��5!      �  � RowDefinitions 5"   
   �� 5#      �� 5$      �� 5%      .+�5'   
   �� -   5        $��txtVisiblePasswordbox��5'   \   $��27��6   $��3,0,3,0q�6      AddItemTextBox#���  63   $��Center=�6y   $��Stretch=�6�   $��Center=�6�   G� 5(   
   -   5        $��txtPasswordbox��5(   m   $��27��6    G� PasswordChar$ *��6"     AddItemTextForegroundColor#���  63   $��Center=�6�   $��Center=�6�   $��Stretch=�6�   $��3,3,3,0q�6�     ProximaRegular#���  5)      )   #PasswordBoss.Helpers.PasswordHelper   Attach .   
FontSize17#���  5+        roundedcornerPasswordBox#���  6G   ��5)   E   ��  �� ElementName$ passwordTextUC�� �� Path$ BorderBrush�   Password 5*      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType.   (PasswordBoss.UserControls.PasswordTextUC"  $ Password� �� Mode$ TwoWay=�5+   G   �� 5-   
   -   5        $��ImgShowHide��5-   ^   $y�Hidden=�6   $��13��6$   $��0 0 7 0q�60   $��Right=�6B   $
��Fill=�6q   $��Center=�6�   $��18��6�     imgLoginShowEye#���  5.      6<   � 51   
   -   5         �Name$ progressBar��51   Y   $	��0 7q�6   $��1��6#   $��5��60   $��Stretch=�6;    	 ProgressBarSetup2#��� 	 6n   �� 53   
   $��2��6    �� Orientation$ 
Horizontal=�6#   .+�54      ��  $
  PasswordBossBlackBackgroundColor#��� 
 6   $��12(�6Y   $��Password Strength:��6g   $��Center=�55   (   #���  56   (   $��Normal"�6W   �� 57      -   5         ��Name$ passwordMeterTxt��57      $��13 0 0 0q�64   #��� 
 58   (   $��12(�6h   $��Center=�59   (   #���  5:   (   $
��Bold"�6W   5;      5>      5?      