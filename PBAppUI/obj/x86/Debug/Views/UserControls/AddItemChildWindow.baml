   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullT6clr-namespace:PasswordBoss.UserControls;assembly=themePasswordBoss.UserControls  5   	    PBAppUI8   2PasswordBoss.Views.UserControls.AddItemChildWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   k gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5   	   Guc6clr-namespace:PasswordBoss.UserControls;assembly=theme    	 
 5   	     � 	Resources  5	         BoolToVisibilityC  =Telerik.Windows.Controls.InvertedBooleanToVisibilityConverter  InvertedBooleanToVisibility  FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  ListBoxItemBorderStyle �� 
TargetType �� Triggers P� Property P� Value  PasswordBossGreenColor	 �� Binding
 �� RelativeSource �� AncestorType �� Value  AddItem_ListBoxItemStyle �� 
TargetType ��Name �� Padding �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName  ThreeColumnsItems A� Columns  FourColumnsItems��%�  &	        &	      &	 &     &	 �     &	 L    7  &	 �    &	     �� 5
   
   6'     5   
   67   �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5      5      5      �� 5   
   " ��6/    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��# �  6;   5      �� 5      	 6   �� *
IsSelected+
 �� *FindAncestor+" �� True6�   .a�5      ��  � ��# �  6;   5      .�5       ��  � ��$ 1q�60   �� 5!       � ��$	 Top=�62   �� 5"       � ��
 ���� 6+   5#      �� 5%   
   " ��61   .�5&      ��  �@J�SnapsToDevicePixels . 64   �� 5'       � ��$
 12 1q�6'   �� 5(       � ��$ 0q�6(   �� 5)       � �� 6;   �� *HorizontalContentAlignment+
 �� " ���� 5*       � �� 69   �� *VerticalContentAlignment+
 �� " ���� 5+       � ��
 ���� 6+   �� 5,       � ��
 ���� 6,   �� 5-       � ��$ 1q�60   �� 5.       �@�FocusVisualStyle8   61   �� 5/       � �� 50      �� 51      " ��6&   .R�52      �� $ Bd��6!   #��z��6-   #��z��6Y   #��z��6�   # z��6�    . ._�53      �� #��z��6/   # z 6r   #��z��6�   54       55      ]� 56       57   "   �� 58   &    �@J�IsMouseOver . 6G   5:      ]� 5;       5<   "   �� 5=   &    �@��IsSelectionActive .  6V   �� 5>   &    �@��
IsSelected . 6F   5@      ]� 5A       5B   "   �� 5C   &    �@��IsSelectionActive . 6V   �� 5D   &    �@��
IsSelected . 6F   5F      P� 5G       � {� .  . �5H   "   �� $ Bd��6K   # � ��6[    � ��5I      5K      5M      5N      �� 5P   
   .R�5Q      A� $ 3��6   5R      �� 5T   
   .R�5U      A� $ 4��6   5V      5Y      1   +PasswordBoss.UserControls.CustomChildWindow   TitelContent 5Z      �� 5[   
   .+�5\      �� -   5         ��Name$ txtTitel��5\   [   $��0 23 0 0q�6     OBProximaRegular#���  6+   $
��Wrap=�6n   $��1��6�    	 SettingsTextLableForeground#��� 	 6�   $��30(�6�   �� 5]      -   5        $ txtSubTitel��5]      $
��Wrap=�6-   $��0 4 0 13q�6B   $��Center=�6T     
 LightGrayTextForegroundColor#��� 
 6p   $��16(�6�   5^      .��5`      � $
��0 20q�6   .+�5a   
   �� -   5         ��Name$ 	typesList��5a      $��0q�6S   $	��Top=�6g   #��[ 6�   ��5b      �� 5c      .R�5d      �� #��[ 6   ._�5e      � $	��136��6   $��Center=�6'   $��Stretch=�6D    � RowDefinitions 5f      �� 5g      �� 5h      $
��auto �6,   .+�5j      ��   TextboxBorderbrush#���  6!   $��1q�6T   $��Stretch=�6h   $	��Top=�6�   $��71��6�   ��6�   �� *BackgoundColor+._�5k      �� $��Center=�6<   $��Center=�6Y   $
��None=�6t   ��6$   �� *Icon+6t   5l      �� 5m      $��1��6$   $
��Wrap=�6L   $��5 4 0 5q�6a   $��Center=�6r   $��Center=�6�   #��� 
 5n   +   $��16(�6h    �� TextAlignment$ Center=�6v   ��5m   1   �� *	ItemTitel+5n   v   5o      5p      5q      ��5s      �� 5t      .R�5u      A� $ 3��6"   5v      5x      �� 5y   
   -   5        $ childTypesList��5y      $��0q�6]   $	��Top=�6q   #��[ 6�   ��5z      �� 5{      .R�5|      �� #��[ 6   ._�5}      � $	��130��6   $��Center=�6+   $	��Top=�6H    5~      �� 5   "   $��65 �60   �� 5�   "   $
��auto �60   .+�5�      ��  ��Name$ addBtn��6%   $��40��6�   $��40��6�   $��Center=�6�   $��Center=�6�    � Cursor$
 Hand��6�   y�66   �� *	IsEmpty+ �� 	Converter# [  ��5�   "   �� 5�   &   $��20��6,   $��20��68     plusThinRed#���  6C   ��5�   "   �� 5�   &   " ��66   .R�5�   *   � .+�5�   .   A�   PasswordBossRedColor#���  66   �� 5�   .   $��Center=�6?   $��Center=�6]   5�   +   5�   '   5�      �� 5�      #���  6%   $��1q�6�   $��Stretch=�6�   $��Stretch=�6�   y�6X   �� *	IsEmpty+# [ ��6�   �� *BackgoundColor+._�5�   "   �� $��Center=�6A   $��Center=�6^   $
��None=�6y   ��6(   �� *Icon+6y   5�      �� 5�      $��1��6(   $
��Wrap=�6P   $��5 4 0 5q�6e   $��Center=�6v   $��Center=�6�   #��� 
 5�   +   $��16(�6h   $ Center=�6v   ��5�   5   �� *	ItemTitel+5�   v   5�      5�      5�      5�      5�      5�      