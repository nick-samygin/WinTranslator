   M S B A M L   `   `   `  ����H  DSecurityScore, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:PasswordBoss.Views.UserControlsPasswordBoss.Views.UserControls  5      7 clr-namespace:PasswordBoss.ViewsPasswordBoss.Views  5      ;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5	      8    2PasswordBoss.Views.UserControls.DuplicatePasswords   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Etelerik1http://schemas.telerik.com/2008/xaml/presentation     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:PasswordBoss.Views.UserControls     5      2panel clr-namespace:PasswordBoss.Views     5      6helpers"clr-namespace:PasswordBoss.Helpers     5	        � 	Resources  5         BoolToVisibility��%   &	        �� 5   
   6'   5      .��5      ��   NoBorderListBoxStyle#���  6A    $  NoMouseOverStyleListBoxItemStyle#���  6q   ��6   �� *DublicateSecurityScoreData+��5   
   �� 5      .R�5      � $��0 18 0 3q�6    � ColumnDefinitions 5      �� 5      $��20 �6+   �� 5      $��* �6+    � RowDefinitions 5      �� 5      �� 5      .+�5      d�  d�Name$ expandToggleBtn��6#     PasswordBossBlueHeaderColor#���  6=    d� 	IsChecked$
 TrueY�5   U   $��1q�6f   $��0��6q   $��0��6~    ! SlightExpanderDownHeaderStyle#���  6�   �� 5       �� Orientation$ 
Horizontal=�6!   $��1��6:   .+�5      ��   SecurityScoreStep1Subhead#���  6$   $
��10pt(�6W   $
��Bold"�6g   #���  6z   �� 5       ���#E7555A$��8 0 0 0q�6f   $
��10pt(�6w   $
��Bold"�6�   ��6:   �� *Count+ �� StringFormat$ ({0})��6�   5!      *  $Telerik.Windows.Controls.RadGridView  5#      $��0 6 0 0q�6+   $��1��6�   $��Hidden=�6�     RadGridViewStyle#���  5$   d   $��1��6�     GeyHeaderRowStyle;  5Telerik.Windows.Controls.GridView.GridViewDataControl   HeaderRowStyle# �  5%   =      	RowHeight$ 47Q�6r   	   HorizontalGridLinesBrush
	 ����   SecureScoreRowStyle
   RowStyle#
 �  6�    	 GridViewAlternateRowStyle   AlternateRowStyle# � 	 5&   J   8  2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��6�   y�5#   <   ��  �� ElementName$ expandToggleBtn�� �� 	Converter# [   �� Path$ 	IsChecked�*  $Telerik.Windows.Controls.DataControl   ItemsSource 5$   J   �� *.+   Columns 5'      1  +Telerik.Windows.Controls.GridViewDataColumn  5(      -  'Telerik.Windows.Controls.GridViewColumn   Width?  9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 275 69      
IsReadOnly . 6  0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Account��6W    &
 "SecureScoreGridViewHeaderCellStyle   HeaderCellStyle# � 
 6h      CellTemplate 5)   "   �� 5*   &   .R�5+   *   ��  ��Name$ LeftPartOfItemGrid��65   $��2��6Q   
������ $��Center=�6z   $��0 0 12 0q�6�   .+�5,   .   �� $
��Left=�68   $	��260��6S   $��3 0 0 0q�6_    $  PasswordBossBlackBackgroundColor#���  5-   %   $
��Wrap=�6e    �� TextAlignment$
 Left=�6y    �� Padding$ 0,0,0,0q�5.   %   $��Bottom=�67   $
��10pt(�6R   $��Normal"�6b     ProximaRegular#���  5/   %   ��6S   �� *
siteName+�� 50   .   $
��Left=�69   $
 Left=�6T   $��3 2 0 0q�6i      LightGrayTextForegroundColor#���  51   !   $��0��6]   $��2��6m   $ 0,0,0,0q�52   !   $	��Top=�63   $
��10pt(�6K   $��Normal"�6[   $
��Wrap=�6q   #���  53   !   ��6O   �� *
userName+54   +   55   '   57        58      $	 185 69    . $ Password Strength��6X   # � 
 6s    59   "   �� 5:   &   .R�5;   *   :	   4PasswordBoss.Views.UserControls.PasswordTextBoxSmall	  $
��Left=�6E   ���#E4E9E9 	  Password 6`   �� *
password+6~   5<   '   5>        5?      $	 165 69    . # � 
 6W   $ Password Age��6�    5@   "   �� 5A   &   .R�5B   *   �� $��Center=�65   $
 Left=�6R   $��3 2 0 0q�6g   #���  5C   !   $��0��6]   $��2��6m   $ 0,0,0,0q�5D   !   $	��Top=�63   $
��10pt(�6K   $��Normal"�6[   $
��Wrap=�6q   #���  5E   !   ��6O   �� *passwordAge+5F   '   5H        5I      $
 auto 69    .  
 Action# �  6X   # � 
 6z    5J   "   �� 5K   &   .R�5L   *   ��   BlueSettingsButton#���  62   $��0 0 30 0q�6`    
 Update#���  5M   P   $��60��6s   #���  6~   ��5L   r   �� *(&DataContext.SecurityScoreUpdateCommand+ �� RelativeSource ��  �� AncestorType2
   ,PasswordBoss.Views.SecurityScoreContentPanel" 
  �� Mode$ FindAncestor=���5M   0   �� *.+6~   5N   '   5P      5R      5S      5T      5V      5X      