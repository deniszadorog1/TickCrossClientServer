   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:TickCrossClient.Pages.GameReqsTickCrossClient.Pages.GameReqs  5      3    -TickCrossClient.Pages.GameReqs.GotGameRequest   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:TickCrossClient.Pages.GameReqs     5        N� Title$  GotGameRequest��5	      .��5      � .+�5   
   �� $	��400��6   $	��200��6   
������� �� CornerRadius$ 6��6=    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5         ShadowDepth$ 2Q�6,      Color$
 Gray��6<   ._�5      �  � RowDefinitions 5      �� 5      �� 5      �� 5      .+�5      �� $��0��6   $��You got game request!��6)   $��Center=�6F   $��Center=�5      $��20(�69   $
��Bold"�6G   �� 5      $��1��6   	 �� Orientation$	 
Horizontal=�6*   .+�5      �� $��You will play with - ��6    $��18(�6=   $��	10 10 0 0q�6K   �� 5      -   5        $��
EnemyLogin��5       $��Enemy Login��64   $��0 10 0 0q�6G   $��18(�6Y   5      � 5!      $��2��6   
 � ColumnDefinitions
 5"      �� 5#      �� 5$      .+�5'      �� -   5        $��	AcceptReq��5'   -   $��0��6      MaterialDesignRaisedButton#��[  5(      $	��150��5)      $��35��6%   
��� � �
������� � ToolTip$ 
Play game!��5*      $��Right=�6.   $��10q�6J   $��Acceot��6V   5+      �� 5-      -   5        $��
DeclineReq��5-   -   $��1��6   #��[  5.      $	��150��5/      $��35��6%   
���  ��
�������
�������$ 	Not play(��50      $
��Left=�6-   $��10q�6H   $��Decline��6T   51      53      55      5:      5;      5<      