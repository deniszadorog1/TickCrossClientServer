   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:TickCrossClient.Pages.FriendPages!TickCrossClient.Pages.FriendPages  5      3    -TickCrossClient.Pages.FriendPages.FriendsPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:TickCrossClient.Pages.FriendPages     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� Title$  FriendsPage��5	      $��Center=�5      $��Center=�6"   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5       K� ZIndex$ 0��6   �� 5   
   $��1��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5      $ 0��6   �� 5   
   -   5        $��
LeftBorder��5   !   $��0��6   $��Right=�63   $��0 0 20 0q�6O   $	��300��5      $	��400��6   
������� �� CornerRadius$ 6��69    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5         ShadowDepth$ 2Q�64   	   Color$
	 Gray��6D   ._�5!      �� .+�5"      �� -   5        $��RemoveFriendsBlock��5"      $��Friends to Remove��67   $��Center=�5#      $��10q�5$      $��24(�6(   $
��Bold"�66   �� 5&      -   5        $��FriendsToRemoveList��5&      $	��250��67   $��10q�5'      $��Visible=�6&   �� 5*      -   5        
 ��Name$
 	RemoveBut��5*      $��
10 0 10 0 q�6-    #  MaterialDesignRaisedLightButton#��[  5+      $��Remove friend��5,      
������ 
�������
���   �50      �� 53      -   5        $
 BackBut��53      $��
10 5 10 0 q�6+   #��[  54      $
��Back��55      
������ 
�������
���   �59      5;      5<      �� 5>   
   -   5        $��RightBorder��5>   !   $��1��6   $
��Left=�64   $��20 0 0 0q�6O   $	��300��5?      $	��400��6   
�������$ 6��69    5@        5A      $ 15Q�6#   $	 -90Q�63   $ Quality=�5B      $ 2Q�64   $
	 Gray��6D   ._�5E      �� .+�5F      �� -   5        $��AddFriendsBlock��5F      $��Friends to add��64   $��Center=�5G      $��10q�69   $��24(�6E   $
��Bold"�6S   �� 5I      $��	10 0 10 0q�6   $��15(�6/   
�������$��Login��6O   �� 5J      -   5        $��FriendsToAddBox��5J      $��Center=�62   $��50��5K      $��10q�6&    �� 	MaxLength$ 24��62   $��16(�6A   �� 5M      $��10 0 10 10 q�6   $��50��61   $��20(�6=   $��Entter new friend login��6K   $��Center=�5N      $
��Bold"�69   
�������6K   �� 5Q      -	   5        $
 AddBut��5Q      $��10 10 10 0 q�6*   #��[  5R      $��Send friend offer��5S      
������ 
�������
���   �5W      �� 5Y      -
   5        $
 UpdateDbBUt��5Y      $��10 10 10 0 q�6/   #��[  5Z      $��Update page��5[      
������ 
�������
���   �5_      5a      5b      5e      5f      