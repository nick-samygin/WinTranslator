   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullL2clr-namespace:QuickZip.UserControls;assembly=themeQuickZip.UserControls  5      C ?Settings, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:PasswordBoss.Views.UserControlsPasswordBoss.Views.UserControls 5      @   :Settings.Views.UserControls.TwoStepVerificationUserControl   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Acc2clr-namespace:QuickZip.UserControls;assembly=theme     5      <uc-clr-namespace:PasswordBoss.Views.UserControls     5      c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      
������   � AllowsTransparency  .  � WindowStyle$
 None=�5
   (    � WindowStartupLocation$ CenterScreen=�5       � ShowInTaskbar .  $
��Auto��5      $
��Auto��6    � 	Resources 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      � .+�5   
   3   -QuickZip.UserControls.ConfirmationPopupWindow  -   5          Name$ TwoStepInitialScreen��5        TwoStepAuthentication'   !QuickZip.UserControls.PopupWindow   Title# �  5        
GetStarted   ConfirmAction# �  5        	LearnMore   AdditionalAction# �  5      !	   AdditionalActionVisibility$	 Visible=�5      y�5      �� *97TwoStepVerificationModel.TwoStepInitialScreenVisibility+
 �� 	Converter#
 [   �� Mode$ TwoWay=�   Command 5      �� *A?TwoStepVerificationModel.ShowTwoStepMasterPasswordScreenCommand+   AdditionalCommand 5      �� *:8TwoStepVerificationModel.TwoStepOpenLearnMoreLinkCommand+.��5      � $��0,0,0,0q�6   .+�5      �� $��0��6   $��0��6*   .+�5      �� $��0��6    $��0��60   $
��Wrap=�6=    �� TextAlignment$
 Left=�6Q   $
��Left=�6f   $	��Top=�6�   $��16(�5         ProximaRegular#���  6'    �� 
LineHeight$ 24��6U    & "TwoStepInitialScreenMessageDesktop#���  6e   5!      5"      5#        5%   
   -   5        $&  TwoStepEnterMasterPasswordScreen��5&      # �  5(        Next# �  5)      y�5'      �� *@>TwoStepVerificationModel.TwoStepMasterPasswordScreenVisibility+#
 [  $ TwoWay=� 5*      �� *=;TwoStepVerificationModel.ShowTwoStepSecreetKeyScreenCommand+   CommandParameter 5+      ��  �� ElementName$ pbMasterPasswordValue��.��5-      � $��0,0,0,0q�6    � RowDefinitions 5.      �� 5/      $��64 �6$   �� 50      $��20 �6$   .+�52      �� $��0��6   $��0��6*   $��0,0,0,0q�6:   .+�53      �� $
��Wrap=�6!   $
 Left=�65   $��16(�6J   #���  6X   $��Medium"�54      $ 24��62    & "TwoStepMasterPasswordScreenMessage#���  6B   55      G� 56      -   5         G�Name$ pbMasterPasswordValue��56   "   $��8,0,40,0q�6A   #���  57        
FontSize17#���  6L    G� 	MaxLength$ 30��6t   $��Center=�58      $��30��6@    	 PasswordBoxTemplate#��� 	 6L    J� InputBindings 59      �� 5:      
 �� Key$ Enter��5;   H    �� Command 5:   )   �� *=;TwoStepVerificationModel.ShowTwoStepSecreetKeyScreenCommand+ �� CommandParameter 5;   	   �� $ pbMasterPasswordValue��5<      5>      5?      �� 5@      $
��Left=�6   $��1��67   $��14(�6D   $
 Left=�5A       
 IncorrectMasterPassword#��� 
 61   ���#ae2b3f#���  6w   y�5B      �� *<:TwoStepVerificationModel.IncorrectMasterPasswordVisibility+#
 [  $ TwoWay=�5C      5D      5E        5G   
   -   5        $ TwoStepSecreetKeyScreen��5H      # �  5J      # �  5K        Back# �  5M      $	 Visible=�5N         ControlHeightSize$ 600.0Q�5P         ContentGridHeightSize$ 540.0Q�5Q         ContentHeightSize$ 480.0Q�5R      y�5I      �� *;9TwoStepVerificationModel.TwoStepSecretKeyScreenVisibility+#
 [  $ TwoWay=� 5L      �� *?=TwoStepVerificationModel.ShowTwoStepConfirmationScreenCommand+ 5O      �� *A?TwoStepVerificationModel.ShowTwoStepMasterPasswordScreenCommand+.��5T      � $��0,0,0,0q�6    � ColumnDefinitions 5U      �� 5V      $	��350 �6'   65   �� 5W      $��* �6'   63    5Y      �� 5Z      $��* �6$   61   �� 5[      $
��Auto �6$   64   .+�5]      �� $��0��6   $��2��6-   $��0��6A   $��0,0,0,0q�6N   $	��Top=�6_   .+�5^      �� $
��Wrap=�6    $
 Left=�64   $
��Left=�6I   $��0,0,0,0q�6e   $��16(�5_      #���  6'   $��Medium"�6U   $ 18��6i   �� 5`       & "TwoStepSecretKeyScreenFirstMessage#��  6   6\    �� 5a      -   5         ��Name$ hyperlinkSupportedApps��5a      #���  6M    $  PasswordBossBlackBackgroundColor#���  5b      $��16(�6]    �� TextDecorations 5a   2   Z�  �� Command 5b   k   �� *=;TwoStepVerificationModel.TwoStepOpenSupportedAppListCommand+.�6�   .�5c      ��  - )TwoStepSecretKeyScreenFirstMessageAppList#���  6(   5d      5e      �� 5f      $
��Wrap=�6    $
 Left=�64   $
��Left=�6I   $��0,20,0,0q�6d   $��16(�5g      #���  6'   $��Medium"�6U   $ 18��6i   �� 5h      $
��Bold"�6    ' #TwoStepSecretKeyScreenSecondMessage#��  60   6o   5i      �� 5j      $
��Wrap=�6    $
 Left=�64   $
��Left=�6I   $��0,5,0,0q�6d   $��16(�5k      #���  6'   $��Medium"�6U   $ 18��6i   �� 5l       + 'TwoStepSecretKeyScreenThirdMessagePart1#��  6   6a    �� 5m      .�6   .�5n      ��  + 'TwoStepSecretKeyScreenThirdMessagePart2#��  6"   6e   5o       �� 5p       + 'TwoStepSecretKeyScreenThirdMessagePart3#��  6   6a   5q      �� 5r      $��Center=�6   $��0,20,0,0q�69   $	��180��5s   `   $	��180��6m   ��6   �� *(&TwoStepVerificationModel.BarcodeSource+$ TwoWay=�5t      �� 5u      $
��Wrap=�6    $ Center=�64   $��Center=�6K   $��0,40,0,0q�6h   $��16(�6z   #���  5v      $��Medium"�6G   $ 24��6[    & "TwoStepSecretKeyScreenForthMessage#���  6k   5w      �� 5x      $
��Wrap=�6    $ Center=�64   $��Center=�6K   $��16(�6h   $
��Bold"�6v   #���  5y      $ 24��6G   ��6W   �� *$"TwoStepVerificationModel.SecretKey+5z      5{      5|      5}        5   
   -   5        $( "TwoStepEnterConfirmationCodeScreen��5�      # �  5�      # �  5�      # �  5�      $	 Visible=�5�      y�5�      �� *><TwoStepVerificationModel.TwoStepConfirmationScreenVisibility+#
 [  $ TwoWay=� 5�      �� *DBTwoStepVerificationModel.ShowTwoStepMobilePhoneNumberScreenCommand+ 5�      �� $ pConfirmationValue�� 5�      �� *=;TwoStepVerificationModel.ShowTwoStepSecreetKeyScreenCommand+.��5�      � $��0,0,0,0q�6    5�      �� 5�      $��64 �6$   �� 5�      $��20 �6$   .+�5�      �� $��0��6   $��0��6-   $��0,0,0,0q�6:     �� Orientation$  Vertical=�6K   $
��Left=�6c   .+�5�      �� $
��Wrap=�6    $
 Left=�64   $��16(�6I   #���  6W   $��Medium"�5�      $ 24��6-    . *TwoStepSecretConfirmationCodeScreenMessage#���  6=   5�      �� 5�      $  
Horizontal=�6!   .+�5�      �� -   5        ! ��Name$!! pConfirmationValueWatermark��5�   "   $��0,0,0,0q�6G   #���  6X   $	��135��5�      $y�Visible=�6)     
FontSize16#���  6>   " �� 	MaxLength$" 6��6f   ���#b8bcbf$��Center=�6�   $
��Left=�5�       ! CheckBoxReadAcceptBorderbrush# �� 
CaretBrush## �  68   $��30��6u    & "TwoStepVerificationDigitsWatermark#���  6�     TextBoxTemplate#��[ 5�   M   .��6v   5�      �� 5�      -   5        $! pConfirmationValue��5�   "   $��0,0,0,0q�6>   $��8,0,40,0q�6O   #���  6b   $	��135��6�   #���  5�      $" 6��6E   $y�	Collapsed=�6S   $��Center=�6j   $
��Left=�6�   ## �  5�      $��30��6Z   #��[ 5�   F   ��5�      �� *+)TwoStepVerificationModel.ConfirmationCode+$ �� UpdateSourceTrigger$$ PropertyChanged=�$ TwoWay=�.��5�   o    5�   	   �� 5�   
   $ Enter��5�   J    5�      �� *DBTwoStepVerificationModel.ShowTwoStepMobilePhoneNumberScreenCommand+ 5�      �� $ pbMasterPasswordValue��5�      .��5�       5�      5�      5�      �� 5�      $
��Left=�6   $��1��67   $��14(�6D   $
 Left=�5�        IncorrectConfirmationCode#���  61   ���#ae2b3f#���  6y   y�5�      �� *><TwoStepVerificationModel.IncorrectConfirmationCodeVisibility+#
 [  $ TwoWay=�5�      5�      5�        5�   
   -	   5        $$ TwoStepMobilePhoneNumberScreen��5�      # �  5�      # �  5�      y�5�      �� *CATwoStepVerificationModel.TwoStepMobilePhoneNumberScreenVisibility+#
 [  $ TwoWay=� 5�      �� *ECTwoStepVerificationModel.ShowTwoStepBackupSecurityCodeScreenCommand+ 5�      �� $ pbMasterPasswordValue��.��5�      � $��0,0,0,0q�6    5�      �� 5�      $	��350 �6'   65   �� 5�      $��* �6'   63    5�      �� 5�      $��* �6$   61   �� 5�      $
��Auto �6$   64   .+�5�      �� $��0��6   $��0��6-   $��2��6:   $��0,0,0,0q�6N   $  Vertical=�6_   $
��Left=�6w   $	��Top=�6�   .+�5�      �� $
��Wrap=�6!   $
 Left=�65   $��16(�6J   #���  6X   $��Medium"�6�   $ 24��6�   �� 5�      .�6   .�5�      ��  . *TwoStepMobilePhoneNumberScreenFirstMessage#��  6"   6h   5�       �� 5�       6 2TwoStepMobilePhoneNumberScreenFirstMessageOptional#��  6   6l   5�      �� 5�      $
��Wrap=�6!   $
 Left=�65   $��0,5,0,0q�6J   $��16(�6[   #���  6i   $��Medium"�6�   $ 18��6�   �� 5�       / +TwoStepMobilePhoneNumberScreenSecondMessage#��  6   6e   5�      �� 5�      $
��Wrap=�6!   $
 Left=�65   $��0,10,0,0q�6J   $��16(�6\   #���  6j   $��Medium"�6�   $ 24��6�   �� 5�       . *TwoStepMobilePhoneNumberScreenThirdMessage#��  6   6d   5�      5�      5�      5�        5�   
   -
   5        $% TwoStepBackupSecurityCodeScreen��5�      # �  5�      # �  5�      $ 345.0Q�5�      $ 285.0Q�5�      $ 225.0Q�5�      y�5�      �� *DBTwoStepVerificationModel.TwoStepBackupSecurityCodeScreenVisibility+#
 [  $ TwoWay=� 5�      �� *<:TwoStepVerificationModel.ShowTwoStepCompletedScreenCommand+ 5�      �� $ pbMasterPasswordValue��.��5�      � $��0,0,0,0q�6    5�      �� 5�      $	��620 �6'   65   .+�5�      �� $��0��6   $��0��6-   $��0,0,0,0q�6:   $  Vertical=�6K   $��Center=�6c   $��Center=�6�   .+�5�      �� $��2q�6   ���#ae2b3f���#f8cbd3._�5�      �� $��
15,10,15,0q�6%   $��Center=�69   .+�5�      �� $
��Wrap=�6)   $
 Left=�6=   $��16(�6R   #���  6`   ���#7e3c46$��Medium"�5�   >   $ 24��6R   �� 5�   "   .�6'   .�5�   &   ��  / +TwoStepBackupSecurityCodeScreenFirstMessage#��  6*   6q   5�   #   5�      �� 5�      $��0,10,0,0q�6(   $
��Wrap=�6:   $
 Left=�6N   $��16(�6c   #���  6q   ���#ae2b3f$��Medium"�5�   :   $ 24��6N   �� 5�   "    0 ,TwoStepBackupSecurityCodeScreenSecondMessage#��  6&   6n   5�      �� 5�      $	��260��6(   $
��Left=�64   $
��Wrap=�6P   $
 Left=�6d   $��20(�5�   !   #���  6/   ���#ae2b3f$��Medium"�6r   $ 24��6�   �� 5�   
   6   �� 5�   "   .�6'   .�5�   &   ��    
BlackColor#���   6*   �6T   �� *=;TwoStepVerificationModel.TwoStepBackupSecurityCodeFormatted+6�   5�   #   �� 5�   "   6.   5�      5�      5�      5�      5�      5�        5�   
   -   5        $ TwoStepCompletedScreen��5�      # �  5      ! Done# � ! 5     y�5      �� *;9TwoStepVerificationModel.TwoStepCompletedScreenVisibility+#
 [  $ TwoWay=� 5     �� *@>TwoStepVerificationModel.CloseTwoStepVerificationDialogCommand+ 5     �� $ pbMasterPasswordValue��.��5     � $��0,0,0,0q�6   .+�5     �� $��0��6   $��0��6-   $��2��6:   $��0,0,0,0q�6N   $  Vertical=�6_   $
��Left=�6w   $	��Top=�6�   .+�5     �� $
��Wrap=�6!   $
 Left=�65   $��16(�6J   #���  6X   $��Medium"�6�   $ 24��6�   �� 5	     .�6   .�5
     ��  &" "TwoStepCompletedScreenFirstMessage#�� " 6"   6`   5     5     �� 5     $
��Wrap=�6!   $��0,10,0,0q�65   $
 Left=�6G   $��16(�6\   #���  6j   $��Medium"�6�   $ 24��6�   �� 5      '# #TwoStepCompletedScreenSecondMessage#�� # 6   6]   5     5     5     5     5     5     