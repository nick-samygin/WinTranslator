   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:PasswordBoss.UserControlsPasswordBoss.UserControls  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   3    -PasswordBoss.UserControls.AddSecureItemWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:PasswordBoss.UserControls     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	   	   1   +PasswordBoss.UserControls.CustomChildWindow    MaxContentHeight$	  446Q�5
       � 	Resources 5      ;  5Telerik.Windows.Controls.BooleanToVisibilityConverter    BooleanToVisibilityConverterC  =Telerik.Windows.Controls.InvertedBooleanToVisibilityConverter ( $InvertedBooleanToVisibilityConverter  FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray   OptionMark.Static.Background �� Color  OptionMarkFocusVisual # OptionMark.MouseOver.Background  OptionMark.MouseOver.Border  OptionMark.MouseOver.Glyph " OptionMark.Disabled.Background 	 OptionMark.Disabled.Border 
 OptionMark.Disabled.Glyph ! OptionMark.Pressed.Background  OptionMark.Pressed.Border  OptionMark.Pressed.Glyph  OptionMark.Static.Glyph  AddItemCheckBoxStyle �� 
TargetType �� 
TargetType	 �Name
 � ColumnDefinitions ��Name F�Name J� Opacity �� Stretch ��Name ��Name �� Triggers P� Property P� Value �� 
TargetName  ListBoxItemStyle �� RelativeSource �� AncestorType �� Padding ]� 
Conditions �� Property �� Value   LightGrayTextForegroundColor��%d  &	        &	      &	 &     &	 �     &	 �     &	 �    &	 �    &	 �    &	     &		 0    &	
 S    &	 v    &	 �    &	 �    &	 �    &	      7  7  7  7  7  7 	 7 
 7 
 7  7  &	 o    7    5   
   6/     5   
   67   �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5      5      5      �� 5   
   $ 	#FFFFFFFF��6?   �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��14,0,0,0q�6$    . #��� ��6Q   $��1��6�   $	 1 2R�6�   5      5!      5"      �� 5#   
   $ 	#FFF3F9FF��6B   �� 5$   
   $ 	#FF5593FF��6>   �� 5%   
   $ 	#FF212121��6=   �� 5&   
   $ 	#FFE6E6E6��6A   �� 5'   
   $ 	#FFBCBCBC��6=   �� 5(   
   $ 	#FF707070��6<   �� 5)   
   $ 	#FFD9ECFF��6@   �� 5*   
   $ 	#FF3C77DD��6<   �� 5+   
   $ 	#FF212121��6;   �� 5,   
   $ #1A3852��6:   �� 5-   
   " ��6-   .�5.      ��  �@�FocusVisualStyle8   61   �� 5/       � ��8  6+   �� 50       � �� �#B9B9B96,   �� 51       � �� �#8F99A06+   �� 52       � ��$ 13(�6)   �� 53       � ��$ 1q�60   �� 54       � ��$ 0 12 0 0q�6'   �� 55       � ��$ Center=�69   �� 56       � �� 57      �� 58      " ��6&   .R�59      � $	 templateRoot��6   
������  . 
 5:      �� 5;   "   $
��Auto �63   �� 5<   "   $��* �63   .+�5>      �� $ checkBoxBorder��6%   $��16��6=   $��16��6H   #��z��6T   #��z��6�   #��z��6�   #��z��6�   $��1q�6!  #��z��6,  ._�5?   "   � $	 markGrid��6'   .+�5A   &   F� $ 
optionMark��6+   $��Stretch=�6?   $��Stretch=�6]   Y�����4� �� Ѽ�G dԊ Ѩ�D dԊ �    ^>O ��z r�5 ��@ ��\ 
ܺ��� @�j�  �>��4� �� 
�������$��1q�6  $ 0Q�6#  $
 None=�6/  �� 5B   &   $ indeterminateMark��60   
���  ��$��2q�6V   $ 0Q�6a   5C   #   5D      �� 5E      $ contentPresenter��6/   $��1��6I   |�.  #��z��6k   $��8 0 0 0q�6�   ��. # z 6�   #��z��6  5F       5G      P� 5H       � �� . . �5I   "   �� 8   �@�FocusVisualStyle6E   �� 5J   "    � ��$ 4,-1,0,0q�6<   5K      P� 5L       �@J�IsMouseOver . . �5M   "   �� $ checkBoxBorder��6@    � �� �#152E456\   �� 5N   "   $ 
optionMark��69    � ��
 �����6Q   �� 5O   "   $ indeterminateMark��69   8   � ��6X   5P      P� 5Q       � {� .  . �5R   "   �� $ checkBoxBorder��6?   8   � ��6[   �� 5S   "   $ checkBoxBorder��6@   8   � ��6\   �� 5T   "   $ 
optionMark��69   8   � ��6Q   �� 5U   "   $ indeterminateMark��69   8   � ��6X   5V      P� 5W       � �� . . �5X   "   �� $ checkBoxBorder��6?    � �� �#1A38526[   �� 5Y   "   $ checkBoxBorder��6@    � �� �#152E456\   �� 5Z   "   $ 
optionMark��69   8   � ��6Q   �� 5[   "   $ indeterminateMark��69   8 	  � ��6X   5\      P� 5]       �@d�	IsChecked$
 trueY�6;   . �5^   "   �� $ 
optionMark��6<    �@J�Opacity$ 1Q�6T   �� 5_   "   $ indeterminateMark��6<    �@J�Opacity$ 0Q�6[   �� 5`   "   $ checkBoxBorder��6?    � �� �#1A38526[   �� 5a   "   $ checkBoxBorder��6@    � �� �#152E456\   5b      P� 5c       6;   Z�  �@d�	IsChecked. �5d   "   �� $ 
optionMark��6<    �@J�Opacity$ 0Q�6T   �� 5e   "   $ indeterminateMark��6<    �@J�Opacity$ 1Q�6[   5f      5h      5j      5k      �� 5q   
   " ��6)   .�5r      ��  �@J�SnapsToDevicePixels . 64   �� 5s       � ��$
 12 1q�6'   �� 5t       � ��$ 0q�6(   �� 5u       � �� 6;   �� *HorizontalContentAlignment+ �� " ���� 5v       � �� 69   �� *VerticalContentAlignment+ �� " ���� 5w       � ��
 ���� 6+   �� 5x       � ��
 ���� 6,   �� 5y       � ��$ 1q�60   �� 5z       �@�FocusVisualStyle8   61   �� 5{       � �� 5|      �� 5}      " ��6&   .R�5~      �� $ Bd��6!   #��z��6-   #��z��6Y   #��z��6�   # z��6�    . ._�5      �� #��z��6/   # z 6r   #��z��6�   5�       5�      ]� 5�       5�   "   �� 5�   &    �@J�IsMouseOver . 6G   ./�5�   "   �� $ Bd��6@   # �  6P    � ��5�      ]� 5�       5�   "   �� 5�   &    �@��IsSelectionActive .  6V   �� 5�   &    �@��
IsSelected . 6F   ./�5�   "   �� $ Bd��6@   # �  6P    � ��5�      ]� 5�       5�   "   �� 5�   &    �@��IsSelectionActive . 6V   �� 5�   &    �@��
IsSelected . 6F   ./�5�   "   �� $ Bd��6@   # �  6P    � ��5�      P� 5�       � {� .  . �5�   "   �� $ Bd��6K   # � ��6[    � ��5�      5�      5�      5�      5�         TitelContent 5�      � 5�   
   $��	0 17 0 20q�6   $��Stretch=�6"   
 5�      �� 5�      $
��auto �6#   �� 5�      $��* �6#    � RowDefinitions 5�      �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6    .+�5�      �� -   5         ��Name$ txtTitel��5�   �   $��0 0 0 12q�6   $
��Left=�6+   $��2��6F     OBProximaRegular#���  6�   $
��Wrap=�6    SettingsTextLableForeground#���  6  $��30(�6S  ��6Z   �� *Title+ �� 3   -PasswordBoss.UserControls.AddSecureItemWindow"  6S  � 5�      $��1��6   $
��Left=�6#   $��0 0 12 0q�6>   .+�5�      �� $��65��6   $	��130��6%   $��Stretch=�6c   $��Stretch=�6�   ��62   �� *
Background+ �� Mode$ TwoWay=�6�   �� 5�      -   5         ��Name$
 icon��5�   2   $��65��6   $	��130��6&   $��Center=�6[   $��Center=�6x   $
��None=�6�   ��6@   �� *Image+6�   5�      0  *Telerik.Windows.Controls.RadDropDownButton  5�      -   5           Name$  colorPicker��5�   (   $��1��5�   (   $��1��66     RadDropDownButtonStyle#���  5�   (   $
��Left=�6Y   $��Center=�6t   y�5�   =   �� *CanPickColor+! �� 	Converter#! [  "   IsOpen" 5�   F   �� *IsOpenColorPicker+$ TwoWay=���5�      �� 5�      # �� Orientation$# 
Horizontal=�6%   .+�5�      ��   colorPicker#���  6$   $��16��6K   $��16��6V   $��Bottom=�6c   �� 5�      $
��Left=�6)   $ �� TextAlignment$
$ Left=�6D   $��9 0 0 0q�6Y   $��Center=�6j   ���#7F7F7F$��0��5�   2   $��2��6B   % �� Padding$% 0,0,0,0q�5�      $��14(�6/   $��Light"�6=   $
��Wrap=�6R     ProximaRegular#���  5�        ChooseIconColor#���  6K   5�      &   DropDownContent& 5�      � 5�      .+�5�      �� #��[ 6"   $��10q�6Z     SecureItemsColors#���  6f   $��0q�6�   ' �� SelectedItem' 6�   �� *
Background+$ TwoWay=���5�      �� 5�   "   .R�5�   &   A� ( A� Columns$( 3��62   5�   #   ��5�      �� 5�   "   .R�5�   &   �� $��40��6-   $��20��68   ) �� CornerRadius$) 2��6D   $��5q�6U   ��6`   �� *.+5�   #   5�      �� 5�      $	��Top=�6"   $��0,5,5,0q�6g     NoHoverEffectButtonStyle#���  6x   $��0q�6�   $��Right=�6�   
������ ��6;   �� *CloseColorPickerCommand+.��5�      ��   imgCloseAlert#���  6$   $��8��6M   $��8��6X   5�      5�      5�      � 5�      -   5        $	 topDetailsGrid��5�   0   $��1��6   $��1��6#   $y�	Collapsed=�6H   $��Right=�6_   
 5�      �� 5�      �� 5�      $��21 �6'   �� 5�       5�      �� 5�      �� 5�      �� 5�      .+�5�      �� $��Right=�6    $  PasswordBossBlackBackgroundColor#���  69   $
��Wrap=�6y   $
$ Left=�5�      $��0 0 0 5q�61   $% 0,0,0,0q�5�   !   $��12(�64   $��SemiBold"�6B   #���  5�   !   $��Created:��6O   �� 5�      $��2��6   $
��Left=�6-   $
$ Left=�6H   $��0 0 0 5q�6]   $% 0,0,0,0q�5�      $	��Top=�6/   $��12(�6G   $
��Wrap=�6U   $��Normal"�5�      #���  60   #���  5�      ��6K   �� *CreatedDate+�� 5�      $��1��6   $��Right=�6+   $��0 0 0 3q�6H   #���  5�      $
��Wrap=�6]   $
$ Left=�5�      $% 0,0,0,0q�5�   !   $��12(�64   $��SemiBold"�6B   #���  5�   !   $��Last Updated:��6O   �� 5�      $��1��6   $��2��6)   $
��Left=�6;   $
$ Left=�6V   #���  5�      $��0 0 0 5q�6Y   $% 0,0,0,0q�5�      $	��Top=�6/   $��12(�6G   $
��Wrap=�6U   $��Normal"�6i   #���  5�      ��6K   �� *LastModifiedDate+�� 5�      $��2��6   $��Right=�6&   $��19��6D   $��16��6O   $	��Top=�6\   * �� BitmapScalingMode$* HighQuality=�6u     Sharing+ � ToolTip#+ �  6�     imgSharePeopleHover#���  6�   �� 5�      $
��Left=�6   $��2��68   $��2��6E     SettingsGreenColor#���  5�   !   $
��Wrap=�6S   $
$ Left=�6g   $% 0,0,0,0q�5�   !   $��Bottom=�63   $��12(�6N   $��Normal"�6\   #���  5�   !     SharedWithMe#���  6O   5�      5�      .��5�      � $��0 7 0 2q�6   $
��Left=�6   $	��448��67   $	��430��6C    5�      �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5      $
��auto �6   .+�5     �� -   5        , ��Name$, contentControl��5     $
��Left=�66   ��6Q   �� *.+- � DataContext- 6g   �� *.+�� 5  
   -   5        $��0 10 0 2q�5     $��1��6$   $
��Left=�61      CommonButtonStyle#���   6L   ��6�   �� *AddNoteCommand+y�6�   �� *	HasNote+#! [ .��5     �� $# 
Horizontal=�6   .+�5     ��  ! ItemFieldAddNote#��� ! 6    " AddItemTextBlock#��� " 6F   $��0q�6q   $��Bottom=�6|   �� 5      # noteGrey#��� # 6   $��15��6A   $��18��6L   $��8 0 0 0q�6X   $��Bottom=�6i   5     5	     � 5
  
   $
��0 10q�6   $��2��6y   y�6"   �� *	HasNote+#! [   5     �� 5     �� 5     $��* �6    .+�5     ��  $ ItemNote#��� $ 6   #��� " 6:   �� 5     #���   6   $��Right=�6�   $	��9 3q�6�   $��0q�6�   y�6A   �� *HasNoteHistory+#! [  .��5     ��  % historyGrey#��� % 6   $
��None=�6?   5     �� 5     -   5        . ��Name$. notesTxt��5  0   $��97��6   $��1��6#    & AddItemTextBox#��� & 6Y   $	��Top=�6�   / �� AcceptsReturn/ . 0 �� TextWrapping$
0 Wrap=�6�   ��6C   �� *Note+.��6�   .  (System.Windows.Interactivity.Interaction1   Triggers1 5     /  )System.Windows.Interactivity.EventTrigger  5     2   	EventName$2 	LostFocus��6%   .  (System.Windows.Interactivity.TriggerBase3   Actions.3 5     6	  0System.Windows.Interactivity.InvokeCommandAction	  4 	  Command4 60   �� *NoteLostFocusCommand+5     .��5  *   5     5     9� 5  
   -   5        5 9�Name$5 advancedExpander��5  !   $��3��6    ' SimpleExpanderStyle#��[' 5     $��0 15 q�6A   y�6P   �� *HasAddvancedSettings+#! [  ��5     �� 5      ( ItemFieldAdvancedSettings#��� ( 6    ) SettingsTextBoxBorderColor#��� ) 6O   $��12(�6�   $��SemiBold"�6�   .��5!     � 
 5"     �� 5#     $��* �6'   �� 5$     $��* �6'    5&     �� 5'     �� 5(     .+�5*     ��  * 	AutoLogin#��� * 6   #��[ 6m   6 d� 	IsChecked6 6A   �� *	Autologin+$ TwoWay=�6m   �� 5+      + 	Subdomain#��� + 6   $��1��6n   #��[ 6{   6 6A   �� *	SubDomain+$ TwoWay=�6{   �� 5,      , UseSecureBrowser#��� , 6   $��1��6|   #��[ 6�   6 6H   �� *UseSecureBrowser+$ TwoWay=�6�   �� 5-      "- ItemFieldRequireMasterPassword#��� - 6   $��1��6V   $��1��6�   #��[ 6�   6 6c   �� *RequireMasterPassword+$ TwoWay=�6�   5.     5/     50     51     