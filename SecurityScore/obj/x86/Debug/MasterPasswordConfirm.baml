   M S B A M L   `   `   `  ����H  DSecurityScore, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5   	   (    "PasswordBoss.MasterPasswordConfirm   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   *v"clr-namespace:PasswordBoss.Helpers  5   	   -   5          � Title$  MasterPwdBox��5   	   $	��285��6   $	��400��6,    � WindowStyle$
 None=�68    � 
ResizeMode$ NoResize=�6K    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   	   
������  � Topmost .  � ShowInTaskbar .   � 	Resources 5
         BoolToVisiblity " PasswordContentControlTemplate �Name	 G�Name  
WhiteColor  ProximaRegular  
FontSize17
 G� 	MaxLength ! CheckBoxReadAcceptBorderbrush G� 
CaretBrush ��Name �� 	MaxLength �� 
CaretBrush � ContextMenu d�Name  ShowHideToggleButtonStyle ��Name  imgLoginHideEye �� Triggers �� Value �� Binding �� ElementName �� 
TargetName  imgLoginShowEye �� Value �� Property !	 MasterPasswordDialogGrayColor��%�  &	        &	      �� 5   
   6'   �� 5   
   .R�5      � $ PasswordGrid��6   .+�5      G� $	 GlobalPasswordTextBox��6   -   5        $��8,0,40,0q�5   =   #���  6P   #���  5      $��38��6L   #���  5      $
 30��6F   $��Center=�5      # �  6@   $y�Visible=�5      5      �� 5      $ ShowTextBox��6   $��8,0,32,0q�6/   #���  6B   #���  5      $��38��6H   #���  6T   $��Center=�5      ���#2E383F$ 30��5      $y�Hidden=�5      # �  5       5   )   Z� .��5   W   5      d� 5      $ ShowPasswordCharsCheckBox��6   $��Right=�6B   $��0,0,12,0q�6^   
������ $��Center=�5   8   |�.  #���  5       $y�Hidden=�6S   .��5!      �� $ ShowHideImage��6   #���  63   $��27��6^   $��21��6i   5#      5$       5%      �� 5&      $
 True��6c    6   �� *	IsPressed+$ ShowPasswordCharsCheckBox��.a�5'      �� $ ShowHideImage��6/   # �  6J    � ���� 5(      $ ShowTextBox��63    � y�$ Visible=�6L   �� 5)      $ GlobalPasswordTextBox��63    � y�$ Hidden=�6V   5*      �� 5+      $ 	Collapsed��6d    6   �� *
Visibility+$ ShowPasswordCharsCheckBox��.a�5,      �� $ ShowTextBox��63   # � 	 6L    � ���� 5-      $ ShowTextBox��63    � y�$ Visible=�6L   5.      �� 5/      $
 True��6_    6   �� *	IsFocused+$ GlobalPasswordTextBox��.a�50      �� $ ShowTextBox��63    � y�$ Hidden=�6L   51      53      54      .��56      �  � RowDefinitions 57   
   �� 58      $��70 �6   �� 59      $��* �6   .+�5;   
   � -   5        $ 
HeaderGrid��5;      $��0��6#   #���  60   $��Stretch=�5<       � ColumnDefinitions 5=      �� 5>      $	��.4* �6#   �� 5?      $	��.6* �6#   .+�5A      ��  
 imgLoginLogoGreen#��� 
 6   $	��130��6A   $��45��6M   $��20,0,0,0q�6Y   �� 5B      -   5         ��Name$ BtnClose��5B      $��1��6'   $��0,0,0,0q�67    K� ZIndex$ 1��6Q   $��10��6c   $��10��5C      $��	0,10,10,0q�6!   $��Right=�5D      $	��Top=�61     NoHoverEffectButtonStyle#���  6I   .��5E      ��   imgCloseAlert#���  6   5F      5G      � 5I   
   $��1��6    $  PasswordBossBlackBackgroundColor#���  6    5J      �� 5K      $��35 �6    �� 5L      $��35 �6    �� 5M      $��40 �6    �� 5N      $��35 �6    �� 5O      $��40 �6    �� 5P      $��* �6     5S      �� 5T      $��67 �6#   �� 5U      $��* �6#   �� 5V      $��67 �6#   .+�5Y      �� $��1��6   $��1��6%   #���  65     MasterPasswordDialogHeader#���  6_   $��17(�5Z      #���  6&   $
��Left=�6T   $��5,0,0,0q�6o   �� 5\      -   5         ��Name$ PasswordContentControl��5\      $��1��6=   $��2��6M   #��[ 5]      5a      �� 5c      -   5         ��Name$ ErrorTextBox��5c      $��3��6.   $��1��6;   
���  ��$��15(�5d      #���  6&   $
��Left=�6T   $��5,5,0,0q�6o   � 5f      $��4��6   $��1��6     5g      �� 5h      $��* �6'   �� 5i      $	��120 �6'   .+�5l      �� -   5        
������  
 Cancel#���  5l   2   #���  6U   $��60��6   $��17(�6�   $��Right=�5m      $��0,0,25,0q�65   #���  6G   #���  5n       � Cursor$
 Hand��6d   �� 5p      -   5        $��4��5p      $��1��6&   $��Right=�66   $	��115��6R     SignInButtonStyle#���  6^   #���  5q   )     OK#���  6W   $��17(�5r        �� 	IsDefault  . 6'   5s      5z      5{      5g     