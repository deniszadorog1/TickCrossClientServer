   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:TickCrossClient.Pages.GameReqsTickCrossClient.Pages.GameReqs  5      1    +TickCrossClient.Pages.GameReqs.GameRequests   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:TickCrossClient.Pages.GameReqs     5        N� Title$  GameRequests��5	      .��5
      �  � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5       K� ZIndex$ 0��6   �� 5   
   $��1��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5      $ 0��6   �� 5   
   -   5         ��Name$ SentRequestsBorder��5      $��0��5      $	��200��6!   $	��340��6-   
������� �� CornerRadius$ 6��5      $��Right=�6"   $��0 0 20 0q�6>    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5      	   ShadowDepth$	 2Q�6-   
   Color$

 Gray��6=   ._�5      ��  �� Orientation$ Vertical=�6   .+�5      �� -   5        $��SentTextBlock��5      $��Sent Requests��62   $��18(�5       $
��Bold"�6&   $��Center=�68   $��15q�5!      �� 5#      -   5         ��Name$ ToRemoveReqsListBox��5#      $	��200��67   $��5q�6D   �� 5%      -   5         ��Name$ RemoveRequestBut��5%      $��
10 0 10 0 q�64    #  MaterialDesignRaisedLightButton#��[  5&      $��Remove��5'      
������ 
�������
���   �5+      � 5-       5.      �� 5/      �� 50      .+�53      �� -   5        $ BackBut��53   -   $��0��6   $��	10 5 2 0 q�6?   #��[  54      $
��Back��55      
������ 
�������
���   �59      �� 5;      -   5        $ UpdatePageBut��5;   -   $��1��6   $��	2 5 10 0 q�6E   #��[  5<      $��Update��5=      
������ 
�������
���   �5A      5B      5E      5F      �� 5H   
   -   5        $ ConfirmRequestsBorder��5H      $��1��5I      $	��200��6!   $	��340��6-   
�������$ 6��5J      $
��Left=�6"   $��20 0 2 0q�6=    5K        5L      $ 15Q�6#   $	 -90Q�63   $ Quality=�5M      $	 2Q�6-   $

 Gray��6=   ._�5P      �� .+�5Q      �� -   5        $��GotTextBlock��5Q      $��Got Requests��61   $��18(�5R      $
��Bold"�6&   $��Center=�68   $��15q�5S      �� 5U      -	   5        $ SetReqsListBox��5U      $	��200��62   $��5q�6?   �� 5W      -
   5        $ AcceptGameBut��5W      $��
10 0 10 0 q�61   #��[  5X      $��Accept��5Y      
������ 
�������
���   �5]      �� 5_      -   5        $ RejectGameBut��5_      $��
10 5 10 0 q�61   #��[  5`      $��Reject��5a      
������ 
�������
���   �5e      5f      5g      5h      5i      