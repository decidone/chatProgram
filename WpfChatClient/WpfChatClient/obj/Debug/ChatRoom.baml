   M S B A M L   `   `   `  ����H  DWpfChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfChatClientWpfChatClient  5          WpfChatClient.ChatRoom   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:WpfChatClient     5        N� Title$  ChatRoom��5	      .��5      � .+�5   
   �� -   5         ��Name$ friend_list��5   *   $��10,13,411,112q�6    �� View 5      �� 5      .P�5      �� -   5        
sel_id��$
��user��5   5    �� Width$	 100��6C    �� DisplayMemberBinding 6O   �� *	chat_id+�� 5      -   5        sel_message��$��message��5   :   $	 150��6K    6W   �� *chat_message+�� 5      -   5        sel_time��$
��time��5   7   $	 120��6E    6Q   �� *	chat_time+5      5      �� 5   
   $��1��6   $��499,29,26,128q�5      ��5      �� 5      .R�5      �� $	��300��6!   .+�5      ��  �� Orientation$ 
Horizontal=�6%   .+�5      �� $	��asd��6(   5       �� 5!      $	��asd��6$   $��20,0,0,0q�5"   $   $
��Wrap=�5#   $   5$      5%      5'      �� 5(   
   -   5         ��Name$ invite��5(      $��친구초대��6!   $
��Left=�60   $��
394,29,0,0q�6K   $	��Top=�6_   $��75��6w   6�   �� 5)   
   -   5        $��메뉴로 돌아가기��5)      $
��Left=�6$   $��
394,95,0,0q�6?   $	��Top=�6S   $��75��6k   6v   �� 5*   
   -   5        $��채팅방 나가기��5*      $
��Left=�6#   $��394,288,0,0q�6>   $	��Top=�6S   $��75��6k   6v   �� 5+   
   -   5         ��Name$ message��5+      $
��Left=�6#   $��23��6>   $��
10,363,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $��TextBox��6r   $	��Top=�6�   $	��225��6�   �� 5,   
   -	   5        $��전송��5,      $
��Left=�6   $��253,363,0,0q�69   $	��Top=�6N   $��75��6f   $��23��6q   6}   5-      5.      