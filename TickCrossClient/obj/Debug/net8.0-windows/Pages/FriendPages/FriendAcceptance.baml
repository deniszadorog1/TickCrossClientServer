   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:TickCrossClient.Pages.FriendPages!TickCrossClient.Pages.FriendPages  5      8    2TickCrossClient.Pages.FriendPages.FriendAcceptance   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:TickCrossClient.Pages.FriendPages     5        N� Title$  FriendAcceptance��5	      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5       K� ZIndex$ 0��6   �� 5   
   $��1��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5      $ 0��6   �� 5   
   -   5        $��
LeftBorder��5   !   $��0��6   $��Right=�63   $��0 0 20 0q�6O   $	��300��5      $	��400��6   
������� �� CornerRadius$ 6��6=    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5         ShadowDepth$ 2Q�6-   	   Color$
	 Gray��6=   ._�5      �� .+�5       �� -   5        $��SentFriendOfferBlock��5       $��
Sent offer��69   $��Center=�5!      $��10q�5"      $��24(�6!   $
��Bold"�6/   �� 5$      -   5        $��SentOffersList��5$      $	��250��62   $��10q�5%      $��Visible=�6   �� 5'      -   5        
 ��Name$
 	RemoveBut��5'      $��
10 0 10 0 q�6-    #  MaterialDesignRaisedLightButton#��[  5(      $��Remove offer��5)      
������ 
�������
���   �5-      � 5/       50      �� 51      �� 52      .+�55      �� -   5        $
 UpdatePageBut��55   -   $��0��6   $��	10 5 5 0 q�6E   #��[  56      $��Update page��57      
������ 
�������
���   �5;      �� 5=      -   5        $
 BackBut��5=   -   $��1��6   $��	5 5 10 0 q�6?   #��[  5>      $
��Back��5?      
������ 
�������
���   �5C      5D      5F      5G      �� 5J   
   -   5        $��RightBorder��5J   !   $��1��6   $
��Left=�64   $��20 0 0 0q�6O   $	��300��5K      $	��400��6   
�������$ 6��6=    5L        5M      $ 15Q�6#   $	 -90Q�63   $ Quality=�5N      $ 2Q�6-   $
	 Gray��6=   ._�5Q      �� .+�5R      �� -   5        $��RecivedFriendOfferBlock��5R      $��Recived offers��6<   $��Center=�5S      $��10q�5T      $��24(�6!   $
��Bold"�6/   �� 5V      -	   5        $��RecivedOffersList��5V      $	��250��65   $��10q�5W      $��Visible=�6   �� 5Y      -
   5        $
 	AcceptBut��5Y      $��
10 0 10 0 q�6-   #��[  5Z      $��Accept��5[      
������ 
�������
���   �5_      �� 5a      -   5        $
 
DeclineBut��5a      $��
10 5 10 0 q�6.   #��[  5b      $��Decline��5c      
������ 
�������
���   �5g      5j      5k      5n      5o      