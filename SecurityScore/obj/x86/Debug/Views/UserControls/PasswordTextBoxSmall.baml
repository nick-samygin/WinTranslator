   M S B A M L   `   `   `  ����H  DSecurityScore, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:PasswordBoss.Views.UserControlsPasswordBoss.Views.UserControls  5      ;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5      :    4PasswordBoss.Views.UserControls.PasswordTextBoxSmall   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:PasswordBoss.Views.UserControls     5      6helpers"clr-namespace:PasswordBoss.Helpers     5      -   5          >�Name$  Parent��5   A   $	��115��5      ���#E4E9E9 � 	Resources 5	         PasswordBoxTemplate �� 
TargetType �� Property  
WhiteColor �� Value  DialogGrayColor �� 
TargetType J� SnapsToDevicePixels �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName  ProgressBarSetup  PasswordBossGreenColor   LightGrayTextForegroundColor  ProductTourGrayColor �Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard g� RepeatBehavior �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value8  2System.Windows.Media.Animation.EasingPointKeyFrame $� KeyTime $� Value ��Name �� RadiusX �� RadiusY J� RenderTransformOrigin �� Angle��%�  &	        &	 �    �� 5
   
   " G�6,   .�5      ��  � ��# �  6+   �� 5       � ��# �  6,   �� 5       � �� 5      �� 5      " G�6&   .R�5      �� $��Bd��6!   $��2q�6+   #��z��5   .   #��z��5   .    . $ 2��5   I   ._�5      �� $��PART_ContentHost��6+   #��z��6C   # z 5   +   5       5      P� 5      	 � {�
 .  . �5   "   �� $ PART_ContentHost��6)   # �  6]    � ��5      5      5      5      �� 5!   
   " �6)   .�5"      ��  � ��# �  6+   �� 5#       � ��# �  6+   �� 5$       � ��# �  6,   �� 5%       � ��$ 0q�60   �� 5&       � �� 5'      �� 5(      " �6&   .R�5)      � $ TemplateRoot��6    5*        5+   "   CommonStates��. 5,   &     Determinate��62     5-   &   Indeterminate��. 5.   *   �� $ Forever��65   .��5/   .   U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�50   -   $ 	Animation��51   -   .\�52   2     $ 0��6G   $
 0.25Q�6S     53   2   $ 0:0:1��6G   $
 0.25Q�6W     54   2   $ 0:0:2��6G   $
 0.25Q�6W   55   /   *� 56   .   $' !(UIElement.RenderTransformOrigin)�57   -   $ 	Animation��58   -   .'�59   2     $ 0��6F   $ -0.5,0.5&�6R     5:   2   $ 0:0:1��6F   $ 0.5,0.5&�6V     5;   2   $ 0:0:2��6F   $ 1.5,0.5&�6V   5<   /   5=   +   5>   '   5?   #   .+�5A      �� #��z��6%   $ 3��6Q   #��z��5B   !   #��z��5C   !   �� 5D      $ 
PART_Track��6(   � 5E      $ PART_Indicator��6#   }�. $
��Left=�6O   .+�5F   "   �� $ 	Indicator��6,   $ 3��6?   $ 3��6K   #��z��6W   �� 5G   "   $ 	Animation��6,   $ 3��6?   $ 3��6K   #��z��6W   $ 0.5,0.5&�5H   (   z�5I   &   V� 5J   *   .~�5K   .   �� �� 5L   .   �� 5M   .   T� 5N   .   5O   +   5Q   #   5R      5S       5T      P� 5U      	 �@�Orientation$
 Vertical=�6=   . �5V   "   �� $ TemplateRoot��6D    �@�LayoutTransform 5W   &   �� 5X   *   $	 -90Q�6:   5Z   #   5[      P� 5\      	 �@�IsIndeterminate
 . . �5]   "   �� $ 	Indicator��6?    � y�$ 	Collapsed=�6V   5^      5`      5b      5c      5f      .��5g      �  � RowDefinitions 5h   
   �� 5i      �� 5j      �� 5k      .+�5m   
   �� -   5        $��txtVisiblePasswordbox��5m   �   $��18��6   $��3,0,3,0q�6    {�.  $��85��6F   $��5 2 0 0q�6Q     AddItemTextBox#���  6c   
������ 
������ $��Center=�6�   $
��Left=�6�   $��Center=�6  G� 5n   
   -   5        $��txtPasswordbox��5n   �   $��18��6    G� PasswordChar$ *��6"   {�.    AddItemTextForegroundColor#���  6E   
������ 
������ $��Center=�6�   $��Center=�6�   $
��Left=�6  $��3,3,3,0q�6    	 ProximaRegular#��� 	 5o      $��85��6E   $��5 2 0 0q�6R   )   #PasswordBoss.Helpers.PasswordHelper   Attach . $��16(�6�    
 roundedcornerPasswordBox#��� 
 5q         Password 5p      ��   �� RelativeSource  �� *FindAncestor+! �� AncestorType:   4PasswordBoss.Views.UserControls.PasswordTextBoxSmall"!  " �� Path$" Password�# �� Mode$# TwoWay=�5q      �� 5t   
   -   5        $��ImgShowHide��5t   ^   $y�Hidden=�6   $��12��6$   $��0 0 7 0q�60   $��Right=�6B   $
��Fill=�6q   $��Center=�6�   $��18��6�     imgLoginShowEye#���  5u      6<   � 5x   
   -   5        $ �Name$$ progressBar��5x   Y   $	��4 7q�6   $��1��6#   $��7��60   $��Stretch=�6;   #��[ 6n   ��6�   �� *
Foreground+% �� ElementName$% Parent��5z      5{      