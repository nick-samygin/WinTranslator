   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   scanResultGridStyle  �� 
TargetType&    System.Windows.Controls.DataGrid �� Property �� Value �� 
TargetType �� Padding J� SnapsToDevicePixels ��Name � ColumnDefinitions � RowDefinitions	    SelectAllCommand
 �� 
ResourceId �� TypeInTargetAssembly7  1System.Windows.Controls.DataGridHeadersVisibility
   All �� ConverterParameter!    HeadersVisibilityConverter �� 	Converter �� RelativeSource �� AncestorTypeG  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter  Name   Column ��Name �� CanContentScroll ��Name �� ScrollableHeight � Maximum �� Orientation* �� #ComputedVerticalScrollBarVisibility �� ViewportHeight �� ViewportSize � Value �� Mode �� ScrollableWidth,  �� %ComputedHorizontalScrollBarVisibility! �� ViewportWidth" J� IsHitTestVisible# �� Triggers$ P� Property% P� Value�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %=  &	        �� 5      "    6(   .�5   
   ��  � ��# � ��6'   �� 5   
    � ��# � ��6'   �� 5   
     �@  RowDetailsVisibilityMode$ VisibleWhenSelected=�65   �� 5   
    � �� . 6:   �� 5	   
    �@��PanningMode$
 Both=�65   �� 5
   
    �@��IsFlicksEnabled .  63   �� 5   
    � �� 5      �� 5      "   6"   .R�5      �� #��z��6   #��z��6I   #��z��5      # z��6C    . ._�5      �� ���#f2f6f9$��1,0,1,0q�67   ._�5      �� $ DG_ScrollViewer��6'   |�.  ��5      �� 5   "   " ��62   .R�5   &   �  5   *   �� 5   .   $
��Auto �6?   �� 5   .   $��* �6?   �� 5   .   $
��Auto �6?    5   *   �� 5   .   $
��Auto �6<   �� 5   .   $��* �6<   �� 5   .   $
��Auto �6<   .+�5   *   �� #��Z	 61   |�.  ��5    -   C� *�� $"
 DataGridSelectAllButtonStyle��"   +y�5"   -   �� *HeadersVisibility+# Z # Z  �� "   ��5%   -   �� *CellsPanelHorizontalOffset+ �� "     5'   *   $! PART_ColumnHeadersPresenter��6I   $��1��6n   y�5(   -   �� *HeadersVisibility+# Z # Z  �� "   �� 5+   *   $! PART_ScrollContentPresenter��6A   # z��5,   -   $��2��6c   $��1��6w   �� 5-   *   $ PART_VerticalScrollBar��64   $��2��6T   # z 6d   $ Vertical=�5.   -   $��1��6D   #y�z 5/   -   # z 51   -    50   -   �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+51   -   � 52   *   $��1��6/   $��2��6?    53   .   �� 54   2   ��6C   �� **(NonFrozenColumnsViewportHorizontalOffset+ �� "   �� 56   2   $��* �6C   .+�58   .   �� $ PART_HorizontalScrollBar��68   $��1��6Z   # z 6j   $ 
Horizontal=�59   1   #y�z  6J   # z! 5;   1    5:   1   �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+5;   1   5<   +   5=   '   5>   #   .��5@      �� # z 6-   " .  6i   5A      5B      5C      5D      5F      # 5G   
   P� 5H      $ �@��
IsGrouping% . . �5I      ��  � �� .  6B   5J      5L      5M      