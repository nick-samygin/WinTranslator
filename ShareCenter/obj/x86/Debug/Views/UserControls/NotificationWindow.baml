   M S B A M L   `   `   `  ����F  BShareCenter, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:PasswordBoss.ViewModel.NotificationViewModels-PasswordBoss.ViewModel.NotificationViewModels  5      @ <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullT6clr-namespace:PasswordBoss.UserControls;assembly=themePasswordBoss.UserControls 5      8    2PasswordBoss.Views.UserControls.NotificationWindow   5   !   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ^notificationViewModels;clr-namespace:PasswordBoss.ViewModel.NotificationViewModels     5      OuserControls6clr-namespace:PasswordBoss.UserControls;assembly=theme     5      1  +PasswordBoss.UserControls.CustomChildWindow    OkButtonContent  5      ��  �� Path$ 	OkCaption�   CancelButtonContent 5      �� $ CancelCaption� � 	Resources 5       �� DataTypeH   BPasswordBoss.ViewModel.NotificationViewModels.ChangeShareViewModel �� Orientation $   PasswordBossBlackBackgroundColor �� TextAlignment  ProximaRegular  HowLong  SettingsComboboxStyle d� 	IsChecked �� Mode  PasswordVisibleJ  DPasswordBoss.ViewModel.NotificationViewModels.CancelSharingViewModel��%�  (��       5
      *, +)(�� q    5)      *, +)�� 5
   
   "  6   .R�5      �� $ Vertical=�6   $��	17,20,0,0q�60   .+�5      �� $
��Left=�6   $	��340��5      #���   5      $
��Wrap=�5      $
 Left=�5      $	��Top=�5      $��18(�5      $
��Bold"�5      #���  5      #���  5      �� 5      $
��Left=�6   #���  5      $	��200��5      $��30��5      $��0,3,0,0q�5      �� 5      $
��Left=�6   $��	0,25,0,30q�5       5      �� $ IsPasswordVisible�$ TwoWay=�.��5      �� #���   5       $��14(�5!      #���  5"      $
��Left=�5#      #���  5$      5%      5&      5'      �� 5)   
   "  6   .R�5*      �� $ Vertical=�6   .+�5+      �� $	��Top=�6   $	��350��5,      $��
17,20,0,60q�5-      #���   5.      $��14(�5/      #���  50      $
��Left=�51      ��52      �� $ Question�53      54      55      	   TitelContent	 56      �� 57   
   .+�58      �� $��	0 23 0 15q�6     OBProximaRegular#���  59      $
��Wrap=�5;        SettingsTextLableForeground#���  5<      $��30(�5=      ��5:      �� $ Caption�5=      5>      .��5@      � $	��450��6   .+�5A   
   �� ��6   �� $ DataContext�
 �� RelativeSource
 �� *FindAncestor+ �� AncestorType"  5B      5C      