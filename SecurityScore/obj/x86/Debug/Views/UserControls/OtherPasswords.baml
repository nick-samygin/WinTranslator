   M S B A M L   `   `   `  ����H  DSecurityScore, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:PasswordBoss.ViewsPasswordBoss.Views  5      Q-clr-namespace:PasswordBoss.Views.UserControlsPasswordBoss.Views.UserControls  5      4    .PasswordBoss.Views.UserControls.OtherPasswords   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Etelerik1http://schemas.telerik.com/2008/xaml/presentation     5      2panel clr-namespace:PasswordBoss.Views     5      ?local-clr-namespace:PasswordBoss.Views.UserControls     5      .��5      � $��0 18 0 3q�6   .+�5   
   *  $Telerik.Windows.Controls.RadGridView  $��0 6 0 0q�6   $��1��5      $��Hidden=�6,      RadGridViewStyle#���   5   I   $��1��6t     GeyHeaderRowStyle;  5Telerik.Windows.Controls.GridView.GridViewDataControl    HeaderRowStyle#  �  5         	RowHeight$ 47Q�6T      HorizontalGridLinesBrush
 ����   SecureScoreRowStyle   RowStyle# �  6�     GridViewAlternateRowStyle   AlternateRowStyle# �  5      8  2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��6_   *  $Telerik.Windows.Controls.DataControl   ItemsSource 5      �� *SecurityScoreData+   Columns 5      1  +Telerik.Windows.Controls.GridViewDataColumn  5      -  'Telerik.Windows.Controls.GridViewColumn   Width?  9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 275 6-   	   
IsReadOnly	 . 6  0Telerik.Windows.Controls.GridViewBoundColumnBase
   Header$
 Account��6K    & "SecureScoreGridViewHeaderCellStyle   HeaderCellStyle# �  6\      CellTemplate 5      �� 5      .R�5      ��  ��Name$ LeftPartOfItemGrid��6)   $��2��6E   
������ $��Center=�6n   $��0 0 12 0q�6�   .+�5   "   �� $
��Left=�6,   $	��260��6G   $��3 0 0 0q�6S    $  PasswordBossBlackBackgroundColor#���  5   %   $
��Wrap=�6e    �� TextAlignment$
 Left=�6y    �� Padding$ 0,0,0,0q�5   %   $��Bottom=�67   $
��10pt(�6R   $��Normal"�6b     ProximaRegular#���  5   %   ��6S   �� *
siteName+�� 5   "   $
��Left=�6-   $
 Left=�6H   $��3 2 0 0q�6]      LightGrayTextForegroundColor#���  5   !   $��0��6]   $��2��6m   $ 0,0,0,0q�5   !   $	��Top=�63   $
��10pt(�6K   $��Normal"�6[   $
��Wrap=�6q   #���  5   !   ��6O   �� *
userName+5      5      5!        5"      $	 185 6-   	 . $
 Password Strength��6L   # �  6g    5#      �� 5$      .R�5%      :	   4PasswordBoss.Views.UserControls.PasswordTextBoxSmall	  $
��Left=�69    	  Password 6T   �� *
password+5&      5(        5)      $	 185 6-   	 . # �  6K   $
 Password Age��6�    5*      �� 5+      .R�5,      �� $��Center=�6)   $
 Left=�6F   $��3 2 0 0q�6[   #���  5-   !   $��0��6]   $��2��6m   $ 0,0,0,0q�5.   !   $	��Top=�63   $
��10pt(�6K   $��Normal"�6[   $
��Wrap=�6q   #���  5/   !   ��6O   �� *passwordAge+50      52        53      $
 auto 6-   	 .  
 Action#
 �  6L   # �  6n    54      �� 55      .R�56      ��  	 BlueSettingsButton#��� 	 6&   $��0 0 30 0q�6T    

 Update#��� 
 58   &   $��60��6I   #���  6T   ��56   f   �� *(&DataContext.SecurityScoreUpdateCommand+ �� RelativeSource ��  �� AncestorType2
   ,PasswordBoss.Views.SecurityScoreContentPanel" 
  �� Mode$ FindAncestor=���57   0   �� *.+58   T   59      5;      5=      5>      5?      