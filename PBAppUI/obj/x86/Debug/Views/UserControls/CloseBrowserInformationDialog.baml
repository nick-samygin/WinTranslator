   M S B A M L   `   `   `  ����  PBAppUIC    =PasswordBoss.Views.UserControls.CloseBrowserInformationDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 
ResizeMode$  NoResize=�5       � Title$ Password Boss��5      
������  � AllowsTransparency .  � WindowStyle$
 None=�5	   (    � WindowStartupLocation$ CenterOwner=�5
       � ShowInTaskbar .  $	��300��5   7   $	��159��6C    � 	Resources 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      �� $��2,2,2,2q�6     
BlackColor#���  6'   ._�5   
   � -   5         �Name$ MessageBoxGrid��5      $��Center=�6'   $��Center=�6D   $	��300��6_     
WhiteColor#���  5      $	��159��6h    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   	 � RowDefinitions	 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      � $��0��6   $��0��6    $��2��60   $��Stretch=�6D   	 5      �� 5      $��30��6$   �� 5      �� 5      $��10��6$   �� 5       �� 5!      $��2��6$   �� 5"      �� 5#      $��15��6$   .+�5%      �� -   5        
 ��Name$
 
HeaderText��5%      $��1��60   $��0��6=   $��3��6M   $��18(�6a   $��	20,0,20,0q�6o     TextForegroundcolor#���  5&      $��Center=�5'        ProximaRegular#���  69   $��SemiBold"�5(      �� 5)      -   5        $
 DialogMessageText��5)      $��3��67   $��1��6D   $��	20,0,20,0q�6T   $��14(�6g   $	��300��6u   $��Center=�5*         LightGrayTextForegroundColor#���  69    �� TextAlignment$ Center=�6u   $
��Wrap=�6�     ProximaLight#���  5+      5,      �� 5-      -   5         ��Name$ ButtonCancel��5-      $��1��6+   $��0��68   $��55��6H   $	��150��6T   $��SemiBold"�5.      $��16(�6+   $��Stretch=�69     ProximaSemiBold#���  5/        RoundButtonGrayColor#���  6D    
	 Cancel#��� 	 50      $��Bottom=�68   #���  6S    �� IsCancel .  
 btnStyleNoHover#��� 
 51   @   5H      �� 5J      -   5        $ ButtonOK��5J      $��1��6'   $��1��64   $��Stretch=�6D   $��55��5K      #���  6!   $��SemiBold"�5L      $��16(�6+     PasswordBossGreenColor#���  69   #���  5M      $��Bottom=�6D     OK#���  5N       �� 	IsDefault . #��� 
 6\   5e      5f      5g      5h      