   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:QuickZip.UserControlsQuickZip.UserControls  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� BasedOn �� TypeInTargetAssembly0    *QuickZip.UserControls.VirutalWrapPanelView �� 
ResourceId �� Property �� Value �� RelativeSource,   &QuickZip.UserControls.VirtualWrapPanel �� AncestorType   Orientation	   	ItemWidth
   
ItemHeight   lvItemSelectStyle �� 	Resources  Key �� EndPoint �� 
StartPoint � Color � Offset �� 
TargetType � RowDefinitions �� Triggers ]� 
Conditions �� Property �� Value  fileListHotTrackBrush P� Property P� Value  fileListSelectionBrush+   %QuickZip.UserControls.SelectionHelper�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      *uc#clr-namespace:QuickZip.UserControls  5      %�  (��       5      "   $ virtualWrapPanelViewDSK��)7��&	  �    7��(�� �    5Y      "   $! virtualWrapPanelViewItemDSK��)7   �� 5      "  ��5   )   8   .�5   
   ��  � ��$ Stretch=�67   �� 5
   
    � �� 5      �� *$"(ListView.View).ItemContainerStyle+ �� *Self+�� 5   
    � �� 5      �� *(ListView.View).ItemTemplate+ �� *Self+�� 5   
    � �� 5      �� 5      .R�5        ��6*   �� * (FrameworkElement.ActualWidth)+ �� $ ScrollContentPresenterL� 5      �� *(ListView.View).Orientation+ �� $ ListViewL�	 5      �� *(ListView.View).ItemWidth+ �� $ ListViewL���5      �� *	ItemWidth+ �� *Self+
 5      �� *(ListView.View).ItemHeight+ �� $ ListViewL�5      5      5      �� 5       "  ��6&   8    5!   
   ���� 5"      fileListHotTrackBrush $	 0,1&�6@   $	 0,0&�6O   .��5#      � $ #E8F6FD��6   $ 0Q�6/   � 5$      $ #D7EFFB��6   $	 0.5Q�6/   � 5%      $ #CCEAFF��6   $ 1Q�6/   5&      �� 5(      fileListSelectionBrush $	 0,1&�6A   $	 0,0&�6P   .��5)      � $ White��6   $ 0Q�6-   � 5*      # � ��6   $
 0.55Q�6o   � 5+      # � ��6   $ 1Q�6g   5,      5-      .�50   
   ��  � ��$ Stretch=�60   �� 51   
    � ��$
 Left=�67   �� 52   
    � ��$ Stretch=�65   �� 55   
    � �� 56      �� 57      " ��6"   .R�58      � .+�59      �� #��z��6!   �� 5:      ���	#BEFFFFFF$	��1,1q�68   ._�5;      �  5<   "   �� 5=   &   �� 5>   &   .+�5@   "   �� $��2,1,2,0q�6)   $��0��6:   ���	#57FFFFFF6G   5A      5B      �� 5C      $	��5,0q�6+   5D       5E      ]� 5F       5G      �� 5H   "    �@J�IsMouseOver . 6C   �� 5I   "    �@��
IsSelected .  6B   ./�5K      �� # �  6;    � ��5L      P� 5M       �@��
IsSelected . . �5N      �� # �  6;    � ��5O      P� 5P       �@ 
IsDragging . . �5Q      ��  � ��
 �   �6;   5R      5T      5V      5W      �� 5Y      "  ��5Z      8   5\      5`      