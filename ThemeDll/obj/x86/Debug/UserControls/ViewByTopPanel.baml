   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:PasswordBoss.UserControlsPasswordBoss.UserControls  5      .    (PasswordBoss.UserControls.ViewByTopPanel   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:PasswordBoss.UserControls     5      .��5      � -   5          �Name$  ViewBy��5       !  MainWindowGrayBackgroundColor#���   6   y�6X   �� *ShowLandingScreen+  InvertedBooleanToVisibility �� 	Converter# [  � ColumnDefinitions 5	   
   �� 5
      $��1 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��1 �6   �� 5      $
��auto �6   �� 5      $
��auto �6    J� InputBindings 5   
   d� 5       d� MouseAction$ 	LeftClicke�6    �� Command 63   �� *ItemsGridGotFocusCommand+.+�5   
   ��    LightGrayTextForegroundColor#���  6   $��Stretch=�6J   $��1��6f   1   +PasswordBoss.UserControls.ImageToggleButton  5   
   $��1��6"   $
��10 0q�62   $��17��6@   $��17��6L   $��Center=�6W   $��Center=�6�     tile_view_grey   	Unchecked# �  5        tile_view_green   MouseOverUnchecked# �  5        list_view_grey   Checked# �  5        list_view_green	   MouseOverChecked#	 �  5      
   	IsChecked
 5   t   �� *
IsTileView+ �� Mode$ TwoWay=�5      d� 5   
   $��2��6   $��Center=�6)   $��Center=�6s   $��0 0 10 0q�6�   $��0q�6�    d� 	IsChecked 6F   �� *	ExpandAll+$ TwoWay=�.��5      �� $��14��6   $��16��6   ��5      �� 5       �� 
TargetType" ��6    �� Triggers 5      �� 5        �� Binding 6*   �� *	IsChecked+ �� RelativeSource �� *FindAncestor+ �� AncestorType" d� �� Value true6�   .a�5!   "   ��  �� Property � ��  collapse_all_grey �� Value# �  6;   5"      �� 5#       6*   �� *	IsChecked+ �� *FindAncestor+" d�	 false6�   .a�5$   "   ��  � ��  expand_all_grey# �  6;   5%      5'      5)      5*      �� 5,   
   #���  6   $��Stretch=�6J   $��1��6f   $��3��6p   �� 5/   
    
	 ViewBy#��� 	 6   $
��10 0q�65   $��4��6D   $��Center=�6T   #���  6o   $��13(�6�   $
��Wrap=�6�   �� 51   
   -   5        $��comboboxFilter��51       K� ZIndex$ 0��6)   $	��175��6;   $��5��6G   $��6q�6W    
 GayBorderComboStyle#��� 
 52        AddSecureComboBoxItemStyle#���  6M    �� SelectedIndex 51   b   �� *SortBySelectedIndex+$ TwoWay=���53      �� *SortByItems+54      57      58      