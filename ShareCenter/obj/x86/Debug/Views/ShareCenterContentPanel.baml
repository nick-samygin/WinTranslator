   M S B A M L   `   `   `  ����F  BShareCenter, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:PasswordBoss.ViewsPasswordBoss.Views  5      ;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5      Q-clr-namespace:PasswordBoss.Views.UserControlsPasswordBoss.Views.UserControls  5	      0    *PasswordBoss.Views.ShareCenterContentPanel   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4pbviews clr-namespace:PasswordBoss.Views     5      c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      +ui"clr-namespace:PasswordBoss.Helpers  5      6uc-clr-namespace:PasswordBoss.Views.UserControls  5	      $
��Auto��5      $
��Auto��6     � 	Resources  5      .   (PasswordBoss.Helpers.ParametersConverter   ParametersConverter5   /PasswordBoss.Helpers.ActionToVisibiltyConverter  ActionToVisibilty  BoolToVisibility��%W   &	        &	      &	 &       5   
   6!     5   
   6(   �� 5   
   6'   5      .��5   
   � -   5         �Name$ sharecenter_grid��5      ���#e4e9e8 � RowDefinitions 5      �� 5      $��44 �6    �� 5      $��40 �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5      $��* �6#   .+�5      �� $��0��6   $��0��6"     
WhiteColor#���  63   $��Stretch=�6]   $��Stretch=�6{   ._�5      ��   ShareCenter#���  6     HeaderForegroundColorColor#���  6>   $��Center=�6x   $��22,0,0,0q�6�     ProximaSemiBold#���  5    $   $��14(�6S   $
��Bold"�6a   5!      �� 5#   
   $��1��6   $��2,0,0,0q�6#   
������ $��0,-1,0,0q�6W   #���  6i   $��Stretch=�6�    �� SelectedIndex 5$   !   �� *SelectedIndexTabControl+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�.  (System.Windows.Interactivity.Interaction   Triggers 5%      /  )System.Windows.Interactivity.EventTrigger  5&         	EventName$ SelectionChanged��6%   .  (System.Windows.Interactivity.TriggerBase	   Actions.	 5'      6  0System.Windows.Interactivity.InvokeCommandAction  
   Command
 60   �� *$"ShareCenterSelectionChangedCommand+$ OneWay=�   CommandParameter 5(   8   ��  �� ElementName$ SharecenterCombobox��5)        5+       �� 
TargetType �� Property �� Value �� 
TargetType  TextboxBorderbrush �� Orientation �� Triggers P� Property P� Value �� 
TargetName  PasswordBossGreenColor 	 
BlackColor 
 TextBoxBackground   LightGrayTextForegroundColor��%�  '��       �� 5,      " ��6   .�5-      ��  � �� 5.      �� 5/      " ��6.   .R�50   "   � $
 grid��6'   .+�51   &   �� $��Border��6-   #���  6;   $��0 2 0 0q�6m   ._�52   *   �� $ Vertical=�65   $��Center=�6L   $��Center=�53   5   .+�54   .   �� $��Header��6?   $��Center=�6V   $��Stretch=�55   ?   56   +   57   '   58   #    59   "   P� 5:   &    �@��
IsSelected . . �5;   *   �� $ Border��61   # �  6[    � ���� 5<   *   $ Border��61   # �  6\    � ���� 5=   *   $ Border��61    � ��$ 0,2,0,0q�6`   �� 5>   *   # � 	 6G    � ��5?   '   P� 5@   &    �@��
IsSelected .  . �5A   *   �� $ Border��61   # � 
 6[    � ���� 5B   *   $ Border��61    � ��$ 0,0,1,0q�6`   �� 5C   *   $ Border��61   # �  6\    � ���� 5D   *   # �  6G    � ��5E   '   5G      5I      5J      5K      .@�5N      �� $	��120��6   $��30��6&   ��5O      �� 5P        	Passwords#���  6    $��12(�6D   $��SemiBold"�6R     ProximaRegular#���  5Q   ,   5U      �� 5W      $	��120��6   $��30��6&   ��5X      �� 5Y        DigitalWallet#���  6    $��12(�6H   $��SemiBold"�6V   #���  5Z   ,   5^      �� 5`      $	��120��6   $��30��6&   ��5a      �� 5b        PersonalInfo#���  6    $��12(�6G   $��SemiBold"�6U   #���  5c   ,   5g      5h      � 5j      $��2��6   ���#ffff 5k      �� 5m      $��20 �6$   �� 5n      $��10 �6$   �� 5o      $��* �6$   �� 5p      $��20 �6$   �� 5q      $��20 �6$   �� 5r      $��10 �6$   �� 5s      $��* �6$    5u      �� 5v      $��0.02* �6'   �� 5w      $��1* �6'   �� 5x      $��0.02* �6'   .+�5~      �� $��0��6   $��1��6%     
SharedByMe#���  65   $
��12.8(�6Z     ProximaLight#���  5   0   $��SemiBold"�6\   � 5�      -   5        $ SharedByMeGrid��5�      $��2��6/   $��1��6<   $
��Auto��6M    5�      �� 5�      $��20 �6(   �� 5�      $��2 �6(   �� 5�      $��* �6(    5�      �� 5�      $
��1.5* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      �� $��0��6!   $��0��6.   $ 
Horizontal=�6>   $��Stretch=�5�   5   .+�5�      ��   	Recipient#���  6%   $
��12.8(�6H   #���  5�   9   #���  6g   �� 5�      $ Vertical=�6%   $��Right=�6<   $��Center=�5�   9   $��	12,-5,0,0q�6T   .+�5�      �� $��8��6    $��5��6*    �� BitmapScalingMode$ HighQuality=�65     
imgUpArrow#���  5�   ,   y�5�   c   �� *#!SortSharedByMeAscendingVisibility+ �� 	Converter# [  J� InputBindings 5�      d� 5�   "    d� MouseAction$ 	LeftClicke�6/    �� Command 6G   �� * SortSharedByMeAscendingCommand+5�      �� 5�      $��8��6    $��5��6*   $��0,2,0,0q�65     imgDownArrow1#���  5�   ,   $ HighQuality=�5�   ,   y�5�   F   �� *$"SortSharedByMeDescendingVisibility+# [  5�      d� 5�   "   $ 	LeftClicke�6/    6G   �� *!SortSharedByMeDescendingCommand+5�      5�      5�      �� 5�      $��0��6    $��1��6-     Nickname#���  6=   $
��12.8(�6_   #���  5�   4   #���  6b   $
��Left=�6�   �� 5�      $��0��6    $��2��6-     Sent#���  6=   $
��12.8(�6[   #���  5�   4   #���  6b   $
��Left=�6�   �� 5�      $��0��6    $��3��6-    
 Status#���  6=   $
��12.8(�6]   #���  5�   4   #���  6b   $
��Left=�6�   �� 5�      $��0��6    $��4��6-    
 Action#���  6=   $
��12.8(�6]   #���  5�   4   #���  6b   $
��Left=�6�   �� 5�      $��1��6    $��0��6-   $��5��6=   $��Stretch=�5�   4   $	��Top=�6R   #��� 
 6j   $��2��5�   4   $��0,0,0,0q�6?   �� 5�      -   5        $��ShareCenterPasswordsList��5�      $��2��6;   $��0��6H   $��6��6X   $��0q�5�   #   
������ $��Stretch=�5�   /     SecureShareListViewStyle#���  6N   $	��Top=�5�   /   $��Stretch=�6G    ! SecurityScoreListViewTemplate#���  5�   /   ��5�   /   ��  �� Path$ ShareCenterSentData�$ PropertyChanged=���5�      �� 5�   "   .R�5�   &   � $��Stretch=�6+   $��0 0 0 0q�6I    5�   *   �� 5�   .   $��30 �6<    5�   *   �� 5�   .   $
��1.5* �6?   �� 5�   .   $��* �6?   �� 5�   .   $��* �6?   �� 5�   .   $��* �6?   �� 5�   .   $
��0.5* �6?   �� 5�   .   $
��0.5* �6?   .+�5�   *   �� $��0��65   $��12(�6`   $
��Left=�6n     TextForegroundcolor#���  5�   5   #���  6h   $��Center=�5�   5   ��5�   E   �� *	recipient+5�   5   �� 5�   *   $��1��64   $��12(�6^   $��0,0,0,0q�6l   #���  5�   5   #���  6h   $
��Left=�6�   $��Center=�5�   5   ��5�   D   �� *
nickname+5�   5   �� 5�   *   $��2��65   $��12(�6F   $��0,0,0,0q�6j   #���  5�   =   #���  6p   $
��Left=�6�   $��Center=�5�   =   ��5�   T   �� *sent+5�   +   �� 5�   *   $��3��65   $��12(�6f   $��0,0,0,0q�6t   #���  5�   =   #���  6p   $
��Left=�6�   $��Center=�5�   =   ��5�   E   �� *localizedStatus+5�   =   �� 5�   &   $��4��6-   $��0,0,0,0q�6=   $
��Left=�6N   $��Center=�5�   -   $��Center=�6O   $��22��6j   $��60��6v     roundShareCentarButton#���  5�   -   ��5�   -   �� $ SharedWithMePasswords��$$ DataContext.CancelShareCommand���6�   �� *uuid+y�5�   .   �� *sharedStatus+# [  �� ConverterParameter$ cancel��.��5�   .   �� $ 
Horizontal=�69   $��Center=�6R   .+�5�   2   �� $��12(�6<    �� Padding$ 0,0,0,0q�6J   $��Center=�6\   #���  5�   4    
 Cancel#���  5�   4   5�   /   5�   '   �� 5�   &   $��4��6-   $��0,0,0,0q�6=   $
��Left=�6N   $��Center=�5�   -   $��Center=�6O   $��22��6j   $��60��6v   #���  5�   -   ��5�   -   �� $ SharedWithMePasswords��$$ DataContext.RevokeShareCommand���6�   �� *uuid+y�5�   -   �� *sharedStatus+# [ $ revoke��.��5�   *   �� $ 
Horizontal=�65   $��Center=�6N   .+�5�   .   �� $��12(�68   $ 0,0,0,0q�6F   $��Center=�6X   #���  5�   4    
 Revoke#���  5�   4   5�   +   5�   '   �� 5  &   $��4��6-   $��0,0,0,0q�6=   $
��Left=�6N   $��Center=�5  -   $��Center=�6O   $��22��6j   $��60��6v   #���  5  -   ��5  -   �� $ SharedWithMePasswords��$&  DataContext.SendDataShareCommand���6�   �� *uuid+y�5  -   �� *sharedStatus+# [ $ 	shareData��.��5  *   �� $ 
Horizontal=�65   $��Center=�6N   .+�5	  .   �� $��12(�68   $ 0,0,0,0q�6F   $��Center=�6X   #���  5
  4     	ShareData#���  5  4   5  +   5  '   �� 5"  &   $��5��6-   $��0,0,0,0q�6=   $��Center=�6N   $
��Left=�6i   #���  5#  9   $��12(�6i   $��SemiBold"�6w    
  Resend#���   6�   
������ #���  5$  9    ! CancelButton_style#��� ! 5%  9   ��5$  o   �� $ SharedWithMePasswords��$$ DataContext.ResendShareCommand���6�   �� *uuid+y�5&  9   �� *sharedStatus+# [ $ resend��5'  +   �� 5)  "   $��5��6)   $��0,0,0,0q�69   $��Center=�6J   $
��Left=�6e   #���  5*  9   $��12(�6i   $��SemiBold"�6w    
" Remove#��� " 6�   
������ #���  5+  9   #��� ! 5,  9   ��5+  o   �� $ SharedWithMePasswords��$$ DataContext.RemoveShareCommand���6�   �� *uuid+y�5-  9   �� *sharedStatus+# [ $ remove��5.  #   5/     50  #   52     54     �� 56     $��4��6   $��1��6)    # SharedWithMe#��� # 69   $
��12.8(�6_   #���  57  0   $��SemiBold"�6\   � 58     -   5        $ SharedWithMeGrid��58     $��6��6-   $��1��6:    59     �� 5:     $
��1.5* �6+   �� 5;     $��* �6+   �� 5=     $��* �6+   �� 5>     $��* �6+   �� 5?     $��* �6+    5A     �� 5B     $��20 �6(   �� 5C     $��2 �6$   �� 5D     $��* �6$   .+�5F     �� $��0��6!   $��0��6.   $ 
Horizontal=�6>   .+�5G     ��  
$ Sender#��� $ 6%   $
��12.8(�6E   #���  5H  9   #���  6g   �� 5I     $ Vertical=�6%   $��Right=�6<   $��Center=�5J  9   $��	12,-5,0,0q�6T   .+�5K     �� $��8��6    $��5��6*   $ HighQuality=�65   #���  5L  ,   y�5K  c   �� *%#SortSharedWithMeAscendingVisibility+# [  5M     d� 5N  "   $ 	LeftClicke�6/    6G   �� *" SortSharedWithMeAscendingCommand+5P     �� 5Q     $��8��6    $��5��6*   $��0,2,0,0q�65   #���  5R  ,   $ HighQuality=�5S  ,   y�5Q  F   �� *&$SortSharedWithMeDescendingVisibility+# [  5T     d� 5U  "   $ 	LeftClicke�6/    6G   �� *#!SortSharedWithMeDescendingCommand+5W     5Y     5Z     �� 5[     $��0��6    $��1��6-   #���  6=   $
��12.8(�6_   #���  5\  4   #���  6b   $
��Left=�6�   �� 5]     $��0��6    $��2��6-    % Received#��� % 6=   $
��12.8(�6_   #���  5^  4   #���  6b   $
��Left=�6�   �� 5_     $��0��6    $��3��6-    & Expires#��� & 6=   $
��12.8(�6^   #���  5`  4   #���  6b   $
��Left=�6�   �� 5a     $��0��6    $��4��6-   #���  6=   $
��12.8(�6]   #���  5b  4   #���  6b   $
��Left=�6�   �� 5e     $��1��6   $��0��6)   $��6��69   $��Stretch=�5f  4   $	��Top=�6R   #��� 
 6j   $��2��5g  4   $��0,0,0,0q�6?   �� 5i     -   5         ��Name$ SharedWithMePasswords��5i  #   $��0q�6B   $	��Top=�6V   $��2��5j  #   $��0��60   $��6��6A   $��Stretch=�6V   $��Stretch=�5k  8   #���  5l  7   $��0,0,0,0q�6w   #���  5m  #   ��5k  V   �� $ ShareCenterReceivedData���5n     �� 5o  "   .R�5p  "   � $��Stretch=�6'   $��0 0 0 0q�6E    5q  *   �� 5r  .   $
��1.5* �6?   �� 5s  .   $��* �6?   �� 5t  .   $��* �6?   �� 5v  .   $��* �6?   �� 5w  .   $
��0.5* �6?   �� 5x  .   $
��0.5* �6?    5z  *   �� 5{  .   $��30 �6<   .+�5}  *   �� $��0��65   $��12(�6]    ' foregroundsharecolor#��� ' 5~  4   #���  6h   $��0,0,0,0q�6�   $��Center=�5  4   $
��Left=�6O   ��5}  E   �� *sender+5  O   �� 5�  *   $��1��64   $��12(�6^   #��� ' 5�  4   #���  6h   $��Center=�5�  4   $
��Left=�6O   ��5�  D   �� *
nickname+5�  O   �� 5�  *   $��2��65   $��12(�6F   #��� ' 5�  4   #���  6h   $��Center=�5�  4   $
��Left=�6O   ��5�  T   �� *sent+5�  +   �� 5�  *   $��3��65   $��12(�6E   #��� ' 5�  4   #���  6h   $��Center=�5�  4   $
��Left=�6O   ��5�  S   �� *	expires+5�  l   �� 5�  &   $��4��6-   $��0,0,0,0q�6=   $
��Left=�6N   $��Center=�5�  -   $��Center=�6O   $��22��6j   $��60��6v   #���  5�  -   ��5�  ,   �� $ SharedWithMePasswords��$$ DataContext.AcceptShareCommand���6�   �� *uuid+y�5�  ,   �� *sharedStatus+# [ $ accept��.��5�  *   �� $ 
Horizontal=�65   $��Center=�6N   .+�5�  .   �� $��12(�68   $ 0,0,0,0q�6F   $��Center=�6X   #���  5�  4    
( Accept#��� ( 5�  4   5�  +   5�  '   �� 5�  &   $��4��6-   $��0,0,0,0q�6=   $
��Left=�6N   $��Center=�5�  -   $��Center=�6O   $��22��6j   $��60��6v   #���  5�  -   ��5�  ,   �� $ SharedWithMePasswords��$+ %DataContext.AcceptShareRequestCommand���6�   �� *uuid+y�5�  +   �� *sharedStatus+# [ $ acceptRequest��.��5�  *   �� $ 
Horizontal=�65   $��Center=�6N   .+�5�  .   �� $��12(�68   $ 0,0,0,0q�6F   $��Center=�6X   #���  5�  4   #��� ( 5�  4   5�  +   5�  '   �� 5�  &   $��5��6-   $��0,0,0,0q�6=   $
��Left=�6N   $��Center=�5�  -   $��Center=�6O   $��22��6j   $��60��6v   #���  5�  -   ��5�  ,   �� $ SharedWithMePasswords��$$ DataContext.RejectShareCommand���6�   �� *uuid+y�5�  +   �� *sharedStatus+# [ $ reject��.��5�  *   �� $ 
Horizontal=�65   $��Center=�6N   .+�5�  .   �� $��12(�68   $ 0,0,0,0q�6F   $��Center=�6X   #���  5�  4    
) Reject#��� ) 5�  4   5�  +   5�  '   5�  '   5�  #   5�     5�     5�     A   ;PasswordBoss.Views.UserControls.CustomMessageBoxUserControl  5�  
      CustomMessageBoxType$ Confirmation=�5�     '     MessageBoxDialogCancelButtonText#  �  5�      * Continue(!   !MessageBoxDialogConfirmButtonText#! � * 5�      + SharingConfirmationHeader!"   MessageBoxDialogHeaderText#" � + 5�      , ShareAccept#   MessageBoxDialogText## � , 5�     !$   MessageBoxDialogVisibility$ 5�     �� *AcceptMessageBoxVisibility+%%   MessageBoxDialogConfirmCommand% 5�     �� *AcceptShareCommand+$&   MessageBoxDialogCancelCommand& 5�     �� *CancelCommand+  5�  
   $ Confirmation=�5�     #  �  5�     #! � * 5�     #" � + 5�      - ShareItemReject## � - 5�     $ 5�     �� *RejectMessageBoxVisibility+% 5�     �� *RejectShareCommand+& 5�     �� *CancelCommand+  5�  
   $ Confirmation=�5�     #  �  5�     #! � * 5�     #" � + 5�      . ShareItemCancel## � . 5�     $ 5�     �� *CancelMessageBoxVisibility+% 5�     �� *CancelShareCommand+& 5�     �� *CancelCommand+  5�  
   $ Confirmation=�5�     #  �  5�     #! � * 5�     #" � + 5�      / ShareItemRemove## � / 5�     $ 5�     �� *RemoveMessageBoxVisibility+% 5�     �� *RemoveShareCommand+& 5�     �� *CancelCommand+  5  
   $ Confirmation=�5     #  �  5     #! � * 5     #" � + 5      0 ShareResend## � 0 5     $ 5     �� *ResendMessageBoxVisibility+% 5     �� *ResendShareCommand+& 5	     �� *CancelCommand+  5  
   $ Confirmation=�5     #  �  5     #! � * 5     #" � + 5      1 ShareRevoke## � 1 5     $ 5     �� *UnshareMessageBoxVisibility+% 5     �� *RevokeShareCommand+& 5     �� *CancelCommand+5     5     