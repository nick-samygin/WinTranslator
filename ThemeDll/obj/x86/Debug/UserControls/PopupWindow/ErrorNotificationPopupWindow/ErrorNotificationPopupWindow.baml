   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:QuickZip.UserControlsQuickZip.UserControls  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0898    2QuickZip.UserControls.ErrorNotificationPopupWindow  �� 
TargetType �� Property �� Value �� 
TargetType'   !QuickZip.UserControls.PopupWindow   Title   LogoVisibility   LogoWithTextVisibility   ContentGridHeightSize   ControlHeightSize	 � RowDefinitions
 � 	Resources   ProximaRegular  
FontSize14#    BottomPanelContentVisibility � ColumnDefinitions �� Orientation �� CornerRadius J� SnapsToDevicePixels �� Triggers P� Property P� Value ]� 
Conditions �� Property �� Value 
 Cancel  TryAgain �� 	IsDefault�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      -local#clr-namespace:QuickZip.UserControls  5      %�
  '         �� 5      "    6	   .�5      ��  � �� 5      �� 5      "   6   .R�5	        #��z��6   # z 5
      # z 5      # z 5      # z 6O   # z 6�   .��5      � $��Bottom=�6   $��Stretch=�5      #��z 5      #��z��5      	 5   	   �� 5   
   $	��130 �6   �� 5   
   $��60 �6   
 5   	   ���� 5   
   '��       "  ��6   .�5      ��  � ��# �   6(   �� 5       � ��# �  6&   �� 5       � ��$ Light"�6(   5      5   
   .+�5   	   �� $��0��6   $��0,0,0,0q�6'   $��Center=�68   $��Center=�6U   � 5   	   #y�z 6   $��1��5       $��Stretch=�5!      $��Bottom=�5"      $��60��5#      ���#F0F0F0 5%   
   �� 5&      $	��269 �6   �� 5'      $	��383 �6   .+�5)   
   �� $y�Hidden=�6   $ 
Horizontal=�6)   $��Center=�6B   $��1��6]   $��0��6j   $��43,0,0,0q�6z   .+�5*      �� $��Error code hardcode:��6   �� 5+      $�� ��6   �� 5,      ��6   �� *	ErrorCode+5-      �� 5.   
   $ 
Horizontal=�6   $��Center=�6.   $��	18,0,18,0q�6I   $��1��5/      $��1��50      
 51      ���� 52      '��       "  ��6   .�53      ��  �@�Cursor$
 Hand��6&   �� 54       � �� �#ffffff6*   �� 55       � ��$ 30��6&   �� 56       � ��$	 165��6%   �� 57       � ��# �   6*   �� 58       � ��$ Center=�61   �� 59       � ��# �  6(   �� 5:       � ��$ Light"�6*   �� 5;       � ��$ 4q�6&   �� 5<       � �� 5=      �� 5>      " ��6   .R�5?      �� #��z��6   $ 2��5@      ���#7e7e7e$��1q�5B       . ._�5D      �� $��Center=�6"   $��Center=�6=   $��5q�6Z   5E       5F      P� 5G       �@��	IsDefault . . �5H      ��  � �� �#1f47636/   �� 5I       � ��
 �����6/   5J      ]� 5L       5M      �� 5N       �@��	IsDefault . 62   �� 5O       �@J�IsMouseOver . 64   ./�5Q      ��  � �� �#27597d6/   5R      ]� 5T       5U      �� 5V       �@��	IsDefault . 62   �� 5W       � �� . 62   ./�5Y      ��  � �� �#17354a6/   5Z      ]� 5\       5]      �� 5^       �@��	IsDefault . 62   �� 5_       �@J�	IsFocused . 62   ./�5a      ��  � �� �#27597d6/   5b      5d      5f      5h      5i      .+�5j      �� #���  6   ��65   �� *%#ErrorNotificationPopupCancelCommand+�� 5l      #���  6    . ��67   �� *'%ErrorNotificationPopupTryAgainCommand+6q   5m      5n   
   5o   	   5p      5q      5s      5t      5v      