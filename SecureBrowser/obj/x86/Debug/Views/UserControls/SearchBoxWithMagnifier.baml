   M S B A M L   `   `   `  ����H  DSecureBrowser, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5      <    6PasswordBoss.Views.UserControls.SearchBoxWithMagnifier   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6helpers"clr-namespace:PasswordBoss.Helpers     5      .��5	      �   � UseLayoutRounding  .  � RowDefinitions 5
   
   �� 5      $��35 �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ txtSearchBoxWatermarked��5      $��0��6      TextBoxRoundedBorders#���   6@   $��0��6p   $��	32,1,25,0q�6�   $��30��5        SearchTheWeb#���  6    �� TextAlignment$
 Left=�6D     	GrayColor#���  6Y   $
��Left=�6�   $��Stretch=�6�   $��0,0,10,0q�6�   �� 5   
   -   5        $ txtSearchBox��5      $��0��6   $y�	Collapsed=�65   #���   6L   $��0��6|   $��	32,1,25,0q�6�   $��30��5      $
 Left=�6   $
��Left=�6�   $��Stretch=�6�   $��0,0,10,0q�6  ��6R   �� *SecureSearchQuery+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6  �� 5   
   $��0��6   $��10,0,0,0q�6     imgLockGray#���  5      $��0��6?   $��14��6O   $��14��6[   $
��Left=�5       �� BitmapScalingMode$ HighQuality=�63   �� 5   
   $��0��6   $��0,0,0,0q�6     ButtonRoundedBorders#���  6/   $��30��5      $��30��6&   $��0q�61   $��Right=�5      $��Center=�65     PasswordBossGreenColor#���  6P    J� InputBindings 5      d� 5      	 d� MouseAction$	 	LeftClicke�6   
 �� Command
 67   �� *SecureSearchCommand+5      5      5       