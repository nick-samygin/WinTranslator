   M S B A M L   `   `   `  ����F  BProductTour, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ProductTour.Views.ScansProductTour.Views.Scans  5   	   I)clr-namespace:ProductTour.ViewModel.ScansProductTour.ViewModel.Scans  5   	   C&clr-namespace:ProductTour.Views.StylesProductTour.Views.Styles  5   	   @ <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullL2clr-namespace:QuickZip.UserControls;assembly=themeQuickZip.UserControls 5	   	   1    +ProductTour.Views.Scans.ScanResultWithItems   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   4on%clr-namespace:ProductTour.Views.Scans     5   	   9vms)clr-namespace:ProductTour.ViewModel.Scans     5   	   5cc&clr-namespace:ProductTour.Views.Styles     5   	   Bgcc2clr-namespace:QuickZip.UserControls;assembly=theme     5	   	   
�������   OBProximaRegular#���   5   	     � 	Resources  5       �� MergedDictionaries �� Source �� 
TargetType  contentGrid �� Triggers P� Property P� Value N� EnterActions  fadeInAnimation�� 5       5      �� 5      $# ../Styles/ScanResultGrid.xaml?�6   �� 5      $) #../Styles/ActionRequiredHeader.xaml?�6   �� 5      $% ../Styles/ScanSummaryBlock.xaml?�6   �� 5      $% ../Styles/ActivateNowBlock.xaml?�6   �� 5      $ ../Styles/Animations.xaml?�6   �� 5      $! ../Styles/ViewBoxStyle.xaml?�6   %�   &	       7  �� 5      " �6    5      P� 5       � y�$ Visible=�6%    5      �� 5   	   8��  6   5      5      5       .��5$      � 
�������#��[ 6    � RowDefinitions 5%      �� 5&      $
��Auto �6   6$   �� 5'      $	��199 �6   6#   �� 5(      $	��264 �6   6#   �� 5)      $	��110 �6   6#   .+�5,      3   -ProductTour.Views.Styles.ActionRequiredHeader  $��0��6   $��0,3,0,0q�6)   /   )ProductTour.Views.Styles.ScanSummaryBlock  5-      $��1��6   $��	0,-30,0,6q�6%   $��Center=�68   �� 51      $��2��6   $��0,-1,0,0q�6   ���#f2f6f9���#dce1e3$��0,1,0,1q�6U   ._�52      �  53      �� 54      $��7* �6   6#   �� 55      $	��19* �6   6$   .+�59      � $��0��6   $
��16pt(�6   $��Center=�62   $��	25,11,0,0q�6M    5:      �� 5;      $��44 �6   6$   	 � ColumnDefinitions	 5=      �� 5>      $��45 �6   6&   �� 5?      $��45 �6   6&   �� 5@      $	��257 �6   6'   �� 5A      6   �� 5B      $	��100 �6   6'   .+�5E      �� $��1��6   $��Center=�6   $��0,1,0,0q�68   $��Right=�6I     imgWarningRedLarge#���  6e   � 5G      $��2��6   $��0��6   $��6,0,10,0q�6)    5H      �� 5I   	   $��* �6   �� 5J   	   $��* �6   .+�5L      (� 
 (� Stretch$
 Uniform=�6   $��0,0,0,2q�6"    (� StretchDirection$ DownOnly=�63   $
��Left=�6O   $��0��6j   $��Bottom=�6w   .��5M   	   �� $��Bottom=�6   ���#768387 $  Onboardv4Screen3PasswordSecurity#���  6D   $��12.5pt(�6~   5N   	   (� 5O      $
 Uniform=�6   $��1,1,0,0q�6"   $ DownOnly=�63   $
��Left=�6O   $��1��6j   $	��Top=�6w   .��5P   	   �� ���#de171e$	��Top=�6)     Onboardv4StatusAtRickCaps#���  6A   $
��15pt(�6t   $
��Bold"�6�   5Q   	   5R      � 5T      $��3��6   $��0��6   ���#de1920$��0,0,0,0q�6H    5U      �� 5V   	   $��30 �6   �� 5W   	   $��15 �6   	 5Y      �� 5Z   	   $	��179 �6   6(   �� 5[   	   $��10 �6   6'   �� 5\   	   $��85 �6   6'   �� 5]   	   $��85 �6   6'   .+�5`      �� $��0��6   $��4��6     �� Orientation$ 
Horizontal=�64   .+�5a   	   �� ���#d6dadb���#768387$��1q�6;   $	��274��6O   $��20��6[   �� 5b   	   
����������#768387$��1q�69   $��85��6M   $��20��6X   5c   	   (� 5d      $
��Left=�6   $��1��6-   $��0��6:   $��0,0,0,0q�6J   .��5e   	   �� $ 
Horizontal=�6   $	��Top=�6-   .+�5f   
   �� $��10.5pt(�6.   ��6   �� *
Insecure+6.   �� 5g   
   $��5,0,0,0q�6    " Onboardv4Screen3PasswordIssues#���  6%   $��10.5pt(�6]   5h   
   5i   	   (� 5j      $��1��6   $��2��6   $��2��6-   .��5k   	   �� $��Center=�6   ���#de171e  Onboardv4StatusAtRisk#���  6E   $��Bottom=�6t   $��10.5pt(�6�   $��0,0,0,-2q�6�   5l   	   �� 5m      $��0��6   $��3��6     imgWarningIconRed#���  6+   $��	-15,0,0,0q�6X   $
��Left=�6k   5n      5p      � 5r      $��1��6   $��0,-1,0,0q�6   	 5s      �� 5t      $	��101 �6   �� 5u      $	��625 �6   �� 5v      $	��110 �6   .+�5x      &   System.Windows.Controls.DataGrid  -   5        $��	0,8,114,9q�5x      $
��Auto=�5y   
      EnableRowVirtualization .  ���#1f4763$	��Top=�5|   
    	 scanResultGridStyle#��� 	 5~   
      HeadersVisibility$ Column=�5   
      GridLinesVisibility$
 None=�5�   
      
IsReadOnly .  �� AlternationCount$ 2��5�   
      AlternatingRowBackground �#F2F6F9���#919c9f$��1q�5�   
   $��1��5�   
      AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .  $	��170��5�   
   $   HorizontalScrollBarVisibility$ Hidden=�5�   
   "   VerticalScrollBarVisibility$ Visible=�5�   
   $��2��5�   
   ��5�   
   �� *
ScanList+  5�       

 tbCell �� Property �� Value  ColumnHeaderStyleBase=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn  PasswordColumnnHeaderStyle��%{  &	
       &	 �     '  �    7  &	 �    7  �� 5�   	   " ��6   .�5�   
   ��  � ��$
 13px(�6%   �� 5�   
    � ��$ Center=�6.   �� 5�   
    � ��$
 Left=�60   �� 5�   
    � ��$ 0,0,5,0q�6#   5�   
   �� 5�   	   "  6-   .�5�   
   ��  � ��$ 30��6#   �� 5�   
    � ��$
 17px(�6%   �� 5�   
    � �� �#1f47636'   �� 5�   
    � �� �#fbc4046'   �� 5�   
    � ��$ Stretch=�60   �� 5�   
    � ��$
 Left=�67   5�   
   �� 5�   	   "  6   8   5�   
   �� 5�   	   "  62   8   .�5�   
   ��  � �� �#fe84226'   5�   
   5�   	      RowStyle 5�      �� 5�   	   )  #System.Windows.Controls.DataGridRow"  6   .�5�   
   ��  � �� �#15222b6'    5�   
   P� 5�       �@J�IsMouseOver . 5�      5�   
      Columns 5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�   	   ,  &System.Windows.Controls.DataGridColumn   Header$  ��6        Width5  /System.Windows.Controls.DataGridLengthConverter$  21 6*   !   
IsReadOnly! . 5�   
     5�   	   $ Account��6    $	  120 61   ! . "   CellTemplate" 5�   
   �� 5�      .R�5�      (� $ DownOnly=�6   $
��Left=�60   .��5�      �� #��[
 6.   ��6   �� *Site+6.   5�      5�      5�   
     5�   	    " Onboardv4Screen3ColumnUsername# �  6    $	  208 6Z   ! . " 5�   
   �� 5�      .R�5�      (� $ DownOnly=�6   $
��Left=�61   .��5�      �� #��[
 62   ���#778488��6   �� *
Username+6R   5�      5�      5�   
     5�   	   #   HeaderStyle## [ 6     " Onboardv4Screen3ColumnPassword# �  6Z   $	  120 6�   ! . " 5�   
   �� 5�      .R�5�      (� $ DownOnly=�6   $
��Left=�61   .��5�      �� #��[
 6   ���#778488��67   �� *
Password+6R   5�      5�      5�   
     5�   	     Onboardv4Screen3ColumnRisk# �  6    $	  130 6V   ! . " 5�   
   �� 5�      .R�5�      � 	 5�      �� 5�      $��20 �6   �� 5�      $��* �6    5�      �� 5�      $��25 �6   .+�5�      �� $��16��6   $��17��6   $��Uniform=�6*   $��0,2,7,0q�6<   $��0��6M   $	��Top=�6z   $��Center=�6�   ��6]   �� *	RiskImage+6�   (� 5�      $ DownOnly=�6   $
��Left=�63   $��1��6N   .��5�      �� #��[
 6   $��1��68   $��Center=�6�   $
��Left=�6�   ��6H   �� *
RiskText+��6b   �� *	RiskColor+6�   5�      5�      5�      5�   
   5�      (� 5�      $��2��6   5�      5�      5�      5�      �� 5�      $ Vertical=�6   $��3��6&   $��
56,18,56,0q�63   .+�5�      /	   )ProductTour.Views.Styles.ActivateNowBlock	  $��0,0,0,0q�6   .��5�      �� $ Vertical=�6   
�������.+�5�      (� $��0,10,0,0q�6   $��Center=�6!   $��Bottom=�6>   .��5�      ��  " Onboardv4Screen1ButtonHeadline#���  6   $��26(�6K   5�      (� 5�      $��0,0,0,6q�6   $��Center=�6    $��Bottom=�6=   .��5�      ��   Onboardv4Screen3FreeAccount#���  6   $��14(�6H   5�      5�      5�      � 5�      	 5�      �� 5�      $	��421 �6   �� 5�      $	��309 �6   �� 5�      $��56 �6   .+�5�      (� $��1��6   $��Center=�6   $	��Top=�6;   $��0,6,0,0q�6S   .��5�      �� �� 5�      ���#19262E#���   6B   $
��10pt(�5�      $ �� CommandParameter$$ SignIn��6C   % �� TextDecorations% 5�   '   Z� & �� Command& 5�      �� *CloseWizardCommand+.�6]   .�5   	   �� $��0,0,0,1q�6   ���#c4c7ca._�5  
   ��  # Onboardv3Screen1ExistingAccount#���  6   5  
   5  	   5     5     5     5     5	     5
     