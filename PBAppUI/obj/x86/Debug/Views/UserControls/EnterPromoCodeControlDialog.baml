   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullL2clr-namespace:QuickZip.UserControls;assembly=themeQuickZip.UserControls  5   	    PBAppUIA   ;PasswordBoss.Views.UserControls.EnterPromoCodeControlDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Acc2clr-namespace:QuickZip.UserControls;assembly=theme     5   	   -   5          �Name$  promoCodeDialog��5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ Password Boss��5	   	   
������  � AllowsTransparency .  � WindowStyle$
 None=�5
   #    � ShowInTaskbar .   � 	Resources 5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      � .+�5   
   3   -QuickZip.UserControls.ConfirmationPopupWindow  -   5          Name$ EnterPromoCodeScreen��5   	     EnterPromoCodeCAPITAL'   !QuickZip.UserControls.PopupWindow   Title# �  5   	     Next	   ConfirmAction#	 �  5   	   y�5   	   �� * EnterPromoCodeScreenVisibility+
 �� 	Converter#
 [     Command 5   	   �� *NextButtonCommand+.��5      � $��0,30,0,0q�6    � RowDefinitions 5      �� 5      $��64 �6$   �� 5      $��20 �6$   .+�5      �� $��0��6   $��1��6*   $��0,0,0,0q�6:   .+�5      �� $��0��6    $��0��61   $
��Wrap=�6>    �� TextAlignment$
 Left=�6R   $
��Left=�6g   $��Center=�6�   $��16(�5         ProximaRegular#���  6'    �� 
LineHeight$ 24��6U     EnterPromoCodeText#���  5!      �� 5"      -   5         ��Name$ pConfirmationValue��5"   U   $��0��6q   $��8,0,40,0q�6~   #���  5#      $	��400��6K     
FontSize16#���  6W     TextBoxTemplate#��[ 6   $��Center=�5$      $
��Left=�6?   $��30��5%      ��6)   �� *PromotionCode+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��6{    J� InputBindings 5&      �� 5'      
 �� Key$ Enter��6n    �� Command 6)   �� *20ForgotMasterPasswordModel.ShowThirdScreenCommand+6n   .��5(   1   5)      5*      �� 5+      -   5         ��Name$ errorMessage��5+   )   $��1��6   $��14(�6m   $
��Left=�6{   $��Center=�5,      $	��350��67   $
��Wrap=�6C   $
 Left=�5-      #���  61   ���#ae2b3f$��0,0,0,0q�6t   y�5+   ?   �� *ErrorMessageVisibility+��5,   W   �� *ErrorMessageText+5-   t   5.      50        52   
   -   5        $ PromoCodeSuccessScreen��53      # �  55       
 Finish#	 �  56      $��0��59      $��0��6   y�54      �� *" PromoCodeSuccessScreenVisibility+#
 [   57      �� *ClosePromoCodeDialogCommand+   CommandParameter 58      ��  �� ElementName$ promoCodeDialog��.��5:      � $��0,30,0,0q�6   .+�5;      �� $��0��6   $��1��6*   $��0,0,0,0q�6:   .+�5<      �� $
��Wrap=�6    $
 Left=�64   $
��Left=�6I   $��Center=�6d   $��16(�5=      #���  6'   $
��Bold"�6U   $ 24��6g     PromoCodeApplied#���  5>      �� 5?      $
��Wrap=�6    $
 Left=�64   $
��Left=�6I   $��Center=�6d   $��14(�5@      #���  6'   $��Medium"�6U   $ 24��6i    	 PromoCodeAppliedInfo#��� 	 5A      $��0,5,0,0q�5B      5C      5D      5E      5F      5G      